using TMPro;
using UnityEngine;

public class TurretPrompt : MonoBehaviour {
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private GameObject inputImage;

    public void UpdatePromptUI(int cost, int level, string stats) {
        UpdateCost(cost);
        UpdateLevel(level + 1);
        UpdateStats(stats);
    }
    
    private void UpdateCost(int value) {
        if (value == 0) {
            costText.gameObject.SetActive(false);
            inputImage.SetActive(false);
            return;
        }
        costText.text = $"{value}<sprite name=\"gold\">";
    }

    private void UpdateLevel(int level) {
        string newLevelText = null;

        for (int i = 0; i < level; i++) {
            newLevelText += "<sprite name=\"star\">";
        }
        
        levelText.text = newLevelText;
    }

    private void UpdateStats(string stats) {
        statsText.text = stats;
    }
}
