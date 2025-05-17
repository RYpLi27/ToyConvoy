using UnityEngine;

public class HomingProjectile : MonoBehaviour {
    private Transform target;
    private float projectileSpeed;
    private float damage;

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * projectileSpeed);
        
        if(target.gameObject.activeInHierarchy == false) {ObjectPoolManager.ReturnObjectToPool(gameObject);} // IF ENEMY DIES OR REACHES LAST NODE DISABLE THIS PROJECTILE
    }

    public void SetupProjectile(float projSpeed, Transform newTarget, float newDamage) {
        target = newTarget;
        projectileSpeed = projSpeed;
        damage = newDamage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform == target) {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
            other.GetComponent<HealthManager>().TakeDamage(damage, transform.position);
        }
    }
}
