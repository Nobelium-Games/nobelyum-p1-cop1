using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text NpcIsimText;
    public TMP_Text NpcSozuText;
    public TMP_Text SecenekButon1Text;
    public TMP_Text SecenekButon2Text;
    public Image PortreImage;

    private DialogueData aktifDiyalog;
    private DialogueNode aktifNode;
    public GameObject SecenekButon2;
    public Button SecenekButon1Buton;
    public Button SecenekButon2Buton;
    public OrderManager Orders;
    public GameObject DiyalogKutusuKok;

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

    SecenekButon1Buton.gameObject.SetActive(true);
    SecenekButon1Text.text = aktifNode.Secenekler[0].SecenekMetni;
    SecenekButon1Buton.interactable = SecenekKarsilanabilirMi(aktifNode.Secenekler[0]);

    if (aktifNode.Secenekler.Count > 1)
    {
        SecenekButon2.SetActive(true);
        SecenekButon2Text.text = aktifNode.Secenekler[1].SecenekMetni;
        SecenekButon2Buton.interactable = SecenekKarsilanabilirMi(aktifNode.Secenekler[1]);
    }
    else
    {
        SecenekButon2.SetActive(false);
    }
}

    public void Secenek1Secildi()
    {
        SecenekUygula(aktifNode.Secenekler[0]);
    }

    public void Secenek2Secildi()
    {
        SecenekUygula(aktifNode.Secenekler[1]);
    }

    public string MaliyetMetniAl(int secenekIndex)
    {
        if (secenekIndex >= aktifNode.Secenekler.Count)
        {
            return "";
        }

        OrderData emir = aktifNode.Secenekler[secenekIndex].VerilecekEmir;

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
            aktifKoy.Erzak += miktar;
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
            SecenekButon1Buton.gameObject.SetActive(false);
            SecenekButon2.SetActive(false);

            OrderData sablon = secenek.VerilecekEmir;
            KoySecimPaneli.Instance.KoySec((KoyData secilenKoy) =>
            {
                if (sablon.BinaSlotuKullanir)
                {
                    secilenKoy.DoluBinaSlotu++;
                }
                Orders.EmirEkle(sablon.KopyalaVeKoyAta(secilenKoy));
                DiyalogBitir();
            }, sablon);
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
        SecenekButon1Text.text = "";
        SecenekButon2Text.text = "";
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