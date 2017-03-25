using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombBlock : MonoBehaviour {

	int numberOfSteps = 3;
	bool active = false;
	List<GameObject> currentBlocks = new List<GameObject>();

	public void decreaseBombSteps () {
		if (active) {
			numberOfSteps--;
			if (numberOfSteps <= 0) {
				currentBlocks = Camera.main.GetComponent<LevelBuilder>().getBlocks();
				int direction = 1;
				for (int i = 0; i < currentBlocks.Count; i++) {
					if (gameObject.transform.position.x == currentBlocks[i].transform.position.x && gameObject.transform.position.z == currentBlocks[i].transform.position.z + direction) {
						destroy(currentBlocks[i]);
					} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x + direction && gameObject.transform.position.z == currentBlocks[i].transform.position.z + direction) {
						destroy(currentBlocks[i]);
					} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x + direction && gameObject.transform.position.z == currentBlocks[i].transform.position.z) {
						destroy(currentBlocks[i]);
					} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x + direction && gameObject.transform.position.z == currentBlocks[i].transform.position.z - direction) {
						destroy(currentBlocks[i]);
					} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x && gameObject.transform.position.z == currentBlocks[i].transform.position.z - direction) {
						destroy(currentBlocks[i]);
					} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x - direction && gameObject.transform.position.z == currentBlocks[i].transform.position.z - direction) {
						destroy(currentBlocks[i]);
					} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x - direction && gameObject.transform.position.z == currentBlocks[i].transform.position.z) {
						destroy(currentBlocks[i]);
					} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x - direction && gameObject.transform.position.z == currentBlocks[i].transform.position.z + direction) {
						destroy(currentBlocks[i]);
					}
				}
				destroy(gameObject);
			}
		}
	}

	public void activateBomb () {
		active = true;
	}

	public void destroy (GameObject block) {
		if (block.GetComponent<FallingBlock>() == null) {
			GameObject player = GameObject.FindGameObjectWithTag (VariableManagement.player);
			List<GameObject> path = new List<GameObject>();
			path = Camera.main.GetComponent<CharacterMovement>().getPath();
			for (int i = 0; i < path.Count; i++) {
				if (path[i] == block) {
					Camera.main.GetComponent<CharacterMovement>().deletePath(block);
					break;
				}
			}
			if (player.transform.position == block.transform.position) {
				player.transform.localScale = Vector3.zero;
				Camera.main.GetComponent<CharacterMovement>().lost("Destroyed");
			}
			block.AddComponent<FallingBlock>();
		}
	}
}
