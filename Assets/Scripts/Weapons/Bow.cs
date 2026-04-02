using UnityEngine;

public class Bow : Weapon
{    

    public override void LeftClick()
    {
        if (animator)
        { 
            animator.SetTrigger("MainAttack");
        }
        Debug.Log("First attack BOW");        
    }

    public override void RightClick()
    {
        if (animator)
        {
            animator.SetTrigger("SecondAttack");
        }
        Debug.Log("Second attack BOW");
    }
}
