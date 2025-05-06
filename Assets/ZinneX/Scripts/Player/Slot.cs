using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    public GameObject heldObject;
    [SerializeField] private Image highlight;

    public GameObject SelectSlot() {
        highlight.enabled = true;
        return heldObject;
    }

    public void DeselectSlot() {
        highlight.enabled = false;
    }
}
