using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelBuilder : MonoBehaviour {

	static float blockAlpha = 0.75f;
	static int startOrthoMin = 5;
	static int startOrthoMax = 14;
	static int endOrthoMin = 4;
	static int endOrthoMax = 8;

	List<GameObject> blocks = new List<GameObject>();
	List<GameObject> redBlocks = new List<GameObject>();
	List<GameObject> blueBlocks = new List<GameObject>();
	List<GameObject> bombBlocks = new List<GameObject>();
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
	GameObject cubes, standardBlock, multistepBlock, switchBlock, redBlock, blueBlock, rotatorR, rotatorL, bombBlock;

	void Awake() {
		cubes = GameObject.Find("Cubes");
		standardBlock = GameObject.Find(VariableManagement.standardBlock);
		multistepBlock = GameObject.Find(VariableManagement.multistepBlock);
		switchBlock = GameObject.Find(VariableManagement.switchBlock);
		rotatorR = GameObject.Find(VariableManagement.rotateRBlock);
		rotatorL = GameObject.Find(VariableManagement.rotateLBlock);
		redBlock = GameObject.Find(VariableManagement.activeBlock);
		blueBlock = GameObject.Find(VariableManagement.inactiveBlock);
		bombBlock = GameObject.Find(VariableManagement.bombBlock);
		builder(parseFile());
	}

	string [] parseFile() {
		TextAsset t = new TextAsset();
		string[] level;
		if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.worldMenu && GetComponent<CharacterMovement>() != null) {
			if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 0) {
				t = Resources.Load(VariableManagement.world1) as TextAsset;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 1) {
				t = Resources.Load(VariableManagement.world2) as TextAsset;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 2) {
				t = Resources.Load(VariableManagement.world3) as TextAsset;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 3) {
				t = Resources.Load(VariableManagement.world4) as TextAsset;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 4) {
				t = Resources.Load(VariableManagement.world5) as TextAsset;
			}
			level = t.text.Split(VariableManagement.levelDelimiter.ToString()[0]);
			return level[(GetComponent<VariableManagement>().getWorldLevel() - 1) % 16].Split("\n"[0]);
		} else {
			string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
			StreamReader r;
			if (File.Exists(filePath)) {
				r = File.OpenText(filePath);
			} else {
				if (PlayerPrefs.GetString(VariableManagement.userName) == "") {
					File.AppendAllText(
						filePath, "Untitled\nUnknown\n" + 
						MenuColors.editorInterface.r + "," + MenuColors.editorInterface.g + "," + MenuColors.editorInterface.b + 
						"\n0.1,0.2,0.3\n0.05,0,0\n0,0.03,0.015\n"
					);
				} else {
					File.AppendAllText(
						filePath, "Untitled\n" + PlayerPrefs.GetString(VariableManagement.userName) + "\n" + 
						MenuColors.editorInterface.r + "," + MenuColors.editorInterface.g + "," + MenuColors.editorInterface.b + 
						"\n0.1,0.2,0.3\n0.05,0,0\n0,0.03,0.015\n"
					);
				}
				for (int i = 0; i < 14; i++) {
					File.AppendAllText(filePath, "--------\n");
				}
				File.AppendAllText(filePath, VariableManagement.levelDelimiter.ToString());
				r = File.OpenText(filePath);
			}
			level = r.ReadToEnd().Split(VariableManagement.levelDelimiter.ToString()[0]);
			return level[0].Split("\n"[0]);
		}
	}

	public void builder(string[] lines) {
		setVariables(lines);
		setBlocks(lines);
		if (GetComponent<CharacterMovement>() != null) {
			setCamera();
		}
	}

	void setVariables(string[] lines) {
		string title = lines[0];
		PlayerPrefs.SetString(VariableManagement.userMapName, lines[0]);
		string[] bg = lines[2].Split(","[0]);
		string[] blockRGB = lines[3].Split(","[0]);
		string[] rgbIncX = lines[4].Split(","[0]);
		string[] rgbIncZ = lines[5].Split(","[0]);
		Camera.main.backgroundColor = new Color32(byte.Parse(bg[0]), byte.Parse(bg[1]), byte.Parse(bg[2]), 255);
		r = float.Parse(blockRGB[0]);
		g = float.Parse(blockRGB[1]);
		b = float.Parse(blockRGB[2]);
		rIncX = float.Parse(rgbIncX[0]);
		gIncX = float.Parse(rgbIncX[1]);
		bIncX = float.Parse(rgbIncX[2]);
		rIncZ = float.Parse(rgbIncZ[0]);
		gIncZ = float.Parse(rgbIncZ[1]);
		bIncZ = float.Parse(rgbIncZ[2]);
		if (GetComponent<CharacterMovement>() == null) {
			GetComponent<EditorInterface>().setVariables(r, g, b, rIncX, gIncX, bIncX, rIncZ, gIncZ, bIncZ, title);
		}
	}

	void setBlocks(string[] lines) {
		for (int i = 6; i < lines.Length; i++) {
			for (int j = 0; j < lines[i].Length; j++) {
				if (lines[i][j] == VariableManagement.standardBlockTile) {
					numberOfBlocks++;
					createBlock(standardBlock, j, i).tag = VariableManagement.block;
				} else if (lines[i][j] == VariableManagement.multistepBlockTile) {
					numberOfBlocks++;
					createBlock(multistepBlock, j, i).tag = VariableManagement.block;
				} else if (lines[i][j] == VariableManagement.switchBlockTile) {
					createBlock(switchBlock, j, i).tag = VariableManagement.switchTag;
				} else if (lines[i][j] == VariableManagement.activeBlockTile) {
					numberOfBlocks++;
					createBlock(redBlock, j, i).tag = VariableManagement.active;
				} else if (lines[i][j] == VariableManagement.inactiveBlockTile) {
					numberOfBlocks++;
					createBlock(blueBlock, j, i).tag = VariableManagement.inactive;
				} else if (lines[i][j] == VariableManagement.rotateRBlockTile) {
					createBlock(rotatorR, j, i).tag = VariableManagement.rotateR;
				} else if (lines[i][j] == VariableManagement.rotateLBlockTile) {
					createBlock(rotatorL, j, i).tag = VariableManagement.rotateL;
				} else if (lines[i][j] == VariableManagement.bombBlockTile) {
					numberOfBlocks++;
					createBlock(bombBlock, j, i).tag = VariableManagement.bomb;
				} 
			}
		}
	}

	GameObject createBlock(GameObject block, int x, int z) {
		GameObject temp = Instantiate(block);
		temp.layer = 8;
		if (GetComponent<CharacterMovement>() != null) {
			temp.transform.position = new Vector3(x - 3.5f, 0, -z + 12.5f);
		} else {
			temp.transform.position = new Vector3(x, 0, -z + 19);
			GetComponent<LevelEditor>().addToBlockList(x, -z + 19, temp);
		}
		addBlock(temp);
		changeBlockColor(temp);
		if ((temp.transform.position.x + temp.transform.position.z) > rightMost) {
			rightMost = (temp.transform.position.x + temp.transform.position.z);
		} 
		if ((temp.transform.position.x + temp.transform.position.z) < leftMost) {
			leftMost = (temp.transform.position.x + temp.transform.position.z);
		}
		if ((-x + 4 + -z + 12) > topMost) {
			topMost = (-x + 4 + -z + 12);
		} 
		if ((-x + 4 + -z + 12) < bottomMost) {
			bottomMost = (-x + 4 + -z + 12);
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
		temp.transform.SetParent(cubes.transform);
		return temp;
	}

	public void recalculateBlocks () {
		for (int i = 0; i < blocks.Count; i++) {
			if (blocks[i].transform.localPosition.x < xMin) {
				xMin = blocks[i].transform.localPosition.x;
			} 
			if (blocks[i].transform.localPosition.x > xMax) {
				xMax = blocks[i].transform.localPosition.x;
			}
			if (blocks[i].transform.localPosition.z < zMin) {
				zMin = blocks[i].transform.localPosition.z;
			} 
			if (blocks[i].transform.localPosition.z > zMax) {
				zMax = blocks[i].transform.localPosition.z;
			}
		}
		float yHeight1 = Mathf.Abs(xMin - xMax);
		float yHeight2 = Mathf.Abs(zMin - zMax);
		Vector3 newCenter;
		if ((yHeight2 / yHeight1) < 1.625f) {
			newCenter = new Vector3((xMin + xMax) / 2, Mathf.Clamp(yHeight1 + 0.5f, endOrthoMin, endOrthoMax * 2), (zMin + zMax) / 2);
		} else {
			newCenter = new Vector3((xMin + xMax) / 2, Mathf.Clamp((yHeight2 / 2) + 1.25f, endOrthoMin, endOrthoMax * 2), (zMin + zMax) / 2);
		}
		if (Vector3.Distance(newCenter, center) > 0.2f) {
			center = newCenter;
			if (GetComponent<CharacterMovement>() != null) {
				GetComponent<CharacterMovement>().setPan();
			}
		}
	}

	void setCamera() {
		float yHeight1 = Mathf.Abs(xMin - xMax) + ((Screen.height/Screen.width) * 0.5f);
		float yHeight2 = Mathf.Abs(zMin - zMax) + ((Screen.height/Screen.width) * 0.5f);
		Camera.main.orthographicSize = ((rightMost - leftMost) - (rightMost - leftMost) / 4.5f);
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, startOrthoMin, startOrthoMax);
		if ((yHeight2 / yHeight1) < 1.625f) {
			center = new Vector3((xMin + xMax) / 2, Mathf.Clamp(yHeight1 + 0.5f, endOrthoMin, endOrthoMax), (zMin + zMax) / 2);
		} else {
			center = new Vector3((xMin + xMax) / 2, Mathf.Clamp((yHeight2 / 2) + 1.25f, endOrthoMin, endOrthoMax), (zMin + zMax) / 2);
		}
		if (rightMost != Mathf.Abs(leftMost)) {
			Camera.main.transform.position = new Vector3(
				Camera.main.transform.position.x + ((rightMost + leftMost) / 4), 
				Camera.main.transform.position.y, 
				Camera.main.transform.position.z + ((rightMost + leftMost) / 4)
			);
		}
		Camera.main.transform.position = new Vector3(
			Camera.main.transform.position.x, 
			Camera.main.transform.position.y + ((topMost + bottomMost) * 0.6f), 
			Camera.main.transform.position.z
		);
	}

	public void changeBlockColor(GameObject block) {
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
		if (block.name == (VariableManagement.multistepBlock + VariableManagement.clone) && GetComponent<CharacterMovement>() == null) {
			tempR = ((tempR + Camera.main.backgroundColor.r) / 2);
			tempG = ((tempG + Camera.main.backgroundColor.g) / 2);
			tempB = ((tempB + Camera.main.backgroundColor.b) / 2);
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB, blockAlpha);
		} else {
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB);
		}
	}

	public List<GameObject> getBlocks() {
		return blocks;
	}

	void addBlock(GameObject b) {
		blocks.Add(b);
		if (b.name == VariableManagement.activeBlock + VariableManagement.clone) {
			addSpecialBlock(redBlocks, b);
		} else if (b.name == VariableManagement.inactiveBlock + VariableManagement.clone) {
			addSpecialBlock(blueBlocks, b);
		} else if (b.name == VariableManagement.bombBlock + VariableManagement.clone) {
			addSpecialBlock(bombBlocks, b);
		}
	}

	void addSpecialBlock(List<GameObject> list, GameObject b) {
		list.Add(b);
	}

	public void removeBlock(GameObject b) {
		blocks.Remove(b);
		if (b.tag == VariableManagement.block || b.tag == VariableManagement.active || b.tag == VariableManagement.inactive || b.tag == VariableManagement.bomb) {
			numberOfBlocks--;
		}
	}

	public void removeRedBlock(GameObject b) {
		redBlocks.Remove(b);
	}

	public void removeBlueBlock(GameObject b) {
		blueBlocks.Remove(b);
	}

	public void removeBombBlock(GameObject b) {
		bombBlocks.Remove(b);
	}

	public List<GameObject> getRedBlocks() {
		return redBlocks;
	}

	public List<GameObject> getBlueBlocks() {
		return blueBlocks;
	}

	public List<GameObject> getBombBlocks() {
		return bombBlocks;
	}

	public int getNumberOfBlocks() {
		return numberOfBlocks;
	}

	public Vector3 getCenter() {
		return center;
	}
}
