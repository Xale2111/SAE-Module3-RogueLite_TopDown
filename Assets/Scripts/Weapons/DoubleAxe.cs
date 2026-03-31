using UnityEngine;

public class DoubleAxe : Weapon
{
    public override void LeftClick()
    {
        animator.SetBool("MainAttack", true);
        Debug.Log("First attack Double Axe");
    }

    public override void RightClick()
    {
        animator.SetBool("SecondAttack", true);
        Debug.Log("Second attack Double Axe");
    }
}
