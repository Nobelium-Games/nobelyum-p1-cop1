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

    void ZarAtVeUygula(GameState state, OrderData emir, List<string> mesajListesi)
    {
        float zar = Random.Range(0f, 1f);
        bool basarili = zar < emir.BasariSansi;

        if (emir.IsyanBastirir && emir.HedefKoy != null)
        {
            if (basarili)
            {
                emir.HedefKoy.IsyanHalinde = false;
                mesajListesi.Add("<color=green>" + emir.HedefKoy.Isim + " koyundeki isyan bastirildi!</color>");
            }
            else
            {
                mesajListesi.Add("<color=red>" + emir.HedefKoy.Isim + " koyundeki isyani bastiramadik.</color>");
            }
            return;
        }

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