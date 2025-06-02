using UnityEngine;
using UnityEngine.UI;

public class AK : MonoBehaviour
{
    public float gunDamage;              // Damage per shot
    public float gunRange;              // Maximum shooting distance
    public float fireRate;              // Bullets per second
    public float soundRange;            // Radius for alerting enemies
    public LayerMask enemyLayerMask;    // Layer to detect enemies
    public Text ammoText;               // UI element to show current ammo
    public AudioSource arFire;          // Sound effect when firing
    public AudioSource emptyMag;        // Sound effect when out of ammo
    public Camera fpsCam;               // First-person camera

    private float nextTimeToFire = 0;   // Time check for next allowed shot
    private Transform player;           // Reference to the player's transform
    private Animator gunAnim;           // Animator component for firing animation

    void Start()
    {
        gunAnim = GetComponent<Animator>();

        // Find the player in the scene
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
        // Update ammo count on the UI
        ammoText.text = GlobalAmmo.heavyAmmo.ToString();

        // Handle firing input and cooldown
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (GlobalAmmo.heavyAmmo > 0)
            {
                nextTimeToFire = Time.time + 1 / fireRate;
                Fire();
            }
            else
            {
                // Play empty mag sound only if not already playing
                if (!emptyMag.isPlaying)
                {
                    emptyMag.Play();
                }
            }
        }
    }

    void Fire()
    {
        // Consume one bullet
        GlobalAmmo.heavyAmmo--;

        // Play fire sound and animation
        arFire.Play();
        gunAnim.SetTrigger("Fire");

        // Simulate gunshot sound alerting nearby enemies
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

        // Detect enemies in a horizontal cone using SphereCast
        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.5f, horizontalDirection, gunRange, enemyLayerMask);

        RaycastHit? bestTarget = null;
        float bestDistance = float.MaxValue;

        foreach (RaycastHit h in hits)
        {
            if (h.collider.CompareTag("Enemy"))
            {
                // Perform additional raycast to ensure no obstacles are in the way
                Vector3 toTarget = h.collider.bounds.center - origin;
                Ray rayToTarget = new Ray(origin, toTarget.normalized);
                float distanceToTarget = toTarget.magnitude;

                // Ignore trigger colliders during obstacle check
                if (Physics.Raycast(rayToTarget, out RaycastHit obstacleHit, distanceToTarget, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (!obstacleHit.collider.CompareTag("Enemy"))
                    {
                        continue; // Obstructed by a non-enemy object, skip
                    }
                }

                // No obstruction, consider as valid target
                if (distanceToTarget < bestDistance)
                {
                    bestDistance = distanceToTarget;
                    bestTarget = h;
                }
            }
        }

        // Apply damage to the selected target
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
