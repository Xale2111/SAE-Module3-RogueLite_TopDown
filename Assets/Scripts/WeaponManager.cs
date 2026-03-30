using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{   
    [SerializeField] Weapon[] possibleWeapons;

    Weapon currentWeapon;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWeapon = possibleWeapons[Random.Range(0, possibleWeapons.Length)].Equip(transform);
        Debug.Log(currentWeapon.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (currentWeapon != null)
            {
                currentWeapon.LeftClick();
            }
        }
    }

    public void RightClick(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (currentWeapon != null)
            {
                currentWeapon.RightClick();
            }
        }
    }
}
