using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public enum HaritaGorunumu
{
    Siyasi,
    Terrain,
    Kaynak
}

public class HexHaritaCizici : MonoBehaviour
{
    public static HexHaritaCizici Instance;

    public HaritaGorunumu AktifGorunum = HaritaGorunumu.Siyasi;

    public RectTransform Icerik;

    public float TileBoyutu = 40f;
    public float TileSaydamligi = 0.35f;
    public float YerlesimBoyutu = 50f;
    public float KaynakIkonBoyutu = 16f;
    public Color SinirRengi = Color.black;
    public Color ErzakIkonRengi = new Color(0.3f, 0.8f, 0.3f);
    public Sprite KoyIkonu;
    public Sprite SehirIkonu;
    public Sprite KaleIkonu;
    public Sprite DegirmenIkonu;
    public float DegirmenIkonBoyutu = 24f;
    public TMP_Text GorunumMetni;
    public GameObject KapatButonu;
    public HaritaKontrol Kontrol;
    public float IcerikKenarPayi = 100f;

    [Header("Ordu Hareketi Gorseli")]
    public Color OrduOkuRengi = Color.white;
    public Sprite AskerIkonu;
    public int OkParcaSayisi = 30;
    public float OkParcaBoyutu = 14f;
    public float OkParcaAraligi = 36f;
    public float AskerIkonuBoyutu = 20f;
    public float OkYanipSonmeHizi = 4f;

    public Color YildizRengi = new Color(1f, 0.85f, 0.2f);
    public float YildizBoyutu = 16f;

    Sprite hexagonSprite;
    Sprite hexagonCerceveSprite;
    Sprite kareSprite;
    Sprite daireSprite;
    Sprite okSprite;
    Sprite yildizSprite;

    Dictionary<KoyData, Image> yildizGorselleri = new Dictionary<KoyData, Image>();

    Dictionary<OrderData, OrduYoluGorseli> orduGorselleri = new Dictionary<OrderData, OrduYoluGorseli>();

    Dictionary<KoyData, List<GameObject>> koyTileSinirlari = new Dictionary<KoyData, List<GameObject>>();
    Dictionary<HexTileData, Image> tileGorselleri = new Dictionary<HexTileData, Image>();
    Dictionary<KoyData, Image> yerlesimGorselleri = new Dictionary<KoyData, Image>();
    Dictionary<HexTileData, GameObject> kaynakGorselleri = new Dictionary<HexTileData, GameObject>();
    Dictionary<KoyData, GameObject> yerlesimIsimGorselleri = new Dictionary<KoyData, GameObject>();
    Dictionary<KoyData, GameObject> merkezSinirGorselleri = new Dictionary<KoyData, GameObject>();
    Dictionary<HexTileData, GameObject> degirmenGorselleri = new Dictionary<HexTileData, GameObject>();

    GameObject sabitArkaplan;

    bool tileSecimModuAktif;
    KoyData tileSecimKoyu;
    System.Action<HexTileData> tileSecimCallback;

    void Start()
    {
        Instance = this;

        hexagonSprite = SekilUretici.HexagonSprite();
        hexagonCerceveSprite = SekilUretici.HexagonCerceveSprite();
        kareSprite = SekilUretici.KareSprite();
        daireSprite = SekilUretici.DaireSprite();
        okSprite = SekilUretici.OkUcuSprite();
        yildizSprite = SekilUretici.YildizSprite();

        SabitArkaplaniOlusturVeyaGuncelle();
        EskiCizimleriTemizle();
        koyTileSinirlari.Clear();
        tileGorselleri.Clear();
        yerlesimGorselleri.Clear();
        kaynakGorselleri.Clear();
        yerlesimIsimGorselleri.Clear();
        merkezSinirGorselleri.Clear();
        degirmenGorselleri.Clear();
        orduGorselleri.Clear();
        yildizGorselleri.Clear();
        IcerikBoyutunuGuncelle();
        TileleriCiz();
        DegirmenIkonlariniGuncelle();
        YerlesimleriCiz();
        GorunumMetniniGuncelle();
        OrduGorselleriniIlkYukle();

        if (Kontrol != null)
        {
            Kontrol.YenidenHesaplaVeSinirla();
        }
    }

