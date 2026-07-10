using UnityEngine;
using TMPro;

public class KoyBilgiPaneli : MonoBehaviour
{
    public static KoyBilgiPaneli Instance;

    public GameObject Panel;
    public TMP_Text IsimText;
    public TMP_Text SadakatText;
    public TMP_Text ErzakText;
    public TMP_Text NufusText;
    public TMP_Text ErzakYieldText;
    public TMP_Text AltinYieldText;
    public TMP_Text SlotText;
    public TMP_Text DurumText;
    public TMP_Text SavunmaText;

    void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
    }

    public void Goster(KoyData koy)
    {
        IsimText.text = koy.Isim;
        DurumText.text = koy.IsyanHalinde ? "<color=red>ISYAN HALINDE</color>" : "";
        SadakatText.text = "Sadakat: " + koy.Sadakat;
        ErzakText.text = "Erzak: " + koy.Erzak;
        NufusText.text = "Nufus: " + koy.Nufus + " <sup>" + YieldMetni(KoyYoneticisi.Instance.NufusYieldHesapla(koy), koy.IsyanHalinde) + "</sup>";
        ErzakYieldText.text = "Erzak Yield: " + YieldMetni(koy.ErzakYield, koy.IsyanHalinde);
        AltinYieldText.text = "Altin Yield: " + YieldMetni(koy.AltinYield, koy.IsyanHalinde);
        SlotText.text = "Bina Slotu: " + koy.DoluBinaSlotu + "/" + koy.MaxBinaSlotu;
        SavunmaText.text = "Savunma: " + koy.Savunma;

        Panel.SetActive(true);
    }

    string YieldMetni(int miktar, bool isyanHalinde)
    {
        if (isyanHalinde)
        {
            string isaretGri = miktar > 0 ? "+" : "";
            return "<color=grey>" + isaretGri + miktar + "</color>";
        }

        string renk = miktar > 0 ? "green" : miktar < 0 ? "red" : "white";
        string isaret = miktar > 0 ? "+" : "";
        return "<color=" + renk + ">" + isaret + miktar + "</color>";
    }

    public void Kapat()
    {
        Panel.SetActive(false);
    }
}
