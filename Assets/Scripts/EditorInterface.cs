using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EditorInterface : MonoBehaviour {

	GameObject changeBlock;
	GameObject changeColors;

	GameObject colorHolder;
	GameObject blockHolder;

	GameObject r, g, b;
	GameObject rB, gB, bB;
	GameObject rInc, gInc, bInc;
	GameObject rXorZ, gXorZ, bXorZ;

	bool menuOn = true;

	void Start () {
		changeBlock = GameObject.Find("Change Block");
		changeColors = GameObject.Find("Change Colors");
		colorHolder = GameObject.Find("Color Holder");
		blockHolder = GameObject.Find("Block Holder");
		r = GameObject.Find("R");
		g = GameObject.Find("G");
		b = GameObject.Find("B");
		rB = GameObject.Find("Block R");
		gB = GameObject.Find("Block G");
		bB = GameObject.Find("Block B");
		rInc = GameObject.Find("R Inc");
		gInc = GameObject.Find("G Inc");
		bInc = GameObject.Find("B Inc");
		rXorZ = GameObject.Find("R Toggle");
		gXorZ = GameObject.Find("G Toggle");
		bXorZ = GameObject.Find("B Toggle");
		showMain();
	}

	public void showMain () {
		menuOn = true;
		blockMenu(Color.clear, false);
		colorMenu(Color.clear, false);
		mainMenu(Color.white, true);
	}

	public void showBlockMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		colorMenu(Color.clear, false);
		blockMenu(Color.white, true);
	}

	public void showColorMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		blockMenu(Color.clear, false);
		colorMenu(Color.white, true);
	}

	void blockMenu (Color c, bool b) {
		blockHolder.transform.position = Vector3.zero;
		colorHolder.transform.position = Vector3.one * 1000;
	}

	void colorMenu (Color c, bool b) {
		blockHolder.transform.position = Vector3.one * 1000;
		colorHolder.transform.position = Vector3.zero;
	}

	void mainMenu (Color c, bool b) {
		changeBlock.GetComponent<Image> ().raycastTarget = b;
		changeBlock.GetComponent<Button> ().image.color = c;
		changeBlock.GetComponentInChildren<Text> ().color = c;
		changeBlock.GetComponentInChildren<Text> ().raycastTarget = b;
		changeColors.GetComponent<Image> ().raycastTarget = b;
		changeColors.GetComponent<Button> ().image.color = c;
		changeColors.GetComponentInChildren<Text> ().color = c;
		changeColors.GetComponentInChildren<Text> ().raycastTarget = b;
		blockHolder.transform.position = Vector3.one * 1000;
		colorHolder.transform.position = Vector3.one * 1000;
	}

	public bool isMenuOn () {
		return menuOn;
	}

	public void changeColor () {
		Camera.main.backgroundColor = new Color(r.GetComponent<Slider>().value, g.GetComponent<Slider>().value, b.GetComponent<Slider>().value);
	}

	public void changeBlockColors () {
		List<List<GameObject>> blocks = GetComponent<LevelEditor>().getBlocks();
		for (int i = 0; i < 14; i++) {
			for (int j = 0; j < 8; j++) { 
				if (blocks[i][j] != null) {
					float tempR, tempG, tempB;
					if (rXorZ.GetComponent<Toggle>().isOn) {
						tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.x + 1));
					} else {
						tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.z + 1));
					}
					if (gXorZ.GetComponent<Toggle>().isOn) {
						tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.x + 1));
					} else {
						tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.z + 1));
					}
					if (bXorZ.GetComponent<Toggle>().isOn) {
						tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.x + 1));
					} else {
						tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.z + 1));
					}
					blocks[i][j].GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
				}
			}
		}
	}

	public void changeBlockColor (GameObject block) {
		float tempR, tempG, tempB;
		if (rXorZ.GetComponent<Toggle>().isOn) {
			tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (block.transform.position.x + 1));
		} else {
			tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (block.transform.position.z + 1));
		}
		if (gXorZ.GetComponent<Toggle>().isOn) {
			tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (block.transform.position.x + 1));
		} else {
			tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (block.transform.position.z + 1));
		}
		if (bXorZ.GetComponent<Toggle>().isOn) {
			tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (block.transform.position.x + 1));
		} else {
			tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (block.transform.position.z + 1));
		}
		block.GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
	}
}
