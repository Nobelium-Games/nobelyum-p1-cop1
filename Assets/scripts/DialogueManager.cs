using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text NpcSozuText;
    public TMP_Text SecenekButon1Text;
    public TMP_Text SecenekButon2Text;

    private DialogueData aktifDiyalog;
    private DialogueNode aktifNode;
    public GameObject SecenekButon2;

    public void DiyalogBaslat(DialogueData diyalog)
    {
        aktifDiyalog = diyalog;
        aktifNode = aktifDiyalog.Nodler[0];
        NodeGoster();
    }

    void NodeGoster()
{
    NpcSozuText.text = aktifNode.NPCSozu;

    SecenekButon1Text.text = aktifNode.Secenekler[0].SecenekMetni;

    if (aktifNode.Secenekler.Count > 1)
    {
        SecenekButon2.SetActive(true);
        SecenekButon2Text.text = aktifNode.Secenekler[1].SecenekMetni;
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

    void SecenekUygula(DialogueChoice secenek)
    {
        if (!string.IsNullOrEmpty(secenek.EtkilenenStat))
        {
            GameManager.Instance.State.StatDegistir(secenek.EtkilenenStat, secenek.StatDegisimi);
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