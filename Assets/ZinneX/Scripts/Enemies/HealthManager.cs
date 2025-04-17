using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    [SerializeField] [ReadOnly] private float health;
    [SerializeField] private EnemySO enemySO;

    private void Start() {
        health = enemySO.maxHealth;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Death();
        }

    }
    
    public void Death() {
        foreach (TurretBehaviour turret in FindObjectsByType<TurretBehaviour>(FindObjectsSortMode.None)) {
            turret.EnemyDeactivated(transform);
        }

        foreach (Spikes spikes in FindObjectsByType<Spikes>(FindObjectsSortMode.None)) {
            spikes.EnemyDeactivated(GetComponent<Collider>());
        }
        
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    
}
