using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using System.Collections;

public class Flee : Action
{
	public Transform target;
	public float speed;
	public float UpdateRate = 2f;
	public Path path;
	public float y=4f;
	public float x=6f;
	public ForceMode2D fMode;


	private Rigidbody2D rb;
	private int counter;
	private Vector3 toLeft;
	private Vector3 toRight;
	public float nextWayPointDistance = 3;

	//the waypoint we are currently moving towards
	private int currentWayPoint = 0;

	public override void OnAwake()
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		rb = GetComponent<Rigidbody2D> ();
		counter = 0;
		toLeft = new Vector3 (x * -1, y).normalized;
		toRight = new Vector3 (x, y).normalized;
	}

	public override TaskStatus OnUpdate()
	{
		//Direction to the next WayPoint
		Vector3 dir = new Vector3 (0, 0, 0);
		if(transform.position.x > target.position.x){
			dir = toRight;
		}else{
			dir = toLeft;
		}

		dir *= speed * Time.fixedDeltaTime;

		if (dir.x * transform.localScale.x >= 0) {
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		//Move the AI
		rb.AddForce (dir, fMode);
		return TaskStatus.Running;	
	}
}