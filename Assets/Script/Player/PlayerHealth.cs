using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public bool isPlayerAlive = true;
    public int maxHealth = 100; // 玩家的最大生命值
    public Text hpText;
    public GameObject ui;
    public AudioSource hitAudio;
    public static int currentHealth; // 玩家的当前生命值
   
    [SerializeField]
    private float hitSoundCooldown = 2f; // 音效播放间隔
    private float lastHitTime = -1f;
 
    
    void Start()
    {
        currentHealth = maxHealth;  
    }

    void Update()
    {
        hpText.text = currentHealth.ToString();

     
    }

    public void TakeDamage(int damage)
    {
        int remainingDamage = damage;
        if (Time.time - lastHitTime >= hitSoundCooldown)
        {
            hitAudio.Play();
            lastHitTime = Time.time;
        }


        // 如果还有剩余伤害，减少生命值
        if (remainingDamage > 0)
        {
            currentHealth -= remainingDamage; // 减少生命值

            // 检查玩家是否死亡
            if (currentHealth <= 0)
            {
                Die();
                ui.SetActive(false);
            }
        }

       
    }
    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // 增加护甲值，但不能超过最大值
    }
   

    private void Die()
    {
        
        Debug.Log("Player has died.");
        SceneManager.LoadScene(1);
    }
}