using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexHaritaCizici : MonoBehaviour
{
    public static HexHaritaCizici Instance;

    public RectTransform Icerik;

    public float TileBoyutu = 40f;
    public float TileSaydamligi = 0.35f;
    public float YerlesimBoyutu = 50f;
    public Color SinirRengi = Color.black;
    public Sprite YerlesimIkonu;

    Sprite hexagonSprite;
    Sprite hexagonCerceveSprite;
    Sprite kareSprite;

    Dictionary<KoyData, List<GameObject>> koyTileSinirlari = new Dictionary<KoyData, List<GameObject>>();
    Dictionary<HexTileData, Image> tileGorselleri = new Dictionary<HexTileData, Image>();
    Dictionary<KoyData, Image> yerlesimGorselleri = new Dictionary<KoyData, Image>();

    void Start()
    {
        Instance = this;

        hexagonSprite = SekilUretici.HexagonSprite();
        hexagonCerceveSprite = SekilUretici.HexagonCerceveSprite();
        kareSprite = SekilUretici.KareSprite();

        EskiCizimleriTemizle();
        koyTileSinirlari.Clear();
        tileGorselleri.Clear();
        yerlesimGorselleri.Clear();
        TileleriCiz();
        YerlesimleriCiz();
    }

    public void RenkleriGuncelle()
    {
        foreach (KeyValuePair<HexTileData, Image> kv in tileGorselleri)
        {
            Color renk = HaritaYoneticisi.Instance.TileRengi(kv.Key);
            renk.a = TileSaydamligi;
            kv.Value.color = renk;
        }

        if (YerlesimIkonu == null)
        {
            foreach (KeyValuePair<KoyData, Image> kv in yerlesimGorselleri)
            {
                kv.Value.color = kv.Key.Sahip != null ? kv.Key.Sahip.HaritaRengi : Color.white;
            }
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
            if (cocuk.name.StartsWith("Tile_") || cocuk.name.StartsWith("Yerlesim_"))
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
            Color renk = HaritaYoneticisi.Instance.TileRengi(tile);
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
