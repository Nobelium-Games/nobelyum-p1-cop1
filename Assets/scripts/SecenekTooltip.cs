using UnityEngine;
using UnityEngine.EventSystems;

public class SecenekTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DialogueManager Dialog;
    [HideInInspector] public DialogueChoice Secenek;

    public void OnPointerEnter(PointerEventData eventData)
    {
        string metin = Dialog.MaliyetMetniAl(Secenek);

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
