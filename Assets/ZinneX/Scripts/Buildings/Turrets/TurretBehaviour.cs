using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour, IBuilding, IInteractable {
    [SerializeField] [InfoBox("To see values click on pen at the right side")] private TurretSO turretSO;
    
    [SerializeField] private Transform firepoint;
    private List<Transform> enemiesInRange = new();
    private float lastShootTime;
    [HideInInspector] public Transform currentTarget;

    [SerializeField] private CapsuleCollider triggerCol;
    [SerializeField] private Collider normalCol;
    [SerializeField] private GameObject rangeBorder;
    [SerializeField] private TurretPrompt upgradePrompt;
    [SerializeField] private InteractTrigger interactTrigger;

    private int currentLevel;
    
    private void Start() {
        triggerCol.radius = turretSO.turretStats[currentLevel].range;

        rangeBorder.GetComponent<SphereCollider>().radius = 1;
        rangeBorder.transform.localScale = Vector3.one * turretSO.turretStats[currentLevel].range * 2;
    }

    private void FixedUpdate() {
        Shoot();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Add(other.transform);
            UpdateTarget();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Remove(other.transform);
            UpdateTarget();
        }
    }

    private void Shoot() {
        if (enemiesInRange.Count == 0 || GameManager.gameState != GameManager.GameState.Ongoing) return; // IF NO ENEMIES IN RANGE DON'T SHOOT

        if (Time.time - lastShootTime > 1/turretSO.turretStats[currentLevel].fireRate) { // FIRERATE LOGIC
            lastShootTime = Time.time;
            turretSO.Shoot(firepoint, currentTarget, turretSO.turretStats[currentLevel]);
        }
    }

    private void UpdateTarget() {
        enemiesInRange.RemoveAll(e => e.gameObject.activeInHierarchy == false); // REMOVES ALL 'EMPTY' ENTRIES (DEAD ENEMIES ETC.)

        currentTarget = enemiesInRange.Count == 0 ? null : enemiesInRange[0]; // SET CURRENT TARGET TO FIRST ENEMY THAT ENTERED RANGE OR NO TARGET WHEN NO ENEMIES ARE IN RANGE
    }

    public void EnemyDeactivated(Transform obj) { // CALLED WHEN ENEMY DIES OR REACHES LAST NODE
        enemiesInRange.Remove(obj);
        UpdateTarget();
    }

    // public int GetPrice() {
    //     return turretSO.price;
    // }

    public bool EnableBuilding() {
        if (GetComponent<PlacementCheck>().canPlace == false || GoldManager.instance.PriceCheck(turretSO.price, true) == false) return false;

        GetComponent<PlacementCheck>().enabled = false;
        if (TryGetComponent(out TurretRotateToTarget rotate)) rotate.enabled = true;
        triggerCol.enabled = true;
        normalCol.enabled = true;
        rangeBorder.SetActive(false);
        interactTrigger.gameObject.SetActive(true);
        
        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentInChildren<MeshRenderer>().material = turretSO.objectMaterial;

        return true;
    }

    public bool PriceCheck() {
        return GoldManager.instance.PriceCheck(turretSO.price, false);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        if(turretSO != null)
            Gizmos.DrawWireSphere(transform.position, turretSO.turretStats[currentLevel].range);
    }

    public string GetDescription() {
        return $"{turretSO.buildingName}\n\n" +
               $"Damage: {turretSO.turretStats[0].damage}\n" +
               $"Attacks per second: {turretSO.turretStats[0].fireRate}\n" +
               $"Range: {turretSO.turretStats[0].range}\n" +
               $"Cost: {turretSO.price}\n\n" +
               $"{turretSO.description}";
    }
    public void ShowPrompt(bool value) {
        upgradePrompt.UpdatePromptUI(turretSO.turretStats[currentLevel].upgradePrice, currentLevel);
        upgradePrompt.gameObject.SetActive(value);
    }
    
    public void Interact() {
        if (turretSO.turretStats.Count - 1 == currentLevel || GoldManager.instance.PriceCheck(turretSO.turretStats[currentLevel].upgradePrice, true) == false) return;

        currentLevel++;
        upgradePrompt.UpdatePromptUI(turretSO.turretStats[currentLevel].upgradePrice, currentLevel);
    }
}
