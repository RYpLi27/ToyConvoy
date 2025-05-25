using UnityEngine;

// THIS SCRIPT WORKS AS A BRIDGE BETWEEN LOOK AND BUILDING SCRIPT TO AVOID WEIRD COLLISION DETECTION
public class InteractTrigger : MonoBehaviour {
    private IInteractable parentObject;

    private void Start() {
        parentObject = GetComponentInParent<IInteractable>();
    }

    public void Interact() {
        parentObject.Interact();
    }

    public void ShowPrompt(bool value) {
        parentObject.ShowPrompt(value);
    }
}
