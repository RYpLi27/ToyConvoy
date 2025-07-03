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
        sequence.Append(text.GetComponent<RectTransform>().DOSizeDelta(new Vector2(190f, 75f), .4f)).SetEase(Ease.OutSine);
        sequence.AppendInterval(timeToVanish * .75f);
        sequence.Append(text.DOColor(color, timeToVanish * .25f).SetEase(Ease.InExpo));
        sequence.AppendCallback(() => {
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(470f, 75f);
            gameObject.transform.SetParent(null);
            GoldManager.instance.PopupFinish();
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        });
        
        sequence.Play();
    }
}
