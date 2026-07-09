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
  bireysel yapalım") önce özelliği alt parçalara böl, kullanıcıya sırayı sor/onaylat, sonra tek tek
  uygula — bu yöntem hem "Şahsi Oda yeniden tasarımı" hem "köy hedefleme" işlerinde iyi çalıştı.
- Kullanıcı bazen bir özelliği kendi önerip sonra "zor olursa yapmayabiliriz, sadece fikir sundum"
  diyor — bu durumda dürüst bir değerlendirme yap (gerçekten zor mu, yoksa zaten elimizdeki bir
  teknikle mi çözülüyor), tercih ona kalsın.
- Hata geldiğinde önce sakinleştir ("bu normal, panik yok"), sonra sırayla kontrol ettir: Console'daki
  EN ESKİ/İLK hata ne diyor, dosya kaydedilmiş mi (Ctrl+S), doğru obje/script Inspector'a sürüklenmiş mi,
  Button'ın `On Click()` listesinde **doğru obje + doğru fonksiyon** seçili mi (eski/placeholder bir
  fonksiyon kalmış olabilir).
- Kullanıcı "Create → C# Script" ile MonoBehaviour şablonu oluşturduğunda kafası karışabiliyor;
  hangi script'in MonoBehaviour (sahneye yapıştırılacak) hangisinin düz veri sınıfı (`: MonoBehaviour`
  YOK) olduğunu her seferinde net söyle.
- Unity Editor ekran görüntülerinden Hierarchy/Inspector okuyup yönlendirmek çok işe yarıyor —
  kullanıcı ekran görüntüsü attığında oradaki obje isimlerini/alan değerlerini birebir okuyup
  ona göre spesifik adımlar ver (genel geçer talimat değil).
- Sahne dosyası (`SampleScene.unity`) bir YAML metni — bir buton gerçekten doğru fonksiyona mı bağlı
  diye kontrol etmek gerektiğinde, kullanıcıya sormadan önce dosyayı grep'leyip doğrulamak çok işe
  yarıyor (bkz. Mektuplar butonu kontrolü örneği).

---

## 2) OYUN KONSEPTİ

