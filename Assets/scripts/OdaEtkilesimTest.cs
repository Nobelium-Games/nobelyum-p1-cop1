using UnityEngine;

public class OdaEtkilesimTest : MonoBehaviour
{
    public GameObject DanismanListesiPaneli;
    public GameObject HaritaEkrani;

    public void KapiTiklandi()
    {
        DanismanListesiPaneli.SetActive(!DanismanListesiPaneli.activeSelf);
    }

    public void AnsiklopediTiklandi()
    {
        Debug.Log("Ansiklopedi tiklandi.");
    }

    public void HaritaTiklandi()
    {
        HaritaEkrani.SetActive(true);
    }
}
