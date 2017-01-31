using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour {

	private float timerLimit = 0.25f;
	private float pathThickness = 0.03f;
	private int playerSpeed = 10;

	GameObject player;
	bool isMouseDrag;
	List<GameObject> path;
	bool moveCharacter = false;
	int playerPosIndex = 0;
	bool playerFirstMoved = false;
	bool touchingSpinner = false;
	float initialSpinnerTouch;
	float currentSpinnerTouch;
	Transform initialCameraPos;
	float timer = 0.01f;
	public Material mat;
	Vector3 center;
	bool pointOnSwitch = false;
	bool cameraFixed = false;
	Vector3 mousePosPrev;
	Vector3 mousePosCurrent;
	Quaternion camRotateTarget;
	float shiftTimer;
	bool checkForSolution = false;
	float timer2 = 0;
	int numberOfBlocks;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		center = GameObject.Find ("Center").transform.position;
		initialCameraPos = GameObject.Find ("Initial Camera Spot").transform;
		initialCameraPos.position = transform.position;
		path = new List<GameObject>();
		camRotateTarget = Quaternion.Euler(90, 0, 0);
		if (PlayerPrefs.GetInt ("Shift Camera", 0) == 1) {
			Camera.main.orthographicSize = center.y;
			transform.position = center;
			transform.rotation = camRotateTarget;
			cameraFixed = true;
			PlayerPrefs.SetInt ("Shift Camera", 1);
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
			timer += Time.deltaTime;
			if (timer >= timerLimit) {
				mouseDrag ();
			}
		}
		if (moveCharacter) {
			movePlayer ();
		} else {
			if (checkForSolution) {
				timer2 += Time.deltaTime;
				if (timer2 > 0.5f) {
					checkSolution ();
					checkForSolution = false;
					timer2 = 0;
				}
			}
		}
	}

	void moveCamera () {
		if (playerFirstMoved) {
			shiftTimer += Time.deltaTime / 2;
			Camera.main.orthographicSize -= shiftTimer;
			Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, center.y, 20);
			transform.position = Vector3.Slerp(transform.position, center, shiftTimer);
			transform.rotation = Quaternion.Lerp (transform.rotation, camRotateTarget, shiftTimer);
		}
		if (transform.rotation.eulerAngles.x >= 90 && Camera.main.orthographicSize <= center.y) {
			transform.position = center;
			transform.rotation = camRotateTarget;
			cameraFixed = true;
			PlayerPrefs.SetInt ("Shift Camera", 1);
		}
	}

	void mouseDown () {
		isMouseDrag = true;
		mouseDrag ();
	}

	void mouseUp () {
		timer = timerLimit;
		isMouseDrag = false;
		touchingSpinner = false;
		if (path.Count > 0) {
			moveCharacter = true;
			if (!playerFirstMoved) {
				player.transform.localScale = Vector3.one;
				player.transform.position = new Vector3 (path[0].transform.position.x, 0, path[0].transform.position.z);
				playerFirstMoved = true;
			}
		}
		if (pointOnSwitch) {
			GetComponent<SwitchAttributes> ().loadSaveState ();
			pointOnSwitch = false;
		}
	}

	void mouseDrag () {
		if (!GetComponent<GameplayInterface> ().isMenuOn ()) {
			GameObject target = returnClickedObject ();
			if (target != null) {
				if (target.layer == 8 && !touchingSpinner && !moveCharacter) {
					dragBlocks (target);
				} else if (target.tag == "Spinner" && !touchingSpinner && path.Count == 0) {
					touchingSpinner = true;
					initialSpinnerTouch = Input.mousePosition.x;
				}
			}
			if (touchingSpinner) {
				currentSpinnerTouch = Input.mousePosition.x;
				float angle = (initialSpinnerTouch - currentSpinnerTouch) / 250;
				transform.RotateAround (Vector3.zero, Vector3.up, -angle);
				initialCameraPos.RotateAround (Vector3.zero, Vector3.up, -angle);
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
		if (Vector3.Distance (point, path [path.Count - 1].transform.position) > 0.75f) {
			GameObject lastBlock = path [path.Count - 1];
			List <GameObject> nearbyBlocks = new List<GameObject> ();
			List <GameObject> allBlocks = GetComponent<BlockManagement> ().getBlocks ();
			GameObject returnedBlock = null;
			float shortestDist = 10000;
			for (int i = 0; i < allBlocks.Count; i++) {
				Transform blockTransform = allBlocks [i].transform;
				if (checkAdjacent (blockTransform.position, lastBlock.transform.position) && blockTransform.localScale.y > 0.5f) {
					nearbyBlocks.Add (allBlocks [i]);
				}
				if (path.Count > 1) {
					if (path[path.Count - 2] == allBlocks[i] && path[path.Count - 1].tag == "Switch" && path[path.Count - 2].transform.localScale.x < 1) {
						nearbyBlocks.Add (allBlocks [i]);
					}
				}
				if (nearbyBlocks.Count >= 4) {
					break;
				}
			}
			for (int i = 0; i < nearbyBlocks.Count; i++) {
				float dist = Vector3.Distance (nearbyBlocks [i].transform.position, point);
				if (dist < shortestDist) {
					returnedBlock = nearbyBlocks [i];
					shortestDist = dist;
				}
			}
			if (Vector3.Distance (lastBlock.transform.position, point) < Vector3.Distance (returnedBlock.transform.position, point)) {
				returnedBlock = null;
			}
			if (path.Count > 1) {
				if (lastBlock.tag == "Switch" && path[path.Count - 2] == returnedBlock) {
					GetComponent<SwitchAttributes> ().buttonPress ();
				}
			}
			return returnedBlock;
		}
		return null;
	}
		
	void dragBlocks (GameObject target) {
		Vector3 nextPoint = new Vector3 (target.transform.position.x, 1, target.transform.position.z);
		if (path.Count == 0) {
			Vector3 roundedPlayerPos = new Vector3 (
				Mathf.Round (player.transform.position.x), 
				Mathf.Round (player.transform.position.y), 
				Mathf.Round (player.transform.position.z)
			);
			if (playerFirstMoved) {
				addToPath (player.GetComponent<DeleteCubes>().playerCurrentlyOn());
				if (checkAdjacent (nextPoint, roundedPlayerPos)) {
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
		path.Add (block);
		if (block.tag == "Switch" && player.GetComponent<DeleteCubes>().playerCurrentlyOn() != block) {
			GetComponent<SwitchAttributes> ().buttonPress ();
			pointOnSwitch = true;
		}
	}

	void removeFromPath (int pos) {
		path.RemoveAt (pos);
	}

	public List <GameObject> getPath () {
		if (path != null) {
			return path;
		} else {
			path = new List <GameObject>();
			return path;
		}
	}

	void movePlayer () {
		player.transform.position = Vector3.MoveTowards (
			player.transform.position,
			path[playerPosIndex].transform.position, 
			Time.deltaTime * playerSpeed
		);
		if (Vector3.Distance (player.transform.position, path[playerPosIndex].transform.position) < 0.2f) {
			player.transform.position = path [playerPosIndex].transform.position;
			if (playerPosIndex < path.Count - 1) {
				removeFromPath (0);
			} else if (playerPosIndex == path.Count - 1) {
				moveCharacter = false;
				playerPosIndex = 0;
				path.Clear ();
				checkForSolution = true;
			}
		}
	}

	void OnPostRender() {
		if (path.Count > 0) {
			GL.PushMatrix ();
			mat.SetPass (0);
			GL.Begin (GL.QUADS);
			GL.Color (Color.white);
			for (int i = 1; i < path.Count; i++) {
				Vector3 point1 = path [i].transform.position;
				Vector3 point2 = path [i - 1].transform.position;
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
		if ((GetComponent<BlockManagement> ().getNumberOfBlocks() == 1 &&
		    player.GetComponent<DeleteCubes> ().playerCurrentlyOn ().tag != "Switch") ||
			GetComponent<BlockManagement> ().getNumberOfBlocks() < 1) {
			GetComponent<GameplayInterface> ().winText ();
		} else {
			bool lose = true;
			for (int i = 0; i < GetComponent<BlockManagement> ().getBlocks ().Count; i++) {
				if (checkAdjacent (GetComponent<BlockManagement> ().getBlocks () [i].transform.position, player.transform.position)) {
					lose = false;
					break;
				}
			}
			if (lose) {
				GetComponent<GameplayInterface> ().loseText ();
			}
		}
	}
}
