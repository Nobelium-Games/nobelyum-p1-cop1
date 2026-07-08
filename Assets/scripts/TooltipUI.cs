using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject TooltipPanel;
    public TMP_Text TooltipText;

    void Awake()
    {
        Instance = this;
        TooltipPanel.SetActive(false);
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
