using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;

public class MainMenuInterface : MonoBehaviour {

	static int maxAmountOfUserLevels = 100;
	static int minAmountOfUserLevels = 1;

	bool loading = false;
	GameObject mainMenu, worlds, levels, userCreated, confirmation, popUp, intro, particles, worldLevels, database;
	GameObject publicConfirmation, search, settings, store;
	GameObject blockHolder, standardBlock, multistepBlock, switchBlock, redBlock, blueBlock, rotateRBlock, rotateLBlock;
	int levelMultiplier = 1;
	int interfaceMenu = 0;
	bool databaseOrSearch = true;
	Vector3 levelIconStart, levelIconEnd;
	int publicLevelCount = 0;
	string userNameOfMap, nameOfUserMap, dataOfUserMap, idOfMap, userID = "";
	int mapDownloadCount;
	List<float> filePositions;
	public Material mat;
	int lastDatabaseMenu = 0;

	void Start() {
		blockHolder = GameObject.Find("Block Holder");
		particles = GameObject.Find("Sprite Holder");
		mainMenu = findAndSetUi("Menu");
		worlds = findAndSetUi("Worlds");
		levels = findAndSetUi("Levels");
		userCreated = findAndSetUi("User Created");
		confirmation = findAndSetUi("Confirmation");
		popUp = findAndSetUi("Pop Up");
		intro = findAndSetUi("Intro Title");
		worldLevels = findAndSetUi("World Levels");
		database = findAndSetUi("Database");
		publicConfirmation = findAndSetUi("Public Confirmation");
		search = findAndSetUi("Search");
		settings = findAndSetUi("Settings");
		store = findAndSetUi("Store");
		standardBlock = GameObject.Find(VariableManagement.standardBlock);
		multistepBlock = GameObject.Find(VariableManagement.multistepBlock);
		switchBlock = GameObject.Find(VariableManagement.switchBlock);
		redBlock = GameObject.Find(VariableManagement.activeBlock);
		blueBlock = GameObject.Find(VariableManagement.inactiveBlock);
		rotateRBlock = GameObject.Find(VariableManagement.rotateRBlock);
		rotateLBlock = GameObject.Find(VariableManagement.rotateLBlock);
		if (GetComponent<VariableManagement>().getUserLevel() < minAmountOfUserLevels ||
		    GetComponent<VariableManagement>().getUserLevel() > maxAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, minAmountOfUserLevels);
		}
		goToLastMenu();
		PlayerPrefs.SetString(VariableManagement.lastMenu, ""); 
		GetComponent<VariableManagement>().turnOffCameraShift();
		updateFiles();
	}

	void goToLastMenu () {
		if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.worldMenu) {
			interfaceMenu = 2;
			toLevelSelect((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16);
			if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 0) {
				Camera.main.backgroundColor = MenuColors.world1Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 1) {
				Camera.main.backgroundColor = MenuColors.world2Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 2) {
				Camera.main.backgroundColor = MenuColors.world3Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 3) {
				Camera.main.backgroundColor = MenuColors.world4Color;
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
	}

	/* ------------------------------------------------voyage-------------------------------------------------------- */
		
	public void toIntro () {
		particles.GetComponentInChildren<ParticleSystem>().Play();
		if (GetComponent<Intro>() == null) {
			gameObject.AddComponent<Intro>();
		}
		gameObject.AddComponent<MenuTransitions>().setScreens(null, intro, MenuColors.menuColor);
		interfaceMenu = -1;
	}
		
	public void toMainMenu() {
		destroyBlockChildren();
		if (GetComponent<Intro>() != null) {
			Destroy(GetComponent<Intro>());
		}
		if (interfaceMenu == 1) {
			gameObject.AddComponent<MenuTransitions>().setScreens(worlds, mainMenu, MenuColors.menuColor);
		} else if (interfaceMenu == 3) {
			gameObject.AddComponent<MenuTransitions>().setScreens(userCreated, mainMenu, MenuColors.menuColor);
		} else if (interfaceMenu == -1) {
			gameObject.AddComponent<MenuTransitions>().setScreens(intro, mainMenu, MenuColors.menuColor);
			particles.GetComponentInChildren<ParticleSystem>().Stop();
			particles.GetComponentInChildren<ParticleSystem>().Clear();
		} else if (interfaceMenu == 6) {
			gameObject.AddComponent<MenuTransitions>().setScreens(database, mainMenu, MenuColors.menuColor);
		} else if (interfaceMenu == 10) {
			gameObject.AddComponent<MenuTransitions>().setScreens(settings, mainMenu, MenuColors.menuColor);
		} else if (interfaceMenu == 11) {
			gameObject.AddComponent<MenuTransitions>().setScreens(store, mainMenu, MenuColors.menuColor);
		}
		interfaceMenu = 0;
	}

	public void toSettings () {
		gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, settings, MenuColors.settingsColor);
		interfaceMenu = 10;
	}

	public void toStore () {
		gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, store, MenuColors.storeColor);
		interfaceMenu = 11;
	}
		
	public void toWorldSelect() {
		if (PlayerPrefs.GetInt(VariableManagement.newWorldUnlocked, 0) > 0) {
			PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 0);
			if (interfaceMenu == 0) {
				gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, popUp, MenuColors.worldColor);
			} else if (interfaceMenu == 2) {
				gameObject.AddComponent<MenuTransitions>().setScreens(levels, popUp, MenuColors.worldColor);
			}
			interfaceMenu = 5;
			popUp.GetComponentsInChildren<Text>()[0].text = "Congrats! You just unlocked a new world and new blocks!";
		} else {
			if (interfaceMenu == 0) {
				gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, worlds, MenuColors.worldColor);
			} else if (interfaceMenu == 2) {
				gameObject.AddComponent<MenuTransitions>().setScreens(levels, worlds, MenuColors.worldColor);
			} else if (interfaceMenu == 5) {
				gameObject.AddComponent<MenuTransitions>().setScreens(popUp, worlds, MenuColors.worldColor);
			}
			interfaceMenu = 1;
		}
		for (int i = 1; i < 4; i++) {
			if (PlayerPrefs.GetInt(("World" + (i - 1)).ToString(), 0) == 0) {
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Image>().color = Color.black;
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Text>().color = Color.clear;
			} else {
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Image>().color = Color.white;
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Text>().color = Color.white;
			}
		}
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
			gameObject.AddComponent<MenuTransitions>().setScreens(worlds, popUp, MenuColors.worldColor);
			popUp.GetComponentsInChildren<Text>()[0].text = "Locked!\nMust beat all levels from the previous world.";
		} else {
			if (world == 0) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world1Color);
			} else if (world == 1) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world2Color);
			} else if (world == 2) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world3Color);
			} else if (world == 3) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world4Color);
			}
			interfaceMenu = 2;
			levels.GetComponentInChildren<Text>().text = "World " + (world + 1);
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

	/* -------------------------------------------play, create, share------------------------------------------------ */

	public void toUserCreatedLevels() {
		userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Post";
		showLevel();
		if (interfaceMenu == 0) {
			gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, userCreated, MenuColors.editorColor);
		} else if (interfaceMenu == 4) {
			gameObject.AddComponent<MenuTransitions>().setScreens(confirmation, userCreated, MenuColors.editorColor);
		} else {
			gameObject.AddComponent<MenuTransitions>().setScreens(null, userCreated, MenuColors.editorColor);
		}
		updateFiles();
		interfaceMenu = 3;
	}

	public void openConfirmation() {
		if (interfaceMenu == 3) {
			gameObject.AddComponent<MenuTransitions>().setScreens(userCreated, confirmation, MenuColors.deletion);
			interfaceMenu = 4;
		}
	}

	public void deleteLevel() {
		string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
			userCreated.GetComponentInChildren<Text>().text = GetComponent<VariableManagement>().getUserLevel() + "\nEmpty";
			destroyBlockChildren();
			GetComponent<VariableManagement>().setLevelAuthorization(0);
			GetComponent<VariableManagement>().setLevelPostValue(0);
			updateFiles();
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
		interfaceMenu = 0;
		PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.toEditorFromMain);
	}
		
	public void loadUserLevel() {
		interfaceMenu = 0;
		string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		if (File.Exists(filePath)) {		
			PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
			gameObject.AddComponent<BackgroundColorTransition>();
			GetComponent<BackgroundColorTransition>().transition(VariableManagement.levelFromMain);
		}
	}

	public void left() {
		nameOfUserMap = "";
		dataOfUserMap = "";
		if (GetComponent<VariableManagement>().getUserLevel() > minAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, GetComponent<VariableManagement>().getUserLevel() - 1);
		} else {
			PlayerPrefs.SetInt(VariableManagement.userLevel, maxAmountOfUserLevels);
		}
		showLevel();
	}

	public void right() {
		nameOfUserMap = "";
		dataOfUserMap = "";
		if (GetComponent<VariableManagement>().getUserLevel() < maxAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, GetComponent<VariableManagement>().getUserLevel() + 1);
		} else {
			PlayerPrefs.SetInt(VariableManagement.userLevel, minAmountOfUserLevels);
		}
		showLevel();
	}

	public void leftTen() {
		nameOfUserMap = "";
		dataOfUserMap = "";
		if (GetComponent<VariableManagement>().getUserLevel() == minAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, maxAmountOfUserLevels);
		} else if (GetComponent<VariableManagement>().getUserLevel() - 10 > minAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, GetComponent<VariableManagement>().getUserLevel() - 10);
		} else {
			PlayerPrefs.SetInt(VariableManagement.userLevel, minAmountOfUserLevels);
		}
		showLevel();
	}

	public void rightTen() {
		nameOfUserMap = "";
		dataOfUserMap = "";
		if (GetComponent<VariableManagement>().getUserLevel() == maxAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, minAmountOfUserLevels);
		} else if (GetComponent<VariableManagement>().getUserLevel() + 10 < maxAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, GetComponent<VariableManagement>().getUserLevel() + 10);
		} else {
			PlayerPrefs.SetInt(VariableManagement.userLevel, maxAmountOfUserLevels);
		}
		showLevel();
	}

	void disableButtons() {
		turnOffButton(userCreated.GetComponentsInChildren<Button>()[0]);
		turnOffButton(userCreated.GetComponentsInChildren<Button>()[1]);
		turnOffButton(userCreated.GetComponentsInChildren<Button>()[2]);
		userCreated.GetComponentsInChildren<Image>()[0].color = Color.clear;
		userCreated.GetComponentsInChildren<Button>()[5].GetComponentInChildren<Text>().text = "Create";
	}

	void enableButtons() {
		if (GetComponent<VariableManagement>().isLevelAuthorized()) {
			userCreated.GetComponentsInChildren<Image>()[0].color = Color.white;
		} else {
			userCreated.GetComponentsInChildren<Image>()[0].color = Color.clear;
		}
		if (GetComponent<VariableManagement>().isLevelAuthorized() &&
				userNameOfMap == PlayerPrefs.GetString(VariableManagement.userName, "Unknown") &&
				PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0 &&
				!GetComponent<VariableManagement>().isLevelPosted()) {
			turnOnButton(userCreated.GetComponentsInChildren<Button>()[2]);
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Post";
		} else if (GetComponent<VariableManagement>().isLevelPosted()) {
			userCreated.GetComponentsInChildren<Button>()[2].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Posted";
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.clear;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().color = Color.white;
		} else if (userNameOfMap != PlayerPrefs.GetString(VariableManagement.userName, "Unknown")) {
			turnOffButton(userCreated.GetComponentsInChildren<Button>()[2]);
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "";
		} else if (PlayerPrefs.GetString(VariableManagement.userName, "Unknown") == "Unknown" ||
				PlayerPrefs.GetString(VariableManagement.userName, "Unknown") == "" ||
				PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 1) {
			turnOnButton(userCreated.GetComponentsInChildren<Button>()[2]);
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Sign in to Post";
		} else if (!GetComponent<VariableManagement>().isLevelAuthorized() &&
				userNameOfMap == PlayerPrefs.GetString(VariableManagement.userName, "Unknown") &&
				PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			userCreated.GetComponentsInChildren<Button>()[2].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Play to Authorize";
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().color = Color.white;
		}
		turnOnButton(userCreated.GetComponentsInChildren<Button>()[0]);
		turnOnButton(userCreated.GetComponentsInChildren<Button>()[1]);
		userCreated.GetComponentsInChildren<Button>()[5].GetComponentInChildren<Text>().text = "Edit";
	}

	public void postLevel () {
		if (dataOfUserMap != "" && GetComponent<VariableManagement>().isLevelAuthorized() && 
				userNameOfMap == PlayerPrefs.GetString (VariableManagement.userName, "Unknown") && 
				PlayerPrefs.GetString(VariableManagement.userName, "Unknown") != "Unknown" && 
				PlayerPrefs.GetString(VariableManagement.userName, "Unknown") != "" && 
				PlayerPrefs.GetInt (VariableManagement.isOnline, 0) == 0 && 
				!GetComponent<VariableManagement>().isLevelPosted()) {
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Posting...";
			GetComponent<FirebaseDatabases>().postLevel(dataOfUserMap, nameOfUserMap, userCreated.GetComponentsInChildren<Button>()[2]);
		}
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 1) {
			GetComponent<GooglePlay>().logIn();
			enableButtons();
		}
	}

	/* -------------------------------------------beyond the voyage-------------------------------------------------- */

	public void toDatabase () {
		databaseOrSearch = true;
		database.GetComponentInChildren<Text>().text = "Beyond the Voyage";
		destroyBlockChildren();
		if (interfaceMenu == 0) {
			gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, database, MenuColors.dataBaseInterface);
		} else if (interfaceMenu == 7) {
			gameObject.AddComponent<MenuTransitions>().setScreens(worldLevels, database, MenuColors.dataBaseInterface);
		} else if (interfaceMenu == 8) {
			gameObject.AddComponent<MenuTransitions>().setScreens(publicConfirmation, database, MenuColors.dataBaseInterface);
		} else if (interfaceMenu == 9) {
			gameObject.AddComponent<MenuTransitions>().setScreens(search, database, MenuColors.dataBaseInterface);
		}
		interfaceMenu = 6; 
	}

	public void toPublicLevels() {
		showPublicLevel();
		if (interfaceMenu == 6) {
			gameObject.AddComponent<MenuTransitions>().setScreens(database, worldLevels, MenuColors.dataBaseInterface);
		} else if (interfaceMenu == 8) {
			gameObject.AddComponent<MenuTransitions>().setScreens(publicConfirmation, worldLevels, MenuColors.dataBaseInterface);
		} else if (interfaceMenu == 9) {
			gameObject.AddComponent<MenuTransitions>().setScreens(search, worldLevels, MenuColors.dataBaseInterface);
		}
		interfaceMenu = 7;
	}

	public void getMostDownloadedLevels () {
		publicLevelCount = 0;
		lastDatabaseMenu = 0;
		database.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().fireBaseMostDownloaded(database.GetComponentInChildren<Text>());
	}

	public void getMostRecentLevels () {
		publicLevelCount = 0;
		lastDatabaseMenu = 1;
		database.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().fireBaseMostRecent(database.GetComponentInChildren<Text>());
	}

	public void getYourLevels () {
		publicLevelCount = 0;
		lastDatabaseMenu = 2;
		database.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().getYourLevels(database.GetComponentInChildren<Text>());
	}

	public void toDeletionConfirmationPublicLevel () {
		if (interfaceMenu == 7) {
			gameObject.AddComponent<MenuTransitions>().setScreens(worldLevels, publicConfirmation, MenuColors.deletion);
		}
		interfaceMenu = 8;
	}

	public void deletePublicLevel () {
		if (userID == PlayerPrefs.GetString(VariableManagement.userId, "Unknown")) {
			GetComponent<FirebaseDatabases>().deleteLevel(idOfMap, database.GetComponentInChildren<Text>(), lastDatabaseMenu, search.GetComponentInChildren<InputField>().text);
			destroyBlockChildren();
		}
	}

	public void toSearch () {
		search.GetComponentInChildren<Text>().text = "Search";
		databaseOrSearch = false;
		if (interfaceMenu == 6) {
			gameObject.AddComponent<MenuTransitions>().setScreens(database, search, MenuColors.dataBaseInterface);
		} else if (interfaceMenu == 7) {
			destroyBlockChildren();
			gameObject.AddComponent<MenuTransitions>().setScreens(worldLevels, search, MenuColors.dataBaseInterface);
		}
		interfaceMenu = 9;
	}

	public void searchUserName () {
		publicLevelCount = 0;
		lastDatabaseMenu = 3;
		search.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().searchUsername(search.GetComponentInChildren<Text>(), search.GetComponentInChildren<InputField>().text);
	}

	public void searchMapName () {
		publicLevelCount = 0;
		lastDatabaseMenu = 4;
		search.GetComponentInChildren<Text>().text = "Loading...";
		GetComponent<FirebaseDatabases>().searchMapName(search.GetComponentInChildren<Text>(), search.GetComponentInChildren<InputField>().text);
	}

	public void showPublicLevel() {
		destroyBlockChildren();
		if (publicLevelCount > GetComponent<FirebaseDatabases>().getLevelList().Count - 1) {
			publicLevelCount = GetComponent<FirebaseDatabases>().getLevelList().Count - 1;
		}
		turnOnButton(worldLevels.GetComponentsInChildren<Button>()[2]);		
		turnOnButton(worldLevels.GetComponentsInChildren<Button>()[4]);
		worldLevels.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Download";
		if (publicLevelCount == 0) {
			turnOffButton(worldLevels.GetComponentsInChildren<Button>()[1]);
		} else {
			turnOnButton(worldLevels.GetComponentsInChildren<Button>()[1]);
		}
		if (publicLevelCount == GetComponent<FirebaseDatabases>().getLevelList().Count - 1) {
			turnOffButton(worldLevels.GetComponentsInChildren<Button>()[3]);
		} else {
			turnOnButton(worldLevels.GetComponentsInChildren<Button>()[3]);
		}
		string[] lines;
		if (GetComponent<FirebaseDatabases>().getLevelList() != null) {
			worldLevels.GetComponentInChildren<Text>().text = "";
			worldLevels.GetComponentInChildren<Text>().text += GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][0] + "\n";
			worldLevels.GetComponentInChildren<Text>().text += "By: " + GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][1] + "\n";
			worldLevels.GetComponentInChildren<Text>().text += "Downloads: " + GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][3];
			mapDownloadCount = int.Parse(GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][3]);
			idOfMap = GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][5];
			userID = GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][6];
			lines = GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][4].Split(" "[0]);
			dataOfUserMap = GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][0] + "\n" + 
				GetComponent<FirebaseDatabases>().getLevelList()[publicLevelCount][1] + "\n" + 
				lines[0] + "\n" + lines[1] + "\n" + lines[2] + "\n" + lines[3];
			createSpriteLevel(lines, 4, "\n");
			dataOfUserMap += "*";
			if (userID == PlayerPrefs.GetString(VariableManagement.userId, "Unknown")) {
				turnOnButton(worldLevels.GetComponentsInChildren<Button>()[0]);
			} else {
				turnOffButton(worldLevels.GetComponentsInChildren<Button>()[0]);
			}
		}
	}

	public void nextPublicLevel () {
		if (GetComponent<FirebaseDatabases>().getLevelList() != null) {
			if (publicLevelCount < GetComponent<FirebaseDatabases>().getLevelList().Count - 1) {
				publicLevelCount++;
				showPublicLevel();
			}
		}
	}

	public void previousPublicLevel () {
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
				worldLevels.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Saved In Slot " + i;
				File.AppendAllText(filePath, dataOfUserMap);
				break;
			}
		}
		if (!saved) {
			worldLevels.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Storage Full!";
		}
	}

	public void toDatabaseOrSearch () {
		if (databaseOrSearch) {
			toDatabase();
		} else {
			toSearch();
		}
	}

	/* -------------------------------------------------------------------------------------------------------------- */
		
	public void nextScene(int n) {
		if (loading == false) {
			SceneManager.LoadScene(n);
		}
		loading = true;
	}

	public void showLevel() {
		int n = GetComponent<VariableManagement>().getUserLevel();
		string level = "";
		string[] userLevel;
		string[] lines;
		string filePath = Application.persistentDataPath + "/" + n + ".txt";
		level += "Slot " + n + "\n";
		destroyBlockChildren();
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
			disableButtons();
			level += "Empty";
		}
		userCreated.GetComponentInChildren<Text>().text = level;
	}

	public void createSpriteLevel (string[] lines, int num, string delimeter) {
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

	GameObject findAndSetUi (string s) {
		GameObject temp = GameObject.Find(s);
		temp.transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
		return temp;
	}

	void turnOffButton (Button b) {
		if (b.GetComponentInChildren<Text>() != null) {
			b.GetComponentInChildren<Text>().color = Color.clear;
		}
		b.GetComponent<Image>().color = Color.clear;
		b.interactable = false;
	}

	void turnOnButton (Button b) {
		if (b.GetComponentInChildren<Text>() != null) {
			b.GetComponentInChildren<Text>().color = Color.white;
		}
		b.GetComponent<Image>().color = Color.white;
		b.interactable = true;
	}

	void updateFiles () {
		if (filePositions == null) {
			filePositions = new List<float>();
		}
		filePositions.Clear();
		for (int i = minAmountOfUserLevels; i <= maxAmountOfUserLevels; i++) {
			string filePath = Application.persistentDataPath + "/" + i + ".txt";
			if (File.Exists(filePath)) {
				filePositions.Add(i * 0.15f);
			}
		}
	}

	void OnPostRender () {
		if (interfaceMenu == 3) {
			GL.PushMatrix();
			mat.SetPass(0);
			GL.Begin(GL.QUADS);
			GL.Color(Color.gray);
			GL.Vertex3((0.075f) - 7.5f, 0, 11.175f);
			GL.Vertex3((0.075f) - 7.5f, 0, 11.025f);
			GL.Vertex3(((15) - 0.075f) - 7.5f, 0, 11.025f);
			GL.Vertex3(((15) - 0.075f) - 7.5f, 0, 11.175f);
			GL.Color(Color.white);
			for (int i = 0; i < filePositions.Count; i++) {
				GL.Vertex3((filePositions[i] + 0.075f) - 7.5f, 0, 11.175f);
				GL.Vertex3((filePositions[i] + 0.075f) - 7.5f, 0, 11.025f);
				GL.Vertex3((filePositions[i] - 0.075f) - 7.5f, 0, 11.025f);
				GL.Vertex3((filePositions[i] - 0.075f) - 7.5f, 0, 11.175f);
			}
			GL.Color(Color.red);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * 0.15f) + 0.075f) - 7.5f, 0, 11.175f);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * 0.15f) + 0.075f) - 7.5f, 0, 11.025f);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * 0.15f) - 0.075f) - 7.5f, 0, 11.025f);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * 0.15f) - 0.075f) - 7.5f, 0, 11.175f);
			GL.End();
			GL.PopMatrix();
		}
	}

	public void unlockAllWorlds () {
		PlayerPrefs.SetInt("World0", 1);
		PlayerPrefs.SetInt("World1", 1);
		PlayerPrefs.SetInt("World2", 1);
	}
}
