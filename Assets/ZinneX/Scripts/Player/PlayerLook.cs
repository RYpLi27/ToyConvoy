using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{   
    [SerializeField] private float zoomSensitivityMult;
    [SerializeField] private Transform cameraFollowTransform;
    private float sensitivity; // is set in menu
    public float Sensitivity { set => sensitivity = value; }

    private float yRotation, xRotation;

    private bool isCursorLocked;
    public static bool isZoomed;

    private Rigidbody rb;

    private HealthManager currentTarget;
    private InteractTrigger currentBuilding;

    
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        
        isCursorLocked = true;
        ApplySens();
        
        yRotation = transform.rotation.eulerAngles.y;
        xRotation = transform.rotation.eulerAngles.x;
    }

    public void ApplySens() {
        sensitivity = PlayerPrefs.GetFloat("sens", .5f);
    }
    
    private void FixedUpdate() {
        HandleLook();
    }

    private void HandleLook() {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (BuildingLook(ray) == true) return;
        EnemyLook(ray);
    }

    private bool BuildingLook(Ray ray) {
        if (Physics.Raycast(ray, out RaycastHit buildingHit, 15f, StaticVariables.whatIsInteractable, QueryTriggerInteraction.Collide)) {
            InteractTrigger building = buildingHit.collider.GetComponent<InteractTrigger>();
            if (building != null && currentBuilding != building) {
                currentBuilding?.ShowPrompt(false);
                currentBuilding = building;
                currentBuilding.ShowPrompt(true);
                return true;
            }
        } else {
            currentBuilding?.ShowPrompt(false);
            currentBuilding = null;
        }

        return false;
    }

    private void EnemyLook(Ray ray) {
        if (Physics.Raycast(ray, out RaycastHit enemyHit, Mathf.Infinity, StaticVariables.whatIsEnemy, QueryTriggerInteraction.Ignore)) {
            HealthManager enemy = enemyHit.collider.GetComponentInParent<HealthManager>();
            if (enemy != null && currentTarget != enemy)
            {
                if (currentTarget != null)
                    currentTarget.ShowBar(false);

                currentTarget = enemy;
                currentTarget.ShowBar(true);
            }
        } else if (currentTarget != null) {
            currentTarget.ShowBar(false);
            currentTarget = null;
        }
    }
    
    public void MousePositionInput(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector2 mouseInput = context.ReadValue<Vector2>();

            float mouseX = mouseInput.x * sensitivity;
            float mouseY = mouseInput.y * sensitivity;

            if (isZoomed == true) {
                mouseY *= zoomSensitivityMult;
                mouseX *= zoomSensitivityMult;
            }
            
            Rotate(mouseX, mouseY);
        }
    }

    
    private void Rotate(float xRot, float yRot) {
        if (isCursorLocked == false || GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        yRotation += xRot;
        xRotation -= yRot;
        xRotation = Mathf.Clamp(xRotation, -85, 85);

        rb.MoveRotation(Quaternion.Euler(0f, yRotation, 0f)); // HORIZONTAL ROTATION
        cameraFollowTransform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // VERTICAL ROTATION
    }
    
    public void LockCursorInput(InputAction.CallbackContext context) {
        if (GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        if (context.performed) {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isCursorLocked = false;
        }
        
        if (context.canceled) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorLocked = true;
        }
    }

    public void UpgradeTurret(InputAction.CallbackContext context) {
        if (context.performed && currentBuilding != null) currentBuilding.Interact();
    }
}
