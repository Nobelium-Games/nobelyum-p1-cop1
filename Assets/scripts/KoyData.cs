using System;

[Serializable]
public class KoyData
{
    public string Isim;
    public int Sadakat = 50;
    public int Erzak = 20;
    public int Nufus = 30;
    public int ErzakYield = 1;
    public int AltinYield = 0;
    public int Savunma = 20;

    public int MaxBinaSlotu = 3;
    public int DoluBinaSlotu = 0;

    public bool IsyanHalinde = false;

    public Krallik Sahip = Krallik.Oyuncu;
    public int Garnizon = 0;
}
