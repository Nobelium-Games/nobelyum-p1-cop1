using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TooltipMetni;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.Instance.Goster(TooltipMetni);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Gizle();
    }
}
