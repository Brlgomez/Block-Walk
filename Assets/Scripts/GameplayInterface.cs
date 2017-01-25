using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameplayInterface : MonoBehaviour {

	GameObject restartButton;
	GameObject gameStatus;
	GameObject nextLevel;
	GameObject mainMenu;
	bool loading = false;
	int levelNum;

	void Start () {
		restartButton = GameObject.Find ("Restart Button");
		gameStatus = GameObject.Find ("Game Status");
		nextLevel = GameObject.Find ("Next Level");
		mainMenu = GameObject.Find ("Main Menu");
		nextLevel.GetComponent<Button> ().enabled = false;
		nextLevel.GetComponent<Button> ().image.color = new Color (1, 1, 1, 0);
		nextLevel.GetComponentInChildren<Text>().color = new Color (0, 0, 0, 0);
		restartButton.GetComponent<Button>().onClick.AddListener(delegate { restartButtonClick(); });
		nextLevel.GetComponent<Button>().onClick.AddListener(delegate { nextLevelClick(); });
		mainMenu.GetComponent<Button>().onClick.AddListener(delegate { mainMenuClick(); });
		levelNum = SceneManager.GetActiveScene ().buildIndex;
		gameStatus.GetComponent<Text>().text = (((levelNum - 1)/ 16) + 1) + "-" + (((levelNum - 1) % 16) + 1);
	}

	public void restartButtonClick () {
		PlayerPrefs.SetInt ("Shift Camera", 1);
		GetComponent<BackgroundColorTransition> ().transition (levelNum, "Restart");
	}

	public void nextLevelClick () {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		GetComponent<BackgroundColorTransition> ().transition (levelNum + 1, "Next Level From Game");
	}

	public void mainMenuClick () {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		GetComponent<BackgroundColorTransition> ().transition (0, "Main Menu");
	}

	public void nextScene (int n) {
		if (loading == false) {
			SceneManager.LoadScene(n);
		}
		loading = true;
	}

	IEnumerator loadNewScene (int level) {
		yield return null;
		SceneManager.LoadScene(level);
	}

	public void winText () {
		if (SceneManager.sceneCountInBuildSettings > levelNum + 1) {
			nextLevel.GetComponent<Button> ().enabled = true;
			nextLevel.GetComponent<Button> ().image.color = new Color (1, 1, 1, 1);
			nextLevel.GetComponentInChildren<Text> ().color = new Color (1, 1, 1, 1);
		}
		gameStatus.GetComponent<Text>().text = "You Win";
	}
}
