using UnityEngine;

[CreateAssetMenu(fileName = "New HominngTurret", menuName = "Scriptable Objects/Turrets/Homing Turret")]
public class HomingTurret : TurretSO {
    
    public override void Shoot(Transform firepoint, Transform target) {
        Instantiate(projectilePrefab, firepoint.position, Quaternion.identity).GetComponent<HomingProjectile>().SetupProjectile(projectileSpeed, target, damage);
        // BULLETS LOGIC LIKE TRAJECTORY AND LAUNCHING AT TARGET IS ALL MADE IN PROJECTILE SCRIPT
    }
}
