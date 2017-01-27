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
	GameObject slider;
	GameObject sliderBackground;
	bool loading = false;
	int levelNum;
	float timer;
	bool sliderMoving = false;
	int sliderDirection = 1;

	void Start () {
		restartButton = GameObject.Find ("Restart Button");
		gameStatus = GameObject.Find ("Game Status");
		nextLevel = GameObject.Find ("Next Level");
		mainMenu = GameObject.Find ("Main Menu");
		slider = GameObject.Find ("Slider");
		sliderBackground = GameObject.Find ("Slider Background");
		nextLevel.GetComponent<Button> ().enabled = false;
		nextLevel.GetComponent<Button> ().image.color = new Color (1, 1, 1, 0);
		nextLevel.GetComponentInChildren<Text>().color = new Color (0, 0, 0, 0);
		restartButton.GetComponent<Button>().onClick.AddListener(delegate { restartButtonClick(); });
		nextLevel.GetComponent<Button>().onClick.AddListener(delegate { nextLevelClick(); });
		mainMenu.GetComponent<Button>().onClick.AddListener(delegate { mainMenuClick(); });
		slider.GetComponent<Slider> ().onValueChanged.AddListener (delegate { movingSlider();});
		levelNum = SceneManager.GetActiveScene ().buildIndex;
		gameStatus.GetComponent<Text>().text = (((levelNum - 1)/ 16) + 1) + "-" + (((levelNum - 1) % 16) + 1);
	}

	void Update () {
		if (sliderMoving) {
			sliderBackground.GetComponent<Image> ().color = new Color (0.75f, 0.75f, 0.75f, (slider.GetComponent<Slider> ().value * 0.5f)/1);
			if (!Input.GetMouseButton (0)) {
				timer += Time.deltaTime / 2;
				slider.GetComponent<Slider> ().value += timer * sliderDirection;
				if (slider.GetComponent<Slider> ().value == 0 || slider.GetComponent<Slider> ().value == 1) {
					sliderMoving = false;
				}
			}
		}
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
		timer = 0;
		sliderMoving = true;
		sliderDirection = 1;
		turnOnButtons ();
	}

	public void movingSlider () {
		if (Input.GetMouseButton (0)) {
			timer = 0;
			sliderMoving = true;
			if (slider.GetComponent<Slider> ().value < 0.5f) {
				sliderDirection = -1;
			} else if (slider.GetComponent<Slider> ().value > 0.5f) {
				sliderDirection = 1;
			}
			turnOnButtons ();
		}
	}

	void turnOnButtons () {
		if (mainMenu.GetComponent<Button> ().image.color.a == 0) {
			mainMenu.GetComponent<Button> ().image.color = new Color (1, 1, 1, 1);
			mainMenu.GetComponentInChildren<Text> ().color = new Color (1, 1, 1, 1);
			restartButton.GetComponent<Button> ().image.color = new Color (1, 1, 1, 1);
			restartButton.GetComponentInChildren<Text> ().color = new Color (1, 1, 1, 1);
		}
	}

	public bool isMenuOn () {
		if (slider.GetComponent<Slider> ().value > 0) {
			return true;
		} else {
			return false;
		}
	}
}
