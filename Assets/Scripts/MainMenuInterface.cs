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
	GameObject mainMenu, worlds, levels, userCreated, confirmation, popUp, intro, particles, worldLevels, database, floor, backgroundParticles;
	GameObject publicConfirmation, search, settings, store, musicObj;
	GameObject blockHolder, standardBlock, multistepBlock, switchBlock, redBlock, blueBlock, rotateRBlock, rotateLBlock, bombBlock, unknownBlock;
	List<Sprite> levelImages;
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
	bool loadingDatabase = false;
	string filePath;
	string currentLevelData;
	float guiBoxLength;
	float guiStart;
	float guiHeight = 15.5f;
	bool validLevel = false;

	void Start() {
		//PlayerPrefs.DeleteAll();
		floor = GameObject.Find("Floor");
		floor.transform.position = new Vector3 (Screen.width, 0, 0);
		blockHolder = GameObject.Find("Block Holder");
		particles = GameObject.Find("Sprite Holder");
		musicObj = GameObject.Find("Music");
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
		backgroundParticles = GameObject.Find("Particles2");
		standardBlock = GameObject.Find(VariableManagement.standardBlock);
		multistepBlock = GameObject.Find(VariableManagement.multistepBlock);
		switchBlock = GameObject.Find(VariableManagement.switchBlock);
		redBlock = GameObject.Find(VariableManagement.activeBlock);
		blueBlock = GameObject.Find(VariableManagement.inactiveBlock);
		rotateRBlock = GameObject.Find(VariableManagement.rotateRBlock);
		rotateLBlock = GameObject.Find(VariableManagement.rotateLBlock);
		bombBlock = GameObject.Find(VariableManagement.bombBlock);
		unknownBlock = GameObject.Find("Unknown Block");
		if (GetComponent<VariableManagement>().getUserLevel() < minAmountOfUserLevels ||
		    GetComponent<VariableManagement>().getUserLevel() > maxAmountOfUserLevels) {
			PlayerPrefs.SetInt(VariableManagement.userLevel, minAmountOfUserLevels);
		}
		guiBoxLength = ((float)Screen.width/(float)Screen.height) * 0.2725f;
		guiStart = guiBoxLength * (maxAmountOfUserLevels/2);
		levelImages = new List<Sprite>();
		for (int i = 0; i < 80; i++) {
			levelImages.Add(Resources.Load<Sprite>("Levels/" + (((i/16) + 1) + "-" + ((i%16) + 1))));
		}
		goToLastMenu();
		PlayerPrefs.SetString(VariableManagement.lastMenu, ""); 
		GetComponent<VariableManagement>().turnOffCameraShift();
		updateFiles();
		refreshStore();
		if (PlayerPrefs.GetInt(VariableManagement.playMusic, 0) == 0) {
			settings.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = "Music: On";
		} else {
			settings.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = "Music: Off";
		}
		if (PlayerPrefs.GetInt(VariableManagement.playSounds, 0) == 0) {
			settings.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = "Sounds: On";
		} else {
			settings.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = "Sounds: Off";
		}
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			settings.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Power Saver: Off";
			Application.targetFrameRate = 60;
		} else {
			settings.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Power Saver: On";
			Application.targetFrameRate = 30;
		}
		gameObject.AddComponent<BackgroundColorTransition>().levelStarting();
	}

	void goToLastMenu () {
		if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.worldMenu) {
			interfaceMenu = 2;
			toLevelSelect((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16);
			if (((GetComponent<VariableManagement>().getWorldLevel()) / 16) == 0) {
				Camera.main.backgroundColor = MenuColors.world1Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel()) / 16) == 1) {
				Camera.main.backgroundColor = MenuColors.world2Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel()) / 16) == 2) {
				Camera.main.backgroundColor = MenuColors.world3Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel()) / 16) == 3) {
				Camera.main.backgroundColor = MenuColors.world4Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel()) / 16) == 4) {
				Camera.main.backgroundColor = MenuColors.world5Color;
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
		if (PlayerPrefs.GetInt(VariableManagement.savePower) == 0) {
			particles.GetComponentInChildren<ParticleSystem>().Play();
			if (GetComponent<Intro>() == null) {
				gameObject.AddComponent<Intro>();
			}
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
		if (PlayerPrefs.GetInt(VariableManagement.savePower) == 0 && !backgroundParticles.GetComponent<ParticleSystem>().isPlaying) {
			backgroundParticles.GetComponent<ParticleSystem>().Play();
		}
	}

	public void toSettings () {
		gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, settings, MenuColors.settingsColor);
		interfaceMenu = 10;
	}

	public void savePower () {
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			PlayerPrefs.SetInt(VariableManagement.savePower, 1);
			settings.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Power Saver: On";
			Application.targetFrameRate = 30;
			backgroundParticles.GetComponent<ParticleSystem>().Clear();
			backgroundParticles.GetComponent<ParticleSystem>().Stop();
		} else {
			PlayerPrefs.SetInt(VariableManagement.savePower, 0);
			settings.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Power Saver: Off";
			Application.targetFrameRate = 60;
			backgroundParticles.GetComponent<ParticleSystem>().Play();
		}
	}

	public void music () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic, 0) == 0) {
			settings.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = "Music: Off";
			musicObj.GetComponent<MusicManager>().turnOffOnMusic();
		} else {
			settings.GetComponentsInChildren<Button>()[0].GetComponentInChildren<Text>().text = "Music: On";
			musicObj.GetComponent<MusicManager>().turnOffOnMusic();
		}
	}

	public void sounds () {
		if (PlayerPrefs.GetInt(VariableManagement.playSounds, 0) == 0) {
			PlayerPrefs.SetInt(VariableManagement.playSounds, 1);
			settings.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = "Sounds: Off";
			GetComponent<SoundsAndMusic>().updatePlaySounds();
		} else {
			PlayerPrefs.SetInt(VariableManagement.playSounds, 0);
			settings.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = "Sounds: On";
			GetComponent<SoundsAndMusic>().updatePlaySounds();
		}
	}

	public void restorePurchases () {
		GetComponent<InAppPurchases>().RestorePurchases();
	}

	public void toStore () {
		store.GetComponentInChildren<Text>().text = "Store";
		gameObject.AddComponent<MenuTransitions>().setScreens(mainMenu, store, MenuColors.storeColor);
		interfaceMenu = 11;
	}

	public void boughtItem () {
		if (interfaceMenu == 5) {
			toWorldSelect();
		} else {
			GetComponent<SoundsAndMusic>().playUnlockSound();
		}
		refreshStore();
	}

	public void refreshStore () {
		store.GetComponentInChildren<Text>().text = "Unlocked!";
		if (PlayerPrefs.GetInt(VariableManagement.world0) == 1) {
			turnOffStoreButton(store.GetComponentsInChildren<Button>()[0]);
		}
		if (PlayerPrefs.GetInt(VariableManagement.world1) == 1) {
			turnOffStoreButton(store.GetComponentsInChildren<Button>()[1]);
		}
		if (PlayerPrefs.GetInt(VariableManagement.world2) == 1) {
			turnOffStoreButton(store.GetComponentsInChildren<Button>()[2]);
		}
		if (PlayerPrefs.GetInt(VariableManagement.world3) == 1) {
			turnOffStoreButton(store.GetComponentsInChildren<Button>()[3]);
		}
		bool unlockedAll = true;
		for (int i = 0; i < 50; i++) {
			if (PlayerPrefs.GetInt("World" + i) == 0) {
				unlockedAll = false;
				break;
			}
		}
		if (unlockedAll) {
			turnOffStoreButton(store.GetComponentsInChildren<Button>()[4]);
		}
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
			GetComponent<SoundsAndMusic>().playUnlockSound();
			popUp.GetComponentsInChildren<Text>()[0].text = "Congrats!\nNew Unlocks!";
			popUp.GetComponentsInChildren<Text>()[1].text = "";
			turnOffButton(popUp.GetComponentsInChildren<Button>()[0]);
			turnOffButton(popUp.GetComponentsInChildren<Button>()[1]);
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
		for (int i = 1; i < 5; i++) {
			if (PlayerPrefs.GetInt(("World" + (i - 1)).ToString(), 0) == 0) {
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Image>().color = Color.black;
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Text>().color = Color.clear;
			} else {
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Image>().color = Color.white;
				worlds.GetComponentsInChildren<Button>()[i].GetComponentInChildren<Text>().color = Color.white;
				if (PlayerPrefs.GetInt("Beat World" + (i - 1).ToString(), 0) == 1) {
					worlds.GetComponentsInChildren<Button>()[i - 1].GetComponentsInChildren<Image>()[1].enabled = true;
				}
			}
		}
	}

	public void toLevelSelect(int world) {
		bool beatAllLevels = true;
		PlayerPrefs.SetInt(VariableManagement.worldLevel, (world * 16));
		if (world > 0) {
			if (PlayerPrefs.GetInt("World" + (world - 1).ToString(), 0) == 0) {
				beatAllLevels = false;
			}
		} 
		if (!beatAllLevels) {
			interfaceMenu = 5;
			gameObject.AddComponent<MenuTransitions>().setScreens(worlds, popUp, MenuColors.worldColor);
			popUp.GetComponentsInChildren<Text>()[0].text = "Locked!";
			popUp.GetComponentsInChildren<Text>()[1].text = "Must beat all levels from the previous world or select option:";
			turnOnButton(popUp.GetComponentsInChildren<Button>()[0]);
			turnOnButton(popUp.GetComponentsInChildren<Button>()[1]);
		} else {
			if (world == 0) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world1Color);
				levels.GetComponentInChildren<Text>().text = "Block World";
			} else if (world == 1) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world2Color);
				levels.GetComponentInChildren<Text>().text = "Multistep World";
			} else if (world == 2) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world3Color);
				levels.GetComponentInChildren<Text>().text = "Switch World";
			} else if (world == 3) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world4Color);
				levels.GetComponentInChildren<Text>().text = "Rotate World";
			} else if (world == 4) {
				gameObject.AddComponent<MenuTransitions>().setScreens(worlds, levels, MenuColors.world5Color);
				levels.GetComponentInChildren<Text>().text = "Bomb World";
			}
			interfaceMenu = 2;
			levelMultiplier = world * 16;
			for (int i = 0; i < 16; i++) {
				int levelNumber = (world * 16) + i;
				if (PlayerPrefs.GetInt(levelNumber.ToString(), 0) == 1) {
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Text>()[0].text = "";
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].color = Color.white;
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].sprite = levelImages[levelNumber];
				} else {
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Image>()[1].color = Color.clear;
					levels.GetComponentsInChildren<Button>()[i].GetComponentsInChildren<Text>()[0].text = (i + 1).ToString();
				}
			}
		}
		if (PlayerPrefs.GetInt(VariableManagement.savePower) == 0 && !backgroundParticles.GetComponent<ParticleSystem>().isPlaying) {
			backgroundParticles.GetComponent<ParticleSystem>().Play();
		}
	}

	/* -------------------------------------------play, create, share------------------------------------------------ */

	public void toUserCreatedLevels() {
		userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Post";
		userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.clear;
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
		if (PlayerPrefs.GetInt(VariableManagement.savePower) == 0 && !backgroundParticles.GetComponent<ParticleSystem>().isPlaying) {
			backgroundParticles.GetComponent<ParticleSystem>().Play();
		}
	}

	public void openConfirmation() {
		if (interfaceMenu == 3) {
			gameObject.AddComponent<MenuTransitions>().setScreens(userCreated, confirmation, MenuColors.deletion);
			interfaceMenu = 4;
		}
	}

	public void deleteLevel() {
		string path = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		if (File.Exists(path)) {
			File.Delete(path);
			userCreated.GetComponentInChildren<Text>().text = GetComponent<VariableManagement>().getUserLevel() + "\nEmpty";
			destroyBlockChildren();
			GetComponent<VariableManagement>().setLevelAuthorization(0);
			GetComponent<VariableManagement>().setLevelPostValue(0);
			PlayerPrefs.SetString("Data" + GetComponent<VariableManagement>().getUserLevel(), "");
			updateFiles();
		}
		toUserCreatedLevels();
	}

	public void LoadLevel(int level) {
		if (gameObject.GetComponent<MenuTransitions>() == null && gameObject.GetComponent<BackgroundColorTransition>() == null) {
			PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.worldMenu);
			PlayerPrefs.SetInt(VariableManagement.worldLevel, level + levelMultiplier);
			gameObject.AddComponent<BackgroundColorTransition>();
			GetComponent<BackgroundColorTransition>().transition(VariableManagement.levelFromMain);
		}
	}

	public void openEditor() {
		if (gameObject.GetComponent<MenuTransitions>() == null && gameObject.GetComponent<BackgroundColorTransition>() == null) {
			interfaceMenu = 0;
			destroyBlockChildren();
			PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
			gameObject.AddComponent<BackgroundColorTransition>();
			GetComponent<BackgroundColorTransition>().transition(VariableManagement.toEditorFromMain);
		}
	}
		
	public void loadUserLevel() {
		if (gameObject.GetComponent<MenuTransitions>() == null && gameObject.GetComponent<BackgroundColorTransition>() == null) {
			interfaceMenu = 0;
			destroyBlockChildren();
			filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
			if (File.Exists(filePath)) {		
				PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
				gameObject.AddComponent<BackgroundColorTransition>();
				GetComponent<BackgroundColorTransition>().transition(VariableManagement.levelFromMain);
			}
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
		userCreated.GetComponentsInChildren<Image>()[1].color = Color.clear;
		userCreated.GetComponentsInChildren<Button>()[5].GetComponentInChildren<Text>().text = "Create";
	}

	void enableButtons() {
		turnOnButton(userCreated.GetComponentsInChildren<Button>()[0]);
		turnOnButton(userCreated.GetComponentsInChildren<Button>()[1]);
		turnOnButton(userCreated.GetComponentsInChildren<Button>()[5]);
		userCreated.GetComponentsInChildren<Button>()[5].GetComponentInChildren<Text>().text = "Edit";
		if (GetComponent<VariableManagement>().isLevelAuthorized()) {
			userCreated.GetComponentsInChildren<Image>()[1].color = Color.white;
		} else {
			userCreated.GetComponentsInChildren<Image>()[1].color = Color.clear;
		}
		if (!validLevel) {
			userCreated.GetComponentsInChildren<Button>()[1].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Image>().color = Color.clear;
			userCreated.GetComponentsInChildren<Button>()[2].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.clear;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().color = Color.white;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Unknown Block!";
			userCreated.GetComponentsInChildren<Button>()[5].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[5].GetComponentInChildren<Image>().color = Color.clear;
			userCreated.GetComponentsInChildren<Button>()[5].GetComponentInChildren<Text>().text = "Update may be needed";
		} else if (GetComponent<VariableManagement>().isLevelAuthorized() &&
				userNameOfMap == PlayerPrefs.GetString(VariableManagement.userName) &&
				GetComponent<VariableManagement>().isOnlineCheck() &&
				!GetComponent<VariableManagement>().isLevelPosted() && 
				PlayerPrefs.GetString("Data" + GetComponent<VariableManagement>().getUserLevel()) == currentLevelData) {
			turnOnButton(userCreated.GetComponentsInChildren<Button>()[2]);
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.white;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Post";
		} else if (GetComponent<VariableManagement>().isLevelPosted()) {
			userCreated.GetComponentsInChildren<Button>()[2].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.clear;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().color = Color.white;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Posted";
		} else if (userNameOfMap != PlayerPrefs.GetString(VariableManagement.userName)) {
			turnOffButton(userCreated.GetComponentsInChildren<Button>()[2]);
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.clear;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "";
		} else if (!GetComponent<VariableManagement>().isOnlineCheck()) {
			turnOnButton(userCreated.GetComponentsInChildren<Button>()[2]);
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.white;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Sign in to Post";
		} else if (PlayerPrefs.GetInt("User" + GetComponent<VariableManagement>().getUserLevel()) == 0 && 
				GetComponent<VariableManagement>().isOnlineCheck() && 
				PlayerPrefs.GetString("Data" + GetComponent<VariableManagement>().getUserLevel()) == currentLevelData) {
			userCreated.GetComponentsInChildren<Button>()[2].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().color = Color.white;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Play to Authorize";
		} else if (PlayerPrefs.GetString("Data" + GetComponent<VariableManagement>().getUserLevel()) != currentLevelData) {
			userCreated.GetComponentsInChildren<Button>()[2].interactable = false;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Image>().color = Color.clear;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().color = Color.white;
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Resave and Test";
		}
	}

	public void postLevel () {
		if (dataOfUserMap != "" && GetComponent<VariableManagement>().isLevelAuthorized() && 
				userNameOfMap == PlayerPrefs.GetString (VariableManagement.userName) && 
				GetComponent<VariableManagement>().isOnlineCheck() && 
				!GetComponent<VariableManagement>().isLevelPosted() && 
				PlayerPrefs.GetString("Data" + GetComponent<VariableManagement>().getUserLevel()) == currentLevelData) {
			userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Posting...";
			if (!loadingDatabase) {
				StartCoroutine(postLevelCoroutine());
			}
		}
		if (!GetComponent<VariableManagement>().isOnlineCheck()) {
			GetComponent<OnlineServices>().logIn();
			enableButtons();
		}
	}

	IEnumerator postLevelCoroutine () {
		loadingDatabase = true;
		yield return new WaitForSeconds(0.1f);
		GetComponent<FirebaseDatabases>().postLevel(dataOfUserMap, nameOfUserMap, userCreated.GetComponentsInChildren<Button>()[2]);
		loadingDatabase = false;
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
		if (!loadingDatabase) {
			publicLevelCount = 0;
			lastDatabaseMenu = 0;
			database.GetComponentInChildren<Text>().text = "Loading...";
			StartCoroutine(mostDownloadedCoroutine());
		}
	}

	IEnumerator mostDownloadedCoroutine () {
		loadingDatabase = true;
		yield return new WaitForSeconds(0.1f);
		GetComponent<FirebaseDatabases>().fireBaseMostDownloaded(database.GetComponentInChildren<Text>());
		loadingDatabase = false;
	}

	public void getMostRecentLevels () {
		if (!loadingDatabase) {
			publicLevelCount = 0;
			lastDatabaseMenu = 1;
			database.GetComponentInChildren<Text>().text = "Loading...";
			StartCoroutine(mostRecentCoroutine());
		}
	}

	IEnumerator mostRecentCoroutine () {
		loadingDatabase = true;
		yield return new WaitForSeconds(0.1f);
		GetComponent<FirebaseDatabases>().fireBaseMostRecent(database.GetComponentInChildren<Text>());
		loadingDatabase = false;
	}

	public void getYourLevels () {
		if (!loadingDatabase) {
			publicLevelCount = 0;
			lastDatabaseMenu = 2;
			database.GetComponentInChildren<Text>().text = "Loading...";
			StartCoroutine(yourLevelsCoroutine());
		}
	}

	IEnumerator yourLevelsCoroutine () {
		loadingDatabase = true;
		yield return new WaitForSeconds(0.1f);
		GetComponent<FirebaseDatabases>().getYourLevels(database.GetComponentInChildren<Text>());
		loadingDatabase = false;
	}

	public void toDeletionConfirmationPublicLevel () {
		if (interfaceMenu == 7) {
			gameObject.AddComponent<MenuTransitions>().setScreens(worldLevels, publicConfirmation, MenuColors.deletion);
		}
		interfaceMenu = 8;
	}

	public void deletePublicLevel () {
		if (userID == PlayerPrefs.GetString(VariableManagement.userId)) {
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
		if (!loadingDatabase) {
			publicLevelCount = 0;
			lastDatabaseMenu = 3;
			search.GetComponentInChildren<Text>().text = "Loading...";
			StartCoroutine(userNameCoroutine());
		}
	}

	IEnumerator userNameCoroutine () {
		loadingDatabase = true;
		yield return new WaitForSeconds(0.1f);
		GetComponent<FirebaseDatabases>().searchUsername(search.GetComponentInChildren<Text>(), search.GetComponentInChildren<InputField>().text);
		loadingDatabase = false;
	}

	public void searchMapName () {
		if (!loadingDatabase) {
			publicLevelCount = 0;
			lastDatabaseMenu = 4;
			search.GetComponentInChildren<Text>().text = "Loading...";
			StartCoroutine(mapNameCoroutine());
		}
	}

	IEnumerator mapNameCoroutine () {
		loadingDatabase = true;
		yield return new WaitForSeconds(0.1f);
		GetComponent<FirebaseDatabases>().searchMapName(search.GetComponentInChildren<Text>(), search.GetComponentInChildren<InputField>().text);
		loadingDatabase = false;
	}

	public void checkText () {
		string newInputFiltered = ""; 
		foreach (char c in search.GetComponentInChildren<InputField>().text) { 
			if (System.Convert.ToInt32(c) < 50000 && c != '\n') { 
				newInputFiltered += c;
			} 
		}
		search.GetComponentInChildren<InputField>().text = newInputFiltered;
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
			if (userID == PlayerPrefs.GetString(VariableManagement.userId)) {
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
			string path = Application.persistentDataPath + "/" + i + ".txt";
			if (!File.Exists(path)) {
				saved = true;
				GetComponent<FirebaseDatabases>().incrementDownloadCount(idOfMap, mapDownloadCount);
				GetComponent<OnlineServices>().downloadLevelUnlock();
				worldLevels.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Saved In Slot " + i;
				File.AppendAllText(path, dataOfUserMap);
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
		string data;
		filePath = Application.persistentDataPath + "/" + n + ".txt";
		level += "Slot " + n + "\n";
		destroyBlockChildren();
		if (File.Exists(filePath)) {
			StreamReader r = File.OpenText(filePath);
			data = r.ReadToEnd();
			currentLevelData = data;
			userLevel = data.Split(VariableManagement.levelDelimiter.ToString()[0]);
			lines = userLevel[0].Split("\n"[0]);
			level += lines[0] + "\n";
			level += "By: " + lines[1];
			nameOfUserMap = lines[0];
			userNameOfMap = lines[1];
			dataOfUserMap = lines[2] + " " + lines[3] + " " + lines[4] + " " + lines[5];
			createSpriteLevel(lines, 6, " ");
			if (PlayerPrefs.GetString("Data" + n) != currentLevelData) {
				GetComponent<VariableManagement>().setLevelPostValue(0);
				userCreated.GetComponentsInChildren<Button>()[2].GetComponentInChildren<Text>().text = "Edit and Test Again";
			}
			enableButtons();
		} else {
			disableButtons();
			level += "Empty";
		}
		userCreated.GetComponentInChildren<Text>().text = level;
	}

	public void createSpriteLevel (string[] lines, int num, string delimeter) {
		validLevel = true;
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
				} else if (lines[i][j] == VariableManagement.bombBlockTile) {
					displayBlockImage(i - num, j, bombBlock);
				} else if (lines[i][j] == VariableManagement.noBlockTile) {
					
				} else {
					displayBlockImage(i - num, j, unknownBlock);
					validLevel = false;
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

	void turnOffStoreButton (Button b) {
		if (b.GetComponentInChildren<Text>() != null) {
			b.GetComponentInChildren<Text>().color = Color.clear;
			b.GetComponentInChildren<Text>().raycastTarget = false;
		}
		b.GetComponent<Image>().color = Color.black;
		b.GetComponent<Image>().raycastTarget = false;
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
			string path = Application.persistentDataPath + "/" + i + ".txt";
			if (File.Exists(path)) {
				filePositions.Add(i * guiBoxLength);
			}
		}
	}

	void OnPostRender () {
		if (interfaceMenu == 3) {
			GL.PushMatrix();
			mat.SetPass(0);
			GL.Begin(GL.QUADS);
			GL.Color(Color.gray);
			GL.Vertex3((guiBoxLength/2) - guiStart, 0, guiHeight - guiBoxLength);
			GL.Vertex3((guiBoxLength/2) - guiStart, 0, guiHeight);
			GL.Vertex3(((guiBoxLength * maxAmountOfUserLevels) - guiBoxLength/2) - guiStart, 0, guiHeight);
			GL.Vertex3(((guiBoxLength * maxAmountOfUserLevels) - guiBoxLength/2) - guiStart, 0, guiHeight - guiBoxLength);
			GL.Color(Color.white);
			for (int i = 0; i < filePositions.Count; i++) {
				GL.Vertex3((filePositions[i] + (guiBoxLength/2)) - guiStart, 0, guiHeight - guiBoxLength);
				GL.Vertex3((filePositions[i] + (guiBoxLength/2)) - guiStart, 0, guiHeight);
				GL.Vertex3((filePositions[i] - (guiBoxLength/2)) - guiStart, 0, guiHeight);
				GL.Vertex3((filePositions[i] - (guiBoxLength/2)) - guiStart, 0, guiHeight - guiBoxLength);
			}
			GL.Color(Color.red);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * guiBoxLength) + (guiBoxLength/2)) - guiStart, 0, guiHeight - guiBoxLength);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * guiBoxLength) + (guiBoxLength/2)) - guiStart, 0, guiHeight);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * guiBoxLength) - (guiBoxLength/2)) - guiStart, 0, guiHeight);
			GL.Vertex3(((GetComponent<VariableManagement>().getUserLevel() * guiBoxLength) - (guiBoxLength/2)) - guiStart, 0, guiHeight - guiBoxLength);
			GL.End();
			GL.PopMatrix();
		}
	}

	public void unlockWorld () {
		int world = (GetComponent<VariableManagement>().getWorldLevel() - 1) / 16;
		if (world == 0) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld2();
		} else if (world == 1) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld3();
		} else if (world == 2) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld4();
		} else if (world == 3) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld5();
		}
	}

	public void unlockWorldByNum (int world) {
		if (world == 0) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld2();
		} else if (world == 1) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld3();
		} else if (world == 2) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld4();
		} else if (world == 3) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld5();
		}
	}

	public void unlockAllWorlds () {
		GetComponent<InAppPurchases>().BuyNonConsumableAll();
	}
}
