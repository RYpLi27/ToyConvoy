using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New HominngTurret", menuName = "Scriptable Objects/Turrets/Homing Turret")]
public class HomingTurret : TurretSO {
    
    public override void Shoot(Transform firepoint, Transform target, TurretStats stats) {
        GameObject projectile = ObjectPoolManager.SpawnObject(projectilePrefab, firepoint.position, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile);
        projectile.GetComponent<HomingProjectile>().SetupProjectile(stats.projectileSpeed, target, stats.damage);
        // BULLETS LOGIC LIKE TRAJECTORY AND LAUNCHING AT TARGET IS ALL MADE IN PROJECTILE SCRIPT
    }
}
