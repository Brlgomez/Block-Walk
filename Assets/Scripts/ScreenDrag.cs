using UnityEngine;
using System.Collections;

public class ScreenDrag : MonoBehaviour {

	GameObject target;
	GameObject activeObject;
	GameObject prevTarget;
	GameObject player;
	Vector3 lastPos;
	bool firstBlockPressed = false;

	GameObject playerClone;
	GameObject targetClone;
	GameObject activeObjectClone;
	GameObject prevTargetClone;
	Vector3 lastPosClone;
	bool firstBlockClone = false;

	float xDir;
	float zDir;
	bool isMouseDrag;
	int numberOfBlocks;
	string currentColor = "Red";
	float orthoZoomSpeed = 0.05f;

	bool touchingCylinder = false;
	float initialSpinnerTouch;
	float currentSpinnerTouch;

	void Start () {
		numberOfBlocks = GameObject.FindGameObjectsWithTag ("Block").Length;
		numberOfBlocks += GameObject.FindGameObjectsWithTag ("Red").Length;
		numberOfBlocks += GameObject.FindGameObjectsWithTag ("Blue").Length;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerClone = GameObject.FindGameObjectWithTag ("Player Clone");
		foreach (GameObject block in GameObject.FindGameObjectsWithTag("Blue")) {
			block.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		}
	}

	void Update () {
		if (activeObject != null) {
			Vector3 newPosition = new Vector3 (
				activeObject.transform.position.x, 
				activeObject.transform.position.y + 1, 
				activeObject.transform.position.z
			);
			float speed = Time.deltaTime * Vector3.Distance (player.transform.position, newPosition) * 10;
			player.transform.position = Vector3.MoveTowards (player.transform.position, newPosition, speed);
			if (playerClone != null && activeObjectClone != null) {
				Vector3 newPositionClone = new Vector3 (
					activeObjectClone.transform.position.x, 
					activeObjectClone.transform.position.y + 1, 
					activeObjectClone.transform.position.z
				);
				float cloneSpeed = Time.deltaTime * Vector3.Distance (playerClone.transform.position, newPositionClone) * 10;
				playerClone.transform.position = Vector3.MoveTowards (playerClone.transform.position, newPositionClone, cloneSpeed);
			}
		}
		if (Input.touchCount > 1) {
			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			// ... change the orthographic size based on the change in distance between the touches.
			Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
			// Make sure the orthographic size never drops below zero.
			Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 5, 20);
		} else {
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
	}

