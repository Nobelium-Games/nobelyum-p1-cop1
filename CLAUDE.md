//# KRAL SİMÜLASYONU — Proje Bağlamı

> Bu dosya, bu Unity projesinde daha önceki bir Claude sohbetinde yapılan çalışmanın özetidir.
> Amaç: yeni bir sohbet/oturum bu dosyayı okuduğunda, sanki aynı sohbetten kaldığı yerden
> devam ediyormuş gibi bağlamı hızlıca kavraması.
>
> **Yeni oturuma not:** Bu dosya bir yol haritasıdır, ama gerçek kod her zaman "zemin gerçeği"dir.
> Özellikle "Şu Anki Durum" bölümündeki bazı Unity sahne bağlantıları (Inspector'da hangi objenin
> hangi alana sürüklendiği) bu özeti hazırlarken doğrudan görülmedi — işe başlamadan önce ilgili
> script dosyalarını oku/incele ve Unity'de Hierarchy'yi gözden geçir, bu özetle karşılaştır.
> **Mektuplar/görev sistemi tamamen silindi** (kullanıcı henüz nasıl bir tasarım istediğine karar
> veremedi) — eğer sahnede hâlâ buna ait kalıntı obje görürsen bu beklenmedik bir şey, kullanıcıya sor.

---

## 1) TAKIM VE ANLATIM TARZI (HER ZAMAN UYGULANMALI)

- 3 kişilik küçük bir ekibiz, proje yönetimi ve Unity/C# konusunda ileri seviye değiliz, sıfırdan öğreniyoruz.
- Unity'ye genel hakimiyet var, ama **C# bilgisi gelişmiş değil.**
- Açıklamalar HER ZAMAN basit, az teknik jargonlu, adım adım olmalı — sanki hiç bilmiyormuşuz gibi anlat.
- Yeni bir C# kavramı ilk kez geçtiğinde (fonksiyon/parametre, switch, enum, constructor, HashSet,
  foreach, ScriptableObject, Singleton, SetActive, CanvasGroup, lambda/`Action`, `Mathf.Clamp`,
  `RectTransformUtility` vs.) mutlaka kısa (1-3 cümle), günlük hayattan bir benzetmeyle açıklanmalı.
  Örnek benzetmeler daha önce işe yaradı: fonksiyon = matematikteki çok değişkenli fonksiyon f(x,y);
  GameManager.Instance.State.Erzak zinciri = apartman → daire → çekmece; CanvasGroup = bir objenin
  görünürlüğünü tek sayıyla (0-1) ayarlayan düğme; lambda (`() => ...`) = "şunu yap" diye isimsiz,
  paketlenmiş bir tarif, hemen değil doğru zamanda çalıştırılsın diye başka bir fonksiyona gönderiliyor;
  `Mathf.Clamp` = bir değeri belirlediğin alt-üst sınırın dışına çıkmasın diye kelepçeleyen fonksiyon.
- **Küçük adımlarla ilerle.** Her seferinde tek bir parça ver, kullanıcı deneyip "oldu" ya da hata
  paylaşana kadar bir sonrakine geçme. Büyük, çok-parçalı adımlar kullanıcıyı bunalttı ve bir kez
  (ekonomi sistemi eklerken) "korkup her şeyi geri almasına" yol açtı — bundan kaçın, mümkün olan
  en küçük, test edilebilir parçalara böl. Büyük bir özellik isteği geldiğinde (örn. "köyleri
  bireysel yapalım", "isyan bastırma emri ekleyelim") önce özelliği alt parçalara böl, gerekirse
  belirsiz tasarım noktalarını (debuff türü, bildirim şekli, slot sayısı nasıl belirlenir vs.) soru
  olarak kullanıcıya sor/onaylat, sonra tek tek uygula — bu yöntem "Şahsi Oda yeniden tasarımı",
  "köy hedefleme" ve "isyan mekaniği" işlerinde iyi çalıştı.
- Kullanıcı bazen bir özelliği kendi önerip sonra "zor olursa yapmayabiliriz, sadece fikir sundum"
  diyor — bu durumda dürüst bir değerlendirme yap (gerçekten zor mu, yoksa zaten elimizdeki bir
  teknikle mi çözülüyor), tercih ona kalsın. (Örnek: Manpower miktarını Slider ile seçtirmek —
  "zor mu?" diye soruldu, aslında kolay çıktı çünkü Unity'nin hazır Slider component'i tam bu işe
  uygun, InputField'dan bile daha az hataya açık.)
- Hata geldiğinde önce sakinleştir ("bu normal, panik yok"), sonra sırayla kontrol ettir: Console'daki
  EN ESKİ/İLK hata ne diyor, dosya kaydedilmiş mi (Ctrl+S), doğru obje/script Inspector'a sürüklenmiş mi,
  Button'ın `On Click()` listesinde **doğru obje + doğru fonksiyon** seçili mi (eski/placeholder bir
  fonksiyon kalmış olabilir). **Sessizce çalışmayan (hata vermeyen ama beklenen etkiyi yapmayan)
  bug'larda** tahminle vakit kaybetmek yerine geçici `Debug.Log` satırları ekleyip gerçek veriyi
  (instance ID, string değerleri vs.) görmek çok daha hızlı sonuç veriyor — bkz. Bilinen Tuzaklar,
  "danışman kilidi çalışmıyor" örneği.
- Kullanıcı "Create → C# Script" ile MonoBehaviour şablonu oluşturduğunda kafası karışabiliyor;
  hangi script'in MonoBehaviour (sahneye yapıştırılacak) hangisinin düz veri sınıfı (`: MonoBehaviour`
  YOK) olduğunu her seferinde net söyle.
- Unity Editor ekran görüntülerinden Hierarchy/Inspector okuyup yönlendirmek çok işe yarıyor —
  kullanıcı ekran görüntüsü attığında oradaki obje isimlerini/alan değerlerini birebir okuyup
  ona göre spesifik adımlar ver (genel geçer talimat değil).
- Sahne dosyası (`SampleScene.unity`) bir YAML metni — bir buton gerçekten doğru fonksiyona mı bağlı
  diye kontrol etmek gerektiğinde, kullanıcıya sormadan önce dosyayı grep'leyip doğrulamak çok işe
  yarıyor (bkz. Mektuplar butonu kontrolü örneği).
- Kullanıcı CAPS LOCK ile ("LAN", "AMK" gibi argo/sinirli ifadelerle) yazdığında bu gerçek bir
  hakaret/öfke değil, sadece yorgunluk/hayal kırıklığı ifadesi — sakin, çözüm odaklı, teknik
  yanıt vermeye devam et, savunmaya geçme ya da özür dileme, direkt sorunu çöz.

---

## 2) OYUN KONSEPTİ

