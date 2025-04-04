using UnityEngine;

public class HomingProjectile : MonoBehaviour {
    private Transform target;
    private float projectileSpeed;
    private float damage;

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * projectileSpeed);
        
        if(target.gameObject.activeInHierarchy == false) {gameObject.SetActive(false);} // IF ENEMY DIES OR REACHES LAST NODE DISABLE THIS PROJECTILE
    }

    public void SetupProjectile(float projSpeed, Transform newTarget, float newDamage) {
        target = newTarget;
        projectileSpeed = projSpeed;
        damage = newDamage;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            gameObject.SetActive(false);
            other.GetComponent<HealthManager>().TakeDamage(damage);
        }
    }
}
