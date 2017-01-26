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
	GameObject worldLevels;
	GameObject worldText;
	int levelMultiplier = 1;
	int loadedLevel;
	int interfaceMenu = 0;
	float deltaTime = 0;
	bool transition = false;

	void Start () {
		worldSelectButton = GameObject.Find ("World Select");
		scrollBar = GameObject.Find ("Content");
		worldLevels = GameObject.Find ("World Levels");
		worldText = GameObject.Find ("World Text");
		worldSelect ();
	}

	void Update () {
		if (transition) { 
			deltaTime += Time.deltaTime;
			if (deltaTime > 2f) {
				transition = false;
			}
			if (interfaceMenu == 0) {
				scrollBar.transform.localScale = Vector3.Lerp (scrollBar.transform.localScale, Vector3.zero, deltaTime);
				worldLevels.transform.localScale = Vector3.Lerp (worldLevels.transform.localScale, Vector3.zero, deltaTime);
				worldSelectButton.transform.localScale = Vector3.Lerp (worldSelectButton.transform.localScale, Vector3.one, deltaTime);
			} else if (interfaceMenu == 1) {
				scrollBar.transform.localScale = Vector3.Lerp (scrollBar.transform.localScale, Vector3.one, deltaTime);
				worldLevels.transform.localScale = Vector3.Lerp (worldLevels.transform.localScale, Vector3.zero, deltaTime);
				worldSelectButton.transform.localScale = Vector3.Lerp (worldSelectButton.transform.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 2) {
				scrollBar.transform.localScale = Vector3.Lerp (scrollBar.transform.localScale, Vector3.zero, deltaTime);
				worldLevels.transform.localScale = Vector3.Lerp (worldLevels.transform.localScale, Vector3.one, deltaTime);
				worldSelectButton.transform.localScale = Vector3.Lerp (worldSelectButton.transform.localScale, Vector3.zero, deltaTime);
			}
		}
	}

	public void LoadLevel (int level) {
		loadedLevel = level + levelMultiplier;
		GetComponent<BackgroundColorTransition> ().transition (loadedLevel, "Next Level From Main Menu");
	}

	public void nextScene (int n) {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		if (loading == false) {
			SceneManager.LoadScene(loadedLevel);
		}
		loading = true;
	}

	public void mainMenu () {
		enableTransition (0);
	}

	public void worldSelect () {
		enableTransition (1);
	}

	public void levelSelect (int world) {
		enableTransition (2);
		worldText.GetComponent<Text>().text = "World " + (world + 1);
		levelMultiplier = world * 16;
	}

	void enableTransition (int i) {
		interfaceMenu = i;
		deltaTime = 0;
		transition = true;
	}
}
