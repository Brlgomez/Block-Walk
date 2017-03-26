using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombBlock : MonoBehaviour {

	int numberOfSteps = 4;
	bool active = false;
	List<GameObject> currentBlocks = new List<GameObject>();
	List<GameObject> bombBlocks = new List<GameObject>();

	public void decreaseBombSteps () {
		if (active) {
			numberOfSteps--;
			gameObject.transform.localScale *= 1.25f;
			if (numberOfSteps <= 0) {
				bombAdjacent(true);
			}
		}
	}

	public void activateBomb () {
		if (!active) {
			Camera.main.GetComponent<SoundsAndMusic>().playFuseSound();
			active = true;
		}
	}
		
	public void bombAdjacent (bool sound) {
		currentBlocks = Camera.main.GetComponent<LevelBuilder>().getBlocks();
		int direction = 1;
		for (int i = 0; i < currentBlocks.Count; i++) {
			if (gameObject.transform.position.x == currentBlocks[i].transform.position.x && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z + direction) {
				destroy(currentBlocks[i]);
			} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x + direction && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z + direction) {
				destroy(currentBlocks[i]);
			} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x + direction && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z) {
				destroy(currentBlocks[i]);
			} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x + direction && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z - direction) {
				destroy(currentBlocks[i]);
			} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z - direction) {
				destroy(currentBlocks[i]);
			} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x - direction && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z - direction) {
				destroy(currentBlocks[i]);
			} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x - direction && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z) {
				destroy(currentBlocks[i]);
			} else if (gameObject.transform.position.x == currentBlocks[i].transform.position.x - direction && 
				gameObject.transform.position.z == currentBlocks[i].transform.position.z + direction) {
				destroy(currentBlocks[i]);
			}
		}
		if (PlayerPrefs.GetInt(VariableManagement.savePower) == 0) {
			gameObject.GetComponent<ParticleSystem>().Play();
			if (sound) { 
				Camera.main.GetComponent<SoundsAndMusic>().playExplosionSound();
			}
		}
		destroy(gameObject);
		for (int i = 0; i < bombBlocks.Count; i++) {
			bombBlocks[i].GetComponent<BombBlock>().bombAdjacent(false);
		}
	}

	public void destroy (GameObject block) {
		if (block.GetComponent<FallingBlock>() == null) {
			if (block.tag == VariableManagement.bomb && block != gameObject) {
				bombBlocks.Add(block);
			}
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
				Camera.main.GetComponent<CharacterMovement>().blownUp();
			}
			block.AddComponent<FallingBlock>();
		}
	}
}
