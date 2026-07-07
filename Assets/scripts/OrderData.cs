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
    public bool SonucSansaBagli;  // true = surenin sonunda zar atilir, false = garanti tamamlanir

    public OrderData(string danisman, string emir, string stat, int basariliMiktar, int basarisizMiktar,
        float sans, int toplamSure, bool sonucSansaBagli)
    {
        DanismanTipi = danisman;
        EmirTuru = emir;
        EtkilenenStat = stat;
        BasariliDegisim = basariliMiktar;
        BasarisizDegisim = basarisizMiktar;
        BasariSansi = sans;
        ToplamSure = toplamSure;
        SonucSansaBagli = sonucSansaBagli;
    }
}