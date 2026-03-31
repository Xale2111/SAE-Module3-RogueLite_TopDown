using UnityEngine;

public class Bow : Weapon
{    

    public override void LeftClick()
    {
        if (animator)
        { 
            animator.SetBool("MainAttack",true);
        }
        Debug.Log("First attack BOW");        
    }

    public override void RightClick()
    {
        if (animator)
        {
            animator.SetBool("SecondAttack", true);
        }
        Debug.Log("Second attack BOW");
    }
}
