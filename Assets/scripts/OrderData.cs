using System;

[Serializable]
public class OrderData
{
    public string DanismanTipi;   // orn: "Insaatci", "Askerbasi"
    public string EmirTuru;       // orn: "Degirmen Insa Et"
    public string EtkilenenStat;  // basari olursa hangi stat degisecek
    public int BasariliDegisim;   // basarili olursa ne kadar degisecek
    public int BasarisizDegisim;  // basarisiz olursa ne kadar degisecek
    public float BasariSansi;     // 0 ile 1 arasi (orn 0.7 = %70 basari sansi)

    public OrderData(string danisman, string emir, string stat, int basariliMiktar, int basarisizMiktar, float sans)
    {
        DanismanTipi = danisman;
        EmirTuru = emir;
        EtkilenenStat = stat;
        BasariliDegisim = basariliMiktar;
        BasarisizDegisim = basarisizMiktar;
        BasariSansi = sans;
    }
}
