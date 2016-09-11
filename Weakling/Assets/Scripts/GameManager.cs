using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	//setup static reference so that other classes can call game manager directly
	public static GameManager gm;

	//setup scene load after this level
	public string levelAfterVictory;
	public string levelAfterGameOver;

	//private variables
	public GameObject _player;
	Vector3 _spawnLocation;

	//UI elements
	public GameObject UIGamePaused;

	void Awake()
	{
		if (gm == null) {
			gm = this.GetComponent<GameManager> ();
		}

		setupDefaults ();
	}

	//main game loop
	void Update()
	{
		//if Esc is pressed, then pause/unpause the game
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(Time.timeScale > 0f){
				UIGamePaused.SetActive(true);
				Time.timeScale = 0f;
			}else{
				Time.timeScale = 1f;
				UIGamePaused.SetActive(false);
			}

		}
	}

	//setup all variables and provide debug lines
	void setupDefaults()
	{
		//reference to player
		if (_player == null) {
			_player = GameObject.FindGameObjectWithTag ("Player");
		}
		if (_player == null) {
			Debug.LogError ("Player object is not found");
		}

		//get initial spawn location
		_spawnLocation = _player.transform.position;

		if (levelAfterVictory == "") {
			Debug.LogWarning ("Level after victory is not specified, using current level");
			levelAfterVictory = SceneManager.GetActiveScene ().name;
		}

		if (levelAfterGameOver == "") {
			Debug.LogWarning ("Level after Game Over is not specified, using current level");
			levelAfterGameOver = SceneManager.GetActiveScene ().name;
		}

		if (UIGamePaused == null) {
			Debug.LogError ("Need to set UIGamePaused on Game Manager.");
		}
	}

	public void resetGame()
	{
		_player.GetComponent<CharacterController2D>().respawn (_spawnLocation);

	}

}
