using UnityEngine;

public class ShieldAndSpear : Weapon
{
    public override void LeftClick()
    {
        Debug.Log("First attack Shield");
    }

    public override void RightClick()
    {
        Debug.Log("Second attack Spear");
    }
}
