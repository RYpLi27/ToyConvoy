using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class RatSteps : MonoBehaviour {
    private EventInstance stepsInstance;
    
    private void OnEnable() {
        stepsInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.ratCharge);
        stepsInstance.start();
    }

    private void Update() {
        stepsInstance.set3DAttributes(transform.position.To3DAttributes());
    }

    private void OnDisable() {
        stepsInstance.stop(STOP_MODE.IMMEDIATE);
        stepsInstance.release();
    }
}
