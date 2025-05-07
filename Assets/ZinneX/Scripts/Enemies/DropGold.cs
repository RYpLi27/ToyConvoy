using Sirenix.OdinInspector;
using UnityEngine;

public class DropGold : MonoBehaviour {
    [SerializeField] [HorizontalGroup("Gold")] private int minGold;
    [SerializeField] [HorizontalGroup("Gold")] private int maxGold;

    public void AddGold() {
        GoldManager.instance.UpdateGoldAmount(Random.Range(minGold, maxGold));
    }
}
