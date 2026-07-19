using System.Collections.Generic;
using UnityEngine;

public class KoyYoneticisi : MonoBehaviour
{
    public static KoyYoneticisi Instance;

    public List<KoyData> Koyler = new List<KoyData>();

    [Header("Krallik Ayarlari")]
    public KrallikData OyuncuKralligi;

    // Her krallligin baskenti kendi KrallikData asset'inde (BaskentIsmi, string) tutuluyor -
    // KoyData bir ScriptableObject olmadigi icin Inspector'dan dogrudan referans verilemiyor
    // (verilirse Unity bos, yeni bir KoyData ornegi olusturuyor, Koyler listesindeki gercek
    // koyle hicbir baglantisi olmuyor). Bu yuzden isim uzerinden arama yapiyoruz.
    public KoyData BaskentiBul(KrallikData krallik)
    {
        if (krallik == null || string.IsNullOrEmpty(krallik.BaskentIsmi))
        {
            return null;
        }
        return Koyler.Find(koy => string.Equals(koy.Isim.Trim(), krallik.BaskentIsmi.Trim(), System.StringComparison.OrdinalIgnoreCase));
    }

    // Bizim baskentimiz - eskiden ayri bir "Baskent" alani olarak tutulan yer, artik
    // OyuncuKralligi'nin BaskentIsmi'nden hesaplaniyor (bkz. yukaridaki not).
    public KoyData Baskent => BaskentiBul(OyuncuKralligi);

    [Header("Nufus Buyume Ayarlari")]
    public float NufusEsik = 1f;
    public float NufusKatsayi = 10f;

    public float SinirBaskisiMenzili = 5f;
    public float SinirBaskisiMax = 2f;

    [Header("Savas Ayarlari")]
    public float GarnizonKatsayisi = 10f;
    public float DusmanSaldiriIhtimali = 0.2f;

    [Header("Diplomasi Ayarlari")]
    public List<DiplomasiVerisi> Diplomasiler = new List<DiplomasiVerisi>();
    public int DiplomasiEsikDegeri = 40;

    [Header("Harita Baglantisi")]
    public HaritaYoneticisi Harita;

