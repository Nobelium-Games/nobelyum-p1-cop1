using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public TMP_Text StatlarText;

    void Update()
    {
        GameState state = GameManager.Instance.State;

        string erzakGelir = (state.ErzakBaseGelir >= 0 ? "+" : "") + state.ErzakBaseGelir;
        string altinGelir = (state.AltinBaseGelir >= 0 ? "+" : "") + state.AltinBaseGelir;

        StatlarText.text =
            "Erzak: " + state.Erzak + " <sup>" + erzakGelir + "</sup>\n" +
            "Altin: " + state.Altin + " <sup>" + altinGelir + "</sup>\n" +
            "Manpower: " + state.Manpower + "\n" +
            "Sadakat: " + state.Sadakat;
    }
}
