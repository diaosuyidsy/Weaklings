using UnityEngine;
using System.Collections;

using Panda;

public class _bt_EnemyMove : MonoBehaviour {
	public float speed = 0.01f;
	private int t = 0;
	[Task]
	void MoveTo(float x, float y){
		float posx = 0, posy = 0;
		if (t == 0) {
			posx = transform.position.x;
			posy = transform.position.y;
		}
		t++;
		Vector3 destination = new Vector3(posx + x, posy + y, 0.0f);
		Vector3 delta = destination - transform.position;
		Vector3 velocity = speed * delta.normalized;

		transform.position = transform.position + velocity * Time.deltaTime;

		Vector3 newDelta = (destination - transform.position);
		float d = newDelta.magnitude;

		if (Task.isInspected)
			Task.current.debugInfo = string.Format("d={0:0.000}", d);

		if ( Vector3.Dot(delta, newDelta) <= 0.0f || d < 1e-3)
		{
			transform.position = destination;
			Task.current.Succeed();
			d = 0.0f;
			Task.current.debugInfo = "d=0.000";
		}

	}
}

