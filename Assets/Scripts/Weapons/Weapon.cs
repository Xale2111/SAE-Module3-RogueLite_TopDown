using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Weapon_SO WeaponData;
    public int BaseDamage;
    public int Level;
    [SerializeField] protected float coolDown;

    [SerializeField] protected Animator animator;

    public Weapon Equip(Transform parent)
    {
        Weapon instance = Instantiate(this, parent);
        return instance;
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
