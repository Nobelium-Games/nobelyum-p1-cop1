//# KRAL SİMÜLASYONU — Proje Bağlamı

> Bu dosya, bu Unity projesinde daha önceki bir Claude sohbetinde yapılan çalışmanın özetidir.
> Amaç: yeni bir sohbet/oturum bu dosyayı okuduğunda, sanki aynı sohbetten kaldığı yerden
> devam ediyormuş gibi bağlamı hızlıca kavraması.
>
> **Yeni oturuma not:** Bu dosya bir yol haritasıdır, ama gerçek kod her zaman "zemin gerçeği"dir.
> Özellikle "Şu Anki Durum" bölümündeki bazı dosyalar (NPCData.cs, DialogueManager.cs) bu özeti
> hazırlarken doğrudan görülmedi — işe başlamadan önce ilgili script dosyalarını oku/incele ve
> bu özetle karşılaştır.

---

## 1) TAKIM VE ANLATIM TARZI (HER ZAMAN UYGULANMALI)

- 3 kişilik küçük bir ekibiz, proje yönetimi ve Unity/C# konusunda ileri seviye değiliz, sıfırdan öğreniyoruz.
- Unity'ye genel hakimiyet var, ama **C# bilgisi gelişmiş değil.**
- Açıklamalar HER ZAMAN basit, az teknik jargonlu, adım adım olmalı — sanki hiç bilmiyormuşuz gibi anlat.
- Yeni bir C# kavramı ilk kez geçtiğinde (fonksiyon/parametre, switch, enum, constructor, HashSet,
  foreach, ScriptableObject, Singleton, SetActive, vs.) mutlaka kısa (1-3 cümle), günlük hayattan
  bir benzetmeyle açıklanmalı. Örnek benzetmeler daha önce işe yaradı: fonksiyon = matematikteki
  çok değişkenli fonksiyon f(x,y); GameManager.Instance.State.Erzak zinciri = apartman → daire → çekmece.
- **Küçük adımlarla ilerle.** Her seferinde tek bir parça ver, kullanıcı deneyip "oldu" ya da hata
  paylaşana kadar bir sonrakine geçme. Büyük, çok-parçalı adımlar kullanıcıyı bunalttı ve bir kez
  (ekonomi sistemi eklerken) "korkup her şeyi geri almasına" yol açtı — bundan kaçın, mümkün olan
  en küçük, test edilebilir parçalara böl.
- Hata geldiğinde önce sakinleştir ("bu normal, panik yok"), sonra sırayla kontrol ettir: Console'daki
  EN ESKİ/İLK hata ne diyor, dosya kaydedilmiş mi (Ctrl+S), doğru obje/script Inspector'a sürüklenmiş mi.
- Kullanıcı "Create → C# Script" ile MonoBehaviour şablonu oluşturduğunda kafası karışabiliyor;
  hangi script'in MonoBehaviour (sahneye yapıştırılacak) hangisinin düz veri sınıfı (`: MonoBehaviour`
  YOK) olduğunu her seferinde net söyle.

---

## 2) OYUN KONSEPTİ

