using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KoySecimPaneli : MonoBehaviour
{
    public static KoySecimPaneli Instance;

    public GameObject Panel;
    public GameObject ButonSablonu;

    private Action<KoyData> secilinceCagrilacak;
    private List<GameObject> olusturulanButonlar = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        ButonSablonu.SetActive(false);
    }

    public void KoySec(Action<KoyData> callback, OrderData emirSablonu = null)
    {
        secilinceCagrilacak = callback;

        foreach (GameObject eskiButon in olusturulanButonlar)
        {
            Destroy(eskiButon);
        }
        olusturulanButonlar.Clear();

        foreach (KoyData koy in KoyYoneticisi.Instance.Koyler)
        {
            GameObject yeniButon = Instantiate(ButonSablonu, ButonSablonu.transform.parent);
            yeniButon.SetActive(true);

            bool slotDolu = emirSablonu != null && emirSablonu.BinaSlotuKullanir
                && koy.DoluBinaSlotu >= koy.MaxBinaSlotu;

            yeniButon.GetComponentInChildren<TMP_Text>().text = slotDolu
                ? koy.Isim + " (Dolu)"
                : koy.Isim;

            Button buton = yeniButon.GetComponent<Button>();
            buton.interactable = !slotDolu;

            KoyData secilenKoy = koy;
            buton.onClick.AddListener(() => KoySecildi(secilenKoy));

            olusturulanButonlar.Add(yeniButon);
        }

        Panel.SetActive(true);
    }

    void KoySecildi(KoyData koy)
    {
        Panel.SetActive(false);
        secilinceCagrilacak?.Invoke(koy);
    }
}
