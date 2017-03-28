using UnityEngine;
using System.Collections;

public class RotateBlock : MonoBehaviour {

	private float timerCap = 0.25f;

	Vector3 point;
	float timer;
	bool firstBlock = false;

	void Start () {
		Camera.main.GetComponent<CharacterMovement>().setIfPlayerCanMove(false);
	}

	void Update () {
		if (timer < timerCap && PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			timer += Time.deltaTime;
			transform.position = Vector3.Slerp(transform.position, point, timer);
		} else {
			transform.position = point;
			if (Camera.main.GetComponent<CharacterMovement>() != null) {
				Camera.main.GetComponent<CharacterMovement>().setIfPlayerCanMove(true);
			}
			if (firstBlock) {
				Camera.main.GetComponent<LevelBuilder>().recalculateBlocks();
			}
			Destroy(GetComponent<RotateBlock>());
		}
	}

	public void setPoint (Vector3 p) {
		point = p;
	}

	public void firstBlockCheck () {
		firstBlock = true;
	}
}
