using System;

[Serializable]
public class GameState
{
    public int Gun = 1;

    public int Erzak = 50;
    public int Sadakat = 50;
    public int Altin = 50;
    public int Manpower = 50;

    public int ErzakBaseGelir = 5;
    public int AltinBaseGelir = 3;

    public void BaseGeliriUygula()
    {
        Erzak += ErzakBaseGelir;
        Altin += AltinBaseGelir;
    }

    public void BaseGeliriArtir(string statAdi, int miktar)
    {
        switch (statAdi)
        {
            case "Erzak":
                ErzakBaseGelir += miktar;
                break;
            case "Altin":
                AltinBaseGelir += miktar;
                break;
            default:
                UnityEngine.Debug.LogWarning("Bu stat'in base geliri yok: " + statAdi);
                break;
        }
    }

    public int StatDegerAl(string statAdi)
    {
        switch (statAdi)
        {
            case "Erzak":
                return Erzak;
            case "Sadakat":
                return Sadakat;
            case "Altin":
                return Altin;
            case "Manpower":
                return Manpower;
            default:
                UnityEngine.Debug.LogWarning("Boyle bir stat yok: " + statAdi);
                return 0;
        }
    }

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
            case "Altin":
                Altin += miktar;
                break;
            case "Manpower":
                Manpower += miktar;
                break;
            default:
                UnityEngine.Debug.LogWarning("Boyle bir stat yok: " + statAdi);
                break;
        }
    }
}

