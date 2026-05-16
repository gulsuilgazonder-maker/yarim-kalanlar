using System.Globalization;

namespace YarimKalanlar.Helpers;

public static class TarihHelpers
{
    private static readonly CultureInfo TrCulture = new("tr-TR");

    // Türkiye saat dilimi - Render Linux sunucularda "Europe/Istanbul",
    // Windows'ta "Turkey Standard Time" çalışır. İkisini de dene.
    private static readonly TimeZoneInfo TurkiyeSaati = GetTurkiyeSaati();

    private static TimeZoneInfo GetTurkiyeSaati()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul"); }
        catch
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"); }
            catch { return TimeZoneInfo.Utc; } // son çare
        }
    }

    /// <summary>
    /// UTC bir DateTime'ı Türkiye saatine çevirir.
    /// </summary>
    public static DateTime TurkiyeSaatine(this DateTime utcTarih)
    {
        // Eğer Kind belirsizse UTC kabul et (veritabanından öyle gelir)
        if (utcTarih.Kind == DateTimeKind.Unspecified)
            utcTarih = DateTime.SpecifyKind(utcTarih, DateTimeKind.Utc);

        if (utcTarih.Kind == DateTimeKind.Local)
            utcTarih = utcTarih.ToUniversalTime();

        return TimeZoneInfo.ConvertTimeFromUtc(utcTarih, TurkiyeSaati);
    }

    /// <summary>Uzun format: "16 Mayıs 2026, 14:35"</summary>
    public static string TrUzun(this DateTime utcTarih) =>
        utcTarih.TurkiyeSaatine().ToString("d MMMM yyyy, HH:mm", TrCulture);

    /// <summary>Kısa format: "16 May, 14:35"</summary>
    public static string TrKisa(this DateTime utcTarih) =>
        utcTarih.TurkiyeSaatine().ToString("d MMM, HH:mm", TrCulture);

    /// <summary>Sadece tarih: "16 May 2026"</summary>
    public static string TrSadeceTarih(this DateTime utcTarih) =>
        utcTarih.TurkiyeSaatine().ToString("d MMM yyyy", TrCulture);
}
