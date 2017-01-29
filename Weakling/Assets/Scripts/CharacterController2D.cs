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

	public bool canDoubleJump = true;
	 
	public LayerMask whatIsGround;
	public LayerMask whatCanBePossessed;

	public Transform groundCheck;
	public Transform possessionCheck;

	[HideInInspector]
	bool playerCanMove = true;

	bool doubleJumpCount = true;

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
//		DontDestroyOnLoad (this);
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
		//check if next move is to possess an enemy
		checkPossession ();
		//check if next move is basic movement
		checkMovement ();

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

	void checkPossession()
	{
		//Check if player can possess a enemy
		if(Input.GetButtonDown("Fire1") && (bool)Physics2D.Linecast(_transform.position, possessionCheck.position, whatCanBePossessed)//if player hit left mouse and an enemy is withinrange possess
			&& Physics2D.Linecast(_transform.position, possessionCheck.position, whatCanBePossessed).transform.localScale.x * _transform.localScale.x < 0 //if player is on enemy's back
			&& Physics2D.Linecast(_transform.position, possessionCheck.position, whatCanBePossessed).transform.GetComponent<EnemyStatus>().canNowBePossessed){ //if enemy is under 10% HP
			onPossesionStart (Physics2D.Linecast(_transform.position, possessionCheck.position, whatCanBePossessed)); //then start possesion
		}
	}

	void checkMovement()
	{
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

		if(Input.GetButtonDown("Jump"))
		{
			if (isGrounded) {
				_vy = 0f;
				//add a force in the up direction
				_rigidbody.AddForce (new Vector2 (0, jumpForce));
				//play the jumping sound, need to be done
				doubleJumpCount = true;
			} else {
				if (canDoubleJump && doubleJumpCount) {
					_vy = 0f;
					_rigidbody.AddForce (new Vector2 (0, jumpForce));
					doubleJumpCount = false;
				}
			}
		}

		//		 If the player stops jumping mid jump and player is not yet falling
		//		 then set the vertical velocity to 0 (he will start to fall from gravity)
		if(Input.GetButtonUp("Jump") && _vy>0f)
		{
			_vy = 0f;
		}

		_rigidbody.velocity = new Vector2(_vx * moveSpeed, _vy);
	}

	//when player possess the enemy 
	void onPossesionStart(RaycastHit2D enemy)
	{
		this.transform.parent = enemy.transform;

		//disable everything in player
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<CharacterController2D> ().enabled = false;
		//disabel rigidbody
		_rigidbody.isKinematic = true;
		this.GetComponent<CircleCollider2D> ().enabled = false;

		//set GameManager's player to enemy
		GameObject.Find("GameManager").GetComponent<GameManager>()._player = enemy.transform.gameObject;

		//set enemy's tag to Player
		enemy.collider.gameObject.tag = "PossessedPlayer";
		//enable enemy's script
		enemy.collider.GetComponentInParent<EnemyController>().enabled = true;
		enemy.collider.GetComponentInParent<M4Attack>().enabled = true;
		//disable enemy's moving script (AI script)
		enemy.collider.GetComponentInParent<EnemyMovements>().enabled = false;
		//make camera follow possessed enemy
		Camera.main.GetComponent<UnityStandardAssets._2D.CameraFollow2D>().target = enemy.transform;
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
		if (other.gameObject.layer == 12) {
			//if enemy is on the rhs of player
			float onHitDir = 1f;
			//if enemy is on the lhs
			if (_transform.position.x > other.gameObject.transform.position.x) {
				onHitDir = -1f;
			}
			_rigidbody.AddForce(new Vector2(-1500 * onHitDir, 0));
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

	public int getFacing(){
		if (facingRight) {
			return 1;
		} else {
			return -1;
		}
	}
}