Diyalog ve karar temalı bir **"kral simülasyonu"** oyunu (Unity, C#, **2D**). Oyuncu bir kral,
ülkeyi yönetiyor.

### Stat'lar
- **Erzak**: Artık ayrı bir "genel/kingdom" deposu YOK. Tek gerçek kaynak, köylerin kendi Erzak
  stoklarının toplamı (bkz. "Köyler" bölümü ve Mimari Kararları #7).
- **Sadakat**: 0-100 arası, hem "genel" bir bileşeni var hem köylerin kendi Sadakat'ları — ekranda
  gösterilen "Sadakat", genel + köylerin ortalaması. **Sadakat asla bir seçeneği kilitlemiyor**
  (harcanan bir kaynak değil, sadece 0-100 arasına kelepçeleniyor).
- **Altın, Manpower**: Hâlâ tamamen "genel/kingdom-wide" — köylere bölünmüyor, harcanabilir kaynaklar
  (yetersizse ilgili seçenek/emir kilitleniyor).

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
ile görsel kilitleniyor, gerçek kısıtlama `OrderManager` seviyesinde). Emir verildiğinde **hiçbir
stat anında değişmiyor**, emir sadece "bekleyen emirler" listesine kaydediliyor.

#### Danışman diyalog akışı (Warband tarzı, kapı sistemi)
`DialogueData`/`DialogueNode`/`DialogueChoice` sistemi genişletildi: bir `DialogueChoice`, isteğe
bağlı bir **`VerilecekEmir`** (`OrderData`) taşıyabiliyor. Seçildiğinde otomatik `OrderManager.EmirEkle()`'ye
gönderiliyor. **General**: "Bir görevim var" → "Köy Yağmala" (hangi köy? diye sorup devam eden bir
node) VEYA "Asker Topla" (direkt emir). **İnşaatçı**: "Değirmen İnşa Et" seçilince artık **hangi
köyde yapılacağı, diyalog kutusunun İÇİNDE, kaydırılabilir bir köy listesiyle** soruluyor (bkz.
Mimari Kararları #9, `KoySecimPaneli.cs`) — köy seçilene kadar diyalog kutusu kapanmıyor.

### Köyler
Oyuncu ülkeyi tek bir "kingdom" olarak görse de, arka planda bunun kaynağı **köyler** (`KoyData`,
bkz. Script Envanteri). Her köyün kendi `Sadakat`, `Erzak`, `ErzakYield` (günlük Erzak artışı),
`AltinYield`'i var. **Kingdom Erzak'ı = tüm köylerin Erzak'ının toplamı, tam olarak** (ayrı bir
genel depo yok, matematiksel sapma imkansız). Kingdom Sadakat'ı = genel + köylerin ortalaması.

- **Köylü NPC'si artık gerçek bir köyü temsil ediyor.** Her gün, Erzak'ı 50'nin altında olan **her
  köy** kendi 0-50 arası zarını atıyor; zar köyün Erzak'ından yüksek çıkarsa o köy kendi Köylü'sünü
  taht odasına gönderiyor (aynı gün birden fazla köy tetiklenebilir, bkz. `DaySequencer.cs`).
  Diyalog metninde `{KOY}` yazan yer, o köyün adıyla değiştiriliyor. "Tamam al" (erzak ver) seçeneği:
  **Altın -20** (kingdom-wide, merkezi kaynaktan gidiyor) + **Erzak +20** (o köye gidiyor, köy hedefli)
  + **Sadakat +10** (o köyün kendi Sadakat'ı artıyor). "Yok git": **Sadakat -15** (o köyün Sadakat'ı,
  0'ın altına düşmüyor, seçenek asla kilitlenmiyor).
- **İnşaatçı/Değirmen artık belirli bir köyü hedefliyor.** Değirmen tamamlanınca genel `ErzakBaseGelir`
  yerine, oyuncunun seçtiği **o köyün** `ErzakYield`'i +1 artıyor.
- **Building slot/sınırı sistemi bilinçli olarak ERTELENDİ** (kullanıcı kararı) — henüz başlanmadı.

### Harita
Şahsi Oda'daki Harita objesine tıklayınca, ekranı kaplayan bir harita sekmesi açılıyor (sol üstteki
X'e basana kadar kapanmıyor). Fare tekerleğiyle zoom, sol tıkla sürükleyerek pan yapılabiliyor
(bkz. `HaritaKontrol.cs`). Zoom'un minimum seviyesi haritanın tam ekranı kapladığı nokta (daha fazla
uzaklaşılamıyor), sürükleme haritanın kenarları ekran dışına çıkmayacak şekilde sınırlı. Köylerin
isimleri şu an **elle/statik olarak** haritanın üzerine TextMeshPro etiketleriyle yerleştirildi —
gerçek `KoyData`'ya veri olarak BAĞLI DEĞİL (köy sayısı değişirse elle yeni etiket eklenmesi gerekir).

### Uyu Tuşu (Gece / Resolve)
Oyuncu "Uyu" tuşuna basınca döngü sona eriyor ve şunlar oluyor:
- `GameState.Gun` +1 artıyor.
- Her köyün Erzak'ı kendi `ErzakYield`'i kadar artıyor (`KoyYoneticisi.ErzagiGunlukArtir()`), genel
  `ErzakBaseGelir` de (varsa) köylere dağıtılarak uygulanıyor (bkz. Mimari Kararları #7).
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
- **Şansa bağlı sonuç** (örn. Köy Yağmalama): süre dolunca zar atılır, başarısız da olabilir.

### UI/Görsel Cila
- Stat değişikliklerini gösteren geçici bildirim kutusu fade in/out ile açılıp kapanıyor.
- Taht Odası'ndan Şahsi Oda'ya geçişte kısa bir siyah ekran (fade to black) geçiş efekti var.
- Diyalog kutusu "Good Pizza Great Pizza" tarzı: konuşma metni kutunun içinde, seçenekler kutunun
  altında, portre yanında. Seçenek butonlarının üzerine gelince maliyet bilgisi hover tooltip'te
  gösteriliyor (`SecenekTooltip.cs`, dinamik — diyaloğa göre değişiyor).

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

**7. Erzak nerede tutuluyor? (bu oturumda köklü değişiklik)**
Artık ayrı bir "genel Erzak" alanı **yok** — `GameState.Erzak` alanı tamamen kaldırıldı. Tek kaynak
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
genel `ErzakBaseGelir` yerine doğrudan o köyün `ErzakYield`'ini artırıyor.

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

---

## 4) SCRIPT ENVANTERİ

### Veri sınıfları (düz C#, MonoBehaviour DEĞİL)
- **`GameState.cs`** — `Gun`, `Sadakat`, `Altin`, `Manpower`, `ErzakBaseGelir`, `AltinBaseGelir`.
  **`Erzak` alanı KALDIRILDI** (bkz. Mimari Kararları #7). `StatDegerAl`/`StatDegistir`, Erzak için
  `KoyYoneticisi`'ye yönlendiriyor, Sadakat için genel+köy ortalamasını topluyor ve `Mathf.Clamp`
  ile 0-100 arasına sıkıştırıyor. `BaseGeliriUygula()`: her yeni günde `KoyYoneticisi.ErzakDegistir(ErzakBaseGelir)`
  + `Altin += AltinBaseGelir`.
- **`KoyData.cs`** *(yeni)* — Bir köyün "kimlik kartı": `Isim`, `Sadakat` (varsayılan 50), `Erzak`
  (varsayılan 20), `ErzakYield` (varsayılan 1), `AltinYield` (varsayılan 0). ScriptableObject DEĞİL.
- **`OrderData.cs`** — Ana constructor aynı (`DanismanTipi, EmirTuru, EtkilenenStat, ...`). **Yeni
  alanlar:** `KoySecimiGerekli` (bool), `HedefKoy` (nullable `KoyData`). **Yeni fonksiyon:**
  `KopyalaVeKoyAta(KoyData koy)` — tüm alanları kopyalayıp `HedefKoy`'u dolduran bir kopya döndürür
  (şablonu bozmamak için).
- **`SiraGirisi.cs`** *(yeni)* — `NPCData Npc` + `KoyData IlgiliKoy` (nullable). `DaySequencer`'ın
  ürettiği sıradaki bir "ziyaretin" hangi NPC ve (varsa) hangi köyle ilgili olduğunu taşıyan basit
  bir zarf sınıfı.
- **`DevamEdenEmir.cs`** — `OrderData Emir` + `int KalanGun`. Değişmedi.
- **`DaySequencer.cs`** — `SiradakiListeyiOlustur(state, koyluNpc, askerNpc, ayyasNpc)` artık
  `List<SiraGirisi>` döndürüyor. Köylü artık "her köy kendi zarını atar" mantığıyla ekleniyor
  (bkz. Mimari Kararları #10). `Gun == 10` kuralı (Ayyaş Adam) değişmedi.
- **`DayResolver.cs`** — Aynı genel yapı (tek günlük emirler anında, çok günlüler `devamEdenler`'e).
  **Değişen:** çok günlü emrin "başladı" mesajı kaldırıldı; `BaseGeliriEtkiler` dalı artık
  `HedefKoy` doluysa (ve `Isim`'i boşsa saymıyor) o köyün `ErzakYield`'ini artırıyor, değilse eskisi
  gibi genel `ErzakBaseGelir`'i artırıyor. Her stat değişiminde `BildirimYoneticisi.Bildirim(...)`
  çağrılıyor. Sonuç mesajları renkli: şansa bağlı başarı yeşil, başarısızlık kırmızı, garanti
  tamamlanma sarı.
- **`NPCData.cs`** — ScriptableObject. `ID`, `Isim`, `DialogueData Diyalog`, `Sprite Portre`.
  Danışmanlar için de kullanılıyor (`General_NPC.asset`, `Insaatci_NPC.asset`).
- **`DialogueData.cs`** — ScriptableObject. `DialogueChoice`: `SecenekMetni`, `SonrakiNodeID`,
  `List<StatEtkisi> StatEtkileri` (her biri `StatAdi`+`Miktar`), `OrderData VerilecekEmir`.

### MonoBehaviour'lar (sahnede bir GameObject'e eklenmiş script'ler)
- **`GameManager.cs`** — Singleton, `public GameState State`. Sadece veri kutusu tutucu.
- **`KoyYoneticisi.cs`** *(yeni)* — Singleton, `public List<KoyData> Koyler`. Fonksiyonlar:
  `ToplamErzak()`, `ErzakDegistir(miktar)` (hem artış hem azalışı köylere dağıtıyor, kalanı ilk
  köylere fazladan veriyor ki toplam tam tutsun), `ErzagiGunlukArtir()` (her köyü kendi `ErzakYield`'i
  kadar artırır), `ToplamErzakYieldi()`, `ToplamAltinYieldi()`, `OrtalamaSadakat()`.
- **`OrderManager.cs`** — Değişmedi. `EmirEkle`, `DanismanKullanildiMi`, `YeniDongueBasla`.
- **`DialogueManager.cs`** — `NpcIsimText`, `NpcSozuText`, `SecenekButon1/2Text`, `PortreImage`,
  `SecenekButon1/2Buton`, `Orders`, `DiyalogKutusuKok`. **Yeni:** `aktifKoy` (nullable `KoyData`),
  `DiyalogBaslat`'ın son parametresi `KoyData ilgiliKoy = null`. `NodeGoster()`: `{KOY}` yer
  tutucusunu `aktifKoy.Isim` ile değiştiriyor, her node gösteriminde `SecenekButon1Buton`'u tekrar
  aktif ediyor (köy seçimi sırasında gizlendiği için). `GuncelDeger`/`DegeriUygula`: Erzak/Sadakat'ı
  `aktifKoy` doluysa doğrudan köye, değilse `GameManager.State` üzerinden kingdom-wide uyguluyor.
  `SecenekKarsilanabilirMi`: Sadakat'ı hiç kontrol etmiyor (asla kilitlemiyor), `VerilecekEmir`
  maliyetini de kontrol ediyor. `SecenekUygula`: `VerilecekEmir.KoySecimiGerekli` true ise emri hemen
  eklemiyor, `KoySecimPaneli.KoySec(callback)` açıp seçim yapılana kadar diyaloğu açık tutuyor (normal
  seçenek butonları gizleniyor), seçilince `KopyalaVeKoyAta` ile emri ekleyip `DiyalogBitir()` çağırıyor.
  `MaliyetMetniAl(index)`: seçeneğin `VerilecekEmir` maliyetini "-X StatAdi" formatında döndürüyor.
- **`SecenekTooltip.cs`** *(yeni)* — Diyalog seçenek butonlarına eklenen hover script'i (`IPointerEnterHandler`/
  `IPointerExitHandler`). `Dialog` (DialogueManager) + `SecenekIndex` (0/1) alır, `Dialog.MaliyetMetniAl`'i
  çağırıp `TooltipUI.Goster`/`Gizle` ile gösterir/gizler.
- **`KoySecimPaneli.cs`** *(yeni)* — Singleton. `Panel` (GameObject) + `ButonSablonu` (GameObject,
  Awake'te gizleniyor). `KoySec(Action<KoyData> callback)`: `KoyYoneticisi.Koyler`'daki her köy için
  şablon butonu `Instantiate` ile çoğaltıp isim yazıyor ve `Button.onClick.AddListener(...)` ile
  (Inspector'dan değil, KOD içinden) tıklama olayını bağlıyor — çünkü köy sayısı dinamik, Inspector'da
  elle bağlanamaz. Panel, `DiyalogAlani`'nın içine yerleştirildi, `Scroll Rect` + `Viewport` (Rect
  Mask 2D) + `Content` (Vertical Layout Group, `Control Child Size`+`Child Force Expand` Width
  işaretli) ile kaydırılabilir.
- **`HaritaKontrol.cs`** *(yeni)* — Harita içeriğinin (`Icerik`, RectTransform) sürüklenmesini/
  yakınlaştırılmasını yönetir. `IBeginDragHandler`/`IDragHandler`: `RectTransformUtility.
  ScreenPointToLocalPointInRectangle` ile Canvas modundan/ölçeğinden bağımsız, imlecin gerçek yerel
  konumunu hesaplayıp fark alır (Canvas `scaleFactor` farkından kaynaklanan "kayma" bugı bu şekilde
  çözüldü). `IScrollHandler`: zoom (fare tekerleği). `minZoom`, `Awake`'te viewport/içerik oranından
  otomatik hesaplanıyor (harita tam ekranı kaplayana kadar uzaklaşılabiliyor, sabit sayı değil).
  `SinirlaKonum()`: sürükleme sonrası haritanın kenarları ekran dışına taşmayacak şekilde
  `anchoredPosition`'ı kelepçeliyor.
- **`GunUI.cs`** — Ekranın üstünde "Gun X" gösteriyor. Değişmedi.
- **`BildirimYoneticisi.cs`** — Singleton. `Bildirim(statAdi, miktar)`: şablonu çoğaltıp fade in
  (`CanvasGroup`) → bekle → fade out → `Destroy`. Değişmedi (bu oturumda daha önce eklenmişti).
- **`DayCycleManager.cs`** — Singleton, state machine'in kalbi. `gunlukSira` artık `List<SiraGirisi>`.
  `YeniGuneBasla()`: `BaseGeliriUygula()` + `KoyYoneticisi.ErzagiGunlukArtir()` + Altın base geliri,
  sonra `DaySequencer` ile sıra oluşturuluyor. `SiradakiNpcyiGoster()`: `Dialog.DiyalogBaslat(npc.Diyalog,
  npc.Portre, npc.Isim, false, girisi.IlgiliKoy)` — artık ilgili köyü de iletiyor. `UyuyaBas()`:
  `Gun`'ı +1 artırıyor, Resolve'a geçiyor, DayResolver'ı çalıştırıyor.
- **`DanismanCagir.cs`** — Kapıdan açılan danışman listesindeki butonlara bağlı. **`MaliyeBakani`
  alanı/fonksiyonu `Insaatci`/`InsaatciCagir()` olarak DEĞİŞTİRİLDİ** (Maliye Bakanı konsepti terk
  edildi). `GeneralCagir()`, `InsaatciCagir()`: ilgili danışmanın diyaloğunu `danismanDiyalogu=true`
  ile başlatıyor.
- **`DanismanButon.cs`** — Danışman kullanılınca ilgili butonun görsel kilitlenmesi. Artık HEM eski
  (kaldırılan) 4-buton sisteminde HEM kapıdaki General/İnşaatçı butonlarında **ortak** kullanılıyor —
  **silinmemeli**, iki sistem paylaşıyor.
- **`StatsUI.cs`** — Sol üstteki canlı stat göstergesi. Erzak artık `KoyYoneticisi.ToplamErzak()`'tan
  okunuyor (genel bileşen yok), Sadakat genel+köy ortalaması.
- **`TooltipUI.cs`** + **`ButtonTooltip.cs`** — Hover bilgi kutusu sistemi. `TooltipUI` hem eski
  statik (`ButtonTooltip`) hem yeni dinamik (`SecenekTooltip`) hover sistemleri tarafından paylaşılıyor.
- **`EkranGecisi.cs`** — Singleton, siyah ekran geçişi. Değişmedi.
- **`OdaEtkilesimTest.cs`** — `KapiTiklandi()`, `AnsiklopediTiklandi()` (hâlâ placeholder),
  `HaritaTiklandi()`: artık gerçekten `HaritaEkrani.SetActive(true)` çağırıyor (eskiden placeholder
  log basıyordu). **`MektuplarTiklandi()` fonksiyonu tamamen KALDIRILDI.**

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
- Danışman diyalog akışı (kapı sistemi): General (Köy Yağmala + Asker Topla) ve İnşaatçı (Değirmen
  İnşa Et) uçtan uca test edildi, danışman başına döngüde 1 kullanım kısıtlaması ortak `DanismanButon.cs`
  ile çalışıyor.
- **Mektuplar/görev sistemi tamamen kaldırıldı** (kod + asset'ler silindi, ilgili Unity objeleri
  kullanıcı tarafından silindi) — kullanıcı henüz nasıl bir tasarım istediğine karar vermedi.
- Anlık bildirim (toast) sistemi, hover tooltip sistemi (hem eski statik hem yeni dinamik diyalog
  seçenek maliyeti gösterimi, stuck-tooltip bugı düzeltildi).
- **Harita sistemi**: tam ekran, sürüklenebilir (Canvas ölçeğinden bağımsız, `RectTransformUtility`
  ile), zoom sınırlı (min = ekranı tam kaplama, max ayarlanabilir), pozisyon kenarlara kilitleniyor.
  Köy isimleri elle yerleştirildi (veri-bağlı değil).
- **Köy sistemi**: `KoyData`/`KoyYoneticisi` kuruldu, kingdom Erzak = köylerin toplamı (tam, sapma
  imkansız), Sadakat = genel + köy ortalaması (0-100 kelepçeli, hiçbir seçeneği kilitlemiyor).
- **Köylü NPC'si artık gerçek bir köyü temsil ediyor**: her gün her köy kendi zarını atıyor, diyalog
  metninde köy adı geçiyor, verdiği/aldığı Erzak+Sadakat doğrudan o köyü etkiliyor, Altın kingdom-wide.
- **İnşaatçı/Değirmen artık hangi köyde yapılacağını soruyor** (diyalog kutusunun içinde, kaydırılabilir
  bir listeyle — ayrı bir popup değil), tamamlanınca sadece o köyün `ErzakYield`'i artıyor.

⚠️ Yarım kaldı / doğrulanmadı / bilinçli ertelendi:
- `DanismanPaneli.cs` ve `TahtOdasiTest.cs` muhtemelen artık ölü kod, kontrol edilip silinebilir.
- Ansiklopedi hâlâ tamamen boş placeholder, hiç tasarlanmadı.
- Harita'daki köy etiketleri gerçek `KoyData`'ya bağlı değil (elle yerleştirildi) — yeni köy eklenirse
  elle yeni etiket eklenmesi gerekiyor; "köye tıklayınca bilgi göster" gibi bir özellik istenirse bu
  bağlantı kurulmalı.
- **Building slot/sınırı sistemi bilinçli olarak ERTELENDİ** (kullanıcı kararı), henüz hiç başlanmadı.
- Şu an sadece **General** ve **İnşaatçı** var, başka danışman yok (Maliye Bakanı konsepti tamamen terk edildi).
- Mektuplar/görev sistemi silindiği için, "oyuncuya görev/istek gelen" bir mekanik yok — istenirse
  sıfırdan, farklı bir tasarımla ele alınabilir.

---

## 6) SIRADAKİ ADIMLAR

"Köyleri gerçek/bireysel yapalım" planı büyük ölçüde tamamlandı:
1. ✅ Altyapı (diyalog etkilerinin belirli bir köyü hedefleyebilmesi) + Köylü NPC'sinin gerçek,
   zar-bazlı bir köyden gelmesi — kuruldu, test edildi.
2. ✅ İnşaatçı/Değirmen'in hangi köyde yapılacağının seçilmesi (diyalog içi, kaydırılabilir liste) —
   kuruldu, test edildi.
3. ⬜ **Building slot/sınırı sistemi — bilinçli olarak ERTELENDİ, henüz başlanmadı.** Gündeme
   gelirse kullanıcıyla kapsamı konuşulmalı (her köyde kaç slot, hangi bina tipleri vs. hiç konuşulmadı).

Diğer, henüz gündeme gelmemiş ama olası konular (öncelik sırası belirlenmedi):
- `DanismanPaneli.cs`/`TahtOdasiTest.cs`'in gerçekten ölü kod olduğunu doğrulayıp temizlemek.
- Başka danışmanlar eklemek (artık tanıdık bir desen: NPCData + DialogueData + buton bağlama).
- Ansiklopedi'nin ne göstereceğine karar vermek.
- Harita köy etiketlerini gerçek veriye bağlamak, köye tıklayınca detay/bilgi gösterme.
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
  (bkz. `KoySecimPaneli.cs`).
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
