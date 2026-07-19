using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Haritada bir birligin A koyunden B koyune giderken cizilen kesik kesik ok + asker ikonu.
// HexHaritaCizici, her devam eden (cok gunlu) emir icin bir tane bunu olusturup Guncelle() cagiriyor.
public class OrduYoluGorseli : MonoBehaviour
{
    List<Image> okParcalari = new List<Image>();
    Image askerIkon;
    RectTransform kokRect;
    float yanipSonmeHizi = 4f;
    float okAraligi = 36f;

    public void Kur(Transform ebeveyn, Sprite okSprite, Sprite askerSprite, int okParcaHavuzu,
        float okParcaBoyutu, float askerBoyutu, Color okRengi, Color askerRengi,
        float yanipSonmeHizi, float okAraligi)
    {
        this.yanipSonmeHizi = yanipSonmeHizi;
        this.okAraligi = okAraligi;

        kokRect = gameObject.AddComponent<RectTransform>();
        kokRect.SetParent(ebeveyn, false);
        kokRect.anchorMin = new Vector2(0.5f, 0.5f);
        kokRect.anchorMax = new Vector2(0.5f, 0.5f);
        kokRect.pivot = new Vector2(0.5f, 0.5f);
        kokRect.anchoredPosition = Vector2.zero;
        kokRect.localScale = Vector3.one;

        // Havuz, olasi en uzun yolculuk icin yetecek kadar ok parcasi iceriyor - mesafe
        // kisaldikca fazlasi gizleniyor (bkz. Guncelle), sayilari degil araliklari sabit tutmak icin.
        for (int i = 0; i < okParcaHavuzu; i++)
        {
            GameObject parca = new GameObject("OkParcasi_" + i);
            RectTransform parcaRect = parca.AddComponent<RectTransform>();
            parcaRect.SetParent(kokRect, false);
            parcaRect.anchorMin = new Vector2(0.5f, 0.5f);
            parcaRect.anchorMax = new Vector2(0.5f, 0.5f);
            parcaRect.pivot = new Vector2(0.5f, 0.5f);
            parcaRect.sizeDelta = new Vector2(okParcaBoyutu, okParcaBoyutu);
            parcaRect.localScale = Vector3.one;

            Image resim = parca.AddComponent<Image>();
            resim.sprite = okSprite;
            resim.color = okRengi;
            resim.raycastTarget = false;
            okParcalari.Add(resim);
        }

        GameObject asker = new GameObject("AskerIkon");
        RectTransform askerRect = asker.AddComponent<RectTransform>();
        askerRect.SetParent(kokRect, false);
        askerRect.anchorMin = new Vector2(0.5f, 0.5f);
        askerRect.anchorMax = new Vector2(0.5f, 0.5f);
        askerRect.pivot = new Vector2(0.5f, 0.5f);
        askerRect.sizeDelta = new Vector2(askerBoyutu, askerBoyutu);
        askerRect.localScale = Vector3.one;

        askerIkon = asker.AddComponent<Image>();
        askerIkon.sprite = askerSprite;
        askerIkon.color = askerRengi;
        askerIkon.raycastTarget = false;
    }

    // baslangic: birligin su anki konumu (kaynaktan hedefe dogru ilerledikce degisir)
    // bitis: hedef koyun sabit konumu
    public void Guncelle(Vector2 baslangic, Vector2 bitis)
    {
        Vector2 fark = bitis - baslangic;
        float mesafe = fark.magnitude;

        if (mesafe < 1f)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        Vector2 yon = fark / mesafe;
        // Ok ucu sprite'i varsayilan olarak asagi (-Y) bakiyor, o yuzden +90 ekliyoruz.
        float aci = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg + 90f;
        Quaternion rotasyon = Quaternion.Euler(0f, 0f, aci);

        // Mesafeyi sabit sayida parcaya bolmek yerine, hedeften geriye dogru SABIT
        // araliklarla diziyoruz - boylece mesafe kisaldikca aralik degil, sadece
        // gorunen parca SAYISI azaliyor. i. parca bitis'ten (araligi*(i+0.5)) kadar uzakta
        // oldugu icin, bu mesafenin baslangic'i asmamasi (asker ikonuyla ust uste binmemesi)
        // icin -0.5 duzeltmesiyle hesaplaniyor.
        int gosterilecekSayi = Mathf.Clamp(
            Mathf.FloorToInt(mesafe / okAraligi - 0.5f) + 1, 0, okParcalari.Count);

        for (int i = 0; i < okParcalari.Count; i++)
        {
            bool gorunur = i < gosterilecekSayi;
            okParcalari[i].gameObject.SetActive(gorunur);
            if (!gorunur)
            {
                continue;
            }

            Vector2 konum = bitis - yon * (okAraligi * (i + 0.5f));
            RectTransform rect = okParcalari[i].rectTransform;
            rect.anchoredPosition = konum;
            rect.localRotation = rotasyon;
        }

        askerIkon.rectTransform.anchoredPosition = baslangic;
        askerIkon.rectTransform.localRotation = rotasyon;
    }

    void Update()
    {
        for (int i = 0; i < okParcalari.Count; i++)
        {
            if (!okParcalari[i].gameObject.activeSelf)
            {
                continue;
            }

            float dalga = Mathf.Sin(Time.time * yanipSonmeHizi - i * 0.9f);
            float alfa = 0.25f + 0.75f * (0.5f + 0.5f * dalga);
            Color renk = okParcalari[i].color;
            renk.a = alfa;
            okParcalari[i].color = renk;
        }
    }
}
