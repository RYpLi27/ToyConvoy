using UnityEngine;

public class Landmine : MonoBehaviour, IBuilding, IInteractable {
    [SerializeField] private LandmineSO landmineSO;
    public int Price => landmineSO.price;
    
    [SerializeField] private Collider triggerCol;
    [SerializeField] private Collider normalCol;
    [SerializeField] private GameObject rangeBorder;
    [SerializeField] private TurretPrompt upgradePrompt;
    [SerializeField] private MeshRenderer objectModel;
    private int currentLevel;
    
    private void Start() {
        rangeBorder.GetComponent<SphereCollider>().radius = 1;
        rangeBorder.transform.localScale = Vector3.one * landmineSO.turretStats[0].explosionRange * 2;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Explode();
        }
    }
    
    private void Explode() {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, landmineSO.turretStats[0].explosionRange, StaticVariables.whatIsEnemy);

        foreach (Collider col in enemiesInRange) {
            col.GetComponent<HealthManager>().TakeDamage(landmineSO.turretStats[0].damage, col.transform.position + Vector3.up);
        }
        
        // ObjectPoolManager.ReturnObjectToPool(gameObject);
        
        // IS ONLY DISABLED FOR ONE WAVE
        objectModel.material = landmineSO.usedMat;
        triggerCol.enabled = false;
        normalCol.enabled = false;
        // GetComponent<PlacementCheck>().enabled = true;
    }

    public void RestoreLandmine() {
        objectModel.material = landmineSO.objectMaterial;
        triggerCol.enabled = true;
        normalCol.enabled = true;
    }
    
    // public int GetPrice() {
    //     return landmineSO.price;
    // }
    
    public bool EnableBuilding() {
        if (GetComponent<PlacementCheck>().canPlace == false || GoldManager.instance.PriceCheck(landmineSO.price, true) == false) return false;

        GetComponent<PlacementCheck>().enabled = false;
        GetComponent<Collider>().enabled = true;
        triggerCol.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Turret");
        normalCol.enabled = true;
        rangeBorder.SetActive(false);
        
        transform.SetParent(GameObject.Find("Turrets").transform);
        GetComponentInChildren<MeshRenderer>().material = landmineSO.objectMaterial;
        return true;
    }

    public bool PriceCheck() {
        return GoldManager.instance.PriceCheck(landmineSO.price, false);
    }
    
    public string GetDescription() {
        return $"{landmineSO.buildingName}\n\n" +
               $"Damage: {landmineSO.turretStats[0].damage}\n" +
               $"Range: {landmineSO.turretStats[0].explosionRange}\n" +
               $"Cost: {landmineSO.price}\n\n" +
               $"{landmineSO.description}";
    }

    public void ShowPrompt(bool value) {
        upgradePrompt.UpdatePromptUI(landmineSO.turretStats[currentLevel].upgradePrice, currentLevel);
        upgradePrompt.gameObject.SetActive(value);
    }

    public void Interact() {
        if (landmineSO.turretStats.Count - 1 == currentLevel || GoldManager.instance.PriceCheck(landmineSO.turretStats[currentLevel].upgradePrice, true) == false) return;

        currentLevel++;
        upgradePrompt.UpdatePromptUI(landmineSO.turretStats[currentLevel].upgradePrice, currentLevel);
    }
    
    private void OnDrawGizmos() {
        if (landmineSO == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, landmineSO.turretStats[currentLevel].explosionRange);
    }
}
