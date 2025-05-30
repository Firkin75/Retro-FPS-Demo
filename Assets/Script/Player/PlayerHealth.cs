using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public bool isPlayerAlive = true;
    public int maxHealth = 100; // ��ҵ��������ֵ
    public Text hpText;
    public GameObject ui;
    public AudioSource hitAudio;
    public static int currentHealth; // ��ҵĵ�ǰ����ֵ
   
    [SerializeField]
    private float hitSoundCooldown = 2f; // ��Ч���ż��
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


        // �������ʣ���˺�����������ֵ
        if (remainingDamage > 0)
        {
            currentHealth -= remainingDamage; // ��������ֵ

            // �������Ƿ�����
            if (currentHealth <= 0)
            {
                Die();
                ui.SetActive(false);
            }
        }

       
    }
    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // ���ӻ���ֵ�������ܳ������ֵ
    }
   

    private void Die()
    {
        
        Debug.Log("Player has died.");
        SceneManager.LoadScene(1);
    }
}