using UnityEngine;
using System.Collections;

public class M4BulletBehavior : MonoBehaviour {

	private float disappearTimer = 0.5f;

	public float bulletDmg = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (disappearTimer > 0) {
			disappearTimer -= Time.deltaTime;
		} else {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Enemy") {
			Debug.Log ("on hit enemy");
			coll.SendMessageUpwards("dealDamage", bulletDmg);
			Destroy (this.gameObject);
		}
	}
}
