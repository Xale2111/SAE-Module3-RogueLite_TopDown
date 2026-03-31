using System;
using UnityEngine;

public class DoubleDagger : Weapon
{
    public override void LeftClick()
    {
        Debug.Log("First attack Double Dagger");
    }

    public override void RightClick()
    {
        Debug.Log("Second attack Double Dagger");
    }
}