	void mouseDown () {
		mouseDrag ();
		isMouseDrag = true;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
			if (hit.transform.tag == "Spinner") {
				touchingCylinder = true;
				initialSpinnerTouch = Input.mousePosition.x;
			}
		}
	}

	void mouseUp () {
		isMouseDrag = false;
		if (touchingCylinder) {
			touchingCylinder = false;
		}
	}

	void mouseDrag () {
		if (touchingCylinder) {
			currentSpinnerTouch = Input.mousePosition.x;
			float angle = (initialSpinnerTouch - currentSpinnerTouch) / 100;
			Camera.main.transform.RotateAround (Vector3.zero, Vector3.up, -angle);
		} else {
			RaycastHit hitInfo;
			target = returnClickedObject (out hitInfo);
			if (target != null) {
				if (checkIfValidSpot (target.transform.position)) {
					xDir = target.transform.position.x - lastPos.x;
					zDir = target.transform.position.z - lastPos.z;
					activeObject = target;
					lastPos = target.transform.position;
					if (target.tag != "Switch") {
						target.tag = "Active";
					} else {
						changeColorState ();
					}
					if (playerClone != null) {
						if (!firstBlockClone) {
							Vector3 oppositeSide = new Vector3 (
								                        -target.transform.position.x, 
								                        target.transform.position.y + 0.5f, 
								                        -target.transform.position.z
							                        );
							if (Physics.CheckSphere (oppositeSide, 0.25f)) {
								GameObject oppCube = Physics.OverlapSphere (oppositeSide, 0.25f) [0].gameObject;
								if (checkIfValidClone (oppCube.transform.position)) {
									activeObjectClone = oppCube;
									lastPosClone = oppCube.transform.position;
									if (oppCube.tag != "Switch") {
										oppCube.tag = "Active";
									} else {
										changeColorState ();
									}
								}
							} 
						} else {
							Vector3 newClonePosition = new Vector3 (
								                            activeObjectClone.transform.position.x - xDir, 
								                            activeObjectClone.transform.position.y, 
								                            activeObjectClone.transform.position.z - zDir
							                            );
							Vector3 checkPosition = new Vector3 (
								                         activeObjectClone.transform.position.x - xDir, 
								                         activeObjectClone.transform.position.y + 0.5f, 
								                         activeObjectClone.transform.position.z - zDir
							                         );
							if (Physics.CheckSphere (checkPosition, 0.25f)) {
								GameObject oppCube = Physics.OverlapSphere (checkPosition, 0.25f) [0].gameObject;
								if (checkIfValidClone (newClonePosition)) {
									activeObjectClone = oppCube;
									lastPosClone = newClonePosition;
									if (oppCube.tag != "Switch") {
										oppCube.tag = "Active";
									} else {
										changeColorState ();
									}
								}
							}
						}
					}
					if (numberOfBlocks == 1 || (numberOfBlocks == 2 && playerClone != null)) {
						if (target.tag != "Switch") {
							Camera.main.GetComponent<GameplayInterface> ().winText ();
						}
					}
				}
			}
		}
	}

	GameObject returnClickedObject (out RaycastHit hit) {
		GameObject target = null;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
			if (hit.collider.tag == "Block" || hit.collider.tag == "Switch" || hit.collider.tag == currentColor) {
				target = hit.collider.gameObject;
				return target;
			} else {
				return null;
			}
		}
		return null;
	}

	bool checkIfValidSpot (Vector3 targetPos) {
		if (!firstBlockPressed) {
			firstBlockPressed = true;
			player.transform.position = targetPos;
			return true;
		} else {
			if (targetPos.x == lastPos.x && targetPos.z + 1 == lastPos.z) {
				destroyPreviousBlock ();
				return true;
			}
			if (targetPos.x == lastPos.x && targetPos.z - 1 == lastPos.z) {
				destroyPreviousBlock ();
				return true;
			}
			if (targetPos.x + 1 == lastPos.x && targetPos.z == lastPos.z) {
				destroyPreviousBlock ();
				return true;
			}
			if (targetPos.x - 1 == lastPos.x && targetPos.z == lastPos.z) {
				destroyPreviousBlock ();
				return true;
			}
			return false;
		}
	}

	bool checkIfValidClone (Vector3 targetPos) {
		if (!firstBlockClone) {
			firstBlockClone = true;
			playerClone.transform.position = targetPos;
			return true;
		} else {
			if (targetPos.x == lastPosClone.x && targetPos.z + 1 == lastPosClone.z) {
				destroyPreviousBlockClone ();
				return true;
			}
			if (targetPos.x == lastPosClone.x && targetPos.z - 1 == lastPosClone.z) {
				destroyPreviousBlockClone ();
				return true;
			}
			if (targetPos.x + 1 == lastPosClone.x && targetPos.z == lastPosClone.z) {
				destroyPreviousBlockClone ();
				return true;
			}
			if (targetPos.x - 1 == lastPosClone.x && targetPos.z == lastPosClone.z) {
				destroyPreviousBlockClone ();
				return true;
			}
			return false;
		}
	}

	void destroyPreviousBlock () {
		if (activeObject != null && activeObject.name != "Switch") {
			prevTarget = activeObject;
			numberOfBlocks--;
			prevTarget.AddComponent<FallingBlock> ();
		}
	}

	void destroyPreviousBlockClone () {
		if (activeObjectClone != null && activeObjectClone.name != "Switch") {
			prevTargetClone = activeObjectClone;
			numberOfBlocks--;
			prevTargetClone.AddComponent<FallingBlock> ();
		}
	}

	void changeColorState () {
		if (currentColor == "Red") {
			currentColor = "Blue";
			foreach (GameObject block in GameObject.FindGameObjectsWithTag("Red")) {
				block.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			}
			foreach (GameObject block in GameObject.FindGameObjectsWithTag(currentColor)) {
				block.transform.localScale = new Vector3 (1, 1, 1);
			}
		} else {
			currentColor = "Red";
			foreach (GameObject block in GameObject.FindGameObjectsWithTag("Blue")) {
				block.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			}
			foreach (GameObject block in GameObject.FindGameObjectsWithTag(currentColor)) {
				block.transform.localScale = new Vector3 (1, 1, 1);
			}
		}
	}
}
