using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public TMP_Text StatlarText;

    void Update()
    {
        GameState state = GameManager.Instance.State;

        int toplamErzak = KoyYoneticisi.Instance.ToplamErzak();
        int toplamErzakGelir = state.ErzakBaseGelir + KoyYoneticisi.Instance.ToplamErzakYieldi();
        int toplamAltinGelir = state.AltinBaseGelir + KoyYoneticisi.Instance.ToplamAltinYieldi();
        int toplamSadakat = state.Sadakat + KoyYoneticisi.Instance.OrtalamaSadakat();

        string erzakGelir = (toplamErzakGelir >= 0 ? "+" : "") + toplamErzakGelir;
        string altinGelir = (toplamAltinGelir >= 0 ? "+" : "") + toplamAltinGelir;

        StatlarText.text =
            "Erzak: " + toplamErzak + " <sup>" + erzakGelir + "</sup>\n" +
            "Altin: " + state.Altin + " <sup>" + altinGelir + "</sup>\n" +
            "Manpower: " + state.Manpower + "\n" +
            "Sadakat: " + toplamSadakat;
    }
}
