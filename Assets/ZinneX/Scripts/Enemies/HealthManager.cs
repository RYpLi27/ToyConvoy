using Sirenix.OdinInspector;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    [SerializeField] [ReadOnly] private float health;
    [SerializeField] private EnemySO enemySO;
    [ReadOnly] public bool isAlive;

    private void OnEnable() {
        health = enemySO.maxHealth;
        isAlive = true;
    }
    
    public void OnDisable() {
        isAlive = false;
        EnemySpawner.enemyCount--;
        
        foreach (TurretBehaviour turret in FindObjectsByType<TurretBehaviour>(FindObjectsSortMode.None)) {
            turret.EnemyDeactivated(transform);
        }

        foreach (Spikes spikes in FindObjectsByType<Spikes>(FindObjectsSortMode.None)) {
            spikes.EnemyDeactivated(this);
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
