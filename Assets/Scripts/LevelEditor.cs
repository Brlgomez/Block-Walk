using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelEditor : MonoBehaviour {

	GameObject cubes;
	bool isMouseDrag = false;
    GameObject standardBlock;
	GameObject currentObject;
	List<List<GameObject>> blockPos;
	float rayCastCount = 0.05f;
	float rayCastLimit = 0.05f;

	void Awake () {
		cubes = GameObject.Find ("Cubes");
        standardBlock = GameObject.Find ("Standard Block");
		currentObject = standardBlock;
		if (blockPos == null) {
			fillEmptyBlockList();
		}
	}
		
	public void mouseDown () {
		isMouseDrag = true;
	}

	public void mouseUp () {
		isMouseDrag = false;
		rayCastCount = rayCastLimit;
	}

	public void mouseDrag () {
		clickedObject();
	}

	public bool getMouseDrag () {
		return isMouseDrag;
	}

	void clickedObject () {
		rayCastCount += Time.deltaTime;
		if (rayCastCount > rayCastLimit) {
			rayCastCount = 0;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray.origin, ray.direction, out hit)) {
				if (hit.transform.name != (currentObject.name + "(Clone)")) {
					Vector3 point = ray.GetPoint(hit.distance);
					int x = Mathf.RoundToInt(point.x);
					int z = Mathf.RoundToInt(point.z);
					spawnObject(x, z);
				}
			}
		}
	}

	void spawnObject (int x, int z) {
		if (blockPos[z][x] == null && currentObject.name != "Player") {
			createBlock(x, z);
		} else {
			if (currentObject.name == "Player") {
				Destroy(blockPos[z][x]);
			} else {
				Destroy(blockPos[z][x]);
				createBlock(x, z);
			}
		}
	}

	void createBlock (int x, int z) {
		GameObject temp = Instantiate(currentObject);
		temp.transform.position = new Vector3 (x, 0, z);
		GetComponent<EditorInterface>().changeBlockColor(temp);
		temp.transform.SetParent(cubes.transform);
		blockPos[z][x] = temp;
	}

	public void changeBlock (GameObject block){
		currentObject = block;
	}

	public List<List<GameObject>> getBlocks () {
		return blockPos;
	}

	public void addToBlockList (int x, int z, GameObject obj) {
		if (blockPos == null) {
			fillEmptyBlockList();
		}
		blockPos[z][x] = obj;
	}

	void fillEmptyBlockList () {
		blockPos = new List<List<GameObject>>();
		for (int i = 0; i < 14; i++) {
			blockPos.Add(new List<GameObject>());
			for (int j = 0; j < 8; j++) { 
				blockPos[i].Add(null);
			}
		}
	}
}
