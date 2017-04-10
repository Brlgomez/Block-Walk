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
	static int lastLevel = 80;

	GameObject restartButton;
	GameObject gameStatus;
	GameObject nextLevel;
	GameObject mainMenu;
	GameObject handle;
	GameObject authorized;
	GameObject particles;
	GameObject instructions;

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
		authorized = GameObject.Find("Authorized");
		particles = GameObject.Find("Particles");
		instructions = GameObject.Find("Instruction Particles");
		gameStatus.GetComponent<Text>().text = PlayerPrefs.GetString(VariableManagement.userMapName);
		middleWidth = Screen.width / 2;
		height = Screen.height;
		handleHeight = handle.transform.position.y;
		bottomOfScreen = new Vector3(middleWidth, handleHeight, 0);
		topOfScreen = new Vector3(middleWidth, height - handleHeight, 0);
		GetComponent<BlurOptimized>().downsample = blurDownsample;
		authorized.transform.SetParent(handle.transform);
		gameStatus.transform.SetParent(handle.transform);
		GetComponent<BackgroundColorTransition>().levelStarting();
		if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.worldMenu) {
			if (PlayerPrefs.GetInt((GetComponent<VariableManagement>().getWorldLevel() - 1).ToString(), 0) == 0) {
				authorized.GetComponent<Image>().color = Color.clear;
			}
		} else {
			if (!GetComponent<VariableManagement>().isLevelAuthorized()) {
				authorized.GetComponent<Image>().color = Color.clear;
			}
		}
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			particles.GetComponent<ParticleSystem>().Play();
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
		float handleRotation = 180 * ((handle.transform.position.y - handleHeight) / (height - handleHeight * 2));
		handle.GetComponentInChildren<Image>().transform.eulerAngles = new Vector3(0, 0, handleRotation);
		float blurSize = maxBlur * ((handle.transform.position.y - handleHeight) / (height - handleHeight * 2));
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
			GetComponent<SoundsAndMusic>().playButtonSound();
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
		float handleRotation = 180 * ((handlePos.y - handleHeight) / (height - handleHeight * 2));
		handle.GetComponentInChildren<Image>().transform.eulerAngles = new Vector3(0, 0, handleRotation);
		GetComponent<BlurOptimized>().blurSize = maxBlur * ((handlePos.y - handleHeight) / (height - handleHeight * 2));
		if (handlePos.y < towards.y + handleHeight / 2 && handlePos.y > towards.y - handleHeight / 2) {
			handle.transform.position = towards;
			sliderMoving = false;
			timer = 0;
			if (towards == bottomOfScreen) {
				GetComponent<BlurOptimized>().enabled = false;
				handle.GetComponentInChildren<Image>().transform.eulerAngles = new Vector3(0, 0, 0);
			} else {
				handle.GetComponentInChildren<Image>().transform.eulerAngles = new Vector3(0, 0, 180);
			}
		}
	}

	public void restartButtonClick() {
		sliderMoving = false;
		PlayerPrefs.SetInt(VariableManagement.worldLevel, GetComponent<VariableManagement>().getWorldLevel());
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.restartOrNextLevel);
		GetComponent<SoundsAndMusic>().playButtonSound();
	}

	public void nextLevelClick() {
		sliderMoving = false;
		GetComponent<VariableManagement>().turnOffCameraShift();
		PlayerPrefs.SetInt(VariableManagement.worldLevel, GetComponent<VariableManagement>().getWorldLevel() + 1);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.restartOrNextLevel);
		GetComponent<SoundsAndMusic>().playButtonSound();
	}

	public void mainMenuClick() {
		sliderMoving = false;
		GetComponent<SoundsAndMusic>().playButtonSound();
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
		if (PlayerPrefs.GetString(VariableManagement.lastMenu) == VariableManagement.worldMenu) {
			if (GetComponent<VariableManagement>().getWorldLevel() == 1) {
				instructions.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
				instructions.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
				instructions.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
				instructions.transform.GetChild(3).GetComponent<ParticleSystem>().Stop();
			}
		}
		authorized.GetComponent<Image>().color = Color.white;
		if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.worldMenu) {
			PlayerPrefs.SetInt((currentLevel).ToString(), 1);
			GetComponent<OnlineServices>().beatLevel(world);
			bool beatWorld = true;
			for (int i = 0; i < 16; i++) {
				int levelNumber = (world * 16) + i;
				if (PlayerPrefs.GetInt(levelNumber.ToString(), 0) == 0) {
					beatWorld = false;
					break;
				}
			}
			if (beatWorld) {
				PlayerPrefs.SetInt("Beat World" + world.ToString(), 1);
				if (PlayerPrefs.GetInt("World" + world.ToString(), 0) == 0) {
					PlayerPrefs.SetInt("World" + world.ToString(), 1);
					PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, world + 1);
				}
			}
			PlayerPrefs.Save();
			if (GetComponent<VariableManagement>().getWorldLevel() + 1 <= lastLevel && ((currentLevel + 1) % 16) != 0) { 
				nextLevel.GetComponent<Button>().enabled = true;
				nextLevel.GetComponent<Button>().image.color = Color.white;
				nextLevel.GetComponent<BoxCollider2D>().enabled = true;	
			}
		} else if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.userLevelMenu || GetComponent<VariableManagement>().getLastMenu() == VariableManagement.editorMenu) {
			GetComponent<VariableManagement>().setLevelAuthorization(1);
		}
		GetComponent<SoundsAndMusic>().playBeatLevelSound();
		gameStatus.GetComponent<Text>().text = "Success!";
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
	}

	public void loseText(string text) {
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			GetComponent<BlurOptimized>().enabled = true;
		}
		GetComponent<SoundsAndMusic>().playLoseLevelSound();
		gameStatus.GetComponent<Text>().text = text;
		timer = 0;
		sliderMoving = true;
		towards = topOfScreen;
	}

	public void destroyed () {
		StartCoroutine(loseByDestroyed());
	}

	IEnumerator loseByDestroyed () {
		yield return new WaitForSeconds(1);
		loseText("Destroyed!");
	}

	public bool isMenuOn() {
		if (holdingOnToSlider || towards == topOfScreen || sliderMoving) {
			return true;
		} else {
			return false;
		}
	}
}
