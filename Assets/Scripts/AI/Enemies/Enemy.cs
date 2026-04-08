using UnityEngine;

public enum Enemy_Type
{
    SwordMan,
    Archer,
    Kamikaze,
    Leader,
    Follower,
    Boss
}


public class Enemy : MonoBehaviour
{
    [SerializeField] public float MoveSpeed;
    [SerializeField] public int Health;
    [SerializeField] public int Damage;
    [SerializeField] public Enemy_Type Type;
    [SerializeField] public float Weight;
    [SerializeField] public float DetectionRadius;
    [SerializeField] public float AttackRange;
    
    protected Animator animator;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Idle()
    {
    }

    public virtual void SeePlayer()
    {
    }

    public virtual void Move()
    {
    }
    
    public virtual void Attack()
    {
    }
    
    private void OnDrawGizmo()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, DetectionRadius);
    }




}
