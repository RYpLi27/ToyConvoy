using UnityEngine;

public class NonHomingProjectile : MonoBehaviour {
    private Vector3 startPos;
    private Vector3 targetPos;
    private float distance;
    private float entireFlightLength;
    private float currentFlightLength;
    private NonHomingTurret  turretSO;
    
    public void SetupProjectile(NonHomingTurret newTurretSO, Vector3 newTarget, Vector3 movePredict) {
        turretSO = newTurretSO;
        
        startPos = transform.position;
        targetPos = newTarget + movePredict * turretSO.movementPredictDistance;
        
        distance = Vector3.Distance(startPos, targetPos);
        entireFlightLength = distance / turretSO.projectileSpeed;
        
        transform.rotation = Quaternion.LookRotation(targetPos);
        
        gameObject.SetActive(true);
    }

    private void Update() {
        CalculateNextStep();
    }

    private void CalculateNextStep() {
        if (currentFlightLength >= entireFlightLength) { // REACHED TARGET
            Hit();
            gameObject.SetActive(false);
        } 

        currentFlightLength += Time.deltaTime;
        float t = currentFlightLength / entireFlightLength; // CURRENT % OF FLIGHT

        Vector3 newPosition = Vector3.Lerp(startPos, targetPos, t);

        float heightOffset = turretSO.arcHeight * turretSO.heightCurve.Evaluate(t); // CALCULATING Y POSITION BASED ON CURVE (DON'T TOUCH)
        newPosition.y += heightOffset;
        
        transform.position = newPosition;
    }

    private void Hit() {
        Collider[] enemiesInRadius = Physics.OverlapSphere(transform.position, turretSO.hitRadius, StaticVariables.whatIsEnemy);
        if (enemiesInRadius.Length == 0) { return; }

        foreach (Collider other in enemiesInRadius) {
            other.GetComponent<HealthManager>().TakeDamage(turretSO.damage);
        }
        
        gameObject.SetActive(false);
    }
}