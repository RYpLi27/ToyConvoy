using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ResetTimeScaleAndGameState() {
        Time.timeScale = 1;
        if (GameManager.instance != null) { GameManager.gameState = GameManager.GameState.Ongoing;}
    }
}
