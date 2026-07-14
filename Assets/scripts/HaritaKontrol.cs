using UnityEngine;
using UnityEngine.EventSystems;

public class HaritaKontrol : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
{
    public RectTransform Icerik;
    public float ZoomHizi = 0.1f;
    public float MaxZoom = 3f;

    private Canvas anaCanvas;
    private RectTransform ustRect;
    private Vector2 sonNokta;
    private float minZoom;

    void Awake()
    {
        anaCanvas = GetComponentInParent<Canvas>().rootCanvas;
        ustRect = Icerik.parent as RectTransform;
    }

    void Start()
    {
        YenidenHesaplaVeSinirla();
    }

    public void YenidenHesaplaVeSinirla()
    {
        float genislikOrani = ustRect.rect.width / Icerik.rect.width;
        float yukseklikOrani = ustRect.rect.height / Icerik.rect.height;
        // Mathf.Min: haritanin TAMAMI ekrana sigsin (contain) - Mathf.Max kullanilsaydi ekran
        // tamamen kaplanirdi ama uzun kenarda harita tasip goze gorunmezdi (cover davranisi).
        minZoom = Mathf.Min(genislikOrani, yukseklikOrani);

        float olcek = Mathf.Max(Icerik.localScale.x, minZoom);
        olcek = Mathf.Clamp(olcek, minZoom, MaxZoom);
        Icerik.localScale = new Vector3(olcek, olcek, 1f);
        SinirlaKonum();
    }

    Camera KullanilacakKamera()
    {
        return anaCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : anaCanvas.worldCamera;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ustRect, eventData.position, KullanilacakKamera(), out sonNokta);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 yeniNokta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ustRect, eventData.position, KullanilacakKamera(), out yeniNokta);

        Icerik.anchoredPosition += yeniNokta - sonNokta;
        sonNokta = yeniNokta;

        SinirlaKonum();
    }

    public void OnScroll(PointerEventData eventData)
    {
        float yeniOlcek = Icerik.localScale.x + eventData.scrollDelta.y * ZoomHizi;
        yeniOlcek = Mathf.Clamp(yeniOlcek, minZoom, MaxZoom);
        Icerik.localScale = new Vector3(yeniOlcek, yeniOlcek, 1f);

        SinirlaKonum();
    }

    void SinirlaKonum()
    {
        float olcek = Icerik.localScale.x;
        float genislikFarki = Mathf.Max(0f, (Icerik.rect.width * olcek - ustRect.rect.width) / 2f);
        float yukseklikFarki = Mathf.Max(0f, (Icerik.rect.height * olcek - ustRect.rect.height) / 2f);

        Vector2 pozisyon = Icerik.anchoredPosition;
        pozisyon.x = Mathf.Clamp(pozisyon.x, -genislikFarki, genislikFarki);
        pozisyon.y = Mathf.Clamp(pozisyon.y, -yukseklikFarki, yukseklikFarki);
        Icerik.anchoredPosition = pozisyon;
    }
}
