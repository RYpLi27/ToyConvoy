using UnityEngine;

[CreateAssetMenu(fileName = "New TurretSniperNest", menuName = "Scriptable Objects/Turrets/Sniper Nest")]
public class TurretSniperNest : TurretSO
{
    public override void Shoot(Transform firepoint, Transform target) {
        Instantiate(projectilePrefab, firepoint.position, Quaternion.identity).GetComponent<SniperNestProjectile>().SetupProjectile(projectileSpeed, target, damage);
        // BULLETS LOGIC LIKE TRAJECTORY AND LAUNCHING AT TARGET IS ALL MADE IN PROJECTILE SCRIPT
    }
}
