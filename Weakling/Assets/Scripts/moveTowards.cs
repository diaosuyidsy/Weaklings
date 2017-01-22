using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class moveTowards : Action
{
	public Transform target;

	public float UpdateRate = 2f;

	private Seeker seeker;
	private Rigidbody2D rb;
	private SpriteRenderer sr;

	public Path path;

	//The ai's speed
	public float speed = 600f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnd = false;

	public float nextWayPointDistance = 3;

	//the waypoint we are currently moving towards
	private int currentWayPoint = 0;

	public BehaviorTree bt;

	public override void OnStart(){
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();

		seeker.StartPath (transform.position, target.position, OnPathComplete);

		StartCoroutine (UpdatePath()); 
	}

	IEnumerator UpdatePath(){
		seeker.StartPath (transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds (1f / UpdateRate);

		StartCoroutine (UpdatePath ());
	}

	public void OnPathComplete(Path p){
		if (!p.error) {
			path = p;
		}
	}

	public override TaskStatus OnUpdate (){
		if (currentWayPoint >= path.vectorPath.Count -1) {
			if (pathIsEnd) {
				return TaskStatus.Success;
			}

			Debug.Log ("End of Path Reached");
			pathIsEnd = true;
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

		if (!(bool)bt.GetVariable ("PIS").GetValue ()) {
			return TaskStatus.Success;
		}

		return TaskStatus.Running;
	}
}