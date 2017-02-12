using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;

public class MainMenuInterface : MonoBehaviour {

	static int maxAmountOfUserLevels = 100;
	static int minAmountOfUserLevels = 1;

	bool loading = false;
	GameObject mainMenu, worlds, levels, userCreated, confirmation;
	GameObject worldText, userText, blockHolder, standardBlock, multistepBlock, switchBlock, redBlock, blueBlock;
	GameObject rotateRBlock, rotateLBlock, playButton, editButton, deleteButton;
	int levelMultiplier = 1;
	int interfaceMenu = 0;
	bool canTransition = true;
	Vector3 levelIconStart, levelIconEnd;

	void Start() {
		worldText = GameObject.Find("World Text");
		userText = GameObject.Find("Level");
		blockHolder = GameObject.Find("Block Holder");
		playButton = GameObject.Find("Play");
		editButton = GameObject.Find("Edit");
		deleteButton = GameObject.Find("Delete");
		mainMenu = GameObject.Find("Menu");
		mainMenu.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		worlds = GameObject.Find("Worlds");
		worlds.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		levels = GameObject.Find("Levels");
		levels.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		userCreated = GameObject.Find("User Created");
		userCreated.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		confirmation = GameObject.Find("Confirmation");
		confirmation.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		standardBlock = GameObject.Find(VariableManagement.standardBlock);
		multistepBlock = GameObject.Find(VariableManagement.multistepBlock);
		switchBlock = GameObject.Find(VariableManagement.switchBlock);
		redBlock = GameObject.Find(VariableManagement.activeBlock);
		blueBlock = GameObject.Find(VariableManagement.inactiveBlock);
		rotateRBlock = GameObject.Find(VariableManagement.rotateRBlock);
		rotateLBlock = GameObject.Find(VariableManagement.rotateRBlock);

		if (GetComponent<VariableManagement>().getUserLevel() == minAmountOfUserLevels ||
		    GetComponent<VariableManagement>().getUserLevel() > maxAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, minAmountOfUserLevels);
		}
		if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.worldMenu) {
			interfaceMenu = 2;
			toLevelSelect((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16);
			if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 0) {
				Camera.main.backgroundColor = MenuColors.world1Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 1) {
				Camera.main.backgroundColor = MenuColors.world2Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 2) {
				Camera.main.backgroundColor = MenuColors.world3Color;
			}
		} else if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.userLevelMenu) {
			interfaceMenu = 3;
			toUserCreatedLevels();
			Camera.main.backgroundColor = MenuColors.editorColor;
		} else {
			interfaceMenu = 0;
			toMainMenu();
			Camera.main.backgroundColor = MenuColors.menuColor;
		}
		PlayerPrefs.SetString(VariableManagement.lastMenu, "");
		GetComponent<VariableManagement>().turnOffCameraShift();
	}

	public void menuCanTransition() {
		canTransition = true;
	}

	public void toMainMenu() {
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

	public void toWorldSelect() {
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

	public void toLevelSelect(int world) {
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
				levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].sprite = 
					Resources.Load<Sprite>("Levels/" + ((world + 1) + "-" + (i + 1)));
			}
		}
	}

	public void toUserCreatedLevels() {
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

	public void openConfirmation() {
		if (interfaceMenu == 3) {
			confirmation.AddComponent<MenuTransitions>();
			confirmation.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.editorColor);
			userCreated.AddComponent<MenuTransitions>();
			interfaceMenu = 4;
			string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
			if (File.Exists(filePath)) {
			}
		}
	}

	public void deleteLevel() {
		disableButtons();
		string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
			userText.GetComponent<Text>().text = GetComponent<VariableManagement>().getUserLevel() + "\n\n\n\n\n\n\n\n\nEmpty\n\n\n\n\n\n";
			destroyBlockChildren();
		}
		toUserCreatedLevels();
	}

	public void LoadLevel(int level) {
		PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.worldMenu);
		PlayerPrefs.SetInt(VariableManagement.worldLevel, level + levelMultiplier);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.levelFromMain);
	}

	public void openEditor() {
		PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.toEditorFromMain);
	}

	public void cancelDeletion() {
		toUserCreatedLevels();
	}

	public void loadUserLevel() {
		PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
		string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		if (File.Exists(filePath)) {
			gameObject.AddComponent<BackgroundColorTransition>();
			GetComponent<BackgroundColorTransition>().transition(VariableManagement.levelFromMain);
		}
	}

	public void nextScene(int n) {
		if (loading == false) {
			SceneManager.LoadScene(n);
		}
		loading = true;
	}

	public void left() {
		disableButtons();
		if (GetComponent<VariableManagement>().getUserLevel() > minAmountOfUserLevels) {
			destroyBlockChildren();
			PlayerPrefs.SetInt(VariableManagement.userLevel, GetComponent<VariableManagement>().getUserLevel() - 1);
			showLevel();
		} else {
			destroyBlockChildren();
			PlayerPrefs.SetInt(VariableManagement.userLevel, maxAmountOfUserLevels);
			showLevel();
		}
	}

	public void right() {
		disableButtons();
		if (GetComponent<VariableManagement>().getUserLevel() < maxAmountOfUserLevels) {
			destroyBlockChildren();
			PlayerPrefs.SetInt(VariableManagement.userLevel, GetComponent<VariableManagement>().getUserLevel() + 1);
			showLevel();
		} else {
			destroyBlockChildren();
			PlayerPrefs.SetInt(VariableManagement.userLevel, minAmountOfUserLevels);
			showLevel();
		}
	}

	void disableButtons() {
		playButton.GetComponent<Image>().color = Color.clear;
		deleteButton.GetComponent<Image>().color = Color.clear;
		playButton.GetComponentInChildren<Text>().color = Color.clear;
		deleteButton.GetComponentInChildren<Text>().color = Color.clear;
		editButton.GetComponentInChildren<Text>().text = "Create";
	}

	void enableButtons() {
		playButton.GetComponent<Image>().color = Color.white;
		deleteButton.GetComponent<Image>().color = Color.white;
		playButton.GetComponentInChildren<Text>().color = Color.black;
		deleteButton.GetComponentInChildren<Text>().color = Color.black;
		editButton.GetComponentInChildren<Text>().text = "Edit";
	}

	public void showLevel() {
		int n = GetComponent<VariableManagement>().getUserLevel();
		string level = "";
		string[] userLevel;
		string[] lines;
		string filePath = Application.persistentDataPath + "/" + n + ".txt";
		level += n + "\n";
		if (File.Exists(filePath)) {
			enableButtons();
			StreamReader r;
			r = File.OpenText(filePath);
			userLevel = r.ReadToEnd().Split(VariableManagement.levelDelimiter.ToString()[0]);
			lines = userLevel[0].Split("\n"[0]);
			for (int i = 4; i < lines.Length; i++) {
				for (int j = 0; j < lines[i].Length; j++) {
					if (lines[i][j] == VariableManagement.standardBlockTile) {
						displayBlockImage(i, j, standardBlock);
					} else if (lines[i][j] == VariableManagement.multistepBlockTile) {
						displayBlockImage(i, j, multistepBlock);
					} else if (lines[i][j] == VariableManagement.switchBlockTile) {
						displayBlockImage(i, j, switchBlock);
					} else if (lines[i][j] == VariableManagement.activeBlockTile) {
						displayBlockImage(i, j, redBlock);
					} else if (lines[i][j] == VariableManagement.inactiveBlockTile) {
						displayBlockImage(i, j, blueBlock);
					} else if (lines[i][j] == VariableManagement.rotateRBlockTile) {
						displayBlockImage(i, j, rotateRBlock);
					} else if (lines[i][j] == VariableManagement.rotateLBlockTile) {
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

	void displayBlockImage(int i, int j, GameObject b) {
		GameObject block = b;
		GameObject temp = Instantiate(block);
		temp.transform.SetParent(blockHolder.transform);
		temp.transform.position = new Vector3(j - 3.5f, 0, -i + 10.5f);
	}

	void destroyBlockChildren() {
		foreach (Transform child in blockHolder.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
}
