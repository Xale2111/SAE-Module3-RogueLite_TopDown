using UnityEngine;

[CreateAssetMenu(fileName = "New Constant Consideration", menuName = "AI/Considerations/Constant")]
public class ConstantConsideration : Consideration
{
 	public float RawScore;
	public override float Evaluate(Context context)
	{
		return RawScore;
	}
}