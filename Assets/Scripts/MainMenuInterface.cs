using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;

public class MainMenuInterface : MonoBehaviour {

	bool loading = false;
	Transform mainMenu;
	Transform worlds;
	Transform levels;
	Transform userCreated;
	Transform playOrEdit;
	GameObject worldText;
	int levelMultiplier = 1;
	int loadedLevel;
	int interfaceMenu = 0;
	float deltaTime = 0;
	bool transition = false;

	void Start () {
		mainMenu = GameObject.Find ("Menu").transform;
		worlds = GameObject.Find ("Worlds").transform;
		levels = GameObject.Find ("Levels").transform;
		userCreated = GameObject.Find("User Created").transform;
		playOrEdit = GameObject.Find("Play or Edit").transform;
		worldText = GameObject.Find ("World Text");
		toMainMenu();
	}

	void Update () {
		if (transition) { 
			deltaTime += Time.deltaTime * 1.5f;
			if (deltaTime > 1) {
				transition = false;
			}
			if (interfaceMenu == 0) {
				worlds.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.one, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.zero, deltaTime);
				playOrEdit.localScale = Vector3.Slerp(playOrEdit.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 1) {
				worlds.localScale = Vector3.Slerp (worlds.localScale, Vector3.one, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.zero, deltaTime);
				playOrEdit.localScale = Vector3.Slerp(playOrEdit.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 2) {
				worlds.transform.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.one, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.zero, deltaTime);
				playOrEdit.localScale = Vector3.Slerp(playOrEdit.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 3) {
				worlds.transform.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.one, deltaTime);
				playOrEdit.localScale = Vector3.Slerp(playOrEdit.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 4) {
				worlds.transform.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.zero, deltaTime);
				playOrEdit.localScale = Vector3.Slerp(playOrEdit.localScale, Vector3.one, deltaTime);
			}
		}
	}

	public void LoadLevel (int level) {
		loadedLevel = (level + levelMultiplier);
		PlayerPrefs.SetInt ("Level", loadedLevel);
		GetComponent<BackgroundColorTransition> ().transition (loadedLevel, "Level From Main Menu");
	}

	public void playOrEditMenu (int level) {
		loadedLevel = (level + levelMultiplier);
		PlayerPrefs.SetInt ("Level", loadedLevel);
		enableTransition(4);
	}

	public void openEditor () {
		PlayerPrefs.SetInt("Back", 0);
		GetComponent<BackgroundColorTransition> ().transition (loadedLevel, "Editor From Main Menu");
	}

	public void deleteLevel () {
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("Level", 0) - 1) + ".txt";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
		}
	}

	public void loadUserLevel () {
		PlayerPrefs.SetInt("Back", 1);
		GetComponent<BackgroundColorTransition> ().transition (loadedLevel, "Level From Main Menu");
	}

	public void nextScene (int n) {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		if (loading == false) {
			SceneManager.LoadScene (n);
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

	public void toUserCreatedLevels () {
		enableTransition (3);
	}

	void enableTransition (int i) {
		interfaceMenu = i;
		deltaTime = 0;
		transition = true;
	}
}
