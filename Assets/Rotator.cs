using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotator : MonoBehaviour {

	List<GameObject> tempList;

	public void rotateAt (Vector3 point, int direction) {
		tempList = GetComponent<LevelBuilder>().getBlocks();
		if (direction == 1) {
			for (int i = 0; i < tempList.Count; i++) {
				if (tempList[i].transform.position.x == point.x && tempList[i].transform.position.z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x + direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				} else if (tempList[i].transform.position.x == point.x + direction && tempList[i].transform.position.z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z - direction));
				} else if (tempList[i].transform.position.x == point.x + direction && tempList[i].transform.position.z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z - direction));
				} else if (tempList[i].transform.position.x == point.x + direction && tempList[i].transform.position.z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x - direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				} else if (tempList[i].transform.position.x == point.x && tempList[i].transform.position.z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x - direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				} else if (tempList[i].transform.position.x == point.x - direction && tempList[i].transform.position.z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z + direction));
				} else if (tempList[i].transform.position.x == point.x - direction && tempList[i].transform.position.z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z + direction));
				} else if (tempList[i].transform.position.x == point.x - direction && tempList[i].transform.position.z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x + direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				}
			}
		} else {
			for (int i = 0; i < tempList.Count; i++) {
				if (tempList[i].transform.position.x == point.x && tempList[i].transform.position.z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x + direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				} else if (tempList[i].transform.position.x == point.x - direction && tempList[i].transform.position.z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x + direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				} else if (tempList[i].transform.position.x == point.x - direction && tempList[i].transform.position.z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z - direction));
				} else if (tempList[i].transform.position.x == point.x - direction && tempList[i].transform.position.z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z - direction));
				} else if (tempList[i].transform.position.x == point.x && tempList[i].transform.position.z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x - direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				} else if (tempList[i].transform.position.x == point.x + direction && tempList[i].transform.position.z == point.z + direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x - direction, tempList[i].transform.position.y, tempList[i].transform.position.z));
				} else if (tempList[i].transform.position.x == point.x + direction && tempList[i].transform.position.z == point.z) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z + direction));
				} else if (tempList[i].transform.position.x == point.x + direction && tempList[i].transform.position.z == point.z - direction) {
					tempList[i].AddComponent<RotateBlock>();
					tempList[i].GetComponent<RotateBlock>().setPoint(new Vector3(tempList[i].transform.position.x, tempList[i].transform.position.y, tempList[i].transform.position.z + direction));
				}
			}
		}
	} 
}
