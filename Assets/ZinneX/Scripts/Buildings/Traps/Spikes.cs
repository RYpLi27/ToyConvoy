using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spikes : MonoBehaviour, IBuilding, IInteractable {
    [SerializeField] private SpikesSO spikesSO;
    public int Price => spikesSO.price;
    
    [SerializeField] private Collider triggerCol;
    [SerializeField] private Collider normalCol;
    [SerializeField] private TurretPrompt upgradePrompt;

    private int currentLevel;
    
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
                
                enemy.TakeDamage(spikesSO.turretStats[currentLevel].damagePerSecond / spikesSO.hitInstancesPerSecond, enemy.transform.position + Vector3.up);
                enemiesInRange[enemy] = Time.time;
            }
        }
    }
    
    public void EnemyDeactivated(HealthManager enemy) { // CALLED WHEN ENEMY DIES OR REACHES LAST NODE
        if (enemiesInRange.ContainsKey(enemy)) {
            enemiesInRange.Remove(enemy);
        }
    }

    // public int GetPrice() {
    //     return spikesSO.price;
    // }
    
    public bool EnableBuilding() {
        if (GetComponent<PlacementCheck>().canPlace == false || GoldManager.instance.PriceCheck(spikesSO.price, true) == false) return false;
        
        GetComponent<PlacementCheck>().enabled = false;
        triggerCol.enabled = true;
        normalCol.enabled = true;
        
        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentInChildren<MeshRenderer>().material = spikesSO.objectMaterial;
        return true;
    }
    
    public bool PriceCheck() {
        return GoldManager.instance.PriceCheck(spikesSO.price, false);
    }
    
    public string GetDescription() {
        return $"{spikesSO.buildingName}\n\n" +
               $"Damage per second: {spikesSO.turretStats[0].damagePerSecond}\n" +
               $"Cost: {spikesSO.price}\n\n" +
               $"{spikesSO.description}";
    }
    
    public void ShowPrompt(bool value) {
        upgradePrompt.UpdatePromptUI(spikesSO.turretStats[currentLevel].upgradePrice, currentLevel);
        upgradePrompt.gameObject.SetActive(value);
    }
    
    public void Interact() {
        if (spikesSO.turretStats.Count - 1 == currentLevel || GoldManager.instance.PriceCheck(spikesSO.turretStats[currentLevel].upgradePrice, true) == false) return;

        currentLevel++;
        upgradePrompt.UpdatePromptUI(spikesSO.turretStats[currentLevel].upgradePrice, currentLevel);
    }
}
