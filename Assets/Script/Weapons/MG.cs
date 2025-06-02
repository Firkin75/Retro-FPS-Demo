using UnityEngine;
using UnityEngine.UI;

public class MG : MonoBehaviour
{
    public float gunDamage = 30;         // Damage dealt per shot
    public float gunRange;               // Firing range
    public float fireRate;               // Rate of fire (shots per second)
    public float soundRange;             // Radius within which enemies can hear the shot
    public LayerMask enemyLayerMask;     // Layer mask to filter enemies
    public Text ammoText;                // UI element to show ammo count
    public AudioSource mgFire;           // Sound played when firing
    public AudioSource emptyMag;         // Sound played when out of ammo
    public Camera fpsCam;                // Reference to the player's camera

    private float nextTimeToFire = 0;    // Time until next shot is allowed
    private Transform player;            // Reference to the player transform
    private Animator gunAnim;            // Animator for firing animation

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
        // Update ammo UI
        ammoText.text = GlobalAmmo.heavyAmmo.ToString();

        // Handle firing input and rate limit
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (GlobalAmmo.heavyAmmo >= 2)
            {
                nextTimeToFire = Time.time + 1 / fireRate;
                Fire();
            }
            else
            {
                // Play empty sound if out of ammo
                if (!emptyMag.isPlaying)
                {
                    emptyMag.Play();
                }
            }
        }
    }

    void Fire()
    {
        // Consume ammo
        GlobalAmmo.heavyAmmo -= 2;

        // Play firing sound and animation
        mgFire.Play();
        gunAnim.SetTrigger("Fire");

        // Gunshot sound simulation - alert enemies within range
        Collider[] enemyColliders = Physics.OverlapSphere(player.position, soundRange, enemyLayerMask);
        foreach (var enemyCollider in enemyColliders)
        {
            EnemyAI ai = enemyCollider.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.OnHeardGunshot();
            }
        }

        // Horizontal auto-aim logic
        Vector3 origin = fpsCam.transform.position;
        Vector3 horizontalDirection = fpsCam.transform.forward;
        horizontalDirection.y = 0;
        horizontalDirection.Normalize();

        // SphereCast to find enemies within horizontal cone
        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.5f, horizontalDirection, gunRange, enemyLayerMask);

        RaycastHit? bestTarget = null;
        float bestDistance = float.MaxValue;

        foreach (RaycastHit h in hits)
        {
            if (h.collider.CompareTag("Enemy"))
            {
                // Check for obstacles between player and enemy
                Vector3 toTarget = h.collider.bounds.center - origin;
                Ray rayToTarget = new Ray(origin, toTarget.normalized);
                float distanceToTarget = toTarget.magnitude;

                // Raycast that ignores trigger colliders
                if (Physics.Raycast(rayToTarget, out RaycastHit obstacleHit, distanceToTarget, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (!obstacleHit.collider.CompareTag("Enemy"))
                    {
                        continue; // Target is blocked by a non-enemy object
                    }
                }

                // Not obstructed - consider as valid target
                if (distanceToTarget < bestDistance)
                {
                    bestDistance = distanceToTarget;
                    bestTarget = h;
                }
            }
        }

        // Apply damage to the best visible target
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
