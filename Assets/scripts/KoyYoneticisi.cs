using System.Collections.Generic;
using UnityEngine;

public class KoyYoneticisi : MonoBehaviour
{
    public static KoyYoneticisi Instance;

    public List<KoyData> Koyler = new List<KoyData>();

    void Awake()
    {
        Instance = this;

        // Yeni oyuna baslarken her koyun Erzak Yield'i 1-4 arasi rastgele belirlensin
        foreach (KoyData koy in Koyler)
        {
            koy.ErzakYield = Random.Range(1, 5);
        }
    }

    public int ToplamErzak()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            toplam += koy.Erzak;
        }
        return toplam;
    }

    public void ErzakDegistir(int miktar)
    {
        if (Koyler.Count == 0)
        {
            return;
        }

        int koyBasinaDusen = miktar / Koyler.Count;
        int kalan = miktar % Koyler.Count;

        for (int i = 0; i < Koyler.Count; i++)
        {
            int buKoyeUygulanacak = koyBasinaDusen + (i < kalan ? 1 : 0);
            Koyler[i].Erzak += buKoyeUygulanacak;
        }
    }

    public void ErzagiGunlukArtir()
    {
        foreach (KoyData koy in Koyler)
        {
            if (koy.IsyanHalinde)
            {
                continue;
            }
            koy.Erzak += koy.ErzakYield;
        }
    }

    public int ToplamErzakYieldi()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (koy.IsyanHalinde)
            {
                continue;
            }
            toplam += koy.ErzakYield;
        }
        return toplam;
    }

    public int ToplamAltinYieldi()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (koy.IsyanHalinde)
            {
                continue;
            }
            toplam += koy.AltinYield;
        }
        return toplam;
    }

    public void IsyanKontrolEt(List<string> mesajListesi)
    {
        foreach (KoyData koy in Koyler)
        {
            if (koy.IsyanHalinde || koy.Sadakat >= 50)
            {
                continue;
            }

            int zar = Random.Range(1, 51);
            if (zar > koy.Sadakat)
            {
                koy.IsyanHalinde = true;
                mesajListesi.Add("<color=red>" + koy.Isim + " isyan etti!</color>");
            }
        }
    }

    public int OrtalamaSadakat()
    {
        if (Koyler.Count == 0)
        {
            return 0;
        }

        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            toplam += koy.Sadakat;
        }
        return toplam / Koyler.Count;
    }
}
