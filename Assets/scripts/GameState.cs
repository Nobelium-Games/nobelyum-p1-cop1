using System;

[Serializable]
public class GameState
{
    public int Erzak = 50;
    public int Sadakat = 50;
    public int OrduGucu = 50;
    //selamlar deneme
    public void StatDegistir(string statAdi, int miktar)
    {
        switch (statAdi)
        {
            case "Erzak":
                Erzak += miktar;
                break;
            case "Sadakat":
                Sadakat += miktar;
                break;
            case "OrduGucu":
                OrduGucu += miktar;
                break;
            default:
                UnityEngine.Debug.LogWarning("Boyle bir stat yok: " + statAdi);
                break;
        }
    }
}

