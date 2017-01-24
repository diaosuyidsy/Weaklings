using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class FollowTarget : Action
{
	private GameObject target;

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

	private int counter;

	public BehaviorTree bt;

	public override void OnAwake(){
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		counter = 0;
	}

	public override void OnStart(){
//		seeker.StartPath (transform.position, target.position, OnPathComplete);
		target = (GameObject)bt.GetVariable ("Target").GetValue ();
		Debug.Log ("get target: " + target);

		if (counter == 0) {
			StartCoroutine (UpdatePath ()); 
			counter = 1;
		}
	}
		

	IEnumerator UpdatePath(){
		seeker.StartPath (transform.position, target.transform.position, OnPathComplete);

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

		if (!(bool)bt.GetVariable ("PIS").GetValue ()) {
			currentWayPoint = 0;
			return TaskStatus.Failure;
		}

		return TaskStatus.Running;
	}
}