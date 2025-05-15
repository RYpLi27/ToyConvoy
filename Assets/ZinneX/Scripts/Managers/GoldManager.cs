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

    private void Start() {
        UpdateGoldAmount(startGold);
    }

    [SerializeField] [ReadOnly] private int currentGold;
    [SerializeField] private int startGold;

    [SerializeField] private TMP_Text currentGoldText;
    [SerializeField] private GameObject goldPopupPrefab;
    [SerializeField] private Transform goldPopupHolder;


    private void UpdateUI() { currentGoldText.text = currentGold.ToString(); }

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

    public void CollectedGoldPopup(int amount) {
        TMP_Text popupText = ObjectPoolManager.SpawnObject(goldPopupPrefab, goldPopupHolder).GetComponent<TMP_Text>();
        popupText.text = $"+ {amount} <sprite name=\"gold\">";
    }
    
    public bool PriceCheck(int amount) {
        return !(amount > currentGold);
    }
}
