using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Shoot_A : Action
{

	private GameObject Target;

	private bool isAttacking = false;
	private float attackTimer = 0f;
	private float attackCD = 1f;
	private SpriteRenderer sr;

	public float minDamage = 8f;
	public float maxDamage = 12f;

	public BehaviorTree bt;

	public int layerMask_Self = ~(1 << 12);

	public float shootRange = 10;

	private bool sightBlocked = false;

	public override void OnAwake()
	{
		sr = GetComponent<SpriteRenderer> ();
	}

	public override TaskStatus OnUpdate()
	{
		Target = (GameObject)bt.GetVariable ("Target").GetValue ();
		if (Target != null ) {
			if (!isAttacking && withInShootRange ()) {
				isAttacking = true;
				attackTimer = attackCD;
				Debug.Log ("Shoot Target");

				shootTarget ();
			}
			if (isAttacking) {
				if (attackTimer > 0) {
					attackTimer -= Time.deltaTime;
				} else {
					isAttacking = false;
				}
			}
		} else {
			return TaskStatus.Failure;
		}
		if (!withInShootRange () || sightBlocked) {
			return TaskStatus.Success;
		}
		return TaskStatus.Running;
	}

	private void shootTarget(){
		//turn enemy into good facing
		float facing = Target.transform.position.x - transform.position.x;
		if (facing * transform.localScale.x > 0) {
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
		var heading = Target.transform.position - this.transform.position;
		var distance = heading.magnitude;
		var direction = heading / distance;

		RaycastHit2D hitPoint = Physics2D.Raycast (transform.position, direction, 100, layerMask_Self);
		Debug.Log (hitPoint.collider.tag);
		if (hitPoint.collider.tag == "Player") {
			Target.SendMessage ("takeDamage", Random.Range (minDamage, maxDamage));
		} else {
			sightBlocked = true;
		}
	}

	private bool withInShootRange(){
		return Vector3.Distance (Target.transform.position, transform.position) <= shootRange;
	}
}