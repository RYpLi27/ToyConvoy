using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static GameState gameState;
    public enum GameState {
        Ongoing,
        Win,
        Lose,
        Pause
    }
    
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject ongoingUI;
    [SerializeField] private HealthManager baseHP;

    public void EndGame(GameState state) {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        ongoingUI.SetActive(false);
        if (state == GameState.Win) { winScreen.SetActive(true); } 
        else { loseScreen.SetActive(true);}
        
        gameState = state;
    }
    
    public void PauseGameInput(InputAction.CallbackContext context) {
        if (context.performed) {
            PauseGame();
        }
    }

    public void PauseGame() {
        if (gameState == GameState.Ongoing) {
            Time.timeScale = 0;
            gameState = GameState.Pause;
            
            pauseScreen.SetActive(true);
            ongoingUI.SetActive(false);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        } else if (gameState == GameState.Pause) {
            Time.timeScale = 1;
            gameState = GameState.Ongoing;
            
            pauseScreen.SetActive(false);
            ongoingUI.SetActive(true);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void DealDamageToBase(int i) {
        baseHP.TakeDamage(i);
    }
}
