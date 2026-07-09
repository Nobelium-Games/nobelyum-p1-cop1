using UnityEngine;
using TMPro;

public class KoyBilgiPaneli : MonoBehaviour
{
    public static KoyBilgiPaneli Instance;

    public GameObject Panel;
    public TMP_Text IsimText;
    public TMP_Text SadakatText;
    public TMP_Text ErzakText;
    public TMP_Text ErzakYieldText;
    public TMP_Text AltinYieldText;
    public TMP_Text SlotText;

    void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
    }

    public void Goster(KoyData koy)
    {
        IsimText.text = koy.Isim;
        SadakatText.text = "Sadakat: " + koy.Sadakat;
        ErzakText.text = "Erzak: " + koy.Erzak;
        ErzakYieldText.text = "Erzak Yield: " + YieldMetni(koy.ErzakYield);
        AltinYieldText.text = "Altin Yield: " + YieldMetni(koy.AltinYield);
        SlotText.text = "Bina Slotu: " + koy.DoluBinaSlotu + "/" + koy.MaxBinaSlotu;

        Panel.SetActive(true);
    }

    string YieldMetni(int miktar)
    {
        string renk = miktar > 0 ? "green" : miktar < 0 ? "red" : "white";
        string isaret = miktar > 0 ? "+" : "";
        return "<color=" + renk + ">" + isaret + miktar + "</color>";
    }

    public void Kapat()
    {
        Panel.SetActive(false);
    }
}
