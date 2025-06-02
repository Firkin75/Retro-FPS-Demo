using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>(); // List of fixed weapon slots
    public Transform weaponHolder;  // Where the weapons are stored in hierarchy
    public AudioSource pickupSound; // Sound to play on weapon pickup

    private int currentWeaponIndex = 0; // Current selected weapon index
    private const int maxSlots = 4;     // Total number of weapon slots

    void Start()
    {
        // Initialize the weapon list with empty slots
        weapons = new List<GameObject>(new GameObject[maxSlots]);

        // Assign default weapon to slot 0 if any child exists
        if (weaponHolder.childCount > 0)
        {
            GameObject defaultWeapon = weaponHolder.GetChild(0).gameObject;
            defaultWeapon.SetActive(true);
            weapons[0] = defaultWeapon;
        }

        // Ensure default weapon is selected
        SelectWeapon(0);
    }

    void Update()
    {
        // Prevent weapon switching during animation
        if (IsWeaponAnimating("Reload") || IsWeaponAnimating("Fire") || IsWeaponAnimating("Equip")) return;

        // Mouse scroll wheel weapon switching
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            NextWeapon();
        }
        else if (scroll < 0f)
        {
            PreviousWeapon();
        }

        // Switch weapons with number keys (1~4)
        for (int i = 0; i < maxSlots; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && weapons[i] != null)
            {
                if (currentWeaponIndex == i) return; // Do nothing if already holding this weapon
                SelectWeapon(i);
            }
        }
    }

    void NextWeapon()
    {
        // If only one weapon is available, don’t switch
        if (weapons.Where(w => w != null).Count() <= 1) return;

        int startIndex = currentWeaponIndex;
        int nextIndex = (currentWeaponIndex + 1) % maxSlots;

        // Loop until a valid weapon is found or looped back to start
        while (weapons[nextIndex] == null)
        {
            nextIndex = (nextIndex + 1) % maxSlots;
            if (nextIndex == startIndex) return;
        }

        SelectWeapon(nextIndex);
    }

    void PreviousWeapon()
    {
        // If only one weapon is available, don’t switch
        if (weapons.Where(w => w != null).Count() <= 1) return;

        int startIndex = currentWeaponIndex;
        int prevIndex = (currentWeaponIndex - 1 + maxSlots) % maxSlots;

        // Loop until a valid weapon is found or looped back to start
        while (weapons[prevIndex] == null)
        {
            prevIndex = (prevIndex - 1 + maxSlots) % maxSlots;
            if (prevIndex == startIndex) return;
        }

        SelectWeapon(prevIndex);
    }

    void SelectWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count || weapons[index] == null)
            return;

        // Disable current weapon if exists
        if (currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count && weapons[currentWeaponIndex] != null)
        {
            weapons[currentWeaponIndex].SetActive(false);
        }

        // Switch to the selected weapon
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].SetActive(true);

        // Reset animation state to "Equip"
        ResetWeaponAnimation(weapons[currentWeaponIndex]);
    }

    // Check if the current weapon is in the middle of a specific animation
    bool IsWeaponAnimating(string animationName)
    {
        if (weapons[currentWeaponIndex] == null) return false;

        GameObject weapon = weapons[currentWeaponIndex];
        Animator animator = weapon.GetComponent<Animator>();

        if (animator == null) return false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName);
    }

    // Pickup a weapon and assign it to the specified slot
    public void PickupWeapon(string weaponName, int slotIndex)
    {
        if (slotIndex >= maxSlots || slotIndex < 0)
        {
            return;
        }

        // Avoid adding duplicate weapons
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null && weapons[i].name == weaponName)
            {
                return;
            }
        }

        // Find the weapon by name under the weaponHolder
        Transform newWeaponTransform = weaponHolder.Find(weaponName);
        if (newWeaponTransform != null)
        {
            GameObject newWeapon = newWeaponTransform.gameObject;

            // Replace the weapon in the specified slot
            weapons[slotIndex] = newWeapon;
            newWeapon.SetActive(false);

            pickupSound.Play();

            SelectWeapon(slotIndex);
        }
        else
        {
            Debug.LogWarning("Weapon " + weaponName + " not found under weaponHolder!");
        }
    }

    // Reset weapon animation to "Equip" state
    void ResetWeaponAnimation(GameObject weapon)
    {
        if (weapon == null) return;

        Animator animator = weapon.GetComponent<Animator>();
        if (animator != null)
        {
            bool wasInactive = !weapon.activeSelf;

            // Temporarily activate weapon to reset animation state
            if (wasInactive)
            {
                weapon.SetActive(true);
            }

            if (animator.HasState(0, Animator.StringToHash("Equip")))
            {
                animator.Play("Equip", 0);
            }
            else
            {
                Debug.LogWarning($"Animator does not have 'Equip' state, skipping reset.");
            }

            animator.Update(0); // Force animation update

            if (wasInactive)
            {
                weapon.SetActive(false); // Restore original active state
            }
        }
    }
}
