using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour {

	static float maxTimeForDrag = 0.3f;
	static float maxTimeForSolution = 0.5f;
	static float pathThickness = 0.03f;
	static int playerSpeed = 10;

	GameObject player;
	bool isMouseDrag;
	List<GameObject> path;
	bool moveCharacter = false;
	bool playerFirstMoved = false;
	float timerForDrag = 0.3f;
	public Material mat;
	Vector3 center;
	bool pointOnSwitch = false;
	bool cameraFixed = false;
	Vector3 mousePosPrev, mousePosCurrent;
	Quaternion camRotateTarget;
	bool checkForSolution = false;
	float shiftTimer;
	float timerForSolution = 0;
	int numberOfBlocks;
	float initialOrthoSize;
	GameObject playerOn;
	bool canPlayerMove = false;

	void Start () {
		player = GameObject.FindGameObjectWithTag (VariableManagement.player);
		center = GetComponent<LevelBuilder> ().getCenter ();
		path = new List<GameObject> ();
		camRotateTarget = Quaternion.Euler (90, 0, 0);
		initialOrthoSize = Camera.main.orthographicSize;
		if (GetComponent<VariableManagement>().getCameraShift() == 1) {
			setCameraFinalPosition ();
		}
	}

	void Update () {
		if (!cameraFixed) {
			moveCamera ();
		}
		if (Input.GetMouseButtonDown (0)) {
			mouseDown ();
		}
		if (Input.GetMouseButtonUp (0)) {
			mouseUp ();
		}
		if (isMouseDrag && cameraFixed) {
			timerForDrag += Time.deltaTime;
			if (timerForDrag >= maxTimeForDrag) {
				mouseDrag ();
			}
		}
		if (moveCharacter) {
			movePlayer();
		}
		if (checkForSolution && canPlayerMove) {
			timerForSolution += Time.deltaTime;
			if (timerForSolution > maxTimeForSolution) {
				checkSolution ();
			}
		}
	}

	void moveCamera () {
		center = GetComponent<LevelBuilder> ().getCenter ();
		if (playerFirstMoved) {
			shiftTimer += Time.deltaTime / 2;
			if (center.y > initialOrthoSize) {
				Camera.main.orthographicSize += shiftTimer;
				Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, initialOrthoSize, center.y);
			} else {
				Camera.main.orthographicSize -= shiftTimer;
				Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, center.y, initialOrthoSize);
			}
			transform.position = Vector3.Slerp (transform.position, center, shiftTimer);
			transform.rotation = Quaternion.Lerp (transform.rotation, camRotateTarget, shiftTimer);
		}
		if (Camera.main.orthographicSize > center.y - 0.05f && Camera.main.orthographicSize < center.y + 0.05f || PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 1) {
			if (Vector3.Distance(transform.position, center) < 0.05f && transform.rotation.eulerAngles.x >= 90 || PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 1) {
				setCameraFinalPosition();
			}
		}
	}

	void setCameraFinalPosition () {
		center = GetComponent<LevelBuilder> ().getCenter ();
		transform.position = center;
		transform.rotation = camRotateTarget;
		Camera.main.orthographicSize = center.y;
		cameraFixed = true;
		GetComponent<VariableManagement>().turnOnCameraShift();
	}

	public void setPan () {
		initialOrthoSize = Camera.main.orthographicSize;
		cameraFixed = false;
		shiftTimer = 0;
	}

	void mouseDown () {
		isMouseDrag = true;
		mouseDrag ();
	}

	void mouseUp () {
		timerForDrag = maxTimeForDrag;
		isMouseDrag = false;
		timerForSolution = 0;
		if (path.Count > 0) {
			playerOn = path [path.Count - 1];
			moveCharacter = true;
			if (!playerFirstMoved) {
				player.transform.localScale = Vector3.one;
				player.transform.position = new Vector3 (path [0].transform.position.x, 0, path [0].transform.position.z);
				playerFirstMoved = true;
				Behaviour halo = (Behaviour)player.transform.GetChild (0).GetComponent ("Halo");
				halo.enabled = true;
				Camera.main.GetComponent<DeleteCubes> ().enterBlock (path [0]);
			}
		}
		if (pointOnSwitch) {
			GetComponent<SwitchAttributes> ().loadSaveState ();
			pointOnSwitch = false;
		}
	}

	void mouseDrag () {
		if (!GetComponent<GameplayInterface> ().isMenuOn () && !moveCharacter && canPlayerMove) {
			GameObject target = returnClickedObject ();
			if (target != null && target.layer == 8) {
				dragBlocks (target);
			}
		}
	}

	GameObject returnClickedObject () {
		GameObject target = null;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction * center.y, out hit)) {
			if (path.Count == 0) {
				target = hit.collider.gameObject;
				return target;
			} else {
				Vector3 point = ray.GetPoint (hit.distance);
				return findNearestBlock (point);
			}
		}
		return null;
	}

	GameObject findNearestBlock (Vector3 point) {
		if (Mathf.Abs(point.x - path [path.Count - 1].transform.position.x) > 0.6f || Mathf.Abs(point.z - path [path.Count - 1].transform.position.z) > 0.6f) {
			GameObject lastBlock = path [path.Count - 1];
			GameObject secondToLast = null;
			if (path.Count > 1) {
				secondToLast = path [path.Count - 2];
			}
			List <GameObject> nearbyBlocks = new List<GameObject> ();
			List <GameObject> allBlocks = GetComponent<LevelBuilder> ().getBlocks ();
			GameObject returnedBlock = null;
			float shortestDist = 10000;
			// add blocks to list
			for (int i = 0; i < allBlocks.Count; i++) {
				Transform blockTransform = allBlocks [i].transform;
				bool nextToBlock = checkAdjacent (blockTransform.position, lastBlock.transform.position);
				if (nextToBlock && blockTransform.gameObject.GetComponent<BoxCollider>().size.x > 0.5f) {
					nearbyBlocks.Add (allBlocks [i]);
				}
				if (secondToLast != null) {
					bool smallBlockOnPath = (secondToLast == allBlocks [i]);
					if (lastBlock.tag == VariableManagement.switchTag && smallBlockOnPath) {
						nearbyBlocks.Add (allBlocks [i]);
					}
				}
			}
			// pick best block
			for (int i = 0; i < nearbyBlocks.Count; i++) {
				bool test = true;
				if (path.Count > 3) {
					for (int j = 0; j < path.Count - 3; j++) {
						if (nearbyBlocks[i] == path[j]) {
							test = false;
							break;
						}
					}
				} 
				if (test) {
					float dist = Vector3.Distance(nearbyBlocks[i].transform.position, point);
					if (dist < shortestDist) {
						returnedBlock = nearbyBlocks[i];
						shortestDist = dist;
					}
				}
			}
			if (returnedBlock != null) {
				float distToLast = Vector3.Distance(lastBlock.transform.position, point);
				float distToNew = Vector3.Distance(returnedBlock.transform.position, point);
				if (distToLast < distToNew) {
					returnedBlock = null;
				}
				if (secondToLast != null) {
					if (lastBlock.tag == VariableManagement.switchTag && secondToLast == returnedBlock) {
						GetComponent<SwitchAttributes>().buttonPress();
					}
				}
			}
			return returnedBlock;
		}
		return null;
	}

	void dragBlocks (GameObject target) {
		Vector3 nextPoint = new Vector3 (target.transform.position.x, 1, target.transform.position.z);
		if (path.Count == 0) {
			if (playerFirstMoved) {
				addToPath (playerOn);
				if (checkAdjacent (nextPoint, playerOn.transform.position)) {
					addToPath (target);
				}
			} else {
				addToPath (target);
			}
		} else {
			checkNextPosition (target);
		}
	}

	void checkNextPosition (GameObject nextPoint) {
		bool checkOthers = true;
		bool valid = true;
		if (path.Count > 1) {
			if (nextPoint == path [path.Count - 2]) {
				removeFromPath (path.Count - 1);
				checkOthers = false;
			}
		}
		if (checkOthers) {
			for (int i = 0; i < path.Count - 2; i++) {
				if (nextPoint == path [i]) {
					valid = false;
				} 
			}
		}
		if (valid) {
			if (checkAdjacent (nextPoint.transform.position, path [path.Count - 1].transform.position)) {
				addToPath (nextPoint);
			}
		}
	}

	bool checkAdjacent (Vector3 a, Vector3 b) {
		if (a.x == b.x && a.z == b.z + 1) {
			return true;
		} else if (a.x == b.x && a.z == b.z - 1) {
			return true;
		} else if (a.x == b.x + 1 && a.z == b.z) {
			return true;
		} else if (a.x == b.x - 1 && a.z == b.z) {
			return true;
		}
		return false;
	}

	void addToPath (GameObject block) {
		bool blockAdded = false;
		if (path.Count == 0) {
			path.Add(block);
			blockAdded = true;
		} else {
			if ((path[path.Count - 1].tag != VariableManagement.rotateR && 
				path[path.Count - 1].tag != VariableManagement.rotateL) || path.Count == 1) {
				path.Add(block);
				blockAdded = true;
			}
		}
		if (block.tag == VariableManagement.switchTag && playerOn != block && blockAdded) {
			GetComponent<SwitchAttributes>().buttonPress();
			pointOnSwitch = true;
		}
	}

	void removeFromPath (int pos) {
		if (path.Count > 0) {
			path.RemoveAt(pos);
		}
	}

	public List <GameObject> getPath () {
		if (path != null) {
			return path;
		} else {
			path = new List <GameObject> ();
			return path;
		}
	}

	public void deletePath (GameObject block) {
		List<GameObject> tempPath = new List<GameObject>();
		for (int i = 0; i < path.Count; i++) {
			if (path[i] == block) {
				break;
			}
			tempPath.Add(path[i]);
		}
		path.Clear();
		path = tempPath;
		if (tempPath.Count > 1) {
			playerOn = tempPath[tempPath.Count - 1];
		}
	}

	void movePlayer () {
		player.transform.position = Vector3.MoveTowards (
			player.transform.position,
			path [0].transform.position, 
			Time.deltaTime * playerSpeed
		);
		if (Vector3.Distance (player.transform.position, path [0].transform.position) < 0.2f) {
			player.transform.position = path [0].transform.position;
			if (path.Count > 1) {
				Camera.main.GetComponent<DeleteCubes> ().exitBlock (path [0]);
				removeFromPath (0);
				if (path.Count > 0) {
					Camera.main.GetComponent<DeleteCubes>().enterBlock(path[0]);
				}
			} else {
				moveCharacter = false;
				checkForSolution = true;
				timerForSolution = 0;
				path.Clear ();
			}
		}
	}
		
	void OnPostRender () {
		if (path.Count > 0) {
			GL.PushMatrix ();
			mat.SetPass (0);
			GL.Begin (GL.QUADS);
			GL.Color (Color.white);
			for (int i = 1; i < path.Count; i++) {
				Vector3 point1 = path [i].transform.position;
				Vector3 point2 = path [i - 1].transform.position;
				Color lineColor = new Color(
					1 - path[i].GetComponent<Renderer>().material.color.r/4, 
					1 - path[i].GetComponent<Renderer>().material.color.g/4, 
					1 - path[i].GetComponent<Renderer>().material.color.b/4, 
					1
				);
				GL.Color(lineColor);
				GL.Vertex3 (point1.x - pathThickness, point1.y + 1, point1.z + pathThickness);
				GL.Vertex3 (point1.x + pathThickness, point1.y + 1, point1.z + pathThickness);
				GL.Vertex3 (point1.x + pathThickness, point1.y + 1, point1.z - pathThickness);
				GL.Vertex3 (point1.x - pathThickness, point1.y + 1, point1.z - pathThickness);
				if (point2.x == point1.x) {
					GL.Vertex3 (point1.x - pathThickness, point1.y + 1, point1.z);
					GL.Vertex3 (point1.x + pathThickness, point1.y + 1, point1.z);
					GL.Vertex3 (point2.x + pathThickness, point2.y + 1, point2.z);
					GL.Vertex3 (point2.x - pathThickness, point2.y + 1, point2.z);
				} else {
					GL.Vertex3 (point1.x, point1.y + 1, point1.z - pathThickness);
					GL.Vertex3 (point1.x, point1.y + 1, point1.z + pathThickness);
					GL.Vertex3 (point2.x, point2.y + 1, point2.z + pathThickness);
					GL.Vertex3 (point2.x, point2.y + 1, point2.z - pathThickness);
				}
			}
			GL.End ();
			GL.PopMatrix ();
		}
	}

	public void checkSolution () {
		int numberOfBlocks = GetComponent<LevelBuilder> ().getNumberOfBlocks ();
		bool playerOnUnbreakableBlock = (playerOn.tag != VariableManagement.switchTag && 
			playerOn.tag != VariableManagement.rotateR && 
			playerOn.tag != VariableManagement.rotateL);
		checkForSolution = false;
		timerForSolution = 0;
		if ((numberOfBlocks == 1 && playerOnUnbreakableBlock) || numberOfBlocks < 1) {
			GetComponent<GameplayInterface> ().winText ();
			Destroy (GetComponent<CharacterMovement> ());
		} else {
			bool lose = true;
			List<GameObject> tempBlocks = GetComponent<LevelBuilder> ().getBlocks ();
			for (int i = 0; i < tempBlocks.Count; i++) {
				if (tempBlocks[i].transform.localScale.x > 0.5f) {
					if (checkAdjacent(tempBlocks[i].transform.position, playerOn.transform.position)) { 
						lose = false;
						break;
					}
				}
			}
			if (lose) {
				lost("Stuck!");
			}
		}
	}

	public void lost (string text) {
		GetComponent<GameplayInterface> ().loseText (text);
		Destroy (GetComponent<CharacterMovement> ());
	}

	public void setIfPlayerCanMove (bool b) {
		canPlayerMove = b;
	}
}
