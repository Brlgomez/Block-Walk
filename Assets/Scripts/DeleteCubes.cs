using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeleteCubes : MonoBehaviour {

	public void exitBlock (GameObject other) {
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

	public void enterBlock (GameObject other) {
		// switch block, rotate blocks
		if (other.transform.tag == "Switch") {
			Camera.main.GetComponent<SwitchAttributes> ().buttonPress ();
			Camera.main.GetComponent<SwitchAttributes> ().saveState ();
		} else if (other.transform.tag == "RotatorR") {
			GetComponent<Rotator>().rotateAt(other.transform.position, 1);
		} else if (other.transform.tag == "RotatorL") {
			GetComponent<Rotator>().rotateAt(other.transform.position, -1);
		} 
	}
}
