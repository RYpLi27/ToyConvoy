using Sirenix.OdinInspector;
using UnityEngine;

public class PlacementCheck : MonoBehaviour {
    [ReadOnly] public bool canPlace;

    [SerializeField] private MeshRenderer mesh;
    private BoxCollider col;

    private Vector3 colPos;
    private Vector3 colSize;
    
    private void Start() {
        col = GetCollider();
    }

    private BoxCollider GetCollider() {
        BoxCollider[] cols = GetComponents<BoxCollider>();

        foreach (BoxCollider boxCol in cols) {
            if (boxCol.isTrigger) continue;

            return boxCol;
        }

        return null;
    }
    
    private void FixedUpdate() {
        MaterialChange();
    }

    private void MaterialChange() {
        colPos = transform.TransformPoint(col.center) + new Vector3(0f, .05f, 0f);
        colSize = Vector3.Scale(col.size, transform.lossyScale);
        
        switch (transform.tag) {
            case "Turret":
                if (PlaceOnTerrain()) { return; }
                break;
            
            case "Explosive":
            case "TrapDOT":
                if (PlaceOnRoad()) { return; }
                break;
        }
        
        mesh.material = PlayerBuilding.instance.wrongMaterial;
        canPlace = false;
    }

    private bool PlaceOnTerrain() {
        if (Physics.CheckBox(colPos, colSize / 2, transform.rotation, ~0, QueryTriggerInteraction.Ignore) == false) {
            if (GetComponent<TurretBehaviour>().PriceCheck()) {
                mesh.material = PlayerBuilding.instance.correctMaterial;
                canPlace = true;
                return true;
            }
        }

        return false;
    }

    private bool PlaceOnRoad() {
        if (Physics.CheckBox(colPos, colSize / 2, transform.rotation, ~0, QueryTriggerInteraction.Ignore) == true) {
            if(TryGetComponent(out Spikes spikes)) {
                if (spikes.PriceCheck() == false) return false;
                mesh.material = PlayerBuilding.instance.correctMaterial;
                canPlace = true;
                return true;
            } 
            
            if (TryGetComponent(out Landmine landmine)) {
                if (landmine.PriceCheck() == false) return false;
                mesh.material = PlayerBuilding.instance.correctMaterial;
                canPlace = true;
                return true;
            }
        }

        return false;
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        Gizmos.matrix = Matrix4x4.TRS(colPos, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, colSize);
    }
}
