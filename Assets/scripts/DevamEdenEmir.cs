using System;

[Serializable]
public class DevamEdenEmir
{
    public OrderData Emir;
    public int KalanGun;

    public DevamEdenEmir(OrderData emir, int kalanGun)
    {
        Emir = emir;
        KalanGun = kalanGun;
    }
}
