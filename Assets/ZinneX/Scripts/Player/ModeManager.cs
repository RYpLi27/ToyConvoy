using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeManager : MonoBehaviour {
    public static ModeManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    [ReadOnly] public PlayerMode playerMode;
    [SerializeField] private PlayerShooting shooting;
    [SerializeField] private PlayerBuilding building;

    public void ChangeModeInput(InputAction.CallbackContext context) {
        if (context.performed) ChangeMode();
    }

    private void ChangeMode() {
        switch (playerMode) {
            case PlayerMode.Building: // SWAPPING TO SHOOTING
                playerMode = PlayerMode.Shooting;

                building.HideBuilding();
                shooting.ShowWeapon();
                break;
            
            case PlayerMode.Shooting: // SWAPPING TO BUILDING
                playerMode = PlayerMode.Building;
                
                shooting.HideWeapon();
                building.ShowBuilding();
                break;
        }
    }
    
    public enum PlayerMode {
        Shooting,
        Building
    }
}
