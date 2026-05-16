using YarimKalanlar.Data;
using YarimKalanlar.Filters;
using YarimKalanlar.Models;
using YarimKalanlar.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace YarimKalanlar.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<AdminController> _logger;
    private readonly IGorselSaklayicisi _gorselSaklayicisi;

    private const string SessionGiris = "AdminGirisYapti";
    private const string SessionKullanici = "AdminKullaniciAdi";
    private const string SessionAdSoyad = "AdminAdSoyad";

    // Dosya yükleme - izinli MIME tipleri ve magic bytes (uzantı sahteciliği koruması)
    private static readonly Dictionary<string, byte[][]> IzinliDosyaImzalari = new()
    {
        [".jpg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
        [".jpeg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
        [".png"] = new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
        [".gif"] = new[]
        {
            new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, // GIF87a
            new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }  // GIF89a
        },
        [".webp"] = new[] { new byte[] { 0x52, 0x49, 0x46, 0x46 } } // RIFF (WebP container)
    };

    public AdminController(AppDbContext db, IConfiguration config, IWebHostEnvironment env,
        ILogger<AdminController> logger, IGorselSaklayicisi gorselSaklayicisi)
    {
        _db = db;
        _config = config;
        _env = env;
        _logger = logger;
        _gorselSaklayicisi = gorselSaklayicisi;
    }

    private class EditorBilgi
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string AdSoyad { get; set; } = "";
    }

    private List<EditorBilgi> EditorleriYukle()
    {
        var liste = new List<EditorBilgi>();
        _config.GetSection("Editorler").Bind(liste);
        return liste;
    }

    private string MevcutAdSoyad() =>
        HttpContext.Session.GetString(SessionAdSoyad) ?? "Editör";

    private string MevcutKullaniciAdi() =>
        HttpContext.Session.GetString(SessionKullanici) ?? "anon";

    // ============= DOSYA YÜKLEME (Güvenli) =============

    private async Task<string?> GorselYukleAsync(IFormFile? dosya)
    {
        if (dosya == null || dosya.Length == 0) return null;

        // 1. Uzantı kontrolü
        var uzanti = Path.GetExtension(dosya.FileName).ToLowerInvariant();
        if (!IzinliDosyaImzalari.ContainsKey(uzanti))
            throw new InvalidOperationException("Sadece JPG, PNG, GIF veya WebP formatları yüklenebilir.");

        // 2. Boyut kontrolü (5MB)
        if (dosya.Length > 5 * 1024 * 1024)
            throw new InvalidOperationException("Dosya boyutu en fazla 5MB olabilir.");

        if (dosya.Length < 8)
            throw new InvalidOperationException("Dosya bozuk veya geçersiz.");

        // 3. Content-Type kontrolü
        var contentType = dosya.ContentType?.ToLowerInvariant() ?? "";
        if (!contentType.StartsWith("image/"))
            throw new InvalidOperationException("Yalnızca görsel dosyaları yüklenebilir.");

        // 4. Magic byte (dosyanın gerçek içeriği) kontrolü - en kritik
        // Stream'i memory'e alıp hem doğrulama hem upload için kullanacağız
        byte[] dosyaBytes;
        await using (var bakStream = dosya.OpenReadStream())
        {
            using var memStream = new MemoryStream();
            await bakStream.CopyToAsync(memStream);
            dosyaBytes = memStream.ToArray();
        }

        var imzalar = IzinliDosyaImzalari[uzanti];
        bool gecerliImza = imzalar.Any(imza =>
            dosyaBytes.Length >= imza.Length && dosyaBytes.Take(imza.Length).SequenceEqual(imza));

        if (!gecerliImza)
            throw new InvalidOperationException("Dosyanın içeriği uzantısıyla eşleşmiyor.");

        // 5. Yükleme: Cloudinary varsa orada, yoksa yerel diske (geliştirme için)
        if (_gorselSaklayicisi.Yapilandirildi)
        {
            using var uploadStream = new MemoryStream(dosyaBytes);
            var cloudUrl = await _gorselSaklayicisi.YukleAsync(dosya, uploadStream);
            _logger.LogInformation("Görsel Cloudinary'e yüklendi: {Url}", cloudUrl);
            return cloudUrl;
        }

        // Yerel disk - sadece geliştirme ortamı için
        var uploadsKlasoru = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsKlasoru))
            Directory.CreateDirectory(uploadsKlasoru);

        var yeniAd = $"{Guid.NewGuid():N}{uzanti}";
        var tamYol = Path.Combine(uploadsKlasoru, yeniAd);

        var tamUploadsYolu = Path.GetFullPath(uploadsKlasoru);
        var tamHedefYolu = Path.GetFullPath(tamYol);
        if (!tamHedefYolu.StartsWith(tamUploadsYolu + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new InvalidOperationException("Geçersiz dosya yolu.");

        await File.WriteAllBytesAsync(tamYol, dosyaBytes);
        return $"/uploads/{yeniAd}";
    }

    private async Task EskiGorseliSilAsync(string? gorselUrl)
    {
        if (string.IsNullOrEmpty(gorselUrl)) return;

        // Cloudinary URL'i ise oradan sil
        if (gorselUrl.Contains("res.cloudinary.com", StringComparison.OrdinalIgnoreCase))
        {
            await _gorselSaklayicisi.SilAsync(gorselUrl);
            return;
        }

        // Yerel /uploads altındaki dosyalar
        if (!gorselUrl.StartsWith("/uploads/", StringComparison.Ordinal)) return;

        var dosyaAdi = Path.GetFileName(gorselUrl);
        if (string.IsNullOrEmpty(dosyaAdi)) return;

        var uploadsKlasoru = Path.Combine(_env.WebRootPath, "uploads");
        var fizikselYol = Path.Combine(uploadsKlasoru, dosyaAdi);

        var tamUploadsYolu = Path.GetFullPath(uploadsKlasoru);
        var tamFizikselYol = Path.GetFullPath(fizikselYol);
        if (!tamFizikselYol.StartsWith(tamUploadsYolu + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            return;

        if (System.IO.File.Exists(tamFizikselYol))
        {
            try { System.IO.File.Delete(tamFizikselYol); }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Görsel silinemedi: {Yol}", tamFizikselYol);
            }
        }
    }

    // ============= LOGIN / LOGOUT =============

    [HttpGet]
    public IActionResult Login()
    {
        var girisYapildi = HttpContext.Session.GetString(SessionGiris) == "true";
        if (girisYapildi) return RedirectToAction(nameof(Panel));
        return View(new LoginViewModel());
    }

    [HttpPost]
    [EnableRateLimiting("LoginPolicy")] // dakikada en fazla 5 deneme
    public IActionResult Login(LoginViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
        {
            model.Hata = "Kullanıcı adı ve şifre boş bırakılamaz.";
            return View(model);
        }

        var editorler = EditorleriYukle();
        var editor = editorler.FirstOrDefault(e =>
            e.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase)
            && SabitZamanKarsilastir(e.Password, model.Password));

        if (editor != null)
        {
            // Session fixation koruması
            HttpContext.Session.Clear();

            HttpContext.Session.SetString(SessionGiris, "true");
            HttpContext.Session.SetString(SessionKullanici, editor.Username);
            HttpContext.Session.SetString(SessionAdSoyad, editor.AdSoyad);

            _logger.LogInformation("Giriş başarılı: {Kullanici}", editor.Username);

            // Şifreyi hafızadan temizle
            model.Password = string.Empty;
            return RedirectToAction(nameof(Panel));
        }

        _logger.LogWarning("Başarısız giriş denemesi: {Kullanici} (IP: {IP})",
            model.Username, HttpContext.Connection.RemoteIpAddress);

        // Aynı mesaj - kullanıcı/şifre hangisinin yanlış olduğunu sızdırmaz
        model.Hata = "Kullanıcı adı veya şifre hatalı.";
        model.Password = string.Empty;
        return View(model);
    }

    /// <summary>
    /// Timing attack'i önlemek için sabit zamanlı string karşılaştırma.
    /// İki string'in uzunluğu farklı olsa bile aynı sürede sonuç döner.
    /// </summary>
    private static bool SabitZamanKarsilastir(string a, string b)
    {
        if (a == null || b == null) return false;
        if (a.Length != b.Length) return false;

        var diff = 0;
        for (int i = 0; i < a.Length; i++)
            diff |= a[i] ^ b[i];
        return diff == 0;
    }

    [HttpPost]
    public IActionResult Logout()
    {
        var kullanici = MevcutKullaniciAdi();
        HttpContext.Session.Clear();
        _logger.LogInformation("Çıkış: {Kullanici}", kullanici);
        return RedirectToAction(nameof(Login));
    }

    // ============= ADMIN PANEL (yetki gerekli) =============

    [YoneticiGerekli]
    public async Task<IActionResult> Panel()
    {
        var haberler = await _db.Haberler
            .OrderByDescending(h => h.YayinTarihi)
            .ToListAsync();

        ViewBag.AdSoyad = MevcutAdSoyad();
        return View(haberler);
    }

    [HttpGet]
    [YoneticiGerekli]
    public IActionResult Yeni()
    {
        var model = new Haber { Yazar = MevcutAdSoyad() };
        return View(model);
    }

    [HttpPost]
    [YoneticiGerekli]
    [EnableRateLimiting("UploadPolicy")]
    public async Task<IActionResult> Yeni(Haber model, IFormFile? gorselDosya)
    {
        ModelState.Remove(nameof(Haber.GorselUrl));
        if (!ModelState.IsValid) return View(model);

        try
        {
            var yuklenenYol = await GorselYukleAsync(gorselDosya);
            if (yuklenenYol != null)
                model.GorselUrl = yuklenenYol;
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("gorselDosya", ex.Message);
            return View(model);
        }

        // Sanitization - whitespace temizliği
        model.Baslik = (model.Baslik ?? "").Trim();
        model.Spot = (model.Spot ?? "").Trim();
        model.Icerik = (model.Icerik ?? "").Trim();
        model.Yazar = string.IsNullOrWhiteSpace(model.Yazar) ? MevcutAdSoyad() : model.Yazar.Trim();
        model.Kategori = (model.Kategori ?? "Gündem").Trim();

        model.YayinTarihi = DateTime.UtcNow;
        model.Goruntulenme = 0;

        _db.Haberler.Add(model);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Yeni haber eklendi: '{Baslik}' - {Kullanici}", model.Baslik, MevcutKullaniciAdi());
        return RedirectToAction(nameof(Panel));
    }

    [HttpGet]
    [YoneticiGerekli]
    public async Task<IActionResult> Duzenle(int id)
    {
        var haber = await _db.Haberler.FindAsync(id);
        if (haber == null) return NotFound();
        return View(haber);
    }

    [HttpPost]
    [YoneticiGerekli]
    [EnableRateLimiting("UploadPolicy")]
    public async Task<IActionResult> Duzenle(Haber model, IFormFile? gorselDosya, bool gorselSil = false)
    {
        ModelState.Remove(nameof(Haber.GorselUrl));
        if (!ModelState.IsValid) return View(model);

        var haber = await _db.Haberler.FindAsync(model.Id);
        if (haber == null) return NotFound();

        haber.Baslik = (model.Baslik ?? "").Trim();
        haber.Spot = (model.Spot ?? "").Trim();
        haber.Icerik = (model.Icerik ?? "").Trim();
        haber.Yazar = string.IsNullOrWhiteSpace(model.Yazar) ? MevcutAdSoyad() : model.Yazar.Trim();
        haber.Kategori = (model.Kategori ?? "Gündem").Trim();

        try
        {
            var yeniYol = await GorselYukleAsync(gorselDosya);
            if (yeniYol != null)
            {
                await EskiGorseliSilAsync(haber.GorselUrl);
                haber.GorselUrl = yeniYol;
            }
            else if (gorselSil)
            {
                await EskiGorseliSilAsync(haber.GorselUrl);
                haber.GorselUrl = null;
            }
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("gorselDosya", ex.Message);
            return View(model);
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Haber düzenlendi: id={Id} - {Kullanici}", haber.Id, MevcutKullaniciAdi());
        return RedirectToAction(nameof(Panel));
    }

    [HttpPost]
    [YoneticiGerekli]
    public async Task<IActionResult> Sil(int id)
    {
        var haber = await _db.Haberler.FindAsync(id);
        if (haber == null) return NotFound();

        await EskiGorseliSilAsync(haber.GorselUrl);

        _db.Haberler.Remove(haber);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Haber silindi: id={Id} - {Kullanici}", id, MevcutKullaniciAdi());
        return RedirectToAction(nameof(Panel));
    }
}
