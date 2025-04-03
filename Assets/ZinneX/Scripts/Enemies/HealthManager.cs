using UnityEngine;

public class HealthManager : MonoBehaviour {
    private float health;
    [SerializeField] private EnemySO enemySO;

    private void Start() {
        health = enemySO.maxHealth;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            gameObject.SetActive(false);
        }
    }
}
