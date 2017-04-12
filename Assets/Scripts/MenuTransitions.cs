using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuTransitions : MonoBehaviour {

	List<Transform> itemsEntering;
	List<Transform> itemsLeaving;
	Vector3 [] newPosForEntering;
	Vector3 [] newPosForLeaving;
	float direction;
	float timer = 0;
	float timerLimit = 0.75f;
	Color32 backgroundColor;
	bool changeColor;

	void Update () {
		timer += Time.deltaTime * 1.551f;
		if (timer < timerLimit) {
			if (PlayerPrefs.GetInt(VariableManagement.savePower) == 1) {
				timer += Time.deltaTime * 100;
			}
			if (changeColor) {
				Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, backgroundColor, timer);
			}
			for (int i = 0; i < itemsLeaving.Count; i++) {
				itemsLeaving[i].transform.position = Vector3.Lerp(itemsLeaving[i].transform.position, newPosForLeaving[i], timer * ((i + 10) * 0.05f));
			}
			for (int i = 0; i < itemsEntering.Count; i++) {
				itemsEntering[i].transform.position = Vector3.Lerp(itemsEntering[i].transform.position, newPosForEntering[i], timer * ((i + 10) * 0.05f));
			}
		} else {
			if (changeColor) {
				Camera.main.backgroundColor = backgroundColor;
			}
			for (int i = 0; i < itemsLeaving.Count; i++) {
				if (itemsLeaving[i].GetComponent<Button>() != null) {
					itemsLeaving[i].GetComponent<Button>().enabled = false;
					itemsLeaving[i].GetComponent<Button>().interactable = false;
					itemsLeaving[i].transform.parent.transform.parent.GetComponent<GraphicRaycaster>().enabled = false;
					itemsLeaving[i].transform.parent.transform.parent.GetComponent<Canvas>().enabled = false;
				}
				itemsLeaving[i].transform.position = newPosForLeaving[i];
			}
			for (int i = 0; i < itemsEntering.Count; i++) {
				if (itemsEntering[i].GetComponent<Button>() != null) {
					itemsEntering[i].GetComponent<Button>().enabled = true;
					itemsEntering[i].GetComponent<Button>().interactable = true;
				}
				itemsEntering[i].transform.position = newPosForEntering[i];
			}
			Destroy(GetComponent<MenuTransitions>());
		}
	}
		
	public void setScreens (GameObject screenLeaving, GameObject screenEntering, Color32 c) {
		if (c != Camera.main.backgroundColor) {
			changeColor = true;
		} else {
			changeColor = false;
		}
		backgroundColor = c;
		itemsLeaving = new List<Transform>();
		itemsEntering = new List<Transform>();

		if (screenLeaving != null) {
			Transform[] childrenLeaving = screenLeaving.GetComponentsInChildren<Transform>();
			foreach (Transform child in childrenLeaving) {
				if (child.parent == screenLeaving.transform) {
					itemsLeaving.Add(child);
					if (child.GetComponent<Button>() != null) {
						child.GetComponent<Button>().enabled = false;
						child.GetComponent<Button>().interactable = false;
					}
				}
			}
			newPosForLeaving = new Vector3[itemsLeaving.Count];
			for (int i = 0; i < itemsLeaving.Count; i++) {
				if (itemsLeaving[i].transform.position.x > 0) {
					newPosForLeaving[i] = new Vector3((itemsLeaving[i].transform.position.x - Screen.width), itemsLeaving[i].transform.position.y, 0);
				} else {
					newPosForLeaving[i] = itemsLeaving[i].transform.position;
				}
			}
		}
			
		if (screenEntering != null) {
			Transform[] childrenEntering = screenEntering.GetComponentsInChildren<Transform>();
			foreach (Transform child in childrenEntering) {
				if (child.parent == screenEntering.transform) {
					itemsEntering.Add(child);
					if (child.GetComponent<Button>() != null) {
						child.GetComponent<Button>().enabled = false;
						child.GetComponent<Button>().interactable = true;
						child.transform.parent.transform.parent.GetComponent<GraphicRaycaster>().enabled = true;
						child.transform.parent.transform.parent.GetComponent<Canvas>().enabled = true;
					}
				}
			}
			newPosForEntering = new Vector3[itemsEntering.Count];
			for (int i = 0; i < itemsEntering.Count; i++) {
				newPosForEntering[i] = new Vector3((itemsEntering[i].transform.position.x + Screen.width), itemsEntering[i].transform.position.y, 0);
			}
		}
	}
}
