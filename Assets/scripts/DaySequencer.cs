using System.Collections.Generic;
using UnityEngine;

public class DaySequencer
{
    public List<SiraGirisi> SiradakiListeyiOlustur(GameState state, NPCData koyluNpc, NPCData askerNpc, NPCData ayyasNpc)
    {
        List<SiraGirisi> yeniSira = new List<SiraGirisi>();

        // Kural 0: 10. gun sabit hikaye karakteri (ayyas adam) gelir
        if (state.Gun == 10)
        {
            yeniSira.Add(new SiraGirisi { Npc = ayyasNpc, IlgiliKoy = null });
            Debug.Log("10. gun, ayyas adam geldi");
        }

        // Kural 1: Erzagi 50'nin altinda olan her koy, kendi zarini atar.
        // Zar, koyun erzagindan yuksek cikarsa o koy sikayetci koylusunu gonderir.
        foreach (KoyData koy in KoyYoneticisi.Instance.Koyler)
        {
            if (koy.Erzak < 50)
            {
                int zar = Random.Range(0, 51);
                if (zar > koy.Erzak)
                {
                    yeniSira.Add(new SiraGirisi { Npc = koyluNpc, IlgiliKoy = koy });
                    Debug.Log(koy.Isim + " icin koylu geldi (zar: " + zar + ", erzak: " + koy.Erzak + ")");
                }
            }
        }

        // Kural 2: Asker her zaman gelsin (simdilik sabit/test amacli)
        yeniSira.Add(new SiraGirisi { Npc = askerNpc, IlgiliKoy = null });

        return yeniSira;
    }
}
