using Sirenix.OdinInspector;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    [SerializeField] [ReadOnly] private float health;
    [SerializeField] private StatsSO statsSO;
    [ReadOnly] public bool isAlive;

    private void OnEnable() {
        health = statsSO.maxHealth;
        isAlive = true;
    }
    
    public void OnDisable() {
        isAlive = false;

        if (gameObject.CompareTag("Enemy") == false) return; // REST OF THIS METHOD IS ONLY FOR ENEMIES
        
        EnemySpawner.enemyCount--;
        
        //THOSE LOOPS LETS TURRETS AND TRAPS KNOW THAT IT IS DEAD
        foreach (TurretBehaviour turret in FindObjectsByType<TurretBehaviour>(FindObjectsSortMode.None)) {
            turret.EnemyDeactivated(transform);
        }

        foreach (Spikes spikes in FindObjectsByType<Spikes>(FindObjectsSortMode.None)) {
            spikes.EnemyDeactivated(this);
        }
    }

    public void TakeDamage(float damage) {
        damage = Mathf.Max(damage - statsSO.defense, 1);
        
        health -= damage;
        if (health <= 0) {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
