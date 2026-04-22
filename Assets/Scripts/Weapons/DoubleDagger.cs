using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoubleDagger : Weapon
{
    [SerializeField] float _radius;
    [SerializeField] Transform _leftDagger;    
    [SerializeField] Transform _rightDagger;    
    
    [SerializeField] GameObject _throwableDagger;

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

    public void ThrowDagger()
    {
        SpawnDagger(_leftDagger.up, _leftDagger.position);
        SpawnDagger(_rightDagger.up, _rightDagger.position);
    }

    private void SpawnDagger(Vector3 direction, Vector3 spawnPosition)
    {
        // Pour un jeu 2D, calculer l'angle avec Atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90;
        Quaternion aimDirection = Quaternion.Euler(0, 0, angle);

        GameObject throwableDagger = _throwableDagger;
        Throwable throwableStat = throwableDagger.GetComponent<Throwable>();
        throwableStat.SetDamage(GetDamage());

        Instantiate(throwableDagger, spawnPosition, aimDirection);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.rebeccaPurple;

        Gizmos.DrawWireSphere(_leftDagger.position, _radius);
        Gizmos.DrawWireSphere(_rightDagger.position, _radius);
    }
}
