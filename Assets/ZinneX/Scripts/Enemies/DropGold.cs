using Sirenix.OdinInspector;
using UnityEngine;

public class DropGold : MonoBehaviour {
    [SerializeField] private EnemyStatsSO stats;

    public void AddGold() {
        int amount = Random.Range(stats.minGold, stats.maxGold);
        GoldManager.instance.UpdateGoldAmount(amount);
        GoldManager.instance.AddPopupToQueue(amount);
    }
}
