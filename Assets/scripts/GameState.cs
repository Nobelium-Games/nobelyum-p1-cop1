using System;

[Serializable]
public class GameState
{
    public int Gun = 1;

    public int Sadakat = 50;
    public int Altin = 50;
    public int Manpower = 50;

    public int ErzakBaseGelir = 5;
    public int AltinBaseGelir = 3;

    public float ManpowerMaasiBirimMaliyeti = 0.05f;
    public float BinaBakimBirimMaliyeti = 1f;

    public void BaseGeliriUygula()
    {
        KoyYoneticisi.Instance.ErzakDegistir(ErzakBaseGelir);
        KoyYoneticisi.Instance.NufusuGunlukArtir();
        Altin += AltinBaseGelir;
    }

    public void GiderleriUygula()
    {
        Altin -= UnityEngine.Mathf.RoundToInt(Manpower * ManpowerMaasiBirimMaliyeti);
        Altin -= UnityEngine.Mathf.RoundToInt(KoyYoneticisi.Instance.ToplamDoluBinaSlotu() * BinaBakimBirimMaliyeti);
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
                return KoyYoneticisi.Instance.ToplamErzak();
            case "Nufus":
                return KoyYoneticisi.Instance.ToplamNufus();
            case "Sadakat":
                return Sadakat + KoyYoneticisi.Instance.OrtalamaSadakat();
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
                KoyYoneticisi.Instance.ErzakDegistir(miktar);
                break;
            case "Nufus":
                KoyYoneticisi.Instance.NufusDegistir(miktar);
                break;
            case "Sadakat":
                Sadakat = UnityEngine.Mathf.Clamp(Sadakat + miktar, 0, 100);
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

