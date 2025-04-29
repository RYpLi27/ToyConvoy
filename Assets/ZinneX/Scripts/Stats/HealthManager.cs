using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    [SerializeField] [ReadOnly] private float currentHealth;
    [SerializeField] private StatsSO statsSO;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;
    [ReadOnly] public bool isAlive;
    
    private void OnEnable() {
        currentHealth = statsSO.maxHealth;
        UpdateUI();
        isAlive = true;
    }
    
    public void OnDisable() {
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

    public float TakeDamage(float damage) {
        damage = (float)Math.Round(Mathf.Max(damage - statsSO.defense, 1), 2);
        
        currentHealth -= damage;
        UpdateUI();
        if (currentHealth <= 0) {
            Death();
        }

        return damage;
    }

    private void Death() {
        if (isAlive == false) return;
        isAlive = false;
        
        if(transform.CompareTag("Enemy")) GetComponent<DropGold>().AddGold();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    
    private void UpdateUI() {
        hpSlider.value = currentHealth / statsSO.maxHealth;
        hpText.text = $"{currentHealth}/{statsSO.maxHealth}";
    }
}