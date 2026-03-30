using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        transform.position = mousePosition;
    }
}
