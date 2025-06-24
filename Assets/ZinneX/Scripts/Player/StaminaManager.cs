using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private float stamina;
    [SerializeField] private float staminaRecoverAmount;
    [SerializeField] private float staminaRecoveryTime;
    [SerializeField] private Bar staminaBar;
    [SerializeField] private Image fillArea;
    [SerializeField] private Material exhaustedBarColor;
    [SerializeField] private Material normalBarColor;
    private float currentStamina, lastStaminaUse;
    [ReadOnly] public bool isExhausted;

    private bool trailCatchUp;
    
    private void Start() {
        currentStamina = stamina;
        fillArea.material = normalBarColor;
    }

    private void Update()
    {
        if (GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        HandleStamina();
    }
    
    private void HandleStamina() {
        if (Time.time - lastStaminaUse > staminaRecoveryTime / 4 && trailCatchUp) {
            UpdateUI(false);
            trailCatchUp = false;
        } else if (Time.time - lastStaminaUse > staminaRecoveryTime) {
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime * staminaRecoverAmount, stamina);
            
            UpdateUI(false, true);
        }

        if (currentStamina == 0) {
            isExhausted = true;
        }
        
        if (currentStamina == stamina) isExhausted = false;
    }

    public bool DrainStamina(float staminaCost) {
        currentStamina = Mathf.Max(currentStamina - staminaCost, 0);
        lastStaminaUse = Time.time;

        trailCatchUp = true;
        
        UpdateUI(true);
        return currentStamina == 0;
    }

    private void UpdateUI(bool instant, bool withTrail = false) {
        fillArea.material = isExhausted == true ? exhaustedBarColor : normalBarColor;

        switch (instant, withTrail) {
            case (true, false):
                staminaBar.UpdateUIInstant(currentStamina, stamina);
                break;
            
            case (false, true):
                staminaBar.UpdateUIInstantWithTrail(currentStamina, stamina);
                break;
            
            case (false, false):
                staminaBar.UpdateUI(currentStamina, stamina);
                break;
        }
    }
}