    void Awake()
    {
        Instance = this;

        if (Harita != null)
        {
            Harita.TileleriOlustur(Koyler);
        }

        // Yeni oyuna baslarken her koyun Erzak/Altin Yield'i belirlensin.
        // Harita baglanmadiysa eski davranis (1-4 arasi rastgele) korunuyor.
        foreach (KoyData koy in Koyler)
        {
            if (Harita != null)
            {
                koy.ErzakYield = Harita.KoyunErzakToplami(koy);
                koy.AltinYield = Harita.KoyunAltinToplami(koy);
            }
            else
            {
                koy.ErzakYield = Random.Range(1, 5);
            }
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

    public int ToplamDoluBinaSlotu()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (BizeAitDegil(koy))
            {
                continue;
            }
            toplam += koy.DoluBinaSlotu;
        }
        return toplam;
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

    public int ToplamGenelSavunmaBonusu()
    {
        int toplam = 0;
        foreach (KoyData koy in Koyler)
        {
            if (koy.Tip != YerlesimTipi.Kale || BizeAitDegil(koy))
            {
                continue;
            }
            toplam += koy.GenelSavunmaBonusu;
        }
        return toplam;
    }

    public float EtkinSavunmaHesapla(KoyData koy)
    {
        int genelBonus = koy.Sahip == OyuncuKralligi ? ToplamGenelSavunmaBonusu() : 0;
        return (koy.Savunma + genelBonus) * (1f + koy.Garnizon / GarnizonKatsayisi);
    }

    public string SavunmaDagilimKoyBilgisiMetni(KoyData koy)
    {
        int genelBonus = ToplamGenelSavunmaBonusu();
        string metin = "Baz: " + koy.Savunma;
        if (genelBonus > 0)
        {
            metin += "\nKale Bonusu: +" + genelBonus;
        }
        return metin;
    }

    public List<KoyData> GarnizonluKoyler()
    {
        return Koyler.FindAll(koy => koy.Sahip == OyuncuKralligi && koy.Garnizon > 0);
    }

    DiplomasiVerisi DiplomasiVerisiBul(KrallikData krallik)
    {
        return Diplomasiler.Find(veri => veri.Krallik == krallik);
    }

    public int DiplomasiDegerAl(KrallikData krallik)
    {
        DiplomasiVerisi veri = DiplomasiVerisiBul(krallik);
        return veri != null ? veri.Diplomasi : 0;
    }

    public bool SavastaMi(KrallikData krallik)
    {
        DiplomasiVerisi veri = DiplomasiVerisiBul(krallik);
        return veri != null && veri.SavastaMi;
    }

    public void DiplomasiDegistir(KrallikData krallik, int miktar)
    {
        DiplomasiVerisi veri = DiplomasiVerisiBul(krallik);
        if (veri != null)
        {
            veri.Diplomasi = Mathf.Clamp(veri.Diplomasi + miktar, 0, 100);
        }
    }

    public void BarisYap(KrallikData krallik)
    {
        DiplomasiVerisi veri = DiplomasiVerisiBul(krallik);
        if (veri != null)
        {
            veri.SavastaMi = false;
        }
    }

    public void DiplomasiKontrolEt(List<string> mesajListesi)
    {
        foreach (DiplomasiVerisi veri in Diplomasiler)
        {
            if (veri.SavastaMi || veri.Diplomasi >= DiplomasiEsikDegeri)
            {
                continue;
            }

            int zar = Random.Range(1, 51);
            if (zar > veri.Diplomasi)
            {
                veri.SavastaMi = true;
                mesajListesi.Add("<color=red>" + veri.Krallik.Isim + " bize savas actı!</color>");
            }
        }
    }

    public void DusmanSaldirilariniKontrolEt(List<string> mesajListesi)
    {
        if (Harita == null)
        {
            return;
        }

        List<KoyData> bizimKoyler = Koyler.FindAll(koy => koy.Sahip == OyuncuKralligi);
        if (bizimKoyler.Count == 0)
        {
            return;
        }

        foreach (KoyData dusmanKoy in Koyler)
        {
            if (dusmanKoy.Sahip == OyuncuKralligi || dusmanKoy.Garnizon <= 0)
            {
                continue;
            }

            if (!SavastaMi(dusmanKoy.Sahip))
            {
                continue;
            }

            if (Random.Range(0f, 1f) > DusmanSaldiriIhtimali)
            {
                continue;
            }

            KoyData hedefKoy = EnYakinBizimKoy(dusmanKoy, bizimKoyler);
            if (hedefKoy == null)
            {
                continue;
            }

            bool basarili;
            if (hedefKoy.Garnizon <= 0)
            {
                // Hedef koyde hic garnizon yoksa savunmasiz sayilir, zar atilmadan direkt ele geciriliyor.
                basarili = true;
            }
            else
            {
                float etkinSavunma = EtkinSavunmaHesapla(hedefKoy);
                float saldiriSansi = dusmanKoy.Garnizon / (dusmanKoy.Garnizon + etkinSavunma);
                basarili = Random.Range(0f, 1f) < saldiriSansi;
            }

            if (basarili)
            {
                int hayattaKalan = Mathf.RoundToInt(dusmanKoy.Garnizon * (1f - 0.15f));
                dusmanKoy.Garnizon = 0;
                hedefKoy.Sahip = dusmanKoy.Sahip;
                hedefKoy.Garnizon = Mathf.Min(hayattaKalan, hedefKoy.MaxGarnizon);

                mesajListesi.Add("<color=red>" + dusmanKoy.Sahip.Isim + " ordusu " + hedefKoy.Isim + " koyumuzu ele gecirdi!</color>");

                if (HexHaritaCizici.Instance != null)
                {
                    HexHaritaCizici.Instance.RenkleriGuncelle();
                }
            }
            else
            {
                int hayattaKalan = Mathf.RoundToInt(dusmanKoy.Garnizon * (1f - 0.80f));
                dusmanKoy.Garnizon = hayattaKalan;

                mesajListesi.Add("<color=green>" + hedefKoy.Isim + " savunmasi " + dusmanKoy.Sahip.Isim + " saldirisini puskurttu!</color>");
            }
        }
    }

    KoyData EnYakinBizimKoy(KoyData dusmanKoy, List<KoyData> bizimKoyler)
    {
        KoyData enYakin = null;
        int enKucukMesafe = int.MaxValue;
        foreach (KoyData koy in bizimKoyler)
        {
            int mesafe = Harita.KoyMesafesi(dusmanKoy, koy);
            if (mesafe < enKucukMesafe)
            {
                enKucukMesafe = mesafe;
                enYakin = koy;
            }
        }
        return enYakin;
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
        GameState state = GameManager.Instance.State;
        int askerMaasi = Mathf.RoundToInt(state.Manpower * state.ManpowerMaasiBirimMaliyeti);
        int binaBakimGideri = Mathf.RoundToInt(ToplamDoluBinaSlotu() * state.BinaBakimBirimMaliyeti);

        return "Koyler: " + IsaretliMetin(ToplamAltinYieldi())
            + "\nAsker Maasi: -" + askerMaasi
            + "\nBina Bakimi: -" + binaBakimGideri;
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

    public void SadakatDegistir(int miktar)
    {
        List<KoyData> bizimKoylerimiz = Koyler.FindAll(koy => koy.Sahip == OyuncuKralligi);
        if (bizimKoylerimiz.Count == 0)
        {
            return;
        }

        int koyBasinaDusen = miktar / bizimKoylerimiz.Count;
        int kalan = miktar % bizimKoylerimiz.Count;

        for (int i = 0; i < bizimKoylerimiz.Count; i++)
        {
            int buKoyeUygulanacak = koyBasinaDusen + (i < kalan ? 1 : 0);
            bizimKoylerimiz[i].Sadakat = Mathf.Clamp(bizimKoylerimiz[i].Sadakat + buKoyeUygulanacak, 0, 100);
        }
    }

    int SinirBaskisiHesapla(KoyData koy)
    {
        if (Harita == null)
        {
            return 0;
        }

        int enKucukMesafe = int.MaxValue;
        foreach (KoyData digerKoy in Koyler)
        {
            if (digerKoy.Sahip == OyuncuKralligi || digerKoy.Sahip == null || !SavastaMi(digerKoy.Sahip))
            {
                continue;
            }

            int mesafe = Harita.KoyMesafesi(koy, digerKoy);
            if (mesafe < enKucukMesafe)
            {
                enKucukMesafe = mesafe;
            }
        }

        if (enKucukMesafe == int.MaxValue || enKucukMesafe > SinirBaskisiMenzili)
        {
            return 0;
        }

        float oran = 1f - (enKucukMesafe / SinirBaskisiMenzili);
        return -Mathf.RoundToInt(SinirBaskisiMax * oran);
    }

    public string SadakatDagilimKoyBilgisiMetni(KoyData koy)
    {
        int sinirBaskisi = SinirBaskisiHesapla(koy);
        if (sinirBaskisi == 0)
        {
            return "";
        }
        return "Sinir Baskisi: " + IsaretliMetin(sinirBaskisi);
    }

    public void SadakatiGunlukGuncelle()
    {
        foreach (KoyData koy in Koyler)
        {
            if (koy.Sahip != OyuncuKralligi)
            {
                continue;
            }

            int sinirBaskisi = SinirBaskisiHesapla(koy);
            koy.Sadakat = Mathf.Clamp(koy.Sadakat + sinirBaskisi, 0, 100);
        }
    }
}
