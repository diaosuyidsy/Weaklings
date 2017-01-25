using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

public class EnemyStatus : MonoBehaviour {
	public float HP = 100f;
	public float possessionRate = 0.1f;//under possessionRate * MaxHP can a enemy be possessed

	public bool canNowBePossessed = false;

	private float MaxHP;
	private Rigidbody2D rb;
	private Vector3 dir;

	private BehaviorTree[] trees;
	private int counter = 0;

	public float impluseSpeed;
	public float gravityScale = 1f;


	void Awake()
	{
		MaxHP = HP;
	}

	void Start(){
		rb = GetComponent<Rigidbody2D> ();
		dir = new Vector3 (0, 1);
		dir *= impluseSpeed * Time.deltaTime; 
		trees = GetComponents<BehaviorTree> ();
		StartCoroutine (jetpack ());
	}

	//apply a force to keep enemy up in air
	IEnumerator jetpack(){

		rb.AddForce (dir, ForceMode2D.Impulse);
		
		yield return new WaitForSeconds (1f);

		StartCoroutine (jetpack ());
	}

	//note: heals when deal Negative damage
	public void dealDamage(float dmg)
	{
		HP -= dmg;
//		if (GetComponent<EnemyController> ().enabled == false) {
//			StartCoroutine (flashWhenDealtDamage ());
//		}
		if (HP <= 0) {
			if (this.CompareTag ("Enemy")) {
				Destroy (gameObject);
			} else if (this.CompareTag ("PossessedPlayer")) {
				possessEnd ();
			}
		}
		if (HP <= possessionRate * MaxHP) {
			canNowBePossessed = true;
//			if (GetComponent<EnemyController> ().enabled == false) {
//				StartCoroutine (flashWhenCanBePossessed ());
//			}
			//Bring it down
			if(counter == 0){
				rb.gravityScale = this.gravityScale;
				counter = 1;
				Debug.Log ("Gravity Scale went up to: " + rb.gravityScale);
				for (int i = 0; i < trees.Length; i++) {
					if (trees [i].GetBehaviorSource ().behaviorName == "Normal") {
						trees [i].enabled = false;
						Debug.Log ("disabled Normal tree");
					} else {
						trees [i].enabled = true;
						Debug.Log ("enabled flee tree");
					}
				}
			}
			//Disable normal bt, enable flee bt

		}
		if (HP >= possessionRate * MaxHP) {
			canNowBePossessed = false;
			StopCoroutine (flashWhenCanBePossessed ());
		}
	}

	public void possessEnd()
	{
		GameObject player = GameObject.Find ("Alien");

		player.transform.parent = null;
		//Enable everything in player
		player.GetComponent<SpriteRenderer>().enabled = true;
		player.GetComponent<CharacterController2D> ().enabled = true;
		//disabel rigidbody
		player.GetComponent<Rigidbody2D>().isKinematic = false;
		player.GetComponent<CircleCollider2D> ().enabled = true;

		//set GameManager's player to alien
		GameObject.Find("GameManager").GetComponent<GameManager>()._player = player;

		//destroy this enemy object whatsoever
		Destroy(gameObject);
		Destroy (GetComponent<EnemyMovements> ());

		Camera.main.GetComponent<UnityStandardAssets._2D.CameraFollow2D>().target = player.transform;
	}

	IEnumerator flashWhenCanBePossessed()
	{
		while (canNowBePossessed) {
			GetComponent<SpriteRenderer> ().color = Color.white;
			yield return new WaitForSeconds (0.3f);
			GetComponent<SpriteRenderer> ().color = Color.red;
			yield return new WaitForSeconds (0.3f);
		}
	}

	IEnumerator flashWhenDealtDamage()
	{
		GetComponent<SpriteRenderer> ().color = Color.red;
		yield return new WaitForSeconds (0.3f);
		GetComponent<SpriteRenderer> ().color = Color.white;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == 9) {
			Vector3 dir = other.contacts[other.contacts.Length / 2].point - new Vector2(transform.position.x, transform.position.y);
			Debug.Log ("My position: " + transform.position + "hitpoint position: " + other.contacts[other.contacts.Length / 2].point);
			// We then get the opposite (-Vector3) and normalize it
			dir = dir.normalized * -1;


			// And finally we add force in the direction of dir and multiply it by force. 
			// This will push back the player
			rb.AddForce(dir*150f);
		}
	}
}
