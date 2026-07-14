using UnityEngine;

public class HaritaEkraniKontrol : MonoBehaviour
{
    public StatTooltip[] StatTooltipleri;

    void OnEnable()
    {
        foreach (StatTooltip tooltip in StatTooltipleri)
        {
            if (tooltip != null)
            {
                tooltip.enabled = false;
            }
        }

        if (TooltipUI.Instance != null)
        {
            TooltipUI.Instance.Gizle();
        }
    }

    void OnDisable()
    {
        foreach (StatTooltip tooltip in StatTooltipleri)
        {
            if (tooltip != null)
            {
                tooltip.enabled = true;
            }
        }
    }
}
