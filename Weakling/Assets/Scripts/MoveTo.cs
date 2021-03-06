using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using System.Collections;

public class MoveTo : Action
{
	public Transform target;

	public float UpdateRate = 2f;
	public Path path;
	public BehaviorTree bt;

	public float speed = 600f;
	public ForceMode2D fMode;

	private Seeker seeker;
	private Rigidbody2D rb;
	private SpriteRenderer sr;

	[HideInInspector]
	public bool pathIsEnd = false;

	public float nextWayPointDistance = 3;

	//the waypoint we are currently moving towards
	private int currentWayPoint = 0;

	public override void OnAwake(){
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
	}

	public override void OnStart()
	{
		//TODO: Update partol to up, down, left, right four directions.
		seeker.StartPath (transform.position, target.position, OnPathComplete);
	}

	void OnEnable(){
		seeker.StartPath (transform.position, target.position, OnPathComplete);
	}

	public override TaskStatus OnUpdate()
	{
//		if (currentWayPoint >= path.vectorPath.Count -1) {
//			Debug.Log ("End of Path Reached");
//			pathIsEnd = true;
//			if (pathIsEnd) {
//				return TaskStatus.Success;
//			}
//		}
		if (Vector3.Distance (transform.position, target.position) < 4f) {
			Debug.Log ("End of Path Reached");
			pathIsEnd = true;
			if (pathIsEnd) {
				currentWayPoint = 0;
				return TaskStatus.Success;
			}
		}

		pathIsEnd = false;

		//Direction to the next WayPoint
		Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		if (dir.x * transform.localScale.x >= 0) {
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		//Move the AI
		rb.AddForce (dir, fMode);

		if (Vector3.Distance (transform.position, path.vectorPath [currentWayPoint]) < nextWayPointDistance) {
			currentWayPoint++;
			return TaskStatus.Running;
		}


		if ((bool)bt.GetVariable ("PIS").GetValue ()) {
			return TaskStatus.Failure;
		}

		return TaskStatus.Running;
	}

	public void OnPathComplete(Path p){
		if (!p.error) {
			path = p;
		}
	}


}