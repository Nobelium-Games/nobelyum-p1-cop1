using UnityEngine;

[CreateAssetMenu(fileName = "YeniKrallik", menuName = "Krallik/Krallik Verisi")]
public class KrallikData : ScriptableObject
{
    public string Isim;
    public Color HaritaRengi = Color.white;
    public Sprite Bayrak;
}
