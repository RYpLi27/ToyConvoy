using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    [SerializeField] private SpikesSO spikesSO;
    private Dictionary<Collider, float> enemiesInRange = new(); //TARGET, LAST HIT TIME
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Add(other, 0);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            if(enemiesInRange.ContainsKey(other)) enemiesInRange.Remove(other);
        }
    }

    private void Update() {
        if(enemiesInRange.Count > 0) {
            List<Collider> enemies = new(enemiesInRange.Keys);
            
            foreach(Collider enemy in enemies) {
                if (enemy == null) {
                    enemiesInRange.Remove(enemy);
                    continue;
                }
                
                if (Time.time - enemiesInRange[enemy] < 1f / spikesSO.hitInstancesPerSecond) continue;
                
                enemy.GetComponent<HealthManager>().TakeDamage(spikesSO.damagePerSecond / spikesSO.hitInstancesPerSecond);
                enemiesInRange[enemy] = Time.time;
            }
        }
    }
    
    public void EnemyDeactivated(Collider col) { // CALLED WHEN ENEMY DIES OR REACHES LAST NODE
        if (enemiesInRange.ContainsKey(col)) enemiesInRange.Remove(col);
    }
}
