using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{   
    [SerializeField] Weapon[] possibleWeapons;    

    public Weapon CurrentWeapon;
    int currentMaxLevel = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        CurrentWeapon = GetRandomWeapon().Equip(transform);
        currentMaxLevel++;   
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.LeftClick();
            }
        }
    }

    public void RightClick(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.RightClick();
            }
        }
    }

    public Weapon GetRandomWeapon()
    {
        var newWeapon = possibleWeapons[Random.Range(0, possibleWeapons.Length)];
        
        return newWeapon;
    }

    public void EquipCurrentWeapon()
    {
        Destroy(transform.GetChild(0).gameObject);

        CurrentWeapon = CurrentWeapon.Equip(transform);

    }

}
