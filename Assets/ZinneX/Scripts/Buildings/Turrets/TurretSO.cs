using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TurretSO : BuildingSO {
    public List<TurretStats> turretStats;

    public GameObject projectilePrefab;

    public Material objectMaterial;
    
    public virtual void Shoot(Transform firepoint, Transform target, TurretStats stats) {}

    [System.Serializable]
    public class TurretStats {
        public float damage;
        [LabelText("Firerate (Attacks per second)")]
        public float fireRate;
        public float range;
        public float projectileSpeed;
        public int upgradePrice;
    }
}
