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
	GameObject mainMenu, worlds, levels, userCreated, confirmation, popUp, intro, particles, worldLevels, database, publicConfirmation;
	GameObject worldText, userText, blockHolder, standardBlock, multistepBlock, switchBlock, redBlock, blueBlock;
	GameObject rotateRBlock, rotateLBlock, playButton, editButton, deleteButton;
	int levelMultiplier = 1;
	int interfaceMenu = 0;
	Vector3 levelIconStart, levelIconEnd;
	int publicLevelCount = 0;
	string userNameOfMap = "";
	string nameOfUserMap = "";
	string dataOfUserMap = "";
	int mapDownloadCount;
	string idOfMap = "";

	void Start() {
		worldText = GameObject.Find("World Text");
		userText = GameObject.Find("Level");
		blockHolder = GameObject.Find("Block Holder");
		playButton = GameObject.Find("Play");
		editButton = GameObject.Find("Edit");
		deleteButton = GameObject.Find("Delete");
		particles = GameObject.Find("Sprite Holder");
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
		popUp = GameObject.Find("Pop Up");
		popUp.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		intro = GameObject.Find("Intro Title");
		intro.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		worldLevels = GameObject.Find("World Levels");
		worldLevels.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		database = GameObject.Find("Database");
		database.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		publicConfirmation = GameObject.Find("Public Confirmation");
		publicConfirmation.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		standardBlock = GameObject.Find(VariableManagement.standardBlock);
		multistepBlock = GameObject.Find(VariableManagement.multistepBlock);
		switchBlock = GameObject.Find(VariableManagement.switchBlock);
		redBlock = GameObject.Find(VariableManagement.activeBlock);
		blueBlock = GameObject.Find(VariableManagement.inactiveBlock);
		rotateRBlock = GameObject.Find(VariableManagement.rotateRBlock);
		rotateLBlock = GameObject.Find(VariableManagement.rotateRBlock);

		if (GetComponent<VariableManagement>().getUserLevel() < minAmountOfUserLevels ||
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
			interfaceMenu = -1;
			toIntro();
			Camera.main.backgroundColor = MenuColors.menuColor;
		}
		PlayerPrefs.SetString(VariableManagement.lastMenu, ""); 
		GetComponent<VariableManagement>().turnOffCameraShift();
	}

	public void unlockWorld2 () {
		PlayerPrefs.SetInt(VariableManagement.world0, 1);
		PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
	}

	public void unlockWorld3 () {
		PlayerPrefs.SetInt(VariableManagement.world1, 1);
		PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 2);
	}

	public void toIntro () {
		particles.GetComponentInChildren<ParticleSystem>().Play();
		if (GetComponent<Intro>() == null) {
			gameObject.AddComponent<Intro>();
		}
		intro.AddComponent<MenuTransitions>();
		intro.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.menuColor);
		if (interfaceMenu == 0) {
			mainMenu.AddComponent<MenuTransitions>();
		}
		interfaceMenu = -1;
	}
		
	public void toMainMenu() {
		destroyBlockChildren();
		particles.GetComponentInChildren<ParticleSystem>().Stop();
		if (GetComponent<Intro>() != null) {
			Destroy(GetComponent<Intro>());
		}
		mainMenu.AddComponent<MenuTransitions>();
		mainMenu.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.menuColor);
		if (interfaceMenu == 1) {
			worlds.AddComponent<MenuTransitions>();
		} else if (interfaceMenu == 3) {
			userCreated.AddComponent<MenuTransitions>();
		} else if (interfaceMenu == -1) {
			particles.AddComponent<MenuTransitions>();
			intro.AddComponent<MenuTransitions>();
		} else if (interfaceMenu == 6) {
			database.AddComponent<MenuTransitions>();
		}
		interfaceMenu = 0;
	}

	public void toWorldSelect() {
		if (PlayerPrefs.GetInt(VariableManagement.newWorldUnlocked, 0) > 0) {
			PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 0);
			if (interfaceMenu == 0) {
				mainMenu.AddComponent<MenuTransitions>();
			} else if (interfaceMenu == 2) {
				levels.AddComponent<MenuTransitions>();
			}
			interfaceMenu = 5;
			popUp.AddComponent<MenuTransitions>();
			popUp.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.worldColor);
			popUp.GetComponentsInChildren<Text>()[0].text = "Congrats! You just unlocked a new world and new editor blocks!";
		} else {
			worlds.AddComponent<MenuTransitions>();
			worlds.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.worldColor);
			if (interfaceMenu == 0) {
				mainMenu.AddComponent<MenuTransitions>();
			} else if (interfaceMenu == 2) {
				levels.AddComponent<MenuTransitions>();
			} 
			interfaceMenu = 1;
		}
		for (int i = 0; i < 3; i++) {
			if (PlayerPrefs.GetInt(("World" + i).ToString(), 0) == 0) {
				worlds.GetComponentsInChildren<Image>()[i + 1].color = new Color(0.75f, 0.75f, 0.75f, 1);
			} else {
				worlds.GetComponentsInChildren<Image>()[i + 1].color = new Color(1, 1, 1, 1);
			}
		}
	}

	public void toDatabase () {
		database.GetComponentInChildren<Text>().text = "Beyond the Voyage";
		destroyBlockChildren();
		if (interfaceMenu == 0) {
			mainMenu.AddComponent<MenuTransitions>();
		} else if (interfaceMenu == 7) {
			worldLevels.AddComponent<MenuTransitions>();
		} else if (interfaceMenu == 8) {
			publicConfirmation.AddComponent<MenuTransitions>();
		}
		database.AddComponent<MenuTransitions>();
		database.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.dataBaseInterface);
		interfaceMenu = 6;
	}

	public void toPublicLevels() {
		showPublicLevel();
		if (interfaceMenu == 6) {
			database.AddComponent<MenuTransitions>();
		} else if (interfaceMenu == 8) {
			publicConfirmation.AddComponent<MenuTransitions>();
		}
		worldLevels.AddComponent<MenuTransitions>();
		worldLevels.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.dataBaseInterface);
		interfaceMenu = 7;
	}

	public void getMostRecentLevels () {
		publicLevelCount = 0;
		worldLevels.GetComponentsInChildren<Image>()[4].color = Color.clear;
		worldLevels.GetComponentsInChildren<Text>()[5].color = Color.clear;
		worldLevels.GetComponentsInChildren<Button>()[4].interactable = false;
		database.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().fireBaseMostRecent(database.GetComponentInChildren<Text>());
	}

	public void getMostDownloadedLevels () {
		publicLevelCount = 0;
		worldLevels.GetComponentsInChildren<Image>()[4].color = Color.clear;
		worldLevels.GetComponentsInChildren<Text>()[5].color = Color.clear;
		worldLevels.GetComponentsInChildren<Button>()[4].interactable = false;
		database.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().fireBaseMostDownloaded(database.GetComponentInChildren<Text>());
	}

	public void getYourLevels () {
		publicLevelCount = 0;
		worldLevels.GetComponentsInChildren<Image>()[4].color = Color.white;
		worldLevels.GetComponentsInChildren<Text>()[5].color = Color.black;
		worldLevels.GetComponentsInChildren<Button>()[4].interactable = true;
		database.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().getYourLevels(database.GetComponentInChildren<Text>());
	}

	public void toDeletionConfirmationPublicLevel () {
		if (interfaceMenu == 7) {
			worldLevels.AddComponent<MenuTransitions>();
		}
		publicConfirmation.AddComponent<MenuTransitions>();
		publicConfirmation.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.dataBaseInterface);
		interfaceMenu = 8;
	}

	public void deletePublicLevel () {
		GetComponent<FirebaseDatabases>().deleteLevel(idOfMap, database.GetComponentInChildren<Text>());
		destroyBlockChildren();
	}

	public void toLevelSelect(int world) {
		bool beatAllLevels = true;
		if (world > 0) {
			if (PlayerPrefs.GetInt("World" + (world - 1).ToString(), 0) == 0) {
				beatAllLevels = false;
			}
		} 
		if (!beatAllLevels) {
			interfaceMenu = 5;
			worlds.AddComponent<MenuTransitions>();
			popUp.AddComponent<MenuTransitions>();
			popUp.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.worldColor);
			popUp.GetComponentsInChildren<Text>()[0].text = "World " + (world + 1) + " locked. Must beat all levels from World " + world + " .";
		}
		else {
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
				int levelNumber = (world * 16) + i;
				if (PlayerPrefs.GetInt(levelNumber.ToString(), 0) == 1) {
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Text>()[0].text = "";
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].color = Color.white;
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].sprite = 
						Resources.Load<Sprite>("Levels/" + ((world + 1) + "-" + (i + 1)));
				} else {
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].color = Color.clear;
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Text>()[0].text = (i + 1).ToString();
				}
			}
		} 
	}

	public void toUserCreatedLevels() {
		userCreated.GetComponentsInChildren<Text>()[1].text = "Post";
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

	public void exitPopUp () {
		interfaceMenu = 1;
		popUp.AddComponent<MenuTransitions>();
		worlds.AddComponent<MenuTransitions>();
		worlds.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.worldColor);
	}

	public void openConfirmation() {
		if (interfaceMenu == 3) {
			confirmation.AddComponent<MenuTransitions>();
			confirmation.GetComponent<MenuTransitions>().setBackgroundColor(MenuColors.editorColor);
			userCreated.AddComponent<MenuTransitions>();
			interfaceMenu = 4;
		}
	}

	public void deleteLevel() {
		disableButtons();
		string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
			userText.GetComponent<Text>().text = GetComponent<VariableManagement>().getUserLevel() + "\nEmpty";
			destroyBlockChildren();
			GetComponent<VariableManagement>().setLevelAuthorization(0);
			GetComponent<VariableManagement>().setLevelPostValue(0);
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
		string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		if (File.Exists(filePath)) {		
			PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
			PlayerPrefs.SetString(VariableManagement.userMapName, nameOfUserMap);
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
		nameOfUserMap = "";
		dataOfUserMap = "";
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
		nameOfUserMap = "";
		dataOfUserMap = "";
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

	public void postLevel () {
		if (dataOfUserMap != "" && GetComponent<VariableManagement>().isLevelAuthorized() && 
					userNameOfMap == PlayerPrefs.GetString (VariableManagement.userName, "Unknown") && 
					PlayerPrefs.GetString(VariableManagement.userName, "Unknown") != "Unknown" && 
					PlayerPrefs.GetString(VariableManagement.userName, "Unknown") != "" && 
					PlayerPrefs.GetInt (VariableManagement.isOnline, 0) == 0 /*&& 
					!GetComponent<VariableManagement>().isLevelPosted()*/) {
			userCreated.GetComponentsInChildren<Text>()[1].text = "Posting...";
			GetComponent<FirebaseDatabases>().postLevel(dataOfUserMap, nameOfUserMap, userCreated.GetComponentsInChildren<Text>()[1]);
		}
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 1) {
			GetComponent<GooglePlay>().logIn();
			enableButtons();
		}
	}

	void disableButtons() {
		userCreated.GetComponentsInChildren<Image>()[0].color = Color.clear;
		userCreated.GetComponentsInChildren<Text>()[1].color = Color.clear;
		userCreated.GetComponentsInChildren<Text>()[1].text = "";
		userCreated.GetComponentsInChildren<Image>()[1].color = Color.clear;
		playButton.GetComponent<Image>().color = Color.clear;
		deleteButton.GetComponent<Image>().color = Color.clear;
		playButton.GetComponentInChildren<Text>().color = Color.clear;
		deleteButton.GetComponentInChildren<Text>().color = Color.clear;
		editButton.GetComponentInChildren<Text>().text = "Create";
	}

	void enableButtons() {
		if (GetComponent<VariableManagement>().isLevelAuthorized()) {
			userCreated.GetComponentsInChildren<Image>()[1].color = Color.white;
		}
		if (GetComponent<VariableManagement>().isLevelAuthorized() &&
		   			userNameOfMap == PlayerPrefs.GetString(VariableManagement.userName, "Unknown") &&
		    		PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0 /*&&
					!GetComponent<VariableManagement>().isLevelPosted()*/) {
			userCreated.GetComponentsInChildren<Image>()[0].color = Color.white;
			userCreated.GetComponentsInChildren<Text>()[1].color = Color.black;
			userCreated.GetComponentsInChildren<Text>()[1].text = "Post";
		}/*else if (GetComponent<VariableManagement>().isLevelPosted()) {
			userCreated.GetComponentsInChildren<Image>()[0].color = Color.clear;
			userCreated.GetComponentsInChildren<Text>()[1].color = Color.white;
			userCreated.GetComponentsInChildren<Text>()[1].text = "Posted";
		}*/else if (!GetComponent<VariableManagement>().isLevelAuthorized() &&
		           userNameOfMap == PlayerPrefs.GetString(VariableManagement.userName, "Unknown") &&
		           PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			userCreated.GetComponentsInChildren<Image>()[0].color = Color.clear;
			userCreated.GetComponentsInChildren<Text>()[1].color = Color.white;
			userCreated.GetComponentsInChildren<Text>()[1].text = "Play to Authorize";
		} else if (userNameOfMap != PlayerPrefs.GetString(VariableManagement.userName, "Unknown")) {
			userCreated.GetComponentsInChildren<Image>()[0].color = Color.clear;
			userCreated.GetComponentsInChildren<Text>()[1].color = Color.clear;
			userCreated.GetComponentsInChildren<Text>()[1].text = "";
		} else if (PlayerPrefs.GetString(VariableManagement.userName, "Unknown") == "Unknown" ||
		           PlayerPrefs.GetString(VariableManagement.userName, "Unknown") == "" ||
		           PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 1) {
			userCreated.GetComponentsInChildren<Image>()[0].color = Color.white;
			userCreated.GetComponentsInChildren<Text>()[1].color = Color.black;
			userCreated.GetComponentsInChildren<Text>()[1].text = "Sign in to Post";
		} 
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
			StreamReader r;
			r = File.OpenText(filePath);
			userLevel = r.ReadToEnd().Split(VariableManagement.levelDelimiter.ToString()[0]);
			lines = userLevel[0].Split("\n"[0]);
			level += lines[0] + "\n";
			level += "By: " + lines[1];
			nameOfUserMap = lines[0];
			userNameOfMap = lines[1];
			dataOfUserMap = lines[2] + " " + lines[3] + " " + lines[4] + " " + lines[5];
			createSpriteLevel(lines, 6, " ");
			enableButtons();
		} else {
			level += "Empty Slot";
		}
		userText.GetComponent<Text>().text = level;
	}

	public void createSpriteLevel (string[] lines, int num, string delimeter) {
		destroyBlockChildren();
		for (int i = num; i < lines.Length; i++) {
			dataOfUserMap += (delimeter + lines[i]);
			for (int j = 0; j < lines[i].Length; j++) {
				if (lines[i][j] == VariableManagement.standardBlockTile) {
					displayBlockImage(i - num, j, standardBlock);
				} else if (lines[i][j] == VariableManagement.multistepBlockTile) {
					displayBlockImage(i - num, j, multistepBlock);
				} else if (lines[i][j] == VariableManagement.switchBlockTile) {
					displayBlockImage(i - num, j, switchBlock);
				} else if (lines[i][j] == VariableManagement.activeBlockTile) {
					displayBlockImage(i - num, j, redBlock);
				} else if (lines[i][j] == VariableManagement.inactiveBlockTile) {
					displayBlockImage(i - num, j, blueBlock);
				} else if (lines[i][j] == VariableManagement.rotateRBlockTile) {
					displayBlockImage(i - num, j, rotateRBlock);
				} else if (lines[i][j] == VariableManagement.rotateLBlockTile) {
					displayBlockImage(i - num, j, rotateLBlock);
				}
			}
		}
	}

	public void showPublicLevel() {
		if (publicLevelCount > GetComponent<FirebaseDatabases>().getLevelList().Count - 1) {
			publicLevelCount = GetComponent<FirebaseDatabases>().getLevelList().Count - 1;
		}
		worldLevels.GetComponentsInChildren<Image>()[3].color = Color.white;
		worldLevels.GetComponentsInChildren<Text>()[4].color = Color.black;
		worldLevels.GetComponentsInChildren<Button>()[3].interactable = true;
		worldLevels.GetComponentsInChildren<Image>()[0].color = Color.white;
		worldLevels.GetComponentsInChildren<Text>()[1].color = Color.black;
		worldLevels.GetComponentsInChildren<Image>()[2].color = Color.white;
		worldLevels.GetComponentsInChildren<Text>()[3].color = Color.black;
		worldLevels.GetComponentsInChildren<Text>()[4].text = "Download";
		if (publicLevelCount == 0) {
			worldLevels.GetComponentsInChildren<Image>()[0].color = Color.clear;
			worldLevels.GetComponentsInChildren<Text>()[1].color = Color.clear;
		} 
		if (publicLevelCount == GetComponent<FirebaseDatabases>().getLevelList().Count - 1) {
			worldLevels.GetComponentsInChildren<Image>()[2].color = Color.clear;
			worldLevels.GetComponentsInChildren<Text>()[3].color = Color.clear;
		}
		string[] lines;
		if (GetComponent<FirebaseDatabases>().getLevelList() != null) {
			worldLevels.GetComponentInChildren<Text>().text = "";
			worldLevels.GetComponentInChildren<Text>().text += GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][0] + "\n";
			worldLevels.GetComponentInChildren<Text>().text += "By: " + GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][1] + "\n";
			worldLevels.GetComponentInChildren<Text>().text += "Downloads: " + GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][3];
			mapDownloadCount = int.Parse(GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][3]);
			idOfMap = GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][5];
			lines = GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][4].Split(" "[0]);
			dataOfUserMap = GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][0] + "\n" + 
				GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][1] + "\n" + 
				lines[0] + "\n" + lines[1] + "\n" + lines[2] + "\n" + lines[3];
			createSpriteLevel(lines, 4, "\n");
			dataOfUserMap += "*";
		}
	}

	public void nextPublicLevel () {
		worldLevels.GetComponentsInChildren<Text>()[5].text = "Delete";
		if (GetComponent<FirebaseDatabases>().getLevelList() != null) {
			if (publicLevelCount < GetComponent<FirebaseDatabases>().getLevelList().Count - 1) {
				publicLevelCount++;
				showPublicLevel();
			}
		}
	}

	public void previousPublicLevel () {
		worldLevels.GetComponentsInChildren<Text>()[5].text = "Delete";
		if (GetComponent<FirebaseDatabases>().getLevelList() != null) {
			if (publicLevelCount > 0) {
				publicLevelCount--;
				showPublicLevel();
			}
		}
	}

	public void downloadLevel () {
		bool saved = false;
		for (int i = minAmountOfUserLevels; i < maxAmountOfUserLevels; i++) {
			string filePath = Application.persistentDataPath + "/" + i + ".txt";
			if (!File.Exists(filePath)) {
				saved = true;
				GetComponent<FirebaseDatabases>().incrementDownloadCount(idOfMap, mapDownloadCount);
				worldLevels.GetComponentsInChildren<Text>()[4].text = "Saved In Slot " + i;
				File.AppendAllText(filePath, dataOfUserMap);
				break;
			}
		}
		if (!saved) {
			worldLevels.GetComponentsInChildren<Text>()[4].text = "Storage Full!";
		}
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

	public int getInterfaceNumber () {
		return interfaceMenu;
	}
}
