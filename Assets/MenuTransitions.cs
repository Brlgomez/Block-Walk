using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuTransitions : MonoBehaviour {

	List<Transform> items;
	Vector3 [] newPos;
	float direction;
	float timer = 0;
	float timerLimit = 1f;
	Color32 backgroundColor;

	void Start () {
		items = new List<Transform>();
		Transform[] children = transform.GetComponentsInChildren<Transform>();
		foreach (Transform child in children) {
			if (child.parent == transform) {
				items.Add(child);
			}
		}
		newPos = new Vector3[items.Count];
		if (items[0].transform.position.x < 0) {
			direction = Screen.width;
		} else {
			direction = -(Screen.width);
		}
		for (int j = 0; j < items.Count; j++) {
			newPos[j] = new Vector3((items[j].transform.position.x + direction), items[j].transform.position.y, 0);
		}
	}

	void Update () {
		timer += Time.deltaTime * 2f;
		if (timer > timerLimit) {
			for (int i = 0; i < items.Count; i++) {
				if (items[i].GetComponent<Button>() != null) {
					if (direction > 0) {
						items[i].GetComponent<Button>().enabled = true;
					} else {
						items[i].GetComponent<Button>().enabled = false;
					}
				}
				items[i].transform.position = newPos[i];
			}
			Camera.main.GetComponent<MainMenuInterface>().menuCanTransition();
			Destroy(GetComponent<MenuTransitions>());
		} else {
			if (direction > 0) {
				Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, backgroundColor, timer);
			}
			for (int i = 0; i < items.Count; i++) {
				items[i].transform.position = Vector3.Slerp(items[i].transform.position, newPos[i], timer * ((items[i].transform.position.y)/(Screen.height/2)));
			}
		}
	}

	public void setBackgroundColor (Color32 c) {
		backgroundColor = c;
	}
}
