using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    

    public float gunDamage = 30;
    public float gunRange = 100;
    public float fireRate = 2f;
    public float soundRange = 50;

  



    public AudioSource pistolFire;
    public AudioSource emptyMag;
    public Camera fpsCam;
    public LayerMask enemyLayerMask;
   /* public Text ammoText;*/
    
    
    [SerializeField]
    private Text ammoText;
    private Animator gunAnim;
    private Transform player;
           
    private float nextTimeToFire = 0;

    

    void Start()
    {
       

        gunAnim = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player GameObject is tagged as 'Player'.");
        }

    }

    


    // Update is called once per frame
    void Update()
    {
        
        ammoText.text = GlobalAmmo.handGunAmmo.ToString();
     

        
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            if (GlobalAmmo.handGunAmmo > 0)
            {
                
                nextTimeToFire = Time.time + 1 / fireRate;
                Fire();

            }

            else {
                if (!emptyMag.isPlaying)
                {
                    emptyMag.Play();
                    Debug.Log("No Ammo!");
                }
            }

            
        }
        

    }
    void Fire()
    {
        GlobalAmmo.handGunAmmo--;
        pistolFire.Play();
        gunAnim.SetTrigger("Fire");

        // 模拟枪声（吸引敌人）
        Collider[] enemyColliders = Physics.OverlapSphere(player.position, soundRange, enemyLayerMask);
        foreach (var enemyCollider in enemyColliders)
        {
            EnemyAI ai = enemyCollider.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.OnHeardGunshot();
            }
        }

        // 水平自动瞄准部分
        Vector3 origin = fpsCam.transform.position;
        Vector3 horizontalDirection = fpsCam.transform.forward;
        horizontalDirection.y = 0;
        horizontalDirection.Normalize();

        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.5f, horizontalDirection, gunRange, enemyLayerMask);

        RaycastHit? bestTarget = null;
        float bestDistance = float.MaxValue;

        foreach (RaycastHit h in hits)
        {
            if (h.collider.CompareTag("Enemy"))
            {
                // 增加一道射线判断是否有障碍物
                Vector3 toTarget = h.collider.bounds.center - origin;
                Ray rayToTarget = new Ray(origin, toTarget.normalized);
                float distanceToTarget = toTarget.magnitude;

                // 忽略触发器的射线检测
                if (Physics.Raycast(rayToTarget, out RaycastHit obstacleHit, distanceToTarget, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (!obstacleHit.collider.CompareTag("Enemy"))
                    {
                        continue; // 被非敌人挡住，跳过
                    }
                }

                // 没被遮挡，可以作为候选目标
                if (distanceToTarget < bestDistance)
                {
                    bestDistance = distanceToTarget;
                    bestTarget = h;
                }
            }
        }

        if (bestTarget.HasValue)
        {
            EnemyAI enemy = bestTarget.Value.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(gunDamage);
                Debug.Log("Auto-aim hit enemy: " + enemy.name);
            }
        }
        else
        {
            Debug.Log("No enemy hit by auto-aim.");
        }
    }



}
