using UnityEngine;

public abstract class UtilityAction : ScriptableObject
{
    public Consideration Consideration;
    
    public abstract void Initialize(Context context);
    public abstract void Execute(Context context);
    //public abstract void Interrupt(Context context);
    public float Evaluate(Context context) => Consideration.Evaluate(context);
}
