using UnityEngine;

public struct TerrainBilgisi
{
    public Color Renk;
    public int ErzakMin;
    public int ErzakMax;
    public int AltinMin;
    public int AltinMax;
    public float SavunmaCarpani;
}

public static class TerrainVerisi
{
    public static TerrainBilgisi Bilgi(TerrainTipi tip)
    {
        switch (tip)
        {
            case TerrainTipi.Ova:
                return new TerrainBilgisi { Renk = new Color(0.55f, 0.8f, 0.3f), ErzakMin = 3, ErzakMax = 6, AltinMin = 0, AltinMax = 2, SavunmaCarpani = 0.9f };
            case TerrainTipi.Orman:
                return new TerrainBilgisi { Renk = new Color(0.15f, 0.45f, 0.15f), ErzakMin = 2, ErzakMax = 4, AltinMin = 0, AltinMax = 1, SavunmaCarpani = 1.15f };
            case TerrainTipi.Dag:
                return new TerrainBilgisi { Renk = new Color(0.5f, 0.5f, 0.5f), ErzakMin = 0, ErzakMax = 1, AltinMin = 2, AltinMax = 4, SavunmaCarpani = 1.4f };
            case TerrainTipi.Col:
                return new TerrainBilgisi { Renk = new Color(0.85f, 0.75f, 0.35f), ErzakMin = 0, ErzakMax = 2, AltinMin = 0, AltinMax = 1, SavunmaCarpani = 0.85f };
            default:
                return new TerrainBilgisi { Renk = Color.gray, ErzakMin = 0, ErzakMax = 0, AltinMin = 0, AltinMax = 0, SavunmaCarpani = 1f };
        }
    }

    public static TerrainTipi RastgeleTip()
    {
        System.Array degerler = System.Enum.GetValues(typeof(TerrainTipi));
        return (TerrainTipi)degerler.GetValue(Random.Range(0, degerler.Length));
    }
}
