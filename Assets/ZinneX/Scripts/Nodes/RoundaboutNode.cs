using UnityEngine;

public class RoundaboutNode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            other.GetComponentInParent<EnemyBehaviour>().TryFindNode(GetComponent<Node>());
        }
    }
}
