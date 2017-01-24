using UnityEngine;
using System.Collections;

public class DeleteCubes : MonoBehaviour {

	GameObject currentlyOn;

	void OnTriggerExit(Collider other) {
		// regular, multi step, redAndBlue block
		if (other.transform.tag == "Block" || other.transform.tag == "RedBlock" || other.transform.tag == "BlueBlock") {
			if (other.GetComponent<CrumbledBlock> () != null) {
				other.GetComponent<CrumbledBlock> ().decreaseSteps ();
				if (other.GetComponent<CrumbledBlock> ().getSteps () <= 0) {
					other.gameObject.AddComponent<FallingBlock> ();
				}
			} else {
				other.gameObject.AddComponent<FallingBlock> ();
			}
		} 
	}

	void OnTriggerEnter(Collider other) {
		// switch block
		currentlyOn = other.gameObject;
		if (other.transform.tag == "Switch") {
			Camera.main.GetComponent<SwitchAttributes> ().buttonPress ();
			Camera.main.GetComponent<SwitchAttributes> ().saveState ();
		}
	}

	public GameObject playerCurrentlyOn () {
		return currentlyOn;
	}
}
