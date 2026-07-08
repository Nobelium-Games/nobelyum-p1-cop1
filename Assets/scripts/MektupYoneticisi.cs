using UnityEngine;

public class MektupYoneticisi : MonoBehaviour
{
    public DialogueManager Dialog;
    public GameObject MektuplarPaneli;

    public DialogueData TestMektup;

    public void MektuplarTiklandi()
    {
        MektuplarPaneli.SetActive(!MektuplarPaneli.activeSelf);
    }

    public void TestMektubuAc()
    {
        MektuplarPaneli.SetActive(false);
        Dialog.DiyalogBaslat(TestMektup, null, "Mektup", true);
    }
}
