using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace YarimKalanlar.Services;

public interface IGorselSaklayicisi
{
    Task<string> YukleAsync(IFormFile dosya, Stream icerikStream);
    Task SilAsync(string gorselUrl);
    bool Yapilandirildi { get; }
}

/// <summary>
/// Cloudinary üzerinde görsel saklama servisi.
/// CLOUDINARY_URL environment variable'ı set edilmişse aktif olur.
/// </summary>
public class CloudinaryGorselSaklayicisi : IGorselSaklayicisi
{
    private readonly Cloudinary? _cloudinary;
    private readonly ILogger<CloudinaryGorselSaklayicisi> _logger;
    private const string KlasorAdi = "yarimkalanlar";

    public bool Yapilandirildi => _cloudinary != null;

    public CloudinaryGorselSaklayicisi(IConfiguration config, ILogger<CloudinaryGorselSaklayicisi> logger)
    {
        _logger = logger;

        // CLOUDINARY_URL formatı: cloudinary://api_key:api_secret@cloud_name
        var url = Environment.GetEnvironmentVariable("CLOUDINARY_URL")
                  ?? config["Cloudinary:Url"];

        if (string.IsNullOrEmpty(url))
        {
            _logger.LogWarning("CLOUDINARY_URL bulunamadı; Cloudinary devre dışı.");
            _cloudinary = null;
            return;
        }

        try
        {
            _cloudinary = new Cloudinary(url) { Api = { Secure = true } };
            _logger.LogInformation("Cloudinary aktif: {Cloud}", _cloudinary.Api.Account.Cloud);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cloudinary yapılandırılamadı.");
            _cloudinary = null;
        }
    }

    public async Task<string> YukleAsync(IFormFile dosya, Stream icerikStream)
    {
        if (_cloudinary == null)
            throw new InvalidOperationException("Cloudinary yapılandırılmamış.");

        var publicId = $"{KlasorAdi}/{Guid.NewGuid():N}";

        var yuklemeParam = new ImageUploadParams
        {
            File = new FileDescription(dosya.FileName, icerikStream),
            PublicId = publicId,
            UseFilename = false,
            UniqueFilename = false,
            Overwrite = false,
            // Otomatik optimizasyon: kalite ve format Cloudinary'e bırakılır
            Transformation = new Transformation()
                .Quality("auto")
                .FetchFormat("auto")
        };

        var sonuc = await _cloudinary.UploadAsync(yuklemeParam);

        if (sonuc.Error != null)
            throw new InvalidOperationException($"Cloudinary yükleme hatası: {sonuc.Error.Message}");

        // Secure URL döndür (https://)
        return sonuc.SecureUrl?.ToString() ?? sonuc.Url.ToString();
    }

    public async Task SilAsync(string gorselUrl)
    {
        if (_cloudinary == null) return;
        if (string.IsNullOrEmpty(gorselUrl)) return;

        // Cloudinary URL'inden publicId'yi çıkar
        // Örnek: https://res.cloudinary.com/myacc/image/upload/v1234/yarimkalanlar/abc123.jpg
        // publicId: yarimkalanlar/abc123 (uzantı ve versiyon olmadan)
        var publicId = PublicIdCikar(gorselUrl);
        if (publicId == null) return;

        try
        {
            var sonuc = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
            if (sonuc.Error != null)
                _logger.LogWarning("Cloudinary silme hatası: {Hata}", sonuc.Error.Message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cloudinary silme istisnası: {Url}", gorselUrl);
        }
    }

    private static string? PublicIdCikar(string url)
    {
        // URL formatı: https://res.cloudinary.com/<cloud>/image/upload/v<version>/<publicId>.<ext>
        var uploadIndex = url.IndexOf("/upload/", StringComparison.OrdinalIgnoreCase);
        if (uploadIndex < 0) return null;

        var kalan = url.Substring(uploadIndex + "/upload/".Length);

        // Versiyon kısmını (v1234567/) atla
        if (kalan.StartsWith("v"))
        {
            var slashIdx = kalan.IndexOf('/');
            if (slashIdx > 0 && kalan.Substring(1, slashIdx - 1).All(char.IsDigit))
                kalan = kalan.Substring(slashIdx + 1);
        }

        // Uzantıyı kaldır
        var noktaIdx = kalan.LastIndexOf('.');
        if (noktaIdx > 0)
            kalan = kalan.Substring(0, noktaIdx);

        return kalan;
    }
}
