using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{   
    [SerializeField] GameObject[] equipedWeapons;    

    public GameObject CurrentWeaponGO;
    public Weapon CurrentWeapon;
    int currentMaxLevel = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject weaponsGO in equipedWeapons)
        {
            weaponsGO.SetActive(false);
        }
        CurrentWeapon = GetRandomWeapon().Equip();
        CurrentWeaponGO = CurrentWeapon.gameObject;
        currentMaxLevel++;   
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        if (CurrentWeapon != null)
        {
            if (context.ReadValueAsButton())
            {

                CurrentWeapon.LeftClick();

            }
            else
            {
                CurrentWeapon.ReleaseLeftClick();
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
        var newWeapon = equipedWeapons[Random.Range(0, equipedWeapons.Length)];

        return newWeapon.GetComponent<Weapon>();
    }

    public void EquipCurrentWeapon()
    {
        CurrentWeaponGO.SetActive(false);

        CurrentWeapon = CurrentWeapon.Equip();
        CurrentWeaponGO = CurrentWeapon.gameObject;
        CurrentWeaponGO.SetActive(true);

    }

}
