using System;
using UnityEngine;

public class Context : MonoBehaviour
{
    [SerializeField] private Enemy_SO _enemyStat;

    //[SerializeField] private FsmLeader _leader;
    
    private PlayerController _player;
    public Transform GetPlayerTransform() => _player.transform;
    
    public Transform SelfTransform => transform;
    
    private Rigidbody2D _rigidbody;
    
    public Rigidbody2D SelfRigidbody => _rigidbody;

    private RoomManager _roomManager;
    public BoundsInt GetRoomBounds() => _roomManager.CurrentRoomBounds();


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
        _roomManager = FindFirstObjectByType<RoomManager>();
        if (!_enemyStat)
        {
            _enemyStat = GetComponent<Enemy_SO>();
        }
    }
}
