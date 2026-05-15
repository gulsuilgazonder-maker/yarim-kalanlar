using System.ComponentModel.DataAnnotations;

namespace YarimKalanlar.Models;

public class Haber
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Başlık zorunludur")]
    [StringLength(300)]
    public string Baslik { get; set; } = string.Empty;

    [Required(ErrorMessage = "Spot zorunludur")]
    [StringLength(500)]
    public string Spot { get; set; } = string.Empty;

    [Required(ErrorMessage = "İçerik zorunludur")]
    public string Icerik { get; set; } = string.Empty;

    [StringLength(100)]
    public string Yazar { get; set; } = "Editör";

    [StringLength(100)]
    public string Kategori { get; set; } = "Gündem";

    public string? GorselUrl { get; set; }

    public DateTime YayinTarihi { get; set; } = DateTime.Now;

    public int Goruntulenme { get; set; } = 0;
}
