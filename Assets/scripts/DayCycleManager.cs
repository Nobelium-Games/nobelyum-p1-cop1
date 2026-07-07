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
        gunlukSira = sequencer.SiradakiListeyiOlustur(GameManager.Instance.State, KoyluNpc, AskerNpc);
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
        Dialog.DiyalogBaslat(npc.Diyalog);
    }

    public void SiradakiyeGec()
    {
        suankiNpcIndex++;
        SiradakiNpcyiGoster();
    }

    public void UyuyaBas()
    {
        AsamaDegistir(GunAsamasi.Resolve);

        List<string> sonuclar = resolver.SonucMesajlariniOlustur(GameManager.Instance.State, Orders.BekleyenEmirler);

        Debug.Log("--- Gece Sonuclari ---");
        foreach (string mesaj in sonuclar)
        {
            Debug.Log(mesaj);
        }

        Orders.YeniDongueBasla();

        YeniGuneBasla();
    }
}