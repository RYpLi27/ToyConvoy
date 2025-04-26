using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilding : MonoBehaviour {
    public static PlayerBuilding instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private List<GameObject> turrets;
    [SerializeField] private Transform placePoint;

    public Material wrongMaterial;
    public Material correctMaterial;
    
    private GameObject selectedTurret;
    private int selectedIndex;
    
    private void Start() {
        SelectTurret(0);
        HideBuilding();
    }

    public void SelectTurretInput(InputAction.CallbackContext context) {
        if (ModeManager.instance.playerMode != ModeManager.PlayerMode.Building || Cursor.lockState != CursorLockMode.Locked) return;

        if (context.performed) {
            selectedIndex = Mathf.RoundToInt(context.ReadValue<float>());
            SelectTurret(selectedIndex, true);
        }
    }
    
    private void SelectTurret(int i, bool returnToPool = false) {
        if (i >= turrets.Count) return;
        
        if(selectedTurret != null && returnToPool == true) ObjectPoolManager.ReturnObjectToPool(selectedTurret);
        selectedTurret = ObjectPoolManager.SpawnObject(turrets[i], placePoint);
        
        selectedTurret.transform.localPosition = Vector3.zero;
    }

    public void PlaceTurretInput(InputAction.CallbackContext context) {
        if (ModeManager.instance.playerMode != ModeManager.PlayerMode.Building  || Cursor.lockState != CursorLockMode.Locked) return;
        
        if (context.performed) {
            switch (selectedTurret.tag) {
                case "Turret":
                    if(selectedTurret.GetComponent<TurretBehaviour>().EnableTurret())
                        SelectTurret(selectedIndex);
                    break;
                
                case "Explosive":
                    if(selectedTurret.GetComponent<Landmine>().EnableLandmine())
                        SelectTurret(selectedIndex);
                    break;
                
                case "TrapDOT":
                    if(selectedTurret.GetComponent<Spikes>().EnableSpikes())
                        SelectTurret(selectedIndex);
                    break;
            }
        }
    }
    
    public void HideBuilding() {
        // ObjectPoolManager.ReturnObjectToPool(selectedTurret);
        selectedTurret.SetActive(false);
    }
    
    public void ShowBuilding() {
        // selectedTurret = ObjectPoolManager.SpawnObject(selectedTurret, placePoint);
        selectedTurret.SetActive(true);
    }
}
