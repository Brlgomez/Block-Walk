using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

public class GameplayInterface : MonoBehaviour {

	static int speedOfSlider = 2;
	static float lengthOfTap = 0.2f;
	static int blurDownsample = 2;
	static int maxBlur = 10;
	static int lastLevel = 64;

	GameObject restartButton;
	GameObject gameStatus;
	GameObject nextLevel;
	GameObject mainMenu;
	GameObject handle;
	bool loading = false;
	float timer;
	bool sliderMoving = false;
	bool holdingOnToSlider = false;
	float middleWidth, height;
	Vector3 bottomOfScreen, topOfScreen;
	float handleHeight;
	Vector3 towards;

	void Start() {
		restartButton = GameObject.Find("Restart Button");
		gameStatus = GameObject.Find("Game Status");
		nextLevel = GameObject.Find("Next Level");
		mainMenu = GameObject.Find("Main Menu");
		handle = GameObject.Find("Floor");
		gameStatus.GetComponent<Text>().text = PlayerPrefs.GetString(VariableManagement.userMapName);
		middleWidth = Screen.width / 2;
		height = Screen.height;
		handleHeight = handle.transform.position.y;
		bottomOfScreen = new Vector3(middleWidth, handleHeight, 0);
		topOfScreen = new Vector3(middleWidth, height - handleHeight, 0);
		GetComponent<BlurOptimized>().downsample = blurDownsample;
		GetComponent<BackgroundColorTransition>().levelStarting();
		if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.worldMenu) {
			if (PlayerPrefs.GetInt((GetComponent<VariableManagement>().getWorldLevel() - 1).ToString(), 0) == 0) {
				handle.GetComponentsInChildren<Image>()[4].color = Color.clear;
			}
		} else {
			if (!GetComponent<VariableManagement>().isLevelAuthorized()) {
				handle.GetComponentsInChildren<Image>()[4].color = Color.clear;
			}
		}
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			checkOnButtons();
		}
		if (holdingOnToSlider) {
			sliderMovingWithMouse();
		}
		if (Input.GetMouseButtonUp(0) && holdingOnToSlider) {
			letGoOfSlider();
		}
		if (sliderMoving) {
			sliderAutomaticallyMoving();
		}
	}

	void checkOnButtons() {
		if (GetComponent<BackgroundColorTransition>() == null) {
			Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if (handle.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
				holdingOnToSlider = true;
				sliderMoving = false;
				timer = 0;
				if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
					GetComponent<BlurOptimized>().enabled = true;
				}
			} else if (restartButton.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
				restartButtonClick();
			} else if (mainMenu.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
				mainMenuClick();
			} else if (nextLevel.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
				nextLevelClick();
			}
		}
	}

	void sliderMovingWithMouse() {
		timer += Time.deltaTime;
		handle.transform.position = new Vector3(
			middleWidth,
			Mathf.Clamp(Input.mousePosition.y, handleHeight, height - handleHeight), 
			handle.transform.position.z
		);
		float blurSize = ((handle.transform.position.y - handleHeight) * maxBlur) / (height - handleHeight);
		GetComponent<BlurOptimized>().blurSize = (blurSize);
	}

	void letGoOfSlider() {
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

	void sliderAutomaticallyMoving() {
		timer += Time.deltaTime * speedOfSlider;
		Vector3 handlePos = handle.transform.position;
		handle.transform.position = Vector3.Lerp(handlePos, towards, timer);
		GetComponent<BlurOptimized>().blurSize = (((handlePos.y - handleHeight) * maxBlur) / (height - handleHeight));
		if (handlePos.y < towards.y + handleHeight / 2 && handlePos.y > towards.y - handleHeight / 2) {
			handle.transform.position = towards;
			sliderMoving = false;
			timer = 0;
			if (towards == bottomOfScreen) {
				GetComponent<BlurOptimized>().enabled = false;
			}
		}
	}

	public void restartButtonClick() {
		sliderMoving = false;
		PlayerPrefs.SetInt(VariableManagement.worldLevel, GetComponent<VariableManagement>().getWorldLevel());
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.restartOrNextLevel);
	}

	public void nextLevelClick() {
		sliderMoving = false;
		GetComponent<VariableManagement>().turnOffCameraShift();
		PlayerPrefs.SetInt(VariableManagement.worldLevel, GetComponent<VariableManagement>().getWorldLevel() + 1);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.restartOrNextLevel);
	}

	public void mainMenuClick() {
		sliderMoving = false;
		if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.worldMenu ||
		    GetComponent<VariableManagement>().getLastMenu() == VariableManagement.userLevelMenu) {
			gameObject.AddComponent<BackgroundColorTransition>();
			GetComponent<BackgroundColorTransition>().transition(VariableManagement.toMainFromLevel);
		} else if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.editorMenu) {
			gameObject.AddComponent<BackgroundColorTransition>();
			GetComponent<BackgroundColorTransition>().transition(VariableManagement.toEditorFromTest);
		} else {
			gameObject.AddComponent<BackgroundColorTransition>();
			GetComponent<BackgroundColorTransition>().transition(VariableManagement.toMainFromLevel);
		}
	}

	public void nextScene(int n) {
		if (loading == false) {
			SceneManager.LoadScene(n);
		}
		loading = true;
	}

	IEnumerator loadNewScene(int level) {
		yield return null;
		SceneManager.LoadScene(level);
	}

	public void winText() {
		int currentLevel = GetComponent<VariableManagement>().getWorldLevel() - 1;
		int world = currentLevel / 16;
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			GetComponent<BlurOptimized>().enabled = true;
		}
		handle.GetComponentsInChildren<Image>()[4].color = Color.white;
		if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.worldMenu) {
			GetComponent<OnlineServices>().beatLevel(world);
			if (PlayerPrefs.GetInt((currentLevel).ToString(), 0) == 0) {
				PlayerPrefs.SetInt((currentLevel).ToString(), 1);
				bool beatWorld = true;
				for (int i = 0; i < 16; i++) {
					int levelNumber = (world * 16) + i;
					if (PlayerPrefs.GetInt(levelNumber.ToString(), 0) == 0) {
						beatWorld = false;
						break;
					}
				}
				if (beatWorld) {
					if (PlayerPrefs.GetInt("World" + world.ToString(), 0) == 0) {
						PlayerPrefs.SetInt("World" + world.ToString(), 1);
						PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, world + 1);
					}
				}
				PlayerPrefs.Save();
			}
			if (GetComponent<VariableManagement>().getWorldLevel() + 1 <= lastLevel && ((currentLevel + 1) % 16) != 0) { 
				nextLevel.GetComponent<Button>().enabled = true;
				nextLevel.GetComponent<Button>().image.color = Color.white;
				nextLevel.GetComponent<BoxCollider2D>().enabled = true;	
			}
		} else if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.userLevelMenu || GetComponent<VariableManagement>().getLastMenu() == VariableManagement.editorMenu) {
			GetComponent<VariableManagement>().setLevelAuthorization(1);
		}
		gameStatus.GetComponent<Text>().text = "Success";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
	}

	public void loseText() {
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			GetComponent<BlurOptimized>().enabled = true;
		}
		gameStatus.GetComponent<Text>().text = "Stuck!";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
	}

	public bool isMenuOn() {
		if (holdingOnToSlider || towards == topOfScreen || sliderMoving) {
			return true;
		} else {
			return false;
		}
	}
}
