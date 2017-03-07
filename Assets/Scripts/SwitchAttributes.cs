using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchAttributes : MonoBehaviour {

	bool redBlocksActive = true;
	bool redActiveSavedState = true;
	bool playSound = false;

	void Start () {
		buttonPress ();
		saveState ();
	}

	public void buttonPress () {
		redBlocksActive = !redBlocksActive;
		changeBlockSizes ();
		playSound = true;
	}

	public void saveState () {
		redActiveSavedState = !redActiveSavedState;
	}

	public void loadSaveState () {
		redBlocksActive = redActiveSavedState;
		changeBlockSizes ();
	}

	public bool getState () {
		return redBlocksActive;
	}

	void changeBlockSizes () {
		List<GameObject> redBlocks = GetComponent<LevelBuilder>().getRedBlocks();
		List<GameObject> blueBlocks = GetComponent<LevelBuilder>().getBlueBlocks();
		if (playSound) {
			if (redBlocksActive) {
				GetComponent<SoundsAndMusic>().playOnSwitchSound();
			} else {
				GetComponent<SoundsAndMusic>().playOffSwitchSound();
			}
		}
		for (int i = 0; i < redBlocks.Count; i++) {
			if (redBlocks [i].GetComponent<FallingBlock> () == null) {
				if (redBlocksActive) {
					redBlocks [i].GetComponent<BoxCollider> ().size = Vector3.zero;
				} else {
					redBlocks [i].GetComponent<BoxCollider> ().size = Vector3.one;
				}
			}
		}
		for (int i = 0; i < blueBlocks.Count; i++) {
			if (blueBlocks [i].GetComponent<FallingBlock> () == null) {
				if (redBlocksActive) {
					blueBlocks [i].GetComponent<BoxCollider> ().size = Vector3.one;
				} else {
					blueBlocks [i].GetComponent<BoxCollider> ().size = Vector3.zero;
				}
			}
		}
		for (int i = 0; i < GetComponent<CharacterMovement> ().getPath ().Count; i++) {
			if (GetComponent<CharacterMovement> ().getPath () [i].transform.localScale.x < 1) {
				GetComponent<CharacterMovement> ().getPath () [i].GetComponent<BoxCollider> ().size = Vector3.one * 4;
			}
		}
		if (gameObject.GetComponent<SwitchScaling>() == null && (redBlocks.Count > 0 || blueBlocks.Count > 0)) {
			gameObject.AddComponent<SwitchScaling>();
		}
	}
}
