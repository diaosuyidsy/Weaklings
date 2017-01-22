using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WithInSight : Conditional
{

	public BehaviorTree behaviorTree;

	// Use this for initialization
	public override void OnAwake(){
		
	}

	// Update is called once per frame
	public override TaskStatus OnUpdate () {
		if ((bool)behaviorTree.GetVariable ("PIS").GetValue ()) {
			return TaskStatus.Success;
		}

		return TaskStatus.Failure;
	}
		
}