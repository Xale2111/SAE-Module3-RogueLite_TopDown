using UnityEngine;

public class InteractItem : MonoBehaviour
{
    protected Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            if (player.Interacted)
            {               
                OnPickUp();
                player.Interacted = false;
            }
        }
    }

    public virtual void OnPickUp()
    { 
        
    }
}