Diyalog ve karar temalı bir **"kral simülasyonu"** oyunu (Unity, C#, **2D**). Oyuncu bir kral,
ülkeyi yönetiyor.

### Stat'lar
Oyunda bir sürü "stat" (sayı) var: Erzak, Sadakat, Ordu Gücü, ve genişletilecek olan Altın, Manpower
gibi kaynaklar. Bunlar oyundaki olayları belirliyor, verilen kararlara göre değişiyor.

### Döngü (Cycle) Sistemi
Oyun "döngü" (cycle) sistemiyle ilerliyor. **Her döngü = 1 gün**, 2 bölümden oluşuyor:

**1. TAHT ODASI**
Sıradaki NPC'ler tek tek gelip oyuncuyla diyalog kuruyor. Diyalog seçimleri küçük stat
değişikliklerine yol açabiliyor. Sıra bitince otomatik olarak 2. bölüme geçiliyor.

**2. ŞAHSİ ODA**
Oyuncu masasında, harita ve ülke stat'larını gösteren kağıtlara bakabiliyor. 4 farklı danışman
tipine (İnşaatçı, Askerbaşı, Dışişleri Elçisi, Ekonomist vb.) emir verilebiliyor. Her danışman
**döngü başına sadece 1 kez** kullanılabiliyor (kullanılınca o döngü için kilitleniyor). Emir
verildiğinde **hiçbir stat anında değişmiyor**, emir sadece "bekleyen emirler" listesine kaydediliyor.

### Uyu Tuşu (Gece / Resolve)
Oyuncu "Uyu" tuşuna basınca döngü sona eriyor ve şunlar oluyor:
- Bekleyen emirlerin her biri için zar atılıyor (rastgele + ilgili stat'a bağlı başarı ihtimali).
- Başarı/başarısızlık belirleniyor, stat'lar buna göre güncelleniyor.
- Bazı olaylar oyuncu kararından bağımsız, tamamen rastgele de tetiklenebiliyor.
- Bir sonraki günün taht odası sırası (hangi NPC'lerin geleceği) o anki stat'lara göre (örn. düşük
  erzak → kızgın köylü ihtimali artar) VE sabit hikaye olaylarına göre (örn. 10. gün gelen sabit
  quest karakteri) belirlenip bir liste halinde saklanıyor.
- Sonuçlar (inşaat tamamlandı, akın başarılı oldu vb.) bir sonraki sabah taht odasında bir
  elçi/ulak karakteri üzerinden oyuncuya aktarılıyor.

### Çok Günlü Emirler
İnşaat gibi bazı emirlerin sonucu birden fazla döngü sürebiliyor (örn. değirmen birkaç döngü
sonra tamamlanıyor). Bunun içinde de bir ayrım var:
- **Garanti sonuç** (örn. Değirmen İnşası): süre dolunca kesin tamamlanır, zar atılmaz.
- **Şansa bağlı sonuç** (örn. Köy Yağmalama): süre dolunca zar atılır, başarısız da olabilir.

### Ekonomi (HEDEF — bkz. "Sıradaki Adımlar", henüz kodda değil)
- **Altın** ve **Manpower** adında yeni stat'lar eklenecek.
- Değirmen inşası **Altın** harcıyor; Askere asker vermek de **Altın** harcıyor; Köy yağmalamak
  **Manpower** harcıyor (emir verilirken maliyet anında düşülüyor).
- Erzak ve Altın'ın bir **"base değeri"** (pasif, otomatik döngü başı geliri) var — örn. her döngü
  otomatik +3 Altın +5 Erzak kazanılıyor.
- Değirmen inşası tamamlanınca doğrudan Erzak'a eklemek yerine, Erzak'ın **base değerini kalıcı
  olarak artırıyor** (örn. tamamlanınca +1 → her döngü artık +6 Erzak).

---

## 3) MİMARİ KARARLARI (5 TEMEL SORU VE CEVABI)

**1. Stat'lar nerede tutuluyor?**
Merkezi bir **`GameState`** sınıfı (düz C# class, `[Serializable]`, **ScriptableObject DEĞİL** —
çünkü ScriptableObject Editor'da eski değerleri hatırlayabiliyor, save/load kontrolü zorlaşıyor).
`GameState`, **`GameManager`** (MonoBehaviour, Singleton) içinde `public GameState State` olarak
tutuluyor. Her yerden `GameManager.Instance.State.Erzak` gibi erişiliyor.

**2. NPC'ler GameObject mi, veri mi?**
**Veri olarak** tutuluyor: `NPCData` bir ScriptableObject (ID, Isim, Diyalog referansı). Sahnede
NPC başına ayrı GameObject YOK — tek bir görsel "sahne" (şu an sadece UI text) var, hangi NPC'nin
sırası gelirse o NPC'nin verisiyle dolduruluyor. (Not: henüz gerçek bir "karakter görseli" objesi
kurulmadı, şu an sadece metin tabanlı gösterim var.)

**3. Taht odası sırası nerede oluşturuluyor?**
**`DaySequencer`** (düz C# class, MonoBehaviour değil). `SiradakiListeyiOlustur(GameState, ...)`
fonksiyonu, stat'lara bakıp (şu an sadece Erzak'a göre, tek kural) rastgele bir NPC sırası üretip
`List<NPCData>` olarak döndürüyor.

**4. Bekleyen emirler ve zar atma nerede?**
- **`OrderManager`** (MonoBehaviour): `List<OrderData> BekleyenEmirler` + `HashSet<string>
  KullanilanDanismanlar` (danışman başına döngüde 1 kullanım kısıtlaması burada uygulanıyor).
- **`DayResolver`** (düz C# class): zar atma + sonuç hesaplama mantığının tamamı burada.

**5. Döngü akışı nasıl kuruldu?**
Basit bir **state machine**, `enum GunAsamasi { TahtOdasi, SahsiOda, Resolve }` ile.
**`DayCycleManager`** (MonoBehaviour, Singleton) bu enum'u tutuyor, `AsamaDegistir()` fonksiyonu
ilgili UI panelini `SetActive(true/false)` ile açıp kapatıyor. Akış: TahtOdasi (sıra biter) →
SahsiOda (Uyu'ya basılır) → Resolve (zar atılır, yeni gün hazırlanır) → tekrar TahtOdasi.

---

## 4) SCRIPT ENVANTERİ

### Veri sınıfları (düz C#, MonoBehaviour DEĞİL)
- **`GameState.cs`** — `Erzak`, `Sadakat`, `Altin`, `Manpower` (int), `ErzakBaseGelir`, `AltinBaseGelir`
  (int, her yeni günde otomatik kazanılan miktar). Fonksiyonlar: `StatDegistir(statAdi, miktar)` (stat'ı
  değiştirir), `StatDegerAl(statAdi)` (stat'ın şu anki değerini okur), `BaseGeliriUygula()` (her yeni
  günde Erzak/Altın'a base geliri ekler), `BaseGeliriArtir(statAdi, miktar)` (bir stat'ın base gelirini
  kalıcı artırır — Değirmen gibi emirler tamamlanınca kullanılıyor). *(Eskiden `OrduGucu` diye ayrı bir
  stat vardı, `Manpower` ile aynı şey olacaktı — birleştirildi, artık sadece `Manpower` var.)*
- **`OrderData.cs`** — Bir emrin tarifi. Constructor: `DanismanTipi, EmirTuru, EtkilenenStat,
  BasariliDegisim, BasarisizDegisim, BasariSansi, ToplamSure, SonucSansaBagli` (zorunlu) +
  `MaliyetStat="", MaliyetMiktar=0` (opsiyonel, emir verilirken anında düşülüyor) +
  `BaseGeliriEtkiler=false, BaseGeliriStat="", BaseGeliriMiktar=0` (opsiyonel, garanti tamamlanan emir
  normal stat artışı yerine ilgili stat'ın base gelirini kalıcı artırabiliyor).
- **`DevamEdenEmir.cs`** — `OrderData Emir` + `int KalanGun`. Çok günlü, hâlâ devam eden emirlerin
  "şu an ne durumda" bilgisini taşıyor.
- **`DaySequencer.cs`** — bkz. Mimari Kararları #3.
- **`DayResolver.cs`** — `SonucMesajlariniOlustur(GameState, List<OrderData> emirler,
  List<DevamEdenEmir> devamEdenler, List<string> mesajListesi)`: tek günlük emirleri anında zar atıp
  sonuçlandırıyor (`ZarAtVeUygula` yardımcı fonksiyonu); çok günlü emirleri `devamEdenler`'e ekleyip
  her çağrıda 1 gün ilerletiyor; süresi dolanları garanti tamamlıyor (ya normal stat artışı ya da
  `BaseGeliriEtkiler` işaretliyse base gelir artışı) ya da (`SonucSansaBagli` true ise) zar atıp
  sonuçlandırıyor. Güncellenmiş `devamEdenler` listesini geri döndürüyor (bir sonraki geceye taşınsın diye).
- **`NPCData.cs`** — ScriptableObject. `ID`, `Isim`, `DialogueData Diyalog`, `Sprite Portre`
  (karakter görseli — elle Inspector'dan atanıyor). Doğrulandı, sorun yok.
- **`DialogueData.cs`** — ScriptableObject. `DialogueID`, `List<DialogueNode> Nodler`.
  `DialogueNode`: NodeID, NPCSozu, `List<DialogueChoice> Secenekler`. `DialogueChoice`: SecenekMetni,
  SonrakiNodeID, EtkilenenStat, StatDegisimi.

### MonoBehaviour'lar (sahnede bir GameObject'e eklenmiş script'ler)
- **`GameManager.cs`** — Singleton (`public static GameManager Instance`), `public GameState State`.
  Artık sadece bir "veri kutusu tutucu", akış mantığı içermiyor (o kısım DayCycleManager'a taşındı).
- **`OrderManager.cs`** — bkz. Mimari Kararları #4. `EmirEkle` (maliyet kontrolü de burada: yetersizse
  reddediyor, yeterliyse anında düşüp emri ekliyor), `DanismanKullanildiMi`, `YeniDongueBasla`
  (döngü sonunda listeleri temizler).
- **`DialogueManager.cs`** — Aktif diyaloğu ekrana basıyor: `NpcSozuText`, `SecenekButon1Text`,
  `SecenekButon2Text` (TMP_Text referansları), `PortreImage` (Image, NPC portresi), `SecenekButon2`
  (GameObject referansı — tek seçenekli diyaloglarda bu buton `SetActive(false)` ile gizleniyor, örn.
  elçi diyaloğunda). `DiyalogBaslat(DialogueData diyalog, Sprite portre)`, `Secenek1Secildi()`,
  `Secenek2Secildi()`. Diyalog bitince `DayCycleManager.Instance.SiradakiyeGec()` çağırıyor. Doğrulandı, sorun yok.
- **`DayCycleManager.cs`** — Singleton, state machine'in kalbi. `GunlukSira` (List<NPCData>) +
  `suankiNpcIndex` ile Taht Odası kuyruğunu yönetiyor. `YeniGuneBasla()`: **en başta**
  `GameManager.Instance.State.BaseGeliriUygula()` çağrılıp günün base geliri işleniyor, sonra
  DaySequencer ile günün sırası oluşturuluyor (**DaySequencer satırının fonksiyonun EN BAŞLARINDA
  olması şart** — bir ara bu satır kayboldu ve oyun hiç başlamadı, dikkat edilmeli), önceki geceden
  sonuç varsa `ElciDiyaloguOlustur` ile dinamik bir `DialogueData` üretip sıranın başına "Ulak" NPC'sini
  ekliyor (`ScriptableObject.CreateInstance<NPCData>()` ile runtime'da NPC/diyalog üretiliyor — bu
  NPC'nin `Portre`'si atanmadığı için Ulak diyaloğunda portre alanı boş kalıyor, bilinçli).
  `UyuyaBas()`: Resolve'a geçer, DayResolver'ı çalıştırır, sonuçları Console'a basar, OrderManager'ı
  sıfırlar, YeniGuneBasla'yı tekrar çağırır.
- **`DanismanPaneli.cs`** — (eski adı `ManagerTest.cs`'ti; kalıcı mimarinin parçası olarak yeniden
  adlandırıldı, Unity'de test edildi, sorun yok). Şahsi Oda butonlarına (İnşaatçı, Askerbaşı, Asker
  Topla) sabit `OrderData`'lar oluşturup `OrderManager.EmirEkle`'ye gönderiyor: Değirmen İnşa Et (Altın
  maliyetli, tamamlanınca Erzak base gelirini +1 artırıyor), Köy Yağmala (Manpower maliyetli, şansa
  bağlı), Asker Topla (Altın maliyetli, garantiye yakın, Manpower +10 veriyor — Köy Yağmala ile aynı
  danışmanı paylaşıyor).
- **`StatsUI.cs`** — Sol üstteki canlı stat göstergesi. `Update()` içinde her karede `GameManager.Instance.State`'i
  okuyup `StatlarText` (TMP_Text) içine basıyor. Erzak ve Altın satırlarında `<sup>+5</sup>` gibi TMP
  rich-text etiketiyle o anki base geliri üst indis olarak gösteriyor.
- **`TooltipUI.cs`** + **`ButtonTooltip.cs`** — Hover (fare üzerine gelince) bilgi kutusu sistemi.
  `TooltipUI` tek bir panel/text'i açıp kapatıyor (`Goster(metin)` / `Gizle()`), `ButtonTooltip` ise
  herhangi bir butona eklenip `IPointerEnterHandler`/`IPointerExitHandler` ile fare olaylarını dinliyor,
  kendi `TooltipMetni` alanındaki yazıyı gösteriyor. Şu an İnşaatçı/Askerbaşı/Asker Topla butonlarında kullanılıyor.
- **`DanismanButon.cs`** — Unity'de bağlanıp test edildi, sorun yok. Bir danışman butonuna eklenip
  `Update()` içinde `Orders.DanismanKullanildiMi(DanismanTipi)`'a bakarak `Button.interactable`'ı
  otomatik açıp kapatıyor — danışman o döngüde kullanıldıysa buton görsel olarak da kilitleniyor.

---

## 5) ŞU ANKİ DURUM

✅ Çalışıyor (test edildi, onaylandı):
- Temel döngü uçtan uca: Taht Odası → Şahsi Oda → Uyu/Resolve → tekrar Taht Odası.
- ScriptableObject tabanlı diyalog sistemi (node/choice yapısı), seçimler stat değiştirebiliyor.
- NPC sırasının stat'a göre kısmen rastgele oluşması (şu an tek kural: düşük Erzak → köylü ihtimali artıyor).
- Emir verme + "danışman döngü başına 1 kez" kısıtlaması.
- Zar atma + çok günlü emir + garanti/şansa-bağlı sonuç ayrımı (değirmen vs. yağma mantığı).
- Elçi/Ulak NPC'si ile gece sonuçlarının ertesi sabah otomatik anlatılması.
- Sol üstte canlı stat göstergesi (`StatsUI.cs`): Erzak, Altın, Manpower, Sadakat; Erzak ve Altın'ın
  yanında üst indis (`<sup>`) olarak o anki base gelir (`+5`, `+3` gibi) gösteriliyor.
- **Ekonomi sistemi eklendi ve uçtan uca test edildi** (küçük parçalara bölünerek, her adım ayrı
  onaylanarak): `GameState`'te `Altin`, `Manpower`, `ErzakBaseGelir`, `AltinBaseGelir` alanları ve
  `BaseGeliriUygula()` / `BaseGeliriArtir()` / `StatDegerAl()` fonksiyonları var. `OrderData`'da
  `MaliyetStat`/`MaliyetMiktar` (emir verilirken anında düşülüyor, `OrderManager.EmirEkle` yetersizse
  reddediyor) ve `BaseGeliriEtkiler`/`BaseGeliriStat`/`BaseGeliriMiktar` (garanti tamamlanan emir,
  normal stat artışı yerine ilgili stat'ın base gelirini kalıcı artırabiliyor — `DayResolver.cs`'te
  uygulanıyor) var. `ManagerTest.cs`'te 3 örnek emir: Değirmen İnşa Et (Altın maliyetli, tamamlanınca
  Erzak base gelirini +1 artırıyor), Köy Yağmala (Manpower maliyetli, şansa bağlı), Asker Topla
  (Altın maliyetli, aynı gün garantiye yakın sonuçlanıp Manpower +10 veriyor — Köy Yağmala ile aynı
  danışmanı (Askerbaşı) paylaşıyor, bu yüzden bir döngüde ikisinden sadece biri seçilebiliyor).
- Buton üzerine gelince maliyet bilgisi gösteren hover tooltip sistemi (`TooltipUI.cs` + `ButtonTooltip.cs`).
- `OrduGucu` stat'ı kaldırıldı, yerine tamamen `Manpower` kullanılıyor (ikisi aynı şey olacaktı,
  yanlışlıkla ayrı iki değişken açılmıştı — birleştirildi, `Asker_Diyalog.asset` içindeki referans da güncellendi).
- **Danışman Paneli dönüşümü tamamlandı:** `DanismanButon.cs` Unity'de butonlara bağlanıp test edildi
  (danışman kullanılınca butonlar görsel olarak kilitleniyor, Uyu sonrası tekrar açılıyor). Ayrıca
  `ManagerTest.cs`, `DanismanPaneli.cs` olarak yeniden adlandırıldı (dosya + class ismi), Unity'de eski
  component kaldırılıp yenisi eklendi, buton `On Click()` referansları tekrar bağlandı, test edildi —
  eskisi gibi çalışıyor.
- **NPC portre/karakter görseli sistemi eklendi ve test edildi:** `NPCData.cs`'de `Portre` (Sprite)
  alanı var, ScriptableObject'lerde elle atanıyor. `DialogueManager.cs`'de `PortreImage` (UI Image)
  referansı var, `DiyalogBaslat(diyalog, portre)` artık portreyi de alıp ekrana basıyor.
  `DayCycleManager.cs`, `Dialog.DiyalogBaslat(npc.Diyalog, npc.Portre)` şeklinde çağırıyor. Not: Ulak
  (elçi/gece sonucu) NPC'sinin runtime'da oluşturulan `NPCData`'sında portre atanmadığı için o anda
  Image boş kalıyor — bilinçli, sorun değil.

⚠️ Yarım kaldı / geri alındı:
- `NPCData.cs` içeriğinde bir kopyalama hatası (bug) tespit edilip kullanıcı tarafından çözüldü,
  ama düzeltilmiş son hali önceki sohbette doğrulanmamıştı — bu sohbette dosya okunup güncel/doğru
  olduğu teyit edildi (ID, Isim, Diyalog alanları düzgün).

---

## 6) SIRADAKİ ADIMLAR

Kullanıcı "gerçek bir Danışman Paneli yapalım" dedi, bunun üzerine çalışılıyor. Plan 3 küçük adıma bölündü:

1. ✅ Butonların görsel kilitlenmesi: `DanismanButon.cs` oluşturulup Unity'de İnşaatçı/Askerbaşı/Asker
   Topla butonlarına bağlandı, test edildi — çalışıyor.
2. ✅ `ManagerTest.cs`, `DanismanPaneli.cs` olarak yeniden adlandırıldı (dosya + class ismi), Unity'de
   component yeniden bağlandı, buton referansları tekrar kuruldu, test edildi — eskisi gibi çalışıyor.
3. ⬜ **Sıradaki iş — buradan devam et:** Panelin görsel düzenini iyileştirmek (ikon, layout, vs. —
   henüz detay konuşulmadı, kullanıcıya ne istediği sorulmalı).

Diğer, henüz gündeme gelmemiş ama ileride akla gelebilecek konular: daha fazla diyalog/NPC içeriği,
`DaySequencer.cs`'ye Erzak dışında stat'lara bağlı yeni sıra kuralları, kaydet/yükle (save/load) sistemi.

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