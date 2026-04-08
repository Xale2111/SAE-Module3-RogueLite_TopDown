using UnityEngine;

[CreateAssetMenu(fileName = "ChaseAction", menuName = "AI/Actions/Chase")]
public class ChaseUtilityAction : UtilityAction
{
    public override void Initialize(Context context)
    {
    }

    public override void Execute(Context context)
    {
        Vector3 direction = context.GetPlayerTransform().position - context.SelfTransform.position;
        context.MoveTo(direction);
    }
}
