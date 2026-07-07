using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public List<OrderData> BekleyenEmirler = new List<OrderData>();
    public HashSet<string> KullanilanDanismanlar = new HashSet<string>();

    public bool DanismanKullanildiMi(string danismanTipi)
    {
        return KullanilanDanismanlar.Contains(danismanTipi);
    }

    public void EmirEkle(OrderData emir)
    {
        if (DanismanKullanildiMi(emir.DanismanTipi))
        {
            Debug.Log(emir.DanismanTipi + " bu dongude zaten kullanildi!");
            return;
        }

        BekleyenEmirler.Add(emir);
        KullanilanDanismanlar.Add(emir.DanismanTipi);

        Debug.Log(emir.DanismanTipi + " icin emir eklendi: " + emir.EmirTuru);
    }

    public void YeniDongueBasla()
    {
        BekleyenEmirler.Clear();
        KullanilanDanismanlar.Clear();
    }
}