using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/Stats/Enemy Stats")]
public class EnemyStatsSO : StatsSO
{
    public float moveSpeed;
    public int damageToBase;
    [SerializeField] [HorizontalGroup("Gold")] public int minGold;
    [SerializeField] [HorizontalGroup("Gold")] public int maxGold;
}
