using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

public class GameplayInterface : MonoBehaviour {

	private int speedOfSlider = 2;
	private float lengthOfTap = 0.2f;
	private int blurDownsample = 2;
	private int maxBlur = 10;

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
	float middleWidth, height;
	Vector3 bottomOfScreen, topOfScreen;
	float handleHeight;
	Vector3 towards;

	void Start () {
		restartButton = GameObject.Find ("Restart Button");
		gameStatus = GameObject.Find ("Game Status");
		nextLevel = GameObject.Find ("Next Level");
		mainMenu = GameObject.Find ("Main Menu");
		handle = GameObject.Find ("Handle");
		turnOffButtons ();
		levelNum = PlayerPrefs.GetInt ("Level", 0);
		gameStatus.GetComponent<Text> ().text = (((levelNum - 1) / 16) + 1) + "-" + (((levelNum - 1) % 16) + 1);
		middleWidth = Screen.width / 2;
		height = Screen.height;
		handleHeight = handle.transform.position.y;
		bottomOfScreen = new Vector3 (middleWidth, handleHeight, 0);
		topOfScreen = new Vector3 (middleWidth, height - handleHeight, 0);
		GetComponent<BlurOptimized> ().downsample = blurDownsample;
		handle.transform.position = new Vector3 (middleWidth, handleHeight, 0);
		GetComponent<BackgroundColorTransition> ().levelStarting ();
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			checkOnButtons ();
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

	void checkOnButtons () {
		Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		if (handle.GetComponent<BoxCollider2D> ().OverlapPoint (mousePos)) {
			holdingOnToSlider = true;
			sliderMoving = false;
			timer = 0;
			turnOnButtons ();
			GetComponent<BlurOptimized> ().enabled = true;
		} else if (restartButton.GetComponent<BoxCollider2D> ().OverlapPoint (mousePos)) {
			restartButtonClick ();
		} else if (mainMenu.GetComponent<BoxCollider2D> ().OverlapPoint (mousePos)) {
			mainMenuClick ();
		} else if (nextLevel.GetComponent<BoxCollider2D> ().OverlapPoint (mousePos)) {
			nextLevelClick ();
		}
	}

	void sliderMovingWithMouse () {
		timer += Time.deltaTime;
		handle.transform.position = new Vector3 (
			middleWidth,
			Mathf.Clamp (Input.mousePosition.y, handleHeight, height - handleHeight), 
			handle.transform.position.z
		);
		float blurSize = ((handle.transform.position.y - handleHeight) * maxBlur) / (height - handleHeight);
		GetComponent<BlurOptimized> ().blurSize = (blurSize);
	}

	void letGoOfSlider () {
		holdingOnToSlider = false;
		sliderMoving = true;
		if (handle.transform.position.y > Screen.height / 2) {
			towards = topOfScreen;
		} else {
			towards = bottomOfScreen;
		}
		if (timer < lengthOfTap) {
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
		Vector3 handlePos = handle.transform.position;
		handle.transform.position = Vector3.Lerp (handlePos, towards, timer);
		GetComponent<BlurOptimized> ().blurSize = (((handlePos.y - handleHeight) * maxBlur) / (height - handleHeight));
		if (handlePos.y < towards.y + handleHeight / 2 && handlePos.y > towards.y - handleHeight / 2) {
			handle.transform.position = towards;
			sliderMoving = false;
			timer = 0;
			if (towards == bottomOfScreen) {
				GetComponent<BlurOptimized> ().enabled = false;
			}
		}
	}

	public void restartButtonClick () {
		PlayerPrefs.SetInt ("Level", PlayerPrefs.GetInt ("Level", 0));
		gameObject.AddComponent<BackgroundColorTransition> ();
		GetComponent<BackgroundColorTransition> ().transition (PlayerPrefs.GetInt("Level", 0), "Restart");
	}

	public void nextLevelClick () {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		PlayerPrefs.SetInt ("Level", (PlayerPrefs.GetInt ("Level", 0) + 1));
		gameObject.AddComponent<BackgroundColorTransition> ();
		GetComponent<BackgroundColorTransition> ().transition (PlayerPrefs.GetInt ("Level", 0), "Next Level From Game");
	}

	public void mainMenuClick () {
		PlayerPrefs.SetInt ("Shift Camera", 0);
		PlayerPrefs.SetInt ("Level", 0);
		gameObject.AddComponent<BackgroundColorTransition> ();
		GetComponent<BackgroundColorTransition> ().transition (0, "Main Menu");
	}

	public void nextScene (int n) {
		if (loading == false) {
			SceneManager.LoadScene (n);
		}
		loading = true;
	}

	IEnumerator loadNewScene (int level) {
		yield return null;
		SceneManager.LoadScene (level);
	}

	public void winText () {
		GetComponent<BlurOptimized> ().enabled = true;
		if (PlayerPrefs.GetInt("Level", 0) + 1 <= 48) {
			nextLevel.GetComponent<Button> ().enabled = true;
			nextLevel.GetComponent<Button> ().image.color = Color.white;
			nextLevel.GetComponentInChildren<Text> ().color = Color.white;
			nextLevel.GetComponent<BoxCollider2D> ().size = Vector2.one * 64;
		}
		gameStatus.GetComponent<Text> ().text = "Success";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
		turnOnButtons ();
	}

	public void loseText () {
		GetComponent<BlurOptimized> ().enabled = true;
		gameStatus.GetComponent<Text> ().text = "Stuck!";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
		turnOnButtons ();
	}

	void turnOnButtons () {
		if (!mainMenu.GetComponent<Button> ().enabled) {
			mainMenu.GetComponent<Button> ().enabled = true;
			mainMenu.GetComponent<Button> ().image.color = Color.white;
			mainMenu.GetComponentInChildren<Text> ().color = Color.white;
			restartButton.GetComponent<Button> ().enabled = true;
			restartButton.GetComponent<Button> ().image.color = Color.white;
			restartButton.GetComponentInChildren<Text> ().color = Color.white;
		}
	}

	void turnOffButtons () {
		if (mainMenu.GetComponent<Button> ().enabled) {
			mainMenu.GetComponent<Button> ().enabled = false;
			mainMenu.GetComponent<Button> ().image.color = Color.clear;
			mainMenu.GetComponentInChildren<Text> ().color = Color.clear;
			restartButton.GetComponent<Button> ().enabled = false;
			restartButton.GetComponent<Button> ().image.color = Color.clear;
			restartButton.GetComponentInChildren<Text> ().color = Color.clear;
			nextLevel.GetComponent<Button> ().enabled = false;
			nextLevel.GetComponent<Button> ().image.color = Color.clear;
			nextLevel.GetComponentInChildren<Text> ().color = Color.clear;
			nextLevel.GetComponent<BoxCollider2D> ().size = Vector2.zero;
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
