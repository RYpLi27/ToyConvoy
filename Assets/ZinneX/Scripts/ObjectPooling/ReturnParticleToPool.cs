using UnityEngine;

public class ReturnParticleToPool : MonoBehaviour
{
    private void OnParticleSystemStopped() {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
