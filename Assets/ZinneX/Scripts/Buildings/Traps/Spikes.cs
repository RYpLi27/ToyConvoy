using System.Collections.Generic;
using System;
using Sirenix.Utilities;
using UnityEngine;

public class Spikes : MonoBehaviour, IBuilding, IInteractable {
    [SerializeField] private SpikesSO spikesSO;
    
    [SerializeField] private Collider triggerCol;
    [SerializeField] private Collider normalCol;
    [SerializeField] private TurretPrompt upgradePrompt;
    [SerializeField] private InteractTrigger interactTrigger;

    private int currentLevel;
    
    private Dictionary<HealthManager, float> enemiesInRange = new(); //TARGET, LAST HIT TIME
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && enemiesInRange.ContainsKey(other.GetComponentInParent<HealthManager>()) == false) {
            enemiesInRange.Add(other.GetComponentInParent<HealthManager>(), 0);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            if(enemiesInRange.ContainsKey(other.GetComponentInParent<HealthManager>())) enemiesInRange.Remove(other.GetComponentInParent<HealthManager>());
        }
    }

    private void Update() {
        if(enemiesInRange.Count > 0) {
            List<HealthManager> enemies = new(enemiesInRange.Keys);
            List<Transform> enemiesHit = new();
            
            foreach(HealthManager enemy in enemies) {
                if (enemy.isAlive == false) {
                    enemiesInRange.Remove(enemy);
                    continue;
                }
                
                if (Time.time - enemiesInRange[enemy] < 1f / spikesSO.hitInstancesPerSecond || enemiesHit.Contains(enemy.transform)) continue;
                
                enemiesHit.Add(enemy.transform);
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
        
        interactTrigger.gameObject.SetActive(true);
        
        gameObject.layer = LayerMask.NameToLayer("Turret");
        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentsInChildren<MeshRenderer>().ForEach(m => m.material = spikesSO.objectMaterial);
        
        return true;
    }
    
    public bool PriceCheck() => GoldManager.instance.PriceCheck(spikesSO.price, false);

    public string GetDescription() =>
        $"{spikesSO.buildingName}\n\n" +
        $"<sprite name=\"damage\"> {spikesSO.turretStats[0].damagePerSecond}\n" +
        $"<sprite name=\"gold\"> {spikesSO.price}\n\n" +
        $"{spikesSO.description}";

    public void ShowPrompt(bool value) {
        upgradePrompt.UpdatePromptUI(spikesSO.turretStats[currentLevel].upgradePrice, currentLevel, GetStatsText());
        upgradePrompt.gameObject.SetActive(value);
    }

    private string GetStatsText() => currentLevel < spikesSO.turretStats.Count - 1
        ? $"<sprite name=\"damage\"> {spikesSO.turretStats[currentLevel].damagePerSecond}<color=green>(+{Math.Round(spikesSO.turretStats[currentLevel + 1].damagePerSecond - spikesSO.turretStats[currentLevel].damagePerSecond, 2)})</color>\n"
        
        : $"<sprite name=\"damage\"> {spikesSO.turretStats[currentLevel].damagePerSecond}\n";
    
    public void Interact() {
        if (spikesSO.turretStats.Count - 1 == currentLevel || GoldManager.instance.PriceCheck(spikesSO.turretStats[currentLevel].upgradePrice, true) == false) return;

        currentLevel++;
        upgradePrompt.UpdatePromptUI(spikesSO.turretStats[currentLevel].upgradePrice, currentLevel, GetStatsText());
    }
}
