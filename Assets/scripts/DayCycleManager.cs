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
    public NPCData AyyasNpc;

    private DaySequencer sequencer = new DaySequencer();
    private DayResolver resolver = new DayResolver();

    private List<SiraGirisi> gunlukSira;
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
    KoyYoneticisi.Instance.ErzagiGunlukArtir();
    GameManager.Instance.State.Altin += KoyYoneticisi.Instance.ToplamAltinYieldi();

    gunlukSira = sequencer.SiradakiListeyiOlustur(GameManager.Instance.State, KoyluNpc, AskerNpc, AyyasNpc);

    if (sonGeceSonuclari.Count > 0)
    {
        NPCData elci = ScriptableObject.CreateInstance<NPCData>();
        elci.Isim = "Ulak";
        elci.Diyalog = ElciDiyaloguOlustur(sonGeceSonuclari);

        gunlukSira.Insert(0, new SiraGirisi { Npc = elci, IlgiliKoy = null });
    }

    suankiNpcIndex = 0;

    AsamaDegistir(GunAsamasi.TahtOdasi);
    SiradakiNpcyiGoster();
}

    void SiradakiNpcyiGoster()
    {
        if (suankiNpcIndex >= gunlukSira.Count)
        {
            StartCoroutine(EkranGecisi.Instance.KararipAcil(() => AsamaDegistir(GunAsamasi.SahsiOda)));
            return;
        }

        SiraGirisi girisi = gunlukSira[suankiNpcIndex];
        NPCData npc = girisi.Npc;
        Debug.Log("Sirada: " + npc.Isim);
        Dialog.DiyalogBaslat(npc.Diyalog, npc.Portre, npc.Isim, false, girisi.IlgiliKoy);
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
    secenek.StatEtkileri = new List<StatEtkisi>();

    DialogueNode node = new DialogueNode();
    node.NodeID = "baslangic";
    node.NPCSozu = birlesikSoz;
    node.Secenekler = new List<DialogueChoice> { secenek };

    veri.Nodler = new List<DialogueNode> { node };

    return veri;
}
    
    public void UyuyaBas()
{
    if (KoySecimPaneli.Instance != null)
    {
        KoySecimPaneli.Instance.Panel.SetActive(false);
    }

    AsamaDegistir(GunAsamasi.Resolve);

    GameManager.Instance.State.Gun++;
    Debug.Log("Yeni gun: " + GameManager.Instance.State.Gun);

    sonGeceSonuclari = new List<string>();

    KoyYoneticisi.Instance.IsyanKontrolEt(sonGeceSonuclari);

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