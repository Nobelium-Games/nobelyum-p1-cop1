using UnityEngine;
using UnityEngine.EventSystems;

public class TileTiklama : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IScrollHandler, IPointerEnterHandler, IPointerExitHandler
{
    public HexTileData Tile;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HexHaritaCizici.Instance.TileHover(Tile, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HexHaritaCizici.Instance.TileHover(Tile, false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Surukleme yapildiysa tiklama sayilmasin (Unity zaten eligibleForClick'i dusurur ama garanti olsun)
        if (eventData.dragging)
        {
            return;
        }
        HexHaritaCizici.Instance.TileTiklandi(Tile);
    }

    // Tile'lar raycast yakaladigi icin harita pan/zoom olaylari HaritaKontrol'e ulasmiyor,
    // bu yuzden burada yakalayip elle iletiyoruz.
    public void OnBeginDrag(PointerEventData eventData)
    {
        HaritaKontrol kontrol = HexHaritaCizici.Instance.Kontrol;
        if (kontrol != null)
        {
            kontrol.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        HaritaKontrol kontrol = HexHaritaCizici.Instance.Kontrol;
        if (kontrol != null)
        {
            kontrol.OnDrag(eventData);
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        HaritaKontrol kontrol = HexHaritaCizici.Instance.Kontrol;
        if (kontrol != null)
        {
            kontrol.OnScroll(eventData);
        }
    }
}
