using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private float stamina;
    [SerializeField] private float staminaRecoverAmount;
    [SerializeField] private float staminaRecoveryTime;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private Image fillArea;
    [SerializeField] private Color exhaustedBarColor;
    [SerializeField] private Color normalBarColor;
    private float currentStamina, lastStaminaUse;
    [ReadOnly] public bool isExhausted;

    private void Start() {
        currentStamina = stamina;
    }

    private void Update()
    {
        if (GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        HandleStamina();
    }
    
    private void HandleStamina() {
        if (Time.time - lastStaminaUse > staminaRecoveryTime) {
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime * staminaRecoverAmount, stamina);
            UpdateUI();
        }

        if (currentStamina == 0) isExhausted = true;
        
        if (currentStamina == stamina) isExhausted = false;
    }

    public void DrainStamina(float staminaCost) {
        currentStamina = Mathf.Max(currentStamina - staminaCost, 0);
        lastStaminaUse = Time.time;
        
        UpdateUI();
    }

    private void UpdateUI() {
        fillArea.color = isExhausted == true ? exhaustedBarColor : normalBarColor;
        
        staminaBar.value = currentStamina;
    }
}
