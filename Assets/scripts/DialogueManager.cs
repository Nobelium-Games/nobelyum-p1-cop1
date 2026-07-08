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

    public void DiyalogBaslat(DialogueData diyalog, Sprite portre, string isim)
    {
        aktifDiyalog = diyalog;
        aktifNode = aktifDiyalog.Nodler[0];
        PortreImage.sprite = portre;
        NpcIsimText.text = isim;
        NodeGoster();
    }

    void NodeGoster()
{
    NpcSozuText.text = aktifNode.NPCSozu;

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

    bool SecenekKarsilanabilirMi(DialogueChoice secenek)
    {
        foreach (StatEtkisi etki in secenek.StatEtkileri)
        {
            if (etki.Miktar < 0 && GameManager.Instance.State.StatDegerAl(etki.StatAdi) + etki.Miktar < 0)
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
                GameManager.Instance.State.StatDegistir(etki.StatAdi, etki.Miktar);
            }
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
        DayCycleManager.Instance.SiradakiyeGec();
    }
}