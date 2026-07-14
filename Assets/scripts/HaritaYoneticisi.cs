using System.Collections.Generic;
using UnityEngine;

public class HaritaYoneticisi : MonoBehaviour
{
    public static HaritaYoneticisi Instance;

    public List<HexTileData> Tileler = new List<HexTileData>();
    public Vector3Int HaritaMerkezi;

    [Header("Tile Deger Araligi")]
    public int MinErzakDegeri = 1;
    public int MaxErzakDegeri = 4;
    public int MinAltinDegeri = 0;
    public int MaxAltinDegeri = 2;

    [Header("Harita Siniri")]
    public int SinirPayi = 5;

    [Header("Ordu Hareketi")]
    public float OrduHizi = 3f;

    void Awake()
    {
        Instance = this;
    }

    public void TileleriOlustur(List<KoyData> koyler)
    {
        Tileler.Clear();

        if (koyler.Count == 0)
        {
            return;
        }

        HaritaMerkezi = OrtalamaKonumuHesapla(koyler);

        int yaricap = SinirPayi;
        foreach (KoyData koy in koyler)
        {
            yaricap = Mathf.Max(yaricap, HexMesafe(koy.MerkezTileKoordinati, HaritaMerkezi) + SinirPayi);
        }

        for (int q = -yaricap; q <= yaricap; q++)
        {
            int rMin = Mathf.Max(-yaricap, -q - yaricap);
            int rMax = Mathf.Min(yaricap, -q + yaricap);

            for (int r = rMin; r <= rMax; r++)
            {
                Vector3Int koordinat = HaritaMerkezi + new Vector3Int(q, r, 0);

                Tileler.Add(new HexTileData
                {
                    Koordinat = koordinat,
                    ErzakDegeri = Random.Range(MinErzakDegeri, MaxErzakDegeri + 1),
                    AltinDegeri = Random.Range(MinAltinDegeri, MaxAltinDegeri + 1),
                    SahipKoy = EnYakinKoyuBul(koordinat, koyler)
                });
            }
        }
    }

    Vector3Int OrtalamaKonumuHesapla(List<KoyData> koyler)
    {
        int toplamX = 0;
        int toplamY = 0;
        foreach (KoyData koy in koyler)
        {
            toplamX += koy.MerkezTileKoordinati.x;
            toplamY += koy.MerkezTileKoordinati.y;
        }
        return new Vector3Int(
            Mathf.RoundToInt((float)toplamX / koyler.Count),
            Mathf.RoundToInt((float)toplamY / koyler.Count),
            0);
    }

    KoyData EnYakinKoyuBul(Vector3Int koordinat, List<KoyData> koyler)
    {
        KoyData enYakinKoy = null;
        int enKucukEtkinMesafe = int.MaxValue;

        foreach (KoyData koy in koyler)
        {
            int etkinMesafe = HexMesafe(koordinat, koy.MerkezTileKoordinati) - koy.TileMenzili;
            if (etkinMesafe < enKucukEtkinMesafe)
            {
                enKucukEtkinMesafe = etkinMesafe;
                enYakinKoy = koy;
            }
        }

        return enYakinKoy;
    }

    public int KoyunErzakToplami(KoyData koy)
    {
        int toplam = 0;
        foreach (HexTileData tile in Tileler)
        {
            if (tile.SahipKoy == koy)
            {
                toplam += tile.ErzakDegeri;
            }
        }
        return toplam;
    }

    public int KoyunAltinToplami(KoyData koy)
    {
        int toplam = 0;
        foreach (HexTileData tile in Tileler)
        {
            if (tile.SahipKoy == koy)
            {
                toplam += tile.AltinDegeri;
            }
        }
        return toplam;
    }

    public int HexMesafe(Vector3Int a, Vector3Int b)
    {
        int ax = a.x;
        int az = a.y;
        int ay = -ax - az;

        int bx = b.x;
        int bz = b.y;
        int by = -bx - bz;

        return (Mathf.Abs(ax - bx) + Mathf.Abs(ay - by) + Mathf.Abs(az - bz)) / 2;
    }

    public int KoyMesafesi(KoyData koyA, KoyData koyB)
    {
        return HexMesafe(koyA.MerkezTileKoordinati, koyB.MerkezTileKoordinati);
    }

    public int SureHesapla(KoyData kaynakKoy, KoyData hedefKoy)
    {
        if (kaynakKoy == null)
        {
            return 1;
        }
        int mesafe = KoyMesafesi(kaynakKoy, hedefKoy);
        return Mathf.Max(1, Mathf.RoundToInt(mesafe / OrduHizi));
    }

    public Color TileRengi(HexTileData tile)
    {
        if (tile.SahipKoy == null || tile.SahipKoy.Sahip == null)
        {
            return Color.gray;
        }
        return tile.SahipKoy.Sahip.HaritaRengi;
    }
}
