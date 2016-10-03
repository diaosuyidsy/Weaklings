using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	public int HP = 100;

	public void dealDamage(int dmg)
	{
		HP -= dmg;
		if (HP <= 0) {
			if (this.CompareTag ("Enemy")) {
				Destroy (gameObject);
			} else if (this.CompareTag ("PossessedPlayer")) {
				possessEnd ();
			}
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
}
