﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class EditorInterface : MonoBehaviour {

	GameObject menuHolder;
	GameObject colorHolder;
	GameObject blockHolder;

	GameObject r, g, b;
	GameObject rB, gB, bB;
	GameObject rInc, gInc, bInc;
	GameObject rXorZ, gXorZ, bXorZ;

	bool menuOn = true;

	private string filePath;

	void Start () {
		filePath = Application.persistentDataPath + "/Editor.txt";
		menuHolder = GameObject.Find("Menu Holder");
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
		menuHolder.transform.position = Vector3.one * 1000;
		blockHolder.transform.position = Vector3.zero;
		colorHolder.transform.position = Vector3.one * 1000;
	}

	void colorMenu (Color c, bool b) {
		menuHolder.transform.position = Vector3.one * 1000;
		blockHolder.transform.position = Vector3.one * 1000;
		colorHolder.transform.position = Vector3.zero;
	}

	void mainMenu (Color c, bool b) {
		menuHolder.transform.position = Vector3.zero;
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
						tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.x - 3.5f));
					} else {
						tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.z - 6.5f));
					}
					if (gXorZ.GetComponent<Toggle>().isOn) {
						tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.x - 3.5f));
					} else {
						tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.z - 6.5f));
					}
					if (bXorZ.GetComponent<Toggle>().isOn) {
						tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.x - 3.5f));
					} else {
						tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (blocks[i][j].transform.position.z - 6.5f));
					}
					blocks[i][j].GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
				}
			}
		}
	}

	public void changeBlockColor (GameObject block) {
		float tempR, tempG, tempB;
		if (rXorZ.GetComponent<Toggle>().isOn) {
			tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (block.transform.position.x - 3.5f));
		} else {
			tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (block.transform.position.z - 6.5f));
		}
		if (gXorZ.GetComponent<Toggle>().isOn) {
			tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (block.transform.position.x - 3.5f));
		} else {
			tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (block.transform.position.z - 6.5f));
		}
		if (bXorZ.GetComponent<Toggle>().isOn) {
			tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (block.transform.position.x - 3.5f));
		} else {
			tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (block.transform.position.z - 6.5f));
		}
		block.GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
	}

	public void printLevel () {
		//TODO: save level into txt file
		List<List<GameObject>> blocks = GetComponent<LevelEditor>().getBlocks();
		File.Delete(filePath);
		File.AppendAllText(filePath, Mathf.RoundToInt(Camera.main.backgroundColor.r * 255) + "," + Mathf.RoundToInt(Camera.main.backgroundColor.b * 255) + "," + Mathf.RoundToInt(Camera.main.backgroundColor.b * 255));
		File.AppendAllText(filePath, "\n");
		File.AppendAllText(filePath, rB.GetComponent<Slider>().value + "," + gB.GetComponent<Slider>().value + "," + bB.GetComponent<Slider>().value);
		File.AppendAllText(filePath, "\n");
		File.AppendAllText(filePath, rInc.GetComponent<Slider>().value + "," + gInc.GetComponent<Slider>().value + "," + bInc.GetComponent<Slider>().value);
		File.AppendAllText(filePath, "\n");
		File.AppendAllText(filePath, rXorZ.GetComponent<Toggle>().isOn + "," + gXorZ.GetComponent<Toggle>().isOn + "," + bXorZ.GetComponent<Toggle>().isOn);
		File.AppendAllText(filePath, "\n");
		for (int i = 13; i >= 0; i--) {
			for (int j = 0; j < 7; j++) { 
				if (blocks[i][j] == null) {
					File.AppendAllText(filePath, "-");
				} else if (blocks[i][j].name == "Standard Block(Clone)") {
					File.AppendAllText(filePath, "C");
				} else if (blocks[i][j].name == "Multistep Block(Clone)") {
					File.AppendAllText(filePath, "M");
				} else if (blocks[i][j].name == "Switch Block(Clone)") {
					File.AppendAllText(filePath, "S");
				} else if (blocks[i][j].name == "Red Block(Clone)") {
					File.AppendAllText(filePath, "R");
				} else if (blocks[i][j].name == "Blue Block(Clone)") {
					File.AppendAllText(filePath, "B");
				}
			}
			File.AppendAllText(filePath, "\n");
		}
		File.AppendAllText(filePath, "*");
	}
}
