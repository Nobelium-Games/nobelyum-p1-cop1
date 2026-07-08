using UnityEngine;
using UnityEngine.UI;

public class DanismanButon : MonoBehaviour
{
    public string DanismanTipi;
    public OrderManager Orders;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        button.interactable = !Orders.DanismanKullanildiMi(DanismanTipi);
    }
}
