using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchScaling : MonoBehaviour {

	static float unactiveSize = 0.25f;
	static int speed = 4;

	List<GameObject> redBlocks;
	List<GameObject> blueBlocks;
	float timer = 0;
	float timerLimit = 0.25f;

	void Start () {
		redBlocks = GetComponent<LevelBuilder>().getRedBlocks();
		blueBlocks = GetComponent<LevelBuilder>().getBlueBlocks();
		for (int i = 0; i < redBlocks.Count; i++) {
			if (redBlocks[i] != null) {
				if (redBlocks[i].GetComponent<FallingBlock>() == null) {
					if (GetComponent<SwitchAttributes>().getState()) {
						redBlocks[i].GetComponent<BoxCollider> ().size = Vector3.zero;
					} else {
						redBlocks[i].GetComponent<BoxCollider>().size = Vector3.one;
					}
				}
			}
		}
		for (int i = 0; i < blueBlocks.Count; i++) {
			if (blueBlocks[i] != null) {
				if (blueBlocks[i].GetComponent<FallingBlock>() == null) {
					if (GetComponent<SwitchAttributes>().getState()) {
						blueBlocks[i].GetComponent<BoxCollider> ().size = Vector3.one;
					} else {
						blueBlocks[i].GetComponent<BoxCollider> ().size = Vector3.zero;
					}
				}
			}
		}
		for (int i = 0; i < GetComponent<CharacterMovement> ().getPath ().Count; i++) {
			if (GetComponent<CharacterMovement> ().getPath () [i].transform.localScale.x < 1) {
				GetComponent<CharacterMovement> ().getPath () [i].GetComponent<BoxCollider> ().size = Vector3.one * 4;
			}
		}
	}
	
	void Update () {
		timer += Time.deltaTime * speed;
		if (timer < timerLimit) {
			for (int i = 0; i < redBlocks.Count; i++) {
				if (redBlocks[i] != null) {
					if (redBlocks[i].GetComponent<FallingBlock>() == null) {
						if (GetComponent<SwitchAttributes>().getState()) {
							redBlocks[i].transform.localScale = Vector3.Slerp(redBlocks[i].transform.localScale, Vector3.one * unactiveSize, timer);
						} else {
							redBlocks[i].transform.localScale = Vector3.Slerp(redBlocks[i].transform.localScale, Vector3.one, timer);
						}
					}
				}
			}
			for (int i = 0; i < blueBlocks.Count; i++) {
				if (blueBlocks[i] != null) {
					if (blueBlocks[i].GetComponent<FallingBlock>() == null) {
						if (GetComponent<SwitchAttributes>().getState()) {
							blueBlocks[i].transform.localScale = Vector3.Slerp(blueBlocks[i].transform.localScale, Vector3.one, timer);
						} else {
							blueBlocks[i].transform.localScale = Vector3.Slerp(blueBlocks[i].transform.localScale, Vector3.one * unactiveSize, timer);
						}
					}
				}
			}
		} else {
			for (int i = 0; i < redBlocks.Count; i++) {
				if (redBlocks[i] != null) {
					if (redBlocks[i].GetComponent<FallingBlock>() == null) {
						if (GetComponent<SwitchAttributes>().getState()) {
							redBlocks[i].transform.localScale = Vector3.one * unactiveSize;
							redBlocks[i].GetComponent<BoxCollider> ().size = Vector3.zero;
						} else {
							redBlocks[i].GetComponent<BoxCollider>().size = Vector3.one;
							redBlocks[i].transform.localScale = Vector3.one;
						}
					}
				}
			}
			for (int i = 0; i < blueBlocks.Count; i++) {
				if (blueBlocks[i] != null) {
					if (blueBlocks[i].GetComponent<FallingBlock>() == null) {
						if (GetComponent<SwitchAttributes>().getState()) {
							blueBlocks[i].GetComponent<BoxCollider> ().size = Vector3.one;
							blueBlocks[i].transform.localScale = Vector3.one;
						} else {
							blueBlocks[i].GetComponent<BoxCollider> ().size = Vector3.zero;
							blueBlocks[i].transform.localScale = Vector3.one * unactiveSize;
						}
					}
				}
			}
			for (int i = 0; i < GetComponent<CharacterMovement> ().getPath ().Count; i++) {
				if (GetComponent<CharacterMovement> ().getPath () [i].transform.localScale.x < 1) {
					GetComponent<CharacterMovement> ().getPath () [i].GetComponent<BoxCollider> ().size = Vector3.one * 4;
				}
			}
			Destroy(GetComponent<SwitchScaling>());
		}
	}
}
