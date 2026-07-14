using UnityEngine;

public static class SekilUretici
{
    public static Sprite HexagonSprite(int yukseklik = 64)
    {
        int genislik = Mathf.RoundToInt(yukseklik * Mathf.Sqrt(3f) / 2f);
        float yaricap = yukseklik / 2f - 1f;
        Vector2 merkez = new Vector2(genislik / 2f, yukseklik / 2f);

        Vector2[] koseler = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            float aciRadyan = (60 * i - 30) * Mathf.Deg2Rad;
            koseler[i] = merkez + new Vector2(Mathf.Cos(aciRadyan), Mathf.Sin(aciRadyan)) * yaricap;
        }

        return CokgenSpriteOlustur(genislik, yukseklik, koseler);
    }

    public static Sprite HexagonCerceveSprite(int yukseklik = 64, float kalinlik = 4f)
    {
        int genislik = Mathf.RoundToInt(yukseklik * Mathf.Sqrt(3f) / 2f);
        float disYaricap = yukseklik / 2f - 1f;
        float icYaricap = disYaricap - kalinlik;
        Vector2 merkez = new Vector2(genislik / 2f, yukseklik / 2f);

        Vector2[] disKoseler = new Vector2[6];
        Vector2[] icKoseler = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            float aciRadyan = (60 * i - 30) * Mathf.Deg2Rad;
            Vector2 yon = new Vector2(Mathf.Cos(aciRadyan), Mathf.Sin(aciRadyan));
            disKoseler[i] = merkez + yon * disYaricap;
            icKoseler[i] = merkez + yon * icYaricap;
        }

        Texture2D doku = new Texture2D(genislik, yukseklik);
        doku.filterMode = FilterMode.Bilinear;

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                Vector2 nokta = new Vector2(x + 0.5f, y + 0.5f);
                bool cerceve = NoktaCokgenIcindeMi(nokta, disKoseler) && !NoktaCokgenIcindeMi(nokta, icKoseler);
                doku.SetPixel(x, y, cerceve ? Color.white : Color.clear);
            }
        }
        doku.Apply();

        return Sprite.Create(doku, new Rect(0, 0, genislik, yukseklik), new Vector2(0.5f, 0.5f));
    }

    public static Sprite UcgenSprite(int boyut = 64)
    {
        return DuzenliCokgenSprite(boyut, 3, -90);
    }

    public static Sprite KareSprite(int boyut = 64)
    {
        return DuzenliCokgenSprite(boyut, 4, 45);
    }

    public static Sprite DaireSprite(int boyut = 64)
    {
        return DuzenliCokgenSprite(boyut, 32, 0);
    }

    static Sprite DuzenliCokgenSprite(int boyut, int koseSayisi, float baslangicAcisi)
    {
        Vector2 merkez = new Vector2(boyut / 2f, boyut / 2f);
        float yaricap = boyut / 2f - 1f;

        Vector2[] koseler = new Vector2[koseSayisi];
        for (int i = 0; i < koseSayisi; i++)
        {
            float aciRadyan = (baslangicAcisi + 360f / koseSayisi * i) * Mathf.Deg2Rad;
            koseler[i] = merkez + new Vector2(Mathf.Cos(aciRadyan), Mathf.Sin(aciRadyan)) * yaricap;
        }

        return CokgenSpriteOlustur(boyut, boyut, koseler);
    }

    static Sprite CokgenSpriteOlustur(int genislik, int yukseklik, Vector2[] koseler)
    {
        Texture2D doku = new Texture2D(genislik, yukseklik);
        doku.filterMode = FilterMode.Bilinear;

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                bool icinde = NoktaCokgenIcindeMi(new Vector2(x + 0.5f, y + 0.5f), koseler);
                doku.SetPixel(x, y, icinde ? Color.white : Color.clear);
            }
        }
        doku.Apply();

        return Sprite.Create(doku, new Rect(0, 0, genislik, yukseklik), new Vector2(0.5f, 0.5f));
    }

    static bool NoktaCokgenIcindeMi(Vector2 nokta, Vector2[] koseler)
    {
        bool icinde = false;
        int j = koseler.Length - 1;
        for (int i = 0; i < koseler.Length; i++)
        {
            if ((koseler[i].y > nokta.y) != (koseler[j].y > nokta.y) &&
                nokta.x < (koseler[j].x - koseler[i].x) * (nokta.y - koseler[i].y) / (koseler[j].y - koseler[i].y) + koseler[i].x)
            {
                icinde = !icinde;
            }
            j = i;
        }
        return icinde;
    }
}
