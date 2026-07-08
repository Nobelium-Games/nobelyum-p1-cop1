using UnityEngine;
using TMPro;
using System.Collections;

public class BildirimYoneticisi : MonoBehaviour
{
    public static BildirimYoneticisi Instance;

    public GameObject BildirimSablonu;
    public float GosterimSuresi = 2f;

    void Awake()
    {
        Instance = this;
        BildirimSablonu.SetActive(false);
    }

    public void Bildirim(string statAdi, int miktar)
    {
        GameObject yeni = Instantiate(BildirimSablonu, BildirimSablonu.transform.parent);
        yeni.SetActive(true);

        string isaret = miktar >= 0 ? "+" : "";
        string renk = miktar >= 0 ? "green" : "red";

        TMP_Text metin = yeni.GetComponent<TMP_Text>();
        metin.text = "<color=" + renk + ">" + isaret + miktar + " " + statAdi + "</color>";

        StartCoroutine(BirSureSonraYokEt(yeni));
    }

    IEnumerator BirSureSonraYokEt(GameObject obje)
    {
        yield return new WaitForSeconds(GosterimSuresi);
        Destroy(obje);
    }
}
