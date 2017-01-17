using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnterNewLevel : MonoBehaviour {

	public Transform nextSpawn;

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			Debug.Log ("Teleporting");
			GameManager.gm._player.transform.position = nextSpawn.position;
			//set camera look at player immediately
			GameManager.gm.mainCamera.transform.position = GameManager.gm._player.transform.position;
		}
	}
}
