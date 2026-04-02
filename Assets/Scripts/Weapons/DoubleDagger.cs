using System;
using UnityEngine;

public class DoubleDagger : Weapon
{
    private bool attackingDagger = false; //False = left hand, True = right hand 
    
    public override void LeftClick()
    {
        animator.SetBool("AttackingDagger", attackingDagger);
        animator.SetTrigger("MainAttack");
        attackingDagger = !attackingDagger;
        Debug.Log("First attack Double Dagger");
    }

    public override void RightClick()
    {
        animator.SetTrigger("SecondAttack");
        Debug.Log("Second attack Double Dagger");
    }
}
