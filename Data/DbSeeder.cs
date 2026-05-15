using YarimKalanlar.Models;

namespace YarimKalanlar.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Haberler.Any()) return;

        var haberler = new List<Haber>
        {
            new()
            {
                Baslik = "Kadın Cinayetlerini Durduracağız Platformu Mart Raporunu Açıkladı",
                Spot = "Platform, geçtiğimiz ay yaşanan vakaları kamuoyuyla paylaştı; veri tabanlı izleme çalışmaları sürüyor.",
                Icerik = "Kadın Cinayetlerini Durduracağız Platformu, aylık raporunu kamuoyu ile paylaştı. Raporda hayatını kaybeden kadınların isimleri, yaşları ve faillerle olan ilişkileri belgelendi. Platform sözcüsü açıklamasında, vakaların büyük çoğunluğunda kadınların önceden şiddet bildiriminde bulunduğuna dikkat çekti.\n\nRapor, koruma kararlarının uygulanmasındaki eksikliklere de değindi. Sivil toplum kuruluşları, 6284 sayılı kanunun etkin biçimde uygulanması ve önleyici tedbirlerin güçlendirilmesi çağrısında bulundu.\n\nKamu kurumlarının yanı sıra belediyeler, barolar ve üniversiteler de ortak çalışma yürütmeye davet edildi. Veri şeffaflığı ve hesap verebilirlik, raporun öne çıkan başlıkları arasında yer aldı.",
                Yazar = "Haber Merkezi",
                Kategori = "Gündem",
                GorselUrl = "https://images.unsplash.com/photo-1591522810850-58128c5fb089?w=800",
                YayinTarihi = DateTime.Now.AddHours(-2),
                Goruntulenme = 1247
            },
            new()
            {
                Baslik = "İstanbul Sözleşmesi'nin Önemi Akademisyenler Tarafından Tartışıldı",
                Spot = "Üniversitelerden hukukçular, sözleşmenin kadına yönelik şiddetin önlenmesindeki rolünü değerlendirdi.",
                Icerik = "Ankara'da düzenlenen sempozyumda akademisyenler, İstanbul Sözleşmesi'nin uluslararası hukuktaki yerini ve Türkiye'deki uygulama sürecini ele aldı. Hukuk fakültelerinden bir araya gelen uzmanlar, sözleşmenin kadına yönelik şiddetle mücadelede sağladığı çerçevenin altını çizdi.\n\nPanel sonunda kabul edilen ortak metinde, kadın haklarının evrenselliği ve devletin pozitif yükümlülükleri hatırlatıldı. Katılımcılar, hukuki düzenlemelerin yanı sıra toplumsal farkındalık çalışmalarının da kritik önem taşıdığını vurguladı.\n\nSempozyumdan sonra yayımlanması beklenen kitapta, kadına yönelik şiddetin önlenmesine ilişkin politika önerilerine yer verilecek.",
                Yazar = "Mehmet Aydın",
                Kategori = "Hukuk",
                GorselUrl = "https://images.unsplash.com/photo-1589994965851-a8f479c573a9?w=800",
                YayinTarihi = DateTime.Now.AddHours(-5),
                Goruntulenme = 892
            },
            new()
            {
                Baslik = "Sığınma Evleri Kapasite Sorunuyla Karşı Karşıya",
                Spot = "Büyükşehirlerdeki sığınma evlerinin doluluk oranları, sivil toplum kuruluşlarının çağrılarını artırdı.",
                Icerik = "Türkiye genelinde faaliyet gösteren kadın sığınma evlerinin doluluk oranlarının ciddi seviyelere ulaştığı belirtildi. Aile ve Sosyal Hizmetler Bakanlığı verilerine göre özellikle büyükşehirlerde başvuru sayısı son bir yılda belirgin biçimde arttı.\n\nKonuyla ilgili açıklama yapan sivil toplum temsilcileri, sığınma evi kapasitesinin artırılması ve geçici barınma çözümleri için ek bütçe talep etti. Belediyelerin de sürece dahil olması gerektiği vurgulandı.\n\nUzmanlar, sığınma evlerinden çıkış sonrası sosyo-ekonomik destek programlarının da güçlendirilmesi gerektiğine dikkat çekiyor.",
                Yazar = "Ayşe Demirci",
                Kategori = "Sosyal",
                GorselUrl = "https://images.unsplash.com/photo-1518709268805-4e9042af2176?w=800",
                YayinTarihi = DateTime.Now.AddHours(-8),
                Goruntulenme = 654
            },
            new()
            {
                Baslik = "Koruma Kararlarının Etkin Uygulanması İçin Yeni Genelge",
                Spot = "Adalet Bakanlığı, 6284 sayılı kanun kapsamındaki kararların takibine ilişkin yeni bir genelge yayımladı.",
                Icerik = "Adalet Bakanlığı tarafından yayımlanan genelge ile koruma kararlarının uygulanmasındaki süreçler yeniden düzenlendi. Genelgede, kolluk kuvvetlerinin görev ve sorumluluklarına ilişkin ayrıntılı talimatlar yer aldı.\n\nDüzenlemeyle birlikte elektronik kelepçe uygulamasının yaygınlaştırılması ve risk değerlendirme protokollerinin güncellenmesi öngörülüyor. Baroların da bu süreçte aktif rol oynaması bekleniyor.\n\nHukuk çevreleri, genelgenin sahaya yansımasının yakından takip edileceğini belirtti.",
                Yazar = "Selin Karaca",
                Kategori = "Hukuk",
                GorselUrl = "https://images.unsplash.com/photo-1505664194779-8beaceb93744?w=800",
                YayinTarihi = DateTime.Now.AddDays(-1),
                Goruntulenme = 2103
            },
            new()
            {
                Baslik = "Üniversitelerde Toplumsal Cinsiyet Eşitliği Birimleri Yaygınlaşıyor",
                Spot = "YÖK koordinasyonunda yürütülen çalışmalar kapsamında birim sayısı 100'ü aştı.",
                Icerik = "Yükseköğretim Kurulu (YÖK) verilerine göre üniversitelerde kurulan toplumsal cinsiyet eşitliği birimlerinin sayısı geçtiğimiz yıl önemli bir artış gösterdi. Birimler, hem akademik hem de idari personelin yanı sıra öğrencilere yönelik farkındalık programları yürütüyor.\n\nProgramlar kapsamında düzenlenen seminerlerde uzmanlar, kampüs içi şiddet vakalarının önlenmesi ve raporlama mekanizmalarının güçlendirilmesi konularında bilgi paylaşımında bulundu.\n\nYÖK, önümüzdeki dönemde birimlerin sayısının daha da artmasını hedeflediğini açıkladı.",
                Yazar = "Burak Yılmaz",
                Kategori = "Eğitim",
                GorselUrl = "https://images.unsplash.com/photo-1523050854058-8df90110c9f1?w=800",
                YayinTarihi = DateTime.Now.AddDays(-1).AddHours(-3),
                Goruntulenme = 478
            },
            new()
            {
                Baslik = "Barolardan Ortak Açıklama: Adli Destek Hattı 7/24 Hizmette",
                Spot = "Türkiye Barolar Birliği, kadına yönelik şiddet vakalarında ücretsiz hukuki destek sağlandığını hatırlattı.",
                Icerik = "Türkiye Barolar Birliği (TBB) tarafından yapılan açıklamada, kadına yönelik şiddet ve aile içi şiddet vakalarında ücretsiz hukuki destek hattının 7 gün 24 saat hizmet verdiği hatırlatıldı.\n\nAçıklamada, başvuruların gizlilik içerisinde değerlendirildiği ve avukat görevlendirmesinin hızla yapıldığı belirtildi. Hat üzerinden ayrıca psikososyal yönlendirme de sağlanıyor.\n\nBarolar, vatandaşları acil durumlarda 155 Polis İmdat ve Alo 183 Sosyal Destek Hattı ile birlikte baronun adli yardım servisine de başvurmaya davet etti.",
                Yazar = "Haber Merkezi",
                Kategori = "Hukuk",
                GorselUrl = "https://images.unsplash.com/photo-1521791136064-7986c2920216?w=800",
                YayinTarihi = DateTime.Now.AddDays(-2),
                Goruntulenme = 1567
            },
            new()
            {
                Baslik = "Belediyelerden Şiddete Karşı Ortak Eylem Planı",
                Spot = "Büyükşehir belediyeleri, kadına yönelik şiddetle mücadele için ortak bir koordinasyon modeli oluşturdu.",
                Icerik = "İstanbul, Ankara ve İzmir Büyükşehir Belediyeleri başta olmak üzere çok sayıda yerel yönetim, kadına yönelik şiddetle mücadele kapsamında ortak bir eylem planı hazırladı. Planda; danışma merkezleri, meslek edindirme kursları ve psikolojik destek hizmetlerinin yaygınlaştırılması yer alıyor.\n\nBelediye başkanları yaptıkları açıklamalarda, merkezi yönetimle koordineli çalışmanın önemine değindi. Eylem planının ilk uygulama sonuçları yıl sonunda kamuoyu ile paylaşılacak.\n\nSivil toplum kuruluşları, planın izleme komitelerinde yer almak istediğini belirtti.",
                Yazar = "Mehmet Aydın",
                Kategori = "Sosyal",
                GorselUrl = "https://images.unsplash.com/photo-1517433367423-c7e5b0f35086?w=800",
                YayinTarihi = DateTime.Now.AddDays(-2).AddHours(-5),
                Goruntulenme = 723
            },
            new()
            {
                Baslik = "Psikososyal Destek Hizmetleri için Yeni Protokol İmzalandı",
                Spot = "Sağlık Bakanlığı ve Aile Bakanlığı arasındaki protokolle hizmetler genişletiliyor.",
                Icerik = "Sağlık Bakanlığı ile Aile ve Sosyal Hizmetler Bakanlığı arasında imzalanan protokol kapsamında, şiddet mağduru kadınlara yönelik psikososyal destek hizmetleri genişletildi. Protokol; hastanelerdeki sosyal hizmet birimleri ve ŞÖNİM'ler arasında koordinasyonu güçlendirmeyi amaçlıyor.\n\nBakanlık yetkilileri, hizmet alan sayısının önümüzdeki dönemde belirgin biçimde artmasını beklediklerini ifade etti. Sahada görev yapan personelin de eğitim programlarına alınması planlanıyor.\n\nUzmanlar, protokolün uygulama detaylarının izleneceğini ve raporlanacağını belirtti.",
                Yazar = "Selin Karaca",
                Kategori = "Sağlık",
                GorselUrl = "https://images.unsplash.com/photo-1576091160550-2173dba999ef?w=800",
                YayinTarihi = DateTime.Now.AddDays(-3),
                Goruntulenme = 956
            },
            new()
            {
                Baslik = "Eğitim Müfredatına Toplumsal Cinsiyet Eşitliği Modülleri",
                Spot = "Milli Eğitim Bakanlığı, ortaöğretim müfredatında yeni içeriklere yer verdiğini açıkladı.",
                Icerik = "Milli Eğitim Bakanlığı, ortaöğretim müfredatına toplumsal cinsiyet eşitliği başlığı altında yeni modüller eklendiğini açıkladı. Modüller; hak temelli yaklaşım, ayrımcılığın önlenmesi ve şiddetle mücadele konularını kapsıyor.\n\nÖğretmenlere yönelik hizmet içi eğitim programları yıl sonuna kadar tamamlanacak. Bakanlık, sivil toplum kuruluşlarıyla iş birliği yaparak içeriklerin geliştirilmesine devam edeceğini bildirdi.\n\nEğitimciler, programın uygulanmasında öğretmenlerin sahaya hazır olmasının önemine dikkat çekti.",
                Yazar = "Burak Yılmaz",
                Kategori = "Eğitim",
                GorselUrl = "https://images.unsplash.com/photo-1497486751825-1233686d5d80?w=800",
                YayinTarihi = DateTime.Now.AddDays(-3).AddHours(-4),
                Goruntulenme = 612
            },
            new()
            {
                Baslik = "Acil Durum Butonu Uygulaması Yaygınlaştırılıyor",
                Spot = "KADES uygulamasını indiren kullanıcı sayısı bir yılda iki katına çıktı.",
                Icerik = "İçişleri Bakanlığı tarafından geliştirilen Kadın Destek Uygulaması KADES'in indirilme sayısının son bir yılda ikiye katlandığı açıklandı. Uygulama; acil durumlarda tek dokunuşla 155 Polis İmdat hattına konum bilgisi gönderiyor.\n\nBakanlık yetkilileri, uygulama üzerinden yapılan ihbarların hızlı şekilde değerlendirildiğini belirtti. Yaygınlaştırma çalışmaları kapsamında belediyeler ve sivil toplum kuruluşları ile ortak tanıtım programları düzenleniyor.\n\nUygulama, hem Android hem de iOS işletim sistemlerinde ücretsiz olarak indirilebiliyor.",
                Yazar = "Ayşe Demirci",
                Kategori = "Gündem",
                GorselUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=800",
                YayinTarihi = DateTime.Now.AddDays(-4),
                Goruntulenme = 1834
            },
            new()
            {
                Baslik = "Kadın Hakları Alanında Çalışan Gazetecilere Yeni Eğitim Programı",
                Spot = "Basın meslek örgütleri, haberciliğin ilkeleri üzerine kapsamlı bir program başlattı.",
                Icerik = "Türkiye Gazeteciler Cemiyeti ve birçok meslek örgütü, kadın haklarına ilişkin haberciliği geliştirmek amacıyla yeni bir eğitim programı başlattı. Program kapsamında; haber dili, mağdur kimliğinin korunması ve etik ilkeler ele alınıyor.\n\nEğitimlerin önümüzdeki dönemde yerel medya çalışanlarına da ulaştırılması planlanıyor. Akademisyenler ve deneyimli gazetecilerden oluşan eğitmen kadrosu, oturumlarda saha deneyimlerini paylaşıyor.\n\nProgramın sonunda katılımcılara sertifika veriliyor; uygulamalı atölyeler de yıl boyunca sürecek.",
                Yazar = "Haber Merkezi",
                Kategori = "Medya",
                GorselUrl = "https://images.unsplash.com/photo-1504711434969-e33886168f5c?w=800",
                YayinTarihi = DateTime.Now.AddDays(-5),
                Goruntulenme = 389
            },
            new()
            {
                Baslik = "Veri Tabanlı İzleme: Şiddet Vakalarının Haritalandırılması",
                Spot = "Üniversiteler ve sivil toplum kuruluşlarının ortak çalışması veri görselleştirmeyle kamuoyuna sunuldu.",
                Icerik = "Birçok üniversite ve sivil toplum kuruluşunun ortak yürüttüğü araştırma projesi kapsamında, kadına yönelik şiddet vakalarının haritalandırıldığı bir veri platformu kamuoyu ile paylaşıldı. Platform; il bazında istatistikler, koruma kararı verileri ve adli süreçleri içeriyor.\n\nProje koordinatörleri, verilerin politika yapıcılar için kritik bir kaynak haline geldiğini vurguladı. Platform, üç ayda bir güncellenecek ve açık erişim ilkesiyle kullanıma sunulmaya devam edecek.\n\nAraştırmacılar, veri sağlayan kurumların sayısının artmasıyla birlikte analizlerin de derinleşeceğini belirtti.",
                Yazar = "Selin Karaca",
                Kategori = "Araştırma",
                GorselUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=800",
                YayinTarihi = DateTime.Now.AddDays(-6),
                Goruntulenme = 521
            }
        };

        db.Haberler.AddRange(haberler);
        db.SaveChanges();
    }
}
