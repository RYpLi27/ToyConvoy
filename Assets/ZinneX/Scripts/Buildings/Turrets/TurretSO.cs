using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TurretSO : BuildingSO {
    public float damage;
    
    [LabelText("Firerate (Attacks per second)")]
    public float fireRate;
    public float range;
    public float projectileSpeed;

    public GameObject projectilePrefab;

    public Material objectMaterial;
    
    public virtual void Shoot(Transform firepoint, Transform target) {}
}
