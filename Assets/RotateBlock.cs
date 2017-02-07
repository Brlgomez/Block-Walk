using UnityEngine;
using System.Collections;

public class RotateBlock : MonoBehaviour {

	private float timerCap = 0.25f;

	Vector3 point;
	float timer;

	void Update () {
		timer += Time.deltaTime;
		transform.position = Vector3.Slerp(transform.position, point, timer);
		if (timer > timerCap) {
			transform.position = point;
			Destroy (GetComponent<RotateBlock>());
		}
	}

	public void setPoint (Vector3 p) {
		point = p;
	}
}
