using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour {
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private Material enemyHitMaterial;
    [SerializeField] private Material obstacleHitMaterial;
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
                if (ray.transform.CompareTag("Enemy")) {
                    float damageDealt = ray.transform.GetComponent<HealthManager>().TakeDamage(weaponSO.damage);
                    CreateDamageText(ray.point, damageDealt);
                }
                
                CreateHitEffect(ray.point, ray.transform.CompareTag("Enemy") ? enemyHitMaterial : obstacleHitMaterial);
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

    private void CreateDamageText(Vector3 position, float value) {
        TMP_Text dmgText = ObjectPoolManager.SpawnObject(damageText, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Effect).GetComponentInChildren<TMP_Text>();
        dmgText.text = value.ToString();
    }
    
    private void CreateHitEffect(Vector3 position, Material material) {
        ParticleSystemRenderer hitEffect = ObjectPoolManager.SpawnObject(weaponSO.bulletHitPrefab, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile).GetComponent<ParticleSystemRenderer>();
        hitEffect.material = material;
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
