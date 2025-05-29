using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour {
    private Transform target;
    private float projectileSpeed;
    private float damage;

    private List<Transform> enemiesHit  = new();
    
    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * projectileSpeed);
        
        if(target.gameObject.activeInHierarchy == false) {ObjectPoolManager.ReturnObjectToPool(gameObject);} // IF ENEMY DIES OR REACHES LAST NODE DISABLE THIS PROJECTILE
    }

    public void SetupProjectile(float projSpeed, Transform newTarget, float newDamage) {
        target = newTarget;
        projectileSpeed = projSpeed;
        damage = newDamage;
        enemiesHit.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform == target && enemiesHit.Contains(other.transform.parent) == false) {
            enemiesHit.Add(other.transform.parent);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
            other.GetComponentInParent<HealthManager>().TakeDamage(damage, transform.position);
        }
    }
}
