using UnityEngine;

[CreateAssetMenu(fileName = "New PiercingTurret", menuName = "Scriptable Objects/Turrets/Piercing Turret")]
public class PiercingTurret : TurretSO
{
    public float arcHeight;
    public AnimationCurve heightCurve;
    public float movementPredictDistance;
    public float lifetimeAfterReachingEnd;
    public float maxRotation; // PROJECTILE WILL CONSTANTLY ROTATE UP TO THIS ANGLE TO MIMIC GRAVITY
    public float overshootDistance; // PROJECTILE WONT STOP RIGHT ON ENEMY FOOT BUT WILL PASS THROUGH HIM POSSIBLY HITTING ENEMIES BEHIND HIM

    public override void Shoot(Transform firepoint, Transform target) {
        Instantiate(projectilePrefab, firepoint.position, Quaternion.identity).GetComponent<PiercingProjectile>().SetupProjectile(this, target.position, target.GetComponent<EnemyBehaviour>().MoveDir);
        // BULLETS LOGIC LIKE TRAJECTORY AND LAUNCHING AT TARGET IS ALL MADE IN PROJECTILE SCRIPT
    }
}
