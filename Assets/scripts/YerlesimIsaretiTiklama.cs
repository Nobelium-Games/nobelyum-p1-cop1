using UnityEngine;
using UnityEngine.EventSystems;

public class YerlesimIsaretiTiklama : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public KoyData Koy;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Koy != null && HexHaritaCizici.Instance != null)
        {
            HexHaritaCizici.Instance.SinirGoster(Koy);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Koy != null && HexHaritaCizici.Instance != null)
        {
            HexHaritaCizici.Instance.SinirGizle(Koy);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Koy == null)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Koy.Sahip != null && Koy.Sahip != KoyYoneticisi.Instance.OyuncuKralligi)
            {
                DiplomasiBilgiPaneli.Instance.Goster(Koy.Sahip);
            }
            return;
        }

        KoyBilgiPaneli.Instance.Goster(Koy);
    }
}
