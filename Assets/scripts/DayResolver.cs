using System.Collections.Generic;
using UnityEngine;

public class DayResolver
{
    public List<DevamEdenEmir> SonucMesajlariniOlustur(GameState state, List<OrderData> emirler,
        List<DevamEdenEmir> devamEdenler, List<string> mesajListesi)
    {
        // 1) Yeni verilen emirleri isle
        foreach (OrderData emir in emirler)
        {
            if (emir.ToplamSure <= 1)
            {
                // Aninda sonuclanan emir (tek gunluk, hep sansa bagli)
                ZarAtVeUygula(state, emir, mesajListesi);
            }
            else
            {
                // Cok gunlu emir - hep baslar, suresi dolunca sonuclanacak
                devamEdenler.Add(new DevamEdenEmir(emir, emir.ToplamSure));
            }
        }

        // 2) Devam eden isleri bir gun ilerlet
        List<DevamEdenEmir> halaDevamEdenler = new List<DevamEdenEmir>();

        foreach (DevamEdenEmir devam in devamEdenler)
        {
            devam.KalanGun--;

            if (devam.KalanGun <= 0)
            {
                if (devam.Emir.SonucSansaBagli)
                {
                    ZarAtVeUygula(state, devam.Emir, mesajListesi);
                }
                else if (devam.Emir.GarnizonEkler)
                {
                    if (devam.Emir.HedefKoy != null)
                    {
                        devam.Emir.HedefKoy.Garnizon = Mathf.Min(
                            devam.Emir.HedefKoy.Garnizon + devam.Emir.GonderilenManpower,
                            devam.Emir.HedefKoy.MaxGarnizon);
                        BildirimYoneticisi.Instance.Bildirim("Garnizon (" + devam.Emir.HedefKoy.Isim + ")", devam.Emir.GonderilenManpower);
                        mesajListesi.Add("<color=yellow>" + devam.Emir.GonderilenManpower + " asker " + devam.Emir.HedefKoy.Isim + " koyune ulasti, garnizona katildi.</color>");
                    }
                }
                else if (devam.Emir.BaseGeliriEtkiler)
                {
                    if (devam.Emir.HedefKoy != null && !string.IsNullOrEmpty(devam.Emir.HedefKoy.Isim) && devam.Emir.BaseGeliriStat == "Erzak")
                    {
                        int eskiYield = devam.Emir.HedefKoy.ErzakYield;
                        devam.Emir.HedefKoy.ErzakYield = eskiYield * 2;
                        BildirimYoneticisi.Instance.Bildirim(
                            "Erzak Yield (" + devam.Emir.HedefKoy.Isim + ")", devam.Emir.HedefKoy.ErzakYield - eskiYield);
                    }
                    else
                    {
                        state.BaseGeliriArtir(devam.Emir.BaseGeliriStat, devam.Emir.BaseGeliriMiktar);
                        BildirimYoneticisi.Instance.Bildirim(devam.Emir.BaseGeliriStat + " Base Degeri", devam.Emir.BaseGeliriMiktar);
                    }
                    mesajListesi.Add("<color=yellow>" + devam.Emir.EmirTuru + " tamamlandi!</color>");
                }
                else
                {
                    state.StatDegistir(devam.Emir.EtkilenenStat, devam.Emir.BasariliDegisim);
                    BildirimYoneticisi.Instance.Bildirim(devam.Emir.EtkilenenStat, devam.Emir.BasariliDegisim);
                    mesajListesi.Add("<color=yellow>" + devam.Emir.EmirTuru + " tamamlandi!</color>");
                }
            }
            else
            {
                halaDevamEdenler.Add(devam);
                mesajListesi.Add(devam.Emir.EmirTuru + " devam ediyor, " + devam.KalanGun + " gun kaldi.");
            }
        }

        return halaDevamEdenler;
    }

    void AskeriGeriGonder(GameState state, OrderData emir, int miktar)
    {
        if (emir.KaynakKoy != null)
        {
            emir.KaynakKoy.Garnizon = Mathf.Min(emir.KaynakKoy.Garnizon + miktar, emir.KaynakKoy.MaxGarnizon);
            BildirimYoneticisi.Instance.Bildirim("Garnizon (" + emir.KaynakKoy.Isim + ")", miktar);
        }
        else
        {
            state.StatDegistir("Manpower", miktar);
            BildirimYoneticisi.Instance.Bildirim("Manpower", miktar);
        }
    }

