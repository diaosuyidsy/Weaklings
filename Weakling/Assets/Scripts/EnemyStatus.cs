using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {
	public float HP = 100f;
	public float possessionRate = 0.1f;//under possessionRate * MaxHP can a enemy be possessed

	public bool canNowBePossessed = false;

	private float MaxHP;
	private Rigidbody2D rb;
	private Vector3 dir;

	public float impluseSpeed = 6f;

	void Awake()
	{
		MaxHP = HP;
	}

	void Start(){
		rb = GetComponent<Rigidbody2D> ();
		dir = new Vector3 (0, 1);
		dir *= impluseSpeed * Time.deltaTime; 
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
		if (GetComponent<EnemyController> ().enabled == false) {
			StartCoroutine (flashWhenDealtDamage ());
		}
		if (HP <= 0) {
			if (this.CompareTag ("Enemy")) {
				Destroy (gameObject);
			} else if (this.CompareTag ("PossessedPlayer")) {
				possessEnd ();
			}
		}
		if (HP <= possessionRate * MaxHP) {
			canNowBePossessed = true;
			if (GetComponent<EnemyController> ().enabled == false) {
				StartCoroutine (flashWhenCanBePossessed ());
			}
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
}
