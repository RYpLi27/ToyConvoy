using UnityEngine;

public class DamageText : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), .75f);
    }

    private void ReturnToPool() {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void Update() {
        transform.position += Vector3.up * Time.deltaTime;
    }
}
