using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KoyEtiketiTiklama : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        string etiketIsmi = GetComponent<TMP_Text>().text.Trim();

        foreach (KoyData koy in KoyYoneticisi.Instance.Koyler)
        {
            if (!string.IsNullOrEmpty(koy.Isim) && string.Equals(koy.Isim.Trim(), etiketIsmi, System.StringComparison.OrdinalIgnoreCase))
            {
                KoyBilgiPaneli.Instance.Goster(koy);
                return;
            }
        }

        Debug.LogWarning("Eslesen koy bulunamadi: '" + etiketIsmi + "'");
    }
}
