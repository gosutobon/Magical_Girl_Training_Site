using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public TargetManager targetManager;

    private void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    public void ToggleMenu()
    {
        if (pauseMenu == null) return;

        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("S1");
    }

    public void RespawnTargets()
    {
        if (targetManager != null)
            targetManager.RespawnAllTargets();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}