using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {

	List<GameObject> blocks = new List<GameObject> ();
	float r, g, b;
	float rInc, gInc, bInc;
	bool rXorZ, gXorZ, bXorZ;
	int numberOfBlocks;
	Vector3 center;
	float xMin, xMax, zMin, zMax = 0;
	GameObject cubes;
	GameObject standardBlock;

	void Awake () {
		standardBlock = GameObject.Find ("Cube");
		cubes = GameObject.Find ("Cubes");
		TextAsset t = Resources.Load ("Levels") as TextAsset;
		string[] level = t.text.Split ("*"[0]);
		string[] lines = level [PlayerPrefs.GetInt("Level", 0)].Split("\n"[0]);
		string[] color = lines [0].Split (","[0]);
		string[] blockColors = lines [1].Split (","[0]);
		Camera.main.backgroundColor = new Color32 (byte.Parse (color[0]), byte.Parse (color [1]), byte.Parse (color [2]), 255);
		r = float.Parse (blockColors [0]);
		g = float.Parse (blockColors [1]);
		b = float.Parse (blockColors [2]);
		rInc = float.Parse (blockColors [3]);
		gInc = float.Parse (blockColors [4]);
		bInc = float.Parse (blockColors [5]);
		rXorZ = bool.Parse (blockColors [6]);
		gXorZ = bool.Parse (blockColors [7]);
		bXorZ = bool.Parse (blockColors [8]);
		for (int i = 2; i < lines.Length; i++) {
			for (int j = 0; j < lines[i].Length; j++) {
				if (lines [i] [j] == 'S') {
					createBlock (standardBlock, j, i);
				}
			}
		}
		float yHeight1 = Mathf.Abs (xMin) + Mathf.Abs (xMax);
		float yHeight2 = Mathf.Abs (zMin) + Mathf.Abs (zMax);
		Camera.main.orthographicSize = 14;
		if ((yHeight2 / yHeight1) < 1.75f) {
			center = new Vector3 ((xMin + xMax) / 2, yHeight1 + 0.5f, (zMin + zMax) / 2);
		} else {
			center = new Vector3 ((xMin + xMax) / 2, (yHeight2 / 2) + 1.5f, (zMin + zMax) / 2);
		}
	}

	void createBlock (GameObject block, int x, int z) {
		GameObject temp = Instantiate(block);
		temp.layer = 8;
		temp.transform.position = new Vector3 (x - 3.5f, 0, -z + 8.5f);
		addBlock (temp);
		changeBlockColor (temp);
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
	}

	void changeBlockColor (GameObject block) {
		float tempR, tempG, tempB;
		if (rXorZ) {
			tempR = r + (rInc * block.transform.localPosition.x);
		} else {
			tempR = r + (rInc * block.transform.localPosition.z);
		}
		if (gXorZ) {
			tempG = g + (gInc * block.transform.localPosition.x);
		} else {
			tempG = g + (gInc * block.transform.localPosition.z);
		}
		if (bXorZ) {
			tempB = b + (bInc * block.transform.localPosition.x);
		} else {
			tempB = b + (bInc * block.transform.localPosition.z);
		}
		block.GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
	}

	public List<GameObject> getBlocks () {
		return blocks;
	}

	void addBlock (GameObject b) {
		blocks.Add (b);
		numberOfBlocks++;
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
