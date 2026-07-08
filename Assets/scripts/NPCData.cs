using UnityEngine;

[CreateAssetMenu(fileName = "YeniNPC", menuName = "NPC/NPC Verisi")]
public class NPCData : ScriptableObject
{
    public string ID;
    public string Isim;
    public DialogueData Diyalog;
    public Sprite Portre;
}