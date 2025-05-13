using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private GameObject buildingCanvas;
    [SerializeField] private List<Slot> turrets;
    [SerializeField] private Transform placePoint;
    // [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text descriptionText;


    public Material wrongMaterial;
    public Material correctMaterial;
    
    private GameObject selectedTurret;
    private Slot selectedSlot;
    private int selectedIndex;
    
    private void Start() {
        SelectTurret(0);
        HideBuilding();
    }

    public void SelectTurretInput(InputAction.CallbackContext context) {
        if (CanMakeAction() == false) return;

        if (context.performed) {
            selectedIndex = Mathf.RoundToInt(context.ReadValue<float>());
            SelectTurret(selectedIndex, true);
        }
    }
    
    private void SelectTurret(int i, bool returnToPool = false) {
        if (i >= turrets.Count) return;

        if (selectedTurret != null && returnToPool == true) { // DISABLING PREVIOUS TURRET
            selectedSlot.DeselectSlot();
            ObjectPoolManager.ReturnObjectToPool(selectedTurret);
        }

        selectedSlot = turrets[i];
        selectedTurret = ObjectPoolManager.SpawnObject(selectedSlot.SelectSlot(), placePoint);
        selectedTurret.transform.localPosition = Vector3.zero;

        UpdateUI();
    }

    private void UpdateUI() {
        // priceText.text = "Cost: " + selectedTurret.GetComponent<IBuilding>().GetPrice();
        descriptionText.text = selectedTurret.GetComponent<IBuilding>().GetDescription();
    }

    public void PlaceTurretInput(InputAction.CallbackContext context) {
        if (CanMakeAction() == false) return;

        //IF PLAYER CAN AFFORD TO BUY BUILDING AND IT'S NOT COLLIDING WITH ANYTHING THEN PLACE BUILDING
        if (context.performed && selectedTurret.GetComponent<IBuilding>().EnableBuilding()) { SelectTurret(selectedIndex); }
    }
    
    public void HideBuilding() {
        // ObjectPoolManager.ReturnObjectToPool(selectedTurret);
        selectedTurret.SetActive(false);
        buildingCanvas.SetActive(false);
    }
    
    public void ShowBuilding() {
        // selectedTurret = ObjectPoolManager.SpawnObject(selectedTurret, placePoint);
        selectedTurret.SetActive(true);
        buildingCanvas.SetActive(true);
    }

    private bool CanMakeAction() {
        if (GameManager.gameState != GameManager.GameState.Ongoing) return false; // IF GAME IS PAUSED REFUSE
        return ModeManager.instance.playerMode == ModeManager.PlayerMode.Building && Cursor.lockState == CursorLockMode.Locked; // ALLOWS WHEN IN BUILDING MODE
    }
}
