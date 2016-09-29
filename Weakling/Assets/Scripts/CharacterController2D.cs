using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour {
	//player controls
	[Range(0.0f, 10.0f)]
	public float moveSpeed = 3.0f;

	public float jumpForce = 600.0f;

	// player health
	public int playerHealth = 1;
	 
	public LayerMask whatIsGround;

	public Transform groundCheck;

	[HideInInspector]
	bool playerCanMove = true;

	//store references to components on the gameObject
	Transform _transform;
	Rigidbody2D _rigidbody;

	//hold player motion in this timestep
	float _vx;
	float _vy;

	bool facingRight = true;
	bool isGrounded = false;
	bool isRunning = false;

	int _playerLayer;

	void Awake(){
		_transform = GetComponent<Transform> ();
		_rigidbody = GetComponent<Rigidbody2D> ();

		if (_rigidbody == null) {
			Debug.LogError ("Rigidbody2D component is missing from this game object");
		}

		_playerLayer = this.gameObject.layer;

	}
		
	// Update is called once per frame
	void Update () {
		//exit if player can't move or game is paused
		if (!playerCanMove || (Time.timeScale == 0f)) {
			return;
		}

		_vx = Input.GetAxisRaw ("Horizontal");

		if (_vx != 0) {
			isRunning = true;
		} else {
			isRunning = false;
		}

		_vy = _rigidbody.velocity.y;

		//check if player is standing on ground
		isGrounded = Physics2D.Linecast (_transform.position, groundCheck.position, whatIsGround) ||
			Physics2D.Linecast (_transform.position + new Vector3(0.25f, 0, 0), groundCheck.position + new Vector3(0.25f, 0, 0), whatIsGround) ||
			Physics2D.Linecast (_transform.position - new Vector3(0.25f, 0, 0), groundCheck.position - new Vector3(0.25f, 0, 0), whatIsGround);
		
		if(isGrounded && Input.GetButtonDown("Jump"))
		{
			_vy = 0f;
			//add a force in the up direction
			_rigidbody.AddForce(new Vector2(0, jumpForce));
			//play the jumping sound, need to be done
		}

//		 If the player stops jumping mid jump and player is not yet falling
//		 then set the vertical velocity to 0 (he will start to fall from gravity)
		if(Input.GetButtonUp("Jump") && _vy>0f)
		{
			_vy = 0f;
		}

		_rigidbody.velocity = new Vector2(_vx * moveSpeed, _vy);

	}

	void LateUpdate()
	{
		//get the current scale, so that could be flipped later
		Vector3 localScale = _transform.localScale;

		if (_vx > 0) {
			facingRight = true;
		} else if( _vx < 0) {
			facingRight = false;
		}

		if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0)) {
			localScale.x *= -1;
		}

		_transform.localScale = localScale;

	}

	//if the player collide with a moving platform, then
	//make the player a child of that platform so that
	//it could move with the platform
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "MovingPlatform") {
			this.transform.parent = other.transform;
		}
		//if collide with harmful object, deal damage to player
		//and move back a little bit.
		if (other.gameObject.tag == "Harmful") {
			Debug.Log ("on hit enemy");
			_rigidbody.AddForce(new Vector2(-1500, 0));
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag == "MovingPlatform") {
			this.transform.parent = null;
		}
	}

	//do what need to do to freeze the player
	void FreezeMotion()
	{
		playerCanMove = false;
		_rigidbody.isKinematic = true;
	}

	void UnFreezeMotion()
	{
		playerCanMove = true;
		_rigidbody.isKinematic = false;
	}

	public void dealDamage(int damage)
	{
		if (playerCanMove) {
			playerHealth -= damage;
		}
		if (playerHealth <= 0) {
			StartCoroutine (killPlayer ());
		}
	}

	IEnumerator killPlayer()
	{
		if (playerCanMove) {
			FreezeMotion ();

			yield return new WaitForSeconds (2f);

			if (GameManager.gm) {
				GameManager.gm.resetGame ();
			} else {
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			}
		}
	}

	public void respawn(Vector3 loc)
	{
		UnFreezeMotion ();
		playerHealth = 1;
		_transform.parent = null;
		_transform.position = loc;
	}
}
