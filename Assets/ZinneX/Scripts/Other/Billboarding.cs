using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private void Update() {
        transform.forward = Camera.main.transform.forward;
    }
}
