using UnityEngine;
using System.Collections;

public class EkranGecisi : MonoBehaviour
{
    public static EkranGecisi Instance;

    public CanvasGroup KaraPanel;
    public float GecisSuresi = 0.3f;

    void Awake()
    {
        Instance = this;
        KaraPanel.alpha = 0f;
        KaraPanel.blocksRaycasts = false;
    }

    public IEnumerator KararipAcil(System.Action ortadaYapilacakIs)
    {
        KaraPanel.blocksRaycasts = true;

        yield return Fade(0f, 1f);

        ortadaYapilacakIs?.Invoke();

        yield return Fade(1f, 0f);

        KaraPanel.blocksRaycasts = false;
    }

    IEnumerator Fade(float baslangic, float bitis)
    {
        float gecenSure = 0f;
        KaraPanel.alpha = baslangic;

        while (gecenSure < GecisSuresi)
        {
            gecenSure += Time.deltaTime;
            KaraPanel.alpha = Mathf.Lerp(baslangic, bitis, gecenSure / GecisSuresi);
            yield return null;
        }

        KaraPanel.alpha = bitis;
    }
}
