using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class FollowTarget_Walk : Action
{
	public GameObject target;

	public float UpdateRate = 2f;

	private Seeker seeker;
	private Rigidbody2D rb;
	public bool isGrounded=true;

	public Path path;
	public Transform groundCheck;
	public LayerMask whatIsGround;


	//The ai's speed
	public float flyspeed = 600f;
	public float walkspeed = 6f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnd = false;

	public float nextWayPointDistance = 3;

	//the waypoint we are currently moving towards
	private int currentWayPoint = 0;

	private int counter;
	private bool jumping=false;

	public BehaviorTree bt;

	public override void OnAwake(){
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
		counter = 0;
		isGrounded = true;
	}

	public override void OnStart(){
		//		seeker.StartPath (transform.position, target.position, OnPathComplete);
//		target = (GameObject)bt.GetVariable ("Target").GetValue ();
		if (counter == 0) {
			StartCoroutine ("UpdatePath"); 
			counter = 1;
		}
	}


	IEnumerator UpdatePath(){
		seeker.StartPath (transform.position, target.transform.position, OnPathComplete);

		yield return new WaitForSeconds (1f / UpdateRate);

		StartCoroutine ("UpdatePath"); 
	}

	public void OnPathComplete(Path p){
		if (!p.error) {
			path = p;
		}
	}

	public override TaskStatus OnUpdate (){
		checkMovement ();
		if (currentWayPoint >= path.vectorPath.Count - 1 && !jumping) {
			pathIsEnd = true;
			if (pathIsEnd) {
				Debug.Log ("Task succeeded");
				return TaskStatus.Success;
			}
		}else if(currentWayPoint >= path.vectorPath.Count - 1 && jumping){
			//if we get to the corner point of the collider,
			StartCoroutine ("UpdatePath"); 
			isGrounded = true;
		}

		pathIsEnd = false;

		//Direction to the next WayPoint
		Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;

		//if on ground, then just walk
		//if not on ground, then fly to the 
		if(isGrounded){
			jumping = false;
			float _vx = target.transform.position.x - transform.position.x;
			_vx = _vx / Mathf.Abs (_vx);
			rb.velocity = new Vector2(_vx * walkspeed, rb.velocity.y);
		}else{
			//get the point where this object should go next
			Vector3 nextPoint = getNextJumpPoint ();
			//stop current Coroutine
			StopCoroutine ("UpdatePath");
			//update path
			if(!jumping){
				seeker.StartPath (transform.position, nextPoint, OnPathComplete);
				jumping = true;
				rb.velocity = new Vector2 (0, 0);
				Debug.Log ("Jumping is TRUE now");
			}

			dir *= flyspeed * Time.fixedDeltaTime;
			rb.AddForce (dir, fMode);
		}


//		if (dir.x * transform.localScale.x >= 0) {
//			Vector3 theScale = transform.localScale;
//			theScale.x *= -1;
//			transform.localScale = theScale;
//		}
		if(jumping){
			if (Vector3.Distance (transform.position, path.vectorPath [currentWayPoint]) < 0.5) {
				currentWayPoint++;
				return TaskStatus.Running;
			}
		}else{
			if (Vector3.Distance (transform.position, path.vectorPath [currentWayPoint]) < nextWayPointDistance) {
				currentWayPoint++;
				return TaskStatus.Running;
			}
		}


		/*if (!(bool)bt.GetVariable ("PIS").GetValue ()) {
			currentWayPoint = 0;
			return TaskStatus.Failure;
		}*/

		return TaskStatus.Running;
	}

	void checkMovement(){
		isGrounded = Physics2D.Linecast (transform.position, groundCheck.position, whatIsGround) ||
			Physics2D.Linecast (transform.position + new Vector3(0.25f, 0, 0), groundCheck.position + new Vector3(0.25f, 0, 0), whatIsGround) ||
			Physics2D.Linecast (transform.position - new Vector3(0.25f, 0, 0), groundCheck.position - new Vector3(0.25f, 0, 0), whatIsGround);
	}

	private Vector3 getNextJumpPoint(){
		GameObject temp = (GameObject)bt.GetVariable ("NextPlatform").GetValue ();
		BoxCollider2D collider = temp.GetComponent<BoxCollider2D> ();

		Vector2 size = collider.size;
		Vector3 centerPoint = new Vector3( collider.offset.x, collider.offset.y);

		Vector3 worldPos = collider.transform.TransformPoint ( centerPoint );

		Rect rect = new Rect(0f, 0f, size.x, size.y);
		rect.center = new Vector2(worldPos.x, worldPos.y);

		Vector3 topLeft = new Vector3( rect.xMin, rect.yMax);
		Vector3 topRight = new Vector3( rect.xMax, rect.yMax);

		//detect which one is closer to the enemy
		if(Vector3.Distance (topLeft, transform.position) <= Vector3.Distance (topRight, transform.position)){
			//if topLeft coner is closer to the enemy, goto left.
			return topLeft + new Vector3 (0, 10f);
		}else{
			return topRight + new Vector3 (0, 10f);;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log ("On Collision Enter, layer is: "+other.gameObject.layer+". I am "+jumping+" jumping");
		if (other.gameObject.layer == 9 && jumping) {
			Vector3 dir = other.contacts[other.contacts.Length / 2].point - new Vector2(transform.position.x, transform.position.y);
			// We then get the opposite (-Vector3) and normalize it
			dir = dir.normalized * -1;


			// And finally we add force in the direction of dir and multiply it by force. 
			// This will push back the player
			rb.AddForce(dir*150f);
			Debug.Log ("Bounced off the cliff, direction of force is: " + dir);
		}
	}
}