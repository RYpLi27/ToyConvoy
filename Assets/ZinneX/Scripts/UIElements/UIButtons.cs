using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void LoadScene(string sceneName) {
        DOTween.Clear();
        DOTween.SetTweensCapacity(400, 200);
        SceneManager.LoadScene(sceneName);
    }

    public static void DisableObject(GameObject obj) {
        if (obj.TryGetComponent(out Animator anim)) {
            anim.ResetTrigger("Show");
            anim.SetTrigger("Hide");
        } else {
            obj.SetActive(false);
        }
    }

    public static void EnableObject(GameObject obj) {
        obj.SetActive(true);
        if (obj.TryGetComponent(out Animator anim)) {
            anim.ResetTrigger("Hide");
            anim.SetTrigger("Show");
            obj.SetActive(true);
        }
    }
    
    public void ExitGame() {
        Application.Quit();
    }

    public void ResetTimeScaleAndGameState() {
        Time.timeScale = 1;
        if (GameManager.instance != null) { GameManager.gameState = GameManager.GameState.Ongoing;}
    }
}
