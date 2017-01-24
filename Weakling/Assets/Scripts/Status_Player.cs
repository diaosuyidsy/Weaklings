using UnityEngine;
using System.Collections;

public class Status_Player : MonoBehaviour {

	public float HP;
	private SpriteRenderer sr;
	private Color cr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		cr = sr.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void takeDamage(int dmg){
		HP -= dmg;
		StartCoroutine (flashWhenDealtDamage ());
		if (HP <= 0) {
			GameManager.gm.resetGame ();
		}
	}

	IEnumerator flashWhenDealtDamage()
	{
		sr.color = Color.red;
		yield return new WaitForSeconds (0.3f);
		sr.color = cr;
	}
}
