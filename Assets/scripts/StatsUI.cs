using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public TMP_Text ErzakText;
    public TMP_Text AltinText;
    public TMP_Text ManpowerText;
    public TMP_Text NufusText;
    public TMP_Text SadakatText;

    void Start()
    {
        StatTooltip erzakTooltip = ErzakText.GetComponent<StatTooltip>();
        if (erzakTooltip != null)
        {
            erzakTooltip.MetinFonksiyonu = KoyYoneticisi.Instance.ErzakDagilimMetni;
        }
    }

    void Update()
    {
        GameState state = GameManager.Instance.State;

        int toplamErzak = KoyYoneticisi.Instance.ToplamErzak();
        int toplamErzakGelir = state.ErzakBaseGelir + KoyYoneticisi.Instance.ToplamErzakYieldi();
        int toplamAltinGelir = state.AltinBaseGelir + KoyYoneticisi.Instance.ToplamAltinYieldi();
        int toplamSadakat = state.Sadakat + KoyYoneticisi.Instance.OrtalamaSadakat();
        int toplamNufus = KoyYoneticisi.Instance.ToplamNufus();
        int nufusGelir = KoyYoneticisi.Instance.ToplamNufusYieldi();

        string erzakGelir = GelirMetni(toplamErzakGelir);
        string altinGelir = GelirMetni(toplamAltinGelir);
        string nufusGelirMetni = GelirMetni(nufusGelir);

        ErzakText.text = "Erzak: " + toplamErzak + " <sup>" + erzakGelir + "</sup>";
        AltinText.text = "Altin: " + state.Altin + " <sup>" + altinGelir + "</sup>";
        ManpowerText.text = "Manpower: " + state.Manpower;
        NufusText.text = "Nufus: " + toplamNufus + " <sup>" + nufusGelirMetni + "</sup>";
        SadakatText.text = "Sadakat: " + toplamSadakat;
    }

    string GelirMetni(int miktar)
    {
        string renk = miktar > 0 ? "green" : miktar < 0 ? "red" : "white";
        string isaret = miktar > 0 ? "+" : "";
        return "<color=" + renk + ">" + isaret + miktar + "</color>";
    }
}
