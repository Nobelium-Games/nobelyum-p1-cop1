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
    public Sprite YerlesimIkonu;
    public TMP_Text GorunumMetni;

    Sprite hexagonSprite;
    Sprite hexagonCerceveSprite;
    Sprite kareSprite;
    Sprite daireSprite;

    Dictionary<KoyData, List<GameObject>> koyTileSinirlari = new Dictionary<KoyData, List<GameObject>>();
    Dictionary<HexTileData, Image> tileGorselleri = new Dictionary<HexTileData, Image>();
    Dictionary<KoyData, Image> yerlesimGorselleri = new Dictionary<KoyData, Image>();
    Dictionary<HexTileData, GameObject> kaynakGorselleri = new Dictionary<HexTileData, GameObject>();
    Dictionary<KoyData, GameObject> yerlesimIsimGorselleri = new Dictionary<KoyData, GameObject>();
    Dictionary<KoyData, GameObject> merkezSinirGorselleri = new Dictionary<KoyData, GameObject>();

    void Start()
    {
        Instance = this;

        hexagonSprite = SekilUretici.HexagonSprite();
        hexagonCerceveSprite = SekilUretici.HexagonCerceveSprite();
        kareSprite = SekilUretici.KareSprite();
        daireSprite = SekilUretici.DaireSprite();

        EskiCizimleriTemizle();
        koyTileSinirlari.Clear();
        tileGorselleri.Clear();
        yerlesimGorselleri.Clear();
        kaynakGorselleri.Clear();
        yerlesimIsimGorselleri.Clear();
        merkezSinirGorselleri.Clear();
        TileleriCiz();
        YerlesimleriCiz();
        GorunumMetniniGuncelle();
    }

    void Update()
    {
        if (Keyboard.current == null)
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

        string aktifAdi = AktifGorunum.ToString();
        GorunumMetni.text = "F1: Siyasi | F2: Terrain | F3: Kaynak  -  Aktif: " + aktifAdi;
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
            Color rengi = YerlesimIkonu != null ? Color.white : (kv.Key.Sahip != null ? kv.Key.Sahip.HaritaRengi : Color.white);
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
        }

        foreach (KeyValuePair<KoyData, GameObject> kv in merkezSinirGorselleri)
        {
            kv.Value.SetActive(kaynakGorunuyor);
        }
    }

    public void SinirGoster(KoyData koy)
    {
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
        if (koy == null || !koyTileSinirlari.ContainsKey(koy))
        {
            return;
        }
        foreach (GameObject obje in koyTileSinirlari[koy])
        {
            obje.SetActive(false);
        }
    }

    void EskiCizimleriTemizle()
    {
        for (int i = Icerik.childCount - 1; i >= 0; i--)
        {
            Transform cocuk = Icerik.GetChild(i);
            if (cocuk.name.StartsWith("Tile_") || cocuk.name.StartsWith("Yerlesim_") || cocuk.name.StartsWith("Kaynak_") || cocuk.name.StartsWith("MerkezSinir_"))
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

            Image resim = obje.AddComponent<Image>();
            resim.sprite = YerlesimIkonu != null ? YerlesimIkonu : kareSprite;
            resim.color = YerlesimIkonu != null ? Color.white : (koy.Sahip != null ? koy.Sahip.HaritaRengi : Color.white);
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
            isimText.color = Color.white;
            isimText.text = koy.Isim;

            isimObjesi.SetActive(AktifGorunum != HaritaGorunumu.Kaynak);
            yerlesimIsimGorselleri[koy] = isimObjesi;

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

    Vector2 EksenselKonum(Vector3Int koordinat)
    {
        Vector3Int goreli = koordinat - HaritaYoneticisi.Instance.HaritaMerkezi;
        float x = TileBoyutu * (Mathf.Sqrt(3f) * goreli.x + Mathf.Sqrt(3f) / 2f * goreli.y);
        float y = TileBoyutu * (1.5f * goreli.y);
        return new Vector2(x, y);
    }
}
