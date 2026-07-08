//# KRAL SİMÜLASYONU — Proje Bağlamı

> Bu dosya, bu Unity projesinde daha önceki bir Claude sohbetinde yapılan çalışmanın özetidir.
> Amaç: yeni bir sohbet/oturum bu dosyayı okuduğunda, sanki aynı sohbetten kaldığı yerden
> devam ediyormuş gibi bağlamı hızlıca kavraması.
>
> **Yeni oturuma not:** Bu dosya bir yol haritasıdır, ama gerçek kod her zaman "zemin gerçeği"dir.
> Özellikle "Şu Anki Durum" bölümündeki bazı Unity sahne bağlantıları (Inspector'da hangi objenin
> hangi alana sürüklendiği) bu özeti hazırlarken doğrudan görülmedi, sadece ekran görüntülerinden
> takip edildi — işe başlamadan önce ilgili script dosyalarını oku/incele ve Unity'de Hierarchy'yi
> gözden geçir, bu özetle karşılaştır. **Özellikle "Sıradaki Adımlar" bölümündeki yarım kalan iş
> (Mektuplar butonu bağlantısı) yeni oturumun ilk kontrol etmesi gereken şey.**

---

## 1) TAKIM VE ANLATIM TARZI (HER ZAMAN UYGULANMALI)

- 3 kişilik küçük bir ekibiz, proje yönetimi ve Unity/C# konusunda ileri seviye değiliz, sıfırdan öğreniyoruz.
- Unity'ye genel hakimiyet var, ama **C# bilgisi gelişmiş değil.**
- Açıklamalar HER ZAMAN basit, az teknik jargonlu, adım adım olmalı — sanki hiç bilmiyormuşuz gibi anlat.
- Yeni bir C# kavramı ilk kez geçtiğinde (fonksiyon/parametre, switch, enum, constructor, HashSet,
  foreach, ScriptableObject, Singleton, SetActive, CanvasGroup, lambda/`Action`, vs.) mutlaka kısa
  (1-3 cümle), günlük hayattan bir benzetmeyle açıklanmalı. Örnek benzetmeler daha önce işe yaradı:
  fonksiyon = matematikteki çok değişkenli fonksiyon f(x,y); GameManager.Instance.State.Erzak zinciri
  = apartman → daire → çekmece; CanvasGroup = bir objenin görünürlüğünü tek sayıyla (0-1) ayarlayan
  düğme; lambda (`() => ...`) = "şunu yap" diye isimsiz, paketlenmiş bir tarif, hemen değil doğru
  zamanda çalıştırılsın diye başka bir fonksiyona gönderiliyor.
- **Küçük adımlarla ilerle.** Her seferinde tek bir parça ver, kullanıcı deneyip "oldu" ya da hata
  paylaşana kadar bir sonrakine geçme. Büyük, çok-parçalı adımlar kullanıcıyı bunalttı ve bir kez
  (ekonomi sistemi eklerken) "korkup her şeyi geri almasına" yol açtı — bundan kaçın, mümkün olan
  en küçük, test edilebilir parçalara böl. Büyük bir özellik isteği geldiğinde (örn. "Şahsi Oda'yı
  Papers Please tarzı yapalım") önce özelliği alt parçalara böl, kullanıcıya sırayı sor/onaylat,
  sonra tek tek uygula — bu yöntem "Şahsi Oda yeniden tasarımı" işinde iyi çalıştı.
- Hata geldiğinde önce sakinleştir ("bu normal, panik yok"), sonra sırayla kontrol ettir: Console'daki
  EN ESKİ/İLK hata ne diyor, dosya kaydedilmiş mi (Ctrl+S), doğru obje/script Inspector'a sürüklenmiş mi,
  Button'ın `On Click()` listesinde **doğru obje + doğru fonksiyon** seçili mi (eski/placeholder bir
  fonksiyon kalmış olabilir — bu tam olarak "Mektuplar butonu çalışmıyor" sorununun sebebiydi).
- Kullanıcı "Create → C# Script" ile MonoBehaviour şablonu oluşturduğunda kafası karışabiliyor;
  hangi script'in MonoBehaviour (sahneye yapıştırılacak) hangisinin düz veri sınıfı (`: MonoBehaviour`
  YOK) olduğunu her seferinde net söyle.
- Unity Editor ekran görüntülerinden Hierarchy/Inspector okuyup yönlendirmek çok işe yarıyor —
  kullanıcı ekran görüntüsü attığında oradaki obje isimlerini/alan değerlerini birebir okuyup
  ona göre spesifik adımlar ver (genel geçer talimat değil).

---

## 2) OYUN KONSEPTİ

