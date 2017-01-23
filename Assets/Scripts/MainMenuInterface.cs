using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainMenuInterface : MonoBehaviour {

	bool loading = false;
	GameObject worldSelectButton;
	GameObject scrollBar;
	GameObject worldOne;
	int levelMultiplier = 1;

	void Start () {
		worldSelectButton = GameObject.Find ("World Select");
		scrollBar = GameObject.Find ("Scroll View");
		worldOne = GameObject.Find ("World Levels");
		worldSelect ();
	}

	public void LoadLevel (int level) {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		if (loading == false) {
			SceneManager.LoadScene(level + levelMultiplier);
		}
		loading = true;
	}

	public void mainMenu () {
		scrollBar.transform.localScale = Vector3.zero;
		worldSelectButton.transform.localScale = Vector3.one;
		worldOne.transform.localScale = Vector3.zero;
	}

	public void worldSelect () {
		scrollBar.transform.localScale = Vector3.one;
		worldSelectButton.transform.localScale = Vector3.zero;
		worldOne.transform.localScale = Vector3.zero;
	}

	public void levelSelect (int world) {
		scrollBar.transform.localScale = Vector3.zero;
		worldSelectButton.transform.localScale = Vector3.zero;
		worldOne.transform.localScale = Vector3.one;
		levelMultiplier = world * 16;
	}
}
