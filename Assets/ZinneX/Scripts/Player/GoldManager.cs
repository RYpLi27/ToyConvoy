using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour {
    public static GoldManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        UpdateUI();
    }

    [SerializeField] [ReadOnly] private int currentGold;

    [SerializeField] private TMP_Text goldText;

    private void UpdateUI() { goldText.text = $"Gold: {currentGold}"; }

    public void UpdateGoldAmount(int amount) {
        currentGold = Mathf.Clamp(currentGold + amount, 0, 9999);
        UpdateUI();
    }

    public bool BuyTurret(int amount) {
        if (PriceCheck(amount) == false) return false;
        
        UpdateGoldAmount(-amount);
        UpdateUI();
        
        return true;
    }

    public bool PriceCheck(int amount) {
        return !(amount > currentGold);
    }
}
