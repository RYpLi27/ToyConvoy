using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask whatIsTarget;
    // [SerializeField] private Material enemyHitMaterial;
    // [SerializeField] private Material obstacleHitMaterial;
    [SerializeField] private GameObject damageText;
    private RectTransform reticle;

    private float lastShootTime;
    [ReadOnly] public bool isShooting;
    private CinemachineCamera vcam;

    private void Update() {
        HandleReticle();
        if(isShooting == true) Shoot();
    }

    private void OnDisable() {
        isShooting = false;
    }

    public void SetReticle(RectTransform reticleTransform) {
        reticle = reticleTransform;
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
                CreateBulletTrail(firePoint.position, ray.point);
                CreateEffects(ray.point);
            } else {
                CreateBulletTrail(firePoint.position,  GetAimPosition() + randomRecoil);
            }

            CreateMuzzleFlash();
            AudioManager.instance.playOneShot(FMODEvents.instance.shotFired, this.transform.position);
            Debug.Log("audio");
        }

        WeaponRecoil.instance.ApplyRecoil(weaponSO.cameraVerticalRecoil, weaponSO.cameraHorizontalRecoil);
    }
    
    private Vector3 GetAimPosition() {
        Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity, whatIsTarget, QueryTriggerInteraction.Ignore);
        
        if(hit.transform != null) return hit.point;
        return cameraRay.origin + cameraRay.direction * weaponSO.weaponRange;
    }

    private void HandleReticle() {
        if (Physics.Raycast(firePoint.position, GetAimPosition() - firePoint.position, out RaycastHit ray, weaponSO.weaponRange, whatIsTarget, QueryTriggerInteraction.Ignore)) {
            reticle.position = Camera.main.WorldToScreenPoint(ray.point);
        } else reticle.position = new Vector3(Screen.width/2f, Screen.height/2f, 0);
        
        reticle.gameObject.SetActive(Vector2.Distance(reticle.position, new Vector2(Screen.width/2f, Screen.height/2f)) > 20f && Vector3.Distance(transform.position, ray.point) < 5);
    }
    
    // USE THIS WHEN DIFFERENT MATERIAL IS NEEDED WHEN HITTING ENEMY AND OBSTACLE
    // private void CreateHitEffect(Vector3 position, Material material) { 
    //     ParticleSystemRenderer hitEffect = ObjectPoolManager.SpawnObject(weaponSO.bulletHitPrefab, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile).GetComponent<ParticleSystemRenderer>();
    //     if (hitEffect == null) return;
    //     hitEffect.material = material;
    // }
    
    private void CreateEffects(Vector3 position) {
        ObjectPoolManager.SpawnObject(weaponSO.bulletHitPrefab, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Effect);
    }

    private void CreateMuzzleFlash() {
        ObjectPoolManager.SpawnObject(weaponSO.muzzleFlashPrefab, firePoint);
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
