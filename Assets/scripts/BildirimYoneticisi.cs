using UnityEngine;
using TMPro;
using System.Collections;

public class BildirimYoneticisi : MonoBehaviour
{
    public static BildirimYoneticisi Instance;

    public GameObject BildirimSablonu;
    public float GosterimSuresi = 2f;
    public float FadeSuresi = 0.4f;

    void Awake()
    {
        Instance = this;
        BildirimSablonu.SetActive(false);
    }

    public void Bildirim(string statAdi, int miktar)
    {
        GameObject yeni = Instantiate(BildirimSablonu, BildirimSablonu.transform.parent);
        yeni.SetActive(true);

        string isaret = miktar >= 0 ? "+" : "";
        string renk = miktar >= 0 ? "green" : "red";

        TMP_Text metin = yeni.GetComponent<TMP_Text>();
        metin.text = "<color=" + renk + ">" + isaret + miktar + " " + statAdi + "</color>";

        CanvasGroup canvasGroup = yeni.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = yeni.AddComponent<CanvasGroup>();
        }

        StartCoroutine(FadeInFadeOutYokEt(yeni, canvasGroup));
    }

    IEnumerator FadeInFadeOutYokEt(GameObject obje, CanvasGroup canvasGroup)
    {
        yield return FadeCoroutine(canvasGroup, 0f, 1f, FadeSuresi);

        yield return new WaitForSeconds(GosterimSuresi);

        yield return FadeCoroutine(canvasGroup, 1f, 0f, FadeSuresi);

        Destroy(obje);
    }

    IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float baslangic, float bitis, float sure)
    {
        float gecenSure = 0f;
        canvasGroup.alpha = baslangic;

        while (gecenSure < sure)
        {
            gecenSure += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(baslangic, bitis, gecenSure / sure);
            yield return null;
        }

        canvasGroup.alpha = bitis;
    }
}
