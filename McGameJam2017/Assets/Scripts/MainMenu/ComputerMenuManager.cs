using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ComputerMenuManager : MonoBehaviour {

#if UNITY_STANDALONE
    public GameObject editorMenu;
    public void loadEditorMenu() {
        SceneManager.LoadSceneAsync("SceneMaxMap");
    }
#endif

    public GameObject mainMenu;
    public GameObject creditsMenu;
    public void startGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void creditMenu()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }
    public void returnToMenu()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
