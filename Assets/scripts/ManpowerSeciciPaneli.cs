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

    void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
        MiktarSlider.onValueChanged.AddListener(SliderDegisti);
    }

    public void Sor(Action<int> callback)
    {
        secilinceCagrilacak = callback;

        int mevcutManpower = GameManager.Instance.State.Manpower;

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
        int miktar = Mathf.Clamp(Mathf.RoundToInt(MiktarSlider.value), 0, GameManager.Instance.State.Manpower);

        Panel.SetActive(false);
        secilinceCagrilacak?.Invoke(miktar);
    }
}
