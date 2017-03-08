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
			Camera.main.GetComponent<SwitchAttributes>().buttonPress();
			Camera.main.GetComponent<SwitchAttributes>().saveState();
		} else if (other.transform.tag == "RotatorR") {
			Camera.main.GetComponent<SoundsAndMusic>().playRotateRightSound();
			GetComponent<Rotator>().rotateAt(other.transform.position, 1);
		} else if (other.transform.tag == "RotatorL") {
			Camera.main.GetComponent<SoundsAndMusic>().playRotateLeftSound();
			GetComponent<Rotator>().rotateAt(other.transform.position, -1);
		} else {
			float pitch = (
				other.GetComponent<Renderer>().material.color.r +
				other.GetComponent<Renderer>().material.color.g +
				other.GetComponent<Renderer>().material.color.b
			);
			pitch = Mathf.Clamp(pitch, 0.5f, 3);
			if (other.GetComponent<CrumbledBlock>() == null) {
				Camera.main.GetComponent<SoundsAndMusic>().playBlockWalkSound(pitch);
			} else {
				if (other.GetComponent<CrumbledBlock>().getSteps() <= 1) {
					Camera.main.GetComponent<SoundsAndMusic>().playBlockWalkSound(pitch);
				} else { 
					Camera.main.GetComponent<SoundsAndMusic>().playMultiStepSound(pitch);
				}
			}
		}
	}
}
