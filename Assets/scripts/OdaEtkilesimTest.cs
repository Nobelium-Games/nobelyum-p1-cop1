using UnityEngine;

public class OdaEtkilesimTest : MonoBehaviour
{
    public GameObject DanismanListesiPaneli;

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
        Debug.Log("Harita tiklandi.");
    }

    public void MektuplarTiklandi()
    {
        Debug.Log("Mektuplar tiklandi.");
    }
}
