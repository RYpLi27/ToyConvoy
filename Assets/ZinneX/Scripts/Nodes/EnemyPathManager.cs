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

    public Node startingNode;
}
