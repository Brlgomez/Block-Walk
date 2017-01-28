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
	int sliderDirection = 1;
	bool holdingOnToSlider = false;
	float middleWidth;
	float height;
	Vector3 bottomOfScreen, topOfScreen;

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
		bottomOfScreen = new Vector3 (middleWidth, 20, 0);
		topOfScreen = new Vector3 (middleWidth, height - 20, 0);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if (handle.GetComponent<BoxCollider2D> ().OverlapPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y))) {
				holdingOnToSlider = true;
				sliderMoving = false;
				timer = 0;
				turnOnButtons ();
				GetComponent<BlurOptimized> ().enabled = true;
			}
		}
		if (holdingOnToSlider) {
			timer += Time.deltaTime;
			handle.transform.position = new Vector3 (middleWidth, Mathf.Clamp(Input.mousePosition.y, 20, height - 20), handle.transform.position.z);
			GetComponent<BlurOptimized> ().blurSize = (((handle.transform.position.y - 20) * 3.5f) / height);
		}
		if (Input.GetMouseButtonUp (0) && holdingOnToSlider) {
			holdingOnToSlider = false;
			sliderMoving = true;
			if (handle.transform.position.y > Screen.height / 2) {
				sliderDirection = 1;
			} else {
				sliderDirection = -1;
			}
			if (timer < 0.2f) {
				sliderDirection *= -1;
			}
			timer = 0;
		}
		if (sliderMoving) {
			timer += Time.deltaTime;
			if (sliderDirection == 1) {
				handle.transform.position = Vector3.Lerp (handle.transform.position, topOfScreen, timer);
			} else {
				handle.transform.position = Vector3.Lerp (handle.transform.position, bottomOfScreen, timer);
				if (GetComponent<BlurOptimized> ().blurSize <= 0) {
					GetComponent<BlurOptimized> ().enabled = false;
				}
			}
			GetComponent<BlurOptimized> ().blurSize = (((handle.transform.position.y - 20) * 3.5f) / height);
			if (timer > 1f) {
				sliderMoving = false;
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
		GetComponent<BlurOptimized> ().enabled = true;
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

	void turnOnButtons () {
		if (mainMenu.GetComponent<Button> ().image.color.a == 0) {
			mainMenu.GetComponent<Button> ().image.color = new Color (1, 1, 1, 1);
			mainMenu.GetComponentInChildren<Text> ().color = new Color (1, 1, 1, 1);
			restartButton.GetComponent<Button> ().image.color = new Color (1, 1, 1, 1);
			restartButton.GetComponentInChildren<Text> ().color = new Color (1, 1, 1, 1);
		}
	}

	public bool isMenuOn () {
		if (holdingOnToSlider || handle.transform.position.y > 21) {
			return true;
		} else {
			return false;
		}
	}
}
