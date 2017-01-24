using UnityEngine;
using System.Collections;

public class SwitchAttributes : MonoBehaviour {

	bool redBlocksActive = true;
	bool redActiveSavedState = true;
	float unactiveSize = 0.25f;

	void Start () {
		buttonPress ();
		saveState ();
	}

	public void buttonPress () {
		redBlocksActive = !redBlocksActive;
		changeBlockSizes ();
	}

	public void saveState () {
		redActiveSavedState = !redActiveSavedState;
	}

	public void loadSaveState () {
		redBlocksActive = redActiveSavedState;
		changeBlockSizes ();
	}

	void changeBlockSizes () {
		GameObject[] redBlocks = GameObject.FindGameObjectsWithTag ("RedBlock");
		GameObject[] blueBlocks = GameObject.FindGameObjectsWithTag ("BlueBlock");
		for (int i = 0; i < redBlocks.Length; i++) {
			if (redBlocksActive) {
				redBlocks [i].transform.localScale = Vector3.one * unactiveSize;
				redBlocks [i].GetComponent<BoxCollider> ().size = Vector3.zero;
			} else {
				redBlocks [i].transform.localScale = Vector3.one;
				redBlocks [i].GetComponent<BoxCollider> ().size = Vector3.one;
			}
		}
		for (int i = 0; i < blueBlocks.Length; i++) {
			if (redBlocksActive) {
				blueBlocks [i].transform.localScale = Vector3.one;
				blueBlocks [i].GetComponent<BoxCollider> ().size = Vector3.one;
			} else {
				blueBlocks [i].transform.localScale = Vector3.one * unactiveSize;
				blueBlocks [i].GetComponent<BoxCollider> ().size = Vector3.zero;
			}
		}
		for (int i = 0; i < GetComponent<CharacterMovement>().getPath().Count; i++) {
			if (GetComponent<CharacterMovement>().getPath() [i].transform.localScale.x < 1) {
				GetComponent<CharacterMovement>().getPath() [i].GetComponent<BoxCollider> ().size = Vector3.one * 4;
			}
		}
	}
}
