using UnityEngine;

public class TahtOdasiTest : MonoBehaviour
{
    public void KoyluyeYardimEt()
    {
        GameManager.Instance.State.StatDegistir("Sadakat", 5);
        GameManager.Instance.State.StatDegistir("Erzak", -3);

        Debug.Log("Karar verildi -> Sadakat: " + GameManager.Instance.State.Sadakat +
                " Erzak: " + GameManager.Instance.State.StatDegerAl("Erzak"));
    }
}