Diyalog ve karar temalı bir **"kral simülasyonu"** oyunu (Unity, C#, **2D**). Oyuncu bir kral,
ülkeyi yönetiyor.

### Stat'lar
- **Erzak**: Ayrı bir "genel/kingdom" deposu YOK. Tek gerçek kaynak, köylerin kendi Erzak
  stoklarının toplamı (bkz. "Köyler" bölümü ve Mimari Kararları #7).
- **Sadakat**: 0-100 arası, hem "genel" bir bileşeni var hem köylerin kendi Sadakat'ları — ekranda
  gösterilen "Sadakat", genel + köylerin ortalaması. **Sadakat asla bir seçeneği kilitlemiyor**
  (harcanan bir kaynak değil, sadece 0-100 arasına kelepçeleniyor). **Bir köyün Sadakat'ı 50'nin
  altına düşerse o köy isyan riski taşımaya başlar** (bkz. "İsyan Mekaniği").
- **Altın, Manpower**: Hâlâ tamamen "genel/kingdom-wide" — köylere bölünmüyor, harcanabilir kaynaklar
  (yetersizse ilgili seçenek/emir kilitleniyor). Manpower, isyan bastırma emrinde oyuncunun
  kendi seçtiği bir miktar olarak köye "gönderiliyor" (bkz. "İsyan Mekaniği").
- **Nüfus** (bu oturumda eklendi): Erzak ile aynı desen — ayrı bir "genel" deposu yok, kingdom
  Nüfus'u = tüm köylerin Nüfus'unun toplamı. Sol üstteki panelde ve Köy Bilgi Paneli'nde gösteriliyor.
  Günlük artışı sabit değil, köyün kendi Erzak stokuna bağlı dinamik bir formülle hesaplanıyor
  (bkz. "Nüfus Mekaniği").

### Döngü (Cycle) Sistemi
Oyun "döngü" (cycle) sistemiyle ilerliyor. **Her döngü = 1 gün** (`GameState.Gun`, ekranın üstünde
`GunUI.cs` ile "Gun X" olarak gösteriliyor), 2 bölümden oluşuyor:

**1. TAHT ODASI**
Sıradaki NPC'ler tek tek gelip oyuncuyla diyalog kuruyor. Diyalog seçimleri stat değişikliklerine
yol açabiliyor (bir seçenek birden fazla stat'ı aynı anda etkileyebiliyor, bkz. `DialogueData.cs`).
**Bazı NPC'ler belirli bir köyü temsil ediyor** (bkz. aşağıdaki "Köyler" bölümü) — bu durumda o
NPC'nin Erzak/Sadakat etkileri kingdom-wide değil, doğrudan o köyün kendi verilerine işliyor. Sıra
bitince, kısa bir **siyah ekran geçişiyle** (bkz. `EkranGecisi.cs`) otomatik olarak 2. bölüme geçiliyor.

**2. ŞAHSİ ODA**
Kralın (oyuncunun) gözünden, önünde bir masa olan bir oda (Papers Please tarzı POV). Masanın
üzerinde/önünde 2 etkileşim objesi var: **Ansiklopedi** (kitap, hâlâ boş placeholder) ve **Harita**
(bkz. aşağıdaki "Harita" bölümü). **Mektuplar objesi ve tüm ilgili sistem tamamen kaldırıldı.**
Arka planda bir **Kapı** var. Kapıya tıklayınca danışman listesi açılıyor (şu an **General** ve
**İnşaatçı**), listeden birine tıklayınca o danışman "içeri girip" Warband tarzı dallanan bir
diyalog başlatıyor. Her danışman **döngü başına sadece 1 kez** kullanılabiliyor (`DanismanButon.cs`
ile görsel kilitleniyor, gerçek kısıtlama `OrderManager` seviyesinde — **bu kilit `DanismanTipi`
string'inin birebir eşleşmesine dayanıyor, dikkatli olunmalı, bkz. Bilinen Tuzaklar**). Emir
verildiğinde **hiçbir stat anında değişmiyor**, emir sadece "bekleyen emirler" listesine kaydediliyor.

#### Danışman diyalog akışı (Warband tarzı, kapı sistemi)
`DialogueData`/`DialogueNode`/`DialogueChoice` sistemi genişletildi: bir `DialogueChoice`, isteğe
bağlı bir **`VerilecekEmir`** (`OrderData`) taşıyabiliyor. Seçildiğinde otomatik `OrderManager.EmirEkle()`'ye
gönderiliyor. **General**: "Bir görevim var" → **"İsyan Bastır"**, **"Saldır"** (bu oturumda eklendi,
bkz. "Savaş Mekaniği") VEYA "Asker Topla" (direkt emir) — artık 3 seçenek, aşağıdaki dinamik seçenek
sistemi sayesinde ekrana sığıyor. **İnşaatçı**: "Değirmen İnşa Et" seçilince **hangi köyde yapılacağı,
diyalog kutusunun İÇİNDE, kaydırılabilir bir köy listesiyle** soruluyor (bkz. Mimari Kararları #9) —
köy seçilene kadar diyalog kutusu kapanmıyor.

**Diyalog seçenekleri artık tamamen dinamik (bu oturumda eklendi, bkz. Mimari Kararları #20).**
Eskiden diyalog kutusunda sabit 2 buton vardı (`SecenekButon1`/`SecenekButon2`), bu **2 seçenekle
sınırlıydı**. Artık `DialogueManager.NodeGoster()`, bir şablon butonu (`SecenekButonSablonu`) bir
**Scroll View**'ın içine (`SecenekIcerik`, Vertical Layout Group + Content Size Fitter) seçenek
sayısınca çoğaltıyor — 2'den fazla seçenek olunca otomatik kaydırılabilir hale geliyor. Köy seçimi de
(eskiden ayrı bir `KoySecimPaneli` objesiydi) artık aynı dinamik seçenek sistemi üzerinden gösteriliyor
(`DialogueManager.KoySecimGoster()`) — **`KoySecimPaneli.cs` bu oturumda tamamen silindi**, hem script
hem sahnedeki obje. Bu filtreleme mantığı da köy seçimine taşındı: `BinaSlotuKullanir`/`IsyanliKoyGerekli`
işaretliyse ilgili köyler "(Dolu)"/"(Isyan Yok)" etiketiyle **devre dışı** gösteriliyor ama listede
kalıyor; `DusmanKoyuGerekli` işaretliyse (Saldır emri) ise **sadece düşman köyleri listeleniyor**,
bizim köylerimiz hiç görünmüyor (İnşaatçı/İsyan Bastır'da ise tam tersi: sadece bize ait köyler
görünüyor, düşman köyleri hiç listeye eklenmiyor) — bkz. Mimari Kararları #20.

**Diyalogda "Geri" ve "Boşver" (bu oturumda eklendi, bkz. Mimari Kararları #27):** Danışman
diyaloglarında bir alt menüye girdikten sonra (örn. General → "Bir görevim var" → İsyan Bastır)
oyuncunun geri dönüp başka bir seçenek seçme ya da hiçbir emir vermeden çıkma yolu yoktu — kullanıcı
bunu "her seçim illa bir emirle bitiyor" diye rahatsız edici buldu. Çözüm iki parça: **Geri butonu**
(`DialogueManager.GeriButonu`, Chrome'daki geri tuşu gibi) her node değişiminde ziyaret edilen node'u
bir `Stack<DialogueNode>`'a (`gecmisNodeler`) kaydediyor, tıklanınca son node'a geri dönüyor, geçmiş
boşsa buton otomatik gizleniyor. **"Boşver" seçeneği** ise ayrı bir buton DEĞİL — danışman
diyaloglarında (`aktifDanismanDiyalogu == true`), diğer seçeneklerle (ya da köy seçim listesiyle)
aynı görünümde, listenin sonuna otomatik eklenen dinamik bir buton (`BosverSecenegiEkleIstenirse`),
tıklanınca hiçbir stat/emir etkisi olmadan direkt `DiyalogBitir()` çağırıyor. Normal NPC
diyaloglarında (Köylü/Asker/Ayyaş) "Boşver" görünmüyor — kullanıcı bilerek sadece danışman
diyaloglarıyla sınırladı.

### Köyler
Oyuncu ülkeyi tek bir "kingdom" olarak görse de, arka planda bunun kaynağı **köyler** (`KoyData`,
bkz. Script Envanteri). Her köyün kendi `Sadakat`, `Erzak`, `ErzakYield` (günlük Erzak artışı),
`AltinYield`'i, `Nufus`'u (bkz. "Nüfus Mekaniği"), `Savunma`'sı (isyan bastırmada kullanılmıyor,
dış savaşta kullanılıyor, bkz. "Savaş Mekaniği"), `MaxBinaSlotu`/`DoluBinaSlotu` (bina sınırı),
`IsyanHalinde` (isyan durumu), **`Sahip`** (hangi krallığa ait — `Krallik` enum, bu oturumda
eklendi, bkz. "Savaş Mekaniği") ve **`Garnizon`**'u (o köyde konuşlanan Manpower, bu oturumda
eklendi) var.
**Kingdom Erzak'ı/Nüfus'u/Yield'leri = SADECE bize ait (`Sahip == Oyuncu`) VE isyanda olmayan
köylerin toplamı** (bkz. Mimari Kararları #22 — bu oturumda düşman köyleri de bu dışlamaya dahil
edildi, önceden sadece isyan filtreleniyordu). Kingdom Sadakat'ı = genel + (sadece bize ait)
köylerin ortalaması.

- **Köylü NPC'si artık gerçek bir köyü temsil ediyor.** Her gün, Erzak'ı 50'nin altında olan **her
  köy** kendi 0-50 arası zarını atıyor; zar köyün Erzak'ından yüksek çıkarsa o köy kendi Köylü'sünü
  taht odasına gönderiyor (aynı gün birden fazla köy tetiklenebilir, bkz. `DaySequencer.cs`).
  Diyalog metninde `{KOY}` yazan yer, o köyün adıyla değiştiriliyor. "Tamam al" (erzak ver) seçeneği:
  **Altın -20** (kingdom-wide, merkezi kaynaktan gidiyor) + **Erzak +20** (o köye gidiyor, köy hedefli)
  + **Sadakat +10** (o köyün kendi Sadakat'ı artıyor). "Yok git": **Sadakat -15** (o köyün Sadakat'ı,
  0'ın altına düşmüyor, seçenek asla kilitlenmiyor).
- **İnşaatçı/Değirmen artık belirli bir köyü hedefliyor.** Değirmen tamamlanınca genel `ErzakBaseGelir`
  yerine, oyuncunun seçtiği **o köyün** `ErzakYield`'i **2 katına çıkıyor** (eskiden sabit +1'di,
  şimdi çarpım — bu yüzden oyun başındaki rastgele Yield yüksek olan köylere değirmen yapmak daha
  mantıklı, bkz. aşağıdaki "Yeni Oyun Başlangıcı").
- **Building slot/sınırı sistemi kuruldu ve çalışıyor** (eskiden bilinçli ertelenmişti). Her köyün
  `MaxBinaSlotu` (Inspector'dan köy başına ayarlanabilir, varsayılan 3) ve `DoluBinaSlotu`'u var.
  Değirmen emri **köy seçilir seçilmez** (inşaat başlarken, tamamlanmasını beklemeden) o köyün
  `DoluBinaSlotu`'unu +1 artırıyor. Slotu dolu bir köy, `KoySecimPaneli`'nde "(Dolu)" etiketiyle
  görünüp tıklanamıyor.

### Yeni Oyun Başlangıcı
Oyun her başladığında (`KoyYoneticisi.Awake()` — şu an sahneler arası geçiş olmadığı için bu, Play'e
basıldığı an anlamına geliyor), her köyün `ErzakYield`'i **1 ile 4 arası rastgele** bir değere
ayarlanıyor. Bu, değirmen inşa etme kararını (hangi köye yapılır) stratejik kılmak için bilinçli
bir tasarım — Yield'i yüksek gelen köye değirmen yapmak daha karlı.

### İsyan Mekaniği (bu oturumda eklendi)
Sadakat'ı 50'nin altına düşen köyler isyan riski taşır. **Her gece (Resolve aşamasında, Uyu'ya
basılınca)**, `IsyanHalinde` olmayan ve Sadakat'ı 50'den düşük olan **her köy** için 1-50 arası bir
zar atılır (`KoyYoneticisi.IsyanKontrolEt`); zar köyün Sadakat'ından yüksek çıkarsa köy **isyan
durumuna** girer (`KoyData.IsyanHalinde = true`). Bu bir "durum" (status) — köy, isyan bastırılana
kadar bu durumda kalır, kendi kendine düzelmez.

**Debuff:** İsyan halindeki bir köyün `ErzakYield`'i ve `AltinYield`'i **hem gerçek gece
artışında hem sol üstteki tahmini gelir göstergesinde (StatsUI) 0 sayılır** — köyün kendi
`ErzakYield`/`AltinYield` alanları bozulmaz/sıfırlanmaz (değirmen etkisi vs. korunur), sadece
`KoyYoneticisi`'nin toplama/uygulama fonksiyonları (`ErzagiGunlukArtir`, `ToplamErzakYieldi`,
`ToplamAltinYieldi`) isyandaki köyleri atlar. Yani isyan bitince (Sadakat/Yield'e dokunmadan)
üretim otomatik kaldığı yerden devam eder.

**Görsel geri bildirim:**
- İsyan başladığında, ertesi sabah elçi/ulak mesajında kırmızı bir "X isyan etti!" satırı çıkar.
- Harita'daki köy isim etiketi, isyan sürdükçe **kırmızı** renkte görünür (normalde hover dışında
  elle ayarlanmış rengiyle görünür, bkz. `KoyEtiketiTiklama.cs`).
- Köy Bilgi Paneli'nde (etikete tıklayınca açılan popup, bkz. "Köy Bilgi Paneli") kırmızı
  **"ISYAN HALINDE"** yazısı ve Erzak/Altın Yield değerleri (normalde +yeşil/-kırmızı/0-beyaz
  yerine) **gri** gösterilir — "bu sayı gerçek ama bize gelmiyor" anlamında.

**İsyan Bastırma (General → "İsyan Bastır"):** Eski "Köy Yağmala" seçeneğinin yerine geçti.
Seçilince `KoySecimPaneli` açılır ama **sadece isyanda olan köyler tıklanabilir** (diğerleri
"(Isyan Yok)" etiketiyle devre dışı). Köy seçilince `ManpowerSeciciPaneli` açılır (bir **Slider**
ile 0 ile mevcut Manpower arası bir miktar seçiliyor, bkz. "Manpower Seçici Paneli"), seçilen
miktar doğrudan koddan Manpower'dan düşülür ve emir kuyruğa eklenir. Gece (Resolve) bu emir
işlenince, **başarı şansı dinamik hesaplanıyor**: `BasariSansi = GonderilenManpower /
(GonderilenManpower + HedefKoy.Nufus)` (bu oturumda `HedefKoy.Savunma` yerine `HedefKoy.Nufus`
kullanacak şekilde değiştirildi, bkz. Mimari Kararları #17, #19 — gerekçe: isyan halkın kendi
nüfusuyla orantılı bir direnç göstersin, `Savunma` ileride dış savaş için ayrılsın) — ikisi de 0
ise 0'a bölme hatasını önlemek için %50 kabul ediliyor. Ne kadar çok Manpower gönderirsen bastırma
ihtimalin o kadar yükseliyor ama köyün Nüfus'u yüksekse daha fazla Manpower gerekiyor. Başarılıysa
köyün `IsyanHalinde`'si `false` olur ve elçi mesajında yeşil bir "X köyündeki isyan bastırıldı!"
satırı çıkar, başarısızsa kırmızı "X köyündeki isyanı bastıramadık." satırı çıkar — her iki durumda
da mesaja **Manpower kayıp/dönüş** bilgisi eklenir (bu oturumda eklendi, bkz. Mimari Kararları #19):
kazanırsa gönderilen Manpower'ın %15'i, kaybederse %80'i kaybedilir, geri kalanı kingdom'a geri
döner. **Not:** Bastırma sadece `IsyanHalinde` bayrağını kapatıyor, köyün altta yatan düşük
Sadakat'ını düzeltmiyor — yani Sadakat hâlâ 50'nin altındaysa, köy ilerideki bir gece tekrar isyan
riskiyle karşılaşabilir (bu, bilinçli/beklenen bir durum, henüz bir "sonrası" tasarlanmadı).

### Nüfus Mekaniği (bu oturumda eklendi)
Her köyün `Nufus`'u var (varsayılan 30), kingdom Nüfus'u = tüm köylerin toplamı (tıpkı Erzak gibi,
bkz. Mimari Kararları #7). Günlük artış **sabit bir sayı değil**, her gece köyün kendi
`Erzak` stokuna göre dinamik hesaplanıyor (`KoyYoneticisi.NufusYieldHesapla`, bkz. Mimari
Kararları #18): `KisiBasiStok = Erzak / Nufus`, bu değer bir **Eşik**'in üstündeyse köy büyür,
altındaysa küçülür — fark bir **Katsayı** ile çarpılıp yuvarlanıyor. `NufusEsik` (varsayılan 1) ve
`NufusKatsayi` (varsayılan 10), `KoyYoneticisi`'nin Inspector'ında ayarlanabilir. İsyan halindeki
köyler bu hesaptan da atlanıyor (diğer Yield'ler gibi debuff'lanıyor). Sol üstteki StatsUI'de ve
Köy Bilgi Paneli'nde "Nufus: X <sup>+Y</sup>" formatında, üst indis +yeşil/-kırmızı/0-beyaz
renkleniyor (isyanda gri).

**Neden Erzak stoku (ErzakYield değil)?** İlk denemede formül `ErzakYield/Nufus` (günlük üretim)
üzerine kuruluydu, ama kullanıcı bunun mantıksız olduğunu fark etti: üretim sabit kalırken stok
(`Erzak`) birikmeye devam etse bile, üretim/nüfus oranı nüfus arttıkça küçülüp büyümeyi durduruyordu
— yani "depoda erzak yığılıyor ama kimse büyümüyor" gibi bir tuhaflık oluşuyordu. Stok tabanlı
formüle geçilince bu sorun çözüldü: erzak biriktikçe (üretim sabit kalsa bile) kişi başı stok da
büyür, nüfus artışı durmaz.

### Ekonomi Giderleri (bu oturumda eklendi)
Kullanıcı fark etti: oyunda sadece GELİR vardı, hiçbir GİDER yoktu — oyuncu "Uyu"ya basıp beklese
bile hiçbir zaman eksiye düşmüyordu, bu da stratejik gerilimi sıfırlıyordu. İki gider eklendi, ikisi
de `GameState.GiderleriUygula()` (her gün `GelirleriUygula()` içinde çağrılıyor, gelirle aynı anda):
- **Asker Maaşı:** `Manpower * ManpowerMaasiBirimMaliyeti` (varsayılan **0.05** — yani her 20
  Manpower için günde 1 Altın) kadar Altın her gün düşülüyor. **Periyodik/haftalık ödeme YERİNE
  bilinçli olarak GÜNLÜK** yapılıyor — kullanıcı "oyuncu ödeme gününden hemen önce tüm askeri
  savaşa gönderip kaçamak yapabilir mi?" diye sordu, cevap: günlük hesaplama bu kaçamağı yapısal
  olarak imkânsız kılıyor (o gün elinde olmayan asker için zaten ödeme yapılmıyor, ayrıca hesap her
  gün o günkü Manpower'a bakıyor, geçmişi hatırlamıyor).
- **Bina Bakım Gideri:** `KoyYoneticisi.ToplamDoluBinaSlotu() * BinaBakimBirimMaliyeti` (varsayılan
  **1**) — kingdom'daki (sadece bize ait/isyanda olmayan köylerin) toplam dolu bina slotu sayısına
  göre günlük Altın gideri.
- **İlk denenen oranlar (0.5 / 2) tamamen dengesizdi** — Manpower varsayılan 50 olduğu için
  `50*0.5=25 Altın/gün` gider çıkıyordu, ama `AltinBaseGelir` sadece 3 — 2. günden itibaren asla
  Altın kazanılamıyordu. Kullanıcı bunu net görüp "hala para kazanamıyorum" diye bildirdi, oranlar
  0.05/1'e düşürüldü. **Kesin doğru sayılar değil**, ölçek büyüdükçe (kullanıcının planı: Manpower
  ileride 500k-1M gibi gerçekçi büyük sayılara çıkabilir) birlikte yeniden dengelenecek.
- **StatsUI'nin tahmini Altın göstergesi de güncellendi** — bu iki gideri de düşüyor, yoksa
  "Uyu'ya basmadan önce gördüğün tahminle gerçekleşen uyuşmuyor" hatasına (Mimari Kararları #17'de
  bir kere düzeltilmişti) tekrar düşülürdü. Altın hover tooltip'i de artık "Köyler: +X / Asker
  Maaşı: -Y / Bina Bakımı: -Z" şeklinde dökümü gösteriyor (`KoyYoneticisi.AltinYieldDagilimMetni`).

### Ordu Kaynağı ve Mesafeye Bağlı Süre (bu oturumda eklendi)
Kullanıcının sorusu: "Asker nereden yola çıkıyor? Başkentten mi (henüz yok), yoksa en yakın
garnizonlu yerleşkeden mi?" Tartışılan iki uç seçenek yerine (hep Başkent'ten / otomatik en-yakın-
garnizon algoritması) **üçüncü, hibrit bir yol** seçildi: **`GameState.Manpower` (genel havuz)
konumsuz bir "seyyar yedek kuvvet" olarak kalıyor** (Başkent'e gerek yok, hiç değişmedi), buna ek
olarak oyuncu **emir verirken kaynağı kendisi seçiyor** — otomatik "en yakını bul" algoritması
YAZILMADI (kullanıcı haritaya bakıp kendi karar veriyor).

**Akış (İsyan Bastır, Saldır, yeni "Garnizon Gönder" emirlerinin hepsinde ortak):** Hedef köy
seçildikten sonra, emrin `KaynakSecimiGerekli` bayrağı işaretliyse `DialogueManager.KaynakSecimGoster`
devreye giriyor — "Genel Yedek Kuvvet" (her zaman ilk seçenek, `KaynakKoy = null`) + `Garnizon > 0`
olan her bizim yerleşke (`KoyYoneticisi.GarnizonluKoyler()`, hedef köy kendisi hariç tutuluyor)
listeleniyor. Kaynak seçilince Manpower Slider'ı açılıyor (`ManpowerSeciciPaneli.Sor` artık
maksimumu PARAMETRE olarak alıyor — eskiden hep `GameState.Manpower`'a sabitti, artık ya o ya da
seçilen köyün `Garnizon`'u). Seçilen miktar, kaynağa göre doğru yerden düşülüyor (`GameState.Manpower`
ya da `kaynakKoy.Garnizon`).

**Mesafe → süre:** `OrderData.KopyalaVeKoyAta` sonrası, `kopya.ToplamSure = HaritaYoneticisi.
Instance.SureHesapla(kaynakKoy, hedefKoy)` ile OrderData şablonundaki SABİT `ToplamSure` ARTIK
KULLANILMIYOR, her seferinde runtime'da hesaplanıyor. `SureHesapla`: kaynak "Genel Yedek Kuvvet"
(null) ise her zaman **1 gün** (fiziksel konumu olmadığı için anlık), kaynak bir köyse
`HexMesafe(kaynak, hedef) / OrduHizi` (varsayılan `OrduHizi = 3`, `HaritaYoneticisi`'nde
Inspector'dan ayarlanabilir), en az 1 gün.

**Hayatta kalan asker nereye döner:** `DayResolver.AskeriGeriGonder(state, emir, miktar)` — emrin
`KaynakKoy`'u doluysa oraya (`Garnizon`'a) döner, boşsa (Genel Yedek Kuvvet'ten gittiyse) `GameState.
Manpower`'a döner. Hem İsyan Bastır hem Saldır'ın "kayıp/dönüş" mantığı bunu kullanacak şekilde
güncellendi.

**Yeni "Garnizon Gönder" emri (General):** İsyan Bastır ile aynı iskelet ama sonucu ŞANSA BAĞLI
DEĞİL — süre dolunca garanti olarak `GonderilenManpower`, `HedefKoy.Garnizon`'a ekleniyor
(`OrderData.GarnizonEkler` bayrağı, hem anlık (`ZarAtVeUygula`) hem çok-günlü (`DayResolver`'ın
`SonucMesajlariniOlustur`'undaki `else if` zinciri) yollarında ayrı ayrı ele alınıyor gerekiyordu).
Bunu kullanarak oyuncu köyler/şehirler/kaleler arasında garnizon TAŞIYABİLİYOR da (kaynak olarak
başka bir garnizonlu köy seçilebildiği için).

**Önemli tuzak (canlı yaşandı):** `SaldiriBaslatir`/`IsyanBastirir` gibi ZAR ATAN emirler, artık
mesafe yüzünden çoğu zaman **çok günlü** oluyor. Çok günlü bir emrin zar atıp gerçek sonucu (kazanç/
kayıp, sahiplik değişimi) uygulaması için `OrderData.SonucSansaBagli = true` işaretli OLMASI
GEREKİYOR — bu alan eskiden önemsizdi çünkü emirler hep aynı gece (`ToplamSure<=1`) sonuçlanıyordu,
mesafe sistemi gelince gizli kalan bu eksik ortaya çıktı: `Saldır`'ın `Sonuc Sansa Bagli`'sı
işaretli değildi, 3 gün süren bir saldırı "tamamlandı" diyip hiçbir şey yapmadan geçiyordu (ne zar,
ne sahiplik değişimi). Kullanıcıya Inspector'dan bu kutucuğu işaretlettirerek çözüldü — **yeni bir
şansa-bağlı+köy-hedefli emir eklerken bu her zaman kontrol edilmeli.**

### Savaş Mekaniği
Diğer krallıklarla savaşın **ilk temeli** kuruldu. **Önemli:** `Krallik` artık düz bir enum değil —
bkz. "Diplomasi Mekaniği" bölümünün başındaki güncelleme notu ve Mimari Kararları #25. `KrallikData`
(ScriptableObject) ve `KoyData.Sahip` alanı ile her köyün kime ait olduğu belirleniyor. `KoyData.Garnizon` (int, varsayılan 0) o köyde konuşlanan
Manpower'ı tutuyor — **kalıcı bir konsept**: kullanıcı "köyün kendi doğal savunması (köylüler) +
bizim/düşmanın koyduğu garnizon birlikte savunuyor, garnizon savunmayı güçlendiriyor" istedi, bu
yüzden `KoyYoneticisi.EtkinSavunmaHesapla(koy)` = `Savunma * (1 + Garnizon/GarnizonKatsayisi)`
(`GarnizonKatsayisi`, varsayılan 10, Inspector'dan ayarlanabilir) — çarpımsal bir sinerji, düz toplama
değil.

**Saldırı emri (General → "Saldır"):** İsyan Bastır'la aynı iskelet — `KoySecimiGerekli` +
`DusmanKoyuGerekli` (sadece düşman köyleri listelenir) + `ManpowerMiktariSorulsun` (Slider) +
**`SaldiriBaslatir`** (yeni bayrak, `DayResolver`'da özel dal). Gece sonuçlanınca:
`SaldiriSansi = GonderilenManpower / (GonderilenManpower + EtkinSavunma)` (aynı ratio/Tullock
deseni, isyan bastırmayla aynı %15/%80 kazanç/kayıp Manpower yüzdeleri). **Kazanırsan:** köyün
`Sahip`i `Oyuncu` olur, hayatta kalan Manpower **o köyde Garnizon olarak kalır** (kingdom'a geri
dönmez — kullanıcının kararı: yeni alınan köy savunmasız kalmasın). **Kaybedersen:** köy düşmanda
kalır, hayatta kalan Manpower kingdom'a geri döner.

**Düşman köyünün ekonomisi donuk (bilinçli karar):** Düşman köylerinin `Erzak`/`Nufus` gibi
alanları **hiçbir yapay zeka olmadan** statik kalıyor — `ErzagiGunlukArtir()`/`NufusuGunlukArtir()`
onları atladığı için biz fethedene kadar hiç büyümüyor/değişmiyor (bkz. Mimari Kararları #22).
Ekonomiyi canlandıran gerçek bir AI henüz yok (ayrı, büyük bir iş) ama **temel bir saldırı AI'ı bu
oturumda eklendi**, aşağıya bak.

**Düşman AI'ı — ilk versiyon (bu oturumda eklendi):** `KoyYoneticisi.DusmanSaldirilariniKontrolEt`,
her gece (isyan/diplomasi kontrolünden hemen sonra, `DayCycleManager.UyuyaBas()`'ta) çalışıyor.
Sadece **savaşta olan** (`SavastaMi == true`) VE **Garnizonu 0'dan büyük** olan düşman köyleri için
**%20 ihtimalle** (`DusmanSaldiriIhtimali`, Inspector'dan ayarlanabilir) saldırı tetikleniyor. Hedef,
`HaritaYoneticisi.KoyMesafesi` ile saldıran köye **en yakın bizim köy**. Başarı şansı, oyuncunun
Saldır emriyle birebir aynı formül (`DusmanGarnizonu / (DusmanGarnizonu + HedefinEtkinSavunmasi)`),
kazanç/kayıp yüzdeleri de aynı (%15/%80). Kazanırsa köyün `Sahip`i değişiyor, harita rengi otomatik
güncelleniyor. **Bilinçli sınırlama:** düşman köyünün Garnizon'u hâlâ statik/sabit (Inspector'dan elle
girilir, hiç büyümüyor/başka köye taşınmıyor) — gerçek bir "düşünen" rakip (ekonomi + garnizon
transferi + hedef seçme mantığı) için ayrı, büyük bir iş gerekiyor, bilerek ertelendi.

**Harita rengi otomatik:** `KoyEtiketiTiklama.cs` artık köyün rengini `Sahip`e göre kendi hesaplıyor
(isyanda kırmızı, `Sahip == Dusman` ise mavi `DusmanRengi`, aksi halde etiketin kendi normal rengi) —
elle her düşman köyü için renk ayarlamaya gerek yok, bkz. Mimari Kararları #21.

**Köy Bilgi Paneli düşman köyünde farklı görünüyor:** `Sahip != Oyuncu` ise Sadakat/Erzak/Nüfus/
Yield'ler/Bina Slotu **tamamen gizleniyor** (bunlar düşman köyü için anlamsız, çünkü ekonomisi
donuk), sadece İsim + Savunma + **Garnizon** (yeni eklendi) gösteriliyor — bize ait köylerde her şey
eskisi gibi görünmeye devam ediyor.

### Diplomasi Mekaniği (bu oturumda eklendi)
**Önce bir mimari düzeltme notu:** Bu dosyanın önceki bir versiyonu `Krallik`'in düz bir
`enum { Oyuncu, Dusman }` olduğunu söylüyordu — bu artık YANLIŞ/eski bilgi. Bir önceki oturumda
(kullanıcı tarafından, bu özetin hazırlanmasından sonra) `Krallik` **`KrallikData`** adında bir
ScriptableObject'e dönüştürüldü. Artık her krallık kendi asset'i: **`Vollen_Krallik.asset`**
(oyuncunun krallığı, **mavi**) ve **`Babbar_Krallik.asset`** (düşman, **kırmızı** — bu oturumda
Vollen beyazdan maviye, Babbar maviden kırmızıya çevrildi, haritada net ayırt edilsin diye) — `Isim`, `HaritaRengi`,
`Bayrak` (Sprite) alanları var. `KoyYoneticisi.OyuncuKralligi` ve `KoyData.Sahip` artık bu tipte.
Diplomasi sistemi bu çoklu-krallık veri modeli üzerine kuruldu (bkz. Mimari Kararları #25).

Kullanıcının isteği basitti: "diplomasi değerimiz belli bir eşiğin altına düşünce, tıpkı isyan
mekaniğindeki gibi, düşman bize savaş açabilsin." Popüler stratejı oyunlarındaki (HOI4, EU4, Total
War) "ilişki değeri + eşik altına düşünce çatışma riski" deseni, projenin zaten var olan isyan
mekaniğiyle (durum bayrağı + gece zarı + eşik) hibritlenerek kuruldu — aynı zar formülü, aynı
"kendi kendine düzelmeyen durum bayrağı" mantığı kullanıldı, kullanıcı zaten bu deseni tanıyıp
güvendiği için.

**Veri modeli:** Yeni **`DiplomasiVerisi`** (düz `[Serializable]` C# class, ScriptableObject DEĞİL —
`KoyData`/`GameState` ile aynı gerekçe, runtime'da değişen değer Play modundan çıkınca sıfırlanmasın):
`KrallikData Krallik`, `int Diplomasi` (varsayılan 60, 0-100 aralığı), `bool SavastaMi`. `KoyYoneticisi`
artık `public List<DiplomasiVerisi> Diplomasiler` (Inspector'dan elle, her düşman krallık için bir
eleman eklenir — `Koyler` listesiyle birebir aynı desen) ve `public int DiplomasiEsikDegeri`
(varsayılan 40) tutuyor.

**Gece kontrolü (`KoyYoneticisi.DiplomasiKontrolEt`):** `IsyanKontrolEt` ile birebir aynı formül —
her `DiplomasiVerisi` için, `SavastaMi` false VE `Diplomasi < DiplomasiEsikDegeri` ise 1-50 arası bir
zar atılıyor; zar `Diplomasi`'dan yüksekse `SavastaMi = true` olup elçi mesajına kırmızı "X Krallığı
bize savaş açtı!" satırı ekleniyor. `DayCycleManager.UyuyaBas()`'ta `IsyanKontrolEt`'in hemen
ardından çağrılıyor.

**Görüntüleme (`DiplomasiBilgiPaneli`, sağ tık):** Kullanıcının isteği: "HOI4'te bir ülkenin köyüne
sağ tıklayınca o ülkeyle ilişkimizi görürüz, biz de böyle yapalım." `KoyEtiketiTiklama.OnPointerClick`
artık `eventData.button`'a bakıyor: **sol tık** eskisi gibi `KoyBilgiPaneli.Goster(koy)`; **sağ tık**
ise (sadece köy bize ait değilse) `DiplomasiBilgiPaneli.Instance.Goster(koy.Sahip)` çağırıp o
krallığın adını, bayrağını, Diplomasi değerini ve "SAVASTA"/"BARISTA" durumunu gösteriyor. Kendi
köylerimize sağ tıklayınca hiçbir şey açılmıyor (kendimizle diplomasi kavramı yok).

**Diplomasi'nin devamı — Elçi danışmanı ve Barış Yap (bu oturumda eklendi):** Kapıdan çağrılan
üçüncü danışman **Elçi** (`Elci_NPC`/`Elci_Diyalog` asset'leri, `DanismanCagir.ElciCagir()`, diğer
danışmanlarla aynı "döngü başına 1 kez" kilidine tabi) iki seçenek sunuyor, ikisi de **emir olarak
gece işleniyor** (anında değil):
- **Hediye Gönder** → `OrderData.DiplomasiyiArttirir = true` + `DiplomasiMiktari` (örn. +10) +
  `HedefKrallik` (sabit, Inspector'dan atanmış — şu an tek düşman krallık olduğu için diyalogda
  krallık seçimi YOK, doğrudan asset'e gömülü). Altın maliyetli (mevcut `MaliyetStat`/`MaliyetMiktar`
  mekanizmasıyla). Gece `DayResolver.ZarAtVeUygula`'da `KoyYoneticisi.DiplomasiDegistir` çağrılıp
  yeşil bir mesaj ekleniyor.
- **Barış Teklif Et** → `OrderData.BarisTeklifEder = true` + `HedefKrallik`. Altın maliyetli, başarı
  şansı **o anki Diplomasi değerine bağlı** (`Diplomasi/100` — ilişki iyiyse barış kolay). Başarılıysa
  `KoyYoneticisi.BarisYap(krallik)` ile `SavastaMi = false` olur. **Savaşta değilken bu seçenek
  otomatik gri/tıklanamaz** — `DialogueManager.SecenekKarsilanabilirMi` içine eklenen kontrol,
  `BarisTeklifEder` işaretli VE `!SavastaMi(HedefKrallik)` ise butonu kilitliyor (aynı fonksiyonun
  Erzak/Altın yetersizliği kontrolüyle aynı yerde, mevcut deseni genişleterek).

Bu iki emrin ikisi de köy seçimi/Manpower akışına GİRMİYOR (`KoySecimiGerekli = false`) — İsyan
Bastır/Saldır gibi değil, "Asker Topla" gibi **direkt emir**, seçilir seçilmez `OrderManager.EmirEkle()`
ile kuyruğa giriyor.

### Hover Bilgi (Tooltip) Sistemi — genişletildi (bu oturumda, kısmen yarım kaldı)
Sol üstteki stat panelinin üzerine gelince (örn. Erzak) fareyi takip eden bir bilgi kutusu açılıyor
(örn. Erzak için köy köy döküm). Bunun için üç parça değişti:
- **`StatsUI.cs`** artık tek bir `StatlarText` yerine **5 ayrı `TMP_Text`** kullanıyor (Erzak/Altın/
  Manpower/Nüfus/Sadakat) — sebep: hover'ın hangi stat satırının üzerinde olduğunu ayırt edebilmesi
  için her stat'ın kendi objesi olması gerekiyordu (tek metin kutusunda bu ayrım yapılamaz).
- **`StatTooltip.cs`** (eskiden `switch(StatAdi)` ile sabit bir stat listesine bakıyordu) artık
  **genel/ölçeklenebilir** bir tasarıma geçti (bkz. Mimari Kararları #23): sabit string yerine bir
  **`Func<string> MetinFonksiyonu`** taşıyor, bu fonksiyonu ilgili bilgiyi zaten bilen script (örn.
  `StatsUI.Start()`, `KoyYoneticisi.Instance.ErzakDagilimMetni` fonksiyonunu atıyor) koddan
  atıyor. Bu sayede her yeni hover için `StatTooltip.cs` dosyasına yeni bir `case` eklemeye gerek
  kalmıyor, sadece component ekleyip bir satır kod yazmak yeterli.
- **`TooltipUI.cs`** artık fareyi **takip ediyor** (`Update()`'te her karede pozisyon güncelleniyor).
  Bu, bu oturumda hatırı sayılır bir debug sürecinden geçti — bkz. Bilinen Tuzaklar, "yeni Input
  System" ve "anchor/pivot referans noktası" tuzakları.

**Not (yarım kaldı, yarın devam):** Kutunun kendisi hâlâ **görsel olarak çirkin** — arka plan
(Image component'i, okunur olması için yarı saydam olmayan koyu bir renk) henüz eklenmedi, bir
sonraki oturumda bitirilecek. `Content Size Fitter`/`Vertical Layout Group` padding kurulumu
(metnin etrafını boşlukla sarması) da bu oturumda anlatıldı ama test edilip onaylanmadı.

### Köy Bilgi Paneli
Haritadaki bir köy ismine **tıklanınca** (`KoyEtiketiTiklama.cs`, isim eşleştirmesiyle `KoyData`
buluyor — case-insensitive + trim, ama yine de haritadaki yazı ile `KoyYoneticisi`'ndeki `Isim`
**aynı köyü temsil etmeli**, aksi halde "eşleşen köy bulunamadı" uyarısı çıkar), ekranın ortasında
bir bilgi paneli açılıyor (`KoyBilgiPaneli.cs`): köyün ismi, isyan durumu (varsa), Sadakat, Erzak,
Erzak Yield (+yeşil/-kırmızı/0-beyaz, isyanda griye döner), Altın Yield (aynı renklendirme), Bina
Slotu ("1/3" formatında). Panel üzerinde bir Kapat butonu var. **Etiketin üzerine gelince (hover)**
metin rengi değişiyor (görsel geri bildirim, "buraya tıklanabilir" hissi vermek için).

### Manpower Seçici Paneli
`ManpowerSeciciPaneli.cs` — isyan bastırma emrinde, köy seçildikten sonra açılan, bir **Slider**
(0 ile o anki Manpower arası, tam sayı) ve canlı güncellenen "Gönderilecek: X" metni içeren küçük
bir panel. Gönder butonuna basılınca seçilen miktar callback ile geri döner, o miktar doğrudan
`GameManager.Instance.State`'ten düşülür.

### Harita (eski, çoğu artık geçersiz — bkz. "Hex Harita Sistemi")
Şahsi Oda'daki Harita objesine tıklayınca, ekranı kaplayan bir harita sekmesi açılıyor (sol üstteki
X'e basana kadar kapanmıyor). Fare tekerleğiyle zoom, sol tıkla sürükleyerek pan yapılabiliyor
(bkz. `HaritaKontrol.cs`, hâlâ değişmedi). **Bu oturumda köylerin elle/statik TextMeshPro etiketleri
kaldırıldı** (`KoyEtiketiTiklama.cs` ve o objeler artık kullanılmıyor, muhtemelen ölü kod) — yerine
tamamen kod üretimli bir **hex (altıgen) harita sistemi** geldi, aşağıdaki "Hex Harita Sistemi"
bölümüne bakın. `PixelArtFantasyMap_TheSilentMage` görseli de artık KULLANILMIYOR — arka plan artık
düz renkli, bizim oluşturduğumuz basit bir panel (`HaritaArkaplanGorseli`, Source Image: None, sadece
Color).

### Hex Harita Sistemi (bu oturumda baştan sona kuruldu)
Kullanıcının isteği: Civ 6 tarzı, her köyün/şehrin/kalenin kendi altıgen tile'lardan oluşan bir
"toprağı" (kapsama alanı) olsun, her tile'ın kendi Erzak/Altın değeri olsun, köy bu alandaki
tile'ların TOPLAMI kadar üretsin. Yerleşke KONUMLARI her zaman **sabit/elle** (Inspector'dan), sadece
tile DEĞERLERİ her yeni oyunda rastgele — hem iş azalıyor (yer belirleme otomatik değil ama zaten
elle yapılıyor) hem her oyunda ekonomik çeşitlilik oluyor.

**Veri modeli:**
- **`HexTileData`** (yeni, düz `[Serializable]` class) — `Vector3Int Koordinat` (axial q,r, z=0 hep
  0), `int ErzakDegeri`, `int AltinDegeri`, `KoyData SahipKoy` (nullable).
- **`KoyData`**'ya iki yeni alan: `Vector3Int MerkezTileKoordinati` (köyün elle atanan sabit
  konumu) ve `int TileMenzili` (varsayılan 1 — köy tipine göre büyütülebilir, örn. ileride bir
  "Şehir" daha büyük menzil alabilir; **ŞU AN Köy/Şehir/Kale ayrı tipler DEĞİL**, kullanıcı bilinçli
  olarak bunu ertelemeyi seçti, sadece bu tek sayı köy başına ayarlanabilir).
- **`HaritaYoneticisi`** (yeni, MonoBehaviour, Singleton, `KoyYoneticisi`nin kardeşi) —
  `List<HexTileData> Tileler`, `Vector3Int HaritaMerkezi` (köylerin ortalama konumu, otomatik
  hesaplanır). `KoyYoneticisi.Harita` alanına Inspector'dan bağlanıyor.

**Tile üretimi (`HaritaYoneticisi.TileleriOlustur`, `KoyYoneticisi.Awake()`'te çağrılıyor):**
Rastgele "çiçek" (her köy + 6 komşusu) yöntemi TERK EDİLDİ çünkü köyler arasında sahipsiz/boş
tile'lar bırakıyordu (oyunda tile tek tek fethedilmiyor, sadece yerleşke fethediliyor — bu yüzden
HİÇBİR tile boş kalmamalı). Yerine **ağırlıklı en-yakın-yerleşke (weighted Voronoi)** yöntemi
geldi: haritanın (köylerin ortalama merkezine göre, `SinirPayi` kadar geniş) TAMAMI için, her
koordinata `EtkinMesafe = HexMesafe(tile, koy.Merkez) - koy.TileMenzili` değeri en küçük olan köy
sahip oluyor. Bu hem "hiç boşluk kalmasın" hem "menzili büyük olan (örn. ileride Şehir) daha çok
toprak kazansın" isteklerini AYNI ANDA karşılıyor. Harita şekli **gerçek bir altıgen (petek)** —
ilk denemede dikdörtgen bir X/Y taraması yapılmıştı, bu axial koordinat sisteminde PARALELKENAR
şekli veriyordu (bilinen bir hex-grid özelliği), düzeltmek için köylerin ortalama merkezinden
`yarıçap` bazlı gerçek hex-range taraması yapıldı (cube coordinate range formülü).

**Erzak/Altın'ın kaynağı değişti:** `KoyYoneticisi.Awake()`, eskiden `ErzakYield`'i doğrudan
1-4 rastgele atıyordu — artık (Harita bağlıysa) `HaritaYoneticisi.KoyunErzakToplami(koy)`/
`KoyunAltinToplami(koy)` çağırıp o köyün SAHİP OLDUĞU tüm tile'ların toplamını kullanıyor. Harita
bağlı DEĞİLSE eski rastgele davranış hâlâ çalışır (geriye dönük güvenli geçiş için bilinçli
bırakıldı). Değirmen'in "ErzakYield'i 2 katına çıkarma" mantığı DEĞİŞMEDİ, sadece artık çarptığı
sayı tile toplamından geliyor.

**Görsel çizim (`HexHaritaCizici`, `SekilUretici`):** Hiçbir dış görsel/asset indirilmedi —
hexagon/kare/üçgen/daire şekilleri **kod içinde `Texture2D` üzerine nokta-içinde-mi testiyle
(ray casting) çizilip `Sprite.Create` ile üretiliyor** (`SekilUretici.cs`, static yardımcı sınıf).
`HexHaritaCizici` (mevcut `HaritaArkaplanGorseli` objesine eklendi), oyun başında (`Start()`):
- Her tile için yarı saydam (varsayılan %35), krallık rengine göre boyalı bir hexagon `Image`
  oluşturuyor (`TileleriCiz`).
- Her köy için bir işaret (`Yerlesim_<isim>`, varsayılan kare, ama `YerlesimIkonu` alanına bir
  sprite atanırsa onu kullanıyor — kullanıcı `MedievalFantasyUIPack_Kibyra`'daki `icon40.png`
  kale ikonunu seçti) + üstünde köyün ismi (`TextMeshProUGUI`) oluşturuyor (`YerlesimleriCiz`).
- Tıklama: `YerlesimIsaretiTiklama` (yeni script, ESKİ isim-eşleştirme yöntemi YERİNE doğrudan
  `KoyData` referansı taşıyor) — sol tık `KoyBilgiPaneli`, sağ tık (düşmansa) `DiplomasiBilgiPaneli`.
- **Hover'da toprak sınırı:** Her tile için (görünmez, `SetActive(false)`) bir de çerçeve-hexagon
  (`hexagonCerceveSprite`, içi boş, sadece kenar) oluşturuluyor, `Dictionary<KoyData,
  List<GameObject>>` ile hangi köyün hangi sınır objelerine sahip olduğu tutuluyor.
  `YerlesimIsaretiTiklama`'nın `OnPointerEnter`/`OnPointerExit`'i `HexHaritaCizici.SinirGoster`/
  `SinirGizle`'yi çağırıp o köyün TÜM tile sınırlarını aynı anda açıp kapatıyor.
- **Renk güncelleme:** Bir köy el değiştirdiğinde (`Saldır` başarılı olunca), tile'ların rengi
  OTOMATİK güncellenmiyordu (ilk çizimde bir kere hesaplanıp sabitleniyordu) — bunu çözmek için
  her tile/yerleşim görselinin `Image` referansı bir `Dictionary`'de saklanıyor,
  `HexHaritaCizici.RenkleriGuncelle()` (public) bunları TileRengi'ne göre yeniden boyuyor.
  `DayResolver`, saldırı başarılı olup `HedefKoy.Sahip` değiştiğinde bunu çağırıyor.
- **Ortalanma:** `HexHaritaCizici.EksenselKonum`, koordinatı doğrudan pikselleştirmek yerine
  `HaritaYoneticisi.HaritaMerkezi`'ne göre GÖRELİ pozisyon hesaplıyor — böylece harita (köylerin
  konumu ne olursa olsun) ilk açıldığında ekranın ortasında duruyor.

**Mesafe hesaplama altyapısı:** `HaritaYoneticisi.HexMesafe(a, b)` (cube coordinate mesafe formülü)
ve `KoyMesafesi(koyA, koyB)` — bu oturumda **düşman AI'ının hedef seçmesinde** ("en yakın köyümüze
saldır") kullanılmaya başlandı, bkz. yukarıdaki "Savaş Mekaniği" bölümündeki Düşman AI'ı notu.

**Şu anki Inspector kurulumu (Unity tarafı, hatırlatma):** `HaritaYoneticisi` objesi sahnede,
`KoyYoneticisi.Harita` alanına bağlı. `HexHaritaCizici` `HaritaArkaplanGorseli` objesinde, `Icerik`
alanı `HaritaIcerik`'e bağlı (aynı obje `HaritaKontrol.Icerik` ile de aynı olmalı — pan/zoom'un
tile'ları da etkilemesi için). `HaritaMaskeleyici`/`HaritaIcerik`/`HaritaArkaplanGorseli` üçü de
1300x1300 (ya da benzeri) sabit boyuta, stretch olmayan anchor'a ayarlandı — bu oturumda ÇOK uzun
bir debug sürecinden sonra bulundu (bkz. Bilinen Tuzaklar, birkaç yeni madde eklendi).

**Not:** `HaritaEkrani` objesi eskiden sahnede `m_IsActive: 1` (açık) kayıtlıydı, bu yüzden Play'e
basılır basılmaz (Taht Odası'ndan önce) harita direkt açılıyordu — obje elle kapatıldı (Inspector'dan
checkbox kapatılıp sahne kaydedildi), artık oyun her zaman Taht Odası ile başlıyor, harita sadece
Şahsi Oda'daki Harita objesine tıklanınca açılıyor.

### Terrain (Arazi Türü) Sistemi (bu oturumda eklendi)
Kullanıcının isteği: HOI4/Civ6'daki gibi tile'ların bir "arazi türü" olsun, hem görsel çeşitlilik
katsın hem ekonomiye/savunmaya etki etsin. **Kıyı/Su türü bilinçli olarak ATLANDI** (haritada henüz
gerçek bir deniz/kıyı konsepti yok, ayrı bir iş olarak ertelendi) — 4 tür ile başlandı, **tamamen
rastgele dağılım** (biome/kümeleme yok, test amaçlı).

**Veri modeli:** `TerrainTipi` (yeni, düz enum: `Ova, Orman, Dag, Col`) ve `TerrainVerisi` (yeni,
static class — ScriptableObject/asset DEĞİL, sadece kod içinde sabit bir tablo, "basit sistem"
isteğine uygun): her tür için `Renk`, `ErzakMin/Max`, `AltinMin/Max`, `SavunmaCarpani` döndüren
`Bilgi(tip)` fonksiyonu + `RastgeleTip()`. `HexTileData`'ya `Terrain` alanı eklendi.

**Ekonomiye etkisi:** `HaritaYoneticisi.TileleriOlustur`, artık her tile için önce rastgele bir
`Terrain` seçiyor, sonra o terrain'in **kendi** Erzak/Altın aralığından rastgele değer çekiyor (eski
tek-tip sabit `MinErzakDegeri`/`MaxErzakDegeri` Inspector alanları bu yüzden kaldırıldı).

**Savunmaya etkisi:** `HaritaYoneticisi.KoyunTerrainSavunmaCarpani(koy)` — köyün SAHİP OLDUĞU tüm
tile'ların `SavunmaCarpani`'larının **ortalamasını** alıyor (sadece köyün oturduğu tek tile değil,
tüm toprağı). `KoyYoneticisi.EtkinSavunmaHesapla` artık `Savunma * TerrainCarpani * (1 +
Garnizon/GarnizonKatsayisi)` — Garnizon etkisi (çarpımsal) korunuyor, terrain de ayrıca çarpımsal
ekleniyor. **Köy Bilgi Paneli** artık "Savunma: X (baz: Y)" formatında hem etkin (X) hem ham (Y)
değeri gösteriyor.

**F1/F2/F3 harita görünümleri (HOI4 tarzı):** `HexHaritaCizici`'ye `HaritaGorunumu` enum'u
(`Siyasi, Terrain, Kaynak`) ve `Update()`'te yeni Input System ile F1/F2/F3 dinleme eklendi:
- **F1 (Siyasi):** eski davranış, tile'lar köy sahibinin (`KrallikData.HaritaRengi`) rengine boyanır.
- **F2 (Terrain):** tile'lar `TerrainVerisi.Bilgi(tile.Terrain).Renk`'e göre boyanır.
- **F3 (Kaynak):** F2 ile aynı renkler + her tile'ın üzerinde (Civ6 tarzı) küçük bir ikon (yeşil
  daire, `SekilUretici.DaireSprite`) + arkasında okunurluk için yarı saydam koyu bir kutu + yanında
  o tile'ın Erzak değeri ("+X"). Bu objeler oyun başında bir kere oluşturulup sadece `SetActive`
  ile gösterilip gizleniyor (performans için sürekli yaratıp yok etmiyor). **F3'te köy isim
  etiketleri ve kale ikonları gizleniyor** (sayıları kapatmasınlar diye) — bunun yerine köyün
  oturduğu tek tile'ın çevresinde **her zaman görünen** (hover'a gerek yok) ayrı bir siyah çerçeve
  objesi (`MerkezSinir_...`) beliriyor, "burada bir yerleşke var" bilgisini bu veriyor. Bu, mevcut
  hover-sınır sisteminden (bütün toprağı gösteren `SinirGoster`/`SinirGizle`) tamamen bağımsız, ayrı
  bir obje/dictionary (`merkezSinirGorselleri`) — birbirine karışmıyor.
- Ekranda hangi modda olduğunu gösteren opsiyonel bir metin (`HexHaritaCizici.GorunumMetni`,
  TMP_Text) var — `HaritaEkrani`'nın (`HaritaIcerik`/`HaritaMaskeleyici`'nin DIŞINA, sabit dursun
  diye) bir çocuğu olarak eklenmesi gerekiyor, aksi halde harita pan/zoom'unda kayıp gider.

**Harita açıkken arkadaki stat hover'ları sorunu (bu oturumda bulunup düzeltildi):** `StatTooltip.cs`
Unity'nin normal raycast/EventSystem'ini KULLANMIYOR — her karede fare pozisyonunu doğrudan TMP
metninin link geometrisiyle karşılaştırıyor (`TMP_TextUtilities.FindIntersectingLink`), yani üstünde
görsel olarak ne olursa olsun (harita, CanvasGroup fark etmez) çalışmaya devam ediyor. Bu yüzden ilk
denenen çözüm (`StatlarPaneli`'ne `CanvasGroup` ekleyip `blocksRaycasts` kapatmak) İŞE YARAMADI.
Doğru çözüm: yeni **`HaritaEkraniKontrol.cs`** (yeni, `HaritaEkrani` objesine eklendi) —
`OnEnable`/`OnDisable`'da (obje her `SetActive` olduğunda tetiklenir, X butonunun direkt
`GameObject.SetActive(false)` bağlantısı dahil) `StatTooltip` component'lerini (Erzak/Altın/Nüfus,
`Inspector`'dan elle 3'ü de sürüklenmesi gerekiyor) doğrudan `enabled = false/true` yapıp
devre dışı bırakıyor/açıyor, kapanışta da açık kalmış olabilecek `TooltipUI`'yi gizliyor.

### Uyu Tuşu (Gece / Resolve)
Oyuncu "Uyu" tuşuna basınca döngü sona eriyor ve şunlar oluyor:
- Açık kalmış olabilecek `ManpowerSeciciPaneli` **otomatik kapatılıyor** (oyuncu bir seçim ekranını
  açık bırakıp uyursa, ertesi sabah sahnede asılı kalmasın diye). Köy seçimi artık diyalog kutusunun
  kendisi olduğu için (bkz. Mimari Kararları #20), o ayrıca kapatılmaya gerek duymuyor.
- `GameState.Gun` +1 artıyor.
- **Gelir, gece olaylarından (isyan kontrolü, emirlerin sonuçlanması) ÖNCE uygulanıyor**
  (`DayCycleManager.GelirleriUygula()`, bu oturumda eklendi, bkz. Mimari Kararları #17) — her köyün
  Erzak'ı kendi `ErzakYield`'i kadar artıyor (`KoyYoneticisi.ErzagiGunlukArtir()`, isyandaki köyler
  atlanıyor), genel `ErzakBaseGelir` de (varsa) köylere dağıtılarak uygulanıyor (bkz. Mimari
  Kararları #7), Altın da ekleniyor. Bu sıralamanın amacı: Uyu'ya basmadan hemen önce ekranda
  (StatsUI) görünen tahmini günlük gelir, o gece bir köy yeni isyan etse bile birebir gerçekleşsin.
- **İsyan kontrolü yapılıyor** (`KoyYoneticisi.IsyanKontrolEt`, bkz. "İsyan Mekaniği") — gelirden
  SONRA, emirlerin işlenmesinden ÖNCE çalışıyor, yani aynı gece verilen bir "İsyan Bastır" emri o
  gece hemen işlenip köyü kurtarabiliyor (isyan kontrolü zaten `IsyanHalinde` olan köyleri atladığı
  için çakışma yok).
- Bekleyen emirlerin her biri için zar atılıyor (rastgele + ilgili stat'a bağlı başarı ihtimali).
- Başarı/başarısızlık belirleniyor, stat'lar buna göre güncelleniyor (sonuç mesajları renkli:
  başarı yeşil, başarısızlık kırmızı, garanti tamamlanma sarı — bkz. `DayResolver.cs`). Emrin
  "başladı" haberi artık verilmiyor (gereksizdi), sadece "devam ediyor, X gün kaldı" (renksiz) ve
  tamamlanma mesajı gösteriliyor.
- Her stat/köy değişiminde ekranın sağ altında anlık bir bildirim (+yeşil/-kırmızı, fade in/out)
  beliriyor (`BildirimYoneticisi.cs`) — köy hedefli bir değişimse bildirimde köyün adı da geçiyor.
- Bir sonraki günün taht odası sırası (hangi NPC'lerin geleceği) hem köy bazlı Köylü zarına hem
  sabit hikaye olaylarına göre (örn. 10. gün gelen Ayyaş Adam) belirlenip bir liste halinde saklanıyor.
- Sonuçlar bir sonraki sabah taht odasında bir elçi/ulak karakteri üzerinden oyuncuya aktarılıyor.

### Çok Günlü Emirler
İnşaat gibi bazı emirlerin sonucu birden fazla döngü sürebiliyor. Bunun içinde bir ayrım var:
- **Garanti sonuç** (örn. Değirmen İnşası): süre dolunca kesin tamamlanır, zar atılmaz.
- **Şansa bağlı sonuç** (örn. eski Köy Yağmalama — artık yok; İsyan Bastır şu an tek günlük ve
  `Manpower/(Manpower+Nufus)` formülüyle dinamik şanslı, aynı `ZarAtVeUygula` altyapısını
  kullanıyor): süre dolunca zar atılır.

### UI/Görsel Cila
- Stat değişikliklerini gösteren geçici bildirim kutusu fade in/out ile açılıp kapanıyor.
- Taht Odası'ndan Şahsi Oda'ya geçişte kısa bir siyah ekran (fade to black) geçiş efekti var.
- Diyalog kutusu "Good Pizza Great Pizza" tarzı: konuşma metni kutunun içinde, seçenekler kutunun
  altında, portre yanında. Seçenek butonlarının üzerine gelince maliyet bilgisi hover tooltip'te
  gösteriliyor (`SecenekTooltip.cs`, dinamik — diyaloğa göre değişiyor).
- Dinamik köy seçim butonları (`KoySecimPaneli`) ve Köy Bilgi Paneli'ndeki metinler için:
  **Auto Size (Wrapping kapalı, min font düşük)** kombinasyonu "satır atlamadan küçülsün" isteniyorsa;
  **Wrapping + Content Size Fitter + Layout Group'un Control Child Size'ı** kombinasyonu "satır
  kaydırsın, kutu büyüsün" isteniyorsa kullanılıyor — ikisini birbirine karıştırmamak lazım,
  bkz. Bilinen Tuzaklar.

### Görsel Asset Pipeline (bu oturumda başlandı)
Kullanıcı, oyunun **gerçeğe yakın/detaylı pixel art** bir görünüme kavuşmasını istedi (Undertale
kadar basit değil). Ücretsiz/pay-what-you-want itch.io paketleri araştırılıp indirildi, hepsi
**`Assets/ExternalAssets/`** altında paket başına ayrı klasörde duruyor (karışmasın diye):
- **`CastleInteriorBackgrounds/`** — Taht Odası ve Şahsi Oda arka planları (PNG, düşük çözünürlüklü
  576x324 kaynak, ekrana gerilince bulanıklaşıyordu — **Filter Mode: Point (no filter)** + Import
  ayarlarında **Compression: None** yapılınca netleşti, bkz. Bilinen Tuzaklar). `TahtOdasiPanel` ve
  `SahsiOdaPanel`'e birer `Image` component'i eklenip Anchor **stretch-stretch** yapılarak tam ekran
  kaplatıldı.
- **`PixelArtIconPack_Cainos/`** — Erzak/Altın/Manpower/Nüfus gibi stat'lar için ikonlar (Food,
  Material, Misc/Coin, Equipment kategorili), henüz UI'a uygulanmadı.
- **`MedievalFantasyUIPack_Kibyra/`** — beklenenden farklı çıktı: sadece küçük kare ikonlar +
  İngilizce metin gömülü hazır butonlar (bizim Türkçe dinamik metnimize uygun değil), buton gövdesi
  için KULLANILMIYOR, sadece küçük ikon süslemeleri için kullanılabilir.
- **`PixelButtonsPack_BDragon1727/`** ve **`PixelFramesButtonsPanels_BDragon1727/`** — asıl buton
  ihtiyacını karşılayan paketler: geniş dikdörtgen, **boş/yazısız** (metnimizi üstüne koyabileceğimiz)
  9-slice uyumlu butonlar + kare/yuvarlak süslü çerçeveler (portre/danışman ikonu için). **Henüz
  Sprite Editor'da dilimlenmedi** (Grid By Cell Size 64x32), bkz. Sıradaki Adımlar.
- **`PixelArtFantasyMap_TheSilentMage/`** — Harita sekmesi için yer tutucu görsel. **Dikkat:**
  ücretsiz sürümü sadece ticari-olmayan kullanım + kredi şartlı, şimdilik placeholder olarak kullanmak
  sorun değil ama final oyunda kalıcı kullanmadan önce ücretli sürümü alınmalı ya da değiştirilmeli.

**Not:** İndirme işlemi itch.io'nun "name your own price → $0" akışı taklit edilerek (tarayıcı
otomasyonu ile "Download" butonuna tıklanıp ağ isteklerinden 60 saniyelik süreli imzalı indirme
linki yakalanıp hemen `curl` ile çekilerek) yapıldı — bu linkler kısa süre geçerli, tekrar
indirilmesi gerekirse itch.io sayfasından yeniden "download" akışı başlatılmalı.

---

## 3) MİMARİ KARARLARI

**1. Stat'lar nerede tutuluyor?**
Merkezi bir **`GameState`** sınıfı (düz C# class, `[Serializable]`, ScriptableObject DEĞİL).
`GameState`, **`GameManager`** (MonoBehaviour, Singleton) içinde `public GameState State` olarak
tutuluyor.

**2. NPC'ler (ve danışmanlar) GameObject mi, veri mi?**
Veri olarak tutuluyor: `NPCData` bir ScriptableObject (ID, Isim, Diyalog referansı, Portre).
Danışmanlar için ayrı bir veri tipi açılmadı, `NPCData` yeniden kullanıldı.

**3. Taht odası sırası nerede oluşturuluyor?**
**`DaySequencer`** (düz C# class). `SiradakiListeyiOlustur(state, koyluNpc, askerNpc, ayyasNpc)`
artık `List<SiraGirisi>` döndürüyor (bkz. #10). `Gun == 10` gibi sabit kurallara göre hikaye NPC'leri
ekleniyor.

**4. Bekleyen emirler ve zar atma nerede?**
- **`OrderManager`** (MonoBehaviour): `List<OrderData> BekleyenEmirler` + `HashSet<string>
  KullanilanDanismanlar`.
- **`DayResolver`** (düz C# class): zar atma + sonuç hesaplama mantığının tamamı burada.

**5. Döngü akışı nasıl kuruldu?**
`enum GunAsamasi { TahtOdasi, SahsiOda, Resolve }`, **`DayCycleManager`** (Singleton) bu enum'u
tutuyor, `AsamaDegistir()` ilgili paneli açıp kapatıyor.

**6. Diyalog kutusu Taht Odası ile Şahsi Oda arasında nasıl paylaşılıyor?**
`DiyalogAlani` objesi, `Canvas`'ın altında, panellerle kardeş seviyede duruyor. Görünürlüğü
`DialogueManager.DiyalogKutusuKok` üzerinden elle açılıp kapanıyor. Diyaloğun NPC'den mi
danışmandan mı geldiği `DiyalogBaslat`'ın `danismanDiyalogu` (bool) parametresiyle ayrılıyor.
**`KoySecimPaneli` ve `ManpowerSeciciPaneli` de aynı mantıkla `DiyalogAlani`'nın içinde,
panellerle kardeş seviyede duruyor** — yani `AsamaDegistir()` bunları otomatik kapatmıyor,
`DayCycleManager.UyuyaBas()` elle kapatıyor (bkz. Bilinen Tuzaklar, "panel açık kalıyor" bugı).

**7. Erzak nerede tutuluyor?**
Ayrı bir "genel Erzak" alanı **yok** — `GameState.Erzak` alanı tamamen kaldırıldı. Tek kaynak
`KoyYoneticisi.Instance.Koyler`'daki her köyün kendi `Erzak`'ı. `GameState.StatDegerAl("Erzak")`
→ `KoyYoneticisi.ToplamErzak()` (tüm köylerin toplamı). `GameState.StatDegistir("Erzak", miktar)`
→ `KoyYoneticisi.ErzakDegistir(miktar)` (hem artış hem azalış köy sayısına bölünüp her köye
dağıtılıyor — kalan varsa ilk köylere 1 fazla düşüyor/ekleniyor ki toplam tam tutsun). Bu, "hangi
köyden geldiğini bilmediğimiz" kazanç/kayıplar için geçerli (örn. genel `ErzakBaseGelir`). **Sadakat
için farklı bir karar alındı** — genel + köy ortalaması toplanarak gösteriliyor/kontrol ediliyor
(genel `Sadakat` alanı hâlâ var, `StatDegistir`/`StatDegerAl` içinde köy ortalamasıyla toplanıyor).

**8. Bir diyalog/NPC belirli bir köyü nasıl "temsil ediyor"?**
`DialogueManager`'da `aktifKoy` (nullable `KoyData`) alanı var, `DiyalogBaslat`'ın son parametresiyle
set ediliyor (`DayCycleManager`, `SiraGirisi.IlgiliKoy`'u buraya geçiriyor). `aktifKoy` doluyken
Erzak/Sadakat etkileri (`GuncelDeger`/`DegeriUygula` yardımcı fonksiyonları üzerinden) doğrudan o
köyün kendi alanlarına okunuyor/yazılıyor; Altın/Manpower her zaman kingdom-wide
(`GameManager.State`) üzerinden işliyor. Diyalog metnindeki `{KOY}` yer tutucusu `aktifKoy.Isim`
ile değiştiriliyor (`NodeGoster`). Diyalog bitince `aktifKoy` sıfırlanıyor (bir sonrakine sızmasın diye).

**9. Çok günlü bir emrin (örn. Değirmen) hangi köyü hedeflediği nasıl hatırlanıyor?**
`OrderData`'ya `HedefKoy` (nullable `KoyData`) + `KoySecimiGerekli` (bool) eklendi. Bir
`DialogueChoice`'un `VerilecekEmir`'inde `KoySecimiGerekli` işaretliyse, seçilince **otomatik
`OrderManager.EmirEkle()` çağrılmıyor** — bunun yerine diyalog kutusu açık kalıp normal seçenek
butonları gizleniyor, `KoySecimPaneli` (diyalog kutusunun içine yerleştirilmiş, `Scroll Rect` ile
kaydırılabilir, köy sayısınca dinamik buton oluşturan bir panel) açılıyor. Oyuncu bir köy seçince
`OrderData.KopyalaVeKoyAta(koy)` ile emrin bir **KOPYASI** (şablonun kendisi değil — ScriptableObject
asset'i Play modunda kalıcı bozulmasın diye) `HedefKoy` dolu şekilde oluşturulup `OrderManager`'a
ekleniyor, ardından diyalog kapanıyor. `DayResolver`, `BaseGeliriEtkiler` dalında `HedefKoy` doluysa
(ve `Isim`'i boş değilse — bkz. Bilinen Tuzaklar, Unity boş bir `KoyData` otomatik oluşturabiliyor)
genel `ErzakBaseGelir` yerine doğrudan o köyün `ErzakYield`'ini artırıyor (artık 2 katına çıkararak,
bkz. #Köyler).

**10. Köylü NPC'si hangi köyden geliyor, ne zaman geliyor?**
Artık "en düşük köyü seç" mantığı değil (bu ilk denemeydi, sonra değiştirildi): `DaySequencer` her
gün **her köy için ayrı ayrı** zar atıyor (köyün Erzak'ı 50'den düşükse, 0-50 arası zar; zar köyün
Erzak'ından yüksek çıkarsa o köy kendi Köylü ziyaretini gönderiyor — aynı gün birden fazla köy
tetiklenebilir). Bu bilgi `SiraGirisi` (`Npc` + `IlgiliKoy`, sadece 2 alanlı basit bir "zarf"
sınıfı) ile `DayCycleManager`'a taşınıyor; `DaySequencer.SiradakiListeyiOlustur` artık
`List<NPCData>` değil `List<SiraGirisi>` döndürüyor.

**11. Köy verisi neden ScriptableObject değil?**
`KoyData` düz bir `[Serializable]` C# class (`GameState` ile aynı gerekçe — runtime'da sürekli
değişen veri, ScriptableObject Editor'da eski değerleri hatırlayabiliyor). `KoyYoneticisi`
(MonoBehaviour, Singleton) bunları `List<KoyData> Koyler` olarak tutuyor, Inspector'dan elle
girilip yönetiliyor (asset değil, sahne verisi).

**12. Building slot sistemi nasıl kuruldu?**
`KoyData`'ya `MaxBinaSlotu` (varsayılan 3, köy başına Inspector'dan özelleştirilebilir) ve
`DoluBinaSlotu` eklendi. `OrderData`'ya `BinaSlotuKullanir` (bool) eklendi — bu işaretli bir emrin
köy seçim akışında, `koy.DoluBinaSlotu++` **emir verilir verilmez** (inşaatın tamamlanmasını
beklemeden) yapılıyor (`DialogueManager`, `KoySecimPaneli.KoySec` callback'i içinde). Bu bilinçli
bir tasarım kararı — slotu, inşaat süresince de "rezerve" tutuyor, aynı köye ikinci bir inşaat
emri verilmesini engelliyor.

**13. `KoySecimPaneli` iki farklı filtre modunu nasıl destekliyor?**
`KoySec(callback, emirSablonu)` artık opsiyonel bir `OrderData` parametresi alıyor. Bu şablonun
`BinaSlotuKullanir` VE/VEYA `IsyanliKoyGerekli` bayraklarına bakarak, her köy butonu için ayrı ayrı
`tiklanamaz` durumu hesaplanıyor: `BinaSlotuKullanir` işaretliyse slotu dolu ya da isyanda olan
köyler tıklanamaz ("(Dolu)"/"(Isyan Halinde)"); `IsyanliKoyGerekli` işaretliyse SADECE isyanda
OLMAYAN köyler tıklanamaz ("(Isyan Yok)") — yani mantık tam ters çevriliyor. Bu iki bayrak aynı
şablonda birlikte kullanılmıyor (biri İnşaatçı için, diğeri General'ın İsyan Bastır'ı için).

**14. İsyan mekaniği nasıl kuruldu?**
`KoyData.IsyanHalinde` (bool) — basit bir durum bayrağı, kendi kendine düzelmiyor.
`KoyYoneticisi.IsyanKontrolEt(mesajListesi)` — her gece (Resolve'da, emirler işlenmeden ÖNCE)
çağrılıyor, Sadakat'ı 50'nin altında ve henüz isyanda olmayan her köy için 1-50 zar atıp
Sadakat'tan yüksekse `IsyanHalinde = true` yapıyor. `ErzagiGunlukArtir`/`ToplamErzakYieldi`/
`ToplamAltinYieldi` isyandaki köyleri atlıyor (debuff, köyün asıl Yield alanlarına dokunmadan).
İsyan bastırma emri için `OrderData`'ya üç yeni bayrak eklendi: `IsyanliKoyGerekli` (köy seçim
filtresi, #13), `ManpowerMiktariSorulsun` (köy seçilince `ManpowerSeciciPaneli` açılsın mı),
`IsyanBastirir` (bu emrin sonucu bir isyanı kapatsın mı). `DayResolver.ZarAtVeUygula`, bu üçüncü
bayrağı gördüğünde genel `EtkilenenStat`/`BasariliDegisim` mantığını tamamen atlayıp
`emir.HedefKoy.IsyanHalinde = false` yapıyor ve kendi özel mesajını yazıyor — yani bu emrin
`EmirTuru` alanı sadece kozmetik/log amaçlı, sonuç mantığını etkilemiyor.

**15. Manpower gibi dinamik (oyuncunun seçtiği) bir maliyet nasıl işleniyor?**
`OrderData`'nın normal `MaliyetStat`/`MaliyetMiktar` mekanizması SABİT, şablon-zamanı belirlenen
maliyetler için tasarlanmış (`OrderManager.EmirEkle` içinde otomatik kontrol edip düşüyor). Miktarın
oyuncu tarafından runtime'da seçildiği durumlarda (İsyan Bastır'daki Manpower gibi), maliyet
**elle, `DialogueManager` içinde** (`GameManager.Instance.State.StatDegistir("Manpower", -miktar)`)
düşülüyor VE emrin kopyasında `MaliyetStat`/`MaliyetMiktar` **bilinçli olarak boşaltılıyor**
(`kopya.MaliyetStat = ""; kopya.MaliyetMiktar = 0;`) — aksi halde `EmirEkle`'nin kendi maliyet
kontrolü, zaten düşülmüş kaynağı TEKRAR kontrol edip emri sessizce reddedebiliyor (bkz. Bilinen
Tuzaklar, "isyan bastırma emri hiç işlenmedi" bugı — gerçek sebebi buydu).

**16. Köy Bilgi Paneli ve harita etiketi tıklaması nasıl bağlandı?**
Harita üzerindeki köy isim etiketleri (TextMeshPro objeleri) hâlâ pozisyon olarak elle yerleştirilmiş
durumda (bkz. #Harita), ama artık her birine `KoyEtiketiTiklama.cs` ekleniyor. Bu script,
`IPointerClickHandler`/`IPointerEnterHandler`/`IPointerExitHandler` uyguluyor; **kendi üzerindeki
TMP_Text'in yazdığı ismi okuyup** (`Trim()` + case-insensitive), `KoyYoneticisi.Koyler` içinde
aynı isimli köyü arayıp buluyor — yani harita ile köy verisi arasındaki bağlantı, bir referans
değil, **string eşleştirmesi**. Bu kırılgan bir bağlantı (bkz. Bilinen Tuzaklar) ama Inspector'dan
statik bir referans veremeyeceğimiz için (harita objeleri sahne objesi, `KoyData` runtime verisi)
en basit çözüm bu oldu. Bulunca `KoyBilgiPaneli.Instance.Goster(koy)` çağırıyor. Hover rengi ve
isyan rengi, `OnEnable`'da (harita her açıldığında) ve `OnPointerExit`'te güncelleniyor, sabit bir
"orijinal renk" değil dinamik olarak `koy.IsyanHalinde`'ye bakılarak hesaplanıyor.

**17. İsyan bastırma başarı şansı ve gelir uygulama sırası**
İsyan bastırma başarı şansı **oran (ratio/Tullock contest) formülüyle** dinamik hesaplanıyor:
`BasariSansi = GonderilenManpower / (GonderilenManpower + HedefKoy.Nufus)` (bkz. Mimari Kararları
#19 — bu oturumda `HedefKoy.Savunma` yerine `HedefKoy.Nufus` kullanılacak şekilde değiştirildi).
`OrderData`'ya `GonderilenManpower` (int) eklendi — `MaliyetMiktar` zaten dinamik-maliyet bug'ını
önlemek için sıfırlandığı için (#15), gönderilen miktarı hatırlamak üzere ayrı bir alan gerekti.
`DayResolver.ZarAtVeUygula`, `IsyanBastirir` dalında artık `emir.BasariSansi` yerine bu formülü
kullanıyor (ikisi de 0 ise %50 kabul ediliyor, 0'a bölme hatası önleniyor).

Ayrıca, kullanıcı Uyu'ya basmadan hemen önce StatsUI'de gördüğü tahmini günlük Erzak/Altın gelirinin,
ertesi sabah gerçekleşenle uyuşmadığını fark etti (bir köy o gece isyan edip debuff yiyince fark
oluşuyordu — çünkü eskiden gelir, isyan kontrolünden SONRA uygulanıyordu). Çözüm: gelir uygulama
kodu `DayCycleManager.GelirleriUygula()` adıyla ayrı bir fonksiyona çıkarıldı, `UyuyaBas()` içinde
**isyan kontrolünden ve emirlerin sonuçlanmasından ÖNCE** çağrılıyor — yani ekranda görünen tahmini
gelir, o gecenin olaylarından etkilenmeden aynen uygulanıyor. İlk günün başlangıcı (`Start()`) da
aynı fonksiyonu çağırıyor, davranış değişmedi.

**18. Nüfus sistemi ve büyüme formülü (bu oturumda eklendi)**
Nüfus, Erzak ile birebir aynı mimariyi kullanıyor: `KoyData.Nufus` her köyde, kingdom toplamı
`KoyYoneticisi.ToplamNufus()`, `GameState.StatDegerAl`/`StatDegistir`'deki `"Nufus"` case'i
`KoyYoneticisi`'ye yönlendiriyor (ayrı bir genel alan yok). Günlük artış ise Erzak'tan farklı —
sabit bir `NufusYield` alanı YOK, her gece `KoyYoneticisi.NufusYieldHesapla(koy)` ile dinamik
hesaplanıyor: `(Erzak/Nufus - NufusEsik) * NufusKatsayi`, sonucu `Mathf.RoundToInt` ile tam sayıya
yuvarlanıyor. Bu, hem `NufusuGunlukArtir()` (gerçek uygulama) hem `ToplamNufusYieldi()`/StatsUI/
Köy Bilgi Paneli (tahmini gösterim) tarafından çağrılıyor, yani ekranda görünen değer her zaman
güncel `Erzak`'a göre taze hesaplanıyor. `NufusEsik`/`NufusKatsayi`, `KoyYoneticisi`'nin
Inspector'ında ayarlanabilir alanlar — **dikkat**, bunların C# koddaki varsayılan değerini
değiştirmek, sahnede ZATEN VAR OLAN bir `KoyYoneticisi` objesinin Inspector'daki değerini
GÜNCELLEMEZ (bkz. Bilinen Tuzaklar) — bu oturumda tam olarak bu yüzden bir "bug" gibi görünen
ama aslında güncel olmayan Inspector değerlerinden kaynaklanan bir durum yaşandı, kullanıcı elle
düzeltti.

**19. İsyandaki köyler kingdom toplamlarından tamamen dışlanıyor + isyan bastırmada Manpower kaybı
(bu oturumda eklendi)**
Önceden isyandaki köylerin sadece Yield'leri (Erzak/Altın/Nüfus artışı) debuff'lanıyordu ama
`Erzak`/`Nufus` **stokları** hâlâ kingdom toplamına dahildi — kullanıcı bunun mantıksız olduğunu
belirtti ("isyan eden bir köyün nüfusu/erzağı bize ait olamaz"). Çözüm: `KoyYoneticisi.ToplamErzak()`
ve `ToplamNufus()` da artık `IsyanHalinde` olan köyleri tamamen atlıyor (diğer Yield toplama
fonksiyonlarıyla aynı desen). Köyün kendi `Erzak`/`Nufus` alanları bozulmuyor — isyan bastırılınca
o köyün stoku otomatik geri kingdom toplamına dahil olur.

Ayrıca isyan bastırma başarı formülü `HedefKoy.Savunma` yerine **`HedefKoy.Nufus`** kullanacak
şekilde değiştirildi (bkz. #17) — gerekçe: `Savunma` ileride dış savaş/fetih mekaniği için ayrılsın,
isyan (iç mesele) halkın kendi nüfusuyla orantılı bir direnç göstersin istendi. `Savunma` alanı
kod ve Inspector'da hâlâ duruyor, şimdilik kullanılmıyor (bkz. Sıradaki Adımlar, savaş/diplomasi
beyin fırtınası).

Bununla beraber, `DayResolver.ZarAtVeUygula`'nın `IsyanBastirir` dalına **Manpower kayıp/dönüş**
mantığı eklendi: gönderilen Manpower, savaşın sonucuna göre bir kısmı kaybediliyor, geri kalanı
kingdom'a geri dönüyor — `kayipYuzdesi = bastirmaBasarili ? 0.15f : 0.80f` (kazanırsan %15 kayıp,
kaybedersen %80 kayıp), `geriDonenManpower = GonderilenManpower - kaybedilenManpower` doğrudan
`state.StatDegistir("Manpower", geriDonenManpower)` ile iade ediliyor ve normal +yeşil bildirim
çıkıyor. Elçi mesajına da "(X asker kaybettik, Y asker geri döndü)" eklendi. Bu yüzdeler kod
içinde sabit (`DayResolver.cs` içindeki `0.15f`/`0.80f`), istenirse kolayca değiştirilebilir.

**20. Diyalog seçenekleri neden sabit 2 buton yerine dinamik hale getirildi, `KoySecimPaneli` neden
tamamen silindi? (bu oturumda eklendi)**
General'a üçüncü bir seçenek ("Saldır") eklemek gerekince, `DialogueManager`'ın sabit
`SecenekButon1`/`SecenekButon2` sistemi (max 2 seçenek) yetersiz kaldı. Kullanıcının önerisiyle
`KoySecimPaneli`'nde zaten kullandığımız desen (şablon buton + Scroll Rect + Vertical Layout Group +
Content Size Fitter, bkz. #9) diyalog seçeneklerine de uygulandı: `DialogueManager.NodeGoster()`
artık `aktifNode.Secenekler` listesindeki HER seçenek için `SecenekButonSablonu`'nu
`SecenekIcerik`'in (Content) altına `Instantiate` edip metnini/tıklanabilirliğini/tooltip'ini
kod içinde dolduruyor, 2'den fazla seçenek olunca Scroll Rect otomatik kaydırmaya izin veriyor.
Bu değişiklik yapılınca `KoySecimPaneli`'ye artık gerek kalmadı — `DialogueManager.KoySecimGoster()`
adında yeni bir fonksiyon, aynı dinamik seçenek altyapısını kullanarak köy listesini DOĞRUDAN
diyalog kutusunun içinde gösteriyor (ayrı bir panel açmıyor). `KoySecimPaneli.cs` script dosyası
ve sahnedeki objesi **tamamen silindi** (`DayCycleManager.UyuyaBas()`'taki referansı da kaldırıldı).
`SecenekTooltip.cs` de bu değişiklikle uyumlu hale getirildi: eskiden sabit `SecenekIndex` (0/1)
alıyordu, artık doğrudan `DialogueChoice` referansı alıyor (`Dialog.MaliyetMetniAl(DialogueChoice)`),
çünkü butonlar artık dinamik olduğu için sabit bir index kavramı anlamsızlaştı.

**21. Savaş mekaniğinin veri modeli nasıl kuruldu? (bu oturumda eklendi)**
Yeni **`Krallik.cs`** (düz `enum { Oyuncu, Dusman }`). `KoyData`'ya `Sahip` (`Krallik`, varsayılan
`Oyuncu`) ve `Garnizon` (int, varsayılan 0) eklendi. `KoyYoneticisi.EtkinSavunmaHesapla(koy)` =
`Savunma * (1 + Garnizon/GarnizonKatsayisi)` — kullanıcının isteği: garnizon, köyün doğal
savunmasını ÇARPARAK güçlendirsin (düz toplama değil). `OrderData`'ya `DusmanKoyuGerekli` (köy
seçim filtresi — sadece düşman köyleri, İsyan Bastır/İnşaatçı'daki `IsyanliKoyGerekli`/
`BinaSlotuKullanir`'ın tam tersi mantığı) ve `SaldiriBaslatir` (bool) eklendi. `DayResolver.
ZarAtVeUygula`, `SaldiriBaslatir` dalında `EtkinSavunmaHesapla` ile başarı şansını hesaplıyor,
kazanırsa `koy.Sahip = Oyuncu` + hayatta kalan Manpower `koy.Garnizon`'a yazılıyor (kingdom'a geri
DÖNMÜYOR — kullanıcının kararı, yeni alınan köy savunmasız kalmasın), kaybederse hayatta kalan
Manpower kingdom'a geri dönüyor (isyan bastırmayla aynı %15/%80 kayıp yüzdeleri). Yeni bir düşman
köyü, sahne dosyasına (`SampleScene.unity`, `KoyYoneticisi`'nin `Koyler` listesi) doğrudan YAML
düzenlemesiyle eklendi (Inspector'a gerek kalmadan) — bu, plain-data bir liste girişi olduğu için
güvenli, ama GameObject/component YAPISI değiştiren bir sahne düzenlemesi YAPILMADI (o çok daha
riskli olurdu, bkz. Bilinen Tuzaklar).

**22. Kingdom toplamlarından düşman köyleri nasıl dışlandı?**
`KoyYoneticisi`'ye özel bir `BizeAitDegil(koy)` yardımcı fonksiyonu eklendi: `IsyanHalinde ||
Sahip != Oyuncu`. Tüm toplama/gelir fonksiyonları (`ToplamErzak`, `ToplamNufus`, `ToplamErzakYieldi`,
`ToplamAltinYieldi`, `ToplamNufusYieldi`, `ErzagiGunlukArtir`, `NufusuGunlukArtir`) artık bu tek
fonksiyonu kullanıyor (önceden sadece `IsyanHalinde` kontrol ediliyordu, bkz. #19). `ErzakDegistir`/
`NufusDegistir` (genel dağıtım fonksiyonları) da artık SADECE bize ait köylere dağıtım yapıyor.
`OrtalamaSadakat()` ayrıca sadece `Sahip == Oyuncu` köyleri sayıyor (isyandaki-ama-bize-ait köyler
hâlâ dahil, bu değişmedi — sadece düşman köyleri dışlandı).

**23. Hover tooltip sistemi neden `switch`'ten `Func<string>`'e geçirildi? (bu oturumda eklendi)**
İlk versiyonda `StatTooltip.cs` bir `StatAdi` string'i alıp `switch` içinde ilgili
`KoyYoneticisi` fonksiyonuna yönlendiriyordu. Kullanıcı, bu hover sisteminin ileride NEREDEYSE HER
UI objesine ekleneceğini belirtip her yeni hover için bu switch'e satır eklemenin ölçeklenmeyeceğini
söyledi. Çözüm: `StatTooltip`, artık bir **`Func<string> MetinFonksiyonu`** (parametre almayan,
string döndüren, koddan atanan bir fonksiyon referansı) taşıyor. İlgili bilgiyi zaten bilen script
(`StatsUI`, `KoyYoneticisi` vs.) kendi `Start()`'unda `GetComponent<StatTooltip>().MetinFonksiyonu =
BenimFonksiyonum;` diye atıyor. Bu, hem `StatTooltip.cs`'i bir daha değiştirmeden yeni hover
eklemeyi sağlıyor hem de Inspector'a string yazmayı (typo/eşleşme riski, bkz. Bilinen Tuzaklar
"Askerbasi/General" örneği) ortadan kaldırıyor.

**24. Tooltip kutusu fareyi nasıl takip ediyor? (bu oturumda eklendi, debug'ı uzun sürdü)**
`TooltipUI.Update()`, her karede `RectTransformUtility.ScreenPointToLocalPointInRectangle` ile
(aynı `HaritaKontrol.cs`'teki teknik) fare pozisyonunu `TooltipPanel`'ın parent'ının yerel
koordinatına çeviriyor. Bunun düzgün çalışması için: (1) Unity'nin YENİ Input System paketini
kullandığımız için `Input.mousePosition` DEĞİL, `UnityEngine.InputSystem.Mouse.current.position.
ReadValue()` kullanılmalı (bkz. Bilinen Tuzaklar). (2) `anchoredPosition`'ın referans noktası,
panelin kendi **Anchor/Pivot** ayarına göre değişiyor — panel `Anchor Min/Max`'ı stretch'ten tek
bir köşeye (örn. sol-üst, `0,1`) çevrilince, `anchoredPosition` artık parent'ın MERKEZİNE değil
parent'ın O KÖŞESİNE göre ölçülüyor; kod bunu hesaba katmazsa kutu ekran dışına fırlıyor (bkz.
Bilinen Tuzaklar, bu tam olarak yaşandı). Çözüm: kod, panelin `anchorMin`'ine göre parent'ın
kendi `rect`i içindeki karşılık gelen noktayı (`Mathf.Lerp` ile) hesaplayıp farkı alıyor — hangi
köşeye anchor'lanırsa anchor'lansın doğru çalışıyor.

**25. `Krallik` neden enum'dan ScriptableObject'e (`KrallikData`) dönüştürüldü?**
Bu geçiş bu dosyanın hazırlanmasından SONRA, ayrı bir oturumda yapıldı (bu özet o zaman güncellenmedi,
bu oturumda düzeltildi). `KrallikData`: `Isim`, `HaritaRengi` (Color), `Bayrak` (Sprite). İki asset:
`Vollen_Krallik.asset` (oyuncu) ve `Babbar_Krallik.asset` (düşman). Avantajı: yeni bir krallık eklemek
artık enum'a değer ekleyip HER YERDE o değeri kontrol eden kodu güncellemek değil, sadece yeni bir
asset oluşturup ilgili köylere/`Diplomasiler` listesine eklemek. `KoyYoneticisi.OyuncuKralligi` ve
`KoyData.Sahip` bu tipte, karşılaştırmalar (`==`/`!=`) referans eşitliğiyle çalışıyor.

**26. Diplomasi verisi neden `KrallikData` (ScriptableObject) içine değil ayrı bir listeye kondu?**
`KrallikData` bir ScriptableObject/asset — Bilinen Tuzaklar'da zaten not edildiği gibi, ScriptableObject
alanlarına runtime'da yazılan bir değer (örn. `Diplomasi`) Play modunda değişse bile Editor'da Play'den
çıkınca YENİDEN BAŞLAMIYOR/sıfırlanmıyor güvenilir şekilde (asset kalıcı, sahne verisi değil) — bu
`KoyData`/`GameState`'in neden düz C# class olduğuyla birebir aynı gerekçe (bkz. #11). Çözüm: yeni bir
düz `[Serializable]` class **`DiplomasiVerisi`** (`KrallikData Krallik` referansı + `int Diplomasi` +
`bool SavastaMi`), `KoyYoneticisi.Diplomasiler` listesinde tutuluyor — `Koyler` listesiyle birebir aynı
desen (Inspector'dan elle, her düşman krallık için bir eleman).

**27. Diyalogda "Geri" ve "Boşver" nasıl kuruldu, neden "Boşver" her node'a elle eklenmedi?**
İlk yaklaşım her `DialogueChoice` listesine elle bir "Boşver" seçeneği eklemekti, ama bu her danışman
diyaloğundaki HER menüde tekrar tekrar aynı işi yapmak (ve unutma riski) demekti. Bunun yerine
`DialogueManager.NodeGoster()`/`KoySecimGoster()`, seçenek butonlarını oluşturduktan SONRA
`aktifDanismanDiyalogu` true ise otomatik olarak bir "Bosver" butonu ekliyor (`BosverSecenegiEkleIstenirse`)
— diğer seçeneklerle aynı `SecenekButonSablonu`'ndan üretiliyor, tıklanınca direkt `DiyalogBitir()`.
"Geri" ise bir `Stack<DialogueNode> gecmisNodeler` ile çalışıyor: `SecenekUygula` bir sonraki node'a
geçerken (`SonrakiNodeID` ile) VEYA köy seçim ekranına girerken (`KoySecimGoster` çağrılmadan önce)
mevcut node'u stack'e `Push` ediyor; "Geri" tıklanınca `Pop` edip o node'u yeniden gösteriyor. Köy
seçimine girerken de push edilmesi önemli — aksi halde köy seçiminden "Geri" ile çıkmak, bir önceki
emir menüsünü atlayıp direkt en baştaki karşılamaya dönerdi.

**28. Terrain sistemi neden ScriptableObject/asset değil, sadece kod içinde bir tablo?**
Kullanıcı "basit bir sistem olsun ama çeşitlilik katsın" istedi — 4 terrain türü için ayrı ayrı
asset oluşturup Inspector'dan doldurmak yerine, `TerrainVerisi` static class'ı içinde bir
`switch`/tablo (`Bilgi(TerrainTipi)`) yeterli görüldü (`KrallikData` gibi ScriptableObject'e hiç
gerek yok, çünkü terrain türleri runtime'da hiç değişmiyor, sabit veri). Yeni bir terrain türü
eklemek = enum'a bir değer + `Bilgi()`'ye bir `case` eklemek.

**29. Harita F1/F2/F3 görünüm sistemi neden `HexHaritaCizici`'nin kendi içinde, ayrı bir "görünüm
yöneticisi" script'i açılmadan kuruldu?**
Zaten tüm tile/yerleşim görsellerinin referansları (`tileGorselleri`, `yerlesimGorselleri` vb.)
`HexHaritaCizici`'de duruyor — görünüm değişince hangi rengin/objenin güncelleneceğini bilen tek
yer burası. Ayrı bir script açmak sadece bu referansları tekrar dışarı taşımak (gereksiz
karmaşıklık) olurdu, bu yüzden `HaritaGorunumu` enum'u ve F1/F2/F3 tuş dinleme mantığı doğrudan
`HexHaritaCizici`'ye eklendi.

**30. Düşman AI'ının saldırı gücü neden Garnizon'a bağlandı, Savunma'ya değil?**
İsyan bastırmada `Savunma` yerine `Nufus` kullanma kararıyla aynı mantık (#17/#19): `Savunma`,
bir köyün PASİF direncini temsil ediyor (savunurken kullanılıyor), saldıran tarafın gücü ise her
zaman `Garnizon`/`Manpower` gibi "gönderilen asker" kavramına dayanıyor (oyuncunun Saldır emrinde
de aynı desen). Düşman köyünün Garnizon'u bilinçli olarak **statik** bırakıldı (Inspector'dan elle
girilir, hiç büyümez/taşınmaz) — gerçek bir ekonomi/lojistik AI'ı ayrı, büyük bir iş.

**31. Köylü NPC'sinin isyan/köy sahiplik hatası nasıl bulundu ve düzeltildi?**
`DaySequencer.SiradakiListeyiOlustur`, Köylü zarını **tüm** `Koyler` listesi üzerinde atıyordu —
`Sahip`/`Krallik` mekaniği eklenmeden ÖNCE yazıldığı için hiç köy sahipliği kontrolü yoktu. Kullanıcı
bunu, düşmana ait bir köyün (Tartara) hâlâ "bize erzak lazım" diye şikayetçi göndermesiyle fark etti.
Çözüm: döngüye en başa `if (koy.Sahip != OyuncuKralligi) continue;` eklendi — artık sadece bize ait
köyler Köylü gönderebiliyor. Bu, "yeni bir mülkiyet/sahiplik kavramı eklerken, o kavramdan ÖNCE
yazılmış tüm döngülerin gözden geçirilmesi gerekebileceği" konusunda genel bir hatırlatma: Savaş
mekaniği geldiğinde `KoyYoneticisi`'nin toplama fonksiyonları güncellendi (#22) ama `DaySequencer`
gibi ondan bağımsız çalışan başka bir sistem gözden kaçmıştı.

---

## 4) SCRIPT ENVANTERİ

### Veri sınıfları (düz C#, MonoBehaviour DEĞİL)
- **`GameState.cs`** — `Gun`, `Sadakat`, `Altin`, `Manpower`, `ErzakBaseGelir`, `AltinBaseGelir`.
  `Erzak` ve `Nufus` alanları yok (bkz. Mimari Kararları #7, #18) — ikisi de `KoyYoneticisi`'ye
  yönlendiriliyor. `StatDegerAl`/`StatDegistir`'in `"Nufus"` case'i `KoyYoneticisi.ToplamNufus()`/
  `NufusDegistir()`'e gidiyor. Sadakat için genel+köy ortalamasını topluyor ve `Mathf.Clamp`
  ile 0-100 arasına sıkıştırıyor. `BaseGeliriUygula()`: her yeni günde `KoyYoneticisi.ErzakDegistir(ErzakBaseGelir)`
  + `KoyYoneticisi.NufusuGunlukArtir()` (Nüfus'un kendi dinamik formülü, sabit bir "NufusBaseGelir"
  YOK) + `Altin += AltinBaseGelir`. **`GiderleriUygula()`** *(bu oturumda eklendi)*:
  `Altin -= Manpower*ManpowerMaasiBirimMaliyeti` + `Altin -= ToplamDoluBinaSlotu()*
  BinaBakimBirimMaliyeti` (bkz. "Ekonomi Giderleri"), `DayCycleManager.GelirleriUygula()` içinde
  gelirle aynı anda çağrılıyor.
- **`KrallikData.cs`** — **ScriptableObject** (eskiden düz enum'du, önceki bir oturumda dönüştürüldü,
  bkz. Mimari Kararları #25). `Isim`, `HaritaRengi` (Color), `Bayrak` (Sprite). Asset'ler:
  `Vollen_Krallik.asset` (oyuncu), `Babbar_Krallik.asset` (düşman). `KoyData.Sahip`'in tipi.
- **`DiplomasiVerisi.cs`** *(bu oturumda eklendi)* — Düz `[Serializable]` class, ScriptableObject
  DEĞİL (bkz. Mimari Kararları #26). `KrallikData Krallik`, `int Diplomasi` (varsayılan 60),
  `bool SavastaMi`. `KoyYoneticisi.Diplomasiler` listesinde tutuluyor.
- **`KoyData.cs`** — Bir köyün "kimlik kartı": `Isim`, `Sadakat` (varsayılan 50), `Erzak`
  (varsayılan 20), **`Nufus`** (varsayılan 30, bkz. Mimari Kararları #18 — sabit bir `NufusYield`
  alanı YOK, büyüme dinamik hesaplanıyor), `ErzakYield` (varsayılan 1, ama oyun başında
  `KoyYoneticisi.Awake`'te 1-4 arası rastgele üzerine yazılıyor), `AltinYield` (varsayılan 0),
  **`Savunma`** (varsayılan 20 — isyan bastırmada kullanılmıyor, dış savaşta `EtkinSavunmaHesapla`
  üzerinden kullanılıyor, bkz. Mimari Kararları #21), `MaxBinaSlotu` (varsayılan 3),
  `DoluBinaSlotu` (varsayılan 0), `IsyanHalinde` (bool, varsayılan false), **`Sahip`** (`Krallik`,
  varsayılan `Oyuncu`, bu oturumda eklendi) ve **`Garnizon`** (int, varsayılan 0, bu oturumda
  eklendi — o köyde konuşlanan Manpower). **`MerkezTileKoordinati`** (`Vector3Int`) ve
  **`TileMenzili`** (int, varsayılan 1) — bu oturumda eklendi, hex harita sistemi için (bkz. "Hex
  Harita Sistemi"). ScriptableObject DEĞİL.
- **`HexTileData.cs`** *(bu oturumda eklendi)* — Düz `[Serializable]` class. `Vector3Int Koordinat`,
  `int ErzakDegeri`, `int AltinDegeri`, `KoyData SahipKoy` (nullable). `HaritaYoneticisi.Tileler`
  listesinde tutuluyor.
- **`OrderData.cs`** — Ana constructor aynı (`DanismanTipi, EmirTuru, EtkilenenStat, ...`). Alanlar:
  `KoySecimiGerekli` (bool), `HedefKoy` (nullable `KoyData`), `BinaSlotuKullanir` (bool),
  `IsyanliKoyGerekli` (bool — köy seçim filtresi, sadece isyandaki köyler), `ManpowerMiktariSorulsun`
  (bool — köy seçilince Manpower Slider paneli açılsın mı), `IsyanBastirir` (bool — bu emrin
  başarılı sonucu bir köyün isyanını kapatsın mı), `GonderilenManpower` (int — seçilen Manpower
  miktarı, `MaliyetMiktar` sıfırlandığı için ayrı tutuluyor, bkz. Mimari Kararları #17),
  `DusmanKoyuGerekli` (bool — köy seçim filtresi, sadece düşman köyleri), `SaldiriBaslatir` (bool).
  **`KaynakSecimiGerekli`**, **`KaynakKoy`** (nullable `KoyData`), **`GarnizonEkler`** (bool) — bu
  oturumda eklendi, ordu kaynağı/garnizon aktarma için (bkz. "Ordu Kaynağı ve Mesafeye Bağlı Süre").
  `KopyalaVeKoyAta(KoyData koy)` — tüm alanları
  (yenileri dahil) kopyalayıp `HedefKoy`'u dolduran bir kopya döndürür (şablonu bozmamak için;
  `KaynakKoy` kopyaya dahil edilmiyor, `DialogueManager` sonradan elle atıyor). **Not:** `ToplamSure`
  artık İsyan Bastır/Saldır/Garnizon Gönder için şablondaki sabit değer değil, `DialogueManager`
  tarafından `HaritaYoneticisi.SureHesapla` ile RUNTIME'DA ÜZERİNE YAZILIYOR (bkz. mesafe bölümü).
  **`HedefKrallik`** (`KrallikData`), **`DiplomasiyiArttirir`** (bool), **`DiplomasiMiktari`** (int),
  **`BarisTeklifEder`** (bool) — bu oturumda eklendi, Elçi danışmanı için (bkz. "Diplomasi Mekaniği").
  Bu emirler `KoySecimiGerekli = false` ile **direkt** ekleniyor (Asker Topla gibi), `HedefKrallik`
  diyalog Inspector'ında sabit atanıyor — köy seçimindeki gibi runtime seçim akışına girmiyorlar
  (şu an tek düşman krallık olduğu için gerek yok).
- **`SiraGirisi.cs`** — `NPCData Npc` + `KoyData IlgiliKoy` (nullable). `DaySequencer`'ın
  ürettiği sıradaki bir "ziyaretin" hangi NPC ve (varsa) hangi köyle ilgili olduğunu taşıyan basit
  bir zarf sınıfı.
- **`DevamEdenEmir.cs`** — `OrderData Emir` + `int KalanGun`. Değişmedi.
- **`DaySequencer.cs`** — `SiradakiListeyiOlustur(state, koyluNpc, askerNpc, ayyasNpc)`
  `List<SiraGirisi>` döndürüyor. Köylü "her köy kendi zarını atar" mantığıyla ekleniyor
  (bkz. Mimari Kararları #10). `Gun == 10` kuralı (Ayyaş Adam) değişmedi. **Bu oturumda düzeltilen
  bug (bkz. Mimari Kararları #31):** Köylü döngüsü artık en başta `koy.Sahip != OyuncuKralligi` olan
  köyleri atlıyor — eskiden sahiplik hiç kontrol edilmiyordu, düşman köyler bile Köylü gönderebiliyordu.
- **`DayResolver.cs`** — Tek günlük emirler anında, çok günlüler `devamEdenler`'e. `ZarAtVeUygula`
  içinde **`emir.GarnizonEkler` kontrolü EN BAŞTA** *(bu oturumda eklendi)* — garanti (şansa bağlı
  DEĞİL) olarak `GonderilenManpower`'ı `HedefKoy.Garnizon`'a ekliyor; **çok-günlü tamamlanma yolunda
  da** (`SonucMesajlariniOlustur`'daki `else if` zinciri) AYRICA ele alınması gerekiyor çünkü o yol
  `ZarAtVeUygula`'yı çağırmıyor (sadece `SonucSansaBagli` ise çağırıyor). Ardından
  **`emir.IsyanBastirir` kontrolü** — işaretliyse genel stat mantığını atlayıp,
  başarı şansını `GonderilenManpower/(GonderilenManpower+HedefKoy.Nufus)` formülüyle hesaplayıp
  (bkz. Mimari Kararları #17, #19) `emir.HedefKoy.IsyanHalinde`'yi ayarlayıp kendi mesajını yazıyor
  (bkz. Mimari Kararları #14). **Manpower kayıp/dönüş** artık **`AskeriGeriGonder(state, emir,
  miktar)`** yardımcı fonksiyonunu kullanıyor *(bu oturumda eklendi)* — `emir.KaynakKoy` doluysa
  oraya (`Garnizon`'a), boşsa `GameState.Manpower`'a geri dönüyor (bkz. "Ordu Kaynağı ve Mesafeye
  Bağlı Süre"). **`emir.SaldiriBaslatir` kontrolü** — `KoyYoneticisi.EtkinSavunmaHesapla(HedefKoy)`'a
  karşı `GonderilenManpower` ile oran/Tullock formülü, kazanırsa `HedefKoy.Sahip = Oyuncu` +
  hayatta kalan Manpower `HedefKoy.Garnizon`'a yazılıyor (kingdom'a DÖNMÜYOR) + **`HexHaritaCizici.
  Instance.RenkleriGuncelle()` çağrılıyor** *(bu oturumda eklendi)* ki köyün hex tile'ları/işareti
  haritada anında yeni sahibinin rengine dönsün, kaybedersen `AskeriGeriGonder` ile hayatta kalan
  Manpower geri dönüyor (aynı %15/%80 yüzdeleri).
  `BaseGeliriEtkiler` dalı `HedefKoy` doluysa (ve `Isim`'i boş değilse) o köyün `ErzakYield`'ini
  **2 katına çıkarıyor** (eski davranış: sabit +miktar ekliyordu). Her stat değişiminde
  `BildirimYoneticisi.Bildirim(...)` çağrılıyor. Sonuç mesajları renkli: şansa bağlı başarı yeşil,
  başarısızlık kırmızı, garanti tamamlanma sarı. **`ZarAtVeUygula`'nın EN BAŞINA** (bu oturumda
  eklendi) `emir.DiplomasiyiArttirir`/`emir.BarisTeklifEder` kontrolleri geldi (Elçi emirleri) —
  ikisi de tek günlük, `HaritaYoneticisi.SureHesapla`'ya girmiyor. `DiplomasiyiArttirir` direkt
  `KoyYoneticisi.DiplomasiDegistir` çağırıyor; `BarisTeklifEder` başarı şansını `Diplomasi/100`
  olarak hesaplayıp başarılıysa `KoyYoneticisi.BarisYap` çağırıyor.
- **`NPCData.cs`** — ScriptableObject. `ID`, `Isim`, `DialogueData Diyalog`, `Sprite Portre`.
  Danışmanlar için de kullanılıyor (`General_NPC.asset`, `Insaatci_NPC.asset`).
- **`DialogueData.cs`** — ScriptableObject. `DialogueChoice`: `SecenekMetni`, `SonrakiNodeID`,
  `List<StatEtkisi> StatEtkileri` (her biri `StatAdi`+`Miktar`), `OrderData VerilecekEmir`.
- **`SekilUretici.cs`** *(bu oturumda eklendi)* — MonoBehaviour DEĞİL, static yardımcı sınıf, hiç
  sahne objesi yok. `HexagonSprite()`, `HexagonCerceveSprite()` (içi boş, sadece kenar), `KareSprite()`,
  `UcgenSprite()`, `DaireSprite()` — hepsi `Texture2D` üzerine nokta-çokgen-içinde-mi testiyle
  (ray casting algoritması) piksel piksel çizip `Sprite.Create` ile dönüyor. Hiçbir dış görsel/asset
  gerektirmiyor, tamamen kod üretimli (bkz. "Hex Harita Sistemi").
- **`TerrainTipi.cs`** *(bu oturumda eklendi)* — Düz enum: `Ova, Orman, Dag, Col`. Kıyı/Su bilinçli
  olarak eklenmedi (bkz. "Terrain Sistemi").
- **`TerrainVerisi.cs`** *(bu oturumda eklendi)* — MonoBehaviour DEĞİL, static class (bkz. Mimari
  Kararları #28). `TerrainBilgisi` (struct: `Renk`, `ErzakMin/Max`, `AltinMin/Max`, `SavunmaCarpani`),
  `Bilgi(TerrainTipi)` ve `RastgeleTip()`.

### MonoBehaviour'lar (sahnede bir GameObject'e eklenmiş script'ler)
- **`GameManager.cs`** — Singleton, `public GameState State`. Sadece veri kutusu tutucu.
- **`KoyYoneticisi.cs`** — Singleton, `public List<KoyData> Koyler`. `Awake()`'te artık her köyün
  `ErzakYield`'ini 1-4 arası rastgele belirliyor (yeni oyun başlangıcı, bkz. #Köyler). `BizeAitDegil(koy)`
  (private, bu oturumda eklendi, bkz. Mimari Kararları #22) = `IsyanHalinde || Sahip != Oyuncu` —
  tüm toplama fonksiyonları bunu kullanıyor: `ToplamErzak()`, `ErzakDegistir(miktar)`,
  `ErzagiGunlukArtir()`, `ToplamErzakYieldi()`, `ToplamAltinYieldi()`, `ToplamNufus()`,
  `NufusDegistir(miktar)`, `NufusuGunlukArtir()`, `ToplamNufusYieldi()` (hepsi düşman köylerini ve
  isyandaki köyleri atlıyor), `OrtalamaSadakat()` (sadece `Sahip == Oyuncu` filtreli).
  `IsyanKontrolEt(mesajListesi)` (gece isyan zarı, bkz. Mimari Kararları #14 — artık `Sahip !=
  Oyuncu` olan köyleri de atlıyor). `NufusYieldHesapla(koy)` — Nüfus büyüme formülü, bkz. Mimari
  Kararları #18. `NufusEsik` (varsayılan 1) ve `NufusKatsayi` (varsayılan 10), Inspector'dan
  ayarlanabilir. `EtkinSavunmaHesapla(koy)` = `Savunma * (1 + Garnizon/GarnizonKatsayisi)`,
  `GarnizonKatsayisi` (varsayılan 10) Inspector'dan ayarlanabilir, bkz. Mimari Kararları #21.
  `ErzakDagilimMetni()` — bize ait her köyün Erzak'ını "Köy: X" formatında satır satır döken bir
  string döndürüyor, Erzak hover tooltip'i tarafından kullanılıyor. **`Diplomasiler`** (bu oturumda
  eklendi, `List<DiplomasiVerisi>`) ve **`DiplomasiEsikDegeri`** (varsayılan 40, Inspector'dan
  ayarlanabilir): `DiplomasiVerisiBul(krallik)` (private, referansla arıyor), `DiplomasiDegerAl(krallik)`,
  `SavastaMi(krallik)`, `DiplomasiDegistir(krallik, miktar)` (0-100 clamp), `DiplomasiKontrolEt(mesajListesi)`
  (gece zarı, `IsyanKontrolEt` ile birebir aynı formül/desen, bkz. Mimari Kararları #26).
  **`Harita`** (bu oturumda eklendi, `public HaritaYoneticisi`) — Inspector'dan bağlanıyor, `Awake()`
  bağlıysa `Harita.TileleriOlustur(Koyler)` çağırıp `ErzakYield`/`AltinYield`'i tile toplamından
  alıyor (bağlı değilse eski rastgele davranış çalışmaya devam ediyor, bkz. "Hex Harita Sistemi").
  **`GarnizonluKoyler()`** (bu oturumda eklendi) — `Sahip == Oyuncu && Garnizon > 0` filtreli liste,
  ordu kaynağı seçim ekranında kullanılıyor. **`ToplamDoluBinaSlotu()`** (bu oturumda eklendi) —
  bina bakım giderini hesaplamak için (bkz. "Ekonomi Giderleri"). **`KoyunTerrainSavunmaCarpani(koy)`**
  ve **`DusmanSaldiriIhtimali`**/**`DusmanSaldirilariniKontrolEt`**/**`EnYakinBizimKoy`** ve
  **`BarisYap(krallik)`** *(bu oturumda eklendi)* — sırasıyla Terrain sistemi ve Diplomasi/Düşman
  AI'ı için, bkz. "Terrain Sistemi", "Savaş Mekaniği" (Düşman AI'ı) ve "Diplomasi Mekaniği".
  `EtkinSavunmaHesapla` artık terrain çarpanını da içeriyor (bkz. Mimari Kararları #28).
- **`OrderManager.cs`** — `EmirEkle`, `DanismanKullanildiMi`, `YeniDongueBasla`. Mantık değişmedi
  ama **dikkat:** `EmirEkle` kendi `MaliyetStat`/`MaliyetMiktar` kontrolünü de yapıyor — dinamik
  maliyetli emirlerde bu alanların boşaltılması gerekiyor (bkz. Mimari Kararları #15).
- **`DialogueManager.cs`** — Büyük bir revizyon geçirdi (bkz. Mimari Kararları #20, #27).
  `NpcIsimText`, `NpcSozuText`, `PortreImage`, `Orders`, `DiyalogKutusuKok`. Sabit `SecenekButon1/2`
  alanları **kaldırıldı**, yerine `SecenekButonSablonu` (GameObject, şablon) + `SecenekIcerik`
  (Transform, Scroll Rect'in Content'i) geldi. `NodeGoster()`: `{KOY}` yer tutucusunu değiştirdikten
  sonra, önceki seçenek butonlarını `Destroy` edip `aktifNode.Secenekler`'deki HER seçenek için
  `SecenekButonSablonu`'nu `Instantiate` ediyor, metin/tıklanabilirlik/tooltip'i kod içinde
  dolduruyor. `KoySecimGoster(sablon, callback)` — aynı dinamik buton altyapısını
  kullanarak köyleri listeliyor, `sablon`'un `BinaSlotuKullanir`/`IsyanliKoyGerekli` bayraklarına
  göre ilgili köyleri "(Dolu)"/"(Isyan Yok)" etiketiyle DEVRE DIŞI (ama listede) gösteriyor,
  `DusmanKoyuGerekli` bayrağına göre ise düşman-olmayan/düşman köyleri LİSTEDEN TAMAMEN ÇIKARIYOR
  (`continue`, buton hiç oluşturulmuyor) — iki filtre türü farklı davranıyor, bkz. Mimari
  Kararları #20-21. `aktifKoy` (nullable `KoyData`), `GuncelDeger`/`DegeriUygula`: Erzak/Sadakat'ı
  `aktifKoy` doluysa doğrudan köye, değilse `GameManager.State` üzerinden kingdom-wide uyguluyor.
  `SecenekUygula`: `VerilecekEmir.KoySecimiGerekli` true ise `KoySecimGoster(sablon, callback)`
  çağırıyor. Callback içinde: `BinaSlotuKullanir` ise slot +1; `ManpowerMiktariSorulsun` ise
  `ManpowerSeciciPaneli.Sor(...)` açıp seçilen miktarı elle Manpower'dan düşüyor, kopyanın
  `MaliyetStat`/`MaliyetMiktar`'ını sıfırlayıp öyle `EmirEkle` çağırıyor (bkz. Mimari Kararları #15).
  **`GeriButonu`** (bu oturumda eklendi, `public Button`) + **`gecmisNodeler`** (`Stack<DialogueNode>`,
  private): her node geçişinde/köy seçimine girerken mevcut node stack'e push ediliyor, "Geri"
  tıklanınca pop edilip o node yeniden gösteriliyor, `GeriButonunuGuncelle()` stack boşsa butonu
  gizliyor. **`BosverSecenegiEkleIstenirse()`** (bu oturumda eklendi): `aktifDanismanDiyalogu` true
  ise seçenek listesinin sonuna otomatik bir "Bosver" butonu ekliyor (diğer seçeneklerle aynı
  şablondan), tıklanınca hiçbir etki olmadan `DiyalogBitir()` çağırıyor — hem `NodeGoster()` hem
  `KoySecimGoster()` sonunda çağrılıyor (bkz. Mimari Kararları #27).
  **`KaynakSecimGoster(sablon, hedefKoy, callback)`** *(bu oturumda eklendi)* — köy seçiminden SONRA,
  `sablon.KaynakSecimiGerekli` ise devreye giriyor: "Genel Yedek Kuvvet" (`KaynakKoy=null`) + hedef
  köy hariç `KoyYoneticisi.GarnizonluKoyler()` listeleniyor, aynı dinamik buton altyapısını kullanıyor.
  **`ManpowerAdimi(sablon, hedefKoy, kaynakKoy)`** *(bu oturumda eklendi, eski inline kod buraya
  çıkarıldı)* — `ManpowerSeciciPaneli.Sor(mevcutManpower, callback)` artık maksimumu parametre
  olarak alıyor (kaynak null ise `GameState.Manpower`, değilse `kaynakKoy.Garnizon`), seçilen miktar
  doğru yerden düşülüyor, `kopya.ToplamSure = HaritaYoneticisi.SureHesapla(kaynakKoy, hedefKoy)` ile
  mesafeye göre süre atanıyor (bkz. "Ordu Kaynağı ve Mesafeye Bağlı Süre").
  **`SecenekKarsilanabilirMi`** *(bu oturumda genişletildi)* — Erzak/Altın maliyet kontrolünün
  yanına, `emir.BarisTeklifEder` işaretliyse VE `!KoyYoneticisi.SavastaMi(emir.HedefKrallik)` ise
  butonu kilitleyen bir kontrol eklendi (savaşta değilken "Barış Teklif Et" gri/tıklanamaz).
- **`SecenekTooltip.cs`** — Diyalog seçenek butonlarına eklenen hover script'i. Artık sabit
  `SecenekIndex` (0/1) yerine doğrudan **`DialogueChoice Secenek`** referansı alıyor (bu oturumda
  değiştirildi, butonlar dinamik olduğu için index anlamsızlaştı), `Dialog.MaliyetMetniAl(Secenek)`
  çağırıp `TooltipUI.Goster`/`Gizle` ile gösterir/gizler.
- **`KoySecimPaneli.cs`** — **Bu oturumda tamamen silindi** (script + sahne objesi). Görevini artık
  `DialogueManager.KoySecimGoster()` üstleniyor, bkz. Mimari Kararları #20.
- **`ManpowerSeciciPaneli.cs`** — Singleton. `Panel`, `Slider MiktarSlider`, `TMP_Text
  MevcutManpowerText`, `TMP_Text SeciliMiktarText`. **`Sor(int mevcutManpower, Action<int> callback)`**
  *(bu oturumda değişti — eskiden parametresizdi, hep `GameState.Manpower` okurdu)*: artık maksimum
  DIŞARIDAN veriliyor (Genel Yedek Kuvvet ya da bir köyün Garnizonu olabilir, bkz. "Ordu Kaynağı"),
  slider'ın min/max'ını buna göre ayarlayıp paneli açıyor, `Slider.onValueChanged` ile seçili miktar
  metnini canlı güncelliyor. `GonderTiklandi()`: slider değerini `Mathf.RoundToInt` ile tam sayıya
  çevirip (verilen maksimuma clamp'leyerek) callback'i çağırıyor.
- **`KoyBilgiPaneli.cs`** — Singleton. `Panel`, `IsimText`, `DurumText` (isyan varsa kırmızı
  "ISYAN HALINDE"), `SadakatText`, `ErzakText`, `ErzakYieldText`, `AltinYieldText` (ikisi de
  `YieldMetni` yardımcı fonksiyonuyla renkleniyor: +yeşil/-kırmızı/0-beyaz, isyanda gri), `SlotText`
  ("1/3" formatı), `SavunmaText`, `NufusText` ("Nufus: X <sup>+Y</sup>" formatında, Y
  `KoyYoneticisi.NufusYieldHesapla(koy)` ile canlı hesaplanıyor). **`GarnizonText`** *(bu oturumda
  eklendi)*. **`SavunmaText` artık "Savunma: X (baz: Y)" formatında** *(bu oturumda değişti)* — X,
  `KoyYoneticisi.EtkinSavunmaHesapla(koy)` (terrain çarpanı + Garnizon dahil gerçek/etkin değer), Y
  ham `koy.Savunma`. `Goster(koy)` artık `koy.Sahip == Oyuncu` mu diye bakıyor (bkz. Mimari Kararları #21):
  değilse Sadakat/Erzak/Nüfus/Yield/Slot satırlarının GameObject'lerini `SetActive(false)` ile
  gizleyip sadece İsim + Savunma + Garnizon gösteriyor; bize aitse eskisi gibi her şey görünüyor.
- **`KoyEtiketiTiklama.cs`** — **Bu oturumda ARTIK KULLANILMIYOR** (muhtemelen ölü kod, silinip
  silinmeyeceği doğrulanmadı) — elle yerleştirilmiş statik metin etiketleri kaldırıldı, görevini
  `YerlesimIsaretiTiklama.cs` + `HexHaritaCizici.cs` (kod üretimli hex harita) üstlendi. Eski
  davranışı (artık geçersiz): isim eşleştirmesiyle `KoyData` bulup sol/sağ tıkta
  `KoyBilgiPaneli`/`DiplomasiBilgiPaneli` açıyordu.
- **`HaritaYoneticisi.cs`** *(bu oturumda eklendi)* — Singleton, `KoyYoneticisi`'nin kardeşi.
  `List<HexTileData> Tileler`, `Vector3Int HaritaMerkezi` (köylerin ortalama konumu).
  `TileleriOlustur(koyler)`: haritanın (köylerin ortalamasına göre `SinirPayi` kadar geniş) TAMAMINI
  gerçek hex-range (petek) taramasıyla dolduruyor, her tile'ı `EtkinMesafe = HexMesafe(tile,
  koy.Merkez) - koy.TileMenzili` en küçük olan köye atıyor (ağırlıklı Voronoi, bkz. "Hex Harita
  Sistemi"). `KoyunErzakToplami`/`KoyunAltinToplami(koy)` — bir köyün sahip olduğu tile'ların
  toplamı. `TileRengi(tile)` — `tile.SahipKoy.Sahip.HaritaRengi` (sahipsizse gri). `HexMesafe(a,b)`
  (cube coordinate formülü) + `KoyMesafesi(koyA,koyB)` — mesafe hesaplama altyapısı, düşman AI'ı
  HENÜZ bunu kullanmıyor (kasıtlı olarak ertelendi). `SureHesapla(kaynakKoy,hedefKoy)` — kaynak null
  ise 1 gün (Genel Yedek Kuvvet, konumsuz), değilse `mesafe/OrduHizi` (varsayılan 3), en az 1 gün.
- **`HexHaritaCizici.cs`** *(bu oturumda eklendi)* — Haritayı TAMAMEN KOD İÇİNDE çizen script,
  `HaritaArkaplanGorseli` objesinde duruyor. `Icerik` (aynı `HaritaKontrol.Icerik`'e bağlı olmalı ki
  pan/zoom tile'ları da etkilesin). `Start()`'ta `EskiCizimleriTemizle()` (Play modunda script
  yeniden çalışırsa eskiler birikmesin diye, isimleri `Tile_`/`Yerlesim_` ile başlayanları siliyor)
  + `TileleriCiz()` + `YerlesimleriCiz()`. Her oluşturulan obje **`SabitCapaliRectOlustur`** ile
  açıkça sabit/merkez anchor'a ayarlanıyor (bkz. Bilinen Tuzaklar — `AddComponent` ile taze
  RectTransform'un varsayılan anchor'ı güvenilmez çıktı). `TileleriCiz`: her `HexTileData` için
  yarı saydam (varsayılan `TileSaydamligi=0.35`) hexagon `Image` + (sahipliyse) gizli bir çerçeve-
  hexagon (`Sinir_...`, hover'da açılacak) oluşturuyor, ikisinin de `Image` referansı
  `Dictionary`'lerde saklanıyor (`tileGorselleri`, `koyTileSinirlari`) ki sonradan güncellenebilsin.
  `YerlesimleriCiz`: her köy için `YerlesimIkonu` (Inspector'dan atanan bir sprite, örn. bir kale
  ikonu) varsa onu, yoksa düz kare kullanıp bir işaret + üstünde isim (`TextMeshProUGUI`) oluşturuyor,
  `YerlesimIsaretiTiklama` component'i ekliyor. **`SinirGoster(koy)`/`SinirGizle(koy)`** (public) —
  bir köyün TÜM tile sınır objelerini aynı anda açıp kapatıyor. **`RenkleriGuncelle()`** (public) —
  bir köy el değiştirdiğinde (`DayResolver` çağırıyor) tüm tile/yerleşim renklerini
  `HaritaYoneticisi.TileRengi`'ye göre yeniden hesaplayıp uyguluyor (ilk çizimde bir kere hesaplanıp
  SABİTLENDİĞİ için, elle güncellenmezse köy fethedilince renk değişmiyordu). **Bu oturumda eklendi:**
  `HaritaGorunumu` enum'u (`Siyasi/Terrain/Kaynak`) + `Update()`'te F1/F2/F3 dinleme,
  `GorunumMetni` (opsiyonel TMP_Text, F1/F2/F3 metnini gösterir), F3'te tile başına ikon+sayı
  (`kaynakGorselleri`), köy isim etiketlerinin F3'te gizlenmesi (`yerlesimIsimGorselleri`) ve
  köyün oturduğu tek tile için F3'te her zaman görünen ayrı bir çerçeve (`merkezSinirGorselleri`,
  hover-sınır sisteminden bağımsız) — detaylar için "Terrain (Arazi Türü) Sistemi" bölümüne bak.
- **`YerlesimIsaretiTiklama.cs`** *(bu oturumda eklendi)* — `KoyEtiketiTiklama`'nın YERİNİ ALDI,
  ama isim eşleştirmesi YOK — doğrudan `public KoyData Koy` referansı taşıyor (çok daha sağlam).
  `IPointerClickHandler`: sol tık `KoyBilgiPaneli.Goster(Koy)`, sağ tık (düşmansa) `DiplomasiBilgiPaneli.
  Goster(Koy.Sahip)`. `IPointerEnterHandler`/`IPointerExitHandler`: `HexHaritaCizici.Instance.
  SinirGoster`/`SinirGizle(Koy)` çağırıp o köyün toprak sınırını gösterip gizliyor.
- **`HaritaKontrol.cs`** — Harita içeriğinin (`Icerik`, RectTransform) sürüklenmesini/
  yakınlaştırılmasını yönetir. `IBeginDragHandler`/`IDragHandler`: `RectTransformUtility.
  ScreenPointToLocalPointInRectangle` ile Canvas modundan/ölçeğinden bağımsız, imlecin gerçek yerel
  konumunu hesaplayıp fark alır. `IScrollHandler`: zoom (fare tekerleği). `minZoom`, `Awake`'te
  viewport/içerik oranından otomatik hesaplanıyor. `SinirlaKonum()`: sürükleme sonrası haritanın
  kenarları ekran dışına taşmayacak şekilde `anchoredPosition`'ı kelepçeliyor.
- **`GunUI.cs`** — Ekranın üstünde "Gun X" gösteriyor. Değişmedi.
- **`BildirimYoneticisi.cs`** — Singleton. `Bildirim(statAdi, miktar)`: şablonu çoğaltıp fade in
  (`CanvasGroup`) → bekle → fade out → `Destroy`. Değişmedi.
- **`DayCycleManager.cs`** — Singleton, state machine'in kalbi. `gunlukSira` `List<SiraGirisi>`.
  `GelirleriUygula()`: `BaseGeliriUygula()` + `KoyYoneticisi.ErzagiGunlukArtir()` + Altın base
  geliri — hem `Start()`'ta (ilk gün) hem `UyuyaBas()`'ta (isyan kontrolünden ÖNCE) çağrılıyor
  (bkz. Mimari Kararları #17). `YeniGuneBasla()`: artık sadece `DaySequencer` ile sıra oluşturup
  göstermekle ilgileniyor (gelir uygulamıyor). `SiradakiNpcyiGoster()`: `Dialog.DiyalogBaslat(...)`
  ilgili köyü de iletiyor. **`UyuyaBas()`**: en başta `ManpowerSeciciPaneli`'ni açıksa kapatıyor
  (`KoySecimPaneli` referansı bu oturumda kaldırıldı, silindiği için — bkz. Mimari Kararları #20),
  `Gun`'ı +1 artırıp Resolve'a geçiyor, `GelirleriUygula()` çağırıyor (bu artık `GameState.
  GiderleriUygula()`'yı da çağırıyor, bkz. "Ekonomi Giderleri"), sonra **`KoyYoneticisi.
  IsyanKontrolEt(...)` emirlerden ÖNCE çağrılıyor**, sonra `DayResolver` çalışıyor.
- **`DanismanCagir.cs`** — Kapıdan açılan danışman listesindeki butonlara bağlı. `GeneralCagir()`,
  `InsaatciCagir()`, **`ElciCagir()`** *(bu oturumda eklendi, üçüncü danışman "Elçi")*: ilgili
  danışmanın diyaloğunu `danismanDiyalogu=true` ile başlatıyor. Bu oturumda danışman listesi paneline
  Vertical Layout Group + Content Size Fitter eklendi ki yeni butonlar (Elçi gibi) elle
  boyutlandırma/hizalama gerekmeden otomatik sıralansın.
- **`DanismanButon.cs`** — Danışman kullanılınca ilgili butonun görsel kilitlenmesi
  (`button.interactable = !Orders.DanismanKullanildiMi(DanismanTipi)`, her frame `Update()`'te
  kontrol ediliyor). **`DanismanTipi` alanı, o danışmanın `OrderData`'larındaki `DanismanTipi`
  string'iyle BİREBİR aynı olmalı** — aksi halde kilit sessizce çalışmaz (bkz. Bilinen Tuzaklar,
  bu oturumda "Askerbasi" vs "General" mismatch'i canlı olarak yaşandı ve düzeltildi).
- **`StatsUI.cs`** — Sol üstteki canlı stat göstergesi. **Bu oturumda tek bir `StatlarText` yerine
  5 ayrı `TMP_Text`'e bölündü** (`ErzakText`, `AltinText`, `ManpowerText`, `NufusText`,
  `SadakatText`) — sebep: hover sisteminin hangi stat satırının üzerinde olduğunu ayırt edebilmesi
  (bkz. "Hover Bilgi Sistemi", Mimari Kararları #23). Erzak `KoyYoneticisi.ToplamErzak()`'tan,
  tahmini günlük gelir `ToplamErzakYieldi()`/`ToplamAltinYieldi()`/`ToplamNufusYieldi()`'den okunuyor
  (isyandaki VE düşman köyleri otomatik 0/hariç sayılıyor), Sadakat genel+köy ortalaması, Nüfus
  `ToplamNufus()`'tan. Üst indis gelir göstergeleri (`GelirMetni`) +yeşil/-kırmızı/0-beyaz renkli.
  **`Start()`** *(bu oturumda eklendi)*: `ErzakText`'in üzerindeki `StatTooltip` component'ine
  `KoyYoneticisi.Instance.ErzakDagilimMetni`'i `MetinFonksiyonu` olarak atıyor. **Altın tahmini
  geliri artık giderleri de düşüyor** *(bu oturumda eklendi)* — `AltinBaseGelir + ToplamAltinYieldi()
  - askerMaasi - binaBakimGideri`, yoksa Uyu'ya basmadan önceki tahminle gerçekleşen uyuşmazdı
  (Mimari Kararları #17'deki aynı prensip, yeni giderlere de uygulandı).
- **`StatTooltip.cs`** *(yeni, bu oturumda eklendi)* — Genel/ölçeklenebilir hover bilgi script'i
  (bkz. Mimari Kararları #23). Sabit bir stat listesi/switch YOK, sadece koddan atanan bir
  **`Func<string> MetinFonksiyonu`** taşıyor. `OnPointerEnter`'da bu fonksiyonu çağırıp sonucu
  `TooltipUI.Goster`'a veriyor. Yeni bir hover eklemek için bu dosyayı değiştirmeye gerek yok.
- **`TooltipUI.cs`** — Hover bilgi kutusu sistemi, `ButtonTooltip`/`SecenekTooltip`/`StatTooltip`
  tarafından paylaşılıyor. **Bu oturumda fareyi takip edecek şekilde genişletildi** (bkz. Mimari
  Kararları #24): `Update()`'te `UnityEngine.InputSystem.Mouse.current.position` (bu proje YENİ
  Input System kullanıyor, eski `Input.mousePosition` DEĞİL) + `RectTransformUtility.
  ScreenPointToLocalPointInRectangle` ile panel pozisyonu her karede güncelleniyor, panelin kendi
  `anchorMin`'ine göre parent'ın rect'i içindeki karşılık gelen noktayı hesaplayıp farkı alıyor
  (hangi köşeye anchor'lanırsa anchor'lansın doğru çalışsın diye). `FareyeGoreOfset` (Inspector'dan
  ayarlanabilir) ile kutu imlecin biraz sağ-altında duruyor. **Yarım kaldı:** kutunun arka planı
  (Image + Content Size Fitter ile padding) henüz bitmedi, bkz. "Hover Bilgi Sistemi" ve Sıradaki
  Adımlar.
- **`ButtonTooltip.cs`** — Eski statik hover script'i (sabit `TooltipMetni`), değişmedi.
- **`EkranGecisi.cs`** — Singleton, siyah ekran geçişi. Değişmedi.
- **`OdaEtkilesimTest.cs`** — `KapiTiklandi()`, `AnsiklopediTiklandi()` (hâlâ placeholder),
  `HaritaTiklandi()`: `HaritaEkrani.SetActive(true)` çağırıyor. **Not (bu oturumda düzeltildi):**
  `HaritaEkrani` objesi sahnede `m_IsActive: 1` (açık) kayıtlıymış, Play'e basılır basılmaz Taht
  Odası'ndan önce harita açılıyordu — obje Inspector'dan elle kapatıldı, artık script'in bu
  fonksiyonu çağrılana kadar harita kapalı kalıyor.
- **`HaritaEkraniKontrol.cs`** *(bu oturumda eklendi)* — `HaritaEkrani` objesine eklendi.
  `OnEnable`/`OnDisable`'da (obje her `SetActive` olduğunda, X butonunun direkt bağlantısı dahil)
  Inspector'dan atanan `StatTooltip[]` dizisini (Erzak/Altın/Nüfus) `enabled = false/true` yapıp
  devre dışı bırakıyor/açıyor — bkz. "Terrain Sistemi" bölümündeki "harita açıkken arkadaki stat
  hover'ları" notu, `CanvasGroup` YETERSİZ kalmıştı.
- **`DiplomasiBilgiPaneli.cs`** *(bu oturumda eklendi)* — Singleton, `KoyBilgiPaneli.cs` ile birebir
  aynı desen: script her zaman aktif bir "dış" objede duruyor, `Panel` (görünür kutu) onun ÇOCUĞU —
  `Awake()`'te `Panel.SetActive(false)` yapıyor (bkz. Bilinen Tuzaklar, script'in kendi objesini
  değil bir çocuğunu kapatması gerektiği kuralı, bu oturumda tam olarak bunun ihlaliyle bir bug
  yaşandı). `Goster(KrallikData krallik)`: `IsimText`, `BayrakImage` (varsa `krallik.Bayrak`),
  `DiplomasiText` (`KoyYoneticisi.DiplomasiDegerAl`), `DurumText` ("SAVASTA" kırmızı / "BARISTA"
  yeşil, `KoyYoneticisi.SavastaMi`'ye göre). `Kapat()` paneli gizliyor.

### Muhtemelen ölü kod (doğrulanmadı, bir sonraki oturum kontrol etsin)
- **`DanismanPaneli.cs`** — Eski 4 sabit buton (İnşaatçı/Askerbaşı/Asker Topla) sistemine aitti,
  kullanıcı bu butonları Şahsi Oda'dan (Unity Hierarchy'den) sildi. Script dosyası hâlâ duruyor ama
  muhtemelen artık hiçbir objeye bağlı değil — silinmedi çünkü doğrulanmadı, kontrol edilmeli.
- **`TahtOdasiTest.cs`** — `KoyluyeYardimEt()` fonksiyonu olan eski bir test script'i, muhtemelen
  artık kullanılmıyor, kontrol edilmeli.

---

## 5) ŞU ANKİ DURUM

✅ Çalışıyor (test edildi, onaylandı):
- Temel döngü uçtan uca, ScriptableObject tabanlı çoklu-stat diyalog sistemi, danışman kapı sistemi
  (General, İnşaatçı), Diyalogda "Geri"/"Boşver" (bkz. Mimari Kararları #27).
- Köy sistemi, isyan mekaniği (durum+zar+debuff+bastırma emri), Nüfus sistemi, building slot
  sistemi, Köy Bilgi Paneli, Diplomasi mekaniğinin temeli (`DiplomasiVerisi`, gece zar kontrolü,
  sağ-tık paneli) — hepsi önceki oturumlarda kurulup test edildi, değişmedi.
- Savaş mekaniğinin temeli: `KrallikData`/`Sahip`/`Garnizon`, `EtkinSavunmaHesapla`, "Saldır" emri.
- **Ekonomi giderleri** (bu oturumda eklendi, bkz. "Ekonomi Giderleri"): günlük asker maaşı +
  bina bakım gideri, ilk denenen oranlar (0.5/2) dengesiz çıkıp 0.05/1'e düşürüldü, uçtan uca test
  edildi (artık oyuncu sadece bekleyerek sonsuza kadar zenginleşemiyor).
- **Hex Harita Sistemi TAMAMEN KURULDU** (bu oturumun en büyük işi, bkz. "Hex Harita Sistemi") —
  gerçek görünür, kod-üretimli (hiç dış asset yok) altıgen harita: her köyün kendi tile'ları
  (ağırlıklı Voronoi ile boşluksuz dağıtım), krallık rengine göre boyalı tile'lar, hover'da toprak
  sınırı çizgisi, köy fethedilince renklerin otomatik güncellenmesi, tıklanabilir yerleşke işaretleri
  (özel bir ikonla, `icon40.png`). Uçtan uca test edildi, birkaç ciddi debug turu sonunda (yanlış
  RectTransform anchor'ları, `HaritaMaskeleyici`'de unutulmuş 19x scale, paralelkenar şekli) düzeldi.
- **Ordu kaynağı ve mesafeye bağlı süre** (bu oturumda eklendi, bkz. "Ordu Kaynağı ve Mesafeye Bağlı
  Süre") — İsyan Bastır/Saldır/yeni "Garnizon Gönder" emirlerinde artık "Genel Yedek Kuvvet mi yoksa
  hangi köyün garnizonundan mı gönderiyorsun" seçimi var, süre gerçek hex mesafesine göre hesaplanıyor,
  hayatta kalanlar nereden gittiyse oraya dönüyor. Uçtan uca test edildi.
- **Terrain (Arazi Türü) Sistemi** (bu oturumda eklendi, bkz. "Terrain Sistemi") — 4 tür (Ova/Orman/
  Dağ/Çöl), Erzak/Altın/Savunma'ya etkisi, F1/F2/F3 harita görünüm geçişi (Siyasi/Terrain/Kaynak),
  F3'te ikon+sayı gösterimi. Uçtan uca test edildi.
- **Düşman AI'ı — ilk versiyon** (bu oturumda eklendi, bkz. "Savaş Mekaniği") — savaşta olan düşman
  köyleri, statik Garnizon'larıyla en yakın bizim köye belli bir ihtimalle saldırabiliyor. Test edildi.
- **Diplomasi'nin devamı — Elçi danışmanı** (bu oturumda eklendi, bkz. "Diplomasi Mekaniği") —
  Hediye Gönder (Diplomasi artırır) ve Barış Teklif Et (savaşı bitirebilir, Diplomasi'ye bağlı şans,
  savaşta değilken otomatik gri) emirleri. Test edildi.
- Görsel asset pipeline (Taht Odası/Şahsi Oda arka planları) önceki oturumdan, değişmedi.

⚠️ Yarım kaldı / doğrulanmadı / bilinçli ertelendi:
- **Hover tooltip kutusunun görsel tasarımı** hâlâ bitmedi (arka plan/padding eksik) — bu oturumda
  kullanıcı bilerek bu işi ertelemeyi seçti.
- **Buton/ikon sprite'ları** (`PixelButtonsPack_BDragon1727` vb.) hâlâ dilimlenip uygulanmadı.
- **Şahsi Oda'daki eski placeholder'lar** (`OdaGorseli` altındaki mor/bej kutular) hâlâ düzeltilmedi.
- `KoyEtiketiTiklama.cs`, `DanismanPaneli.cs`, `TahtOdasiTest.cs` artık (neredeyse kesin) ölü kod —
  bu oturumda silinmesi düşünüldü ama sahne dosyasında hâlâ referans olduğu görülüp (muhtemelen
  hâlâ bir objeye bağlı komponentler) riskli bulunup ERTELENDİ, ayrı/dikkatli bir oturumda ele alınmalı.
- Ansiklopedi hâlâ boş placeholder. Başkent hâlâ sadece fikir, kod yok — Köy/Şehir/Kale tipleri de
  henüz ayrı değil (bilinçli olarak ertelendi, sadece `TileMenzili` sayısı eklendi).
- **Düşman AI'ı hâlâ çok temel** — ekonomisi donuk, Garnizon'u statik/taşınmıyor, hedef seçimi
  sadece "en yakın köy". Gerçek bir "düşünen rakip" (ekonomi + garnizon transferi + karar mantığı)
  ayrı, büyük bir iş (bkz. Mimari Kararları #30 ve kullanıcıyla yapılan tartışma).
- Kıyı/Su terrain türü bilinçli olarak eklenmedi (haritada henüz gerçek bir deniz konsepti yok).

---

## 6) SIRADAKİ ADIMLAR

Bu oturumda tamamlananlar:
1. ✅ **Play başlarken direkt harita açılma bug'ı düzeltildi** — `HaritaEkrani` artık varsayılan
   kapalı, oyun Taht Odası ile başlıyor.
2. ✅ **Terrain (Arazi Türü) Sistemi kuruldu** — 4 tür, Erzak/Altın/Savunma etkisi, F1/F2/F3 harita
   görünüm geçişi (Siyasi/Terrain/Kaynak, Civ6 tarzı ikon+sayı gösterimi).
3. ✅ **Harita açıkken arkadaki stat hover'ları çalışmaya devam etme bug'ı düzeltildi** —
   `HaritaEkraniKontrol.cs`, `StatTooltip`'in raycast-bypass ettiğini (bkz. Bilinen Tuzaklar) tespit
   edip doğrudan `enabled` ile kapatıyor.
4. ✅ **F3'te köy ikonu/isim sayıları kapatma sorunu düzeltildi** — F3'te isim/ikon gizleniyor, köyün
   oturduğu tile'a her zaman görünen ayrı bir çerçeve ekleniyor.
5. ✅ **Düşman AI'ının ilk versiyonu eklendi** — savaşta olan, Garnizonu olan düşman köyleri belli
   ihtimalle en yakın bizim köye saldırabiliyor (Saldır emriyle aynı formül).
6. ✅ **Diplomasi'nin devamı: Elçi danışmanı + Barış Yap** — Hediye Gönder / Barış Teklif Et emirleri,
   gece işleniyor, Barış Teklif Et savaşta değilken otomatik gri.
7. ✅ **Köylü NPC'sinin köy-sahiplik bug'ı düzeltildi** — düşmana ait bir köy artık Köylü gönderemiyor.
8. ✅ Danışman listesi paneline Vertical Layout Group + Content Size Fitter eklendi (otomatik hizalama).

⚠️ Bu oturumda ortaya çıkan, netleşmemiş/yarım kalan noktalar:
1. **Köy/Şehir/Kale ayrımı henüz yok** — sadece `KoyData.TileMenzili` (menzil sayısı) eklendi,
   gerçek farklı yerleşke tipleri (farklı inşa seçenekleri, farklı savunma vs.) bilinçli olarak
   ertelendi. İleride gerekirse bu, `KoyData`'ya dokunan HER sistemi etkileyecek büyük bir refactor.
2. **Düşman AI'ı hâlâ çok temel** — statik Garnizon, ekonomi yok, sadece "en yakın köy" hedefleme.
   Kullanıcıyla "ilerde bunu geliştirebilir miyiz" konuşuldu — cevap: evet, teknik altyapının çoğu
   (mesafe, saldırı formülü, ekonomi formülleri) zaten var, sadece ekonomiyi canlandırma + garnizon
   transferi + karar mantığı katmanları eklenmesi lazım, ayrı büyük bir iş olarak bekliyor.
3. Buton/ikon sprite dilimleme + hover tooltip görsel tasarımı + Şahsi Oda placeholder'ları hâlâ
   bekliyor (birkaç oturumdur bilerek erteleniyor).
4. `KoyEtiketiTiklama.cs`/`DanismanPaneli.cs`/`TahtOdasiTest.cs` hâlâ silinmedi (sahne dosyasında
   referans olduğu görülüp riskli bulundu, ayrı bir oturumda dikkatlice ele alınmalı).

Önceki oturumlarda tamamlananlar (özet): Ekonomi giderleri, Hex Harita Sistemi (baştan sona), Ordu
kaynağı + mesafeye bağlı süre, Savaş mekaniğinin temeli, Diplomasi mekaniğinin temeli, Diyalogda
Geri/Boşver, görsel asset pipeline, isyan mekaniği, building slot sistemi, Nüfus sistemi, Köy Bilgi
Paneli, StatsUI renklendirme, hover tooltip sistemi (fareyi takip ediyor ama arka planı/padding'i
hâlâ eksik).

Henüz gündeme gelmemiş ama olası konular (öncelik sırası belirlenmedi):
- **Buton/ikon sprite dilimleme + uygulama** ve **hover tooltip kutusunun görsel tasarımı** — en
  eski yarım kalan işler, birkaç oturumdur erteleniyor.
- **Düşman AI'ının geliştirilmesi:** ekonomiyi canlandırmak, garnizon transferi, gerçek bir hedef
  seçme/karar mantığı — büyük bir iş, temel sistemler (Köy/Şehir/Kale ayrımı gibi) oturunca ele alınacak.
- **Köy/Şehir/Kale ayrımı ve Başkent fikri** — hex harita sistemi bunun için zemin hazırladı
  (`TileMenzili` zaten var), ama gerçek tip ayrımı ve Başkent'in özel rolü henüz tasarlanmadı.
- **Kıyı/Su terrain türü** — haritada gerçek bir deniz/kıyı konsepti tasarlanınca eklenebilir.
- `KoyEtiketiTiklama.cs`/`DanismanPaneli.cs`/`TahtOdasiTest.cs`'in gerçekten ölü kod olduğunu
  doğrulayıp (sahnedeki referansları tek tek kontrol ederek) temizlemek.
- Başka danışmanlar eklemek, Ansiklopedi'nin ne göstereceğine karar vermek.
- Mektuplar/görev sistemini (istenirse) sıfırdan ele almak. Kaydet/yükle (save/load) sistemi.

**Genel çalışma tarzı hâlâ geçerli:** her adımı küçük parçalara böl, kullanıcı test edip onaylamadan
bir sonrakine geçme. Bu oturum bu tarza büyük ölçüde sadık kaldı — terrain sistemi bile önce
netleştirici sorularla planlanıp sonra küçük, test edilebilir parçalara (veri modeli → üretim →
F1/F2 → F3 → savunma etkisi) bölünerek uygulandı.

---

## 7) BİLİNEN TUZAKLAR / DİKKAT EDİLECEKLER

- Kod editöründe değişiklik yapıp **kaydetmeden (Ctrl+S)** Unity'ye dönmek "hayalet hatalara" yol
  açıyor — her değişiklikten sonra kaydet, Unity'nin alt köşesindeki "compiling" ikonunun bitmesini bekle.
- Script derleme hatası varken Unity bazen buton `On Click()` bağlantılarını sıfırlıyor.
- Bir dosyada tek bir hata bile olsa TÜM proje derlenemiyor — Console'daki **EN ESKİ (ilk)** hataya
  odaklanmak genelde kök sebebi buluyor.
- Bir C# class/script'i **yeniden adlandırdığında**, Unity o component'i sahnede "Missing Script"
  olarak işaretler — eski component'i silip yeni script'i tekrar ekleyip Inspector referanslarını
  ve buton `On Click()` bağlantılarını yeniden kurmak gerekiyor.
- `foreach` ile gezilen bir liste, aynı döngü içinde direkt değiştirilemiyor — ayrı, yeni bir liste
  oluşturulup dolduruluyor (`DayResolver`).
- Bir script dosyasını (kod ikonu) ile o script'ten üretilmiş bir asset'i (mavi küp ikonu) karıştırmak
  kolay — kullanıcıya hangisini bulması gerektiğini net söyle.
- UI objelerinde **Hierarchy sırası çizim sırasını belirliyor** — sonradan eklenen (daha altta duran)
  objeler öncekilerin üstüne çiziliyor.
- `[Serializable]` bir sınıf (örn. `OrderData`) başka bir serialize edilen sınıfın (örn.
  `DialogueChoice`) içine gömülüp Inspector'da düzenlenebilir olacaksa, **parametresiz bir
  constructor'ı olmalı**.
- Bir Button'ın `On Click()` listesinde eski/placeholder bir fonksiyon kalabilir.
- **Unity, bir asset içine gömülü `[Serializable]` bir sınıfı (örn. `OrderData.HedefKoy`, tipi
  `KoyData`) Inspector'da gösterirken otomatik olarak BOŞ bir örnek oluşturup asset'e kaydediyor** —
  yani "hiç atanmadıysa `null` olur" varsayımı YANLIŞ. Böyle bir alanın "gerçekten atanmış mı" diye
  kontrolünde sadece `!= null` yetmiyor, ek olarak içindeki anlamlı bir alanın (örn. `Isim`) boş
  olup olmadığına da bakmak gerekebiliyor (bkz. `DayResolver`'daki `HedefKoy` kontrolü).
- Bir ScriptableObject asset'in içindeki bir referans alanı, **sahnedeki bir MonoBehaviour'ın
  runtime listesindeki bir örneği asla tutamaz** (asset'ler proje seviyesinde, sahne objeleri
  oturum/instance seviyesinde yaşıyor). Bu yüzden "hangi köy" gibi dinamik bir seçim, Inspector'dan
  statik olarak yapılamıyor — kod içinde, runtime'da dinamik buton oluşturarak çözülüyor
  (bkz. `KoySecimPaneli.cs`). Aynı sebeple, harita etiketleri gibi sahne objelerinin `KoyData`'ya
  bağlanması da bir referans değil, **isim (string) eşleştirmesiyle** yapılıyor (`KoyEtiketiTiklama.cs`).
- Bir UI objesinin Rect Transform sınırlarını **inaktif bir üst obje altındayken** Scene görünümünde
  seçmek bazen seçim anahatını göstermeyebiliyor — üst objeyi geçici olarak aktif yapmak çözüyor.
- **Vertical Layout Group** olan bir objenin çocuklarını fareyle sürükleyip taşımaya çalışmak işe
  yaramaz (Layout Group her karede pozisyonu kendi kurallarına göre geri ayarlıyor). Çocukların
  konteyner genişliğini doldurmasını istiyorsan `Control Child Size` → Width + `Child Force Expand`
  → Width işaretlenmeli.
- Bir UI objesi sürüklenirken (drag), Canvas'ın `Scale Factor`'ü 1'den farklıysa `PointerEventData.delta`'yı
  direkt kullanmak sürüklenen noktanın imleçten kaymasına yol açar — en güvenilir çözüm her karede
  `RectTransformUtility.ScreenPointToLocalPointInRectangle` ile imlecin gerçek yerel konumunu
  hesaplayıp farkı almak.
- Bir objeyi "zoom out" ile tam ekranı kaplayacak şekilde sınırlamak istiyorsan, minimum zoom'u sabit
  bir sayı yerine `viewport boyutu / içerik boyutu` oranından (büyük olanını seçerek) hesaplamak gerekiyor.
- **Kapalı (inaktif) bir GameObject'in üzerindeki script'in `Awake()`'i hiç çalışmaz.** Bir Singleton
  script'i (`Instance = this;` diyen türden) barındıran obje, Play'den ÖNCE Inspector'da **aktif**
  bırakılmalı — script kendi `Awake()`'i içinde kendini gizleyecekse (`Panel.SetActive(false)` gibi),
  bunu KOD yapmalı, kullanıcı elle objeyi kapatmamalı. Aksi halde `Instance` hep `null` kalır ve
  `NullReferenceException` alınır (bu oturumda `KoyBilgiPaneli` ile canlı yaşandı).
- **TMP Auto Size / Wrapping / Content Size Fitter / Vertical Layout Group'u aynı anda karıştırmak
  çakışmalara yol açıyor.** İki net desen var, birbirine karıştırılmamalı:
  - **"Küçülsün ama satır atlamasın"** isteniyorsa: metinde **Wrapping kapalı + Auto Size açık**
    (min font değerini düşür, örn. 6-8), butonda **Layout Element (sabit Min Height)**, parent'ta
    **Control Child Size Height kapalı**.
  - **"Satır kaydırsın, kutu büyüsün"** isteniyorsa: metinde **Wrapping açık, Auto Size kapalı**,
    objede Content Size Fitter YERİNE parent'taki **Vertical Layout Group'un Control Child Size
    Height'ı açık** bırakılmalı — bir Layout Group'un ÇOCUĞUNA ayrıca Content Size Fitter eklemek
    Unity'de aynı anda "uyarı" (İ️ "child of layout group shouldn't have Content Size Fitter") HEM
    de görsel basıklaşmaya yol açıyor, bu ikisi birlikte kullanılmamalı.
- **`Object.GetInstanceID()` bu Unity sürümünde deprecated/hata veriyor** (CS0619) — debug amaçlı
  instance karşılaştırması gerekiyorsa `GetHashCode()` kullan.
- **String alan eşleşmesine dayanan mekanikler (örn. `DanismanButon.DanismanTipi` ile
  `OrderData.DanismanTipi`) sessizce yanlış çalışabilir — HİÇBİR hata vermez, sadece mantık çalışmaz.**
  Bu oturumda kapıdaki General butonunun `DanismanTipi` alanı eski bir isimle ("Askerbasi") kalmış,
  emirler "General" ile kayıtlıyken buton hep kilitsiz görünmüştü. Böyle "hiçbir hata yok ama
  beklenen davranış olmuyor" durumlarında, tahmin etmek yerine geçici `Debug.Log` ile her iki
  taraftaki gerçek string/instance değerlerini yazdırıp karşılaştırmak en hızlı teşhis yöntemi.
- **Bir `DialogueChoice`'u "kopyala-yapıştır" ile türetirken** (örn. yeni bir emir için mevcut
  birini Inspector'da kopyalarken), kopyalanan TÜM alanlar (Maliyet, EtkilenenStat, BasariliDegisim,
  EmirTuru vs.) elle gözden geçirilip yeniden doldurulmalı — unutulan bir alan (bu oturumda
  `MaliyetStat = "Altin"` kalıntısı) sessizce yanlış/çift bir maliyet kontrolüne yol açabiliyor.
- **Oyuncunun runtime'da seçtiği (Slider/InputField gibi) dinamik bir maliyet varsa ve bu maliyet
  kod içinde elle düşülüyorsa, `OrderData`'nın kendi `MaliyetStat`/`MaliyetMiktar` alanları o emrin
  kopyasında BOŞALTILMALI.** Aksi halde `OrderManager.EmirEkle`, zaten elle düşülmüş kaynağı TEKRAR
  kontrol edip (ikinci kez yetersiz görüp) emri **sessizce reddedebiliyor** — hiçbir hata/uyarı
  oyuncuya ulaşmıyor, emir sanki hiç verilmemiş gibi kayboluyor (bkz. Mimari Kararları #15).
- **Bir script'teki `public` alanın C# koddaki varsayılan değerini (örn. `public float X = 5f;`)
  SONRADAN değiştirmek, sahnede ZATEN VAR OLAN bir component'in Inspector'daki değerini
  GÜNCELLEMEZ** — Unity o alanı ilk oluşturulduğunda serialize edip sahne dosyasına yazıyor, kod
  değişse bile o obje hep eski (serialize edilmiş) değeri kullanmaya devam ediyor. Bu, "kodu
  değiştirdim ama davranış değişmedi" diye görünen sessiz bir bug kaynağı — bu oturumda Nüfus
  büyüme formülünün `Eşik`/`Katsayı` sabitleri değiştirildiğinde tam olarak bu yaşandı (kod yeni
  değerleri gösteriyordu ama Inspector'da hâlâ eskisi duruyordu). Çözüm: böyle bir değişiklikten
  sonra kullanıcıya o alanları **Inspector'dan elle güncellemesi** gerektiğini söyle.
- **Bu proje Unity'nin YENİ Input System paketini kullanıyor, eskisini DEĞİL.** Kod içinde
  `UnityEngine.Input.mousePosition` (veya `Input.GetKey` vs.) kullanmak `InvalidOperationException`
  fırlatıyor ("you have switched active Input handling to Input System package"). Fare/klavye
  pozisyonu/girdisi gerekiyorsa `UnityEngine.InputSystem.Mouse.current.position.ReadValue()` (ve
  ilgili `Mouse.current`/`Keyboard.current` API'leri) kullanılmalı (bu oturumda `TooltipUI.cs`'te
  yaşandı, bkz. Mimari Kararları #24).
- **Bir UI hover/tooltip script'i Unity'nin normal raycast/EventSystem'ini KULLANMIYORSA (`StatTooltip.cs`
  gibi, `TMP_TextUtilities.FindIntersectingLink` ile her karede doğrudan fare-pozisyonu/metin-geometrisi
  karşılaştırması yapıyorsa), üstüne başka bir UI paneli (örn. harita) açmak onu ENGELLEMEZ — panel
  görsel olarak üstte dursa bile hover çalışmaya devam eder. `CanvasGroup.blocksRaycasts = false` gibi
  standart "tıklamayı engelle" çözümleri bu tür script'lerde İŞE YARAMAZ, çünkü hiç raycast kullanmıyor.
  Doğru çözüm: o script'i (`enabled = false/true`) doğrudan devre dışı bırakmak (bu oturumda
  `HaritaEkraniKontrol.cs` ile çözüldü) — bir hover/tooltip bug'ında önce script'in GERÇEKTEN
  EventSystem mi kullandığını yoksa kendi custom mantığı mı olduğunu kontrol et.
- **Sahiplik/mülkiyet gibi yeni bir kavram (örn. `KoyData.Sahip`) eklendiğinde, o kavramdan ÖNCE
  yazılmış TÜM döngüler/sistemler tek tek gözden geçirilmeli.** Bu oturumda `DaySequencer`'ın Köylü
  zar döngüsü, `Sahip`/`Krallik` sistemi eklenmeden önce yazıldığı için hiç sahiplik kontrolü
  yapmıyordu — düşman bir köy bile "bize erzak lazım" diye şikayetçi gönderebiliyordu. `KoyYoneticisi`
  gibi merkezi yerler güncellenmişti ama ondan bağımsız çalışan `DaySequencer` gözden kaçmıştı
  (bkz. Mimari Kararları #31). Yeni bir global kavram eklerken "bunu kullanması gereken başka nerede
  var?" diye kod tabanında arama yapmak, sadece "mantıklı" yerlere bakmaktan daha güvenli.
  - Bir UI objesinin **Anchor Preset**'ini Unity Editor'da "stretch"ten tek bir noktaya (örn.
  sol-üst köşe) çevirmek, objenin `Width`/`Height`'ını OTOMATİK küçültmüyor — eski stretch halinin
  boyutunu (örn. tüm ekran, 1920x1080) olduğu gibi koruyor. Anchor'ı değiştirdikten sonra
  `Width`/`Height`'ı elle makul bir değere ayarlamak gerekiyor, yoksa obje "kayboldu" gibi görünür
  (aslında hâlâ var ama koca bir kutu, pozisyonu tuhaf duruyor).
- **`anchoredPosition`'ın ne anlama geldiği, bir UI objesinin `Anchor Min/Max`'ına göre DEĞİŞİR.**
  Kod içinde `RectTransformUtility.ScreenPointToLocalPointInRectangle` ile hesaplanan bir "yerel
  nokta", parent'ın PİVOT'una göredir (genelde merkez) — ama bunu doğrudan bir objenin
  `anchoredPosition`'ına atamak, o obje TEK BİR NOKTAYA (örn. `0,1` sol-üst) anchor'lıysa YANLIŞ
  sonuç verir, çünkü `anchoredPosition`'ın referans noktası artık parent'ın merkezi değil, o anchor
  noktasıdır. Bu oturumda `TooltipUI.cs`'te fareyi takip eden bir kutu, Anchor stretch'ten sol-üst
  köşeye çevrilince aniden ekran dışına kayboldu — çözüm, panelin `anchorMin`'ine göre parent'ın
  kendi `rect`i içindeki karşılık gelen noktayı (`Mathf.Lerp` ile) bulup farkı almaktı (bkz. Mimari
  Kararları #24). Böyle bir "pozisyon tuhaf davranıyor" durumunda önce anchor/pivot ayarlarının
  kod ile uyumlu olup olmadığını kontrol et.
- **Bir hover kutusu (tooltip) fareyi takip ederken kendi üzerine gelip "glitch" gibi yanıp
  sönebilir** (hızlı açılıp kapanma) — sebep genelde kutunun kendisi fare olaylarını (raycast)
  engelliyor olması: imlecin altına giren kutu, asıl hover edilen objeden `OnPointerExit`
  tetikliyor, kutu kapanınca imleç tekrar o objeye "görünür" oluyor, `OnPointerEnter` tekrar
  tetikleniyor — sonsuz döngü. Çözüm: kutunun **TÜM** görsel component'lerinde (hem arka plan
  `Image` hem `TMP_Text`) **Raycast Target** kapatılmalı — sadece birini kapatmak yetmez, ikisi de
  ayrı ayrı kontrol edilmeli (bu oturumda tam olarak bu yaşandı, önce Text kapatıldı yetmedi, sonra
  Image de kapatılınca düzeldi).
- **Bir açılıp-kapanan popup/panel kurarken (`KoyBilgiPaneli` deseni), script'i barındıran obje ile
  `SetActive(false)` yapılan "görünür kutu" objesi AYNI obje OLMAMALI, biri diğerinin ÇOCUĞU
  olmalı.** Script kendi objesini kapatırsa (`Awake()`'te kendi kendini `SetActive(false)` yaparsa)
  o obje bir daha hiç açılamaz (kapalı objenin script'i çalışmaz, bkz. yukarıdaki "Kapalı
  GameObject" tuzağı) — doğru yapı: dış obje (her zaman aktif, script burada) → onun ÇOCUĞU olan
  "Panel" (görünür kutu, bu açılıp kapanıyor) → Panel'in çocukları (metin/görsel/buton). Bu
  oturumda `DiplomasiBilgiPaneli` kurulurken bunun tersi yaşandı: görünür içerik (metinler, arka
  plan kutusu) yanlışlıkla "Panel" objesinin KARDEŞİ olarak eklendi, "Panel"in dışında kaldı —
  sonuç: `Panel.SetActive(false)` hiçbir şeyi gizlemedi, panel her sahnede sürekli açık görünmeye
  devam etti VE bu görünmez-ama-var kocaman kutu altındaki haritaya gelen tıklamaları (raycast)
  yakalayıp bloke etti (sağ tık haritaya hiç ulaşmadı). Yeni bir panel kurarken Hierarchy'de her
  elemanın **doğru objenin ÇOCUĞU** olarak (doğrudan o objeye sağ tıklanarak) eklendiğini, kardeşi
  olarak değil, girinti (indent) seviyesinden teyit et.
- **Zaten bir Canvas'ın altında olmayan düz bir `GameObject`'e (örn. `GameObject > Create Empty`
  ile oluşturulmuş, `RectTransform` içermeyen bir obje) `UI > Panel/Button/Text` eklemeye çalışmak,
  Unity'nin farkında olmadan yepyeni, gereksiz bir `Canvas` oluşturmasına yol açabiliyor** (UI
  elemanları bir Canvas/RectTransform zincirine ihtiyaç duyduğu için). Bu, Hierarchy'de beklenmedik
  bir "Canvas" objesinin belirmesiyle ve world-space (örn. `X: 867` gibi büyük) bir `Position`
  görünmesiyle fark edilir (normal bir UI elemanının `anchoredPosition`'ı böyle büyük değerler
  almaz). Çözüm: yeni bir UI panel/container kurarken dış objeyi düz `Create Empty` ile değil,
  zaten sahnedeki ana `Canvas`'a sağ tıklayıp `UI > Panel` ile oluştur (RectTransform garanti
  gelir), gerekirse üzerindeki `Image` component'ini `Remove Component` ile kaldır.
- **Kod içinde `new GameObject(...)` ile taze bir UI objesi oluşturup `AddComponent<RectTransform>()`
  yaptığında, çapa (anchor/pivot) ayarlarını AÇIKÇA belirtmezsen güvenme.** Bu oturumda hex harita
  çizilirken (`HexHaritaCizici`), `sizeDelta`'ya küçük bir boyut verilmesine rağmen objeler (özellikle
  isim yazıları) ekranı kaplayacak kadar devasa çıktı — sebep, RectTransform'un varsayılan çapa
  davranışının güvenilmez olması. Çözüm: her yeni obje için `anchorMin`/`anchorMax`/`pivot`'u
  `(0.5, 0.5)` (sabit, merkez nokta) olarak KODDA elle ayarlayan ortak bir yardımcı fonksiyon
  yazmak (`SabitCapaliRectOlustur`), varsayılana asla güvenmemek. Ayrıca TMP_Text için: `.text`'i
  RectTransform boyutunu ayarlamadan ÖNCE atarsan, TMP ilk hesaplamayı yanlış (büyük) boyuta göre
  yapıp "kilitleyebiliyor" — güvenli sıra: önce `sizeDelta`/`anchoredPosition`'ı ayarla, TMP
  component'ini SONRA ekleyip `.text`'i EN SON ata.
- **Axial (eksenel) hex koordinat sisteminde DİKDÖRTGEN bir X/Y aralığı taramak, ekranda PARALELKENAR
  şeklinde bir harita üretir** (hex-grid'lerin bilinen bir özelliği, hata değil). Gerçek altıgen
  (petek) şeklinde bir harita istiyorsan, bir merkez noktasından `yarıçap` bazlı "cube coordinate
  range" taraması yapman gerekiyor (`for q in -r..r, for r2 in max(-r,-q-r)..min(r,-q+r)`).
- **Bir UI objesinin (örn. bir harita maskesi/viewport'u) `Scale`'i beklenmedik şekilde 1'den farklı
  çıkabilir** — özellikle bir script'in eskiden (yanlışlıkla) BAŞKA bir objeyi `Icerik` sanıp
  `localScale`'ini değiştirdiği bir geçmişi varsa, bu değer Play modu DIŞINDA yapılan bir
  değişiklikse kalıcı olarak sahne dosyasına yazılıp kalır. Tuhaf/devasa/taşan bir UI görünce ilk
  kontrol edilecek yerlerden biri: ilgili objenin (ve TÜM ata objelerinin) Rect Transform → **Scale**
  alanı gerçekten `1,1,1` mi? Bu oturumda `HaritaMaskeleyici`'nin Scale'i `19.16 / 10.73` çıktı ve
  bütün çocukları (harita içeriği, tüm hex tile'lar) bunu miras aldığı için kocaman/taşkın
  görünüyordu — kaynağı bulmak saatler sürdü, `Scale` alanına önce bakmak çok daha hızlı olurdu.
- **Unity, Play modundayken script'ler yeniden derlenince ("domain reload") bazen singleton
  referanslarını (`Instance` alanları) güvenilmez şekilde sıfırlıyor/geç kuruyor** — bu projede
  canlı olarak `GameManager.Instance` null çıkıp `NullReferenceException` spam'ine yol açtı. Kesin
  çözüm: kod değişikliğinden sonra **Stop'a basıp tam durdurmak, sonra tekrar Play'e basmak**
  (sadece Pause/Resume yetmiyor). Play açıkken uzun bir kod-değiştirme oturumu varsa, test etmeden
  önce her seferinde temiz bir Stop→Play yapmak alışkanlık hâline getirilmeli.
- **Çok günlü (mesafeye/inşaat süresine bağlı) hale gelebilen, ZAR ATAN bir emir varsa,
  `OrderData.SonucSansaBagli = true` işaretli OLMAK ZORUNDA** — aksi halde `DayResolver`'ın çok-
  günlü tamamlanma yolu zar atan özel mantığı (`ZarAtVeUygula`) hiç çağırmadan sadece "tamamlandı"
  deyip geçiyor (sessizce, hata vermeden). Bu alan emir hep aynı gece (`ToplamSure<=1`) sonuçlanırken
  önemsizdi, mesafeye bağlı süre sistemi gelince (bu oturumda) gizli kalmış bir eksiklik ortaya
  çıkardı: `Saldır` emri 3 gün sürünce hiçbir zar atmadan/sahiplik değiştirmeden "tamamlandı" dedi.
  Yeni bir şansa-bağlı+çok-günlü-olabilecek emir eklerken bu kutucuk HER ZAMAN kontrol edilmeli.
