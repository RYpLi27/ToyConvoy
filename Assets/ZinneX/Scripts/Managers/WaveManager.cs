using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
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
    [SerializeField] private TMP_Text waveCount;
    private bool bossAlertIsShown;
    
    [SerializeField] [ListDrawerSettings(NumberOfItemsPerPage = 10, ShowIndexLabels = true)]
    [InfoBox("Those numbers are indexes. Actual wave is number + 1")]
    private List<EnemyWave> enemyWaves;

    private int currentWave;

    private int enemyCount;
    public int EnemyCount {
        get => enemyCount;
        set {
            enemyCount = value;
            if (enemyCount == 0) WaveEnd();
        }
    }

    private void Start() {
        waveCount.text = $"{currentWave+1}/{enemyWaves.Count}";
    }

    private void WaveEnd() {
        //WIN GAME
        if (currentWave == enemyWaves.Count) {
            WinGame();
            return;
        }
        
        // RESTORES MINES
        // foreach (Landmine landmine in FindObjectsByType<Landmine>(FindObjectsSortMode.None)) { landmine.RestoreLandmine(); }
        
        //BOSS ALERT
        if(enemyWaves[currentWave].isBossWave == true) BossAlert(true);
        
        //UPDATE WAVE COUNT
        waveCount.text = $"{currentWave+1}/{enemyWaves.Count}";
        
        //SHOW BUTTON
        spawnWaveButton.SetActive(true);
    }
    
    public void SpawnWave() {
        if (enemyCount > 0) {
            Debug.Log("There are enemies still alive. Can't spawn next wave.");
            return;
        }

        spawnWaveButton.SetActive(false);
        if(bossAlertIsShown == true) BossAlert(false);
        
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

    private void BossAlert(bool show) {
        if (show == true) {   
            bossAlert.Play("Show");
            bossAlertIsShown = true;
        } else {
            bossAlert.Play("Hide");
            bossAlertIsShown = false;
        }
    }
    
    private void WinGame() {
        Debug.Log("WIN");
        enabled = false;
        StartCoroutine(GameManager.instance.EndGame(GameManager.GameState.Win));
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
