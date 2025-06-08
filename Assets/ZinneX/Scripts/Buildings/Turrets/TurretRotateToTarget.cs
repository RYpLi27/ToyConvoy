using UnityEngine;

public class TurretRotateToTarget : MonoBehaviour {
    [SerializeField] private TurretBehaviour turretBehaviour;
    
    private void Update() {
        if (turretBehaviour.currentTarget == null || GameManager.gameState != GameManager.GameState.Ongoing) return;

        Vector3 targetRotation = turretBehaviour.currentTarget.position - transform.position;
        targetRotation.y = 0;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetRotation), .2f);
    }
}
