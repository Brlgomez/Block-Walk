using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelBuilder : MonoBehaviour {

	private int startOrthoMin = 5;
	private int startOrthoMax = 14;
	private int endOrthoMin = 4;
	private int endOrthoMax = 8;

	List<GameObject> blocks = new List<GameObject> ();
	float r, g, b;
	float rIncX, gIncX, bIncX;
	float rIncZ, gIncZ, bIncZ;
	int numberOfBlocks;
	Vector3 center;
	float xMin = 100;
	float xMax = -100;
	float zMin = 100;
	float zMax = -100;
	float rightMost = -100;
	float leftMost = 100;
	float topMost = -100;
	float bottomMost = 100;
	GameObject cubes;
	GameObject standardBlock;
	GameObject multistepBlock;
	GameObject switchBlock;
	GameObject redBlock;
	GameObject blueBlock;
	GameObject rotatorR;
	GameObject rotatorL;

	void Awake () {
		cubes = GameObject.Find ("Cubes");
		standardBlock = GameObject.Find ("Standard Block");
		multistepBlock = GameObject.Find ("Multistep Block");
		switchBlock = GameObject.Find ("Switch Block");
		rotatorR = GameObject.Find ("Rotate Block R");
		rotatorL = GameObject.Find ("Rotate Block L");
		redBlock = GameObject.Find("Red Block");
		blueBlock = GameObject.Find("Blue Block");
		builder (parseFile());
	}

	string [] parseFile () {
		TextAsset t = new TextAsset();
		string[] level;
		if (PlayerPrefs.GetString("Last Menu") == "Campaign" && GetComponent<CharacterMovement>() != null) {
			if (PlayerPrefs.GetInt("Level", 0) >= 1 && PlayerPrefs.GetInt("Level", 0) <= 16) {
				t = Resources.Load("World1") as TextAsset;
			} else if (PlayerPrefs.GetInt("Level", 0) >= 17 && PlayerPrefs.GetInt("Level", 0) <= 32) {
				t = Resources.Load("World2") as TextAsset;
			} else if (PlayerPrefs.GetInt("Level", 0) >= 33 && PlayerPrefs.GetInt("Level", 0) <= 48) {
				t = Resources.Load("World3") as TextAsset;
			}
			level = t.text.Split("*"[0]);
			return level [(PlayerPrefs.GetInt("Level", 0) - 1) % 16].Split("\n"[0]);
		} else {
			string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
			StreamReader r;
			if (File.Exists(filePath)) {
				r = File.OpenText(filePath);
			} else {
				File.AppendAllText(filePath, "160,192,224\n0.1,0.2,0.3\n0.05,0,0\n0,0.03,0.015\n");
				for (int i = 0; i < 14; i++) {
					File.AppendAllText(filePath, "--------\n");
				}
				File.AppendAllText(filePath, "*");
				r = File.OpenText(filePath);
			}
			level = r.ReadToEnd().Split("*"[0]);
			return level[0].Split("\n"[0]);
		}
	}

	public void builder (string[] lines) {
		setVariables(lines);
		setBlocks(lines);
		if (GetComponent<CharacterMovement>() != null) {
			setCamera();
		}
	}

	void setVariables (string[] lines) {
		string[] bg = lines [0].Split (","[0]);
		string[] blockRGB = lines [1].Split (","[0]);
		string[] rgbIncX = lines [2].Split (","[0]);
		string[] rgbIncZ = lines [3].Split (","[0]);
		Camera.main.backgroundColor = new Color32 (byte.Parse (bg [0]), byte.Parse (bg [1]), byte.Parse (bg [2]), 255);
		r = float.Parse (blockRGB [0]);
		g = float.Parse (blockRGB [1]);
		b = float.Parse (blockRGB [2]);
		rIncX = float.Parse (rgbIncX [0]);
		gIncX = float.Parse (rgbIncX [1]);
		bIncX = float.Parse (rgbIncX [2]);
		rIncZ = float.Parse (rgbIncZ [0]);
		gIncZ = float.Parse (rgbIncZ [1]);
		bIncZ = float.Parse (rgbIncZ [2]);
		if (GetComponent<CharacterMovement>() == null) {
			GetComponent<EditorInterface>().setVariables(r, g, b, rIncX, gIncX, bIncX, rIncZ, gIncZ, bIncZ);
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
					createBlock(redBlock, j, i).tag = "RedBlock";
				} else if (lines [i] [j] == 'B') {
					numberOfBlocks++;
					createBlock(blueBlock, j, i).tag = "BlueBlock";
				} else if (lines [i] [j] == 'E') {
					createBlock (rotatorR, j, i).tag = "RotatorR";
				} else if (lines [i] [j] == 'W') {
					createBlock (rotatorL, j, i).tag = "RotatorL";
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
		} 
		if ((temp.transform.position.x + temp.transform.position.z) < leftMost) {
			leftMost = (temp.transform.position.x + temp.transform.position.z);
		}
		if ((-x + 4 + -z + 10) > topMost) {
			topMost = (-x + 4 + -z + 10);
		} 
		if ((-x + 4 + -z + 10) < bottomMost) {
			bottomMost = (-x + 4 + -z + 10);
		}
		if (temp.transform.localPosition.x < xMin) {
			xMin = temp.transform.localPosition.x;
		} 
		if (temp.transform.localPosition.x > xMax) {
			xMax = temp.transform.localPosition.x;
		}
		if (temp.transform.localPosition.z < zMin) {
			zMin = temp.transform.localPosition.z;
		} 
		if (temp.transform.localPosition.z > zMax) {
			zMax = temp.transform.localPosition.z;
		}
		temp.transform.SetParent (cubes.transform);
		return temp;
	}

	void setCamera () {
		float yHeight1 = Mathf.Abs (xMin - xMax);
		float yHeight2 = Mathf.Abs (zMin - zMax);
		Camera.main.orthographicSize = ((rightMost - leftMost) - (rightMost - leftMost) / 4.5f);
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, startOrthoMin, startOrthoMax);
		if ((yHeight2 / yHeight1) < 1.625f) {
			center = new Vector3 ((xMin + xMax) / 2, Mathf.Clamp(yHeight1 + 0.5f, endOrthoMin, endOrthoMax), (zMin + zMax) / 2);
		} else {
			center = new Vector3 ((xMin + xMax) / 2, Mathf.Clamp((yHeight2 / 2) + 1.25f, endOrthoMin, endOrthoMax), (zMin + zMax) / 2);
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
			Camera.main.transform.position.y + ((topMost + bottomMost) * 0.6f), 
			Camera.main.transform.position.z
		);
	}

	public void changeBlockColor (GameObject block) {
		float tempR, tempG, tempB;
		float xDeduct = block.transform.localPosition.x;
		float zDeduct = block.transform.localPosition.z;
		if (GetComponent<CharacterMovement>() == null) {
			xDeduct += -3.5f;
			zDeduct += -6.5f;
		}
		tempR = r + ((rIncX * xDeduct) + (rIncZ * zDeduct));
		tempG = g + ((gIncX * xDeduct) + (gIncZ * zDeduct));
		tempB = b + ((bIncX * xDeduct) + (bIncZ * zDeduct));
		if (block.name == "Multistep Block(Clone)" && GetComponent<CharacterMovement>() == null) {
			tempR = ((tempR + Camera.main.backgroundColor.r) / 2);
			tempG = ((tempG + Camera.main.backgroundColor.g) / 2);
			tempB = ((tempB + Camera.main.backgroundColor.b) / 2);
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB, 0.75f);
		} else {
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB);
		}
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
}
