using UnityEngine;

[CreateAssetMenu(fileName = "New Mortar", menuName = "Scriptable Objects/Turrets/Mortar")]
public class TurretMortar : TurretSO {
    public float arcHeight;
    public float hitRadius;

    public override void Shoot(Transform firepoint, Transform target) {
        Instantiate(projectilePrefab, firepoint.position, Quaternion.identity).GetComponent<ParabolicProjectile>().SetupProjectile(projectileSpeed, target.position, damage, arcHeight, hitRadius);
        // BULLETS LOGIC LIKE TRAJECTORY AND LAUNCHING AT TARGET IS ALL MADE IN PROJECTILE SCRIPT
    }
}
