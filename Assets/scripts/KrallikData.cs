using UnityEngine;

[CreateAssetMenu(fileName = "YeniKrallik", menuName = "Krallik/Krallik Verisi")]
public class KrallikData : ScriptableObject
{
    public string Isim;
    public Color HaritaRengi = Color.white;
    public Sprite Bayrak;

    // Bu krallligin baskenti sayilan koyun ISMI (referans degil - KoyData bir
    // ScriptableObject olmadigi icin Inspector'dan dogrudan referans verilemiyor,
    // KoyYoneticisi bu isimle Koyler listesinde arama yapiyor). Isim, KoyYoneticisi'ndeki
    // ilgili KoyData'nin Isim alaniyla BIREBIR ayni yazilmali (bkz. KoyYoneticisi.BaskentiBul).
    public string BaskentIsmi;
}
