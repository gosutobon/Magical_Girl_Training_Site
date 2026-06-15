using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuRoot;

    public TargetManager targetManager;

    void Start()
    {
        menuRoot.SetActive(false);
    }

    public void ToggleMenu()
    {
        menuRoot.SetActive(
            !menuRoot.activeSelf);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("S1");
    }

    public void RespawnTargets()
    {
        targetManager.RespawnAllTargets();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}