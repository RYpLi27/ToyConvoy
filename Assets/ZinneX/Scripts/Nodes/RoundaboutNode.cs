using UnityEngine;

public class RoundaboutNode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyBehaviour>().TryFindNode(GetComponent<Node>());
        }
    }
}
