using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManpowerSeciciPaneli : MonoBehaviour
{
    public static ManpowerSeciciPaneli Instance;

    public GameObject Panel;
    public Slider MiktarSlider;
    public TMP_Text MevcutManpowerText;
    public TMP_Text SeciliMiktarText;

    private Action<int> secilinceCagrilacak;
    private int seciminMaksimumu;

    void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
        MiktarSlider.onValueChanged.AddListener(SliderDegisti);
    }

    public void Sor(int mevcutManpower, Action<int> callback)
    {
        secilinceCagrilacak = callback;
        seciminMaksimumu = mevcutManpower;

        MiktarSlider.wholeNumbers = true;
        MiktarSlider.minValue = 0;
        MiktarSlider.maxValue = mevcutManpower;
        MiktarSlider.value = 0;

        MevcutManpowerText.text = "Mevcut Manpower: " + mevcutManpower;
        SliderDegisti(0);

        Panel.SetActive(true);
    }

    void SliderDegisti(float deger)
    {
        SeciliMiktarText.text = "Gonderilecek: " + Mathf.RoundToInt(deger);
    }

    public void GonderTiklandi()
    {
        int miktar = Mathf.Clamp(Mathf.RoundToInt(MiktarSlider.value), 0, seciminMaksimumu);

        Panel.SetActive(false);
        secilinceCagrilacak?.Invoke(miktar);
    }
}
