using Sirenix.OdinInspector;
using UnityEngine;

public class DropGold : MonoBehaviour {
    [SerializeField] [HorizontalGroup("Gold")] private int minGold;
    [SerializeField] [HorizontalGroup("Gold")] private int maxGold;

    public void AddGold() {
        int amount = Random.Range(minGold, maxGold);
        GoldManager.instance.UpdateGoldAmount(amount);
        GoldManager.instance.AddPopupToQueue(amount);
    }
}
