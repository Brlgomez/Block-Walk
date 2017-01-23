using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {

	GameObject player;
	bool isMouseDrag;
	int numberOfBlocks;
	List<GameObject> points;
	bool moveCharacter = false;
	int playerPosIndex = 0;
	bool playerFirstMoved = false;
	bool touchingSpinner = false;
	float initialSpinnerTouch;
	float currentSpinnerTouch;
	Transform initialCameraPos;
	float timer = 0.05f;
	float timerLimit = 0.05f;
	private float pathThickness = 0.03f;
	public Material mat;
	GameObject cancel;
	Vector3 center;
	private int playerSpeed = 10;
	bool pointOnSwitch = false;
	bool cameraFixed = false;
	Vector3 mousePosPrev;
	Vector3 mousePosCurrent;
	float dpi;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		numberOfBlocks = GameObject.FindGameObjectsWithTag ("Block").Length + 
			GameObject.FindGameObjectsWithTag ("BlueBlock").Length + 
			GameObject.FindGameObjectsWithTag ("RedBlock").Length ;
		center = GameObject.Find ("Center").transform.position;
		points = new List<GameObject>();
		initialCameraPos = GameObject.Find ("Initial Camera Spot").transform;
		initialCameraPos.position = transform.position;
		cancel = GameObject.Find ("Cancel");
		cancel.GetComponent<Button> ().image.color = new Color (0, 0, 0, 0);
		cancel.GetComponentInChildren<Text>().color = new Color (0, 0, 0, 0);
		dpi = Screen.dpi;
		if (PlayerPrefs.GetInt ("Shift Camera", 0) == 1) {
			Camera.main.orthographicSize = center.y;
			transform.position = center;
			Quaternion target = Quaternion.Euler(90, 0, 0);
			transform.rotation = target;
			cameraFixed = true;
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
			if (pointOnSwitch) {
				GetComponent<SwitchAttributes> ().loadSaveState ();
				pointOnSwitch = false;
			}
		}
		if (isMouseDrag && cameraFixed) {
			if (timer == 0) {
				mousePosPrev = Input.mousePosition;
			}
			timer += Time.deltaTime;
			if (timer >= timerLimit) {
				mousePosCurrent = Input.mousePosition;
				if (Vector3.Distance (mousePosPrev, mousePosCurrent) >= dpi / 25) {
					timer = 0;
					mouseDrag ();
				}
			}
		}
		if (moveCharacter) {
			movePlayer ();
		}
	}

	void moveCamera () {
		if (playerFirstMoved && !cameraFixed) {
			Camera.main.orthographicSize -= Time.deltaTime * (Camera.main.orthographicSize - center.y + 0.5f);
			Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, center.y, 20);
			transform.position = Vector3.Slerp(transform.position, center, Time.deltaTime * 5f);
			Quaternion target = Quaternion.Euler(90, 0, 0);
			transform.rotation = Quaternion.Lerp (transform.rotation, target, Time.deltaTime * 5f);
		}
		if (transform.rotation.eulerAngles.x >= 90 && Camera.main.orthographicSize <= center.y) {
			cameraFixed = true;
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
		if (points.Count > 0) {
			moveCharacter = true;
			if (!playerFirstMoved) {
				player.transform.localScale = Vector3.one;
				player.transform.position = new Vector3 (points [0].transform.position.x, 0, points [0].transform.position.z);
				playerFirstMoved = true;
			}
		}
		cancel.GetComponent<Button> ().image.color = new Color (0, 0, 0, 0);
		cancel.GetComponentInChildren<Text> ().color = new Color (0, 0, 0, 0);
	}

	void mouseDrag () {
		GameObject target = returnClickedObject ();
		if (target != null) {
			if (target.layer == 8 && !touchingSpinner && !moveCharacter) {
				dragBlocks (target);
			} else if (target.tag == "Spinner" && !touchingSpinner && points.Count == 0) {
				touchingSpinner = true;
				initialSpinnerTouch = Input.mousePosition.x;
			}
			//if (!touchingSpinner && !moveCharacter && points.Count > 1){
			//	cancel.GetComponent<Button> ().image.color = new Color (1, 1, 1, 1);
			//	cancel.GetComponentInChildren<Text>().color = new Color (1, 1, 1, 1);
			//}
		}
		if (touchingSpinner) {
			currentSpinnerTouch = Input.mousePosition.x;
			float angle = (initialSpinnerTouch - currentSpinnerTouch) / 250;
			transform.RotateAround (Vector3.zero, Vector3.up, -angle);
			initialCameraPos.RotateAround (Vector3.zero, Vector3.up, -angle);
		} 
	}

	GameObject returnClickedObject () {
		GameObject target = null;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction * center.y, out hit)) {
			if (points.Count == 0) {
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
		if (Vector3.Distance (point, points [points.Count - 1].transform.position) > 0.75f) {
			GameObject lastBlock = points [points.Count - 1];
			List <GameObject> nearbyBlocks = new List<GameObject> ();
			List <GameObject> allBlocks = GetComponent<BlockManagement> ().getBlocks ();
			GameObject returnedBlock = null;
			float shortestDist = 10000;
			for (int i = 0; i < allBlocks.Count; i++) {
				Transform blockTransform = allBlocks [i].transform;
				if (checkAdjacent (blockTransform.position, lastBlock.transform.position) && blockTransform.localScale.y > 0.5f) {
					nearbyBlocks.Add (allBlocks [i]);
				}
				if (points.Count > 2) {
					if (points[points.Count - 2] == allBlocks[i] && points[points.Count - 1].tag == "Switch" && points[points.Count - 2].transform.localScale.x < 1) {
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
			if (points.Count > 2) {
				if (lastBlock.tag == "Switch" && points [points.Count - 2] == returnedBlock) {
					GetComponent<SwitchAttributes> ().buttonPress ();
				}
			}
			return returnedBlock;
		}
		return null;
	}
		
	void dragBlocks (GameObject target) {
		Vector3 nextPoint = new Vector3 (target.transform.position.x, 1, target.transform.position.z);
		if (points.Count == 0) {
			Vector3 roundedPlayerPos = new Vector3 (
				Mathf.Round (player.transform.position.x), 
				Mathf.Round (player.transform.position.y), 
				Mathf.Round (player.transform.position.z)
			);
			if (playerFirstMoved) {
				addToPointsList (player.GetComponent<DeleteCubes>().playerCurrentlyOn());
				if (checkAdjacent (nextPoint, roundedPlayerPos)) {
					addToPointsList (target);
				}
			} else {
				addToPointsList(target);
			}
		} else {
			checkNextPosition (target);
		}
	}

	void checkNextPosition (GameObject nextPoint) {
		bool checkOthers = true;
		bool valid = true;
		if (points.Count > 1) {
			if (nextPoint == points [points.Count - 2]) {
				removeFromPointList (points.Count - 1);
				checkOthers = false;
			}
		}
		if (checkOthers) {
			for (int i = 0; i < points.Count - 2; i++) {
				if (nextPoint == points [i]) {
					valid = false;
				} 
			}
		}
		if (valid) {
			if (checkAdjacent (nextPoint.transform.position, points [points.Count - 1].transform.position)) {
				addToPointsList (nextPoint);
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
		
	void cancelPoints () {
		cancel.GetComponent<Button> ().image.color = new Color (0, 0, 0, 0);
		cancel.GetComponentInChildren<Text>().color = new Color (0, 0, 0, 0);
		points.Clear ();
	}

	void movePlayer () {
		player.transform.position = Vector3.MoveTowards (
			player.transform.position,
			points[playerPosIndex].transform.position, 
			Time.deltaTime * playerSpeed
		);
		if (Vector3.Distance (player.transform.position, points [playerPosIndex].transform.position) < 0.1f) { 
			if (playerPosIndex < points.Count - 1) {
				removeFromPointList (0);
			} else if (playerPosIndex == points.Count - 1) {
				moveCharacter = false;
				playerPosIndex = 0;
				points.Clear ();
			}
		}
	}

	void OnPostRender() {
		if (points.Count > 0) {
			GL.PushMatrix ();
			mat.SetPass (0);
			GL.Begin (GL.QUADS);
			GL.Color (Color.white);
			for (int i = 1; i < points.Count; i++) {
				Vector3 point1 = points [i].transform.position;
				Vector3 point2 = points [i - 1].transform.position;
				if (point2.x == point1.x) {
					GL.Vertex3 (point2.x + pathThickness, point2.y + 0.5f, point2.z);
					GL.Vertex3 (point2.x - pathThickness, point2.y + 0.5f, point2.z);
					GL.Vertex3 (point1.x - pathThickness, point1.y + 0.5f, point1.z);
					GL.Vertex3 (point1.x + pathThickness, point1.y + 0.5f, point1.z);
				} else {
					GL.Vertex3 (point2.x, point2.y + 0.5f, point2.z + pathThickness);
					GL.Vertex3 (point2.x, point2.y + 0.5f, point2.z - pathThickness);
					GL.Vertex3 (point1.x, point1.y + 0.5f, point1.z - pathThickness);
					GL.Vertex3 (point1.x, point1.y + 0.5f, point1.z + pathThickness);
				}
			}
			GL.End ();
			GL.PopMatrix ();
		}
	}
		
	void addToPointsList (GameObject block) {
		points.Add (block);
		if (block.tag == "Switch" && player.GetComponent<DeleteCubes>().playerCurrentlyOn() != block) {
			GetComponent<SwitchAttributes> ().buttonPress ();
			pointOnSwitch = true;
		}
	}

	void removeFromPointList (int pos) {
		points.RemoveAt (pos);
	}
		
	public void deductNumberOfBlocks () {
		numberOfBlocks--;
		if ((numberOfBlocks == 1 && player.GetComponent<DeleteCubes>().playerCurrentlyOn().tag != "Switch") || numberOfBlocks < 1) {
			GetComponent<GameplayInterface> ().winText ();
		}
	}

	public List <GameObject> returnPoints () {
		if (points != null) {
			return points;
		} else {
			points = new List <GameObject>();
			return points;
		}
	}
}
