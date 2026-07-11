using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class StatTooltip : MonoBehaviour
{
    // Metin icinde <link="stok">...</link> ile isaretli kisma gelince StokMetinFonksiyonu,
    // <link="gelir">...</link> ile isaretli kisma gelince GelirMetinFonksiyonu cagriliyor.
    // Bu objeyi sahiplenen script (ornegin StatsUI) kendi Start'inda bunlari atar:
    // GetComponent<StatTooltip>().StokMetinFonksiyonu = BenimStokMetnim;
    public Func<string> StokMetinFonksiyonu;
    public Func<string> GelirMetinFonksiyonu;

    private TMP_Text metin;
    private Canvas canvas;
    private bool gosteriliyor;

    void Awake()
    {
        metin = GetComponent<TMP_Text>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Camera kamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        Vector2 fareKonumu = Mouse.current.position.ReadValue();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(metin, fareKonumu, kamera);
        string linkId = linkIndex != -1 ? metin.textInfo.linkInfo[linkIndex].GetLinkID() : null;

        Func<string> fonksiyon = null;
        if (linkId == "stok")
        {
            fonksiyon = StokMetinFonksiyonu;
        }
        else if (linkId == "gelir")
        {
            fonksiyon = GelirMetinFonksiyonu;
        }

        if (fonksiyon != null)
        {
            string bilgi = fonksiyon();
            if (!string.IsNullOrEmpty(bilgi))
            {
                TooltipUI.Instance.Goster(bilgi);
                gosteriliyor = true;
                return;
            }
        }

        if (gosteriliyor)
        {
            TooltipUI.Instance.Gizle();
            gosteriliyor = false;
        }
    }
}
