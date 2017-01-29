using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;


public class Enemy_Pilot_Status : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector3 dir;
	public float impluseSpeed;
	public float gravityScale = 1.2f;

	private BehaviorTree[] trees;
	private int counter = 0;




	// Use this for initialization
	void Start () {
		rb = GetComponent <Rigidbody2D> ();
		trees = GetComponents<BehaviorTree> ();
		StartCoroutine (jetpack ());
		dir = new Vector3 (0, 1);
		dir *= impluseSpeed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent <EnemyStatus> ().HP <= GetComponent <EnemyStatus> ().possessionRate * GetComponent <EnemyStatus> ().MaxHP) {
			if (counter == 0) {
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
		}
	}

	IEnumerator jetpack(){

		rb.AddForce (dir, ForceMode2D.Impulse);

		yield return new WaitForSeconds (1f);

		StartCoroutine (jetpack ());
	}

	public void down(){
		
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == 9) {
			Vector3 dir = other.contacts[other.contacts.Length / 2].point - new Vector2(transform.position.x, transform.position.y);
			// We then get the opposite (-Vector3) and normalize it
			dir = dir.normalized * -1;


			// And finally we add force in the direction of dir and multiply it by force. 
			// This will push back the player
			rb.AddForce(dir*150f);
		}
	}
}
