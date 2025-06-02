using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    public float gunDamage = 30;        // Damage dealt per shot
    public float gunRange = 100;        // Shooting range
    public float fireRate = 2f;         // Shots per second
    public float soundRange = 50;       // Radius where enemies can hear the shot

    public AudioSource pistolFire;      // Sound effect when firing
    public AudioSource emptyMag;        // Sound effect when ammo is empty
    public Camera fpsCam;               // Reference to the player's camera
    public LayerMask enemyLayerMask;    // Layer mask to detect enemies

    [SerializeField]
    private Text ammoText;              // UI Text to show ammo count

    private Animator gunAnim;           // Gun animation controller
    private Transform player;           // Reference to the player

    private float nextTimeToFire = 0;   // Cooldown timer for firing

    void Start()
    {
        gunAnim = GetComponent<Animator>();

        // Find player object by tag
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
        // Update the ammo text on screen
        ammoText.text = GlobalAmmo.handGunAmmo.ToString();

        // Fire when left mouse button is pressed and cooldown allows it
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            if (GlobalAmmo.handGunAmmo > 0)
            {
                nextTimeToFire = Time.time + 1 / fireRate;
                Fire();
            }
            else
            {
                // Play empty sound only once
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
        // Consume ammo
        GlobalAmmo.handGunAmmo--;
        pistolFire.Play();
        gunAnim.SetTrigger("Fire");

        // Simulate gunshot sound to alert nearby enemies
        Collider[] enemyColliders = Physics.OverlapSphere(player.position, soundRange, enemyLayerMask);
        foreach (var enemyCollider in enemyColliders)
        {
            EnemyAI ai = enemyCollider.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.OnHeardGunshot();
            }
        }

        // Horizontal auto-aim implementation
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
                // Additional raycast to check for obstacles
                Vector3 toTarget = h.collider.bounds.center - origin;
                Ray rayToTarget = new Ray(origin, toTarget.normalized);
                float distanceToTarget = toTarget.magnitude;

                // Ignore trigger colliders in obstacle check
                if (Physics.Raycast(rayToTarget, out RaycastHit obstacleHit, distanceToTarget, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (!obstacleHit.collider.CompareTag("Enemy"))
                    {
                        continue; // Obstructed by non-enemy object, skip
                    }
                }

                // Unobstructed - consider as valid target
                if (distanceToTarget < bestDistance)
                {
                    bestDistance = distanceToTarget;
                    bestTarget = h;
                }
            }
        }

        // Apply damage to the best valid target
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
