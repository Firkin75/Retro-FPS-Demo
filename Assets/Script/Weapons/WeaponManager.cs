using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>(); // **固定槽位的武器列表**
    public Transform weaponHolder; // 武器的存放位置
    public AudioSource pickupSound;

    private int currentWeaponIndex = 0; // 当前武器索引
    private const int maxSlots = 4;  // **设定武器槽位数量**


    void Start()
    {
        // **初始化 weapons，创建 5 个空槽**
        weapons = new List<GameObject>(new GameObject[maxSlots]);

        // 默认武器放入 slot 0
        if (weaponHolder.childCount > 0)
        {
            GameObject defaultWeapon = weaponHolder.GetChild(0).gameObject; // 默认武器
            defaultWeapon.SetActive(true);
            weapons[0] = defaultWeapon;
        }

        SelectWeapon(0); // **确保默认武器可用**
    }

    void Update()
    {
        if (IsWeaponAnimating("Reload") || IsWeaponAnimating("Fire") || IsWeaponAnimating("Equip")) return;

        // **鼠标滚轮切换武器**
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            NextWeapon();
        }
        else if (scroll < 0f)
        {
            PreviousWeapon();
        }

        // **数字键切换武器**
        for (int i = 0; i < maxSlots; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && weapons[i] != null)
            {
                if (currentWeaponIndex == i) return; // **如果已经是当前武器，不切换**
                SelectWeapon(i);
            }
        }
    }

    void NextWeapon()
    {
        if (weapons.Where(w => w != null).Count() <= 1) return; // 只有 1 把武器时不切换

        int startIndex = currentWeaponIndex;
        int nextIndex = (currentWeaponIndex + 1) % maxSlots;

        while (weapons[nextIndex] == null)
        {
            nextIndex = (nextIndex + 1) % maxSlots;
            if (nextIndex == startIndex) return; // 如果遍历一圈都没找到武器，就不切换
        }

        SelectWeapon(nextIndex);
    }

    void PreviousWeapon()
    {
        if (weapons.Where(w => w != null).Count() <= 1) return; // 只有 1 把武器时不切换

        int startIndex = currentWeaponIndex;
        int prevIndex = (currentWeaponIndex - 1 + maxSlots) % maxSlots;

        while (weapons[prevIndex] == null)
        {
            prevIndex = (prevIndex - 1 + maxSlots) % maxSlots;
            if (prevIndex == startIndex) return; // 如果遍历一圈都没找到武器，就不切换
        }

        SelectWeapon(prevIndex);
    }



    void SelectWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count || weapons[index] == null)
            return; // 防止无效索引

        // **先禁用当前武器**
        if (currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count && weapons[currentWeaponIndex] != null)
        {
            weapons[currentWeaponIndex].SetActive(false);
        }

        // **切换到新武器**
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].SetActive(true);

        // **这里调用 ResetWeaponAnimation()，确保对象是激活状态**
        ResetWeaponAnimation(weapons[currentWeaponIndex]);
    }


    bool IsWeaponAnimating(string animationName)
    {
        if (weapons[currentWeaponIndex] == null) return false;

        GameObject weapon = weapons[currentWeaponIndex];
        Animator animator = weapon.GetComponent<Animator>();

        if (animator == null) return false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName);
    }

    // **拾取武器并放入指定槽位**
    public void PickupWeapon(string weaponName, int slotIndex)
    {
        if (slotIndex >= maxSlots || slotIndex < 0)
        {

            return;
        }

        // **如果已有相同武器，则不重复添加**
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null && weapons[i].name == weaponName)
            {

                return;
            }
        }

        // **找到 UI 里的武器对象**
        Transform newWeaponTransform = weaponHolder.Find(weaponName);
        if (newWeaponTransform != null)
        {
            GameObject newWeapon = newWeaponTransform.gameObject;

            // **替换指定槽位的武器**
            weapons[slotIndex] = newWeapon;
            newWeapon.SetActive(false);

            pickupSound.Play();

            SelectWeapon(slotIndex);
        }
        else
        {
            Debug.LogWarning("武器 " + weaponName + " 不在 UI 里！");
        }
    }
    void ResetWeaponAnimation(GameObject weapon)
    {
        if (weapon == null) return; // 避免 null 错误

        Animator animator = weapon.GetComponent<Animator>();
        if (animator != null)
        {
            bool wasInactive = !weapon.activeSelf; // 记录对象是否是非激活的

            if (wasInactive)
            {
                weapon.SetActive(true); // **暂时激活对象**
            }

            if (animator.HasState(0, Animator.StringToHash("Equip"))) // 检查 "Idle" 是否存在
            {
                animator.Play("Equip", 0);
            }
            else
            {
                Debug.LogWarning($"Animator 没有找到 'equip' 状态，跳过重置动画");
            }

            animator.Update(0); // **确保动画状态更新**

            if (wasInactive)
            {
                weapon.SetActive(false); // **恢复原来的激活状态**
            }
        }
    }



}