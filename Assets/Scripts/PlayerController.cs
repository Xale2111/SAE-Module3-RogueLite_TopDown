using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject aimLookAt;
    private Rigidbody2D rb;

    Vector2 move;

    public bool Interacted =false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {                    

    }
    void FixedUpdate()
    {
        rb.linearVelocity = move * speed;
        Vector2 direction = (aimLookAt.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        rb.MoveRotation(angle);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
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
