using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoubleDagger : Weapon
{
    private bool hand;
    public override void LeftClick()
    {
        hand = Random.value < 0.5f ? true : false;
        animator.SetBool("AttackingHand", hand);
        animator.SetTrigger("MainAttack");
        Debug.Log("First attack Double Dagger");
    }

    public override void RightClick()
    {
        animator.SetTrigger("SecondAttack");
        Debug.Log("Second attack Double Dagger");
    }
}
