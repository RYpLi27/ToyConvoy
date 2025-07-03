using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    [SerializeField] [ReadOnly] private float currentHealth;
    [SerializeField] private StatsSO statsSO;
    [SerializeField] private Bar hpBar;
    [SerializeField] private bool isEnemy;
    [SerializeField] [ShowIf("isEnemy")] private GameObject damageText;
    [SerializeField] [ShowIf("isEnemy")] private GameObject positionTracker;
    [ReadOnly] public bool isAlive;
    
    private void OnEnable() {
        if(isEnemy == true) SetComponents(true);
        currentHealth = statsSO.maxHealth;
        UpdateUI();
        isAlive = true;
    }

    public float TakeDamage(float damage, Vector3 damageTextPosition) {
        damage = Mathf.RoundToInt(Mathf.Max(damage - statsSO.defense, 1));
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateUI();
        CreateDamageText(damageTextPosition, damage);
        if (currentHealth <= 0) {
            Death();
        }

        return damage;
    }

    // USED TO DAMAGE BASE | WITHOUT DMG TEXT
    public void TakeDamage(float damage) {
        damage = Mathf.RoundToInt(Mathf.Max(damage - statsSO.defense, 1));
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateUI();
        if (currentHealth <= 0) {
            Death();
        }
    }

    private void CreateDamageText(Vector3 position, float value) {
        TMP_Text dmgText = ObjectPoolManager.SpawnObject(damageText, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Effect).GetComponentInChildren<TMP_Text>();
        dmgText.text = value.ToString();
    }
    
    private void Death() {
        if (isAlive == false) return;
        isAlive = false;
        
        if (isEnemy == true) {
            SetComponents(false);
            GetComponent<DeathAnim>().PlayAnim();
            GetComponent<DropGold>().AddGold();
            WaveManager.instance.EnemyCount--;
            hpBar.gameObject.SetActive(false);

            //THOSE LOOPS LETS TURRETS AND TRAPS KNOW THAT IT IS DEAD
            foreach (TurretBehaviour turret in FindObjectsByType<TurretBehaviour>(FindObjectsSortMode.None)) {
                List<Collider> cols = GetComponentsInChildren<Collider>().ToList();
                cols.ForEach(c => turret.EnemyDeactivated(c.transform));
            }

            foreach (Spikes spikes in FindObjectsByType<Spikes>(FindObjectsSortMode.None)) { spikes.EnemyDeactivated(this); }
        } else {
            StartCoroutine(GameManager.instance.EndGame(GameManager.GameState.Lose));
        }
    }

    private void SetComponents(bool value) {
        GetComponent<EnemyBehaviour>().canMove = value;
        GetComponentsInChildren<Collider>().ToList().ForEach(col => col.enabled = value);
        positionTracker.SetActive(value);
    }
    
    private void UpdateUI() {
        hpBar.UpdateUI(currentHealth, statsSO.maxHealth);
    }

    public void ShowBar(bool show) {
        hpBar.gameObject.SetActive(show);
    }
}