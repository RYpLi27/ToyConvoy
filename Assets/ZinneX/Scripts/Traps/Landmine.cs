using UnityEngine;

public class Landmine : MonoBehaviour {
    [SerializeField] private LandmineSO landmineSO;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Explode();
        }
    }

    private void Explode() {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, landmineSO.explosionRange, StaticVariables.whatIsEnemy);

        foreach (Collider col in enemiesInRange) {
            col.GetComponent<HealthManager>().TakeDamage(landmineSO.damage);
        }
        
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void OnDrawGizmos() {
        if (landmineSO == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, landmineSO.explosionRange);
    }
}
