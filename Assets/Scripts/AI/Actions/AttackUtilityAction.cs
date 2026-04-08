using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "AI/Actions/Attack")]
public class AttackUtilityAction : UtilityAction
{
    public override void Initialize(Context context)
    {
    }

    public override void Execute(Context context)
    {
        context.GetEnemy().Attack();
        //Perform attack then check if player in attack range, if not, reevaluate context
    }
}