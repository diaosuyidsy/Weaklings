using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnterNewLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			Debug.Log ("Load scene");
			SceneManager.LoadScene (GameManager.gm.levelAfterVictory);
		}
	}
}
