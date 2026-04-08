using UnityEngine;

[CreateAssetMenu(fileName = "New SwordMan", menuName = "Enemies/SwordMan")]
public class SwordMan : Enemy
{
    public override void Attack()
    {
        base.Attack();
    }

    public override void Idle()
    {
        base.Idle();
    }
    
    public override void Move()
    {
        base.Move();
    }
    public override void SeePlayer()
    {
        base.SeePlayer();
    }
}
