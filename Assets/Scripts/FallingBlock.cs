using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {

	private int speed = 4;
	private float timerLimit = 0.75f;
	private float timerSpeed = 1.25f;

	float timer = 0;

	void Start() {
		gameObject.GetComponent<BoxCollider>().enabled = false;
		Camera.main.GetComponent<LevelBuilder>().removeBlock(gameObject);
		if (tag == VariableManagement.active) {
			Camera.main.GetComponent<LevelBuilder>().removeRedBlock(gameObject);
		} else if (tag == VariableManagement.inactive) {
			Camera.main.GetComponent<LevelBuilder>().removeBlueBlock(gameObject);
		}
	}

	void Update() {
		float deltaTime = Time.deltaTime * timerSpeed;
		timer += deltaTime;
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, deltaTime * speed);
		if (timer > timerLimit) {
			Destroy(gameObject);
		}
	}
}
