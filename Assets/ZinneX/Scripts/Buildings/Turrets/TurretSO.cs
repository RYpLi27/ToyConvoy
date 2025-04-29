using UnityEngine;

public class TurretSO : BuildingSO {
    public float damage;
    public float fireRate;
    public float range;
    public float projectileSpeed;

    public GameObject projectilePrefab;

    public Material objectMaterial;
    
    public virtual void Shoot(Transform firepoint, Transform target) {}
}
