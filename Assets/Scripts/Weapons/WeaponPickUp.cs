using UnityEngine;
using UnityEngine.UIElements;

public class WeaponPickUp : InteractItem
{
    SpriteRenderer spriteRenderer;
    Weapon weapon;

    UIDocument weaponDocument;    

    WeaponManager weaponManager;


    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var UIDocs = FindObjectsByType<UIDocument>(FindObjectsSortMode.None);
        foreach (var doc in UIDocs)
        {
            if (doc.CompareTag("UI_WeaponDetails"))
            {
                weaponDocument = doc;
                break;
            }
        }

        
        
        weaponManager = FindFirstObjectByType<WeaponManager>();
        weaponDocument.rootVisualElement.visible = false;

        animator = GetComponent<Animator>();
        
        SetWeapon();
    }

    public override void OnPickUp()
    {
        (weapon, weaponManager.CurrentWeapon) = (weaponManager.CurrentWeapon, weapon);
        weaponDocument.rootVisualElement.dataSource = weapon;
        spriteRenderer.sprite = weapon.WeaponData.pickUpSprite;
        weaponManager.EquipCurrentWeapon();
         
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
            //Show weapon data card and update with current weapon infos
            weaponDocument.rootVisualElement.visible = true;
            weaponDocument.rootVisualElement.dataSource = weapon;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Hide weapon data card
            weaponDocument.rootVisualElement.visible = false;
        }
    }
}
