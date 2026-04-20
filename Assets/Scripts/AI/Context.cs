using FSM;
using System;
using UnityEngine;

public class Context : MonoBehaviour
{
    [SerializeField] private EnemyInstance _enemyInstance;    
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _hitTime = 0.2f;

    private PlayerController _player;    
    private Rigidbody2D _rigidbody;

    public float HitTime => _hitTime;
    public Transform GetPlayerTransform => _player.transform;
    
    public Transform SelfTransform => transform;    
    
    public Rigidbody2D SelfRigidbody => _rigidbody;

    public BoundsInt GetRoomBounds() => RoomManager.GetCurrentRoomBounds();

    public Enemy_SO EnemyStat => _enemyInstance.GetEnemyBaseStat();   
    public EnemyInstance EnemyInstance => _enemyInstance;

    public void MoveTo(Vector3 direction)
    {
        if (!_enemyInstance.IsStunned)
        {
            SelfRigidbody.linearVelocity = direction.normalized * EnemyStat.MoveSpeed;

            LookAt(direction);
        }
        else
        {
            StopMove();
        }
    }

    public void FleeTo(Vector3 direction)
    {
        SelfRigidbody.linearVelocity = direction.normalized * EnemyStat.FleeSpeed;

        LookAt(direction);
    }

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = FindFirstObjectByType<PlayerController>();
        if (!_enemyInstance)
        {         
            _enemyInstance = GetComponent<EnemyInstance>();        
        }
    }

    public bool CheckPlayerInGivenRange(float rangeToCheck)
    {
        bool canChase;

        canChase = Vector3.Distance(SelfTransform.position, GetPlayerTransform.position) < rangeToCheck; // if true, player in range
        canChase = canChase ? GetPlayerTransform.position.x > GetRoomBounds().xMin && GetPlayerTransform.position.x < GetRoomBounds().xMax : false; //If true, check player position to be in room, else false

        return canChase;
    }

    public void StopMove()
    { 
        _rigidbody.linearVelocity = Vector3.zero;
    }

    public void KillEntity()
    { 
        gameObject.SetActive(false);
    }

    public void LookAt(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = SelfRigidbody.rotation;

        float smoothAngle = Mathf.LerpAngle(
            currentAngle,
            targetAngle,
            _rotationSpeed * Time.fixedDeltaTime
        );

        SelfRigidbody.MoveRotation(smoothAngle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(SelfTransform.position, EnemyStat.AttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(SelfTransform.position, EnemyStat.DetectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(SelfTransform.position, EnemyStat.FleeRange);
    }
}
