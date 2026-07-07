using UnityEngine;

public class ManagerTest : MonoBehaviour
{
    public OrderManager Orders;

    public void InsaatciEmriVer()
    {
        // Degirmen: 3 gun surer, suresi dolunca GARANTI tamamlanir
        OrderData emir = new OrderData("Insaatci", "Degirmen Insa Et", "Erzak", 15, -2, 0f, 3, false);
        Orders.EmirEkle(emir);
    }

    public void AskerbasiEmriVer()
    {
        // Yagma: 2 gun surer, suresi dolunca %50 sansla basarili olur
        OrderData emir = new OrderData("Askerbasi", "Koy Yagmala", "Erzak", 25, -10, 0.5f, 2, true);
        Orders.EmirEkle(emir);
    }
}