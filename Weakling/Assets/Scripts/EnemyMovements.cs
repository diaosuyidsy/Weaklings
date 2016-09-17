using UnityEngine;
using System.Collections;

public class EnemyMovements : MonoBehaviour {
	
	public float moveSpeed = 1.0f;
	public int enemyHealth = 1;

	public Vector3 initialPosition;
	public bool playerInSight;
	public Vector2 personalLastSight;

	public double distance;

	private GameObject player;
	public Transform groundCheck;
	public LayerMask whatIsGround;

	[HideInInspector]
	bool enemyCanMove = true;

	//store references to components on the gameObject
	Transform _transform;
	Rigidbody2D _rigidbody;
	bool isGrounded = false;
	float _vx;
	float _vy;

	int _enemyLayer;

	int _platformLayer;

	void Awake(){
		_transform = GetComponent<Transform> ();
		_rigidbody = GetComponent<Rigidbody2D> ();

		if (_rigidbody == null) {
			Debug.LogError ("Rigidbody2D component is missing from this game object");
		}

		_enemyLayer = this.gameObject.layer;

		_platformLayer = LayerMask.NameToLayer("Platform");
	}

	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.Linecast (_transform.position, groundCheck.position, whatIsGround);

	
	}

	public void dealDamage(int damage)
	{
		if (enemyCanMove) {
			enemyHealth -= damage;
		}

		if(enemyHealth <= 0)
		{
//			StartCoroutine (killEnemy ());
		}
	}

//	IEnumerator killEnemy()
//	{
//			yield return new WaitForSeconds (2f);
//
//			if (GameManager.gm) {
//			GameManager.gm.respawnEnemy();
//			} 
//	}

	/*void OnTriggerStay(Collider other){
		if (other.gameObject == player) {
			playerInSight == false;

			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle (direction, transform.forward);


		}

	}

	void OnTriggerExit(Collider other){
		if (other.gameObject == player)
			playerInSight = false;
	}*/

	public void respawn(Vector3 loc)
	{
		
		enemyHealth = 1;
		_transform.parent = null;
		_transform.position = loc;
	}
}
