using UnityEngine;
using UnityEngine.SceneManagement;
public class MissionComplete : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            SceneManager.LoadScene(3);
        
        
        }
    }




}
