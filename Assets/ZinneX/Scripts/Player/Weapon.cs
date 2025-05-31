using Sirenix.OdinInspector;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask whatIsTarget;
    // [SerializeField] private Material enemyHitMaterial;
    // [SerializeField] private Material obstacleHitMaterial;
    [SerializeField] private GameObject damageText;

    private float lastShootTime;
    [ReadOnly] public bool isShooting;
    
    private void Update() {
        if(isShooting == true) Shoot();
    }

    private void OnDisable() {
        isShooting = false;
    }

    private void Shoot() {
        if (Time.time - lastShootTime <= 1f / weaponSO.fireRate || Cursor.lockState != CursorLockMode.Locked) return;
        
        lastShootTime = Time.time;

        for (int i = 0; i < weaponSO.numberOfBullets; i++) {
            Vector3 randomRecoil = Vector3.up * Random.Range(-weaponSO.verticalRecoil, weaponSO.verticalRecoil) + Vector3.right * Random.Range(-weaponSO.horizontalRecoil, weaponSO.horizontalRecoil);
            
            if(Physics.Raycast(firePoint.position, GetAimPosition() + randomRecoil - firePoint.position, out RaycastHit ray, weaponSO.weaponRange, whatIsTarget, QueryTriggerInteraction.Ignore)) {
                if (ray.collider.CompareTag("Enemy")) {
                    float damageDealt = ray.collider.GetComponent<Hitbox>().Hit(weaponSO.damage, ray.point);
                }
                
                // CreateHitEffect(ray.point, ray.transform.CompareTag("Enemy") ? enemyHitMaterial : obstacleHitMaterial);
                CreateHitEffect(ray.point);
                CreateBulletTrail(firePoint.position, ray.point);
            } else {
                CreateBulletTrail(firePoint.position,  GetAimPosition() + randomRecoil);
            }
        }
    }
    
    private Vector3 GetAimPosition() {
        Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore);
        
        if(hit.transform != null) return hit.distance > 11 ? hit.point : cameraRay.origin + cameraRay.direction * 11;
        return cameraRay.origin + cameraRay.direction * weaponSO.weaponRange;
    }
    
    // USE THIS WHEN DIFFERENT MATERIAL IS NEEDED WHEN HITTING ENEMY AND OBSTACLE
    // private void CreateHitEffect(Vector3 position, Material material) { 
    //     ParticleSystemRenderer hitEffect = ObjectPoolManager.SpawnObject(weaponSO.bulletHitPrefab, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile).GetComponent<ParticleSystemRenderer>();
    //     if (hitEffect == null) return;
    //     hitEffect.material = material;
    // }
    
    private void CreateHitEffect(Vector3 position) {
        ObjectPoolManager.SpawnObject(weaponSO.bulletHitPrefab, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile).GetComponent<ParticleSystemRenderer>();
    }
    
    private void CreateBulletTrail(Vector3 start, Vector3 end) {
        LineRenderer trail = ObjectPoolManager.SpawnObject(weaponSO.bulletTrialPrefab, start, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile).GetComponent<LineRenderer>();
        trail.SetPosition(0, start);
        trail.SetPosition(1, end);
    }
    
    private void OnDrawGizmos() {
        if (firePoint == null) return;
        
        Gizmos.color = Color.red;
        
        Gizmos.DrawRay(firePoint.position, GetAimPosition() - firePoint.position);
    }
}
