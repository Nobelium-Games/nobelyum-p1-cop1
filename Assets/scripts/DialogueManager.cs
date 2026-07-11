using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text NpcIsimText;
    public TMP_Text NpcSozuText;
    public Image PortreImage;

    private DialogueData aktifDiyalog;
    private DialogueNode aktifNode;
    public GameObject SecenekButonSablonu;
    public Transform SecenekIcerik;
    public OrderManager Orders;
    public GameObject DiyalogKutusuKok;

    private List<GameObject> olusturulanSecenekButonlari = new List<GameObject>();

    private bool aktifDanismanDiyalogu = false;
    private KoyData aktifKoy;

    public void DiyalogBaslat(DialogueData diyalog, Sprite portre, string isim, bool danismanDiyalogu = false, KoyData ilgiliKoy = null)
    {
        aktifDiyalog = diyalog;
        aktifNode = aktifDiyalog.Nodler[0];
        aktifDanismanDiyalogu = danismanDiyalogu;
        aktifKoy = ilgiliKoy;
        PortreImage.sprite = portre;
        NpcIsimText.text = isim;
        DiyalogKutusuKok.SetActive(true);
        NodeGoster();
    }

    void NodeGoster()
{
    string soz = aktifNode.NPCSozu;
    if (aktifKoy != null)
    {
        soz = soz.Replace("{KOY}", aktifKoy.Isim);
    }
    NpcSozuText.text = soz;

    foreach (GameObject eskiButon in olusturulanSecenekButonlari)
    {
        Destroy(eskiButon);
    }
    olusturulanSecenekButonlari.Clear();

    foreach (DialogueChoice secenek in aktifNode.Secenekler)
    {
        GameObject yeniButon = Instantiate(SecenekButonSablonu, SecenekIcerik);
        yeniButon.SetActive(true);
        yeniButon.GetComponentInChildren<TMP_Text>().text = secenek.SecenekMetni;

        Button buton = yeniButon.GetComponent<Button>();
        buton.interactable = SecenekKarsilanabilirMi(secenek);

        DialogueChoice secilenSecenek = secenek;
        buton.onClick.AddListener(() => SecenekUygula(secilenSecenek));

        SecenekTooltip tooltip = yeniButon.GetComponent<SecenekTooltip>();
        if (tooltip != null)
        {
            tooltip.Dialog = this;
            tooltip.Secenek = secilenSecenek;
        }

        olusturulanSecenekButonlari.Add(yeniButon);
    }
}

    void KoySecimGoster(OrderData sablon, Action<KoyData> callback)
    {
        NpcSozuText.text = "Hangi koy?";

        foreach (GameObject eskiButon in olusturulanSecenekButonlari)
        {
            Destroy(eskiButon);
        }
        olusturulanSecenekButonlari.Clear();

        foreach (KoyData koy in KoyYoneticisi.Instance.Koyler)
        {
            if (sablon.DusmanKoyuGerekli && koy.Sahip == KoyYoneticisi.Instance.OyuncuKralligi)
            {
                continue;
            }

            if (!sablon.DusmanKoyuGerekli && koy.Sahip != KoyYoneticisi.Instance.OyuncuKralligi)
            {
                continue;
            }

            GameObject yeniButon = Instantiate(SecenekButonSablonu, SecenekIcerik);
            yeniButon.SetActive(true);

            bool slotDolu = sablon.BinaSlotuKullanir && koy.DoluBinaSlotu >= koy.MaxBinaSlotu;
            bool isyanEngeli = sablon.BinaSlotuKullanir && koy.IsyanHalinde;
            bool isyansizEngeli = sablon.IsyanliKoyGerekli && !koy.IsyanHalinde;
            bool tiklanamaz = slotDolu || isyanEngeli || isyansizEngeli;

            string etiket = koy.Isim;
            if (isyanEngeli)
            {
                etiket += " (Isyan Halinde)";
            }
            else if (slotDolu)
            {
                etiket += " (Dolu)";
            }
            else if (isyansizEngeli)
            {
                etiket += " (Isyan Yok)";
            }

            yeniButon.GetComponentInChildren<TMP_Text>().text = etiket;

            Button buton = yeniButon.GetComponent<Button>();
            buton.interactable = !tiklanamaz;

            KoyData secilenKoy = koy;
            buton.onClick.AddListener(() => callback(secilenKoy));

            SecenekTooltip tooltip = yeniButon.GetComponent<SecenekTooltip>();
            if (tooltip != null)
            {
                tooltip.Dialog = this;
                tooltip.Secenek = null;
            }

            olusturulanSecenekButonlari.Add(yeniButon);
        }
    }

    public string MaliyetMetniAl(DialogueChoice secenek)
    {
        OrderData emir = secenek != null ? secenek.VerilecekEmir : null;

        if (emir == null || string.IsNullOrEmpty(emir.MaliyetStat) || emir.MaliyetMiktar == 0)
        {
            return "";
        }

        return "-" + emir.MaliyetMiktar + " " + emir.MaliyetStat;
    }

    int GuncelDeger(string statAdi)
    {
        if (aktifKoy != null && statAdi == "Erzak")
        {
            return aktifKoy.Erzak;
        }
        if (aktifKoy != null && statAdi == "Sadakat")
        {
            return aktifKoy.Sadakat;
        }
        return GameManager.Instance.State.StatDegerAl(statAdi);
    }

    void DegeriUygula(string statAdi, int miktar)
    {
        if (aktifKoy != null && statAdi == "Erzak")
        {
            aktifKoy.Erzak = Mathf.Max(0, aktifKoy.Erzak + miktar);
        }
        else if (aktifKoy != null && statAdi == "Sadakat")
        {
            aktifKoy.Sadakat = Mathf.Clamp(aktifKoy.Sadakat + miktar, 0, 100);
        }
        else
        {
            GameManager.Instance.State.StatDegistir(statAdi, miktar);
        }
    }

    bool SecenekKarsilanabilirMi(DialogueChoice secenek)
    {
        foreach (StatEtkisi etki in secenek.StatEtkileri)
        {
            if (etki.StatAdi == "Sadakat")
            {
                continue; // Sadakat harcanan bir kaynak degil, 0-100 arasinda kelepceleniyor, hicbir secenegi kilitlemez
            }

            if (etki.Miktar < 0 && GuncelDeger(etki.StatAdi) + etki.Miktar < 0)
            {
                return false;
            }
        }

        OrderData emir = secenek.VerilecekEmir;
        if (emir != null && !string.IsNullOrEmpty(emir.MaliyetStat))
        {
            if (GameManager.Instance.State.StatDegerAl(emir.MaliyetStat) - emir.MaliyetMiktar < 0)
            {
                return false;
            }
        }

        return true;
    }

    void SecenekUygula(DialogueChoice secenek)
    {
        if (!SecenekKarsilanabilirMi(secenek))
        {
            Debug.Log("Bu secenek icin yeterli kaynak yok: " + secenek.SecenekMetni);
            return;
        }

        foreach (StatEtkisi etki in secenek.StatEtkileri)
        {
            if (!string.IsNullOrEmpty(etki.StatAdi))
            {
                DegeriUygula(etki.StatAdi, etki.Miktar);

                string bildirimAdi = aktifKoy != null && (etki.StatAdi == "Erzak" || etki.StatAdi == "Sadakat")
                    ? etki.StatAdi + " (" + aktifKoy.Isim + ")"
                    : etki.StatAdi;
                BildirimYoneticisi.Instance.Bildirim(bildirimAdi, etki.Miktar);
            }
        }

        TooltipUI.Instance.Gizle();

        if (secenek.VerilecekEmir != null && !string.IsNullOrEmpty(secenek.VerilecekEmir.DanismanTipi) && secenek.VerilecekEmir.KoySecimiGerekli)
        {
            // Koy secilene kadar diyalog kutusu acik kalir, secim yapilinca kapanir
            OrderData sablon = secenek.VerilecekEmir;
            KoySecimGoster(sablon, (KoyData secilenKoy) =>
            {
                if (sablon.BinaSlotuKullanir)
                {
                    secilenKoy.DoluBinaSlotu++;
                }

                if (sablon.ManpowerMiktariSorulsun)
                {
                    ManpowerSeciciPaneli.Instance.Sor((int miktar) =>
                    {
                        GameManager.Instance.State.StatDegistir("Manpower", -miktar);
                        BildirimYoneticisi.Instance.Bildirim("Manpower", -miktar);

                        OrderData kopya = sablon.KopyalaVeKoyAta(secilenKoy);
                        kopya.MaliyetStat = "";
                        kopya.MaliyetMiktar = 0;
                        kopya.GonderilenManpower = miktar;
                        Orders.EmirEkle(kopya);
                        DiyalogBitir();
                    });
                    return;
                }

                Orders.EmirEkle(sablon.KopyalaVeKoyAta(secilenKoy));
                DiyalogBitir();
            });
            return;
        }

        if (secenek.VerilecekEmir != null && !string.IsNullOrEmpty(secenek.VerilecekEmir.DanismanTipi))
        {
            Orders.EmirEkle(secenek.VerilecekEmir);
        }

        Debug.Log("Secenek uygulandi: " + secenek.SecenekMetni +
            " -> Sadakat: " + GameManager.Instance.State.Sadakat);

        if (string.IsNullOrEmpty(secenek.SonrakiNodeID))
        {
            DiyalogBitir();
        }
        else
        {
            aktifNode = aktifDiyalog.Nodler.Find(n => n.NodeID == secenek.SonrakiNodeID);
            NodeGoster();
        }
    }

    void DiyalogBitir()
    {
        Debug.Log("Diyalog bitti.");
        NpcSozuText.text = "";
        foreach (GameObject eskiButon in olusturulanSecenekButonlari)
        {
            Destroy(eskiButon);
        }
        olusturulanSecenekButonlari.Clear();
        DiyalogKutusuKok.SetActive(false);
        TooltipUI.Instance.Gizle();
        aktifKoy = null;

        if (aktifDanismanDiyalogu)
        {
            return;
        }

        DayCycleManager.Instance.SiradakiyeGec();
    }
}