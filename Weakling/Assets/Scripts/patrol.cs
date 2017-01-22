using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using System.Collections;

public class patrol : Action
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

	public override void OnStart()
	{
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();

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
		if (Vector3.Distance (transform.position, target.position) < 3f) {
			Debug.Log ("End of Path Reached");
			pathIsEnd = true;
			if (pathIsEnd) {
				return TaskStatus.Success;
			}
		}

		pathIsEnd = false;

		//Direction to the next WayPoint
		Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		if (dir.x >= 0) {
			sr.flipX = true;
		} else {
			sr.flipX = false;
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