    // Harita ilk kez acilirken, daha once (harita hic acilmamisken) verilmis olabilecek
    // emirlerin gorselleri de eksik kalmasin diye hem devam eden hem henuz Uyu'ya basilip
    // islenmemis (onizleme asamasindaki) emirleri tarayip cizdiriyor.
    void OrduGorselleriniIlkYukle()
    {
        if (DayCycleManager.Instance == null)
        {
            return;
        }

        OrduHareketleriniGuncelle(DayCycleManager.Instance.DevamEdenEmirler);

        if (DayCycleManager.Instance.Orders != null)
        {
            foreach (OrderData emir in DayCycleManager.Instance.Orders.BekleyenEmirler)
            {
                OrduOnizlemesiEkle(emir);
            }
        }
    }

    void SabitArkaplaniOlusturVeyaGuncelle()
    {
        // Icerik (harita icerigi) zoom/pan ile kucalup buyuyebiliyor - haritanin tamami ekrana
        // sigdirilinca (contain) kisa kenarda bosluk kalabiliyor. Bu bosluk her zaman kapali
        // kalsin diye, Icerik'in ebeveynine (goruntu alanina), zoom/pan'dan BAGIMSIZ, her zaman
        // tam ekran kaplayan sabit bir arka plan ekliyoruz.
        Transform ebeveyn = Icerik.parent;

        if (sabitArkaplan == null)
        {
            Transform mevcut = ebeveyn.Find("HaritaSabitArkaplan");
            if (mevcut != null)
            {
                sabitArkaplan = mevcut.gameObject;
            }
        }

        if (sabitArkaplan == null)
        {
            sabitArkaplan = new GameObject("HaritaSabitArkaplan");
            RectTransform rect = sabitArkaplan.AddComponent<RectTransform>();
            rect.SetParent(ebeveyn, false);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image resim = sabitArkaplan.AddComponent<Image>();
            resim.color = new Color(0.2830189f, 0.20783761f, 0.14017445f, 1f);
            resim.raycastTarget = false;
        }

        sabitArkaplan.transform.SetAsFirstSibling();
    }

    void IcerikBoyutunuGuncelle()
    {
        float maxX = 0f;
        float maxY = 0f;

        foreach (HexTileData tile in HaritaYoneticisi.Instance.Tileler)
        {
            Vector2 konum = EksenselKonum(tile.Koordinat);
            maxX = Mathf.Max(maxX, Mathf.Abs(konum.x));
            maxY = Mathf.Max(maxY, Mathf.Abs(konum.y));
        }

        Vector2 yeniBoyut = new Vector2((maxX + IcerikKenarPayi) * 2f, (maxY + IcerikKenarPayi) * 2f);
        Icerik.sizeDelta = yeniBoyut;

        // Bu script'in oldugu obje (HaritaArkaplanGorseli), haritanin arka plan rengini kaplayan
        // panel - Icerik ile ayni boyuta gelmezse buyuyen harita onun disina tasar.
        RectTransform arkaplanRect = GetComponent<RectTransform>();
        arkaplanRect.sizeDelta = yeniBoyut;
        arkaplanRect.anchoredPosition = Vector2.zero;
    }

