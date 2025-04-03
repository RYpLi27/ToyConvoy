using UnityEngine;

public class TurretSO : ScriptableObject {
    public float damage;
    public float fireRate;
    public float range;
    public float projectileSpeed;

    public GameObject projectilePrefab;
    
    public virtual void Shoot(Transform firepoint, Transform target) {}
}
