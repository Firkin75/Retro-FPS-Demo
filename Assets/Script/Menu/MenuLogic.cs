using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject settingMenu;
    public GameObject mainMenu;
    public void gameStart()
    {
        SceneManager.LoadScene(1);
        mainMenu.SetActive(false);
    }

    public void ToSettingMenu()
    { 
        settingMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void exitTheGame() { 
    
        Application.Quit();

    }

}
