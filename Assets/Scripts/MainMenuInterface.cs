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
	Transform confirmation;
	GameObject worldText;
	GameObject userText;
	GameObject blockHolder;
	GameObject standardBlock;
	GameObject multistepBlock;
	GameObject switchBlock;
	GameObject redBlock;
	GameObject blueBlock;
	GameObject playButton;
	GameObject editButton;
	GameObject deleteButton;
	int levelMultiplier = 1;
	int interfaceMenu = 0;
	float deltaTime = 0;
	bool transition = false;

	void Start () {
		mainMenu = GameObject.Find ("Menu").transform;
		worlds = GameObject.Find ("Worlds").transform;
		levels = GameObject.Find ("Levels").transform;
		userCreated = GameObject.Find("User Created").transform;
		confirmation = GameObject.Find("Confirmation").transform;
		worldText = GameObject.Find ("World Text");
		userText = GameObject.Find("Level");
		blockHolder = GameObject.Find("Block Holder");
		standardBlock = GameObject.Find("Standard Block");
		multistepBlock = GameObject.Find("Multistep Block");
		switchBlock = GameObject.Find("Switch Block");
		redBlock = GameObject.Find("Red Block");
		blueBlock = GameObject.Find("Blue Block");
		playButton = GameObject.Find("Play");
		editButton = GameObject.Find("Edit");
		deleteButton = GameObject.Find("Delete");
		if (PlayerPrefs.GetInt("User Level") == 0 || PlayerPrefs.GetInt("User Level") > 100) {
			PlayerPrefs.SetInt("User Level", 1);
		}
		if (PlayerPrefs.GetString("Last Menu") == "Campaign") {
			toLevelSelect(((PlayerPrefs.GetInt("Level", 0) - 1)/16));
		} else if (PlayerPrefs.GetString("Last Menu") == "User") {
			toUserCreatedLevels();
		} else {
			toMainMenu();
		}
		PlayerPrefs.SetString("Last Menu", "");
		PlayerPrefs.SetInt("Shift Camera", 0);
	}

	void Update () {
		if (transition) { 
			deltaTime += Time.deltaTime * 1.5f;
			if (deltaTime > 1) {
				transition = false;
			}
			if (interfaceMenu == 0) {
				worlds.localScale = Vector3.Slerp(worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp(levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp(mainMenu.localScale, Vector3.one, deltaTime);
				userCreated.localScale = Vector3.Slerp(userCreated.localScale, Vector3.zero, deltaTime);
				confirmation.localScale = Vector3.Slerp(confirmation.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 1) {
				worlds.localScale = Vector3.Slerp(worlds.localScale, Vector3.one, deltaTime);
				levels.localScale = Vector3.Slerp(levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp(mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp(userCreated.localScale, Vector3.zero, deltaTime);
				confirmation.localScale = Vector3.Slerp(confirmation.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 2) {
				worlds.transform.localScale = Vector3.Slerp(worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp(levels.localScale, Vector3.one, deltaTime);
				mainMenu.localScale = Vector3.Slerp(mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp(userCreated.localScale, Vector3.zero, deltaTime);
				confirmation.localScale = Vector3.Slerp(confirmation.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 3) {
				worlds.transform.localScale = Vector3.Slerp(worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp(levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp(mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp(userCreated.localScale, Vector3.one, deltaTime);
				confirmation.localScale = Vector3.Slerp(confirmation.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 4) {
				worlds.transform.localScale = Vector3.Slerp(worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp(levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp(mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp(userCreated.localScale, Vector3.zero, deltaTime);
				confirmation.localScale = Vector3.Slerp(confirmation.localScale, Vector3.one, deltaTime);
			}
		}
	}

	public void LoadLevel (int level) {
		PlayerPrefs.SetString("Last Menu", "Campaign");
		PlayerPrefs.SetInt ("Level", level + levelMultiplier);
		GetComponent<BackgroundColorTransition> ().transition ("Level From Main Menu");
	}
		
	public void openEditor () {
		PlayerPrefs.SetString("Last Menu", "User");
		destroyBlockChildren();
		GetComponent<BackgroundColorTransition> ().transition ("Editor From Main Menu");
	}

	public void openConfirmation () {
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
		if (File.Exists(filePath)) {
			enableTransition(4);
		}
	}

	public void cancelDeletion () {
		enableTransition(3);
	}

	public void deleteLevel () {
		disableButtons();
		enableTransition (3);
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
			userText.GetComponent<Text>().text = PlayerPrefs.GetInt("User Level") + "\n\n\n\n\n\n\n\n\nEmpty\n\n\n\n\n\n";
			destroyBlockChildren();
		}
	}

	public void loadUserLevel () {
		PlayerPrefs.SetString("Last Menu", "User");
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
		if (File.Exists(filePath)) {
			destroyBlockChildren();
			GetComponent<BackgroundColorTransition>().transition("Level From Main Menu");
		}
	}

	public void nextScene (int n) {
		if (loading == false) {
			SceneManager.LoadScene (n);
		}
		loading = true;
	}

	public void toMainMenu () {
		destroyBlockChildren();
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
		confirmation.localScale = Vector3.zero;
		deltaTime = 0;
		disableButtons();
		showLevel();
		enableTransition (3);
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

	void enableTransition (int i) {
		interfaceMenu = i;
		deltaTime = 0;
		transition = true;
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
