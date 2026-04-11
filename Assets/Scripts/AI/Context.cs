using System;
using UnityEngine;

public class Context : MonoBehaviour
{
    [SerializeField] private Enemy_SO _enemyStat;

    //[SerializeField] private FsmLeader _leader;
    
    private PlayerController _player;
    public Transform GetPlayerTransform => _player.transform;
    
    public Transform SelfTransform => transform;
    
    private Rigidbody2D _rigidbody;
    
    public Rigidbody2D SelfRigidbody => _rigidbody;

    public BoundsInt GetRoomBounds() => RoomManager.GetCurrentRoomBounds();

    public Enemy_SO EnemyStat => _enemyStat;

    public void MoveTo(Vector3 direction)
    {
        SelfRigidbody.linearVelocity = direction.normalized * _enemyStat.MoveSpeed;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        SelfRigidbody.MoveRotation(angle);
    }


    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = FindFirstObjectByType<PlayerController>();        
        if (!_enemyStat)
        {
            _enemyStat = GetComponent<Enemy_SO>();
        }
    }

    public bool CheckPlayerInGivenRange(float rangeToCheck)
    {
        bool canChase;

        canChase = Vector3.Distance(SelfTransform.position, GetPlayerTransform.position) < rangeToCheck; // if true, player in range
        canChase = canChase ? GetPlayerTransform.position.x > GetRoomBounds().xMin && GetPlayerTransform.position.x < GetRoomBounds().xMax : false; //If true, check player position to be in room, else false

        return canChase;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(SelfTransform.position,EnemyStat.AttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(SelfTransform.position, EnemyStat.DetectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(SelfTransform.position,EnemyStat.FleeRange);
    }
}
