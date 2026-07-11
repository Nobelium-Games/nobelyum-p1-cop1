using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject TooltipPanel;
    public TMP_Text TooltipText;
    public Vector2 FareyeGoreOfset = new Vector2(12f, -12f);

    private RectTransform panelRect;
    private RectTransform panelParentRect;
    private Canvas canvas;

    void Awake()
    {
        Instance = this;
        panelRect = TooltipPanel.GetComponent<RectTransform>();
        panelParentRect = panelRect.parent as RectTransform;
        canvas = TooltipPanel.GetComponentInParent<Canvas>();
        TooltipPanel.SetActive(false);
    }

    void Update()
    {
        if (!TooltipPanel.activeSelf)
        {
            return;
        }

        if (Mouse.current == null)
        {
            return;
        }

        Camera kamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        Vector2 fareKonumu = Mouse.current.position.ReadValue();
        Vector2 yerelNokta;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(panelParentRect, fareKonumu, kamera, out yerelNokta))
        {
            // anchoredPosition, panelin kendi anchor noktasina (ornegin sol-ust kose) gore olculuyor,
            // ama yerelNokta parent'in kendi pivot'una (genelde merkez) gore geliyor - ikisini
            // ayni referans noktasina indirgemek icin panelin anchor noktasini parent'in yerel
            // koordinatinda buluyoruz ve farki aliyoruz.
            Rect parentAlani = panelParentRect.rect;
            Vector2 ankorNoktasi = new Vector2(
                Mathf.Lerp(parentAlani.xMin, parentAlani.xMax, panelRect.anchorMin.x),
                Mathf.Lerp(parentAlani.yMin, parentAlani.yMax, panelRect.anchorMin.y)
            );
            panelRect.anchoredPosition = (yerelNokta - ankorNoktasi) + FareyeGoreOfset / canvas.scaleFactor;
        }
    }

    public void Goster(string metin)
    {
        TooltipText.text = metin;
        TooltipPanel.SetActive(true);
    }

    public void Gizle()
    {
        TooltipPanel.SetActive(false);
    }
}
