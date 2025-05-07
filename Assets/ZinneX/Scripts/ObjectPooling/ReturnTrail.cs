using UnityEngine;

public class ReturnTrail : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), .03f);
    }

    private void ReturnToPool() {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
