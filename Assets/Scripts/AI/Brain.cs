using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Context))]
public class Brain : MonoBehaviour
{
    [SerializeField] private List<UtilityAction> actions = new List<UtilityAction>(); 
    
    Context context;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        context = GetComponent<Context>();
        foreach (UtilityAction action in actions)
        {
            action.Initialize(context);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        float bestScore = float.MinValue;
        UtilityAction bestUtilityAction = null;

        foreach (UtilityAction action in actions)
        {
            float score = action.Evaluate(context);
            if (score > bestScore)
            {
                bestScore = score;
                bestUtilityAction = action;
            }
        }
        
        bestUtilityAction?.Execute(context);

        if (bestUtilityAction != null)
        {
            Debug.Log($"Best action: {bestUtilityAction.name}");
        }
        else
        {
            Debug.Log("No action found");
        }
    }
}
