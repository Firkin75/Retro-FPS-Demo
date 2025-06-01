using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Cursor.visible = true;               // ��ʾ���
        Cursor.lockState = CursorLockMode.None;  // ������꣬���������ƶ�
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
