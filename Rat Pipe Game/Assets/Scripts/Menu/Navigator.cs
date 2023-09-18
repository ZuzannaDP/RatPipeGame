using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    public void SelectLevel() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ReturnMenu() {
        SceneManager.LoadScene("Menu");
    }
}
