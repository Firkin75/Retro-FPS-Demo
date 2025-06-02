using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;    // Reference to the player object
    public GameObject pm;        // Pause menu UI panel
    public GameObject fps;       // FPS UI (e.g., crosshair, stats)
    public GameObject weaponUI;  // Weapon UI (e.g., ammo, icons)

    private bool isPaused = false; // Tracks pause state

    void Start()
    {
        Time.timeScale = 1f;
        pm.SetActive(false); // Ensure pause menu is hidden at start
    }

    void Update()
    {
        // Toggle pause menu with ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // Quit the game (note: only works in a built executable)
    public void exitTheGame()
    {
        Application.Quit();
    }

    // Return to main menu (scene index 0)
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Restart the current game (scene index 1)
    public void reStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Pause the game
    public void PauseGame()
    {
        pm.SetActive(true);                     // Show pause menu
        Time.timeScale = 0f;                    // Freeze game time
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true;

        // Disable player control
        player.GetComponent<PlayerController>().enabled = false;

        isPaused = true;

        // Hide in-game UI elements
        fps.SetActive(false);
        weaponUI.SetActive(false);
    }

    // Resume the game
    public void ResumeGame()
    {
        pm.SetActive(false);                    // Hide pause menu
        Time.timeScale = 1f;                    // Resume time
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable player control
        player.GetComponent<PlayerController>().enabled = true;

        isPaused = false;

        // Reactivate in-game UI
        fps.SetActive(true);
        weaponUI.SetActive(true);
    }
}
