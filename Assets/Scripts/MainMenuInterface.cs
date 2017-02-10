using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;

public class MainMenuInterface : MonoBehaviour {

	bool loading = false;
	GameObject mainMenu, worlds, levels, userCreated, confirmation;
	GameObject worldText, userText, blockHolder, standardBlock, multistepBlock, switchBlock, redBlock, blueBlock;
	GameObject rotateRBlock, rotateLBlock, playButton, editButton, deleteButton;
	int levelMultiplier = 1;
	int interfaceMenu = 0;
	bool canTransition = true;
	Vector3 levelIconStart, levelIconEnd;

	void Start () {
		mainMenu = GameObject.Find ("Menu");
		mainMenu.transform.position = new Vector3(-Screen.width/2, Screen.height/2, 0);
		worlds = GameObject.Find ("Worlds");
		worlds.transform.position = new Vector3(-Screen.width/2, Screen.height/2, 0);
		levels = GameObject.Find ("Levels");
		levels.transform.position = new Vector3(-Screen.width/2, Screen.height/2, 0);
		userCreated = GameObject.Find("User Created");
		userCreated.transform.position = new Vector3(-Screen.width/2, Screen.height/2, 0);
		confirmation = GameObject.Find("Confirmation");
		confirmation.transform.position = new Vector3(-Screen.width/2, Screen.height/2, 0);
		worldText = GameObject.Find ("World Text");
		userText = GameObject.Find("Level");
		blockHolder = GameObject.Find("Block Holder");
		standardBlock = GameObject.Find("Standard Block");
		multistepBlock = GameObject.Find("Multistep Block");
		switchBlock = GameObject.Find("Switch Block");
		redBlock = GameObject.Find("Red Block");
		blueBlock = GameObject.Find("Blue Block");
		rotateRBlock = GameObject.Find("Rotate Right Block");
		rotateLBlock = GameObject.Find("Rotate Left Block");
		playButton = GameObject.Find("Play");
		editButton = GameObject.Find("Edit");
		deleteButton = GameObject.Find("Delete");
		if (PlayerPrefs.GetInt("User Level") == 0 || PlayerPrefs.GetInt("User Level") > 100) {
			PlayerPrefs.SetInt("User Level", 1);
		}
		if (PlayerPrefs.GetString("Last Menu") == "Campaign") {
			interfaceMenu = 2;
			toLevelSelect(((PlayerPrefs.GetInt("Level", 0) - 1)/16));
			if (((PlayerPrefs.GetInt("Level", 0) - 1)/16) == 0) {
				Camera.main.backgroundColor = MenuColors.world1Color;
			} else if (((PlayerPrefs.GetInt("Level", 0) - 1)/16) == 1) {
				Camera.main.backgroundColor = MenuColors.world2Color;
			} else if (((PlayerPrefs.GetInt("Level", 0) - 1)/16) == 2) {
				Camera.main.backgroundColor = MenuColors.world3Color;
			}
		} else if (PlayerPrefs.GetString("Last Menu") == "User") {
			interfaceMenu = 3;
			toUserCreatedLevels();
			Camera.main.backgroundColor = MenuColors.editorColor;
		} else {
			interfaceMenu = 0;
			toMainMenu();
			Camera.main.backgroundColor = MenuColors.menuColor;
		}
		PlayerPrefs.SetString("Last Menu", "");
		PlayerPrefs.SetInt("Shift Camera", 0);
	}

	public void menuCanTransition () {
		canTransition = true;
	}

