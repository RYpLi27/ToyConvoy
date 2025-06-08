using DG.Tweening;
using TMPro;
using UnityEngine;

public class RoadBlock : MonoBehaviour, IInteractable {
    [SerializeField] private InteractTrigger interact;
    [SerializeField] private BoxCollider backtrackTrigger;
    [SerializeField] private GameObject prompt;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private TMP_Text costText;
    
    [SerializeField] private int useCost;

    [SerializeField] private Vector3 usedRotation;
    [SerializeField] private Vector3 usedPosition;

    private void Start() {
        costText.text = $"{useCost}<sprite name=\"gold\">";
    }

    public void Interact() {
        if (GoldManager.instance.PriceCheck(useCost, true) == false) return;
        
        interact.gameObject.SetActive(false);

        // BoxCollider col = modelTransform.GetComponent<BoxCollider>();
        
        Sequence sequence = DOTween.Sequence();
        // sequence.AppendCallback(() => col.enabled = false);
        sequence.Join(modelTransform.DORotate(usedRotation, .75f)).SetEase(Ease.InExpo);
        sequence.Join(modelTransform.DOLocalMove(usedPosition, .75f)).SetEase(Ease.InExpo);
        sequence.AppendCallback(() => {
            // col.enabled = true;
            backtrackTrigger.enabled = true;
        });
        sequence.Play();
    }

    public void ShowPrompt(bool value) {
        prompt.SetActive(value);
    }
}
