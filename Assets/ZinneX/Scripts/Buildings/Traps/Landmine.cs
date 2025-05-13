using UnityEngine;

public class Landmine : MonoBehaviour, IBuilding {
    [SerializeField] private LandmineSO landmineSO;
    public int Price => landmineSO.price;
    
    [SerializeField] private Collider triggerCol;
    [SerializeField] private Collider normalCol;
    [SerializeField] private GameObject rangeBorder;

    private void Start() {
        rangeBorder.GetComponent<SphereCollider>().radius = 1;
        rangeBorder.transform.localScale = Vector3.one * landmineSO.explosionRange * 2;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Explode();
        }
    }
    
    private void Explode() {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, landmineSO.explosionRange, StaticVariables.whatIsEnemy);

        foreach (Collider col in enemiesInRange) {
            col.GetComponent<HealthManager>().TakeDamage(landmineSO.damage, col.transform.position + Vector3.up);
        }
        
        ObjectPoolManager.ReturnObjectToPool(gameObject);

        triggerCol.enabled = false;
        normalCol.enabled = false;
        GetComponent<PlacementCheck>().enabled = true;
    }
    
    // public int GetPrice() {
    //     return landmineSO.price;
    // }
    
    public bool EnableBuilding() {
        if (GetComponent<PlacementCheck>().canPlace == false || GoldManager.instance.BuyTurret(landmineSO.price) == false) return false;

        GetComponent<PlacementCheck>().enabled = false;
        GetComponent<Collider>().enabled = true;
        triggerCol.enabled = true;
        normalCol.enabled = true;
        rangeBorder.SetActive(false);
        
        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentInChildren<MeshRenderer>().material = landmineSO.objectMaterial;
        return true;
    }

    public bool PriceCheck() {
        return GoldManager.instance.PriceCheck(landmineSO.price);
    }
    
    public string GetDescription() {
        return $"{landmineSO.buildingName}\n\n" +
               $"Damage: {landmineSO.damage}\n" +
               $"Range: {landmineSO.explosionRange}\n" +
               $"Cost: {landmineSO.price}\n\n" +
               $"{landmineSO.description}";
    }
    
    private void OnDrawGizmos() {
        if (landmineSO == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, landmineSO.explosionRange);
    }
}
