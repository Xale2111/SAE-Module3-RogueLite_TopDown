using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float protectedSpeed = 1.5f;
    [SerializeField] private int maxHp = 120;
    [SerializeField] private float invincibilityDuration = 0.5f;
    [SerializeField] private GameObject aimLookAt;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private UIDocument playerDataUI;
    
    
    private Rigidbody2D rb;
    private float _interactCooldown = 0.5f;

    public int hp = 100;
    [HideInInspector] public float hpBarFill = 100;

    private VisualElement hpBarValue;

    Vector2 move;

    public bool Interacted = false;
    private bool _canInteract = true;

    private bool _isInvincible = false;
    public bool IsProtected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = maxHp;
        
        VisualElement playerHealth = playerDataUI.rootVisualElement.Query("Health").First();
        playerHealth.dataSource = this;
        
        hpBarValue = playerDataUI.rootVisualElement.Query("HPBar_Value").First();
        
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
      
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
            }
            else
            {
                Interacted = false;
            }
            
        }
    }

    public void TakeDamage(int damageToDeal)
    {
        if (!IsProtected && !_isInvincible)
        {
            hp -= damageToDeal;
            UpdateHealthBar();
            StartCoroutine(InvincibilityFrames());
        }
    }

    private IEnumerator InvincibilityFrames()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        _isInvincible = false;
    }

    public int GetHp()
    {
        return hp;
    }
    
    public int GetMaxHp()
    {
        return maxHp;
    }

    private void UpdateHealthBar()
    {
        if (hpBarValue != null)
        {
            float healthRatio = Mathf.Clamp01((float)hp / maxHp);
            hpBarValue.style.width = new StyleLength(new Length(healthRatio * 100f, LengthUnit.Percent));
        }
    }

    private IEnumerator CooldownInteract()
    {
        _canInteract = false;
        yield return new WaitForSeconds(_interactCooldown);
        _canInteract = true;       
    }
}
