using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainMenuInterface : MonoBehaviour {

	bool loading = false;
	Transform mainMenuButtons;
	Transform worlds;
	Transform levels;
	GameObject worldText;
	int levelMultiplier = 1;
	int loadedLevel;
	int interfaceMenu = 0;
	float deltaTime = 0;
	bool transition = false;

	void Start () {
		mainMenuButtons = GameObject.Find ("World Select").transform;
		worlds = GameObject.Find ("Worlds").transform;
		levels = GameObject.Find ("Levels").transform;
		worldText = GameObject.Find ("World Text");
		worldSelect ();
	}

	void Update () {
		if (transition) { 
			deltaTime += Time.deltaTime;
			if (deltaTime > 1) {
				transition = false;
			}
			if (interfaceMenu == 0) {
				worlds.localScale = Vector3.Lerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Lerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenuButtons.localScale = Vector3.Lerp (mainMenuButtons.localScale, Vector3.one, deltaTime);
			} else if (interfaceMenu == 1) {
				worlds.localScale = Vector3.Lerp (worlds.localScale, Vector3.one, deltaTime);
				levels.localScale = Vector3.Lerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenuButtons.localScale = Vector3.Lerp (mainMenuButtons.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 2) {
				worlds.transform.localScale = Vector3.Lerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Lerp (levels.localScale, Vector3.one, deltaTime);
				mainMenuButtons.localScale = Vector3.Lerp (mainMenuButtons.localScale, Vector3.zero, deltaTime);
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
