using System.Collections.Generic;
using UnityEngine;

public class DaySequencer
{
    public List<NPCData> SiradakiListeyiOlustur(GameState state, NPCData koyluNpc, NPCData askerNpc, NPCData ayyasNpc)
    {
        List<NPCData> yeniSira = new List<NPCData>();

        // Kural 0: 10. gun sabit hikaye karakteri (ayyas adam) gelir
        if (state.Gun == 10)
        {
            yeniSira.Add(ayyasNpc);
            Debug.Log("10. gun, ayyas adam geldi");
        }

        // Kural 1: Erzak dusukse, koylu gelme ihtimali yuksek
        if (state.Erzak < 40)
        {
            float sans = Random.Range(0f, 1f);
            if (sans < 0.8f) // %80 ihtimal
            {
                yeniSira.Add(koyluNpc);
                Debug.Log("Erzak dusuk, koylu geldi (sans: " + sans + ")");
            }
        }
        else
        {
            float sans = Random.Range(0f, 1f);
            if (sans < 0.2f) // erzak normalse dusuk ihtimalle yine gelebilir
            {
                yeniSira.Add(koyluNpc);
                Debug.Log("Erzak normal ama koylu yine de geldi (sans: " + sans + ")");
            }
        }

        // Kural 2: Asker her zaman gelsin (simdilik sabit/test amacli)
        yeniSira.Add(askerNpc);

        return yeniSira;
    }
}
