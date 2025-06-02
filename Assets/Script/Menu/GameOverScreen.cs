using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    void Start()
    {
        // Show and unlock the cursor so the player can interact with UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Restart the game by reloading the gameplay scene
    public void reStart()
    {
        SceneManager.LoadSceneAsync(1); // Load scene with build index 1
    }

    // Return to main menu
    public void backToMenu()
    {
        SceneManager.LoadScene(0); // Load main menu (scene index 0)
    }

    // Quit the game
    public void Quit()
    {
        Application.Quit(); // This only works in a built application, not in the editor
    }
}
