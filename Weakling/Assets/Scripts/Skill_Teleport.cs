using UnityEngine;
using System.Collections;

public class Skill_Teleport : MonoBehaviour {

	private GameObject _player;

	private Transform _transform;

	public string hotkey;
	public float teleportRange = 3.0f;

	private float castTime = 2f;
	private float currentTime = 0f;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag ("Player");
		_transform = _player.GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (hotkey)) {
			Debug.Log ("Held Down");
			currentTime += Time.deltaTime;
			if (currentTime >= castTime) {
				Vector3 temp = new Vector3 (teleportRange * _player.GetComponent<CharacterController2D>().getFacing(), 0, 0);
				_transform.position += temp;
				currentTime = 0;
			}
		} else {
			currentTime = 0;
		}
	}
}
