using System.Linq;
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

        return cols.FirstOrDefault(boxCol => boxCol.isTrigger == false);
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
        if (col == null) return false;
    
        // POSITION OF ALL THE CORNERS
        Vector3 center = col.center;
        Vector3 size = col.size;
        Vector3[] localOffsets = {
            new (-.5f, -.5f, -.5f),
            new (-.5f, -.5f, .5f),
            new (.5f, -.5f, -.5f),
            new (.5f, -.5f, .5f),
            new (0f, -.5f, 0f)
        };

        Vector3[] corners = new Vector3[5];
        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 localCorner = Vector3.Scale(localOffsets[i], size);
            Vector3 worldCorner = transform.TransformPoint(center + localCorner);
            corners[i] = worldCorner;
        }
        
        // CHECKS IF ITS ENTIRELY ON THE ROAD
        foreach (Vector3 corner in corners) { 
            Debug.Log(corner);
            Collider[] roadHits = Physics.OverlapSphere(corner, .05f, StaticVariables.whatIsRoad);
            
            bool insideRoad = roadHits.Any(hitCol => hitCol.ClosestPoint(corner) == corner);
            
            if (insideRoad == false) { return false; }
        }
        
        // CHECKS FOR OTHER PLACED TRAPS
        Collider[] trapHits = Physics.OverlapBox(colPos, colSize / 2, transform.rotation, StaticVariables.whatIsBuilding, QueryTriggerInteraction.Ignore);
        if (trapHits.Any(hit => hit != col)) { return false; }
        
        // CHECKS THE PRICES
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

        return false;
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        
        Vector3 center = col.center;
        Vector3 size = col.size;
        Vector3[] localOffsets = {
            new (-.5f, -.5f, -.5f),
            new (-.5f, -.5f, .5f),
            new (.5f, -.5f, -.5f),
            new (.5f, -.5f, .5f),
            new (0f, -.5f, 0f)
        };

        Vector3[] corners = new Vector3[5];
        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 localCorner = Vector3.Scale(localOffsets[i], size);
            Vector3 worldCorner = transform.TransformPoint(center + localCorner);
            corners[i] = worldCorner;
        }

        Gizmos.color = Color.cyan;
        foreach (var corner in corners)
        {
            Gizmos.DrawSphere(corner, 0.05f);
        }
        
        Gizmos.matrix = Matrix4x4.TRS(colPos, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, colSize);
        
    }
}
