using UnityEngine;
using System.Collections;

public class EnemyMovements : MonoBehaviour {
	
	[Range(0.1f, 10f)]
	public float radius = 1;

	[Range(1.0f, 360f)]
	public int fov = 90;//90 degrees

	public Vector2 direction = Vector2.up;

	public Transform testPoint;

	private GameObject player;

	private Vector2 leftLineFOV;
	private Vector2 rightLineFOV;

	public GameObject exMark;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update() {
		if(testPoint != null) {
			rightLineFOV = RotatePointAroundTransform(direction.normalized*radius, -fov/2);
			leftLineFOV = RotatePointAroundTransform(direction.normalized*radius, fov/2);
		}
		exMark.SetActive (InsideFOV(player.transform.position));
	}

	public bool InsideFOV(Vector3 playerPos) {
		float squaredDistance = ((playerPos.x - transform.position.x)*(playerPos.x - transform.position.x)) + ((playerPos.y-transform.position.y)*(playerPos.y-transform.position.y));
		//Debug.Log(squaredDistance);
		if(radius * radius >= squaredDistance) {
			float signLeftLine = (leftLineFOV.x) * (playerPos.y - transform.position.y) - (leftLineFOV.y) * (playerPos.x-transform.position.x);
			float signRightLine = (rightLineFOV.x) * (playerPos.y - transform.position.y) - (rightLineFOV.y) * (playerPos.x-transform.position.x);
			if(fov <= 180) {
				//Debug.Log(signLeftLine + " " + signRightLine);
				if(signLeftLine <= 0 && signRightLine >= 0)
					return true;
			} else {
				if(!(signLeftLine >= 0 && signRightLine <= 0))
					return true;
			}
		}
		return false;
	}

	//Rotate point (px, py) around point (ox, oy) by angle theta you'll get:
	//p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
	//p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy
	private Vector2 RotatePointAroundTransform(Vector2 p, float angles) {
		return new Vector2(Mathf.Cos((angles)  * Mathf.Deg2Rad) * (p.x) - Mathf.Sin((angles) * Mathf.Deg2Rad) * (p.y),
			Mathf.Sin((angles)  * Mathf.Deg2Rad) * (p.x) + Mathf.Cos((angles) * Mathf.Deg2Rad) * (p.y));
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, direction.normalized*radius);

		rightLineFOV = RotatePointAroundTransform(direction.normalized*radius, -fov/2);
		leftLineFOV = RotatePointAroundTransform(direction.normalized*radius, fov/2);

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, rightLineFOV);
		Gizmos.DrawRay(transform.position, leftLineFOV);

		Vector2 p = rightLineFOV;
		for(int i = 1; i <= 20; i++) {
			float step = fov/20;
			Vector2 p1 = RotatePointAroundTransform(direction.normalized*radius, -fov/2 + step*(i));
			Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y) + p, p1-p);
			p = p1;
		}
		Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y) + p, leftLineFOV - p);
	}
}
