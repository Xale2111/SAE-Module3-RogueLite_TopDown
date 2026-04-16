using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int Damage;
    public float StunDuration;

    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.linearVelocity = transform.up * _speed;
    }
}
