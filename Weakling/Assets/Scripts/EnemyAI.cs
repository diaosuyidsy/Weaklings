using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {
	public Transform target;

	public float UpdateRate = 2f;

	private Seeker seeker;
	private Rigidbody2D rb;

	public Path path;

	//The ai's speed
	public float speed = 6f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnd = false;

	public float nextWayPointDistance = 3;

	//the waypoint we are currently moving towards
	private int currentWayPoint = 0;

	void Start(){
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();

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

	void FixedUpdate (){
		if (currentWayPoint >= path.vectorPath.Count -1) {
			if (pathIsEnd) {
				return;
			}

			Debug.Log ("End of Path Reached");
			pathIsEnd = true;
		}

		pathIsEnd = false;

		//Direction to the next WayPoint
		Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		//Move the AI
		rb.AddForce (dir, fMode);

		if (Vector3.Distance (transform.position, path.vectorPath [currentWayPoint]) < nextWayPointDistance) {
			currentWayPoint++;
			return;
		}
	}
}
