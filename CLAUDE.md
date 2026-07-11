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

### Savaş Mekaniği (bu oturumda eklendi)
Diğer krallıklarla savaşın **ilk temeli** kuruldu (başkent/diplomasi hâlâ sadece beyin fırtınası,
bkz. Sıradaki Adımlar). Yeni **`Krallik`** enum'u (`Oyuncu`, `Dusman`) ve `KoyData.Sahip` alanı ile
her köyün kime ait olduğu belirleniyor. `KoyData.Garnizon` (int, varsayılan 0) o köyde konuşlanan
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
Kullanıcı bunu bilerek onayladı, tam bir düşman AI'ı (asker toplama, saldırma, ekonomi yönetme)
kasıtlı olarak yapılmadı — çok daha büyük bir iş, ayrı bir konu.

**Harita rengi otomatik:** `KoyEtiketiTiklama.cs` artık köyün rengini `Sahip`e göre kendi hesaplıyor
(isyanda kırmızı, `Sahip == Dusman` ise mavi `DusmanRengi`, aksi halde etiketin kendi normal rengi) —
elle her düşman köyü için renk ayarlamaya gerek yok, bkz. Mimari Kararları #21.

**Köy Bilgi Paneli düşman köyünde farklı görünüyor:** `Sahip != Oyuncu` ise Sadakat/Erzak/Nüfus/
Yield'ler/Bina Slotu **tamamen gizleniyor** (bunlar düşman köyü için anlamsız, çünkü ekonomisi
donuk), sadece İsim + Savunma + **Garnizon** (yeni eklendi) gösteriliyor — bize ait köylerde her şey
eskisi gibi görünmeye devam ediyor.

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

### Harita
Şahsi Oda'daki Harita objesine tıklayınca, ekranı kaplayan bir harita sekmesi açılıyor (sol üstteki
X'e basana kadar kapanmıyor). Fare tekerleğiyle zoom, sol tıkla sürükleyerek pan yapılabiliyor
(bkz. `HaritaKontrol.cs`). Zoom'un minimum seviyesi haritanın tam ekranı kapladığı nokta (daha fazla
uzaklaşılamıyor), sürükleme haritanın kenarları ekran dışına çıkmayacak şekilde sınırlı. Köylerin
isimleri şu an **elle/statik olarak** haritanın üzerine TextMeshPro etiketleriyle yerleştirildi —
**pozisyonları** hâlâ `KoyData`'ya bağlı değil (köy sayısı değişirse elle yeni etiket eklenmesi
gerekir), ama **tıklama/hover/isyan renklendirmesi artık isim eşleştirmesiyle gerçek veriye bağlı**
(bkz. "Köy Bilgi Paneli").

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

---

## 4) SCRIPT ENVANTERİ

