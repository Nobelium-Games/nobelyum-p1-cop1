using UnityEngine;

public class DanismanCagir : MonoBehaviour
{
    public DialogueManager Dialog;
    public GameObject DanismanListesiPaneli;

    public NPCData General;
    public NPCData MaliyeBakani;

    public void GeneralCagir()
    {
        DanismanListesiPaneli.SetActive(false);
        Dialog.DiyalogBaslat(General.Diyalog, General.Portre, General.Isim, true);
    }

    public void MaliyeBakaniCagir()
    {
        DanismanListesiPaneli.SetActive(false);
        Dialog.DiyalogBaslat(MaliyeBakani.Diyalog, MaliyeBakani.Portre, MaliyeBakani.Isim, true);
    }
}
