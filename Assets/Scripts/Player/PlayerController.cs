using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject aimLookAt;
    [SerializeField] private float rotationSpeed = 10f;
    
    private Rigidbody2D rb;
    private float _interactCooldown = 0.5f;
    

    Vector2 move;

    public bool Interacted =false;
    private bool _canInteract = true;
    
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

    private IEnumerator CooldownInteract()
    {
        _canInteract = false;
        yield return new WaitForSeconds(_interactCooldown);
        _canInteract = true;       
    }
}
