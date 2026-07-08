using UnityEngine;
using TMPro;

public class GunUI : MonoBehaviour
{
    public TMP_Text GunText;

    void Update()
    {
        GunText.text = "Gun " + GameManager.Instance.State.Gun;
    }
}
