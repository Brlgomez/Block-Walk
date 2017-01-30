using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManagement : MonoBehaviour {

	List<GameObject> blocks = new List<GameObject>();
	[Range(0.0f, 2.0f)]
	public float r, g, b;
	[Range(-1.0f, 1.0f)]
	public float rInc, gInc, bInc;
	public bool rXorZ, gXorZ, bXorZ;

	void Awake () {
		//randomColors ();
		blocks = new List<GameObject> ();
		GameObject[] tempBlocks = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject block in tempBlocks) {
			if (block.layer == 8) {
				addBlock (block);
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
	}

	public List<GameObject> getBlocks () {
		return blocks;
	}

	public void addBlock (GameObject b) {
		blocks.Add (b);
	}

	public void removeBlock (GameObject b) {
		blocks.Remove (b);
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
