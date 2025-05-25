using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIButtonHold : MonoBehaviour {
    [SerializeField] private Button targetButton;
    [SerializeField] private Image fillImage;
    [SerializeField] private float holdDuration;
    
    private float holdTimer;
    private bool isHolding;
    
    private void Update() {
        if (isHolding == false) return;

        holdTimer += Time.deltaTime;
        fillImage.fillAmount = holdTimer / holdDuration;

        if (holdTimer >= holdDuration) {
            targetButton.onClick.Invoke();
            ResetButton();
        }
    }

    public void HoldButton(InputAction.CallbackContext context) {
        if (context.started) isHolding = true;

        // if (context.performed) {
        //     ResetButton();
        //     targetButton.onClick.Invoke();
        // }
        
        if (context.canceled) ResetButton();
    }

    private void ResetButton() {
        isHolding = false;
        fillImage.fillAmount = 0;
        holdTimer = 0;
    }
}
