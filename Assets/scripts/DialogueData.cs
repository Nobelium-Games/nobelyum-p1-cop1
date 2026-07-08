using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YeniDiyalog", menuName = "Diyalog/Diyalog Verisi")]
public class DialogueData : ScriptableObject
{
    public string DialogueID;
    public List<DialogueNode> Nodler;
}

[System.Serializable]
public class DialogueNode
{
    public string NodeID;
    public string NPCSozu;
    public List<DialogueChoice> Secenekler;
}

[System.Serializable]
public class DialogueChoice
{
    public string SecenekMetni;
    public string SonrakiNodeID;
    public List<StatEtkisi> StatEtkileri;
}

[System.Serializable]
public class StatEtkisi
{
    public string StatAdi;
    public int Miktar;
}
