using UnityEngine;

public class ShieldAndSpear : Weapon
{
    bool isHoldingShield = false;
    
    public override void LeftClick()
    {
        isHoldingShield = true;
        animator.SetBool("HoldShield",isHoldingShield);
        Debug.Log("Holding Shield");
    }
    
    public override void ReleaseLeftClick()
    {
        isHoldingShield = false;
        animator.SetBool("HoldShield",isHoldingShield);

        Debug.Log("Releasing Shield");
        
    }

    public override void RightClick()
    {
        if (!isHoldingShield)
        {
            animator.SetTrigger("SecondAttack");
            Debug.Log("Second attack Spear");
        }
    }
}
