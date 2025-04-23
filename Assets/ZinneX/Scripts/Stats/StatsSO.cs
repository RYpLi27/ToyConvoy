using UnityEngine;

[CreateAssetMenu(fileName = "StatsSO", menuName = "Scriptable Objects/Enemy Stats")]
public class StatsSO : ScriptableObject {
    public float maxHealth;
    public float defense;
    public float moveSpeed;
}
