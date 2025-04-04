using UnityEngine;

public class ParabolicProjectile : MonoBehaviour {
    private float damage;
    private float projectileSpeed;
    private float arcHeight;
    private float hitRadius;
    private Vector3 startPos;
    private Vector3 target;
    [SerializeField] private LayerMask whatIsEnemy;
    
    //calculations variables
    private float distance;
    private float entireFlightLength;
    private float currentFlightLength;
    
    public void SetupProjectile(float newProjectileSpeed, Vector3 newTarget, float newDamage, float newArcHeight, float newHitRadius) {
        projectileSpeed = newProjectileSpeed;
        target = newTarget;
        damage = newDamage;
        arcHeight = newArcHeight;
        startPos = transform.position;
        hitRadius = newHitRadius;
        
        CalculateLaunchVariables();
        gameObject.SetActive(true);
    }

    private void CalculateLaunchVariables() {
        distance = Vector3.Distance(startPos, target);
        entireFlightLength = distance / projectileSpeed;
    }

    private void Update() {
        CalculateNextPosition();
    }

    private void CalculateNextPosition() {
        if (currentFlightLength >= entireFlightLength) {
            Hit();
            gameObject.SetActive(false);
        } // REACHED TARGET

        currentFlightLength += Time.deltaTime;
        float t = currentFlightLength / entireFlightLength;

        
        Vector3 newPosition = Vector3.Slerp(startPos, target, t);

        float heightOffset = arcHeight * 4 * t * (1 - t); // FORMULA FOR HIGH ANGLE PARABOLIC ARC (DON'T TOUCH)
        newPosition.y += heightOffset;

        transform.position = newPosition;
    }

    private void Hit() {
        Collider[] enemiesInRadius = Physics.OverlapSphere(transform.position, hitRadius, whatIsEnemy);
        if (enemiesInRadius.Length == 0) { return; }

        foreach (Collider other in enemiesInRadius) {
            other.GetComponent<HealthManager>().TakeDamage(damage);
        }
        
        gameObject.SetActive(false);
    }
}