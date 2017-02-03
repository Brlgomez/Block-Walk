using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelEditor : MonoBehaviour {

	bool isMouseDrag = false;
    GameObject cubes;
    GameObject standardBlock;
	GameObject currentObject;
	List<List<GameObject>> blockPos;

	void Awake () {
        cubes = GameObject.Find ("Cubes");
        standardBlock = GameObject.Find ("Standard Block");
		currentObject = standardBlock;
		if (blockPos == null) {
			blockPos = new List<List<GameObject>>();
			for (int i = 0; i < 14; i++) {
				blockPos.Add (new List<GameObject>());
				for (int j = 0; j < 8; j++) { 
					blockPos[i].Add(null);
				}
			}
		}
	}
	
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			mouseDown ();
		}
		if (Input.GetMouseButtonUp (0)) {
			mouseUp ();
		}
		if (isMouseDrag) {
			mouseDrag ();
		}
	}

	void mouseDown () {
		isMouseDrag = true;
	}

	void mouseUp () {
		isMouseDrag = false;
	}

	void mouseDrag () {
		if (GetComponent<EditorInterface>().isMenuOn()) {
			clickedObject();
		}
	}

	void clickedObject () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction, out hit)) {
			if (hit.transform.name != (currentObject.name + "(Clone)")) {
				Vector3 point = ray.GetPoint(hit.distance);
				int x = Mathf.RoundToInt(point.x);
				int z = Mathf.RoundToInt(point.z);
				point = new Vector3(x, 0, z);
				spawnObject (point);
			}
		}
	}

	void spawnObject (Vector3 point) {
		if (blockPos[Mathf.RoundToInt(point.z)][Mathf.RoundToInt(point.x)] == null && currentObject.name != "Player") {
			GameObject temp = Instantiate(currentObject);
			temp.transform.position = point;
			GetComponent<EditorInterface>().changeBlockColor(temp);
			temp.transform.SetParent(cubes.transform);
			blockPos[Mathf.RoundToInt(point.z)][Mathf.RoundToInt(point.x)] = temp;
		} else {
			Destroy(blockPos[Mathf.RoundToInt(point.z)][Mathf.RoundToInt(point.x)]);
			if (currentObject.name != "Player") {
				GameObject temp = Instantiate(currentObject);
				temp.transform.position = point;
				GetComponent<EditorInterface>().changeBlockColor(temp);
				temp.transform.SetParent(cubes.transform);
				blockPos[Mathf.RoundToInt(point.z)][Mathf.RoundToInt(point.x)] = temp;
			}
		}
	}

	public void changeBlock (GameObject block) {
		currentObject = block;
		GetComponent<EditorInterface>().showMain();
	}

	public List<List<GameObject>> getBlocks () {
		return blockPos;
	}

	public void addToBlockList (int x, int z, GameObject obj) {
		if (blockPos == null) {
			blockPos = new List<List<GameObject>>();
			for (int i = 0; i < 14; i++) {
				blockPos.Add(new List<GameObject>());
				for (int j = 0; j < 8; j++) { 
					blockPos[i].Add(null);
				}
			}
		}
		blockPos[z][x] = obj;
	}
}
