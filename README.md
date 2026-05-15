# Sözcü Haber — Haber Sitesi

Kadına yönelik şiddetle mücadele ve farkındalık temalı, ciddi tonlu bir haber sitesi. ASP.NET Core 8 MVC + EF Core (SQLite) ile yazıldı.

## Özellikler

- **Ana sayfa**: Manşet + haber grid'i + kategori filtreleme
- **Backend pagination**: Skip/Take ile (sayfa başına 6 haber)
- **Haber detay**: Her habere ayrı sayfa, görüntülenme sayacı, ilgili haberler
- **Admin login**: Session tabanlı, sadece admin için
- **Admin paneli**: Haber ekle, düzenle, sil (CRUD)
- **Veritabanı**: SQLite — otomatik kurulur, örnek verilerle gelir
- **Tema**: Lacivert/beyaz, Playfair Display + Source Serif 4 (ciddi editöryel)

## Kurulum

Proje klasörünün içindeyken:

```bash
dotnet restore
dotnet run
```

Tarayıcıdan aç: `http://localhost:5000`

İlk çalıştırmada `habersitesi.db` (SQLite) otomatik oluşur ve 12 örnek haber yüklenir.

## Admin Girişi

- Sağ üstte **"Yönetici Girişi"** linki
- **Kullanıcı adı**: `admin`
- **Şifre**: `admin123`

> Bunlar `appsettings.json` içinde `AdminCredentials` bölümünden değiştirilebilir.

## Klasör Yapısı

```
YarimKalanlar/
├── Controllers/
│   ├── HomeController.cs      → Ana sayfa + pagination
│   ├── HaberController.cs     → Haber detay
│   └── AdminController.cs     → Login + CRUD
├── Models/
│   ├── Haber.cs               → Veri modeli
│   └── ViewModels.cs          → Pagination ve Login VM
├── Data/
│   ├── AppDbContext.cs        → EF Core context
│   └── DbSeeder.cs            → Örnek veri
├── Views/                     → Cshtml dosyaları
└── wwwroot/css/site.css       → Tüm stil
```

## Veritabanını Sıfırlama

`habersitesi.db` dosyasını sil ve `dotnet run` ile yeniden çalıştır. Tüm örnek haberler otomatik geri yüklenir.

## Notlar

- Tablo şeması değişirse `db.Database.EnsureCreated()` mevcut DB'yi otomatik güncellemez. Geliştirme aşamasında DB dosyasını silmek en pratiği. Üretimde EF Core migrations'a geçmek lazım.
- Admin login basit tutuldu (session + appsettings credentials). Üretimde ASP.NET Core Identity'ye geçmek daha güvenli olur.
