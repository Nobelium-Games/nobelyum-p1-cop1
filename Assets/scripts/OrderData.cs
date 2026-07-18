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

    public bool KoySecimiGerekli;
    public KoyData HedefKoy;
    public bool BinaSlotuKullanir;
    public bool IsyanliKoyGerekli;
    public bool ManpowerMiktariSorulsun;
    public bool IsyanBastirir;
    public int GonderilenManpower;
    public bool DusmanKoyuGerekli;
    public bool SaldiriBaslatir;
    public bool KaynakSecimiGerekli;
    public KoyData KaynakKoy;
    public bool GarnizonEkler;

    public bool TileSecimiGerekli;
    public HexTileData HedefTile;

    public KrallikData HedefKrallik;
    public bool DiplomasiyiArttirir;
    public int DiplomasiMiktari;
    public bool BarisTeklifEder;

    public OrderData() { }

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

    public OrderData KopyalaVeKoyAta(KoyData koy)
    {
        OrderData kopya = new OrderData(DanismanTipi, EmirTuru, EtkilenenStat, BasariliDegisim, BasarisizDegisim,
            BasariSansi, ToplamSure, SonucSansaBagli, MaliyetStat, MaliyetMiktar,
            BaseGeliriEtkiler, BaseGeliriStat, BaseGeliriMiktar);
        kopya.KoySecimiGerekli = KoySecimiGerekli;
        kopya.HedefKoy = koy;
        kopya.BinaSlotuKullanir = BinaSlotuKullanir;
        kopya.IsyanliKoyGerekli = IsyanliKoyGerekli;
        kopya.ManpowerMiktariSorulsun = ManpowerMiktariSorulsun;
        kopya.IsyanBastirir = IsyanBastirir;
        kopya.GonderilenManpower = GonderilenManpower;
        kopya.DusmanKoyuGerekli = DusmanKoyuGerekli;
        kopya.SaldiriBaslatir = SaldiriBaslatir;
        kopya.KaynakSecimiGerekli = KaynakSecimiGerekli;
        kopya.GarnizonEkler = GarnizonEkler;
        kopya.TileSecimiGerekli = TileSecimiGerekli;
        return kopya;
    }
}