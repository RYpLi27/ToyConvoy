using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public static WaveManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject spawnWaveButton;
    [SerializeField] private Animator bossAlert;
    
    [SerializeField] [ListDrawerSettings(NumberOfItemsPerPage = 10, ShowIndexLabels = true)]
    [InfoBox("Those numbers are indexes. Actual wave is number + 1")]
    private List<EnemyWave> enemyWaves;

    private int currentWave;

    private int enemyCount;
    public int EnemyCount {
        get => enemyCount;
        set {
            enemyCount = value;
            if (enemyCount == 0) RestoreLandMines();
        }
    }

    private void Update() {
        spawnWaveButton.SetActive(enemyCount == 0);
        
        if (enemyCount == 0 && currentWave == enemyWaves.Count) {
            WinGame();
        }
    }

    private void RestoreLandMines() {
        foreach (Landmine landmine in FindObjectsByType<Landmine>(FindObjectsSortMode.None)) { landmine.RestoreLandmine(); }
    }
    
    public void SpawnWave() {
        if (enemyCount > 0) {
            Debug.Log("There are enemies still alive. Can't spawn next wave.");
            return;
        }
        
        if (enemyWaves[currentWave].isBossWave == true) {
            BossAlert();
        }
        
        //SPAWN ENEMIES
        foreach (EnemyInstance enemyInstance in enemyWaves[currentWave].enemies) {
            enemyCount += enemyInstance.amountToSpawn;
            StartCoroutine(EnemySpawnDelay(enemyInstance));
        }

        currentWave++;
    }
    
    private IEnumerator EnemySpawnDelay(EnemyInstance enemyInstance) {
        yield return new WaitForSeconds(enemyInstance.firstSpawnDelay);
        
        StartCoroutine(SpawnEnemies(enemyInstance, 0));
    }

    private IEnumerator SpawnEnemies(EnemyInstance enemyInstance, int enemiesSpawned) {
        if (enemiesSpawned == enemyInstance.amountToSpawn) yield break;
        
        ObjectPoolManager.SpawnObject(enemyInstance.enemyPrefab, spawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolingParent.Enemy);
        
        yield return new WaitForSeconds(enemyInstance.timeBetweenSpawns);
        
        StartCoroutine(SpawnEnemies(enemyInstance, enemiesSpawned + 1));
    }

    private void BossAlert() {
        bossAlert.Play("BossAlert");
    }
    
    private void WinGame() {
        Debug.Log("WIN");
        enabled = false;
        GameManager.instance.EndGame(GameManager.GameState.Win);
    }
}

[System.Serializable]
public class EnemyWave {
    public List<EnemyInstance> enemies;
    public bool isBossWave;
}

[System.Serializable]
public class EnemyInstance {
    public GameObject enemyPrefab;
    public int amountToSpawn;
    public float timeBetweenSpawns;
    public float firstSpawnDelay;
}
