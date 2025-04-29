using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour, IBuilding {
    [SerializeField] private SpikesSO spikesSO;
    public int Price => spikesSO.price;
    
    [SerializeField] private Collider triggerCol;
    [SerializeField] private Collider normalCol;
    
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
                
                enemy.TakeDamage(spikesSO.damagePerSecond / spikesSO.hitInstancesPerSecond, enemy.transform.position + Vector3.up);
                enemiesInRange[enemy] = Time.time;
            }
        }
    }
    
    public void EnemyDeactivated(HealthManager enemy) { // CALLED WHEN ENEMY DIES OR REACHES LAST NODE
        if (enemiesInRange.ContainsKey(enemy)) {
            enemiesInRange.Remove(enemy);
        }
    }

    public int GetPrice() {
        return spikesSO.price;
    }
    
    public bool EnableBuilding() {
        if (GetComponent<PlacementCheck>().canPlace == false || GoldManager.instance.BuyTurret(spikesSO.price) == false) return false;
        
        GetComponent<PlacementCheck>().enabled = false;
        triggerCol.enabled = true;
        normalCol.enabled = true;

        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentInChildren<MeshRenderer>().material = spikesSO.objectMaterial;
        return true;
    }
    
    public bool PriceCheck() {
        return GoldManager.instance.PriceCheck(spikesSO.price);
    }
}
