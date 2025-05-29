using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New NonHomingTurret", menuName = "Scriptable Objects/Turrets/Non Homing Turret")]
public class NonHomingTurret : TurretSO {
    public float arcHeight;
    public float hitRadius;
    public AnimationCurve heightCurve;
    public float movementPredictDistance;

    public override void Shoot(Transform firepoint, Transform target, TurretStats stats) {
        GameObject projectile = ObjectPoolManager.SpawnObject(projectilePrefab, firepoint.position, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile);
        projectile.GetComponent<NonHomingProjectile>().SetupProjectile(this, new Vector3(target.position.x, 0f, target.position.z), target.GetComponentInParent<EnemyBehaviour>().MoveDir, stats);
        // BULLETS LOGIC LIKE TRAJECTORY AND LAUNCHING AT TARGET IS ALL MADE IN PROJECTILE SCRIPT
    }
}
