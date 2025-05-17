using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform cameraFollowTransform;

    private float yRotation, xRotation;

    private bool isCursorLocked;

    private Rigidbody rb;

    private HealthManager currentTarget;
    
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        
        isCursorLocked = true;
        
        yRotation = transform.rotation.eulerAngles.y;
        xRotation = transform.rotation.eulerAngles.x;
    }

    private void Update() {
        IsLookingAtEnemy();
    }

    private void IsLookingAtEnemy() {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, StaticVariables.whatIsEnemy))
        {
            HealthManager enemy = hit.collider.GetComponent<HealthManager>();
            if (enemy != null && currentTarget != enemy)
            {
                if (currentTarget != null)
                    currentTarget.ShowBar(false);

                currentTarget = enemy;
                currentTarget.ShowBar(true);
            }
        }
        else if (currentTarget != null)
        {
            currentTarget.ShowBar(false);
            currentTarget = null;
        }
    }
    
    public void MousePositionInput(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector2 mouseInput = context.ReadValue<Vector2>();

            float mouseX = mouseInput.x * sensitivity;
            float mouseY = mouseInput.y * sensitivity;
            
            Rotate(mouseX, mouseY);
        }
    }

    
    private void Rotate(float xRot, float yRot) {
        if (isCursorLocked == false || GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        yRotation += xRot;
        xRotation -= yRot;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

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
}
