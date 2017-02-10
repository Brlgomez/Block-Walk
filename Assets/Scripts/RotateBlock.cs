using UnityEngine;
using System.Collections;

public class RotateBlock : MonoBehaviour {

	private float timerCap = 0.25f;

	Vector3 point;
	float timer;

	void Start () {
		Camera.main.GetComponent<CharacterMovement>().setIfPlayerCanMove(false);
	}

	void Update () {
		timer += Time.deltaTime;
		transform.position = Vector3.Slerp(transform.position, point, timer);
		if (timer > timerCap) {
			transform.position = point;
			Vector4 temp = Camera.main.GetComponent<LevelBuilder>().getMinAndMax();
			if (point.x < temp.x) {
				Camera.main.GetComponent<LevelBuilder>().setMinAndMax();
			} else if (point.x > temp.y) {
				Camera.main.GetComponent<LevelBuilder>().setMinAndMax();
			}
			if (point.z < temp.z) {
				Camera.main.GetComponent<LevelBuilder>().setMinAndMax();
			} else if (point.z > temp.w) {
				Camera.main.GetComponent<LevelBuilder>().setMinAndMax();
			}
			Destroy (GetComponent<RotateBlock>());
			Camera.main.GetComponent<CharacterMovement>().setIfPlayerCanMove(true);
		}
	}

	public void setPoint (Vector3 p) {
		point = p;
	}
}
