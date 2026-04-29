using FSM;
using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Context : MonoBehaviour
{
    [SerializeField] private EnemyInstance _enemyInstance;    
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _hitTime = 0.2f;

    private PlayerController _player;    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CameraShakeManager _cameraShakeManager;

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
            _rigidbody.constraints = RigidbodyConstraints2D.None;

            LookAt(direction);
        }
        else
        {
            StopMove();
        }
    }

    public void FleeTo(Vector3 direction)
    {
        if (!_enemyInstance.IsStunned)
        {
            SelfRigidbody.linearVelocity = direction.normalized * EnemyStat.FleeSpeed;
            _rigidbody.constraints = RigidbodyConstraints2D.None;

            LookAt(direction);
        }
        else
        {
            StopMove();
        }        
    }

    public void LaunchAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }
    
    public void LaunchReactAnimation()
    {
        Debug.Log("Reacting animation");
        _animator.SetTrigger("React");
    }
    
    public void LaunchHitAnimation()
    {
        Debug.Log("Hit animation");
        _cameraShakeManager.ShakeCameraEnemyHit();       
        _animator.SetTrigger("Hit");
    }

    public bool IsPlayingAttackAnimation()
    {
        if(_animator && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                        _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        
         else 
            return false;
    }

    public bool IsPlayingReactAnimation()
    {
        if(_animator && _animator.GetCurrentAnimatorStateInfo(0).IsName("React") &&
           _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else 
            return false;
    }

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = FindFirstObjectByType<PlayerController>();
        _cameraShakeManager = FindFirstObjectByType<CameraShakeManager>();
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
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
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

        Gizmos.color = Color.orangeRed;
        Gizmos.DrawWireSphere(SelfTransform.position, EnemyStat.ExplosionRange);
    }
}
