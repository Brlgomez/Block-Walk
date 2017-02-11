using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {

	private int speed = 4;
	float timer = 0;
	float timerLimit = 0.75f;

	void Start () {
		gameObject.GetComponent<BoxCollider> ().enabled = false;
		Camera.main.GetComponent<LevelBuilder> ().removeBlock (gameObject);
		if (name == "Red Block(Clone)") {
			Camera.main.GetComponent<LevelBuilder> ().removeRedBlock (gameObject);
		} else if (name == "Blue Block(Clone)") {
			Camera.main.GetComponent<LevelBuilder> ().removeBlueBlock (gameObject);
		}
	}

	void Update () {
		float deltaTime = Time.deltaTime * 1.25f;
		timer += deltaTime;
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.zero, deltaTime * speed);
		if (timer > timerLimit) {
			Destroy (gameObject);
		}
	}
}
