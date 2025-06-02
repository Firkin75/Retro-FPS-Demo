using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    // UI references for different menu screens
    public GameObject settingMenu;
    public GameObject mainMenu;
    public GameObject intro;

    // Called once before the first Update
    void Start()
    {
        // Initialize menu visibility
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
        intro.SetActive(false);

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Called when "Start Game" button is pressed
    public void gameStart()
    {
        SceneManager.LoadScene(1); // Load the game scene (Scene index 1)
        mainMenu.SetActive(false);
        settingMenu.SetActive(false);
        intro.SetActive(false);
    }

    // Show the intro screen
    public void toIntro()
    {
        intro.SetActive(true);
        mainMenu.SetActive(false);
        settingMenu.SetActive(false);
    }

    // Show the settings menu
    public void ToSettingMenu()
    {
        settingMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    // Exit the application
    public void exitTheGame()
    {
        Application.Quit(); // Quit the game (only works in built application)
    }

    // Return to the main menu from settings or intro
    public void backToMainMenu()
    {
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
        intro.SetActive(false);
    }
}