    void ZarAtVeUygula(GameState state, OrderData emir, List<string> mesajListesi)
    {
        if (emir.DiplomasiyiArttirir && emir.HedefKrallik != null)
        {
            KoyYoneticisi.Instance.DiplomasiDegistir(emir.HedefKrallik, emir.DiplomasiMiktari);
            mesajListesi.Add("<color=green>" + emir.HedefKrallik.Isim + " ile iliskilerimiz gelisti. (+" + emir.DiplomasiMiktari + ")</color>");
            return;
        }

        if (emir.BarisTeklifEder && emir.HedefKrallik != null)
        {
            float barisSansi = KoyYoneticisi.Instance.DiplomasiDegerAl(emir.HedefKrallik) / 100f;
            bool barisBasarili = Random.Range(0f, 1f) < barisSansi;

            if (barisBasarili)
            {
                KoyYoneticisi.Instance.BarisYap(emir.HedefKrallik);
                mesajListesi.Add("<color=green>" + emir.HedefKrallik.Isim + " ile baris yapildi!</color>");
            }
            else
            {
                mesajListesi.Add("<color=red>" + emir.HedefKrallik.Isim + " baris teklifimizi reddetti.</color>");
            }
            return;
        }

        if (emir.GarnizonEkler && emir.HedefKoy != null)
        {
            emir.HedefKoy.Garnizon = Mathf.Min(emir.HedefKoy.Garnizon + emir.GonderilenManpower, emir.HedefKoy.MaxGarnizon);
            BildirimYoneticisi.Instance.Bildirim("Garnizon (" + emir.HedefKoy.Isim + ")", emir.GonderilenManpower);
            mesajListesi.Add("<color=yellow>" + emir.GonderilenManpower + " asker " + emir.HedefKoy.Isim + " koyune ulasti, garnizona katildi.</color>");
            return;
        }

        if (emir.IsyanBastirir && emir.HedefKoy != null)
        {
            int toplamGuc = emir.GonderilenManpower + emir.HedefKoy.Nufus;
            float bastirmaSansi = toplamGuc <= 0 ? 0.5f : (float)emir.GonderilenManpower / toplamGuc;
            bool bastirmaBasarili = Random.Range(0f, 1f) < bastirmaSansi;

            float kayipYuzdesi = bastirmaBasarili ? 0.15f : 0.80f;
            int kaybedilenManpower = Mathf.RoundToInt(emir.GonderilenManpower * kayipYuzdesi);
            int geriDonenManpower = emir.GonderilenManpower - kaybedilenManpower;

            if (geriDonenManpower > 0)
            {
                AskeriGeriGonder(state, emir, geriDonenManpower);
            }

            if (bastirmaBasarili)
            {
                emir.HedefKoy.IsyanHalinde = false;
                emir.HedefKoy.Sadakat = Mathf.Clamp(emir.HedefKoy.Sadakat + 10, 0, 100);
                BildirimYoneticisi.Instance.Bildirim("Sadakat (" + emir.HedefKoy.Isim + ")", 10);
                mesajListesi.Add("<color=green>" + emir.HedefKoy.Isim + " koyundeki isyan bastirildi! (" + kaybedilenManpower + " asker kaybettik, " + geriDonenManpower + " asker geri dondu)</color>");
            }
            else
            {
                mesajListesi.Add("<color=red>" + emir.HedefKoy.Isim + " koyundeki isyani bastiramadik. (" + kaybedilenManpower + " asker kaybettik, " + geriDonenManpower + " asker geri dondu)</color>");
            }
            return;
        }

        if (emir.SaldiriBaslatir && emir.HedefKoy != null)
        {
            bool saldiriBasarili;
            if (emir.HedefKoy.Garnizon <= 0)
            {
                // Hedef koyde hic garnizon yoksa savunmasiz sayilir, zar atilmadan direkt ele geciriliyor.
                saldiriBasarili = true;
            }
            else
            {
                float etkinSavunma = KoyYoneticisi.Instance.EtkinSavunmaHesapla(emir.HedefKoy);
                float toplamGuc = emir.GonderilenManpower + etkinSavunma;
                float saldiriSansi = toplamGuc <= 0f ? 0.5f : emir.GonderilenManpower / toplamGuc;
                saldiriBasarili = Random.Range(0f, 1f) < saldiriSansi;
            }

            float kayipYuzdesi = saldiriBasarili ? 0.15f : 0.80f;
            int kaybedilenManpower = Mathf.RoundToInt(emir.GonderilenManpower * kayipYuzdesi);
            int hayattaKalanManpower = emir.GonderilenManpower - kaybedilenManpower;

            if (saldiriBasarili)
            {
                emir.HedefKoy.Sahip = KoyYoneticisi.Instance.OyuncuKralligi;
                emir.HedefKoy.Garnizon = Mathf.Min(hayattaKalanManpower, emir.HedefKoy.MaxGarnizon);

                if (HexHaritaCizici.Instance != null)
                {
                    HexHaritaCizici.Instance.RenkleriGuncelle();
                }

                mesajListesi.Add("<color=green>" + emir.HedefKoy.Isim + " koyu ele gecirildi! (" + kaybedilenManpower + " asker kaybettik, " + hayattaKalanManpower + " asker koyde garnizon olarak kaldi)</color>");
            }
            else
            {
                if (hayattaKalanManpower > 0)
                {
                    AskeriGeriGonder(state, emir, hayattaKalanManpower);
                }
                mesajListesi.Add("<color=red>" + emir.HedefKoy.Isim + " koyune saldiri basarisiz oldu. (" + kaybedilenManpower + " asker kaybettik, " + hayattaKalanManpower + " asker geri dondu)</color>");
            }
            return;
        }

        float zar = Random.Range(0f, 1f);
        bool basarili = zar < emir.BasariSansi;

        if (basarili)
        {
            state.StatDegistir(emir.EtkilenenStat, emir.BasariliDegisim);
            BildirimYoneticisi.Instance.Bildirim(emir.EtkilenenStat, emir.BasariliDegisim);
            mesajListesi.Add("<color=green>" + emir.EmirTuru + " basarili oldu!</color>");
        }
        else
        {
            state.StatDegistir(emir.EtkilenenStat, emir.BasarisizDegisim);
            BildirimYoneticisi.Instance.Bildirim(emir.EtkilenenStat, emir.BasarisizDegisim);
            mesajListesi.Add("<color=red>" + emir.EmirTuru + " basarisiz oldu.</color>");
        }
    }
}