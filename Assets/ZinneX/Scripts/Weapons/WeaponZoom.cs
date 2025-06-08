using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class WeaponZoom : MonoBehaviour {
    [SerializeField] private float zoomFOV;
    private float standardFOV;
    private bool isZoomed;
    private CinemachineCamera cam;

    private InputAction zoomAction;

    [SerializeField] private Volume zoomVolume;
    
    private void Awake() {
        cam = CinemachineBrain.GetActiveBrain(0).ActiveVirtualCamera as CinemachineCamera;
        standardFOV = cam.Lens.FieldOfView;
    }

    private void OnEnable() {
        zoomAction = ModeManager.instance.GetComponent<PlayerInput>().actions["ZoomInput"];
        zoomAction.performed += ZoomInput;
    }

    public void ZoomInput(InputAction.CallbackContext context) {
        if (context.performed) {
            isZoomed = !isZoomed;
            Zoom();
        }
    }

    private void Zoom() {
        DOTween.To(
            () => cam.Lens.FieldOfView,
            v =>  cam.Lens.FieldOfView = v,
            isZoomed == true ? zoomFOV : standardFOV,
            0.2f
            );

        DOTween.To(
            () => zoomVolume.weight,
            weight => zoomVolume.weight = weight,
            isZoomed == true ? 1f : 0f,
            .2f
        );

        PlayerLook.isZoomed = isZoomed;
    }
    
    private void OnDisable() {
        isZoomed = false;
        zoomAction.performed -= ZoomInput;
        Zoom();
    }
}
