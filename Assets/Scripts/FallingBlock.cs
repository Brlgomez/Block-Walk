using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {

	private int speed = 4;
	float timer = 0;

	void Start () {
		gameObject.GetComponent<BoxCollider> ().enabled = false;
		Camera.main.GetComponent<BlockManagement> ().removeBlock (gameObject);
	}

	void Update () {
		float deltaTime = Time.deltaTime;
		timer += deltaTime;
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.zero, deltaTime * speed);
		if (timer > 1) {
			Destroy (gameObject);
		}
	}
}
