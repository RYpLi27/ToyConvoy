using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoldPopup : MonoBehaviour {
    [SerializeField] private float timeToVanish;
    private TextMeshProUGUI text;

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        Color color = text.color;
        color.a = 1f;
        text.color = color;
        color.a = 0f;
        
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(timeToVanish * .75f);
        sequence.Append(text.DOColor(color, timeToVanish * .25f).SetEase(Ease.InExpo));
        sequence.AppendCallback(() => ObjectPoolManager.ReturnObjectToPool(gameObject));
        
        sequence.Play();
    }
}
