using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Bu objeyi olusturan/sahiplenen script (ornegin StatsUI, KoyEtiketiTiklama vs.)
    // kendi Awake/Start'inda bu fonksiyonu atar: GetComponent<StatTooltip>().MetinFonksiyonu = BenimMetnim;
    // Boylece her yeni hover icin bu dosyaya yeni bir case eklemeye gerek kalmiyor.
    public Func<string> MetinFonksiyonu;

    public void OnPointerEnter(PointerEventData eventData)
    {
        string metin = MetinFonksiyonu != null ? MetinFonksiyonu() : "";

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
