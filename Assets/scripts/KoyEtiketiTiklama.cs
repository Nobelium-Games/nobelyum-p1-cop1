using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KoyEtiketiTiklama : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color HoverRengi = Color.yellow;

    private TMP_Text metin;
    private Color normalRenk;

    void Awake()
    {
        metin = GetComponent<TMP_Text>();
        normalRenk = metin.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        metin.color = HoverRengi;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        metin.color = normalRenk;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string etiketIsmi = GetComponent<TMP_Text>().text.Trim();

        foreach (KoyData koy in KoyYoneticisi.Instance.Koyler)
        {
            if (!string.IsNullOrEmpty(koy.Isim) && string.Equals(koy.Isim.Trim(), etiketIsmi, System.StringComparison.OrdinalIgnoreCase))
            {
                KoyBilgiPaneli.Instance.Goster(koy);
                return;
            }
        }

        Debug.LogWarning("Eslesen koy bulunamadi: '" + etiketIsmi + "'");
    }
}
