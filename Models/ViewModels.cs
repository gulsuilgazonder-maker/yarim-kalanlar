namespace YarimKalanlar.Models;

public class HaberListeViewModel
{
    public List<Haber> Haberler { get; set; } = new();
    public int MevcutSayfa { get; set; }
    public int ToplamSayfa { get; set; }
    public int SayfaBasiHaber { get; set; } = 6;
    public int ToplamHaber { get; set; }
    public string? Kategori { get; set; }

    public bool OncekiVar => MevcutSayfa > 1;
    public bool SonrakiVar => MevcutSayfa < ToplamSayfa;
}

public class LoginViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Hata { get; set; }
}
