using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectile : MonoBehaviour {
    [SerializeField] private float rotationOffset;
    
    private Vector3 startPos;
    private Vector3 targetPos;
    private float distance;
    private float entireFlightLength;
    private float currentFlightLength;
    private PiercingTurret turretSO;
    private TurretSO.TurretStats stats;
    private bool isFinished;
    
    private List<Transform> enemiesHit = new();
    
    public void SetupProjectile(PiercingTurret newTurretSO, Vector3 newTarget, Vector3 movePredict, TurretSO.TurretStats newStats) {
        currentFlightLength = 0;
        turretSO = newTurretSO;
        stats = newStats;
        enemiesHit.Clear();
        
        startPos = transform.position;
        targetPos = newTarget + movePredict * turretSO.movementPredictDistance;

        Vector3 overshoot = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z).normalized * turretSO.overshootDistance;
        targetPos += overshoot;
        
        distance = Vector3.Distance(startPos, targetPos);
        entireFlightLength = distance / stats.projectileSpeed;
        
        Vector3 targetEuler = Quaternion.LookRotation(targetPos - transform.position).eulerAngles;
        targetEuler.y += rotationOffset;
        Quaternion targetRotation = Quaternion.Euler(targetEuler);
        
        transform.rotation = targetRotation;
        GetComponent<Collider>().enabled = true;
    }

    private void Update() {
        if (isFinished) return;
        
        if (currentFlightLength >= entireFlightLength) { // REACHED TARGET
            StartCoroutine(Finished());
            return;
        } 
        
        currentFlightLength += Time.deltaTime;
        CalculateNextStep(currentFlightLength / entireFlightLength);
    }

    private void CalculateNextStep(float t) {
        Vector3 newPosition = Vector3.Lerp(startPos, targetPos, t);

        float heightOffset = turretSO.arcHeight * turretSO.heightCurve.Evaluate(t); // CALCULATING Y POSITION BASED ON CURVE
        newPosition.y += heightOffset;
        
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(turretSO.maxRotation * t, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private IEnumerator Finished() {
        isFinished = true;
        GetComponent<Collider>().enabled = false;
        CalculateNextStep(1);

        yield return new WaitForSeconds(turretSO.lifetimeAfterReachingEnd);
        
        currentFlightLength = 0;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
        isFinished = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && enemiesHit.Contains(other.transform.parent) == false) {
            enemiesHit.Add(other.transform.parent);
            other.GetComponentInParent<HealthManager>().TakeDamage(stats.damage, transform.position);
        }
    }
}
