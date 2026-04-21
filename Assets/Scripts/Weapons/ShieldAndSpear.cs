using UnityEngine;

public class ShieldAndSpear : Weapon
{
    bool isHoldingShield = false;

    [SerializeField] float _radius;
    [SerializeField] Transform _centerSpear;
    [SerializeField] PlayerController player;

    bool _isAttacking = false;



    public override void LeftClick()
    {
        isHoldingShield = true;
        player.IsProtected = isHoldingShield;
        animator.SetBool("HoldShield",isHoldingShield);
        Debug.Log("Holding Shield");
    }
    
    public override void ReleaseLeftClick()
    {
        isHoldingShield = false;
        player.IsProtected = isHoldingShield;

        animator.SetBool("HoldShield",isHoldingShield);

        Debug.Log("Releasing Shield");
        
    }

    public override void RightClick()
    {
        if (!isHoldingShield)
        {
            _isAttacking = true;
            animator.SetTrigger("SecondAttack");
            Debug.Log("Second attack Spear");
        }
    }

    private void Update()
    {
        if (_isAttacking)
        {
            CheckCollision();
        }
    }

    private void CheckCollision()
    {
        RaycastHit2D cast = Physics2D.CircleCast(_centerSpear.position, _radius, Vector2.zero);

        if (cast && cast.collider.TryGetComponent(out EnemyInstance enemy))
        {
            Debug.Log($"Dealing damage : {GetDamage()} to {enemy.name}");
            enemy.TakeDamage(GetDamage());
        }
    }

    public void EndAttacking()
    {
        _isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.rebeccaPurple;

        Gizmos.DrawWireSphere(_centerSpear.position, _radius);        
    }
}
