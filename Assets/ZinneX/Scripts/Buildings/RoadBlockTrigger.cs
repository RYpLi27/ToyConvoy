using UnityEngine;

public class RoadBlockTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyBehaviour>().backtrack = true;
        }
    }
}
