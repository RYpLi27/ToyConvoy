using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour {
    [SerializeField] private GameObject shootingCanvas;
    [SerializeField] private List<Slot> weapons;
    [SerializeField] private Transform gunHolder;
    public RectTransform reticle;

    private Slot selectedSlot;
    private Weapon selectedWeapon;

    private void Start() {
        WeaponSwap(0);
    }
    
    public void ShootInput(InputAction.CallbackContext context) {
        if (ModeManager.instance.playerMode != ModeManager.PlayerMode.Shooting) return;

        if (context.performed && selectedWeapon != null) {
            if (GameManager.gameState != GameManager.GameState.Ongoing) return;
            selectedWeapon.isShooting = true;
        }

        if (context.canceled && selectedWeapon != null) selectedWeapon.isShooting = false;
    }
    
    public void WeaponSwapInput(InputAction.CallbackContext context) {
        if (ModeManager.instance.playerMode != ModeManager.PlayerMode.Shooting || GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        if(context.performed) WeaponSwap(Mathf.RoundToInt(context.ReadValue<float>()));
    }

    private void WeaponSwap(int i) {
        if (i >= weapons.Count) return;

        if (selectedWeapon != null) { // DISABLES PREVIOUS WEAPON
            selectedSlot.DeselectSlot();
            ObjectPoolManager.ReturnObjectToPool(selectedWeapon.gameObject);
        }
        
        //ENABLES SELECTED WEAPON
        selectedSlot = weapons[i];
        selectedWeapon = ObjectPoolManager.SpawnObject(selectedSlot.SelectSlot(), gunHolder).GetComponent<Weapon>();
        selectedWeapon.SetReticle(reticle);
    }

    public void HideWeapon() {
        selectedWeapon.gameObject.SetActive(false);
        shootingCanvas.SetActive(false);
    }

    public void ShowWeapon() {
        selectedWeapon.gameObject.SetActive(true);
        shootingCanvas.SetActive(true);
    }
}
