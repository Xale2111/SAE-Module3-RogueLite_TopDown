using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float protectedSpeed = 1.5f;
    [SerializeField] private GameObject aimLookAt;
    [SerializeField] private float rotationSpeed = 10f;
    
    private Rigidbody2D rb;
    private float _interactCooldown = 0.5f;

    private int _hp = 100;


    Vector2 move;

    public bool Interacted =false;
    private bool _canInteract = true;

    public bool IsProtected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Protection is : " + IsProtected);
    }
    void FixedUpdate()
    {
        float currentSpeed = IsProtected ? protectedSpeed : speed;
        rb.linearVelocity = move * currentSpeed;
    
        if (aimLookAt != null)
        {
            Vector2 direction = aimLookAt.transform.position - transform.position;
        
            if (direction.magnitude > 0.01f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            
                rb.rotation = Mathf.LerpAngle(rb.rotation, angle, Time.deltaTime*rotationSpeed);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (_canInteract)
        {
            StartCoroutine(CooldownInteract());
            if (context.ReadValueAsButton())
            {
                Interacted = true;
                Debug.Log("Interact");
            }
            else
            {
                Interacted = false;
            }
            
        }
    }

    public void TakeDamage(int damageToDeal)
    {
        if (!IsProtected)
        {
            _hp -= damageToDeal;
        }
    }

    private IEnumerator CooldownInteract()
    {
        _canInteract = false;
        yield return new WaitForSeconds(_interactCooldown);
        _canInteract = true;       
    }
}
