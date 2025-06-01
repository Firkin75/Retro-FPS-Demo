using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Cursor.visible = true;               // 显示鼠标
        Cursor.lockState = CursorLockMode.None;  // 解锁鼠标，让它可以移动
    }

    public void reStart()
    {

        SceneManager.LoadSceneAsync(1);
    
    }

    public void backToMenu()
    { 
    
        SceneManager.LoadScene(0);

    }

    public void Quit()
    {
        Application.Quit();
    }
}
