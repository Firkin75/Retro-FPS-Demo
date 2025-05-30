using UnityEngine;
using UnityEngine.UI;

public class MG : MonoBehaviour
{
    public float gunDamage = 30;
    public float gunRange;
    public float fireRate;
    public float soundRange;
    public LayerMask enemyLayerMask;
    public Text ammoText;
    public AudioSource mgFire;
    public AudioSource emptyMag;
    public Camera fpsCam;

    private float nextTimeToFire = 0;
    private Transform player;
    private Animator gunAnim;

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



    void Update()
    {
        ammoText.text = GlobalAmmo.heavyAmmo.ToString();


        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (GlobalAmmo.heavyAmmo > 0)
            {
                nextTimeToFire = Time.time + 1 / fireRate;
                Fire();
            }
            else
            {
                if (!emptyMag.isPlaying)
                {
                    emptyMag.Play();
                }
            }
        }
    }

    void Fire()
    {
        GlobalAmmo.heavyAmmo--;
        mgFire.Play();
        gunAnim.SetTrigger("Fire");

        // Gunshot sound simulation
        Collider[] enemyColliders = Physics.OverlapSphere(player.position, soundRange, enemyLayerMask);
        foreach (var enemyCollider in enemyColliders)
        {
            EnemyAI ai = enemyCollider.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.OnHeardGunshot();
            }
        }

        // ˮƽ�Զ���׼����
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
                // ����һ�������ж��Ƿ����ϰ���
                Vector3 toTarget = h.collider.bounds.center - origin;
                Ray rayToTarget = new Ray(origin, toTarget.normalized);
                float distanceToTarget = toTarget.magnitude;

                // ���Դ����������߼��
                if (Physics.Raycast(rayToTarget, out RaycastHit obstacleHit, distanceToTarget, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (!obstacleHit.collider.CompareTag("Enemy"))
                    {
                        continue; // ���ǵ��˵�ס������
                    }
                }

                // û���ڵ���������Ϊ��ѡĿ��
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

            }
        }

    }





}
