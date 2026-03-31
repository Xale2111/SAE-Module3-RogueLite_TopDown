using UnityEngine;

public class Chest : InteractItem
{

    public override void OnPickUp()
    {
        animator.SetBool("PickedUp", true);
        Debug.Log("Open Chest");
    } 
}
