using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainMenuInterface : MonoBehaviour {

	private int interfaceSpeed = 10;

	bool loading = false;
	Transform mainMenu;
	Transform worlds;
	Transform levels;
	GameObject worldText;
	int levelMultiplier = 1;
	int loadedLevel;
	int interfaceMenu = 0;
	float deltaTime = 0;
	bool transition = false;

	void Start () {
		mainMenu = GameObject.Find ("World Select").transform;
		worlds = GameObject.Find ("Worlds").transform;
		levels = GameObject.Find ("Levels").transform;
		worldText = GameObject.Find ("World Text");
		toWorldSelect ();
	}

	void Update () {
		if (transition) { 
			deltaTime = Time.deltaTime * interfaceSpeed;
			if (deltaTime > 1) {
				transition = false;
			}
			if (interfaceMenu == 0) {
				worlds.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.one, deltaTime);
			} else if (interfaceMenu == 1) {
				worlds.localScale = Vector3.Slerp (worlds.localScale, Vector3.one, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 2) {
				worlds.transform.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.one, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
			}
		}
	}

	public void LoadLevel (int level) {
		loadedLevel = (level + levelMultiplier);
		PlayerPrefs.SetInt ("Level", loadedLevel);
		GetComponent<BackgroundColorTransition> ().transition (loadedLevel, "Next Level From Main Menu");
	}

	public void nextScene (int n) {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		if (loading == false) {
			SceneManager.LoadScene (1);
		}
		loading = true;
	}

	public void toMainMenu () {
		enableTransition (0);
	}

	public void toWorldSelect () {
		enableTransition (1);
	}

	public void toLevelSelect (int world) {
		enableTransition (2);
		worldText.GetComponent<Text> ().text = "World " + (world + 1);
		levelMultiplier = world * 16;
	}

	void enableTransition (int i) {
		interfaceMenu = i;
		deltaTime = 0;
		transition = true;
	}
}
