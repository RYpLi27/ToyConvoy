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

    public Transform GetNode(int i, GameObject obj) {
        if (nodes.Count == i) { // IF ENEMY GETS TO FINAL NODE THEN DISABLE IT
            obj.SetActive(false);
            return null;
        }
        
        return nodes[i];   
    }
}
