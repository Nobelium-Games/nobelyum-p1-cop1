using UnityEngine;
using UnityEngine.EventSystems;

public class YerlesimIsaretiTiklama : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IScrollHandler
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
        if (Koy == null || eventData.dragging)
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

    // Yerlesim ikonlari raycast yakaladigi icin harita pan/zoom olaylari HaritaKontrol'e
    // ulasmiyor, bu yuzden burada yakalayip elle iletiyoruz.
    public void OnBeginDrag(PointerEventData eventData)
    {
        HaritaKontrol kontrol = HexHaritaCizici.Instance != null ? HexHaritaCizici.Instance.Kontrol : null;
        if (kontrol != null)
        {
            kontrol.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        HaritaKontrol kontrol = HexHaritaCizici.Instance != null ? HexHaritaCizici.Instance.Kontrol : null;
        if (kontrol != null)
        {
            kontrol.OnDrag(eventData);
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        HaritaKontrol kontrol = HexHaritaCizici.Instance != null ? HexHaritaCizici.Instance.Kontrol : null;
        if (kontrol != null)
        {
            kontrol.OnScroll(eventData);
        }
    }
}
