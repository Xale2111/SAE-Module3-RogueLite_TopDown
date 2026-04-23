using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WeaponManager : MonoBehaviour
{   
    [SerializeField] GameObject[] equipedWeapons;
    [SerializeField] private UIDocument playerDataUI;
    [SerializeField] private UIDocument weaponPickUpDataUI;

    
    //WeaponDisplaySprite
    
    
    public GameObject CurrentWeaponGO;
    public Weapon CurrentWeapon;
    int currentMaxLevel = 1;

    private VisualElement equippedWeaponVisualElement;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        equippedWeaponVisualElement = playerDataUI.rootVisualElement.Query("WeaponDisplay").First();
        
        foreach (GameObject weaponsGO in equipedWeapons)
        {
            weaponsGO.SetActive(false);
        }
        CurrentWeapon = GetRandomWeapon().Equip();
        CurrentWeaponGO = CurrentWeapon.gameObject;
        currentMaxLevel++;
        
        equippedWeaponVisualElement.dataSource = CurrentWeapon;

        HidePickUpWeapon();
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
        if (CurrentWeapon != null)
        {
            if (context.ReadValueAsButton())
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

    public void DisplayPickUpWeapon(Weapon pickUpWeapon)
    {
        weaponPickUpDataUI.gameObject.SetActive(true);
        weaponPickUpDataUI.rootVisualElement.dataSource = pickUpWeapon;
    }
        
    public void HidePickUpWeapon()
    {
        weaponPickUpDataUI.gameObject.SetActive(false);
    }
    
    public void EquipCurrentWeapon()
    {
        CurrentWeaponGO.SetActive(false);

        CurrentWeapon = CurrentWeapon.Equip();
        CurrentWeaponGO = CurrentWeapon.gameObject;
        CurrentWeaponGO.SetActive(true);
        
        equippedWeaponVisualElement.dataSource = CurrentWeapon;
        
    }

}
