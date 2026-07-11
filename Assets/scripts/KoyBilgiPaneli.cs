using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KoyBilgiPaneli : MonoBehaviour
{
    public static KoyBilgiPaneli Instance;

    public GameObject Panel;
    public TMP_Text IsimText;
    public Image BayrakImage;
    public TMP_Text SadakatText;
    public TMP_Text ErzakText;
    public TMP_Text NufusText;
    public TMP_Text ErzakYieldText;
    public TMP_Text AltinYieldText;
    public TMP_Text SlotText;
    public TMP_Text DurumText;
    public TMP_Text SavunmaText;
    public TMP_Text GarnizonText;

    void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
    }

    public void Goster(KoyData koy)
    {
        bool bizeAit = koy.Sahip == KoyYoneticisi.Instance.OyuncuKralligi;

        IsimText.text = koy.Isim;

        bool bayrakVar = koy.Sahip != null && koy.Sahip.Bayrak != null;
        BayrakImage.gameObject.SetActive(bayrakVar);
        if (bayrakVar)
        {
            BayrakImage.sprite = koy.Sahip.Bayrak;
        }

        SavunmaText.text = "Savunma: " + koy.Savunma;
        GarnizonText.text = "Garnizon: " + koy.Garnizon;

        DurumText.gameObject.SetActive(bizeAit);
        SadakatText.gameObject.SetActive(bizeAit);
        ErzakText.gameObject.SetActive(bizeAit);
        NufusText.gameObject.SetActive(bizeAit);
        ErzakYieldText.gameObject.SetActive(bizeAit);
        AltinYieldText.gameObject.SetActive(bizeAit);
        SlotText.gameObject.SetActive(bizeAit);

        if (bizeAit)
        {
            DurumText.text = koy.IsyanHalinde ? "<color=red>ISYAN HALINDE</color>" : "";
            SadakatText.text = "Sadakat: " + koy.Sadakat;
            ErzakText.text = "Erzak: " + koy.Erzak;
            NufusText.text = "Nufus: " + koy.Nufus + " <sup>" + YieldMetni(KoyYoneticisi.Instance.NufusYieldHesapla(koy), koy.IsyanHalinde) + "</sup>";
            ErzakYieldText.text = "<link=\"gelir\">Erzak Yield: " + YieldMetni(KoyYoneticisi.Instance.NetErzakYieldHesapla(koy), koy.IsyanHalinde) + "</link>";

            StatTooltip erzakYieldTooltip = ErzakYieldText.GetComponent<StatTooltip>();
            if (erzakYieldTooltip != null)
            {
                KoyData koyReferansi = koy;
                erzakYieldTooltip.GelirMetinFonksiyonu = () => KoyYoneticisi.Instance.ErzakYieldKoyBilgisiMetni(koyReferansi);
            }
            AltinYieldText.text = "Altin Yield: " + YieldMetni(koy.AltinYield, koy.IsyanHalinde);
            SlotText.text = "Bina Slotu: " + koy.DoluBinaSlotu + "/" + koy.MaxBinaSlotu;
        }

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
