using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject optionPanel;
    public void PlayButton()
    {
        SceneChange("GameScene");
    }

    public void ResumeButton()
    {
        SceneChange("MainMenu");
    }

    public void OptionButton()
    {
        optionPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
