using System.Collections.Generic;
using UnityEngine;

public class KoyYoneticisi : MonoBehaviour
{
    public static KoyYoneticisi Instance;

    public List<KoyData> Koyler = new List<KoyData>();

    [Header("Krallik Ayarlari")]
    public KrallikData OyuncuKralligi;

    [Header("Nufus Buyume Ayarlari")]
    public float NufusEsik = 1f;
    public float NufusKatsayi = 10f;

    [Header("Savas Ayarlari")]
    public float GarnizonKatsayisi = 10f;

    void Awake()
    {
        Instance = this;

        // Yeni oyuna baslarken her koyun Erzak Yield'i 1-4 arasi rastgele belirlensin
        foreach (KoyData koy in Koyler)
        {
            koy.ErzakYield = Random.Range(1, 5);
            koy.BaseErzakYield = koy.ErzakYield;
        }
    }

    bool BizeAitDegil(KoyData koy)
    {
        return koy.IsyanHalinde || koy.Sahip != OyuncuKralligi;
    }

    public int ToplamErzak()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
            {
                continue;
            }
            toplam += koy.Erzak;
        }
        return toplam;
    }

    public void ErzakDegistir(int miktar)
    {
        List<KoyData> bizimKoylerimiz = Koyler.FindAll(koy => !BizeAitDegil(koy));
        if (bizimKoylerimiz.Count == 0)
        {
            return;
        }

        int koyBasinaDusen = miktar / bizimKoylerimiz.Count;
        int kalan = miktar % bizimKoylerimiz.Count;

        for (int i = 0; i < bizimKoylerimiz.Count; i++)
        {
            int buKoyeUygulanacak = koyBasinaDusen + (i < kalan ? 1 : 0);
            bizimKoylerimiz[i].Erzak = Mathf.Max(0, bizimKoylerimiz[i].Erzak + buKoyeUygulanacak);
        }
    }

    public int ToplamNufus()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
            {
                continue;
            }
            toplam += koy.Nufus;
        }
        return toplam;
    }

    public void NufusDegistir(int miktar)
    {
        List<KoyData> bizimKoylerimiz = Koyler.FindAll(koy => !BizeAitDegil(koy));
        if (bizimKoylerimiz.Count == 0)
        {
            return;
        }

        int koyBasinaDusen = miktar / bizimKoylerimiz.Count;
        int kalan = miktar % bizimKoylerimiz.Count;

        for (int i = 0; i < bizimKoylerimiz.Count; i++)
        {
            int buKoyeUygulanacak = koyBasinaDusen + (i < kalan ? 1 : 0);
            bizimKoylerimiz[i].Nufus = Mathf.Max(0, bizimKoylerimiz[i].Nufus + buKoyeUygulanacak);
        }
    }

    public int NufusYieldHesapla(KoyData koy)
    {
        if (koy.Nufus <= 0)
        {
            return 0;
        }

        float kisiBasiStok = (float)koy.Erzak / koy.Nufus;
        return Mathf.RoundToInt((kisiBasiStok - NufusEsik) * NufusKatsayi);
    }

    public void NufusuGunlukArtir()
    {
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
            {
                continue;
            }
            koy.Nufus = Mathf.Max(0, koy.Nufus + NufusYieldHesapla(koy));
        }
    }

    public int ToplamNufusYieldi()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
            {
                continue;
            }
            toplam += NufusYieldHesapla(koy);
        }
        return toplam;
    }

    public int NetErzakYieldHesapla(KoyData koy)
    {
        return koy.ErzakYield - koy.Garnizon;
    }

    public void ErzagiGunlukArtir()
    {
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
            {
                continue;
            }
            koy.Erzak = Mathf.Max(0, koy.Erzak + NetErzakYieldHesapla(koy));
        }
    }

    public int ToplamErzakYieldi()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
            {
                continue;
            }
            toplam += NetErzakYieldHesapla(koy);
        }
        return toplam;
    }

    public int ToplamAltinYieldi()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
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
            if (koy.Sahip != OyuncuKralligi || koy.IsyanHalinde || koy.Sadakat >= 50)
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

    public float EtkinSavunmaHesapla(KoyData koy)
    {
        return koy.Savunma * (1f + koy.Garnizon / GarnizonKatsayisi);
    }

    public string ErzakDagilimMetni()
    {
        return "Koyler: " + ToplamErzak();
    }

    public string ErzakYieldDagilimMetni()
    {
        return "Koyler: " + IsaretliMetin(ToplamErzakYieldi());
    }

    public string ErzakYieldKoyBilgisiMetni(KoyData koy)
    {
        string metin = "Baz: " + IsaretliMetin(koy.BaseErzakYield);

        int degirmenBonus = koy.ErzakYield - koy.BaseErzakYield;
        if (degirmenBonus != 0)
        {
            metin += "\nDegirmen: " + IsaretliMetin(degirmenBonus);
        }

        if (koy.Garnizon != 0)
        {
            metin += "\nGarnizon: -" + koy.Garnizon;
        }

        return metin;
    }

    public string NufusDagilimMetni()
    {
        return "Koyler: " + ToplamNufus();
    }

    public string NufusYieldDagilimMetni()
    {
        return "Koyler: " + IsaretliMetin(ToplamNufusYieldi());
    }

    public string AltinYieldDagilimMetni()
    {
        return "Koyler: " + IsaretliMetin(ToplamAltinYieldi());
    }

    string IsaretliMetin(int miktar)
    {
        return miktar > 0 ? "+" + miktar : miktar.ToString();
    }

    public int OrtalamaSadakat()
    {
        List<KoyData> bizimKoylerimiz = Koyler.FindAll(koy => koy.Sahip == OyuncuKralligi);
        if (bizimKoylerimiz.Count == 0)
        {
            return 0;
        }

        int toplam = 0;
        foreach (KoyData koy in bizimKoylerimiz)
        {
            toplam += koy.Sadakat;
        }
        return toplam / bizimKoylerimiz.Count;
    }
}
