using UnityEngine;

// THIS SCRIPT WORKS AS A BRIDGE BETWEEN LOOK AND BUILDING SCRIPT TO AVOID WEIRD COLLISION DETECTION
public class InteractTrigger : MonoBehaviour {
    [SerializeField] private Transform model;
    private IInteractable parentObject;

    private void Start() {
        parentObject = GetComponentInParent<IInteractable>();

        // Vector3 parentSize = model.localScale;
        // BoxCollider col = GetComponent<BoxCollider>();
        // col.size = new Vector3(parentSize.x + .4f, parentSize.y + .4f, parentSize.z + .4f);
        // col.center = new Vector3(0, col.size.y / 2 -.2f, 0);
    }

    public void Interact() {
        parentObject.Interact();
    }

    public void ShowPrompt(bool value) {
        parentObject.ShowPrompt(value);
    }
}
