using UnityEngine;

[CreateAssetMenu(fileName = "New Mortar", menuName = "Scriptable Objects/Turrets/Mortar")]
public class NonHomingTurret : TurretSO {
    public float arcHeight;
    public float hitRadius;
    public AnimationCurve heightCurve;
    public float movementPredictDistance;

    public override void Shoot(Transform firepoint, Transform target) {
        Instantiate(projectilePrefab, firepoint.position, Quaternion.identity).GetComponent<NonHomingProjectile>().SetupProjectile(this, target.position, target.GetComponent<EnemyBehaviour>().MoveDir);
        // BULLETS LOGIC LIKE TRAJECTORY AND LAUNCHING AT TARGET IS ALL MADE IN PROJECTILE SCRIPT
    }
}
