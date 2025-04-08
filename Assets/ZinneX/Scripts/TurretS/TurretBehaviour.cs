using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour {
    [SerializeField] [InfoBox("To see values click on pen at the right side")] private TurretSO turretSO;
    [SerializeField] private Transform firepoint;
    private List<Transform> enemiesInRange = new();
    private float lastShootTime;
    [HideInInspector] public Transform currentTarget;

    private void Start() {
        GetComponent<CapsuleCollider>().radius = turretSO.range;
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
        if (enemiesInRange.Count == 0) return; // IF NO ENEMIES IN RANGE DON'T SHOOT

        if (Time.time - lastShootTime > turretSO.fireRate) { // FIRERATE LOGIC
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

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        if(turretSO != null)
            Gizmos.DrawWireSphere(transform.position, turretSO.range);
    }
}
