using UnityEngine;
using System.Collections;

public class CameraBackground : MonoBehaviour {
	public Color color1 = Color.white;
	public Color color2 = Color.blue;
	public float duration = 3.0F;

	Camera camera;

	void Start() {
		camera = GetComponent<Camera>();
		camera.clearFlags = CameraClearFlags.SolidColor;
	}

	void Update() {
		float t = Mathf.PingPong(Time.time, duration) / duration;
		camera.backgroundColor = Color.white;
	}
}
