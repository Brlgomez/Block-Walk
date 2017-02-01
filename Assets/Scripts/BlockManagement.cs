using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManagement : MonoBehaviour {

	List<GameObject> blocks = new List<GameObject> ();
	[Range (0.0f, 2.0f)]
	public float r, g, b;
	[Range (-1.0f, 1.0f)]
	public float rInc, gInc, bInc;
	public bool rXorZ, gXorZ, bXorZ;
	int numberOfBlocks;
	Vector3 center;
	float xMin, xMax, zMin, zMax = 0;

	void Awake () {
		//randomColors ();
		blocks = new List<GameObject> ();
		GameObject[] tempBlocks = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];
		foreach (GameObject block in tempBlocks) {
			if (block.layer == 8) {
				addBlock (block);
				changeBlockColor (block);
				if (block.tag == "Block" || block.tag == "RedBlock" || block.tag == "BlueBlock") {
					numberOfBlocks++;
				}
				if (block.transform.position.x < xMin) {
					xMin = block.transform.position.x;
				} else if (block.transform.position.x > xMax) {
					xMax = block.transform.position.x;
				}
				if (block.transform.position.z < zMin) {
					zMin = block.transform.position.z;
				} else if (block.transform.position.z > zMax) {
					zMax = block.transform.position.z;
				}
			}
		}
		float yHeight1 = Mathf.Abs (xMin) + Mathf.Abs (xMax);
		float yHeight2 = Mathf.Abs (zMin) + Mathf.Abs (zMax);
		if ((yHeight2 / yHeight1) < 1.75f) {
			center = new Vector3 ((xMin + xMax) / 2, yHeight1 + 0.5f, (zMin + zMax) / 2);
		} else {
			center = new Vector3 ((xMin + xMax) / 2, (yHeight2 / 2) + 1.5f, (zMin + zMax) / 2);
		}
	}

	void changeBlockColor (GameObject block) {
		float tempR, tempG, tempB;
		if (rXorZ) {
			tempR = r + (rInc * block.transform.position.x);
		} else {
			tempR = r + (rInc * block.transform.position.z);
		}
		if (gXorZ) {
			tempG = g + (gInc * block.transform.position.x);
		} else {
			tempG = g + (gInc * block.transform.position.z);
		}
		if (bXorZ) {
			tempB = b + (bInc * block.transform.position.x);
		} else {
			tempB = b + (bInc * block.transform.position.z);
		}
		block.GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
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

	/*
	void Update () {
		foreach (GameObject block in blocks) {
			float tempR, tempG, tempB;
			if (rXorZ) {
				tempR = r + (rInc * block.transform.position.x);
			} else {
				tempR = r + (rInc * block.transform.position.z);
			}
			if (gXorZ) {
				tempG = g + (gInc * block.transform.position.x);
			} else {
				tempG = g + (gInc * block.transform.position.z);
			}
			if (bXorZ) {
				tempB = b + (bInc * block.transform.position.x);
			} else {
				tempB = b + (bInc * block.transform.position.z);
			}
			block.GetComponent<Renderer> ().material.color = new Color (tempR, tempG, tempB);
		}
	}
	*/
}
