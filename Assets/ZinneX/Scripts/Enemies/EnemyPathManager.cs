using System.Collections.Generic;
using UnityEngine;

public class EnemyPathManager : MonoBehaviour {
    public static EnemyPathManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private List<Transform> nodes = new();

    public Transform GetNode(int i) => nodes.Count == i ? null : nodes[i];
}
