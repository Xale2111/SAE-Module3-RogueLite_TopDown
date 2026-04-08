using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Context : MonoBehaviour
{
    private PlayerController _player;
    public Transform GetPlayerTransform() => _player.transform;
    
    public Transform SelfTransform => transform;
    public Rigidbody2D SelfRigidbody => GetComponent<Rigidbody2D>();

    private RoomManager _roomManager;
    public BoundsInt GetRoomBounds() => _roomManager.CurrentRoomBounds();
    
    private Transform _leader;
    
    public Transform GetLeaderTransform() => _leader.transform;

    private Enemy _enemy;
    public Enemy GetEnemy() => _enemy;
    
    public void MoveTo(Vector3 direction)
    {
        SelfRigidbody.linearVelocity = direction.normalized * _enemy.MoveSpeed;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        SelfRigidbody.MoveRotation(angle);
    }

    private void OnEnable()
    {
        _player = FindFirstObjectByType<PlayerController>();
        _roomManager = FindFirstObjectByType<RoomManager>();
        _enemy = GetComponent<Enemy>();
    }
}
