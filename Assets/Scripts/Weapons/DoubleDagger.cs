using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoubleDagger : Weapon
{
    [SerializeField] float _radius;
    [SerializeField] Transform _leftDagger;    
    [SerializeField] Transform _rightDagger;    

    bool _isAttacking = false;

    private bool _hand;
    public override void LeftClick()
    {
        _hand = Random.value < 0.5f ? true : false;
        animator.SetBool("AttackingHand", _hand);
        animator.SetTrigger("MainAttack");
        Debug.Log("First attack Double Dagger");
    }

    public override void RightClick()
    {
        animator.SetTrigger("SecondAttack");
        Debug.Log("Second attack Double Dagger");
    }

    /*
     Add method in animation to set isAttackin = true;
     Add method in animation to set isAttackin = false;

    Check collisions depending on hand
    Create ThrowableDagger item that will be shoot when second attack
    (add a method in animation for the launch)

     */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.rebeccaPurple;

        Gizmos.DrawWireSphere(_leftDagger.position, _radius);
        Gizmos.DrawWireSphere(_rightDagger.position, _radius);
    }
}
