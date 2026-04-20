using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int Damage;

    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        Destroy(gameObject, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.linearVelocity = transform.up * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyInstance enemy))
        {
            Attack(enemy);
            _speed = 0;
            gameObject.transform.SetParent(enemy.transform);
            Destroy(_collider);
            Destroy(gameObject,4f);
        }
    }
    public void SetDamage(int newDamage)
    {
        Damage = newDamage;
    }

    public virtual void Attack(EnemyInstance enemy)
    {         
    }
}
