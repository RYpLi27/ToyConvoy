using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private Color enemyHitColor;
    [SerializeField] private Color obstacleHitColor;

    private float lastShootTime;
    private bool isShooting;

    private void Update() {
        if(isShooting == true) Shoot();
    }

    public void ShootInput(InputAction.CallbackContext context) {
        if (context.performed && Time.time - lastShootTime > 1f / weaponSO.fireRate) isShooting = true;

        if (context.canceled) isShooting = false;
    }

    private void Shoot() {
        if (Time.time - lastShootTime <= 1f / weaponSO.fireRate || Cursor.lockState != CursorLockMode.Locked) return;
        
        lastShootTime = Time.time;
        
        Vector3 randomRecoil = Vector3.up * Random.Range(-weaponSO.verticalRecoil, weaponSO.verticalRecoil) + Vector3.right * Random.Range(-weaponSO.horizontalRecoil, weaponSO.horizontalRecoil);
        
        if(Physics.Raycast(firePoint.position, GetAimPosition() + randomRecoil - firePoint.position, out RaycastHit ray, weaponSO.weaponRange, whatIsTarget, QueryTriggerInteraction.Ignore)) {
            //LOGIC
            if (ray.transform.CompareTag("Enemy")) {
                ray.transform.GetComponent<HealthManager>().TakeDamage(weaponSO.damage);
            }
            
            //VISUALS
            CreateHitEffect(ray.point, ray.transform.CompareTag("Enemy") ? enemyHitColor : obstacleHitColor);
            CreateBulletTrail(firePoint.position, ray.point);
        } else {
            CreateBulletTrail(firePoint.position,  GetAimPosition() + randomRecoil);
        }
    }

    private Vector3 GetAimPosition() {
        Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore);
        
        if(hit.transform != null) return hit.distance > 11 ? hit.point : cameraRay.origin + cameraRay.direction * 11;
        return cameraRay.origin + cameraRay.direction * weaponSO.weaponRange;
    }

    private void CreateHitEffect(Vector3 position, Color color) {
        ParticleSystem.MainModule hitEffect = ObjectPoolManager.SpawnObject(weaponSO.bulletHitPrefab, position, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile).GetComponent<ParticleSystem>().main;
        hitEffect.startColor = color;
    }
    
    private void CreateBulletTrail(Vector3 start, Vector3 end) {
        LineRenderer trail = ObjectPoolManager.SpawnObject(weaponSO.bulletTrialPrefab, start, Quaternion.identity, ObjectPoolManager.PoolingParent.Projectile).GetComponent<LineRenderer>();
        trail.SetPosition(0, start);
        trail.SetPosition(1, end);

        StartCoroutine(ReturnTrail(trail.gameObject, .03f));
    }

    private IEnumerator ReturnTrail(GameObject obj, float t) {
        yield return new WaitForSeconds(t);
        
        ObjectPoolManager.ReturnObjectToPool(obj);
    }
    
    private void OnDrawGizmos() {
        if (firePoint == null) return;
        
        Gizmos.color = Color.red;
        
        Gizmos.DrawRay(firePoint.position, GetAimPosition() - firePoint.position);
    }
}
