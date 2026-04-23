using UnityEngine;
using UnityEngine.UIElements;

public class WeaponPickUp : InteractItem
{
    SpriteRenderer spriteRenderer;
    Weapon weapon;
    
    WeaponManager weaponManager;


    void Start()
    {
        weaponManager = FindFirstObjectByType<WeaponManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        SetWeapon();
    }

    public override void OnPickUp()
    {
        (weapon, weaponManager.CurrentWeapon) = (weaponManager.CurrentWeapon, weapon);
        spriteRenderer.sprite = weapon.WeaponData.pickUpSprite;
        weaponManager.EquipCurrentWeapon();
        weaponManager.DisplayPickUpWeapon(weapon);
        
        animator.SetBool("PickedUp", true);
        
        Debug.Log("Pick up Weapon");
    }

    private void SetWeapon()
    {
        weapon = weaponManager.GetRandomWeapon();
        spriteRenderer.sprite = weapon.WeaponData.pickUpSprite;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            weaponManager.DisplayPickUpWeapon(weapon);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            weaponManager.HidePickUpWeapon();
        }
    }
}
