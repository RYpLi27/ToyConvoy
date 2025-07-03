using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClickToStart : MonoBehaviour {
    [SerializeField] private RectTransform title;
    [SerializeField] private Image buttonsCover;

    private bool pressed;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.anyKey.wasPressedThisFrame && pressed == false) {
            pressed = true;
            
            Sequence sequence = DOTween.Sequence();

            sequence.Append(gameObject.GetComponent<TextMeshProUGUI>().DOFade(0, .45f)).SetEase(Ease.InOutSine);
            sequence.Join(title.DOLocalMoveY(200, .45f)).SetEase(Ease.InOutSine);
            
            sequence.AppendCallback(() => gameObject.SetActive(false));
            
            
            sequence.Append(buttonsCover.DOFade(0f, .4f)).SetEase(Ease.InOutSine);

            sequence.OnComplete(() => buttonsCover.gameObject.SetActive(false));
        }
    }
}
