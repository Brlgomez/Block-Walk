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
		// regular, multi step, redAndBlue block, resize
		if (other.transform.tag == VariableManagement.block || other.transform.tag == VariableManagement.active || 
			other.transform.tag == VariableManagement.inactive || other.transform.tag == VariableManagement.resizeSmall ||
			other.transform.tag == VariableManagement.resizeNormal || other.transform.tag == VariableManagement.resizeBig) {
			if (other.GetComponent<CrumbledBlock> () != null) {
				other.GetComponent<CrumbledBlock> ().decreaseSteps ();
				if (other.GetComponent<CrumbledBlock> ().getSteps () <= 0) {
					other.gameObject.AddComponent<FallingBlock> ();
				}
			} else {
				other.gameObject.AddComponent<FallingBlock> ();
			} 
			if (other.transform.tag == VariableManagement.resizeSmall || other.transform.tag == VariableManagement.resizeNormal || 
				other.transform.tag == VariableManagement.resizeBig) {
				Destroy(other.transform.GetChild(0).gameObject);
			}
		}
	}

	public void enterBlock (GameObject other) {
		// switch block, rotate blocks
		if (other.transform.tag == VariableManagement.switchTag) {
			Camera.main.GetComponent<SwitchAttributes>().buttonPress(GetComponent<SoundsAndMusic>().getPitchOfBlock(other), other.transform.position);
			Camera.main.GetComponent<SwitchAttributes>().saveState();
		} else if (other.transform.tag == VariableManagement.rotateR) {
			Camera.main.GetComponent<SoundsAndMusic>().playRotateRightSound(GetComponent<SoundsAndMusic>().getPitchOfBlock(other), other.transform.position);
			GetComponent<Rotator>().rotateAt(other.transform.position, 1);
		} else if (other.transform.tag == VariableManagement.rotateL) {
			Camera.main.GetComponent<SoundsAndMusic>().playRotateLeftSound(GetComponent<SoundsAndMusic>().getPitchOfBlock(other), other.transform.position);
			GetComponent<Rotator>().rotateAt(other.transform.position, -1);
		} else if (other.transform.tag == VariableManagement.bomb) {
			other.GetComponent<BombBlock>().activateBomb();
		} else {
			if (other.GetComponent<CrumbledBlock>() == null) {
				Camera.main.GetComponent<SoundsAndMusic>().playBlockWalkSound(GetComponent<SoundsAndMusic>().getPitchOfBlock(other), other.transform.position);
			} else {
				if (other.GetComponent<CrumbledBlock>().getSteps() <= 1) {
					Camera.main.GetComponent<SoundsAndMusic>().playBlockWalkSound(GetComponent<SoundsAndMusic>().getPitchOfBlock(other), other.transform.position);
				} else { 
					Camera.main.GetComponent<SoundsAndMusic>().playMultiStepSound(GetComponent<SoundsAndMusic>().getPitchOfBlock(other), other.transform.position);
				}
			}
		}
	}

	public Material getStandard () {
		return standardBlock;
	}
}
