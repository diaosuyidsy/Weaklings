using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetCollider : MonoBehaviour {

	public BehaviorTree soldierTree;
	private GameObject col;

	void OnCollisionEnter2D(Collision2D other)
	{
		col = other.collider.gameObject;
		soldierTree.SetVariableValue ("NextPlatform", col);
		other.collider.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void OnCollisionExit2D(Collision2D other)
	{
		other.collider.gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
	}
}
