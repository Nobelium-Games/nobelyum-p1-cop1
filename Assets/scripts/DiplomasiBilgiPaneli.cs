using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiplomasiBilgiPaneli : MonoBehaviour
{
    public static DiplomasiBilgiPaneli Instance;

    public GameObject Panel;
    public TMP_Text IsimText;
    public Image BayrakImage;
    public TMP_Text DiplomasiText;
    public TMP_Text DurumText;

    void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
    }

    public void Goster(KrallikData krallik)
    {
        IsimText.text = krallik.Isim;

        bool bayrakVar = krallik.Bayrak != null;
        BayrakImage.gameObject.SetActive(bayrakVar);
        if (bayrakVar)
        {
            BayrakImage.sprite = krallik.Bayrak;
        }

        DiplomasiText.text = "Diplomasi: " + KoyYoneticisi.Instance.DiplomasiDegerAl(krallik);
        DurumText.text = KoyYoneticisi.Instance.SavastaMi(krallik) ? "<color=red>SAVASTA</color>" : "<color=green>BARISTA</color>";

        Panel.SetActive(true);
    }

    public void Kapat()
    {
        Panel.SetActive(false);
    }
}
