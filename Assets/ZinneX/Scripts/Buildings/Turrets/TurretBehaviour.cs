using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour, IBuilding {
    [SerializeField] [InfoBox("To see values click on pen at the right side")] private TurretSO turretSO;
    
    [SerializeField] private Transform firepoint;
    private List<Transform> enemiesInRange = new();
    private float lastShootTime;
    [HideInInspector] public Transform currentTarget;

    [SerializeField] private CapsuleCollider triggerCol;
    [SerializeField] private Collider normalCol;
    [SerializeField] private GameObject rangeBorder;
    
    private void Start() {
        triggerCol.radius = turretSO.range;

        rangeBorder.GetComponent<SphereCollider>().radius = 1;
        rangeBorder.transform.localScale = Vector3.one * turretSO.range * 2;
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

        if (Time.time - lastShootTime > 1/turretSO.fireRate) { // FIRERATE LOGIC
            lastShootTime = Time.time;
            turretSO.Shoot(firepoint, currentTarget);
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
        if (GetComponent<PlacementCheck>().canPlace == false || GoldManager.instance.BuyTurret(turretSO.price) == false) return false;

        GetComponent<PlacementCheck>().enabled = false;
        if (TryGetComponent(out TurretRotateToTarget rotate)) rotate.enabled = true;
        triggerCol.enabled = true;
        normalCol.enabled = true;
        rangeBorder.SetActive(false);
        
        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentInChildren<MeshRenderer>().material = turretSO.objectMaterial;

        return true;
    }

    public bool PriceCheck() {
        return GoldManager.instance.PriceCheck(turretSO.price);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        if(turretSO != null)
            Gizmos.DrawWireSphere(transform.position, turretSO.range);
    }

    public string GetDescription() {
        return $"{turretSO.buildingName}\n\n" +
               $"Damage: {turretSO.damage}\n" +
               $"Attacks per second: {turretSO.fireRate}\n" +
               $"Range: {turretSO.range}\n" +
               $"Cost: {turretSO.price}\n\n" +
               $"{turretSO.description}";
    }
}
