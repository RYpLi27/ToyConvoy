using UnityEngine;

public class TurretRotateToTarget : MonoBehaviour {
    [SerializeField] private TurretBehaviour turretBehaviour;
    [SerializeField] private float rotationOffset;
    
    private void Update() {
        if (turretBehaviour.currentTarget == null || GameManager.gameState != GameManager.GameState.Ongoing) return;

        Vector3 targetPos = turretBehaviour.currentTarget.position - transform.position;
        targetPos.y = 0;

        Vector3 targetEuler = Quaternion.LookRotation(targetPos).eulerAngles;
        targetEuler.y += rotationOffset;
        Quaternion targetRotation = Quaternion.Euler(targetEuler);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, .2f);
    }
}
