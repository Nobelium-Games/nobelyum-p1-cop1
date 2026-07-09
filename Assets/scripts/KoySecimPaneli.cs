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

    public void KoySec(Action<KoyData> callback)
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
            yeniButon.GetComponentInChildren<TMP_Text>().text = koy.Isim;

            KoyData secilenKoy = koy;
            yeniButon.GetComponent<Button>().onClick.AddListener(() => KoySecildi(secilenKoy));

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
