using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotator : MonoBehaviour {

	List<GameObject> tempList;

	public void rotateAt (Vector3 point, int direction) {
		tempList = GetComponent<LevelBuilder>().getBlocks();
		if (direction == 1) {
			for (int i = 0; i < tempList.Count; i++) {
				float x = tempList[i].transform.position.x;
				float y = tempList[i].transform.position.y;
				float z = tempList[i].transform.position.z;
				if (x == point.x && z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x + direction, y, z));
				} else if (x == point.x + direction && z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z - direction));
				} else if (x == point.x + direction && z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z - direction));
				} else if (x == point.x + direction && z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x - direction, y, z));
				} else if (x == point.x && z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x - direction, y, z));
				} else if (x == point.x - direction && z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z + direction));
				} else if (x == point.x - direction && z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z + direction));
				} else if (x == point.x - direction && z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x + direction, y, z));
				}
			}
		} else {
			for (int i = 0; i < tempList.Count; i++) {
				float x = tempList[i].transform.position.x;
				float y = tempList[i].transform.position.y;
				float z = tempList[i].transform.position.z;
				if (x == point.x && z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x + direction, y, z));
				} else if (x == point.x - direction && z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x + direction, y, z));
				} else if (x == point.x - direction && z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z - direction));
				} else if (x == point.x - direction && z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z - direction));
				} else if (x == point.x && z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x - direction, y, z));
				} else if (x == point.x + direction && z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x - direction, y, z));
				} else if (x == point.x + direction && z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z + direction));
				} else if (x == point.x + direction && z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(x, y, z + direction));
				}
			}
		}
	} 
}
