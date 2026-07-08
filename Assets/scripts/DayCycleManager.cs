using UnityEngine;
using System.Collections.Generic;

public enum GunAsamasi
{
    TahtOdasi,
    SahsiOda,
    Resolve
}

public class DayCycleManager : MonoBehaviour
{
    private List<string> sonGeceSonuclari = new List<string>();
    public static DayCycleManager Instance;

    public GunAsamasi SuankiAsama;

    public DialogueManager Dialog;
    public OrderManager Orders;

    public GameObject TahtOdasiPanel;
    public GameObject SahsiOdaPanel;

    public NPCData KoyluNpc;
    public NPCData AskerNpc;

    private DaySequencer sequencer = new DaySequencer();
    private DayResolver resolver = new DayResolver();

    private List<NPCData> gunlukSira;
    private int suankiNpcIndex = 0;
    private List<DevamEdenEmir> devamEdenEmirler = new List<DevamEdenEmir>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        YeniGuneBasla();
    }

    void AsamaDegistir(GunAsamasi yeniAsama)
    {
        SuankiAsama = yeniAsama;

        TahtOdasiPanel.SetActive(yeniAsama == GunAsamasi.TahtOdasi);
        SahsiOdaPanel.SetActive(yeniAsama == GunAsamasi.SahsiOda);

        Debug.Log("Yeni asama: " + yeniAsama);
    }

    void YeniGuneBasla()
{
    GameManager.Instance.State.BaseGeliriUygula();

    gunlukSira = sequencer.SiradakiListeyiOlustur(GameManager.Instance.State, KoyluNpc, AskerNpc);

    if (sonGeceSonuclari.Count > 0)
    {
        NPCData elci = ScriptableObject.CreateInstance<NPCData>();
        elci.Isim = "Ulak";
        elci.Diyalog = ElciDiyaloguOlustur(sonGeceSonuclari);

        gunlukSira.Insert(0, elci);
    }

    suankiNpcIndex = 0;

    AsamaDegistir(GunAsamasi.TahtOdasi);
    SiradakiNpcyiGoster();
}

    void SiradakiNpcyiGoster()
    {
        if (suankiNpcIndex >= gunlukSira.Count)
        {
            AsamaDegistir(GunAsamasi.SahsiOda);
            return;
        }

        NPCData npc = gunlukSira[suankiNpcIndex];
        Debug.Log("Sirada: " + npc.Isim);
        Dialog.DiyalogBaslat(npc.Diyalog, npc.Portre);
    }

    public void SiradakiyeGec()
    {
        suankiNpcIndex++;
        SiradakiNpcyiGoster();
    }

    
    DialogueData ElciDiyaloguOlustur(List<string> sonuclar)
{
    DialogueData veri = ScriptableObject.CreateInstance<DialogueData>();
    veri.DialogueID = "ulak";

    string birlesikSoz = "Kralim, dun geceki haberler:\n";
    foreach (string mesaj in sonuclar)
    {
        birlesikSoz += "- " + mesaj + "\n";
    }

    DialogueChoice secenek = new DialogueChoice();
    secenek.SecenekMetni = "Anladim";
    secenek.SonrakiNodeID = "";
    secenek.EtkilenenStat = "";
    secenek.StatDegisimi = 0;

    DialogueNode node = new DialogueNode();
    node.NodeID = "baslangic";
    node.NPCSozu = birlesikSoz;
    node.Secenekler = new List<DialogueChoice> { secenek };

    veri.Nodler = new List<DialogueNode> { node };

    return veri;
}
    
    public void UyuyaBas()
{
    AsamaDegistir(GunAsamasi.Resolve);

    sonGeceSonuclari = new List<string>();

    devamEdenEmirler = resolver.SonucMesajlariniOlustur(
        GameManager.Instance.State,
        Orders.BekleyenEmirler,
        devamEdenEmirler,
        sonGeceSonuclari
    );

    Debug.Log("--- Gece Sonuclari ---");
    foreach (string mesaj in sonGeceSonuclari)
    {
        Debug.Log(mesaj);
    }

    Orders.YeniDongueBasla();

    YeniGuneBasla();
}

}