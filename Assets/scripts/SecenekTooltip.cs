using UnityEngine;
using UnityEngine.EventSystems;

public class SecenekTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DialogueManager Dialog;
    public int SecenekIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        string metin = Dialog.MaliyetMetniAl(SecenekIndex);

        if (!string.IsNullOrEmpty(metin))
        {
            TooltipUI.Instance.Goster(metin);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Gizle();
    }
}
