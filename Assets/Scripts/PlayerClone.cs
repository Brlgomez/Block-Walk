using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerClone : MonoBehaviour {

	Vector2 direction;
	List <GameObject> nextPositions;
	GameObject currentBlock;

	void Start () {
		nextPositions = new List<GameObject>();
		direction = new Vector2(1, 1);
		Collider[] col1 = Physics.OverlapSphere(transform.position, 0.05f);
		currentBlock = col1[0].gameObject;
		addNextPosition(currentBlock);
	}
	
	public void setDirections (int x, int z) {
		direction = new Vector2(x, z);
	}

	public Vector2 getDirection () {
		return direction;
	}

	public void addNextPosition (GameObject block) {
		nextPositions.Add (block);
	}

	public List <GameObject> getNextPositions () {
		return nextPositions;
	}

	public int getCount () {
		return nextPositions.Count;
	}

	public GameObject getLastInList () {
		return nextPositions[nextPositions.Count - 1];
	}

	public void setCurrentBlock (GameObject current) {
		currentBlock = current;
	}

	public GameObject getCurrentBlock () {
		return currentBlock;
	}
}
