using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoldManager : MonoBehaviour {
    public static GoldManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private List<int> queuedUpGoldPopups = new();
    private int playedPopups;
    

    [SerializeField] [ReadOnly] private int currentGold;
    [SerializeField] private int startGold;

    [SerializeField] private TMP_Text currentGoldText;
    [SerializeField] private GameObject goldPopupPrefab;
    [SerializeField] private Transform goldPopupHolder;

    private void Start() {
        UpdateGoldAmount(startGold);
    }

    private void UpdateUI() { currentGoldText.text = currentGold.ToString(); }

    public void UpdateGoldAmount(int amount) {
        currentGold = Mathf.Clamp(currentGold + amount, 0, 9999);
        UpdateUI();
    }

    /// <summary>
    /// Returns true when you can afford the purchase
    /// </summary>
    public bool PriceCheck(int amount, bool useGold) {
        if (useGold == false) return !(amount > currentGold);
        
        if (amount > currentGold) return false;
        
        UpdateGoldAmount(-amount);
        
        return true;
    }

    public void AddPopupToQueue(int amount) {
        queuedUpGoldPopups.Add(amount);
        PopupSpawn();
    }

    private void PopupSpawn() {
        if (playedPopups >= 4 || queuedUpGoldPopups.Count == 0) return;
        
        playedPopups++;
        TMP_Text popupText = ObjectPoolManager.SpawnObject(goldPopupPrefab, goldPopupHolder).GetComponent<TMP_Text>();
        popupText.text = $"+ {queuedUpGoldPopups[0]} <sprite name=\"gold\">";
        queuedUpGoldPopups.RemoveAt(0);
    }

    public void PopupFinish() {
        playedPopups--;
        PopupSpawn();
    }
    
#if UNITY_EDITOR
    public void AddGoldTesting(InputAction.CallbackContext context) {
        if(context.performed) UpdateGoldAmount(5000);
    }
#endif
}
