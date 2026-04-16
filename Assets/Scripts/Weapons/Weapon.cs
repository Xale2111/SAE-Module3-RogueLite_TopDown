using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Weapon_SO WeaponData;
    public int BaseDamage;
    public int Level;
    [SerializeField] protected float _cooldown;

    [SerializeField] protected Animator animator;

    public Weapon Equip()
    {
        this.gameObject.SetActive(true);
        return this;
    }

    //Main Attack
    public virtual void LeftClick()
    {        
    }

    public virtual void ReleaseLeftClick()
    {
    }

    //Second Attack
    public virtual void RightClick()
    {        
    }
    
}
