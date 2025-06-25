using System.Collections;
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
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject ongoingUI;
    [SerializeField] private HealthManager baseHP;
    
    public IEnumerator EndGame(GameState state) {
        if (gameState != GameState.Ongoing) yield break;
        
        gameState = state;
        
        yield return new WaitForSeconds(1f);
        
        Time.timeScale = 0;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        ongoingUI.SetActive(false);

        GameObject screen = state == GameState.Win ? winScreen : loseScreen;
        
        screen.SetActive(true);
        screen.GetComponent<Animator>().SetTrigger("Show");
        
        
    }
    
    public void PauseGameInput(InputAction.CallbackContext context) {
        if (context.performed) { // ADD THAT WHEN SETTINGS ARE OPEN CLOSE THEM FIRST
            PauseGame();
        }
    }

    public void PauseGame() {
        if (settingsScreen.activeInHierarchy == true) { // Prioritise closing settings before unpausing
            UIButtons.DisableObject(settingsScreen);
            UIButtons.EnableObject(pauseScreen);
            return;
        }

        switch (gameState) {
            case GameState.Ongoing:
                Time.timeScale = 0;
                gameState = GameState.Pause;

                UIButtons.EnableObject(pauseScreen);
                ongoingUI.SetActive(false);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case GameState.Pause:
                Time.timeScale = 1;
                gameState = GameState.Ongoing;

                UIButtons.DisableObject(pauseScreen);
                ongoingUI.SetActive(true);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    public void DealDamageToBase(int i) {
        baseHP.TakeDamage(i);
    }
}
