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
        int toplamNufus = KoyYoneticisi.Instance.ToplamNufus();
        int nufusGelir = KoyYoneticisi.Instance.ToplamNufusYieldi();

        string erzakGelir = GelirMetni(toplamErzakGelir);
        string altinGelir = GelirMetni(toplamAltinGelir);
        string nufusGelirMetni = GelirMetni(nufusGelir);

        StatlarText.text =
            "Erzak: " + toplamErzak + " <sup>" + erzakGelir + "</sup>\n" +
            "Altin: " + state.Altin + " <sup>" + altinGelir + "</sup>\n" +
            "Manpower: " + state.Manpower + "\n" +
            "Nufus: " + toplamNufus + " <sup>" + nufusGelirMetni + "</sup>\n" +
            "Sadakat: " + toplamSadakat;
    }

    string GelirMetni(int miktar)
    {
        string renk = miktar > 0 ? "green" : miktar < 0 ? "red" : "white";
        string isaret = miktar > 0 ? "+" : "";
        return "<color=" + renk + ">" + isaret + miktar + "</color>";
    }
}