Diyalog ve karar temalı bir **"kral simülasyonu"** oyunu (Unity, C#, **2D**). Oyuncu bir kral,
ülkeyi yönetiyor.

### Stat'lar
Oyunda bir sürü "stat" (sayı) var: Erzak, Sadakat, Altın, Manpower gibi kaynaklar. Bunlar oyundaki
olayları belirliyor, verilen kararlara göre değişiyor.

### Döngü (Cycle) Sistemi
Oyun "döngü" (cycle) sistemiyle ilerliyor. **Her döngü = 1 gün** (`GameState.Gun`, ekranın üstünde
`GunUI.cs` ile "Gun X" olarak gösteriliyor), 2 bölümden oluşuyor:

**1. TAHT ODASI**
Sıradaki NPC'ler tek tek gelip oyuncuyla diyalog kuruyor. Diyalog seçimleri stat değişikliklerine
yol açabiliyor (bir seçenek artık **birden fazla stat'ı aynı anda** etkileyebiliyor, bkz. Script
Envanteri → `DialogueData.cs`). Sıra bitince, kısa bir **siyah ekran geçişiyle** (bkz. `EkranGecisi.cs`)
otomatik olarak 2. bölüme geçiliyor.

**2. ŞAHSİ ODA**
Kralın (oyuncunun) gözünden, önünde bir masa olan bir oda (Papers Please tarzı POV). Masanın
üzerinde/önünde 3 etkileşim objesi var: **Ansiklopedi** (kitap), **Harita** (ortada), **Mektuplar**.
Arka planda bir **Kapı** var. Kapıya tıklayınca hangi danışmanları (General, Maliye Bakanı vb.)
çağırabileceğini gösteren bir liste açılıyor; listeden birine tıklayınca o danışman "içeri girip"
Warband tarzı dallanan bir diyalog başlatıyor (bkz. aşağıdaki "Danışman diyalog akışı"). Mektuplar
objesine tıklayınca da benzer şekilde bir liste açılıp, seçilen mektubun "kabul et / reddet" diyaloğu
başlıyor. **Eski "4 sabit danışman butonu" sistemi (`DanismanPaneli.cs`) bu kapı+diyalog sistemiyle
değiştiriliyor** (henüz `DanismanPaneli.cs` silinmedi, ikisi şu an sahnede aynı anda duruyor — bkz.
Sıradaki Adımlar). Her danışman **döngü başına sadece 1 kez** kullanılabiliyor (kullanılınca o döngü
için kilitleniyor, bu kısıtlama `OrderManager` seviyesinde uygulanıyor). Emir verildiğinde **hiçbir
stat anında değişmiyor**, emir sadece "bekleyen emirler" listesine kaydediliyor.

#### Danışman diyalog akışı (Warband tarzı, kapı sistemi)
Mevcut `DialogueData`/`DialogueNode`/`DialogueChoice` sistemi (NPC diyaloglarında kullanılan aynı
yapı) genişletildi: bir `DialogueChoice`, artık isteğe bağlı olarak bir **`VerilecekEmir`**
(`OrderData`) taşıyabiliyor. Oyuncu o seçeneği seçtiğinde, dolu bir `VerilecekEmir` varsa otomatik
olarak `OrderManager.EmirEkle()`'ye gönderiliyor — yani maliyet kontrolü, danışman kilidi gibi her
şey zaten var olan sistemle otomatik işliyor. Bu sayede "General → Bir görevim var → Köy Yağmala →
Hangi köy? → X Köyü" gibi çok adımlı bir dallanma, aynı NPC diyalog node/choice yapısıyla kurulabiliyor,
sadece son seçeneğin `VerilecekEmir`'i doldurulmuş oluyor. **Mektuplar da aynı mekanizmayı kullanıyor**
(kabul/red seçenekleri stat etkisi ve/veya emir taşıyabiliyor) — mektup için ayrı bir veri tipi
icat edilmedi, bilinçli bir tercih. **Danışmanlar da ayrı bir veri tipi değil, doğrudan `NPCData`**
(İsim, Diyalog, Portre) olarak tutuluyor.

### Uyu Tuşu (Gece / Resolve)
Oyuncu "Uyu" tuşuna basınca döngü sona eriyor ve şunlar oluyor:
- `GameState.Gun` +1 artıyor.
- Bekleyen emirlerin her biri için zar atılıyor (rastgele + ilgili stat'a bağlı başarı ihtimali).
- Başarı/başarısızlık belirleniyor, stat'lar buna göre güncelleniyor (sonuç mesajları renkli:
  başarı yeşil, başarısızlık kırmızı, garanti tamamlanma sarı — bkz. `DayResolver.cs`).
- Bazı olaylar oyuncu kararından bağımsız, tamamen rastgele de tetiklenebiliyor.
- Bir sonraki günün taht odası sırası (hangi NPC'lerin geleceği) o anki stat'lara göre (örn. düşük
  erzak → kızgın köylü ihtimali artar) VE sabit hikaye olaylarına göre (örn. 10. gün gelen Ayyaş Adam)
  belirlenip bir liste halinde saklanıyor.
- Sonuçlar (inşaat tamamlandı, akın başarılı oldu vb.) bir sonraki sabah taht odasında bir
  elçi/ulak karakteri üzerinden oyuncuya aktarılıyor.

### Çok Günlü Emirler
İnşaat gibi bazı emirlerin sonucu birden fazla döngü sürebiliyor (örn. değirmen birkaç döngü
sonra tamamlanıyor). Bunun içinde de bir ayrım var:
- **Garanti sonuç** (örn. Değirmen İnşası): süre dolunca kesin tamamlanır, zar atılmaz.
- **Şansa bağlı sonuç** (örn. Köy Yağmalama): süre dolunca zar atılır, başarısız da olabilir.

### Ekonomi
**Altın** ve **Manpower** stat'ları var. Değirmen inşası Altın harcıyor; Asker Topla da Altın
harcıyor; Köy Yağmala Manpower harcıyor (emir verilirken maliyet anında düşülüyor). Erzak ve
Altın'ın bir **"base değeri"** (pasif, otomatik döngü başı geliri) var. Değirmen inşası tamamlanınca
doğrudan Erzak'a eklemek yerine, Erzak'ın base değerini kalıcı olarak artırıyor.

### UI/Görsel Cila
- Stat değişikliklerini gösteren geçici bildirim kutusu **fade in / fade out** ile açılıp kapanıyor
  (aniden belirip kaybolmuyor).
- Taht Odası'ndan Şahsi Oda'ya geçişte kısa bir **siyah ekran (fade to black)** geçiş efekti var.
- Diyalog kutusu "Good Pizza Great Pizza" tarzı: konuşma metni kutunun **içinde**, seçenekler
  kutunun **altında** (dışında), konuşan karakterin portresi kutunun **yanında**.

---

## 3) MİMARİ KARARLARI

**1. Stat'lar nerede tutuluyor?**
Merkezi bir **`GameState`** sınıfı (düz C# class, `[Serializable]`, **ScriptableObject DEĞİL** —
çünkü ScriptableObject Editor'da eski değerleri hatırlayabiliyor, save/load kontrolü zorlaşıyor).
`GameState`, **`GameManager`** (MonoBehaviour, Singleton) içinde `public GameState State` olarak
tutuluyor. Her yerden `GameManager.Instance.State.Erzak` gibi erişiliyor.

**2. NPC'ler (ve danışmanlar) GameObject mi, veri mi?**
**Veri olarak** tutuluyor: `NPCData` bir ScriptableObject (ID, Isim, Diyalog referansı, Portre).
Sahnede NPC/danışman başına ayrı GameObject YOK — tek bir görsel "sahne" (diyalog kutusu) var,
hangi NPC'nin/danışmanın sırası gelirse o kişinin verisiyle dolduruluyor. **Danışmanlar (General,
Maliye Bakanı vb.) için ayrı bir veri tipi açılmadı — `NPCData` doğrudan yeniden kullanıldı**, çünkü
ihtiyaç duyulan alanlar (İsim, Diyalog, Portre) zaten birebir aynıydı.

**3. Taht odası sırası nerede oluşturuluyor?**
**`DaySequencer`** (düz C# class, MonoBehaviour değil). `SiradakiListeyiOlustur(state, koyluNpc,
askerNpc, ayyasNpc)` fonksiyonu, stat'lara bakıp (düşük Erzak → köylü ihtimali artar) VE `Gun == 10`
gibi sabit kurallara göre (Ayyaş Adam gibi hikaye NPC'leri) bir sıra üretip `List<NPCData>` olarak
döndürüyor. Yeni sabit-günlü bir NPC eklenecekse bu fonksiyona benzer şekilde yeni parametre +
`Gun == X` kuralı eklenmeli.

**4. Bekleyen emirler ve zar atma nerede?**
- **`OrderManager`** (MonoBehaviour): `List<OrderData> BekleyenEmirler` + `HashSet<string>
  KullanilanDanismanlar` (danışman başına döngüde 1 kullanım kısıtlaması burada uygulanıyor).
- **`DayResolver`** (düz C# class): zar atma + sonuç hesaplama mantığının tamamı burada.

**5. Döngü akışı nasıl kuruldu?**
Basit bir **state machine**, `enum GunAsamasi { TahtOdasi, SahsiOda, Resolve }` ile.
**`DayCycleManager`** (MonoBehaviour, Singleton) bu enum'u tutuyor, `AsamaDegistir()` fonksiyonu
ilgili UI panelini `SetActive(true/false)` ile açıp kapatıyor. Akış: TahtOdasi (sıra biter, artık
`EkranGecisi` ile siyah ekran geçişi yaşanıyor) → SahsiOda (Uyu'ya basılır) → Resolve (zar atılır,
yeni gün hazırlanır) → tekrar TahtOdasi.

**6. Diyalog kutusu Taht Odası ile Şahsi Oda arasında nasıl paylaşılıyor?**
Diyalog kutusuyla ilgili **her şeyi** (konuşma metni, portre, isim, seçenek butonları) barındıran
`DiyalogAlani` objesi, artık `TahtOdasiPanel`'in **içinde değil**, doğrudan `Canvas`'ın altında,
`TahtOdasiPanel` ve `SahsiOdaPanel` ile **kardeş** seviyede duruyor. Görünürlüğü artık panel
`SetActive`'ine bağlı değil, `DialogueManager.DiyalogKutusuKok` üzerinden **elle** açılıp kapanıyor
(`DiyalogBaslat` içinde açılıyor, `DiyalogBitir` içinde kapanıyor). Bu sayede aynı diyalog kutusu hem
Taht Odası'ndaki NPC'ler için hem Şahsi Oda'daki danışman/mektup diyalogları için kullanılabiliyor.
Diyaloğun **nereden geldiğine göre farklı davranması** gerekiyor (NPC bitince sıradaki NPC'ye geç,
danışman/mektup bitince sadece kutuyu kapat) — bu ayrım `DiyalogBaslat`'ın son parametresi
`danismanDiyalogu` (bool, varsayılan `false`) ile yapılıyor, bkz. Script Envanteri → `DialogueManager.cs`.

---

## 4) SCRIPT ENVANTERİ

### Veri sınıfları (düz C#, MonoBehaviour DEĞİL)
- **`GameState.cs`** — `Erzak`, `Sadakat`, `Altin`, `Manpower`, `Gun` (int, 1'den başlıyor),
  `ErzakBaseGelir`, `AltinBaseGelir` (int, her yeni günde otomatik kazanılan miktar). Fonksiyonlar:
  `StatDegistir(statAdi, miktar)`, `StatDegerAl(statAdi)`, `BaseGeliriUygula()` (her yeni günde
  Erzak/Altın'a base geliri ekler), `BaseGeliriArtir(statAdi, miktar)` (bir stat'ın base gelirini
  kalıcı artırır). *(Eskiden `OrduGucu` diye ayrı bir stat vardı, birleştirildi, artık sadece
  `Manpower` var.)*
- **`OrderData.cs`** — Bir emrin tarifi. Ana constructor: `DanismanTipi, EmirTuru, EtkilenenStat,
  BasariliDegisim, BasarisizDegisim, BasariSansi, ToplamSure, SonucSansaBagli` (zorunlu) +
  `MaliyetStat="", MaliyetMiktar=0` (opsiyonel, emir verilirken anında düşülüyor) +
  `BaseGeliriEtkiler=false, BaseGeliriStat="", BaseGeliriMiktar=0` (opsiyonel, garanti tamamlanan emir
  normal stat artışı yerine ilgili stat'ın base gelirini kalıcı artırabiliyor). **Ayrıca parametresiz
  (boş) bir constructor eklendi** — sırf Unity Inspector'ın bu tipi bir `DialogueChoice`'un içinde
  elle doldurulabilir şekilde göstermesi için gerekliydi.
- **`DevamEdenEmir.cs`** — `OrderData Emir` + `int KalanGun`. Çok günlü, hâlâ devam eden emirlerin
  "şu an ne durumda" bilgisini taşıyor.
- **`DaySequencer.cs`** — bkz. Mimari Kararları #3.
- **`DayResolver.cs`** — `SonucMesajlariniOlustur(GameState, List<OrderData> emirler,
  List<DevamEdenEmir> devamEdenler, List<string> mesajListesi)`: tek günlük emirleri anında zar atıp
  sonuçlandırıyor; çok günlü emirleri `devamEdenler`'e ekleyip her çağrıda 1 gün ilerletiyor; süresi
  dolanları garanti tamamlıyor ya da (`SonucSansaBagli` true ise) zar atıp sonuçlandırıyor. Sonuç
  mesajları renkli: şansa bağlı başarı `<color=green>`, başarısızlık `<color=red>`, garanti tamamlanma
  `<color=yellow>`, "devam ediyor, X gün kaldı" mesajı renksiz.
- **`NPCData.cs`** — ScriptableObject. `ID`, `Isim`, `DialogueData Diyalog`, `Sprite Portre`. Doğrulandı,
  sorun yok. **Artık NPC'ler dışında danışmanlar için de kullanılıyor** (örn. `General_NPC.asset`).
- **`DialogueData.cs`** — ScriptableObject. `DialogueID`, `List<DialogueNode> Nodler`. `DialogueNode`:
  NodeID, NPCSozu, `List<DialogueChoice> Secenekler`. `DialogueChoice`: SecenekMetni, SonrakiNodeID,
  `List<StatEtkisi> StatEtkileri` (her biri `StatAdi`+`Miktar` — bir seçenek birden fazla stat'ı aynı
  anda etkileyebiliyor), **ve `OrderData VerilecekEmir`** (opsiyonel — `DanismanTipi` boş değilse, bu
  seçenek seçildiğinde otomatik `OrderManager.EmirEkle()` çağrılıyor).

### MonoBehaviour'lar (sahnede bir GameObject'e eklenmiş script'ler)
- **`GameManager.cs`** — Singleton, `public GameState State`. Sadece veri kutusu tutucu.
- **`OrderManager.cs`** — bkz. Mimari Kararları #4. `EmirEkle`, `DanismanKullanildiMi`, `YeniDongueBasla`.
- **`DialogueManager.cs`** — Aktif diyaloğu ekrana basıyor: `NpcIsimText`, `NpcSozuText`,
  `SecenekButon1Text`, `SecenekButon2Text`, `PortreImage`, `SecenekButon2` (GameObject — tek seçenekli
  diyaloglarda gizleniyor), `SecenekButon1Buton`/`SecenekButon2Buton` (`SecenekKarsilanabilirMi`
  kontrolüne göre `interactable` ayarlanıyor — bir stat'ı eksiye düşürecek seçenek kilitleniyor).
  **Yeni alanlar (bu oturumda eklendi):** `OrderManager Orders` (seçenekteki `VerilecekEmir`'i iletmek
  için), `GameObject DiyalogKutusuKok` (tüm diyalog kutusu grubunu — `DiyalogAlani`'yı — açıp kapatmak
  için, bkz. Mimari Kararları #6). `DiyalogBaslat(DialogueData diyalog, Sprite portre, string isim,
  bool danismanDiyalogu = false)`: `danismanDiyalogu` true verilirse, diyalog bitince (`DiyalogBitir`)
  `DayCycleManager.SiradakiyeGec()` **çağrılmıyor**, sadece kutu kapanıp mevcut sahnede (Şahsi Oda'da)
  kalınıyor — Taht Odası NPC akışı için hâlâ `false` (varsayılan) kullanılıyor. `SecenekUygula()`: her
  `StatEtkisi` için hem `StatDegistir` hem `BildirimYoneticisi.Bildirim` çağırıyor, `VerilecekEmir`
  doluysa `Orders.EmirEkle()` çağırıyor, sonra sıradaki node'a geçiyor ya da `DiyalogBitir()`'i çağırıyor.
- **`GunUI.cs`** — Ekranın üstünde `GameManager.Instance.State.Gun`'ı "Gun X" olarak gösteriyor.
- **`BildirimYoneticisi.cs`** — Singleton. `Bildirim(statAdi, miktar)`: bir bildirim şablonunu
  (`BildirimSablonu`) `Instantiate` ile çoğaltıp (+yeşil/-kırmızı renkli) gösteriyor. **Bu oturumda
  güncellendi:** artık `CanvasGroup` ile fade in (`FadeSuresi`, varsayılan 0.4s) → `GosterimSuresi`
  bekle → fade out → `Destroy` akışı var (eskiden anında `SetActive`/`Destroy` yapıyordu). Test edildi.
- **`DayCycleManager.cs`** — Singleton, state machine'in kalbi. `GunlukSira` + `suankiNpcIndex` ile
  Taht Odası kuyruğunu yönetiyor. `YeniGuneBasla()`: en başta `BaseGeliriUygula()`, sonra
  `DaySequencer` ile sıra oluşturuluyor (**bu satırın fonksiyonun EN BAŞINDA olması şart**), önceki
  geceden sonuç varsa `ElciDiyaloguOlustur` ile "Ulak" NPC'si ekleniyor. `SiradakiNpcyiGoster()`: sıra
  bitince artık direkt `AsamaDegistir(SahsiOda)` çağırmıyor, bunun yerine
  `StartCoroutine(EkranGecisi.Instance.KararipAcil(() => AsamaDegistir(GunAsamasi.SahsiOda)))` — asama
  değişimi ekran tam siyahken gerçekleşiyor, geçiş görünmüyor. `UyuyaBas()`: `Gun`'ı +1 artırıyor,
  Resolve'a geçiyor, DayResolver'ı çalıştırıyor, OrderManager'ı sıfırlıyor, YeniGuneBasla'yı çağırıyor.
- **`DanismanPaneli.cs`** — (eski adı `ManagerTest.cs`). **Eskiyen sistem** — bkz. Sıradaki Adımlar.
  Şahsi Oda'daki 4 sabit butona (İnşaatçı, Askerbaşı, Asker Topla) sabit `OrderData`'lar oluşturup
  `OrderManager.EmirEkle`'ye gönderiyor. Hâlâ sahnede duruyor ve çalışıyor, ama artık aynı işi yapan bir
  alternatif (kapı → danışman diyaloğu → `VerilecekEmir`) de var — kapı sistemi tamamlanıp içerik
  dolunca bu eski panel kaldırılacak.
- **`StatsUI.cs`** — Sol üstteki canlı stat göstergesi (Erzak, Altın, Manpower, Sadakat), Erzak/Altın
  yanında `<sup>` ile base gelir gösteriyor.
- **`TooltipUI.cs`** + **`ButtonTooltip.cs`** — Hover bilgi kutusu sistemi (maliyet gösterimi).
- **`DanismanButon.cs`** — Danışman kullanılınca ilgili butonun görsel kilitlenmesi. Eski `DanismanPaneli`
  sistemine ait, o kaldırılınca bu da muhtemelen kaldırılacak.
- **`EkranGecisi.cs`** *(yeni, bu oturumda eklendi)* — Singleton. Tam ekranı kaplayan `CanvasGroup
  KaraPanel` üzerinden siyah ekran geçişi yapıyor. `KararipAcil(System.Action ortadaYapilacakIs)`:
  önce ekranı karartır, ekran tam siyahken verilen `Action`'ı çalıştırır (örn. panel değişimi), sonra
  tekrar açar. Şu an sadece Taht Odası → Şahsi Oda geçişinde kullanılıyor. Sahnede
  `EkranGecisiYoneticisi` adlı objede duruyor, `KaraPanel` alanı Canvas'taki `KaraPanel` objesine
  bağlı. Test edildi, çalışıyor.
- **`OdaEtkilesimTest.cs`** *(yeni, bu oturumda eklendi)* — Şahsi Oda'daki masa objelerinin
  (Ansiklopedi, Harita) ve Kapı'nın tıklama olaylarını yakalıyor. `AnsiklopediTiklandi()` ve
  `HaritaTiklandi()` şu an sadece `Debug.Log` yapan **placeholder**'lar — ne göstereceği henüz
  tasarlanmadı. `KapiTiklandi()`: `DanismanListesiPaneli`'nin SetActive'ini tersine çeviriyor (toggle).
  Eskiden bir de `MektuplarTiklandi()` fonksiyonu vardı ama **artık kullanılmıyor** — mektup açma işi
  `MektupYoneticisi.cs`'e taşındı; `MektuplarGorseli` butonunun `On Click()`'inin doğru script'e
  (`MektupYoneticisi`) işaret ettiği **teyit edilmedi**, bkz. Sıradaki Adımlar. `OdaGorseli` objesinde duruyor.
- **`DanismanCagir.cs`** *(yeni, bu oturumda eklendi)* — Kapıdan açılan danışman listesindeki butonlara
  bağlı. `DialogueManager Dialog`, `GameObject DanismanListesiPaneli`, `NPCData General`,
  `NPCData MaliyeBakani`. `GeneralCagir()`: listeyi kapatıp `Dialog.DiyalogBaslat(General.Diyalog,
  General.Portre, General.Isim, true)` çağırıyor. `MaliyeBakaniCagir()` fonksiyonu yazıldı ama
  **`MaliyeBakani` verisi (NPCData asset'i) henüz oluşturulmadı, buton da bağlanmadı** — sadece
  General ile akış doğrulandı. Sahnede `DanismanYoneticisi` adlı objede duruyor.
- **`MektupYoneticisi.cs`** *(yeni, bu oturumda eklendi)* — Mektuplar objesine tıklanınca açılan liste
  ve mektup diyaloğu için. `DialogueManager Dialog`, `GameObject MektuplarPaneli`,
  `DialogueData TestMektup`. `MektuplarTiklandi()`: `MektuplarPaneli`'ni toggle yapıyor (bu fonksiyonun
  `MektuplarGorseli` butonuna **doğru bağlanıp bağlanmadığı teyit edilmedi**). `TestMektubuAc()`:
  listeyi kapatıp `Dialog.DiyalogBaslat(TestMektup, null, "Mektup", true)` çağırıyor (portre `null` —
  mektupların karakter portresi yok). Sahnede `MektupYoneticisi` adlı objede duruyor.

---

## 5) ŞU ANKİ DURUM

✅ Çalışıyor (test edildi, onaylandı):
- Temel döngü uçtan uca: Taht Odası → (siyah ekran geçişi) → Şahsi Oda → Uyu/Resolve → tekrar Taht Odası.
- ScriptableObject tabanlı diyalog sistemi (node/choice yapısı), bir seçenek birden fazla stat'ı aynı
  anda etkileyebiliyor (`StatEtkileri`), yetersiz kaynak gerektiren seçenekler görsel kilitleniyor
  (`SecenekKarsilanabilirMi`).
- NPC sırasının stat'a göre kısmen rastgele oluşması + `Gun == 10` gibi sabit-günlü hikaye NPC'leri
  (Ayyaş Adam).
- Emir verme + "danışman döngü başına 1 kez" kısıtlaması.
- Zar atma + çok günlü emir + garanti/şansa-bağlı sonuç ayrımı, renklendirilmiş sonuç mesajları.
- Elçi/Ulak NPC'si ile gece sonuçlarının ertesi sabah otomatik anlatılması.
- Sol üstte canlı stat göstergesi, üst köşede "Gun X" sayacı.
- Ekonomi sistemi (Altın, Manpower, base gelir, maliyetli emirler) uçtan uca test edildi.
- Buton üzerine gelince maliyet bilgisi gösteren hover tooltip sistemi.
- Danışman kullanılınca butonların görsel kilitlenmesi (`DanismanButon.cs`).
- NPC portre/karakter görseli sistemi, diyalog panelinde NPC ismi gösterimi.
- Anlık bildirim (toast) sistemi — **bu oturumda fade in/fade out eklendi**, artık yumuşak açılıp kapanıyor.
- **Taht Odası → Şahsi Oda geçişinde kısa siyah ekran efekti var** (`EkranGecisi.cs`).
- **Diyalog kutusu düzeni yenilendi:** metin kutunun içinde, portre yanında, seçenekler altında —
  tüm bu objeler tek bir `DiyalogAlani` grubu altında toplandı, bu grup artık Taht Odası/Şahsi Oda
  panellerinden bağımsız, `DialogueManager` tarafından elle açılıp kapanıyor.
- **Danışman diyalog akışı (kapı sistemi) uçtan uca test edildi:** Şahsi Oda'da kapıya tıklayınca
  danışman listesi açılıyor, "General" seçilince Warband tarzı çok adımlı bir diyalog başlıyor
  ("Bir görevim var" → "Köy Yağmala" → "Hangi köyü?" → köy seçilince gerçek bir `OrderData`
  `OrderManager`'a ekleniyor, Console'da "Askerbasi icin emir eklendi" mesajı doğrulandı), diyalog
  bitince Taht Odası'na değil Şahsi Oda'da kalınıyor.
- **Şahsi Oda görsel iskeleti kuruldu** (placeholder renkli kutularla): masa, üzerinde Ansiklopedi/
  Harita/Mektuplar objeleri, arka planda Kapı. Hepsi tıklanabilir (Button + OnClick).

⚠️ Yarım kaldı / doğrulanmadı (bir sonraki oturumun ilk işi olmalı):
- **Mektuplar akışı henüz uçtan uca çalışmıyor.** `MektuplarGorseli` butonunun `On Click()`'i eskiden
  `OdaEtkilesimTest.MektuplarTiklandi` (sadece Console'a log basan eski placeholder) gösteriyordu; bu
  bağlantının `MektupYoneticisi.MektuplarTiklandi`'ye değiştirilmesi gerektiği tespit edildi ve
  kullanıcıya adım adım nasıl düzeltileceği anlatıldı, ama **düzeltmenin yapılıp test edildiği
  onaylanmadan sohbet bitti.** Yeni oturumda ilk iş: bu bağlantıyı kontrol et, gerekirse düzelt, sonra
  "Kabul Et" ile `Test_Mektup`'un gerçekten Erzak -10 / Sadakat +5 uyguladığını doğrula.
- Maliye Bakanı danışmanı henüz eklenmedi (`DanismanCagir.MaliyeBakaniCagir()` fonksiyonu hazır ama
  `MaliyeBakani` NPCData asset'i yok, `MaliyeBakaniButonu`'nun `On Click()`'i bağlanmadı).
- Ansiklopedi ve Harita objelerinin ne göstereceği hiç tasarlanmadı — şu an sadece Console'a log basan
  boş placeholder'lar.
- `DanismanPaneli.cs` (eski 4 buton sistemi) hâlâ sahnede duruyor, kapı sistemi tam anlamıyla onun
  yerini alana kadar kaldırılmayacak.

---

## 6) SIRADAKİ ADIMLAR

"Şahsi Oda'yı gerçek bir oda yapalım" isteği üzerine büyük iş 4 parçaya bölündü, kullanıcı sırayı
**1 → 3 → 2 → 4** olarak seçti:

1. ✅ Danışman diyalog akışının kod altyapısı (`VerilecekEmir`, `DialogueManager` genişletmesi) — kuruldu, test edildi.
2. ✅ Oda görsel düzeni (masa, kitap, harita, mektup, kapı — placeholder kutularla) — kuruldu, test edildi.
3. ✅ Kapı + danışman seçim listesi (tıkla-aç, hover değil — kullanıcının kararı) — kuruldu, General ile test edildi.
4. ⚠️ **Mektuplar/görev sistemi — büyük ölçüde kuruldu ama son bağlantı testi tamamlanmadı.**
   Yeni oturumun ilk işi: yukarıdaki "Yarım kaldı" bölümündeki Mektuplar butonu sorununu çöz ve doğrula.

Bittikten/doğrulandıktan sonra sırada (henüz kullanıcıyla konuşulmadı, öncelik sırası belirlenmedi):
- Maliye Bakanı'nı (ve muhtemelen başka danışmanları) `General` ile aynı desenle eklemek — bu artık
  tekrar eden, tanıdık bir iş (NPCData asset'i + DialogueData + buton bağlama).
- Ansiklopedi ve Harita objelerinin ne göstereceğine karar vermek (henüz hiç konuşulmadı).
- Eski `DanismanPaneli.cs` (4 sabit buton) sistemini kaldırıp yerini tamamen kapı sistemine bırakmak.
- Diğer, henüz gündeme gelmemiş konular: daha fazla diyalog/NPC/danışman içeriği, `DaySequencer.cs`'ye
  Erzak dışında stat'lara bağlı yeni sıra kuralları, kaydet/yükle (save/load) sistemi.

**Genel çalışma tarzı hâlâ geçerli:** her adımı küçük parçalara böl, kullanıcı test edip onaylamadan
bir sonrakine geçme.

---

## 7) BİLİNEN TUZAKLAR / DİKKAT EDİLECEKLER

- Kod editöründe değişiklik yapıp **kaydetmeden (Ctrl+S)** Unity'ye dönmek "hayalet hatalara" yol
  açıyor — her değişiklikten sonra kaydet, Unity'nin alt köşesindeki "compiling" ikonunun bitmesini bekle.
- Script derleme hatası (compile error) varken Unity bazen buton `On Click()` bağlantılarını
  sıfırlıyor — kod hatasını düzelttikten sonra buton bağlantılarını tekrar kontrol etmek gerekebilir.
- Bir dosyada tek bir hata bile olsa (örn. `[Serializable]` etiketinin 2 kere yazılması gibi), TÜM
  proje derlenemiyor ve alakasız görünen onlarca hata ortaya çıkabiliyor — Console'daki **EN ESKİ
  (ilk)** hataya odaklanmak genelde kök sebebi buluyor.
- Inspector'da bir alana GameObject sürüklerken, o objenin üzerinde gerçekten **doğru script/component**
  olduğundan emin olunmalı; yanlış obje sürüklenirse dropdown'da beklenen fonksiyon görünmüyor.
- `foreach` ile gezilen bir liste, aynı döngü içinde direkt değiştirilemiyor (silinemiyor) — bu yüzden
  `DayResolver` içinde "hâlâ devam edenler" için ayrı, yeni bir liste oluşturulup dolduruluyor.
- **Bir script dosyasını (örn. `NPCData.cs`) Project penceresinde tıklamakla, o script'ten üretilmiş
  bir veri (asset, örn. `Ayyas_NPC.asset`) tıklamak FARKLI şeyler** — kullanıcı bunu bir kez
  karıştırdı. Script dosyasının ikonu farklıdır (kod dosyası ikonu), asset'in ikonu farklıdır (mavi
  küp). Kullanıcıya hangisini bulması gerektiğini net söylerken bu ayrımı vurgula.
- UI objelerinde **Hierarchy sırası çizim sırasını belirliyor** — sonradan eklenen (Hierarchy'de daha
  altta duran) objeler öncekilerin **üstüne** çiziliyor. Yeni bir opak arkaplan/panel eklendiğinde
  eski butonların "kaybolması" genelde silinme değil, görsel olarak **kaplanma**dır — Hierarchy'de
  obje hâlâ duruyor mu diye bakmak ilk kontrol noktası olmalı.
- Yeni bir `[Serializable]` sınıfı (örn. `OrderData`) bir `DialogueChoice` gibi başka bir serialize
  edilen sınıfın içine gömülüp Inspector'da düzenlenebilir olması isteniyorsa, o sınıfın **parametresiz
  (boş) bir constructor'ı olmalı** — yoksa Unity Inspector'da düzgün gösteremeyebilir.
- Bir Button'ın `On Click()` listesinde **eski/placeholder bir fonksiyon kalabilir** — özellikle bir
  işlev başka bir script'e taşındığında, eski bağlantı otomatik silinmiyor, elle kontrol edip
  güncellemek gerekiyor (bkz. Mektuplar butonu sorunu, Sıradaki Adımlar).
