using UnityEngine;

public class DoubleAxe : Weapon
{
    [SerializeField] float _radius;
    [SerializeField] Transform _centerWeapon;

    bool _isAttacking = false;

    public override void LeftClick()
    {
        _isAttacking = true;
        animator.SetTrigger("MainAttack");
        Debug.Log("First attack Double Axe");
    }

    public override void RightClick()
    {
        _isAttacking = true;
        animator.SetTrigger("SecondAttack");
        Debug.Log("Second attack Double Axe");
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
        RaycastHit2D cast = Physics2D.CircleCast(_centerWeapon.position,_radius,Vector2.zero);

        if(cast && cast.collider.TryGetComponent(out EnemyInstance enemy))
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

        Gizmos.DrawWireSphere(_centerWeapon.position, _radius);
    }
}
