using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(UpdateValue);
        UpdateValue(slider.value);
    }

    void UpdateValue(float value)
    {
        text.SetText(Math.Round(value,1).ToString());
    }
}
