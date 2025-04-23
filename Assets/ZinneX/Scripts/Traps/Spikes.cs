using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    [SerializeField] private SpikesSO spikesSO;
    private Dictionary<HealthManager, float> enemiesInRange = new(); //TARGET, LAST HIT TIME
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Add(other.GetComponent<HealthManager>(), 0);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            if(enemiesInRange.ContainsKey(other.GetComponent<HealthManager>())) enemiesInRange.Remove(other.GetComponent<HealthManager>());
        }
    }

    private void Update() {
        if(enemiesInRange.Count > 0) {
            List<HealthManager> enemies = new(enemiesInRange.Keys);
            
            foreach(HealthManager enemy in enemies) {
                if (enemy.isAlive == false) {
                    enemiesInRange.Remove(enemy);
                    continue;
                }
                
                if (Time.time - enemiesInRange[enemy] < 1f / spikesSO.hitInstancesPerSecond) continue;
                
                enemy.TakeDamage(spikesSO.damagePerSecond / spikesSO.hitInstancesPerSecond, true);
                enemiesInRange[enemy] = Time.time;
            }
        }
    }
    
    public void EnemyDeactivated(HealthManager enemy) { // CALLED WHEN ENEMY DIES OR REACHES LAST NODE
        if (enemiesInRange.ContainsKey(enemy)) {
            enemiesInRange.Remove(enemy);
        }
    }
}
