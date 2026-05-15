using YarimKalanlar.Data;
using YarimKalanlar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YarimKalanlar.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    // Ana sayfa - pagination backendde
    public async Task<IActionResult> Index(int sayfa = 1, string? kategori = null)
    {
        const int sayfaBasiHaber = 6;

        var query = _db.Haberler.AsQueryable();

        if (!string.IsNullOrEmpty(kategori))
        {
            query = query.Where(h => h.Kategori == kategori);
        }

        var toplamHaber = await query.CountAsync();
        var toplamSayfa = (int)Math.Ceiling(toplamHaber / (double)sayfaBasiHaber);

        if (sayfa < 1) sayfa = 1;
        if (toplamSayfa > 0 && sayfa > toplamSayfa) sayfa = toplamSayfa;

        var haberler = await query
            .OrderByDescending(h => h.YayinTarihi)
            .Skip((sayfa - 1) * sayfaBasiHaber)
            .Take(sayfaBasiHaber)
            .ToListAsync();

        var vm = new HaberListeViewModel
        {
            Haberler = haberler,
            MevcutSayfa = sayfa,
            ToplamSayfa = toplamSayfa,
            SayfaBasiHaber = sayfaBasiHaber,
            ToplamHaber = toplamHaber,
            Kategori = kategori
        };

        return View(vm);
    }

    // Misyon sayfası
    public IActionResult Misyon()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
