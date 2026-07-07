using System.Collections.Generic;
using UnityEngine;

public class DayResolver
{
    public List<string> SonucMesajlariniOlustur(GameState state, List<OrderData> emirler)
    {
        List<string> sonucMesajlari = new List<string>();

        foreach (OrderData emir in emirler)
        {
            float zar = Random.Range(0f, 1f);
            bool basarili = zar < emir.BasariSansi;

            if (basarili)
            {
                state.StatDegistir(emir.EtkilenenStat, emir.BasariliDegisim);
                string mesaj = emir.EmirTuru + " basarili oldu! (zar: " + zar.ToString("F2") + ")";
                sonucMesajlari.Add(mesaj);
                Debug.Log(mesaj);
            }
            else
            {
                state.StatDegistir(emir.EtkilenenStat, emir.BasarisizDegisim);
                string mesaj = emir.EmirTuru + " basarisiz oldu. (zar: " + zar.ToString("F2") + ")";
                sonucMesajlari.Add(mesaj);
                Debug.Log(mesaj);
            }
        }

        return sonucMesajlari;
    }
}