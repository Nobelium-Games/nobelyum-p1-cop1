using System;
using UnityEngine;

[Serializable]
public class KoyData
{
    public string Isim;
    public int Sadakat = 50;
    public int Erzak = 20;
    public int Nufus = 30;
    public int ErzakYield = 1;
    public int BaseErzakYield = 1;
    public int AltinYield = 0;
    public int Savunma = 20;

    public Vector3Int MerkezTileKoordinati;
    public int TileMenzili = 1;

    public int MaxBinaSlotu = 3;
    public int DoluBinaSlotu = 0;

    public bool IsyanHalinde = false;

    public KrallikData Sahip;
    public int Garnizon = 0;
    public int MaxGarnizon = 50;

    public YerlesimTipi Tip = YerlesimTipi.Koy;

    // Sadece Tip == Kale icin anlamli: bu kalenin butun krallige sagladigi genel savunma bonusu.
    public int GenelSavunmaBonusu = 2;
}
