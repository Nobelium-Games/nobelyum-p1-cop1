using UnityEngine;

[System.Serializable]
public class HexTileData
{
    public Vector3Int Koordinat;
    public TerrainTipi Terrain;
    public int ErzakDegeri;
    public int AltinDegeri;
    public KoyData SahipKoy;
    public bool DegirmenVar = false;
}
