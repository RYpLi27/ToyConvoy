using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Utilities;

public class Landmine : MonoBehaviour, IBuilding, IInteractable {
    [SerializeField] private LandmineSO landmineSO;
    
    [SerializeField] private SphereCollider triggerCol;
    [SerializeField] private Collider normalCol;
    [SerializeField] private GameObject rangeBorder;
    [SerializeField] private TurretPrompt upgradePrompt;
    [SerializeField] private InteractTrigger interactTrigger;
    [SerializeField] private Transform launchObj;
    [SerializeField] private MeshRenderer spawnerRenderer;
    private int currentLevel;
    private bool readyToLaunch;
    
    private void Start() {
        rangeBorder.GetComponent<SphereCollider>().radius = 1;
        rangeBorder.transform.localScale = Vector3.one * landmineSO.turretStats[0].explosionRange * 2;
        triggerCol.radius = landmineSO.turretStats[currentLevel].explosionRange;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Enemy")) {
            LaunchAtEnemy(other.transform);
        }
    }

    private void LaunchAtEnemy(Transform enemy) {
        if (readyToLaunch == false) return;

        readyToLaunch = false;
        DOTween.To(
            () => launchObj.position,
            newPos => launchObj.position = newPos,
            enemy.position,
            .15f
        ).OnComplete(Explode);
    }
    
    private void Explode() {
        Collider[] enemiesInRange = Physics.OverlapSphere(launchObj.position, landmineSO.turretStats[0].explosionRange, StaticVariables.whatIsEnemy);

        List<Transform> enemiesHit = new();
        foreach (Collider col in enemiesInRange) {
            if (enemiesHit.Contains(col.transform.parent)) continue;
            enemiesHit.Add(col.transform.parent);
            col.GetComponentInParent<HealthManager>().TakeDamage(landmineSO.turretStats[0].damage, col.transform.position + Vector3.up);
        }
        
        launchObj.gameObject.SetActive(false);
        StartCoroutine(RestoreLandmine());
    }

    public IEnumerator RestoreLandmine() {
        yield return new WaitForSeconds(landmineSO.turretStats[currentLevel].timeToRespawn);
        
        launchObj.position = transform.position;
        launchObj.gameObject.SetActive(true);
        readyToLaunch = true;
    }
    
    // public int GetPrice() {
    //     return landmineSO.price;
    // }
    
    public bool EnableBuilding() {
        if (GetComponent<PlacementCheck>().canPlace == false || GoldManager.instance.PriceCheck(landmineSO.price, true) == false) return false;

        GetComponent<PlacementCheck>().enabled = false;
        GetComponent<Collider>().enabled = true;
        triggerCol.enabled = true;
        normalCol.enabled = true;
        
        readyToLaunch = true;
        
        launchObj.gameObject.SetActive(true);
        rangeBorder.SetActive(false);
        interactTrigger.gameObject.SetActive(true);
        
        gameObject.layer = LayerMask.NameToLayer("EnvironmentalTrap");
        transform.SetParent(GameObject.Find("Turrets").transform);
        spawnerRenderer.material = landmineSO.objectMaterial;
        
        return true;
    }

    public bool PriceCheck() => GoldManager.instance.PriceCheck(landmineSO.price, false);

    public string GetDescription() =>
        $"{landmineSO.buildingName}\n\n" +
        $"<sprite name=\"damage\"> {landmineSO.turretStats[0].damage}\n" +
        $"<sprite name=\"timer\"> {landmineSO.turretStats[0].timeToRespawn}s\n" +
        $"<sprite name=\"range\"> {landmineSO.turretStats[0].explosionRange}\n" +
        $"<sprite name=\"gold\"> {landmineSO.price}\n\n" +
        $"{landmineSO.description}";

    public void ShowPrompt(bool value) {
        upgradePrompt.UpdatePromptUI(landmineSO.turretStats[currentLevel].upgradePrice, currentLevel, GetStatsText());
        upgradePrompt.gameObject.SetActive(value);
    }

    private string GetStatsText() => currentLevel < landmineSO.turretStats.Count - 1
        ? $"<sprite name=\"damage\"> {landmineSO.turretStats[currentLevel].damage}<color=green>(+{Math.Round(landmineSO.turretStats[currentLevel + 1].damage - landmineSO.turretStats[currentLevel].damage, 2)})</color>\n" +
          $"<sprite name=\"timer\"> {landmineSO.turretStats[currentLevel].timeToRespawn}s<color=green>({Math.Round(landmineSO.turretStats[currentLevel + 1].timeToRespawn - landmineSO.turretStats[currentLevel].timeToRespawn, 2)}s)</color>\n" +
          $"<sprite name=\"range\"> {landmineSO.turretStats[currentLevel].explosionRange}<color=green>(+{Math.Round(landmineSO.turretStats[currentLevel + 1].explosionRange - landmineSO.turretStats[currentLevel].explosionRange, 2)})</color>\n"
          
        : $"<sprite name=\"damage\"> {landmineSO.turretStats[currentLevel].damage}\n" +
          $"<sprite name=\"timer\"> {landmineSO.turretStats[currentLevel].timeToRespawn}\n" +
          $"<sprite name=\"range\"> {landmineSO.turretStats[currentLevel].explosionRange}\n";
    
    public void Interact() {
        if (landmineSO.turretStats.Count - 1 == currentLevel || GoldManager.instance.PriceCheck(landmineSO.turretStats[currentLevel].upgradePrice, true) == false) return;

        currentLevel++;
        upgradePrompt.UpdatePromptUI(landmineSO.turretStats[currentLevel].upgradePrice, currentLevel, GetStatsText());
        triggerCol.radius = landmineSO.turretStats[currentLevel].explosionRange;
    }
    
    private void OnDrawGizmos() {
        if (landmineSO == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, landmineSO.turretStats[currentLevel].explosionRange);
    }
}