	public void toMainMenu () {
		if (canTransition) {
			canTransition = false;
			destroyBlockChildren();
			mainMenu.AddComponent<MenuTransitions>();
			mainMenu.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.menuColor);
			if (interfaceMenu == 1) {
				worlds.AddComponent<MenuTransitions>();
			} else if (interfaceMenu == 3) {
				userCreated.AddComponent<MenuTransitions>();
			}
			interfaceMenu = 0;
		}
	}

	public void toWorldSelect () {
		if (canTransition) {
			canTransition = false;
			worlds.AddComponent<MenuTransitions>();
			worlds.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.worldColor);
			if (interfaceMenu == 0) {
				mainMenu.AddComponent<MenuTransitions>();
			} else if (interfaceMenu == 2) {
				levels.AddComponent<MenuTransitions>();
			}
			interfaceMenu = 1;
		}
	}

	public void toLevelSelect (int world) {
		if (canTransition) {
			canTransition = false;
			if (interfaceMenu == 1) {
				worlds.AddComponent<MenuTransitions>();
			}
			levels.AddComponent<MenuTransitions>();
			if (world == 0) {
				levels.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.world1Color);
			} else if (world == 1) {
				levels.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.world2Color);
			} else if (world == 2) {
				levels.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.world3Color);
			}
			interfaceMenu = 2;
			worldText.GetComponent<Text>().text = "World " + (world + 1);
			levelMultiplier = world * 16;
			for (int i = 0; i < 16; i++) {
				levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Levels/" + ((world + 1) + "-" + (i + 1)));
			}
		}
	}

	public void toUserCreatedLevels () {
		if (canTransition) {
			disableButtons();
			showLevel();
			if (interfaceMenu == 0) {
				mainMenu.AddComponent<MenuTransitions>();
			} else if (interfaceMenu == 4) {
				confirmation.AddComponent<MenuTransitions>();
			}
			userCreated.AddComponent<MenuTransitions>();
			userCreated.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.editorColor);
			interfaceMenu = 3;
		}
	}

	public void openConfirmation () {
		if (interfaceMenu == 3) {
			confirmation.AddComponent<MenuTransitions>();
			confirmation.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.editorColor);
			userCreated.AddComponent<MenuTransitions>();
			interfaceMenu = 4;
			string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
			if (File.Exists(filePath)) {
			}
		}
	}

	public void deleteLevel () {
		disableButtons();
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
			userText.GetComponent<Text>().text = PlayerPrefs.GetInt("User Level") + "\n\n\n\n\n\n\n\n\nEmpty\n\n\n\n\n\n";
			destroyBlockChildren();
		}
		toUserCreatedLevels();
	}

	public void LoadLevel (int level) {
		PlayerPrefs.SetString("Last Menu", "Campaign");
		PlayerPrefs.SetInt ("Level", level + levelMultiplier);
		gameObject.AddComponent<BackgroundColorTransition> ();
		GetComponent<BackgroundColorTransition> ().transition ("Level From Main Menu");
	}
		
	public void openEditor () {
		PlayerPrefs.SetString("Last Menu", "User");
		gameObject.AddComponent<BackgroundColorTransition> ();
		GetComponent<BackgroundColorTransition> ().transition ("Editor From Main Menu");
	}

	public void cancelDeletion () {
		toUserCreatedLevels();
	}

	public void loadUserLevel () {
		PlayerPrefs.SetString("Last Menu", "User");
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
		if (File.Exists(filePath)) {
			gameObject.AddComponent<BackgroundColorTransition> ();
			GetComponent<BackgroundColorTransition>().transition("Level From Main Menu");
		}
	}

	public void nextScene (int n) {
		if (loading == false) {
			SceneManager.LoadScene (n);
		}
		loading = true;
	}

	public void left () {
		disableButtons();
		if (PlayerPrefs.GetInt("User Level") > 1) {
			destroyBlockChildren();
			PlayerPrefs.SetInt("User Level", PlayerPrefs.GetInt("User Level") - 1);
			showLevel();
		} else {
			destroyBlockChildren();
			PlayerPrefs.SetInt("User Level", 100);
			showLevel();
		}
	}

	public void right () {
		disableButtons();
		if (PlayerPrefs.GetInt("User Level") < 100) {
			destroyBlockChildren();
			PlayerPrefs.SetInt("User Level", PlayerPrefs.GetInt("User Level") + 1);
			showLevel();
		} else {
			destroyBlockChildren();
			PlayerPrefs.SetInt("User Level", 1);
			showLevel();
		}
	}

	void disableButtons () {
		playButton.GetComponent<Image>().color = Color.clear;
		deleteButton.GetComponent<Image>().color = Color.clear;
		playButton.GetComponentInChildren<Text>().color = Color.clear;
		deleteButton.GetComponentInChildren<Text>().color = Color.clear;
		editButton.GetComponentInChildren<Text>().text = "Create";
	}

	void enableButtons () {
		playButton.GetComponent<Image>().color = Color.white;
		deleteButton.GetComponent<Image>().color = Color.white;
		playButton.GetComponentInChildren<Text>().color = Color.black;
		deleteButton.GetComponentInChildren<Text>().color = Color.black;
		editButton.GetComponentInChildren<Text>().text = "Edit";
	}

	public void showLevel() {
		int n = (PlayerPrefs.GetInt("User Level", 0));
		string level = "";
		string[] userLevel;
		string[] lines;
		string filePath = Application.persistentDataPath + "/" + n + ".txt";
		level += n + "\n";
		if (File.Exists(filePath)) {
			enableButtons();
			StreamReader r;
			r = File.OpenText(filePath);
			userLevel = r.ReadToEnd().Split("*"[0]);
			lines = userLevel[0].Split("\n"[0]);
			for (int i = 4; i < lines.Length; i++) {
				for (int j = 0; j < lines[i].Length; j++) {
					if (lines[i][j] == 'C') {
						displayBlockImage(i, j, standardBlock);
					} else if (lines[i][j] == 'M') {
						displayBlockImage(i, j, multistepBlock);
					} else if (lines[i][j] == 'S') {
						displayBlockImage(i, j, switchBlock);
					} else if (lines[i][j] == 'R') {
						displayBlockImage(i, j, redBlock);
					} else if (lines[i][j] == 'B') {
						displayBlockImage(i, j, blueBlock);
					} else if (lines[i][j] == 'E') {
						displayBlockImage(i, j, rotateRBlock);
					} else if (lines[i][j] == 'W') {
						displayBlockImage(i, j, rotateLBlock);
					}
				}
				level += "\n";
			}
		} else {
			level += "\n\n\n\n\n\n\n\nEmpty\n\n\n\n\n\n\n";
		}
		userText.GetComponent<Text>().text = level;
	}

	void displayBlockImage (int i, int j, GameObject b) {
		GameObject block = b;
		GameObject temp = Instantiate(block);
		temp.transform.SetParent(blockHolder.transform);
		temp.transform.position = new Vector3(j - 3.5f, 0, -i + 10.5f);
	}

	void destroyBlockChildren () {
		foreach (Transform child in blockHolder.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
}
