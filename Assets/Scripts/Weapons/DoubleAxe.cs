using UnityEngine;

public class DoubleAxe : Weapon
{
    public override void LeftClick()
    {
        animator.SetTrigger("MainAttack");
        Debug.Log("First attack Double Axe");
    }

    public override void RightClick()
    {
        animator.SetTrigger("SecondAttack");
        Debug.Log("Second attack Double Axe");
    }
}
