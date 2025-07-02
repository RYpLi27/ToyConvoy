using System.Collections.Generic;
using UnityEngine;

public class NonHomingProjectile : MonoBehaviour {
    private Vector3 startPos;
    private Vector3 targetPos;
    private float distance;
    private float entireFlightLength;
    private float currentFlightLength;
    private NonHomingTurret  turretSO;
    private TurretSO.TurretStats stats;
    
    public void SetupProjectile(NonHomingTurret newTurretSO, Vector3 newTarget, Vector3 movePredict, TurretSO.TurretStats newStats) {
        turretSO = newTurretSO;
        stats = newStats;
        
        startPos = transform.position;
        targetPos = newTarget + movePredict * turretSO.movementPredictDistance;
        
        distance = Vector3.Distance(startPos, targetPos);
        entireFlightLength = distance / stats.projectileSpeed;
        
        transform.rotation = Quaternion.LookRotation(targetPos);
    }

    private void Update() {
        CalculateNextStep();
    }

    private void CalculateNextStep() {
        if (currentFlightLength >= entireFlightLength) { // REACHED TARGET
            Hit();
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        } 

        currentFlightLength += Time.deltaTime;
        float t = currentFlightLength / entireFlightLength; // CURRENT % OF FLIGHT

        Vector3 newPosition = Vector3.Lerp(startPos, targetPos, t);

        float heightOffset = turretSO.arcHeight * turretSO.heightCurve.Evaluate(t); // CALCULATING Y POSITION BASED ON CURVE (DON'T TOUCH)
        newPosition.y += heightOffset;
        
        transform.position = newPosition;
    }

    private void Hit() {
        AudioManager.instance.playOneShot(FMODEvents.instance.mortarBoom, transform.position);
        Vector3 hitEffectPos = transform.position;
        hitEffectPos.y = .5f;
        ObjectPoolManager.SpawnObject(turretSO.hitEffect, hitEffectPos, Quaternion.identity);
        
        currentFlightLength = 0;
        Collider[] enemiesInRadius = Physics.OverlapSphere(transform.position, turretSO.hitRadius, StaticVariables.whatIsEnemy);
        if (enemiesInRadius.Length == 0) { return; }

        List<Transform> enemiesHit = new();
        
        foreach (Collider other in enemiesInRadius) {
            if (other.gameObject.activeInHierarchy == false || enemiesHit.Contains(other.transform.parent)) continue;
            enemiesHit.Add(other.transform.parent);
            other.GetComponentInParent<HealthManager>().TakeDamage(stats.damage, transform.position);
        }
        
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}