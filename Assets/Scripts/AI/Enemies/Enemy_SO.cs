using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy_SO : ScriptableObject
{
    [SerializeField] public float MoveSpeed;
    [SerializeField] public float FleeSpeed;
    [SerializeField] public int Health;
    [SerializeField] public int Damage;
    [SerializeField] public float Weight;
    [SerializeField] public int MaxAmountPerRoom;
    [SerializeField] public float DetectionRadius;
    [SerializeField] public float AttackRange;
    [SerializeField] public float FleeRange;
    [SerializeField] public float ExplosionRange;
}