### Veri sınıfları (düz C#, MonoBehaviour DEĞİL)
- **`GameState.cs`** — `Gun`, `Sadakat`, `Altin`, `Manpower`, `ErzakBaseGelir`, `AltinBaseGelir`.
  `Erzak` ve `Nufus` alanları yok (bkz. Mimari Kararları #7, #18) — ikisi de `KoyYoneticisi`'ye
  yönlendiriliyor. `StatDegerAl`/`StatDegistir`'in `"Nufus"` case'i `KoyYoneticisi.ToplamNufus()`/
  `NufusDegistir()`'e gidiyor. Sadakat için genel+köy ortalamasını topluyor ve `Mathf.Clamp`
  ile 0-100 arasına sıkıştırıyor. `BaseGeliriUygula()`: her yeni günde `KoyYoneticisi.ErzakDegistir(ErzakBaseGelir)`
  + `KoyYoneticisi.NufusuGunlukArtir()` (Nüfus'un kendi dinamik formülü, sabit bir "NufusBaseGelir"
  YOK) + `Altin += AltinBaseGelir`.
- **`Krallik.cs`** *(yeni, bu oturumda eklendi)* — Düz `enum { Oyuncu, Dusman }`. `KoyData.Sahip`'in
  tipi, bkz. Mimari Kararları #21.
- **`KoyData.cs`** — Bir köyün "kimlik kartı": `Isim`, `Sadakat` (varsayılan 50), `Erzak`
  (varsayılan 20), **`Nufus`** (varsayılan 30, bkz. Mimari Kararları #18 — sabit bir `NufusYield`
  alanı YOK, büyüme dinamik hesaplanıyor), `ErzakYield` (varsayılan 1, ama oyun başında
  `KoyYoneticisi.Awake`'te 1-4 arası rastgele üzerine yazılıyor), `AltinYield` (varsayılan 0),
  **`Savunma`** (varsayılan 20 — isyan bastırmada kullanılmıyor, dış savaşta `EtkinSavunmaHesapla`
  üzerinden kullanılıyor, bkz. Mimari Kararları #21), `MaxBinaSlotu` (varsayılan 3),
  `DoluBinaSlotu` (varsayılan 0), `IsyanHalinde` (bool, varsayılan false), **`Sahip`** (`Krallik`,
  varsayılan `Oyuncu`, bu oturumda eklendi) ve **`Garnizon`** (int, varsayılan 0, bu oturumda
  eklendi — o köyde konuşlanan Manpower). ScriptableObject DEĞİL.
- **`OrderData.cs`** — Ana constructor aynı (`DanismanTipi, EmirTuru, EtkilenenStat, ...`). Alanlar:
  `KoySecimiGerekli` (bool), `HedefKoy` (nullable `KoyData`), `BinaSlotuKullanir` (bool),
  `IsyanliKoyGerekli` (bool — köy seçim filtresi, sadece isyandaki köyler), `ManpowerMiktariSorulsun`
  (bool — köy seçilince Manpower Slider paneli açılsın mı), `IsyanBastirir` (bool — bu emrin
  başarılı sonucu bir köyün isyanını kapatsın mı), `GonderilenManpower` (int — seçilen Manpower
  miktarı, `MaliyetMiktar` sıfırlandığı için ayrı tutuluyor, bkz. Mimari Kararları #17),
  **`DusmanKoyuGerekli`** (bool — köy seçim filtresi, sadece düşman köyleri; bu oturumda eklendi,
  bkz. Mimari Kararları #21), **`SaldiriBaslatir`** (bool — bu emrin sonucu bir Saldırı işlemi mi;
  bu oturumda eklendi). `KopyalaVeKoyAta(KoyData koy)` — tüm alanları
  (yenileri dahil) kopyalayıp `HedefKoy`'u dolduran bir kopya döndürür (şablonu bozmamak için).
- **`SiraGirisi.cs`** — `NPCData Npc` + `KoyData IlgiliKoy` (nullable). `DaySequencer`'ın
  ürettiği sıradaki bir "ziyaretin" hangi NPC ve (varsa) hangi köyle ilgili olduğunu taşıyan basit
  bir zarf sınıfı.
- **`DevamEdenEmir.cs`** — `OrderData Emir` + `int KalanGun`. Değişmedi.
- **`DaySequencer.cs`** — `SiradakiListeyiOlustur(state, koyluNpc, askerNpc, ayyasNpc)`
  `List<SiraGirisi>` döndürüyor. Köylü "her köy kendi zarını atar" mantığıyla ekleniyor
  (bkz. Mimari Kararları #10). `Gun == 10` kuralı (Ayyaş Adam) değişmedi.
- **`DayResolver.cs`** — Tek günlük emirler anında, çok günlüler `devamEdenler`'e. `ZarAtVeUygula`
  içinde **`emir.IsyanBastirir` kontrolü en başta** — işaretliyse genel stat mantığını atlayıp,
  başarı şansını `GonderilenManpower/(GonderilenManpower+HedefKoy.Nufus)` formülüyle hesaplayıp
  (bkz. Mimari Kararları #17, #19) `emir.HedefKoy.IsyanHalinde`'yi ayarlayıp kendi mesajını yazıyor
  (bkz. Mimari Kararları #14). **Manpower kayıp/dönüş** (bkz. #19): kazanırsa %15, kaybederse
  %80 kayıp, geri kalanı `state.StatDegistir("Manpower", ...)` ile kingdom'a iade ediliyor, elçi
  mesajına kaç asker kaybedildiği/döndüğü de ekleniyor. **`emir.SaldiriBaslatir` kontrolü** (bu
  oturumda eklendi, bkz. Mimari Kararları #21) — `KoyYoneticisi.EtkinSavunmaHesapla(HedefKoy)`'a
  karşı `GonderilenManpower` ile oran/Tullock formülü, kazanırsa `HedefKoy.Sahip = Oyuncu` +
  hayatta kalan Manpower `HedefKoy.Garnizon`'a yazılıyor (kingdom'a DÖNMÜYOR), kaybederse hayatta
  kalan Manpower kingdom'a geri dönüyor (aynı %15/%80 yüzdeleri).
  `BaseGeliriEtkiler` dalı `HedefKoy` doluysa (ve `Isim`'i boş değilse) o köyün `ErzakYield`'ini
  **2 katına çıkarıyor** (eski davranış: sabit +miktar ekliyordu). Her stat değişiminde
  `BildirimYoneticisi.Bildirim(...)` çağrılıyor. Sonuç mesajları renkli: şansa bağlı başarı yeşil,
  başarısızlık kırmızı, garanti tamamlanma sarı.
- **`NPCData.cs`** — ScriptableObject. `ID`, `Isim`, `DialogueData Diyalog`, `Sprite Portre`.
  Danışmanlar için de kullanılıyor (`General_NPC.asset`, `Insaatci_NPC.asset`).
- **`DialogueData.cs`** — ScriptableObject. `DialogueChoice`: `SecenekMetni`, `SonrakiNodeID`,
  `List<StatEtkisi> StatEtkileri` (her biri `StatAdi`+`Miktar`), `OrderData VerilecekEmir`.

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
  ayarlanabilir. **`EtkinSavunmaHesapla(koy)`** *(bu oturumda eklendi)* = `Savunma * (1 +
  Garnizon/GarnizonKatsayisi)`, `GarnizonKatsayisi` (varsayılan 10) Inspector'dan ayarlanabilir,
  bkz. Mimari Kararları #21. **`ErzakDagilimMetni()`** *(bu oturumda eklendi)* — bize ait her
  köyün Erzak'ını "Köy: X" formatında satır satır döken bir string döndürüyor, Erzak hover
  tooltip'i tarafından kullanılıyor (bkz. "Hover Bilgi Sistemi").
- **`OrderManager.cs`** — `EmirEkle`, `DanismanKullanildiMi`, `YeniDongueBasla`. Mantık değişmedi
  ama **dikkat:** `EmirEkle` kendi `MaliyetStat`/`MaliyetMiktar` kontrolünü de yapıyor — dinamik
  maliyetli emirlerde bu alanların boşaltılması gerekiyor (bkz. Mimari Kararları #15).
- **`DialogueManager.cs`** — Bu oturumda büyük bir revizyon geçirdi (bkz. Mimari Kararları #20).
  `NpcIsimText`, `NpcSozuText`, `PortreImage`, `Orders`, `DiyalogKutusuKok`. Sabit `SecenekButon1/2`
  alanları **kaldırıldı**, yerine `SecenekButonSablonu` (GameObject, şablon) + `SecenekIcerik`
  (Transform, Scroll Rect'in Content'i) geldi. `NodeGoster()`: `{KOY}` yer tutucusunu değiştirdikten
  sonra, önceki seçenek butonlarını `Destroy` edip `aktifNode.Secenekler`'deki HER seçenek için
  `SecenekButonSablonu`'nu `Instantiate` ediyor, metin/tıklanabilirlik/tooltip'i kod içinde
  dolduruyor. `KoySecimGoster(sablon, callback)` *(yeni)* — aynı dinamik buton altyapısını
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
- **`SecenekTooltip.cs`** — Diyalog seçenek butonlarına eklenen hover script'i. Artık sabit
  `SecenekIndex` (0/1) yerine doğrudan **`DialogueChoice Secenek`** referansı alıyor (bu oturumda
  değiştirildi, butonlar dinamik olduğu için index anlamsızlaştı), `Dialog.MaliyetMetniAl(Secenek)`
  çağırıp `TooltipUI.Goster`/`Gizle` ile gösterir/gizler.
- **`KoySecimPaneli.cs`** — **Bu oturumda tamamen silindi** (script + sahne objesi). Görevini artık
  `DialogueManager.KoySecimGoster()` üstleniyor, bkz. Mimari Kararları #20.
- **`ManpowerSeciciPaneli.cs`** — Singleton. `Panel`, `Slider MiktarSlider`, `TMP_Text
  MevcutManpowerText`, `TMP_Text SeciliMiktarText`. `Sor(Action<int> callback)`: slider'ın min/max'ını
  (0 / mevcut Manpower) ayarlayıp paneli açıyor, `Slider.onValueChanged` ile seçili miktar metnini
  canlı güncelliyor. `GonderTiklandi()`: slider değerini `Mathf.RoundToInt` ile tam sayıya çevirip
  callback'i çağırıyor.
- **`KoyBilgiPaneli.cs`** — Singleton. `Panel`, `IsimText`, `DurumText` (isyan varsa kırmızı
  "ISYAN HALINDE"), `SadakatText`, `ErzakText`, `ErzakYieldText`, `AltinYieldText` (ikisi de
  `YieldMetni` yardımcı fonksiyonuyla renkleniyor: +yeşil/-kırmızı/0-beyaz, isyanda gri), `SlotText`
  ("1/3" formatı), `SavunmaText`, `NufusText` ("Nufus: X <sup>+Y</sup>" formatında, Y
  `KoyYoneticisi.NufusYieldHesapla(koy)` ile canlı hesaplanıyor). **`GarnizonText`** *(bu oturumda
  eklendi)*. `Goster(koy)` artık `koy.Sahip == Oyuncu` mu diye bakıyor (bkz. Mimari Kararları #21):
  değilse Sadakat/Erzak/Nüfus/Yield/Slot satırlarının GameObject'lerini `SetActive(false)` ile
  gizleyip sadece İsim + Savunma + Garnizon gösteriyor; bize aitse eskisi gibi her şey görünüyor.
- **`KoyEtiketiTiklama.cs`** — Harita üzerindeki her köy isim etiketine eklenen script.
  `IPointerClickHandler`/`IPointerEnterHandler`/`IPointerExitHandler`. `BulKoy()`: kendi TMP_Text'inin
  yazdığı ismi `KoyYoneticisi.Koyler`'daki `Isim`lerle case-insensitive+trim karşılaştırıyor (bkz.
  Mimari Kararları #16). Tıklanınca `KoyBilgiPaneli.Goster`. `GuncelleRenk()` (bu oturumda
  genişletildi, bkz. Mimari Kararları #21): isyanda `IsyanRengi` (kırmızı), `Sahip == Dusman` ise
  **`DusmanRengi`** (mavi, yeni eklendi), aksi halde etiketin kendi normal rengi — artık düşman
  köyünü elle mavi yapmaya gerek yok, otomatik. Hover'da `HoverRengi` gösteriyor.
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
  `Gun`'ı +1 artırıp Resolve'a geçiyor, `GelirleriUygula()` çağırıyor, sonra **`KoyYoneticisi.
  IsyanKontrolEt(...)` emirlerden ÖNCE çağrılıyor**, sonra `DayResolver` çalışıyor.
- **`DanismanCagir.cs`** — Kapıdan açılan danışman listesindeki butonlara bağlı. `GeneralCagir()`,
  `InsaatciCagir()`: ilgili danışmanın diyaloğunu `danismanDiyalogu=true` ile başlatıyor.
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
  `KoyYoneticisi.Instance.ErzakDagilimMetni`'i `MetinFonksiyonu` olarak atıyor.
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
  `HaritaTiklandi()`: `HaritaEkrani.SetActive(true)` çağırıyor.

### Muhtemelen ölü kod (doğrulanmadı, bir sonraki oturum kontrol etsin)
- **`DanismanPaneli.cs`** — Eski 4 sabit buton (İnşaatçı/Askerbaşı/Asker Topla) sistemine aitti,
  kullanıcı bu butonları Şahsi Oda'dan (Unity Hierarchy'den) sildi. Script dosyası hâlâ duruyor ama
  muhtemelen artık hiçbir objeye bağlı değil — silinmedi çünkü doğrulanmadı, kontrol edilmeli.
- **`TahtOdasiTest.cs`** — `KoyluyeYardimEt()` fonksiyonu olan eski bir test script'i, muhtemelen
  artık kullanılmıyor, kontrol edilmeli.

---

## 5) ŞU ANKİ DURUM

✅ Çalışıyor (test edildi, onaylandı):
- Temel döngü uçtan uca, siyah ekran geçişi, ScriptableObject tabanlı çoklu-stat diyalog sistemi,
  karşılanabilirlik kontrolü (Sadakat hariç), renkli gece sonuç mesajları, elçi/Ulak anlatımı.
- Danışman diyalog akışı (kapı sistemi): General (**İsyan Bastır** + Asker Topla) ve İnşaatçı
  (Değirmen İnşa Et) uçtan uca test edildi, danışman başına döngüde 1 kullanım kısıtlaması ortak
  `DanismanButon.cs` ile çalışıyor (bu oturumda bir isim uyuşmazlığı bugı bulunup düzeltildi).
- Anlık bildirim (toast) sistemi, hover tooltip sistemi.
- **Harita sistemi**: tam ekran, sürüklenebilir, zoom sınırlı, pozisyon kenarlara kilitleniyor.
  Köy isimleri elle yerleştirildi (pozisyon veri-bağlı değil) ama **tıklama/hover/isyan
  renklendirmesi artık gerçek `KoyData`'ya bağlı**.
- **Köy sistemi**: `KoyData`/`KoyYoneticisi` kuruldu, kingdom Erzak = köylerin toplamı, Sadakat =
  genel + köy ortalaması. Oyun başında her köyün Erzak Yield'i 1-4 arası rastgele.
- **Köylü NPC'si gerçek bir köyü temsil ediyor**, İnşaatçı/Değirmen hangi köyde yapılacağını soruyor
  ve tamamlanınca o köyün Erzak Yield'ini 2'ye katlıyor.
- **Building slot sistemi**: her köyün Inspector'dan ayarlanabilir slot sınırı var, dolan köyler
  İnşaatçı panelinde tıklanamıyor.
- **Köy Bilgi Paneli**: haritadaki köy ismine tıklayınca Sadakat/Erzak/Yield'ler/slot/isyan durumu
  gösteren bir popup açılıyor, Yield'ler renkli (yeşil/kırmızı/beyaz/isyan grisi).
- **İsyan mekaniği**: Sadakat<50 olan köyler gece zar atıp isyan edebiliyor, isyandaki köylerin
  Yield'leri debuff'lanıyor (0 sayılıyor) VE **stokları (Erzak/Nüfus) da kingdom toplamından
  tamamen dışlanıyor** (bu oturumda eklendi, bkz. Mimari Kararları #19), harita/panelde görsel
  olarak belli oluyor, General'ın "İsyan Bastır" emriyle (köy seçimi → Manpower miktarı Slider'la
  seçimi → gece dinamik başarı şansıyla — `Manpower/(Manpower+HedefKoy.Nufus)`, bu oturumda
  `Savunma` yerine `Nufus`'a çevrildi — bastırma) uçtan uca test edildi. **Manpower kayıp/dönüş**
  sistemi eklendi: kazanırsa %15, kaybederse %80 Manpower kaybediliyor, geri kalanı kingdom'a
  geri dönüyor (bkz. Mimari Kararları #19).
- **Gelir zamanlaması düzeltildi**: Uyu'ya basmadan önce ekranda görünen tahmini günlük gelir,
  artık o gece bir köy isyan etse bile ertesi sabah birebir gerçekleşiyor (bkz. Mimari Kararları #17).
- **Nüfus sistemi**: kingdom Nüfus'u = köylerin toplamı (Erzak ile aynı desen), sol üstte ve Köy
  Bilgi Paneli'nde gösteriliyor, günlük büyüme her köyün kendi Erzak stokuna bağlı dinamik bir
  formülle hesaplanıyor (`Erzak/Nufus` eşiğe göre büyür/küçülür, Inspector'dan ayarlanabilir),
  test edildi (bkz. Mimari Kararları #18).
- **Stat panelindeki üst indis gelir göstergeleri renklendirildi** (StatsUI, +yeşil/-kırmızı/0-beyaz).
- **Diyalog seçenekleri artık dinamik/sınırsız** (Scroll View, 2 buton sınırı kalktı), `KoySecimPaneli`
  tamamen silindi, köy seçimi diyalog kutusunun içinde gösteriliyor (bkz. Mimari Kararları #20).
- **Savaş mekaniğinin temeli**: `Krallik`/`Sahip`/`Garnizon`, `EtkinSavunmaHesapla`, General'ın
  "Saldır" emri (köy seç → Manpower Slider → gece `Manpower/(Manpower+EtkinSavunma)` formülü) uçtan
  uca test edildi. Kazanınca köy `Oyuncu`'ya geçiyor, Manpower orada garnizon kalıyor. Bir düşman
  köyü ("Dusman Koyu") sahneye eklendi, haritada otomatik mavi görünüyor. Düşman köylerinin
  Erzak/Nüfus/Yield'leri kingdom toplamından tamamen dışlanıyor, ekonomisi bilinçli olarak donuk
  (AI yok). Köy Bilgi Paneli düşman köyünde sadece İsim/Savunma/Garnizon gösteriyor (bkz. Mimari
  Kararları #21-22).

⚠️ Yarım kaldı / doğrulanmadı / bilinçli ertelendi:
- **Hover tooltip kutusunun görsel tasarımı bitmedi** — fareyi takip etme ve içerik (Erzak dökümü)
  çalışıyor, ama kutunun arka planı (Image + padding) henüz eklenmedi, yarın devam edilecek
  (bkz. "Hover Bilgi Sistemi").
- `DanismanPaneli.cs` ve `TahtOdasiTest.cs` muhtemelen artık ölü kod, kontrol edilip silinebilir.
- Ansiklopedi hâlâ tamamen boş placeholder, hiç tasarlanmadı.
- Harita'daki köy etiketlerinin **pozisyonu** hâlâ `KoyData`'ya bağlı değil (elle yerleştirildi) —
  yeni köy eklenirse elle yeni etiket eklenmesi gerekiyor (ama artık en azından tıklama/renklendirme
  isim eşleştirmesiyle veriye bağlı).
- Şu an sadece **General** ve **İnşaatçı** var, başka danışman yok.
- Mektuplar/görev sistemi silindiği için, "oyuncuya görev/istek gelen" bir mekanik yok — istenirse
  sıfırdan, farklı bir tasarımla ele alınabilir.
- Başkent fikri ve diplomasi (Barış/İttifak) hâlâ sadece beyin fırtınası, kod yazılmadı.

---

## 6) SIRADAKİ ADIMLAR

Bu oturumda tamamlananlar:
1. ✅ **Diyalog seçenekleri dinamikleştirildi** (Scroll View, 2 buton sınırı kalktı), `KoySecimPaneli`
   tamamen silinip yerine `DialogueManager.KoySecimGoster()` geçti (bkz. Mimari Kararları #20).
2. ✅ **Savaş mekaniğinin temeli kuruldu**: `Krallik` enum, `KoyData.Sahip`/`Garnizon`,
   `EtkinSavunmaHesapla` (çarpımsal sinerji), General'a "Saldır" emri (isyan bastırmayla aynı akış,
   `EtkinSavunma`'ya karşı savaşıyor), kazanınca köy+garnizon bize geçiyor (bkz. Mimari Kararları #21).
   Garnizon sorusu netleşti: **kalıcı** bir konsept, köylülerin doğal savunması + garnizon birlikte
   savunuyor (çarpımsal), tek seferlik değil.
3. ✅ Bir düşman köyü sahneye eklendi (`Sahip: Dusman`), haritada otomatik mavi görünüyor (elle renk
   ayarına gerek yok, bkz. Mimari Kararları #21).
4. ✅ Kingdom toplamlarından (Erzak/Nüfus/Yield'ler/Sadakat ortalaması) düşman köyleri de tamamen
   dışlandı (`BizeAitDegil` yardımcı fonksiyonu, bkz. Mimari Kararları #22). Düşman köyünün
   ekonomisi bilinçli olarak donuk (AI yok, kullanıcı onayladı).
5. ✅ İsyan Bastır/Değirmen köy listelerinde artık sadece bize ait köyler görünüyor, Saldır'da
   sadece düşman köyleri (bkz. Mimari Kararları #20-21).
6. ✅ Köy Bilgi Paneli, düşman köyünde farklı alanlar gösteriyor (sadece İsim/Savunma/Garnizon).
7. ⚠️ **Hover tooltip sistemi genişletildi ama YARIM KALDI** (bkz. Mimari Kararları #23-24,
   "Hover Bilgi Sistemi"): `StatTooltip` artık `Func<string>` tabanlı (genel/ölçeklenebilir),
   `TooltipUI` fareyi takip ediyor (yeni Input System + anchor/pivot referans noktası debug'ı uzun
   sürdü ama çözüldü). **Yarın devam:** kutunun arka planı (Image) + padding (Content Size Fitter +
   Vertical Layout Group) henüz eklenmedi, kutu hâlâ çirkin/okunaksız görünüyor.

Önceki oturumda tamamlananlar (özet): İsyandaki köylerin stokları toplamdan dışlandı, isyan bastırma
formülü Nüfus'a çevrildi, Manpower kayıp/dönüş sistemi.

Daha önceki oturumlarda tamamlananlar (özet): Building slot sistemi, isyan mekaniği (durum+zar+
debuff+görsel+bastırma emri), Köy Bilgi Paneli, gelir uygulama zamanlaması, Nüfus sistemi, StatsUI
renklendirme.

Henüz gündeme gelmemiş ama olası konular (öncelik sırası belirlenmedi):
- **Hover tooltip kutusunun görsel tasarımını bitirmek** (Image arka plan + padding) — en güncel
  yarım kalan iş, muhtemelen bir sonraki oturumun ilk maddesi.
- **Başkent fikri:** Kullanıcının önerisi — ileride bir "Başkent" (aslında normal bir `KoyData`,
  `bool Baskent` gibi bir bayrakla işaretli) olacak, "hangi köye gideceği belirsiz" kazanımlar
  (örn. genel `ErzakBaseGelir`) oraya birikip sonra oyuncu tarafından diğer köylere dağıtılacak.
  Kullanıcının planında 3 farklı harita elementi olacak: **köy, şehir, kale** (başkent bir şehir).
  **Henüz hiçbir kod yazılmadı**, sadece kabul edilen bir yön var.
- **Diplomasi:** Barış/İttifak, "Elçi" danışmanı, "Husumet" gibi bir stat — kullanıcı tarafından
  "savaş sistemi netleşmeden ele alınmayacak" diye işaretlenmişti, savaşın temeli artık kuruldu,
  istenirse bir sonraki adım bu olabilir.
- Diğer krallıkların da (savaş dışında) kendi köylerini büyütmesi/asker toplaması gibi gerçek bir
  AI — kullanıcı şimdilik istemiyor, ekonomisi donuk kalmaya devam edecek.
- `DanismanPaneli.cs`/`TahtOdasiTest.cs`'in gerçekten ölü kod olduğunu doğrulayıp temizlemek.
- Başka danışmanlar eklemek (artık tanıdık bir desen: NPCData + DialogueData + buton bağlama).
- Ansiklopedi'nin ne göstereceğine karar vermek.
- Harita köy etiketlerinin pozisyonunu da gerçek veriye bağlamak (şu an sadece tıklama/renk bağlı).
- Mektuplar/görev sistemini (istenirse) sıfırdan, farklı bir tasarımla ele almak.
- Kaydet/yükle (save/load) sistemi.

**Genel çalışma tarzı hâlâ geçerli:** her adımı küçük parçalara böl, kullanıcı test edip onaylamadan
bir sonrakine geçme.

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
