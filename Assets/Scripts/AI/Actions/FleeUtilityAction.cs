using UnityEngine;

[CreateAssetMenu(fileName = "FleeAction", menuName = "AI/Actions/Flee")]
public class FleeUtilityAction : UtilityAction
{
    private Transform center;
    public override void Initialize(Context context)
    {
        center = context.SelfTransform;   
    }

    public override void Execute(Context context)
    {
        if (Vector3.Distance(context.GetPlayerTransform().position, context.SelfTransform.position) < context.GetEnemy().DetectionRadius)
        {
            Vector3 direction = context.SelfTransform.position - context.GetPlayerTransform().position;
            context.MoveTo(direction);
        }
    }
}
