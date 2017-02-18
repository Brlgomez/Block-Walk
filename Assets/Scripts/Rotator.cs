using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotator : MonoBehaviour {

	List<GameObject> tempList;
	bool addToFirstBlock = true;

	public void rotateAt (Vector3 point, int direction) {
		addToFirstBlock = true;
		tempList = GetComponent<LevelBuilder>().getBlocks();
		if (direction == 1) {
			for (int i = 0; i < tempList.Count; i++) {
				float x = tempList[i].transform.position.x;
				float y = tempList[i].transform.position.y;
				float z = tempList[i].transform.position.z;
				if (x == point.x && z == point.z + direction) {
					addRotateBlockComponent(tempList[i], x + direction, y, z);
				} else if (x == point.x + direction && z == point.z + direction) {
					addRotateBlockComponent(tempList[i], x, y, z - direction);
				} else if (x == point.x + direction && z == point.z) {
					addRotateBlockComponent(tempList[i], x, y, z - direction);
				} else if (x == point.x + direction && z == point.z - direction) {
					addRotateBlockComponent(tempList[i], x - direction, y, z);
				} else if (x == point.x && z == point.z - direction) {
					addRotateBlockComponent(tempList[i], x - direction, y, z);
				} else if (x == point.x - direction && z == point.z - direction) {
					addRotateBlockComponent(tempList[i], x, y, z + direction);
				} else if (x == point.x - direction && z == point.z) {
					addRotateBlockComponent(tempList[i], x, y, z + direction);
				} else if (x == point.x - direction && z == point.z + direction) {
					addRotateBlockComponent(tempList[i], x + direction, y, z);
				}
			}
		} else {
			for (int i = 0; i < tempList.Count; i++) {
				float x = tempList[i].transform.position.x;
				float y = tempList[i].transform.position.y;
				float z = tempList[i].transform.position.z;
				if (x == point.x && z == point.z - direction) {
					addRotateBlockComponent(tempList[i], x + direction, y, z);
				} else if (x == point.x - direction && z == point.z - direction) {
					addRotateBlockComponent(tempList[i], x + direction, y, z);
				} else if (x == point.x - direction && z == point.z) {
					addRotateBlockComponent(tempList[i], x, y, z - direction);
				} else if (x == point.x - direction && z == point.z + direction) {
					addRotateBlockComponent(tempList[i], x, y, z - direction);
				} else if (x == point.x && z == point.z + direction) {
					addRotateBlockComponent(tempList[i], x - direction, y, z);
				} else if (x == point.x + direction && z == point.z + direction) {
					addRotateBlockComponent(tempList[i], x - direction, y, z);
				} else if (x == point.x + direction && z == point.z) {
					addRotateBlockComponent(tempList[i], x, y, z + direction);
				} else if (x == point.x + direction && z == point.z - direction) {
					addRotateBlockComponent(tempList[i], x, y, z + direction);
				}
			}
		}
	} 

	void addRotateBlockComponent (GameObject b, float x, float y, float z) {
		b.AddComponent<RotateBlock>();
		b.GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z));
		if (addToFirstBlock) {
			b.GetComponent<RotateBlock>().firstBlockCheck();
		}
		addToFirstBlock = false;
	}
}
