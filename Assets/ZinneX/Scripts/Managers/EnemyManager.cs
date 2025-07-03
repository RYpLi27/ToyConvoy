using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private List<EnemyBehaviour> enemies = new();

    private void FixedUpdate() {
        if (GameManager.gameState != GameManager.GameState.Ongoing || enemies.Count == 0) return;

        List<EnemyBehaviour> tempEnemies = enemies;
        for (int i = 0; i < tempEnemies.Count; i++) {
            if (tempEnemies[i] == null) continue;
            tempEnemies[i].CustomUpdate();
        }
    }

    public void AddEnemy(EnemyBehaviour enemy) {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyBehaviour enemy) {
        enemies.Remove(enemy);
    }
}
