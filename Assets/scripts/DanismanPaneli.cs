using UnityEngine;

public class DanismanPaneli : MonoBehaviour
{
    public OrderManager Orders;

    public void InsaatciEmriVer()
    {
        // Degirmen: 3 gun surer, suresi dolunca GARANTI tamamlanir, 20 Altin maliyeti var
        // Tamamlaninca Erzak'a direkt eklemek yerine, Erzak'in base gelirini kalici olarak +1 artirir
        OrderData emir = new OrderData("Insaatci", "Degirmen Insa Et", "Erzak", 15, -2, 0f, 3, false, "Altin", 20,
            true, "Erzak", 1);
        Orders.EmirEkle(emir);
    }

    public void AskerbasiEmriVer()
    {
        // Yagma: 2 gun surer, suresi dolunca %50 sansla basarili olur, 15 Manpower maliyeti var
        OrderData emir = new OrderData("Askerbasi", "Koy Yagmala", "Erzak", 25, -10, 0.5f, 2, true, "Manpower", 15);
        Orders.EmirEkle(emir);
    }

    public void AskerToplaEmriVer()
    {
        // Asker Topla: ayni gun (1 gun) icinde GARANTI sonuclanir, 15 Altin maliyeti var, Manpower +10 kazandirir
        // Not: Yagma ile ayni danisman (Askerbasi) oldugu icin, bir dongude ikisinden sadece biri secilebilir
        OrderData emir = new OrderData("Askerbasi", "Asker Topla", "Manpower", 10, 0, 1f, 1, false, "Altin", 15);
        Orders.EmirEkle(emir);
    }
}
