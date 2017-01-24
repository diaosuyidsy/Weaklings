using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsLowOnHealth : Conditional
{

	public BehaviorTree bt;

	public override TaskStatus OnUpdate()
	{
		if ((bool)bt.GetVariable ("lowOnHealth").GetValue ()) {
			return TaskStatus.Success;
		} else {
			return TaskStatus.Running;
		}
	}
}