    void Update()
    {
        if (Keyboard.current == null || tileSecimModuAktif)
        {
            return;
        }

        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            AktifGorunum = HaritaGorunumu.Siyasi;
            RenkleriGuncelle();
            GorunumMetniniGuncelle();
        }
        else if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            AktifGorunum = HaritaGorunumu.Terrain;
            RenkleriGuncelle();
            GorunumMetniniGuncelle();
        }
        else if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            AktifGorunum = HaritaGorunumu.Kaynak;
            RenkleriGuncelle();
            GorunumMetniniGuncelle();
        }
    }

    void GorunumMetniniGuncelle()
    {
        if (GorunumMetni == null)
        {
            return;
        }

        // Tile secim modunda F tuslari zaten devre disi, yazi da gizli dursun.
        GorunumMetni.gameObject.SetActive(!tileSecimModuAktif);

        string aktifAdi = AktifGorunum.ToString();
        GorunumMetni.text = "F1: Siyasi | F2: Terrain | F3: Kaynak  -  Aktif: " + aktifAdi;
    }

    Sprite YerlesimIkonuSec(YerlesimTipi tip)
    {
        switch (tip)
        {
            case YerlesimTipi.Sehir:
                return SehirIkonu;
            case YerlesimTipi.Kale:
                return KaleIkonu;
            default:
                return KoyIkonu;
        }
    }

    // Bir koyun ismi, o anki sahibinin baskenti ile ayniysa true donuyor. Her krallik kendi
    // baskentini KrallikData.BaskentIsmi ile tutuyor (bkz. KoyYoneticisi.BaskentiBul), bu
    // yuzden herhangi bir krallik (sadece oyuncu degil) icin de calisiyor.
    bool KoyBaskentMi(KoyData koy)
    {
        return koy.Sahip != null && !string.IsNullOrEmpty(koy.Sahip.BaskentIsmi)
            && string.Equals(koy.Isim.Trim(), koy.Sahip.BaskentIsmi.Trim(), System.StringComparison.OrdinalIgnoreCase);
    }

    Color TileRenginiHesapla(HexTileData tile)
    {
        if (AktifGorunum == HaritaGorunumu.Terrain || AktifGorunum == HaritaGorunumu.Kaynak)
        {
            return HaritaYoneticisi.Instance.TerrainRengi(tile);
        }
        return HaritaYoneticisi.Instance.TileRengi(tile);
    }

    public void RenkleriGuncelle()
    {
        foreach (KeyValuePair<HexTileData, Image> kv in tileGorselleri)
        {
            Color renk = TileRenginiHesapla(kv.Key);
            renk.a = TileSaydamligi;
            kv.Value.color = renk;
        }

        bool kaynakGorunuyor = AktifGorunum == HaritaGorunumu.Kaynak;

        foreach (KeyValuePair<KoyData, Image> kv in yerlesimGorselleri)
        {
            bool ikonVar = YerlesimIkonuSec(kv.Key.Tip) != null;
            Color rengi = ikonVar ? Color.white : (kv.Key.Sahip != null ? kv.Key.Sahip.HaritaRengi : Color.white);
            rengi.a = kaynakGorunuyor ? 0f : 1f;
            kv.Value.color = rengi;
        }

        foreach (KeyValuePair<HexTileData, GameObject> kv in kaynakGorselleri)
        {
            kv.Value.SetActive(kaynakGorunuyor);
        }

        foreach (KeyValuePair<KoyData, GameObject> kv in yerlesimIsimGorselleri)
        {
            kv.Value.SetActive(!kaynakGorunuyor);

            TMP_Text isimText = kv.Value.GetComponent<TMP_Text>();
            if (isimText != null)
            {
                isimText.color = kv.Key.IsyanHalinde ? Color.red : Color.white;
            }

            if (yildizGorselleri.ContainsKey(kv.Key))
            {
                yildizGorselleri[kv.Key].gameObject.SetActive(KoyBaskentMi(kv.Key));
            }
        }

        foreach (KeyValuePair<KoyData, GameObject> kv in merkezSinirGorselleri)
        {
            kv.Value.SetActive(kaynakGorunuyor);
        }

        // Gece tamamlanan yeni degirmenlerin ikonlari da cizilsin.
        DegirmenIkonlariniGuncelle();
    }

    public void SinirGoster(KoyData koy)
    {
        // Tile secim modunda sadece hedef koyun siniri gorunur, hover ile baskasi acilamaz.
        if (tileSecimModuAktif && koy != tileSecimKoyu)
        {
            return;
        }
        if (koy == null || !koyTileSinirlari.ContainsKey(koy))
        {
            return;
        }
        foreach (GameObject obje in koyTileSinirlari[koy])
        {
            obje.SetActive(true);
        }
    }

    public void SinirGizle(KoyData koy)
    {
        // Tile secim modunda hedef koyun siniri hep acik kalir, hover-cikisi onu kapatamaz.
        if (tileSecimModuAktif && koy == tileSecimKoyu)
        {
            return;
        }
        if (koy == null || !koyTileSinirlari.ContainsKey(koy))
        {
            return;
        }
        foreach (GameObject obje in koyTileSinirlari[koy])
        {
            obje.SetActive(false);
        }
    }

    HaritaGorunumu tileSecimOncesiGorunum;

    public void TileSecimModunuBaslat(KoyData koy, System.Action<HexTileData> callback)
    {
        tileSecimModuAktif = true;
        tileSecimKoyu = koy;
        tileSecimCallback = callback;

        // Secim sirasinda hangi tile'da ne kadar Erzak oldugu gorunsun diye
        // harita gecici olarak Kaynak (F3) gorunumune aliniyor.
        tileSecimOncesiGorunum = AktifGorunum;
        AktifGorunum = HaritaGorunumu.Kaynak;
        RenkleriGuncelle();
        GorunumMetniniGuncelle();

        SinirGoster(koy);
        TileSecimGorselleriniGuncelle();

        if (KapatButonu != null)
        {
            KapatButonu.SetActive(false);
        }

        // Harita, secilen koyun merkezine odaklanarak acilsin.
        if (Kontrol != null)
        {
            Kontrol.Odaklan(EksenselKonum(koy.MerkezTileKoordinati), 2f);
        }
    }

    public void TileSecimModunuBitir()
    {
        // Once mod bayragi kapatiliyor - aksi halde SinirGizle'deki "mod aktifken hedef
        // koyun siniri kapanamaz" korumasi, buradaki gizlemeyi de engelliyor.
        KoyData kapanacakKoy = tileSecimKoyu;
        tileSecimModuAktif = false;
        tileSecimKoyu = null;
        tileSecimCallback = null;
        SinirGizle(kapanacakKoy);

        AktifGorunum = tileSecimOncesiGorunum;
        RenkleriGuncelle();
        GorunumMetniniGuncelle();

        if (KapatButonu != null)
        {
            KapatButonu.SetActive(true);
        }

        TileSecimGorselleriniGuncelle();
    }

    public void DegirmenIkonlariniGuncelle()
    {
        foreach (HexTileData tile in HaritaYoneticisi.Instance.Tileler)
        {
            if (!tile.DegirmenVar || degirmenGorselleri.ContainsKey(tile))
            {
                continue;
            }

            GameObject obje = new GameObject("Degirmen_" + tile.Koordinat.x + "_" + tile.Koordinat.y);
            RectTransform rect = SabitCapaliRectOlustur(obje, Icerik);

            Image resim = obje.AddComponent<Image>();
            resim.sprite = DegirmenIkonu != null ? DegirmenIkonu : daireSprite;
            resim.color = DegirmenIkonu != null ? Color.white : new Color(0.85f, 0.7f, 0.4f, 1f);
            resim.raycastTarget = false;

            rect.sizeDelta = new Vector2(DegirmenIkonBoyutu, DegirmenIkonBoyutu);
            rect.anchoredPosition = EksenselKonum(tile.Koordinat);

            // Degirmen ikonu, yerlesim ikonlarinin/isimlerinin ARKASINDA cizilsin diye
            // Hierarchy'de onlardan once (ustte) bir siraya taşınıyor.
            int enKucukYerlesimSirasi = int.MaxValue;
            foreach (KeyValuePair<KoyData, Image> yerlesim in yerlesimGorselleri)
            {
                enKucukYerlesimSirasi = Mathf.Min(enKucukYerlesimSirasi, yerlesim.Value.transform.GetSiblingIndex());
            }
            if (enKucukYerlesimSirasi != int.MaxValue)
            {
                obje.transform.SetSiblingIndex(enKucukYerlesimSirasi);
            }

            degirmenGorselleri[tile] = obje;
        }
    }

    public void TileHover(HexTileData tile, bool hoverda)
    {
        // Sadece tile secim modunda, secilebilir (hedef koye ait ve bos) tile'larda hover etkisi var.
        if (!tileSecimModuAktif || tile.SahipKoy != tileSecimKoyu || tile.DegirmenVar)
        {
            return;
        }

        if (!tileGorselleri.ContainsKey(tile))
        {
            return;
        }

        tileGorselleri[tile].color = hoverda
            ? new Color(0.7f, 0.58f, 0.12f, 0.75f)
            : new Color(1f, 0.85f, 0.2f, 0.6f);
    }

    public void TileTiklandi(HexTileData tile)
    {
        if (!tileSecimModuAktif || tile.SahipKoy != tileSecimKoyu || tile.DegirmenVar)
        {
            return;
        }

        System.Action<HexTileData> callback = tileSecimCallback;
        TileSecimModunuBitir();
        callback?.Invoke(tile);
    }

    void TileSecimGorselleriniGuncelle()
    {
        foreach (KeyValuePair<HexTileData, Image> kv in tileGorselleri)
        {
            HexTileData tile = kv.Key;
            Image resim = kv.Value;

            if (!tileSecimModuAktif)
            {
                Color normalRenk = TileRenginiHesapla(tile);
                normalRenk.a = TileSaydamligi;
                resim.color = normalRenk;
                resim.raycastTarget = false;

                if (degirmenGorselleri.ContainsKey(tile))
                {
                    degirmenGorselleri[tile].SetActive(true);
                }
                continue;
            }

            bool bizimTile = tile.SahipKoy == tileSecimKoyu;
            bool doluTile = bizimTile && tile.DegirmenVar;
            bool secilebilir = bizimTile && !doluTile;

            resim.raycastTarget = secilebilir;

            if (secilebilir)
            {
                resim.color = new Color(1f, 0.85f, 0.2f, 0.6f);
            }
            else if (doluTile)
            {
                resim.color = new Color(0.5f, 0.1f, 0.1f, 0.5f);
            }
            else
            {
                // Secim modunda hedef koy disindaki tile'lar tamamen gizli.
                resim.color = new Color(0f, 0f, 0f, 0f);
            }

            // Erzak sayilari da sadece hedef koyun tile'larinda gorunsun.
            if (kaynakGorselleri.ContainsKey(tile))
            {
                kaynakGorselleri[tile].SetActive(bizimTile);
            }

            // Degirmen ikonlari da secim modunda sadece hedef koyde gorunsun.
            if (degirmenGorselleri.ContainsKey(tile))
            {
                degirmenGorselleri[tile].SetActive(!tileSecimModuAktif || bizimTile);
            }
        }

        if (tileSecimModuAktif)
        {
            // Diger koylerin merkez cerceveleri de secim sirasinda gizlensin.
            foreach (KeyValuePair<KoyData, GameObject> kv in merkezSinirGorselleri)
            {
                kv.Value.SetActive(kv.Key == tileSecimKoyu);
            }
        }

        // Secim modunda gorunmez yerlesim ikonlari hover/tiklama yakalamasin.
        foreach (KeyValuePair<KoyData, Image> kv in yerlesimGorselleri)
        {
            kv.Value.raycastTarget = !tileSecimModuAktif;
        }
    }

    void EskiCizimleriTemizle()
    {
        for (int i = Icerik.childCount - 1; i >= 0; i--)
        {
            Transform cocuk = Icerik.GetChild(i);
            if (cocuk.name.StartsWith("Tile_") || cocuk.name.StartsWith("Yerlesim_") || cocuk.name.StartsWith("Kaynak_") || cocuk.name.StartsWith("MerkezSinir_") || cocuk.name.StartsWith("Degirmen_") || cocuk.name.StartsWith("OrduYolu"))
            {
                Destroy(cocuk.gameObject);
            }
        }
    }

    void TileleriCiz()
    {
        foreach (HexTileData tile in HaritaYoneticisi.Instance.Tileler)
        {
            GameObject obje = new GameObject("Tile_" + tile.Koordinat.x + "_" + tile.Koordinat.y);
            RectTransform rect = SabitCapaliRectOlustur(obje, Icerik);

            Image resim = obje.AddComponent<Image>();
            resim.sprite = hexagonSprite;
            Color renk = TileRenginiHesapla(tile);
            renk.a = TileSaydamligi;
            resim.color = renk;
            resim.raycastTarget = false;
            tileGorselleri[tile] = resim;

            TileTiklama tileTiklama = obje.AddComponent<TileTiklama>();
            tileTiklama.Tile = tile;

            rect.sizeDelta = new Vector2(TileBoyutu * Mathf.Sqrt(3f), TileBoyutu * 2f);
            rect.anchoredPosition = EksenselKonum(tile.Koordinat);

            if (tile.SahipKoy != null)
            {
                GameObject sinirObjesi = new GameObject("Sinir_" + tile.Koordinat.x + "_" + tile.Koordinat.y);
                RectTransform sinirRect = SabitCapaliRectOlustur(sinirObjesi, Icerik);

                Image sinirResim = sinirObjesi.AddComponent<Image>();
                sinirResim.sprite = hexagonCerceveSprite;
                sinirResim.color = SinirRengi;
                sinirResim.raycastTarget = false;

                sinirRect.sizeDelta = rect.sizeDelta;
                sinirRect.anchoredPosition = rect.anchoredPosition;

                sinirObjesi.SetActive(false);

                if (!koyTileSinirlari.ContainsKey(tile.SahipKoy))
                {
                    koyTileSinirlari[tile.SahipKoy] = new List<GameObject>();
                }
                koyTileSinirlari[tile.SahipKoy].Add(sinirObjesi);
            }

            float bosluk = 2f;
            float metinGenislik = 24f;
            float kaynakGenislik = KaynakIkonBoyutu + bosluk * 3f + metinGenislik;
            float kaynakYukseklik = KaynakIkonBoyutu + 2f;

            GameObject kaynakObjesi = new GameObject("Kaynak_" + tile.Koordinat.x + "_" + tile.Koordinat.y);
            RectTransform kaynakRect = SabitCapaliRectOlustur(kaynakObjesi, Icerik);
            kaynakRect.sizeDelta = new Vector2(kaynakGenislik, kaynakYukseklik);
            kaynakRect.anchoredPosition = rect.anchoredPosition;

            GameObject kaynakArkaplanObjesi = new GameObject("KaynakArkaplan");
            RectTransform kaynakArkaplanRect = SabitCapaliRectOlustur(kaynakArkaplanObjesi, kaynakObjesi.transform);
            kaynakArkaplanRect.sizeDelta = new Vector2(kaynakGenislik, kaynakYukseklik);
            kaynakArkaplanRect.anchoredPosition = Vector2.zero;

            Image kaynakArkaplan = kaynakArkaplanObjesi.AddComponent<Image>();
            kaynakArkaplan.sprite = kareSprite;
            kaynakArkaplan.color = new Color(0f, 0f, 0f, 0.45f);
            kaynakArkaplan.raycastTarget = false;

            GameObject kaynakIkonObjesi = new GameObject("KaynakIkon");
            RectTransform kaynakIkonRect = SabitCapaliRectOlustur(kaynakIkonObjesi, kaynakObjesi.transform);
            kaynakIkonRect.sizeDelta = new Vector2(KaynakIkonBoyutu, KaynakIkonBoyutu);
            kaynakIkonRect.anchoredPosition = new Vector2(-kaynakGenislik / 2f + bosluk + KaynakIkonBoyutu / 2f, 0f);

            Image kaynakIkon = kaynakIkonObjesi.AddComponent<Image>();
            kaynakIkon.sprite = daireSprite;
            kaynakIkon.color = ErzakIkonRengi;
            kaynakIkon.raycastTarget = false;

            GameObject kaynakMetinObjesi = new GameObject("KaynakMetin");
            RectTransform kaynakMetinRect = SabitCapaliRectOlustur(kaynakMetinObjesi, kaynakObjesi.transform);
            kaynakMetinRect.sizeDelta = new Vector2(metinGenislik, kaynakYukseklik);
            kaynakMetinRect.anchoredPosition = new Vector2(-kaynakGenislik / 2f + bosluk * 2f + KaynakIkonBoyutu + metinGenislik / 2f, 0f);

            TMP_Text kaynakMetin = kaynakMetinObjesi.AddComponent<TextMeshProUGUI>();
            kaynakMetin.enableAutoSizing = false;
            kaynakMetin.enableWordWrapping = false;
            kaynakMetin.overflowMode = TextOverflowModes.Overflow;
            kaynakMetin.fontSize = 12f;
            kaynakMetin.fontStyle = FontStyles.Bold;
            kaynakMetin.alignment = TextAlignmentOptions.Center;
            kaynakMetin.raycastTarget = false;
            kaynakMetin.color = Color.white;
            kaynakMetin.text = "+" + tile.ErzakDegeri;

            kaynakObjesi.SetActive(AktifGorunum == HaritaGorunumu.Kaynak);
            kaynakGorselleri[tile] = kaynakObjesi;
        }
    }

    void YerlesimleriCiz()
    {
        foreach (KoyData koy in KoyYoneticisi.Instance.Koyler)
        {
            GameObject obje = new GameObject("Yerlesim_" + koy.Isim);
            RectTransform rect = SabitCapaliRectOlustur(obje, Icerik);

            Sprite yerlesimIkonu = YerlesimIkonuSec(koy.Tip);

            Image resim = obje.AddComponent<Image>();
            resim.sprite = yerlesimIkonu != null ? yerlesimIkonu : kareSprite;
            resim.color = yerlesimIkonu != null ? Color.white : (koy.Sahip != null ? koy.Sahip.HaritaRengi : Color.white);
            yerlesimGorselleri[koy] = resim;

            float boyut = YerlesimBoyutu * (1f + 0.25f * (koy.TileMenzili - 1));

            rect.sizeDelta = new Vector2(boyut, boyut);
            rect.anchoredPosition = EksenselKonum(koy.MerkezTileKoordinati);

            YerlesimIsaretiTiklama tiklama = obje.AddComponent<YerlesimIsaretiTiklama>();
            tiklama.Koy = koy;

            GameObject isimObjesi = new GameObject("Isim");
            RectTransform isimRect = SabitCapaliRectOlustur(isimObjesi, obje.transform);
            isimRect.sizeDelta = new Vector2(160f, 30f);
            isimRect.anchoredPosition = new Vector2(0f, boyut / 2f + 18f);

            TMP_Text isimText = isimObjesi.AddComponent<TextMeshProUGUI>();
            isimText.enableAutoSizing = false;
            isimText.enableWordWrapping = false;
            isimText.overflowMode = TextOverflowModes.Overflow;
            isimText.fontSize = 18f;
            isimText.alignment = TextAlignmentOptions.Center;
            isimText.raycastTarget = false;
            isimText.color = koy.IsyanHalinde ? Color.red : Color.white;
            isimText.text = koy.Isim;

            isimObjesi.SetActive(AktifGorunum != HaritaGorunumu.Kaynak);
            yerlesimIsimGorselleri[koy] = isimObjesi;

            // Baskent isaretini isim metninin karakteri olarak degil, ayri bir sprite ikonu
            // olarak (TMP font'unda "★" karakteri olmayabildigi icin, bkz. SekilUretici.YildizSprite)
            // ismin hemen soluna cizip sadece baskent olan yerlesimlerde aktif ediyoruz. Isim
            // ortalanmis oldugu (Center alignment) icin sabit kutu genisligi (160) yerine
            // metnin GERCEK genisligini olcup ona gore yakinlastiriyoruz.
            float isimGenisligi = isimText.GetPreferredValues().x;
            GameObject yildizObjesi = new GameObject("Yildiz");
            RectTransform yildizRect = SabitCapaliRectOlustur(yildizObjesi, isimObjesi.transform);
            yildizRect.sizeDelta = new Vector2(YildizBoyutu, YildizBoyutu);
            yildizRect.anchoredPosition = new Vector2(-(isimGenisligi / 2f) - YildizBoyutu / 2f - 2f, 0f);

            Image yildizResim = yildizObjesi.AddComponent<Image>();
            yildizResim.sprite = yildizSprite;
            yildizResim.color = YildizRengi;
            yildizResim.raycastTarget = false;

            yildizObjesi.SetActive(KoyBaskentMi(koy));
            yildizGorselleri[koy] = yildizResim;

            GameObject merkezSinirObjesi = new GameObject("MerkezSinir_" + koy.Isim);
            RectTransform merkezSinirRect = SabitCapaliRectOlustur(merkezSinirObjesi, Icerik);
            merkezSinirRect.sizeDelta = new Vector2(TileBoyutu * Mathf.Sqrt(3f), TileBoyutu * 2f);
            merkezSinirRect.anchoredPosition = EksenselKonum(koy.MerkezTileKoordinati);

            Image merkezSinirResim = merkezSinirObjesi.AddComponent<Image>();
            merkezSinirResim.sprite = hexagonCerceveSprite;
            merkezSinirResim.color = SinirRengi;
            merkezSinirResim.raycastTarget = false;

            merkezSinirObjesi.SetActive(AktifGorunum == HaritaGorunumu.Kaynak);
            merkezSinirGorselleri[koy] = merkezSinirObjesi;
        }
    }

    RectTransform SabitCapaliRectOlustur(GameObject obje, Transform ebeveyn)
    {
        RectTransform rect = obje.AddComponent<RectTransform>();
        rect.SetParent(ebeveyn, false);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.localScale = Vector3.one;
        return rect;
    }

    // Her devam eden (cok gunlu) emir icin, KaynakKoy'u olanlari haritada A koyunden B koyune
    // giden bir ok+asker gorseliyle gosterir. Genel Yedek Kuvvet'ten (KaynakKoy == null) giden
    // emirler zaten hep 1 gunde vardigi icin gosterilmiyor. DayCycleManager.UyuyaBas() her gece
    // bunu cagirip gorselleri o gecenin KalanGun'una gore guncelliyor.
    public void OrduHareketleriniGuncelle(List<DevamEdenEmir> devamEdenler)
    {
        HashSet<OrderData> aktifEmirler = new HashSet<OrderData>();

        foreach (DevamEdenEmir devam in devamEdenler)
        {
            OrderData emir = devam.Emir;
            KoyData efektifKaynak = EfektifKaynakKoy(emir);
            if (efektifKaynak == null || emir.HedefKoy == null)
            {
                continue;
            }

            aktifEmirler.Add(emir);

            float ilerleme = emir.ToplamSure <= 0
                ? 1f
                : Mathf.Clamp01((float)(emir.ToplamSure - devam.KalanGun) / emir.ToplamSure);

            Vector2 kaynakKonum = EksenselKonum(efektifKaynak.MerkezTileKoordinati);
            Vector2 hedefKonum = EksenselKonum(emir.HedefKoy.MerkezTileKoordinati);
            Vector2 guncelKonum = Vector2.Lerp(kaynakKonum, hedefKonum, ilerleme);

            OrduGorseliniGetirVeyaOlustur(emir, efektifKaynak).Guncelle(guncelKonum, hedefKonum);
        }

        List<OrderData> tamamlananlar = new List<OrderData>();
        foreach (KeyValuePair<OrderData, OrduYoluGorseli> kv in orduGorselleri)
        {
            if (!aktifEmirler.Contains(kv.Key))
            {
                if (kv.Value != null)
                {
                    Destroy(kv.Value.gameObject);
                }
                tamamlananlar.Add(kv.Key);
            }
        }
        foreach (OrderData emir in tamamlananlar)
        {
            orduGorselleri.Remove(emir);
        }
    }

    // Emir verildigi an (henuz Uyu'ya basilmadan, DevamEdenEmir listesine girmeden once)
    // birligin kaynak koyde durdugunu gostermek icin cagriliyor - DialogueManager.ManpowerAdimi'nda.
    public void OrduOnizlemesiEkle(OrderData emir)
    {
        KoyData efektifKaynak = EfektifKaynakKoy(emir);
        if (efektifKaynak == null || emir.HedefKoy == null)
        {
            return;
        }

        Vector2 kaynakKonum = EksenselKonum(efektifKaynak.MerkezTileKoordinati);
        Vector2 hedefKonum = EksenselKonum(emir.HedefKoy.MerkezTileKoordinati);
        OrduGorseliniGetirVeyaOlustur(emir, efektifKaynak).Guncelle(kaynakKonum, hedefKonum);
    }

    // Emrin KaynakKoy'u null ise (Genel Yedek Kuvvet secildiyse), artik Baskent'i gercek
    // kaynak konumu sayiyoruz - bkz. KoyYoneticisi.Baskent. Baskent atanmadiysa (null) donuyor,
    // bu durumda o emir icin hareket gorseli/ok cizilmiyor (eski davranis).
    KoyData EfektifKaynakKoy(OrderData emir)
    {
        if (emir.KaynakKoy != null)
        {
            return emir.KaynakKoy;
        }
        return KoyYoneticisi.Instance != null ? KoyYoneticisi.Instance.Baskent : null;
    }

    OrduYoluGorseli OrduGorseliniGetirVeyaOlustur(OrderData emir, KoyData kaynakKoy)
    {
        if (orduGorselleri.ContainsKey(emir))
        {
            return orduGorselleri[emir];
        }

        GameObject obje = new GameObject("OrduYolu_" + kaynakKoy.Isim + "_" + emir.HedefKoy.Isim);
        OrduYoluGorseli gorsel = obje.AddComponent<OrduYoluGorseli>();
        Sprite askerSprite = AskerIkonu != null ? AskerIkonu : kareSprite;
        gorsel.Kur(Icerik, okSprite, askerSprite, OkParcaSayisi, OkParcaBoyutu,
            AskerIkonuBoyutu, OrduOkuRengi, Color.white, OkYanipSonmeHizi, OkParcaAraligi);
        orduGorselleri[emir] = gorsel;
        return gorsel;
    }

    Vector2 EksenselKonum(Vector3Int koordinat)
    {
        Vector3Int goreli = koordinat - HaritaYoneticisi.Instance.HaritaMerkezi;
        float x = TileBoyutu * (Mathf.Sqrt(3f) * goreli.x + Mathf.Sqrt(3f) / 2f * goreli.y);
        float y = TileBoyutu * (1.5f * goreli.y);
        return new Vector2(x, y);
    }
}
