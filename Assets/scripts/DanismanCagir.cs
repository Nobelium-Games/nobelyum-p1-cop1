using UnityEngine;

public class DanismanCagir : MonoBehaviour
{
    public DialogueManager Dialog;
    public GameObject DanismanListesiPaneli;

    public NPCData General;
    public NPCData Insaatci;

    public void GeneralCagir()
    {
        DanismanListesiPaneli.SetActive(false);
        Dialog.DiyalogBaslat(General.Diyalog, General.Portre, General.Isim, true);
    }

    public void InsaatciCagir()
    {
        DanismanListesiPaneli.SetActive(false);
        Dialog.DiyalogBaslat(Insaatci.Diyalog, Insaatci.Portre, Insaatci.Isim, true);
    }
}
