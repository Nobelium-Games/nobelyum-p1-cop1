using System;

[Serializable]
public class OrderData
{
    public string DanismanTipi;
    public string EmirTuru;
    public string EtkilenenStat;
    public int BasariliDegisim;
    public int BasarisizDegisim;
    public float BasariSansi;
    public int ToplamSure;
    public bool SonucSansaBagli;
    public string MaliyetStat;
    public int MaliyetMiktar;
    public bool BaseGeliriEtkiler;
    public string BaseGeliriStat;
    public int BaseGeliriMiktar;

    public OrderData(string danisman, string emir, string stat, int basariliMiktar, int basarisizMiktar,
        float sans, int toplamSure, bool sonucSansaBagli, string maliyetStat = "", int maliyetMiktar = 0,
        bool baseGeliriEtkiler = false, string baseGeliriStat = "", int baseGeliriMiktar = 0)
    {
        DanismanTipi = danisman;
        EmirTuru = emir;
        EtkilenenStat = stat;
        BasariliDegisim = basariliMiktar;
        BasarisizDegisim = basarisizMiktar;
        BasariSansi = sans;
        ToplamSure = toplamSure;
        SonucSansaBagli = sonucSansaBagli;
        MaliyetStat = maliyetStat;
        MaliyetMiktar = maliyetMiktar;
        BaseGeliriEtkiler = baseGeliriEtkiler;
        BaseGeliriStat = baseGeliriStat;
        BaseGeliriMiktar = baseGeliriMiktar;
    }
}