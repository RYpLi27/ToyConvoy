using UnityEngine;

public class Steps : MonoBehaviour
{
    public void PlayStepSound() {
        AudioManager.instance.playOneShot(FMODEvents.instance.steps, transform.position);
    }
}
