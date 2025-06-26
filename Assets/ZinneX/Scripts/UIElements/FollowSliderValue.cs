using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FollowSliderValue : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    public void SetValue() {
        text.text = Math.Round(slider.value, 2).ToString();
    }
}
