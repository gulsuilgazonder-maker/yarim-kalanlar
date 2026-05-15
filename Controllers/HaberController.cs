using YarimKalanlar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YarimKalanlar.Controllers;

public class HaberController : Controller
{
    private readonly AppDbContext _db;

    public HaberController(AppDbContext db)
    {
        _db = db;
    }

    // Haber detay sayfası - ayrı sayfa olarak açılır
    public async Task<IActionResult> Detay(int id)
    {
        var haber = await _db.Haberler.FirstOrDefaultAsync(h => h.Id == id);
        if (haber == null) return NotFound();

        // Görüntülenme sayısını artır
        haber.Goruntulenme++;
        await _db.SaveChangesAsync();

        // İlgili haberler (aynı kategoriden 3 tane)
        var ilgili = await _db.Haberler
            .Where(h => h.Kategori == haber.Kategori && h.Id != haber.Id)
            .OrderByDescending(h => h.YayinTarihi)
            .Take(3)
            .ToListAsync();

        ViewBag.IlgiliHaberler = ilgili;
        return View(haber);
    }
}
