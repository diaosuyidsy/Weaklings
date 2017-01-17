using UnityEngine;
using System.Collections;

public class Skill_Run : MonoBehaviour {

	private GameObject _player;

	private Rigidbody2D _rigidBody;

	public float moveSpeed = 6f;

	public int clicks = 0;

	private float lastClickTime = 0f;

	private float maxComboDelay = 0.5f;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag ("Player");
		_rigidBody = _player.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		//check for running combo:
		//twice moving key pressed, then run
		if (Time.time - lastClickTime > maxComboDelay) {
			clicks = 0;
		}


		//if player trys to run
		if (Input.GetButtonDown("Horizontal")) {
			lastClickTime = Time.time;
			clicks++;

			if (clicks >= 2) {
				_player.GetComponent<CharacterController2D>().moveSpeed = this.moveSpeed;
			}
		}

		//if player relase the run
		if(Input.GetButtonUp("Horizontal")){
			_player.GetComponent<CharacterController2D>().moveSpeed = 3f;
		}


	}
}
