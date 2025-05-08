using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    [SerializeField] [ReadOnly] private float currentHealth;
    [SerializeField] private StatsSO statsSO;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Bar hpBar;
    [ReadOnly] public bool isAlive;
    
    private void OnEnable() {
        currentHealth = statsSO.maxHealth;
        UpdateUI();
        isAlive = true;
    }
    
    public void OnDisable() {
        if (gameObject.CompareTag("Enemy") == false) return; // REST OF THIS METHOD IS ONLY FOR ENEMIES
        
        WaveManager.enemyCount--;
        
        //THOSE LOOPS LETS TURRETS AND TRAPS KNOW THAT IT IS DEAD
        foreach (TurretBehaviour turret in FindObjectsByType<TurretBehaviour>(FindObjectsSortMode.None)) {
            turret.EnemyDeactivated(transform);
        }

        foreach (Spikes spikes in FindObjectsByType<Spikes>(FindObjectsSortMode.None)) {
            spikes.EnemyDeactivated(this);
        }
    }

    public float TakeDamage(float damage, Vector3 damageTextPosition) {
        damage = Mathf.RoundToInt(Mathf.Max(damage - statsSO.defense, 1));
        
        currentHealth -= damage;
        UpdateUI();
        CreateDamageText(damageTextPosition, damage);
        if (currentHealth <= 0) {
            Death();
        }

        return damage;
    }

    private void CreateDamageText(Vector3 position, float value) {
        TMP_Text dmgText = ObjectPoolManager.SpawnObject(damageText, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Effect).GetComponentInChildren<TMP_Text>();
        dmgText.text = value.ToString();
    }
    
    private void Death() {
        if (isAlive == false) return;
        isAlive = false;
        
        if(transform.CompareTag("Enemy")) GetComponent<DropGold>().AddGold();
        else if(transform.CompareTag("Player")) GameManager.instance.EndGame(GameManager.GameState.Lose);
            
        ObjectPoolManager.ReturnObjectToPool(gameObject); // LATER ADD DEATH ANIM
    }
    
    private void UpdateUI() {
        hpBar.UpdateUI(currentHealth, statsSO.maxHealth);
    }

    public void ShowBar(bool show) {
        hpBar.gameObject.SetActive(show);
    }
}