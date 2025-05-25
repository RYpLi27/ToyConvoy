using System.Collections;
using UnityEngine;

public class PiercingProjectile : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 targetPos;
    private float distance;
    private float entireFlightLength;
    private float currentFlightLength;
    private PiercingTurret turretSO;
    private TurretSO.TurretStats stats;
    
    public void SetupProjectile(PiercingTurret newTurretSO, Vector3 newTarget, Vector3 movePredict, TurretSO.TurretStats newStats) {
        currentFlightLength = 0;
        turretSO = newTurretSO;
        stats = newStats;
        
        startPos = transform.position;
        targetPos = newTarget + movePredict * turretSO.movementPredictDistance;
        targetPos += (targetPos - startPos).normalized * turretSO.overshootDistance; // APPLYING OVERSHOOT (MORE DETAILS IN PiercingTurret.cs)
        
        distance = Vector3.Distance(startPos, targetPos);
        entireFlightLength = distance / stats.projectileSpeed;
        
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
        GetComponent<Collider>().enabled = true;
    }

    private void Update() {
        if (currentFlightLength >= entireFlightLength) { // REACHED TARGET
            StartCoroutine(Finished());
            return;
        } 
        
        CalculateNextStep();
    }

    private void CalculateNextStep() {
        currentFlightLength += Time.deltaTime;
        float t = currentFlightLength / entireFlightLength; // CURRENT % OF FLIGHT

        Vector3 newPosition = Vector3.Lerp(startPos, targetPos, t);

        float heightOffset = turretSO.arcHeight * turretSO.heightCurve.Evaluate(t); // CALCULATING Y POSITION BASED ON CURVE (DON'T TOUCH)
        newPosition.y += heightOffset;
        
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(turretSO.maxRotation * t, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private IEnumerator Finished() {
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(turretSO.lifetimeAfterReachingEnd);
        
        currentFlightLength = 0;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            other.GetComponent<HealthManager>().TakeDamage(stats.damage, transform.position);
        }
    }
}
