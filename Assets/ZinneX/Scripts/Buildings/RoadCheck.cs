using System.Collections.Generic;
using UnityEngine;

public class RoadCheck : MonoBehaviour {
    public static RoadCheck instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private LayerMask roadLayer;
    [SerializeField] private List<Collider> colliders;

    // public bool TrapPlacement(Collider trapCollider) {
    // if (trapCollider == null) return false;
    //
    // Vector3 center = trapCollider.bounds.center;
    // Vector3 extents = trapCollider.bounds.extents;
    //
    // Vector3[] corners = new Vector3[4];
    // corners[0] = center + new Vector3(-extents.x, -extents.y, -extents.z);
    // corners[1] = center + new Vector3(extents.x, -extents.y, -extents.z);
    // corners[2] = center + new Vector3(-extents.x, -extents.y, extents.z);
    // corners[3] = center + new Vector3(extents.x, -extents.y, extents.z);
    //
    // foreach (Vector3 corner in corners) {
    //     
    // }
    // }
}
