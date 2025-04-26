using UnityEngine;

public class Landmine : MonoBehaviour {
    [SerializeField] private LandmineSO landmineSO;
    [SerializeField] private Collider triggerCol;
    [SerializeField] private Collider normalCol;

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

        triggerCol.enabled = false;
        normalCol.enabled = false;
        GetComponent<PlacementCheck>().enabled = true;
    }
    
    public bool EnableLandmine() {
        if (GetComponent<PlacementCheck>().canPlace == false) return false;

        GetComponent<PlacementCheck>().enabled = false;
        GetComponent<Collider>().enabled = true;
        triggerCol.enabled = true;
        normalCol.enabled = true;
        
        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentInChildren<MeshRenderer>().material = landmineSO.objectMaterial;
        return true;
    }

    private void OnDrawGizmos() {
        if (landmineSO == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, landmineSO.explosionRange);
    }
}
