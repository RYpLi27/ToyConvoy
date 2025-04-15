using UnityEngine;

public class TurretRotateToTarget : MonoBehaviour {
    [SerializeField] private TurretBehaviour turretBehaviour;
    
    private void Update() {
        if (turretBehaviour.currentTarget == null) return;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(turretBehaviour.currentTarget.position - transform.position), .2f);
    }
}
