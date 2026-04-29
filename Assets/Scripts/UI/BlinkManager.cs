using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BlinkManager : MonoBehaviour
{
    [SerializeField] private UIDocument blinkUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blinkUI.enabled = false;
    }

    public void BlinkUI(float blinkTime)
    {
        blinkUI.enabled = true;
        StartCoroutine(Blink(blinkTime));
    }

    private IEnumerator Blink(float blinkTime)
    {
        yield return new WaitForSeconds(blinkTime);
        blinkUI.enabled = false;
    }
    
    
}
