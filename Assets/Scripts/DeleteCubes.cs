using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeleteCubes : MonoBehaviour {

	Material standardBlock;

	void Start () {
		standardBlock = (Material)Resources.Load("Materials/Block", typeof(Material));
	}

	public void exitBlock (GameObject other) {
		if (GetComponent<LevelBuilder>().getBombBlocks().Count > 0) {
			for (int i = 0; i < GetComponent<LevelBuilder>().getBombBlocks().Count; i++) {
				GetComponent<LevelBuilder>().getBombBlocks()[i].GetComponent<BombBlock>().decreaseBombSteps();
			}
		}
		// regular, multi step, redAndBlue block
		if (other.transform.tag == VariableManagement.block || other.transform.tag == VariableManagement.active || other.transform.tag == VariableManagement.inactive) {
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
		} else if (other.transform.tag == VariableManagement.bomb) {
			other.GetComponent<BombBlock>().activateBomb();
		} else {
			float pitch = (
				other.GetComponent<Renderer>().material.color.r +
				other.GetComponent<Renderer>().material.color.g +
				other.GetComponent<Renderer>().material.color.b
			);
			pitch = Mathf.Clamp(pitch, 0.25f, 3);
			if (other.GetComponent<CrumbledBlock>() == null) {
				Camera.main.GetComponent<SoundsAndMusic>().playBlockWalkSound(pitch, other.transform.position);
			} else {
				if (other.GetComponent<CrumbledBlock>().getSteps() <= 1) {
					Camera.main.GetComponent<SoundsAndMusic>().playBlockWalkSound(pitch, other.transform.position);
				} else { 
					Camera.main.GetComponent<SoundsAndMusic>().playMultiStepSound(pitch, other.transform.position);
				}
			}
		}
	}

	public Material getStandard () {
		return standardBlock;
	}
}
