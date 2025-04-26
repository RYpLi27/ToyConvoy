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
        colPos = transform.TransformPoint(col.center) + new Vector3(0f, .05f, 0f);
        colSize = Vector3.Scale(col.size, transform.lossyScale);
        
        if (Physics.CheckBox(colPos, colSize/2, transform.rotation, ~0, QueryTriggerInteraction.Ignore)) {
            mesh.material = PlayerBuilding.instance.wrongMaterial;
            canPlace = false;
        } else {
            mesh.material = PlayerBuilding.instance.correctMaterial;
            canPlace = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        Gizmos.matrix = Matrix4x4.TRS(colPos, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, colSize);
    }
}
