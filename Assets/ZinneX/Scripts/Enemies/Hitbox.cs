using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    public HitboxType hitboxType;
    private HealthManager healthManager;

    private Dictionary<HitboxType, float> hitboxMults = new() {
        {HitboxType.Head, 1.75f},
        {HitboxType.Body , 1}
    };
    
    private void Start() {
        healthManager = GetComponentInParent<HealthManager>();
    }

    public float Hit(float damage, Vector3 hitPoint) {
        return healthManager.TakeDamage(damage * hitboxMults[hitboxType], hitPoint);
    }
    
    public enum HitboxType { // can add legs etc. later
        Body,
        Head
    }
}
