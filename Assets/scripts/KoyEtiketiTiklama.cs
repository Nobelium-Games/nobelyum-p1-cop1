using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KoyEtiketiTiklama : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color HoverRengi = Color.yellow;
    public Color IsyanRengi = Color.red;

    private TMP_Text metin;
    private Color normalRenk;

    void Awake()
    {
        metin = GetComponent<TMP_Text>();
        normalRenk = metin.color;
    }

    void OnEnable()
    {
        GuncelleRenk();
    }

    void GuncelleRenk()
    {
        KoyData koy = BulKoy();

        if (koy != null && koy.IsyanHalinde)
        {
            metin.color = IsyanRengi;
        }
        else if (koy != null && koy.Sahip != null && koy.Sahip != KoyYoneticisi.Instance.OyuncuKralligi)
        {
            metin.color = koy.Sahip.HaritaRengi;
        }
        else
        {
            metin.color = normalRenk;
        }
    }

    KoyData BulKoy()
    {
        string etiketIsmi = metin.text.Trim();

        foreach (KoyData koy in KoyYoneticisi.Instance.Koyler)
        {
            if (!string.IsNullOrEmpty(koy.Isim) && string.Equals(koy.Isim.Trim(), etiketIsmi, System.StringComparison.OrdinalIgnoreCase))
            {
                return koy;
            }
        }

        return null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        metin.color = HoverRengi;

        KoyData koy = BulKoy();
        if (koy != null && koy.Sahip != null)
        {
            TooltipUI.Instance.Goster(koy.Sahip.Isim);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GuncelleRenk();
        TooltipUI.Instance.Gizle();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        KoyData koy = BulKoy();

        if (koy != null)
        {
            KoyBilgiPaneli.Instance.Goster(koy);
        }
        else
        {
            Debug.LogWarning("Eslesen koy bulunamadi: '" + metin.text.Trim() + "'");
        }
    }
}
