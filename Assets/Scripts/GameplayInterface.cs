﻿using UnityEngine;
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
	static int lastLevel = 48;

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

	void Start() {
		restartButton = GameObject.Find("Restart Button");
		gameStatus = GameObject.Find("Game Status");
		nextLevel = GameObject.Find("Next Level");
		mainMenu = GameObject.Find("Main Menu");
		handle = GameObject.Find("Floor");
		if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.worldMenu) {
			levelNum = GetComponent<VariableManagement>().getWorldLevel();
			gameStatus.GetComponent<Text>().text = (((levelNum - 1) / 16) + 1) + "-" + (((levelNum - 1) % 16) + 1);
		} else {
			levelNum = GetComponent<VariableManagement>().getUserLevel();
			gameStatus.GetComponent<Text>().text = levelNum.ToString();
		}
		middleWidth = Screen.width / 2;
		height = Screen.height;
		handleHeight = handle.transform.position.y;
		bottomOfScreen = new Vector3(middleWidth, handleHeight, 0);
		topOfScreen = new Vector3(middleWidth, height - handleHeight, 0);
		GetComponent<BlurOptimized>().downsample = blurDownsample;
		GetComponent<BackgroundColorTransition>().levelStarting();
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
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		if (handle.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
			holdingOnToSlider = true;
			sliderMoving = false;
			timer = 0;
			GetComponent<BlurOptimized>().enabled = true;
		} else if (restartButton.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
			restartButtonClick();
		} else if (mainMenu.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
			mainMenuClick();
		} else if (nextLevel.GetComponent<BoxCollider2D>().OverlapPoint(mousePos)) {
			nextLevelClick();
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
		PlayerPrefs.SetInt(VariableManagement.worldLevel, GetComponent<VariableManagement>().getWorldLevel());
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.restartOrNextLevel);
	}

	public void nextLevelClick() {
		GetComponent<VariableManagement>().turnOffCameraShift();
		PlayerPrefs.SetInt(VariableManagement.worldLevel, GetComponent<VariableManagement>().getWorldLevel() + 1);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.restartOrNextLevel);
	}

	public void mainMenuClick() {
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
		if (PlayerPrefs.GetInt((currentLevel).ToString(), 0) == 0) {
			PlayerPrefs.SetInt((currentLevel).ToString(), 1);
			int world = currentLevel / 16;
			bool beatWorld = true;
			for (int i = 0; i < 16; i++) {
				int levelNumber = (world * 16) + i;
				if (PlayerPrefs.GetInt(levelNumber.ToString(), 0) == 0) {
					beatWorld = false;
					break;
				}
			}
			if (beatWorld) {
				PlayerPrefs.SetInt("World" + world.ToString(), 1);
				PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, world + 1);
			}
			PlayerPrefs.Save();
		}
		GetComponent<BlurOptimized>().enabled = true;
		if (GetComponent<VariableManagement>().getWorldLevel() + 1 <= lastLevel &&
		    GetComponent<VariableManagement>().getLastMenu() == VariableManagement.worldMenu) {
			if ((currentLevel + 1)%16 == 0 && PlayerPrefs.GetInt("World" + ((currentLevel/16).ToString()), 0) == 1) {
				nextLevel.GetComponent<Button>().enabled = true;
				nextLevel.GetComponent<Button>().image.color = Color.white;
				nextLevel.GetComponentInChildren<Text>().color = Color.black;
				nextLevel.GetComponent<BoxCollider2D>().enabled = true;
			} else if ((currentLevel + 1)%16 != 0) {
				nextLevel.GetComponent<Button>().enabled = true;
				nextLevel.GetComponent<Button>().image.color = Color.white;
				nextLevel.GetComponentInChildren<Text>().color = Color.black;
				nextLevel.GetComponent<BoxCollider2D>().enabled = true;
			}
		}
		gameStatus.GetComponent<Text>().text = "Success";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
	}

	public void loseText() {
		GetComponent<BlurOptimized>().enabled = true;
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
