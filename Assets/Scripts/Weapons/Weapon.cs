using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Weapon_SO WeaponData;
    public int BaseDamage;
    public int Level;
    [SerializeField] protected float coolDown;

    protected Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public Weapon Equip(Transform parent)
    {
        Instantiate(this, parent);
        return this;
    }

    //Main Attack
    public virtual void LeftClick()
    {        
    }


    //Second Attack
    public virtual void RightClick()
    {        
    }
}
