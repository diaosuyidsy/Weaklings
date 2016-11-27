using UnityEngine;
using System.Collections;

public class M4Attack : MonoBehaviour {

	public GameObject bullet;
	public Transform shotSpawn;

	public float bulletSpeed = 50.0f;

	private Transform shooter;

	// Use this for initialization
	void Start () {
		shooter = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			fire ();
		}
	}

	void fire()
	{
		GameObject bulletClone = Instantiate (bullet, shotSpawn.position, shotSpawn.rotation) as GameObject;
		Vector3 bulletDirection = (shotSpawn.position - shooter.position).normalized;
		bulletClone.GetComponent<Rigidbody2D> ().velocity = bulletDirection * bulletSpeed;
	}
}
