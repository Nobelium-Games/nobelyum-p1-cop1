using UnityEngine;

public class ManagerTest : MonoBehaviour
{
    public OrderManager Orders;

    public void InsaatciEmriVer()
    {
        OrderData emir = new OrderData("Insaatci", "Degirmen Insa Et", "Erzak", 15, -2, 0.7f);
        Orders.EmirEkle(emir);
    }
}
