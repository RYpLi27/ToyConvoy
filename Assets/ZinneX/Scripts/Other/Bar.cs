using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    [SerializeField] private Image fillBar;
    [SerializeField] private Image trailBar;
    [SerializeField] [InfoBox("Text Is Not Needed")] private TMP_Text valueText;

    [SerializeField] private float trailDelay;

    public void UpdateUI(float curValue, float maxValue) {
        float ratio = curValue / maxValue;
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(fillBar.DOFillAmount(ratio, .25f)).SetEase(Ease.InOutSine);
        sequence.AppendInterval(trailDelay);
        sequence.Append(trailBar.DOFillAmount(ratio, .3f)).SetEase(Ease.InOutSine);
        
        sequence.Play();

        if (valueText == null) return;
            valueText.text = $"{curValue}/{maxValue}";
    }
    
    public void UpdateUIInstant(float curValue, float maxValue) {
        float ratio = curValue / maxValue;

        fillBar.fillAmount = ratio;

        if (valueText == null) return;
        valueText.text = $"{curValue}/{maxValue}";
    }
    
    public void UpdateUIInstantWithTrail(float curValue, float maxValue) {
        float ratio = curValue / maxValue;

        fillBar.fillAmount = ratio;
        trailBar.fillAmount = ratio;

        if (valueText == null) return;
        valueText.text = $"{curValue}/{maxValue}";
    }
}
