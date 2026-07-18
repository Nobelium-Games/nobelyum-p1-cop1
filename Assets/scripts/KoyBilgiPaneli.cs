using System;
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
    public TMP_Text KaleBonusText;
    public ScrollRect IcerikScrollRect;

    void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
    }

    public void Goster(KoyData koy)
    {
        bool bizeAit = koy.Sahip == KoyYoneticisi.Instance.OyuncuKralligi;

        if (IcerikScrollRect != null)
        {
            IcerikScrollRect.verticalNormalizedPosition = 1f;
        }

        IsimText.text = koy.Isim;

        bool bayrakVar = koy.Sahip != null && koy.Sahip.Bayrak != null;
        BayrakImage.gameObject.SetActive(bayrakVar);
        if (bayrakVar)
        {
            BayrakImage.sprite = koy.Sahip.Bayrak;
        }

        int genelSavunmaBonusu = bizeAit ? KoyYoneticisi.Instance.ToplamGenelSavunmaBonusu() : 0;
        SavunmaText.text = "<link=\"gelir\">Savunma: " + (koy.Savunma + genelSavunmaBonusu) + "</link>";

        StatTooltip savunmaTooltip = SavunmaText.GetComponent<StatTooltip>();
        if (savunmaTooltip != null)
        {
            KoyData koyReferansiSavunma = koy;
            savunmaTooltip.GelirMetinFonksiyonu = bizeAit
                ? (Func<string>)(() => KoyYoneticisi.Instance.SavunmaDagilimKoyBilgisiMetni(koyReferansiSavunma))
                : null;
        }

        GarnizonText.text = "Garnizon: " + koy.Garnizon + "/" + koy.MaxGarnizon;

        bool kale = koy.Tip == YerlesimTipi.Kale;

        DurumText.gameObject.SetActive(bizeAit);
        SadakatText.gameObject.SetActive(bizeAit);
        ErzakText.gameObject.SetActive(bizeAit);
        NufusText.gameObject.SetActive(bizeAit);
        ErzakYieldText.gameObject.SetActive(bizeAit);
        AltinYieldText.gameObject.SetActive(bizeAit);
        SlotText.gameObject.SetActive(bizeAit);
        KaleBonusText.gameObject.SetActive(bizeAit && kale);

        if (bizeAit)
        {
            DurumText.text = koy.IsyanHalinde ? "<color=red>ISYAN HALINDE</color>" : "";
            SadakatText.text = "<link=\"gelir\">Sadakat: " + koy.Sadakat + "</link>";

            StatTooltip sadakatTooltip = SadakatText.GetComponent<StatTooltip>();
            if (sadakatTooltip != null)
            {
                KoyData koyReferansiSadakat = koy;
                sadakatTooltip.GelirMetinFonksiyonu = () => KoyYoneticisi.Instance.SadakatDagilimKoyBilgisiMetni(koyReferansiSadakat);
            }

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

            if (kale)
            {
                KaleBonusText.text = "Savunma Bonusu: +" + koy.GenelSavunmaBonusu;
            }
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
