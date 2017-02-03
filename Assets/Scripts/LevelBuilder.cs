using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelBuilder : MonoBehaviour {

	List<GameObject> blocks = new List<GameObject> ();
	float r, g, b;
	float rInc, gInc, bInc;
	bool rXorZ, gXorZ, bXorZ;
	int numberOfBlocks;
	Vector3 center;
	float xMin, xMax, zMin, zMax = 0;
	float rightMost = -100;
	float leftMost = 100;
	float topMost = -100;
	float bottomMost = 100;
	GameObject cubes;
	GameObject standardBlock;
	GameObject multistepBlock;
	GameObject switchBlock;
	GameObject redOrBlueBlock;

	void Awake () {
		cubes = GameObject.Find ("Cubes");
		standardBlock = GameObject.Find ("Standard Block");
		multistepBlock = GameObject.Find ("Multistep Block");
		switchBlock = GameObject.Find ("Switch Block");
		redOrBlueBlock = GameObject.Find ("Red or Blue Block");
		TextAsset t = new TextAsset();
		string[] level;
		string[] lines;
		if (PlayerPrefs.GetInt("Level", 0) >= 1 && PlayerPrefs.GetInt("Level", 0) <= 16) {
			t = Resources.Load("World1") as TextAsset;
		} else if (PlayerPrefs.GetInt("Level", 0) >= 17 && PlayerPrefs.GetInt("Level", 0) <= 32) {
			t = Resources.Load("World2") as TextAsset;
		} else if (PlayerPrefs.GetInt("Level", 0) >= 33 && PlayerPrefs.GetInt("Level", 0) <= 48) {
			t = Resources.Load("World3") as TextAsset;
		} 
		if (PlayerPrefs.GetInt("Level", 0) <= 1600) {
			level = t.text.Split("*"[0]);
			lines = level [(PlayerPrefs.GetInt("Level", 0) - 1) % 16].Split("\n"[0]);
		} else {
			string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("Level", 0) - 1) + ".txt";
			StreamReader r;
			if (File.Exists(filePath)) {
				r = File.OpenText(filePath);
			} else {
				File.AppendAllText(filePath, "128,128,128\n0.1,0.2,0.3\n0.05,0.03,0.015\ntrue, false, false\n");
				for (int i = 0; i < 14; i++) {
					File.AppendAllText(filePath, "--------\n");
				}
				File.AppendAllText(filePath, "*");
				r = File.OpenText(filePath);
			}
			level = r.ReadToEnd().Split("*"[0]);
			lines = level[0].Split("\n"[0]);
		}
		builder (lines);
	}

	public void builder (string[] lines) {
		setVariables(lines);
		setBlocks(lines);
		if (GetComponent<CharacterMovement>() != null) {
			setCamera();
		}
	}

	void setVariables (string[] lines) {
		string[] color = lines [0].Split (","[0]);
		string[] rgb = lines [1].Split (","[0]);
		string[] rgbInc = lines [2].Split (","[0]);
		string[] xOrZ = lines [3].Split (","[0]);
		Camera.main.backgroundColor = new Color32 (byte.Parse (color[0]), byte.Parse (color [1]), byte.Parse (color [2]), 255);
		r = float.Parse (rgb [0]);
		g = float.Parse (rgb [1]);
		b = float.Parse (rgb [2]);
		rInc = float.Parse (rgbInc [0]);
		gInc = float.Parse (rgbInc [1]);
		bInc = float.Parse (rgbInc [2]);
		rXorZ = bool.Parse (xOrZ [0]);
		gXorZ = bool.Parse (xOrZ [1]);
		bXorZ = bool.Parse (xOrZ [2]);
		if (GetComponent<CharacterMovement>() == null) {
			GetComponent<EditorInterface>().setVariables(r, g, b, rInc, gInc, bInc, rXorZ, gXorZ, bXorZ);
		}
	}

	void setBlocks (string[] lines) {
		for (int i = 4; i < lines.Length; i++) {
			for (int j = 0; j < lines[i].Length; j++) {
				if (lines [i] [j] == 'C') {
					numberOfBlocks++;
					createBlock (standardBlock, j, i).tag = "Block";
				} else if (lines [i] [j] == 'M') {
					numberOfBlocks++;
					createBlock (multistepBlock, j, i).tag = "Block";
				} else if (lines [i] [j] == 'S') {
					createBlock (switchBlock, j, i).tag = "Switch";
				} else if (lines [i] [j] == 'R') {
					numberOfBlocks++;
					createBlock (redOrBlueBlock, j, i).tag = "RedBlock";
				} else if (lines [i] [j] == 'B') {
					numberOfBlocks++;
					createBlock (redOrBlueBlock, j, i).tag = "BlueBlock";
				} 
			}
		}
	}

	GameObject createBlock (GameObject block, int x, int z) {
		GameObject temp = Instantiate(block);
		temp.layer = 8;
		if (GetComponent<CharacterMovement>() != null) {
			temp.transform.position = new Vector3(x - 3.5f, 0, -z + 10.5f);
		} else {
			temp.transform.position = new Vector3(x, 0, -z + 17);
			GetComponent<LevelEditor>().addToBlockList(x, -z + 17, temp);
		}
		addBlock (temp);
		changeBlockColor (temp);
		if ((temp.transform.position.x + temp.transform.position.z) > rightMost) {
			rightMost = (temp.transform.position.x + temp.transform.position.z);
		} if ((temp.transform.position.x + temp.transform.position.z) < leftMost) {
			leftMost = (temp.transform.position.x + temp.transform.position.z);
		}
		if ((-temp.transform.position.x + -temp.transform.position.z) > topMost) {
			topMost = (-temp.transform.position.x + -temp.transform.position.z);
		} if ((-temp.transform.position.x + -temp.transform.position.z) < bottomMost) {
			bottomMost = (-temp.transform.position.x + -temp.transform.position.z);
		}
		temp.transform.SetParent (cubes.transform);
		if (temp.transform.localPosition.x < xMin) {
			xMin = temp.transform.localPosition.x;
		} else if (temp.transform.localPosition.x > xMax) {
			xMax = temp.transform.localPosition.x;
		}
		if (temp.transform.localPosition.z < zMin) {
			zMin = temp.transform.localPosition.z;
		} else if (temp.transform.localPosition.z > zMax) {
			zMax = temp.transform.localPosition.z;
		}
		return temp;
	}

	void changeBlockColor (GameObject block) {
		float tempR, tempG, tempB;
		float xDeduct = 0;
		float zDeduct = 0;
		if (GetComponent<CharacterMovement>() == null) {
			xDeduct = -3.5f;
			zDeduct = -6.5f;
		}
		if (rXorZ) {
			tempR = r + (rInc * (block.transform.localPosition.x + xDeduct));
		} else {
			tempR = r + (rInc * (block.transform.localPosition.z + zDeduct));
		}
		if (gXorZ) {
			tempG = g + (gInc * (block.transform.localPosition.x + xDeduct));
		} else {
			tempG = g + (gInc * (block.transform.localPosition.z + zDeduct));
		}
		if (bXorZ) {
			tempB = b + (bInc * (block.transform.localPosition.x + xDeduct));
		} else {
			tempB = b + (bInc * (block.transform.localPosition.z + zDeduct));
		}
		block.GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
	}

	void setCamera () {
		float yHeight1 = Mathf.Abs (xMin) + Mathf.Abs (xMax);
		float yHeight2 = Mathf.Abs (zMin) + Mathf.Abs (zMax);
		Camera.main.orthographicSize = (rightMost + Mathf.Abs(leftMost) - (rightMost + Mathf.Abs(leftMost)) / 4.5f);
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, 14);
		if ((yHeight2 / yHeight1) < 1.75f) {
			center = new Vector3 ((xMin + xMax) / 2, yHeight1 + 0.5f, (zMin + zMax) / 2);
		} else {
			center = new Vector3 ((xMin + xMax) / 2, (yHeight2 / 2) + 1.5f, (zMin + zMax) / 2);
		}
		if (rightMost != Mathf.Abs(leftMost)) {
			Camera.main.transform.position = new Vector3 (
				Camera.main.transform.position.x + ((rightMost + leftMost) / 4), 
				Camera.main.transform.position.y, 
				Camera.main.transform.position.z + ((rightMost + leftMost) / 4)
			);
		}
		Camera.main.transform.position = new Vector3 (
			Camera.main.transform.position.x, 
			Camera.main.transform.position.y + ((topMost + bottomMost)/16), 
			Camera.main.transform.position.z
		);
	}

	public List<GameObject> getBlocks () {
		return blocks;
	}

	void addBlock (GameObject b) {
		blocks.Add (b);
	}

	public void removeBlock (GameObject b) {
		blocks.Remove (b);
		numberOfBlocks--;
	}

	public int getNumberOfBlocks () {
		return numberOfBlocks;
	}

	public Vector3 getCenter () {
		return center;
	}

	void randomColors () {
		Camera.main.backgroundColor = new Color (
			Random.Range (0.0f, 1.0f), 
			Random.Range (0.0f, 1.0f), 
			Random.Range (0.0f, 1.0f)
		);
		if (Random.Range (0.0f, 1.0f) > 0.5) {
			rXorZ = true;
		} else {
			rXorZ = false;
		}
		if (Random.Range (0.0f, 1.0f) > 0.5) {
			gXorZ = true;
		} else {
			gXorZ = false;
		}
		if (Random.Range (0.0f, 1.0f) > 0.5) {
			bXorZ = true;
		} else {
			bXorZ = false;
		}
		r = Random.Range (0.0f, 0.5f);
		g = Random.Range (0.0f, 0.5f);
		b = Random.Range (0.0f, 0.5f);
		rInc = Random.Range (-0.15f, 0.15f);
		gInc = Random.Range (-0.15f, 0.15f);
		bInc = Random.Range (-0.15f, 0.15f);
	}
}
