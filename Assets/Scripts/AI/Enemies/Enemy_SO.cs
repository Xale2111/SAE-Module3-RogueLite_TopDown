using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy_SO : ScriptableObject
{
    [SerializeField] public float MoveSpeed;
    [SerializeField] public int Health;
    [SerializeField] public int Damage;
    [SerializeField] public float Weight;
    [SerializeField] public float DetectionRadius;
    [SerializeField] public float AttackRange;
    [SerializeField] public float FleeRange;
    
    protected Animator animator;
}
