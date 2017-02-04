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
	GameObject worldText;
	GameObject userText;
	GameObject blockHolder;
	GameObject standardBlock;
	GameObject multistepBlock;
	GameObject switchBlock;
	GameObject redBlock;
	GameObject blueBlock;
	int levelMultiplier = 1;
	int loadedLevel;
	int interfaceMenu = 0;
	float deltaTime = 0;
	bool transition = false;
	int currentLevel = 1601;
	bool movingFromLeft, movingToRightFromLeft, movingFromRight, movingToLeftFromRight = false;

	void Start () {
		mainMenu = GameObject.Find ("Menu").transform;
		worlds = GameObject.Find ("Worlds").transform;
		levels = GameObject.Find ("Levels").transform;
		userCreated = GameObject.Find("User Created").transform;
		worldText = GameObject.Find ("World Text");
		userText = GameObject.Find("Level");
		blockHolder = GameObject.Find("Block Holder");
		standardBlock = GameObject.Find("Standard Block");
		multistepBlock = GameObject.Find("Multistep Block");
		switchBlock = GameObject.Find("Switch Block");
		redBlock = GameObject.Find("Red Block");
		blueBlock = GameObject.Find("Blue Block");
		toMainMenu();
	}

	void Update () {
		if (transition) { 
			deltaTime += Time.deltaTime * 1.25f;
			if (deltaTime > 1) {
				transition = false;
			}
			if (interfaceMenu == 0) {
				worlds.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.one, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 1) {
				worlds.localScale = Vector3.Slerp (worlds.localScale, Vector3.one, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 2) {
				worlds.transform.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.one, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.zero, deltaTime);
			} else if (interfaceMenu == 3) {
				worlds.transform.localScale = Vector3.Slerp (worlds.localScale, Vector3.zero, deltaTime);
				levels.localScale = Vector3.Slerp (levels.localScale, Vector3.zero, deltaTime);
				mainMenu.localScale = Vector3.Slerp (mainMenu.localScale, Vector3.zero, deltaTime);
				userCreated.localScale = Vector3.Slerp (userCreated.localScale, Vector3.one, deltaTime);
			}
		}
		if (movingFromLeft) {
			blockHolder.transform.position = Vector3.Lerp(blockHolder.transform.position, Vector3.right * 15, Time.deltaTime * 20);
			if (Vector3.Distance(blockHolder.transform.position, Vector3.right * 15) < 1) {
				destroyBlockChildren();
				blockHolder.transform.position = Vector3.zero;
				showLevel(currentLevel);
				blockHolder.transform.position = Vector3.left * 15;
				movingFromLeft = false;
				movingToRightFromLeft = true;
			}
		} else if (movingToRightFromLeft) {
			blockHolder.transform.position = Vector3.Lerp(blockHolder.transform.position, Vector3.zero, Time.deltaTime * 20);
			if (Vector3.Distance(blockHolder.transform.position, Vector3.zero) < 0.01f) {
				blockHolder.transform.position = Vector3.zero;
				movingToRightFromLeft = false;
			}
		} 
		if (movingFromRight) {
			blockHolder.transform.position = Vector3.Lerp(blockHolder.transform.position, Vector3.left * 15, Time.deltaTime * 20);
			if (Vector3.Distance(blockHolder.transform.position, Vector3.left * 15) < 1) {
				destroyBlockChildren();
				blockHolder.transform.position = Vector3.zero;
				showLevel(currentLevel);
				blockHolder.transform.position = Vector3.right * 15;
				movingFromRight = false;
				movingToLeftFromRight = true;
			}
		} else if (movingToLeftFromRight) {
			blockHolder.transform.position = Vector3.Lerp(blockHolder.transform.position, Vector3.zero, Time.deltaTime * 20);
			if (Vector3.Distance(blockHolder.transform.position, Vector3.zero) < 0.01f) {
				blockHolder.transform.position = Vector3.zero;
				movingToLeftFromRight = false;
			}
		}
	}

	public void LoadLevel (int level) {
		loadedLevel = (level + levelMultiplier);
		PlayerPrefs.SetInt ("Level", loadedLevel);
		GetComponent<BackgroundColorTransition> ().transition (loadedLevel, "Level From Main Menu");
	}
		
	public void openEditor () {
		destroyBlockChildren();
		PlayerPrefs.SetString("Back", "Go Back To Editor");
		GetComponent<BackgroundColorTransition> ().transition (loadedLevel, "Editor From Main Menu");
	}

	public void deleteLevel () {
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("Level", 0) - 1) + ".txt";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
			userText.GetComponent<Text>().text = (currentLevel - 1600) + "\n\n\n\n\n\n\n\n\nEmpty\n\n\n\n\n\n";
			destroyBlockChildren();
		}
	}

	public void loadUserLevel () {
		PlayerPrefs.SetString("Back", "Go Back To Menu");
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("Level", 0) - 1) + ".txt";
		if (File.Exists(filePath)) {
			destroyBlockChildren();
			GetComponent<BackgroundColorTransition>().transition(loadedLevel, "Level From Main Menu");
		}
	}

	public void nextScene (int n) {
		PlayerPrefs.SetInt ("Shift Camera", 0);
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
		showLevel(currentLevel);
		blockHolder.transform.position = Vector3.left * 15;
		movingToRightFromLeft = true;
		enableTransition (3);
	}

	public void left () {
		if (!movingFromLeft && !movingFromRight && !movingToLeftFromRight && !movingToRightFromLeft) {
			movingFromLeft = true;
			if (currentLevel > 1601) {
				currentLevel--;
			} else {
				currentLevel = 1700;
			}
		}
	}

	public void right () {
		if (!movingFromLeft && !movingFromRight && !movingToLeftFromRight && !movingToRightFromLeft) {
			movingFromRight = true;
			if (currentLevel < 1700) {
				currentLevel++;
			} else {
				currentLevel = 1601;
			}
		}
	}

	void enableTransition (int i) {
		interfaceMenu = i;
		deltaTime = 0;
		transition = true;
	}

	public void showLevel(int n) {
		PlayerPrefs.SetInt("Level", n);
		string level = "";
		string[] userLevel;
		string[] lines;
		string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("Level", 0) - 1) + ".txt";
		level += (n - 1600) + "\n";
		if (File.Exists(filePath)) {
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
		temp.transform.position = new Vector3(j, 0, -i);
	}

	void destroyBlockChildren () {
		foreach (Transform child in blockHolder.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
}
