using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

public class GameplayInterface : MonoBehaviour {

	GameObject restartButton;
	GameObject gameStatus;
	GameObject nextLevel;
	GameObject mainMenu;
	GameObject handle;
	bool loading = false;
	int levelNum;
	float timer;
	bool sliderMoving = false;
	bool holdingOnToSlider = false;
	float middleWidth;
	float height;
	Vector3 bottomOfScreen, topOfScreen;
	float handleHeight;
	Vector3 towards;
	private int speedOfSlider = 2;

	void Start () {
		restartButton = GameObject.Find ("Restart Button");
		gameStatus = GameObject.Find ("Game Status");
		nextLevel = GameObject.Find ("Next Level");
		mainMenu = GameObject.Find ("Main Menu");
		handle = GameObject.Find ("Handle");
		nextLevel.GetComponent<Button> ().enabled = false;
		nextLevel.GetComponent<Button> ().image.color = new Color (1, 1, 1, 0);
		nextLevel.GetComponentInChildren<Text>().color = new Color (0, 0, 0, 0);
		restartButton.GetComponent<Button>().onClick.AddListener(delegate { restartButtonClick(); });
		nextLevel.GetComponent<Button>().onClick.AddListener(delegate { nextLevelClick(); });
		mainMenu.GetComponent<Button>().onClick.AddListener(delegate { mainMenuClick(); });
		levelNum = SceneManager.GetActiveScene ().buildIndex;
		gameStatus.GetComponent<Text>().text = (((levelNum - 1)/ 16) + 1) + "-" + (((levelNum - 1) % 16) + 1);
		middleWidth = Screen.width / 2;
		height = Screen.height;
		handleHeight = Screen.dpi / 6;
		bottomOfScreen = new Vector3 (middleWidth, handleHeight, 0);
		topOfScreen = new Vector3 (middleWidth, height - handleHeight, 0);
		GetComponent<BlurOptimized> ().downsample = 2;
		handle.transform.position = new Vector3 (middleWidth, handleHeight, 0);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			checkOnSlider ();
		}
		if (holdingOnToSlider) {
			sliderMovingWithMouse ();
		}
		if (Input.GetMouseButtonUp (0) && holdingOnToSlider) {
			letGoOfSlider ();
		}
		if (sliderMoving) {
			sliderAutomaticallyMoving ();
		}
	}

	void checkOnSlider () {
		if (handle.GetComponent<BoxCollider2D> ().OverlapPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y))) {
			holdingOnToSlider = true;
			sliderMoving = false;
			timer = 0;
			turnOnButtons ();
			GetComponent<BlurOptimized> ().enabled = true;
		}
	}

	void sliderMovingWithMouse () {
		timer += Time.deltaTime;
		handle.transform.position = new Vector3 (middleWidth, Mathf.Clamp(Input.mousePosition.y, handleHeight, height - handleHeight), handle.transform.position.z);
		GetComponent<BlurOptimized> ().blurSize = (((handle.transform.position.y - handleHeight) * 10) / (height - handleHeight));
	}

	void letGoOfSlider () {
		holdingOnToSlider = false;
		sliderMoving = true;
		if (handle.transform.position.y > Screen.height / 2) {
			towards = topOfScreen;
		} else {
			towards = bottomOfScreen;
		}
		if (timer < 0.2f) {
			if (towards == bottomOfScreen) {
				towards = topOfScreen;
			} else {
				towards = bottomOfScreen;
			}
		}
		timer = 0;
	}

	void sliderAutomaticallyMoving () {
		timer += Time.deltaTime * speedOfSlider;
		handle.transform.position = Vector3.Lerp (handle.transform.position, towards, timer);
		GetComponent<BlurOptimized> ().blurSize = (((handle.transform.position.y - handleHeight) * 10) / (height - handleHeight));
		if (Mathf.Abs(handle.transform.position.y - towards.y) < handleHeight && towards == bottomOfScreen) {
			GetComponent<BlurOptimized> ().enabled = false;
		}
		if (handle.transform.position == towards) {
			sliderMoving = false;
			timer = 0;
		}
	}

	public void restartButtonClick () {
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
		GetComponent<BlurOptimized> ().enabled = true;
		if (SceneManager.sceneCountInBuildSettings > levelNum + 1) {
			nextLevel.GetComponent<Button> ().enabled = true;
			nextLevel.GetComponent<Button> ().image.color = new Color (1, 1, 1, 1);
			nextLevel.GetComponentInChildren<Text> ().color = new Color (1, 1, 1, 1);
		}
		gameStatus.GetComponent<Text>().text = "Success";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
		turnOnButtons ();
	}

	public void loseText () {
		GetComponent<BlurOptimized> ().enabled = true;
		gameStatus.GetComponent<Text>().text = "Game Over";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
		turnOnButtons ();
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
		if (holdingOnToSlider || towards == topOfScreen || sliderMoving) {
			return true;
		} else {
			return false;
		}
	}
}
