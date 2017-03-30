using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class EditorInterface : MonoBehaviour {

	static int maxBlurSize = 10;
	static int blurDownsample = 2;
	static int minOrtho = 8;
	static int maxOrtho = 15;
	static int transitionLength = 1;
	static float timeSpeed = 1.5f;
	static float blockAlpha = 0.75f;

	GameObject cubes, menuHolder, colorHolder, optionHolder, content, popUp, uiHolder, highlight;
	GameObject r, g, b;
	GameObject rB, gB, bB;
	GameObject rHandle, gHandle, bHandle;
	GameObject changeColors, options, scroll, authorized;
	Vector2 rGradient, gGradient, bGradient;
	bool touchingRhandle, touchingGhandle, touchingBhandle, changingColor = false;

	float colorTimerLimit = 0.05f;
	float colorTimer = 1;
	float gradientSize = 25;
	bool menuOn = true;
	private string filePath;
	bool transition;
	int transitionNum = 0;
	float deltaTime;
	Vector3 initialCamPos;
	Vector3 colorMenuCamPos;
	bool deauthorize = false;
	bool movingSlider = false;
	int worldNumber = 0;

	void Start() {
		filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
		cubes = GameObject.Find("Cubes");
		menuHolder = GameObject.Find("Menu Holder");
		optionHolder = GameObject.Find("Option Holder");
		highlight = GameObject.Find("Block Highlight");
		content = GameObject.Find("Content");
		popUp = GameObject.Find("Pop Up");
		uiHolder = GameObject.Find("Floor");
		changeColors = GameObject.Find("Change Colors");
		options = GameObject.Find("Options");
		scroll = GameObject.Find("Scroll View");
		authorized = GameObject.Find("Authorized");
		changeColors.transform.SetParent(menuHolder.transform);
		options.transform.SetParent(menuHolder.transform);
		scroll.transform.SetParent(menuHolder.transform);
		authorized.transform.SetParent(menuHolder.transform);
		GetComponent<BlurOptimized>().downsample = blurDownsample;
		GetComponent<BackgroundColorTransition>().levelStarting();
		initialCamPos = transform.position;
		colorMenuCamPos = new Vector3(transform.position.x, transform.position.y, 5.25f);
		if (!GetComponent<VariableManagement>().isLevelAuthorized()) {
			uiHolder.GetComponentsInChildren<Image>()[1].color = Color.clear;
		}
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 1) {
			colorTimerLimit = 0.1f;
		}
		showButtons();
	}

	void showButtons () {
		content.GetComponentsInChildren<Button>()[2].onClick.RemoveAllListeners();
		content.GetComponentsInChildren<Button>()[3].onClick.RemoveAllListeners();
		content.GetComponentsInChildren<Button>()[4].onClick.RemoveAllListeners();
		content.GetComponentsInChildren<Button>()[5].onClick.RemoveAllListeners();
		content.GetComponentsInChildren<Button>()[6].onClick.RemoveAllListeners();
		content.GetComponentsInChildren<Button>()[7].onClick.RemoveAllListeners();
		content.GetComponentsInChildren<Button>()[8].onClick.RemoveAllListeners();
		if (PlayerPrefs.GetInt(VariableManagement.world0, 0) == 1) {
			turnOnButton(content.GetComponentsInChildren<Button>()[2], GameObject.Find("Multistep Block"));
		} else {
			turnOffButton(content.GetComponentsInChildren<Button>()[2], 0);
		}
		if (PlayerPrefs.GetInt(VariableManagement.world1, 0) == 1) {
			turnOnButton(content.GetComponentsInChildren<Button>()[3], GameObject.Find("Switch Block"));
			turnOnButton(content.GetComponentsInChildren<Button>()[4], GameObject.Find("Red Block"));
			turnOnButton(content.GetComponentsInChildren<Button>()[5], GameObject.Find("Blue Block"));
		} else {
			turnOffButton(content.GetComponentsInChildren<Button>()[3], 1);
			turnOffButton(content.GetComponentsInChildren<Button>()[4], 1);
			turnOffButton(content.GetComponentsInChildren<Button>()[5], 1);
		}
		if (PlayerPrefs.GetInt(VariableManagement.world2, 0) == 1) {
			turnOnButton(content.GetComponentsInChildren<Button>()[6], GameObject.Find("Rotate Block R"));
			turnOnButton(content.GetComponentsInChildren<Button>()[7], GameObject.Find("Rotate Block L"));
		} else {
			turnOffButton(content.GetComponentsInChildren<Button>()[6], 2);
			turnOffButton(content.GetComponentsInChildren<Button>()[7], 2);
		}
		if (PlayerPrefs.GetInt(VariableManagement.world3, 0) == 1) {
			turnOnButton(content.GetComponentsInChildren<Button>()[8], GameObject.Find("Bomb Block"));
		} else {
			turnOffButton(content.GetComponentsInChildren<Button>()[8], 3);
		}
	}

	public void setVariables(float sR, float sG, float sB, float sRInc, float sGInc, float sBInc, float sRInc2, float sGInc2, float sBInc2, string title) {
		optionHolder = GameObject.Find("Option Holder");
		if (title != "Untitled") {
			optionHolder.GetComponentInChildren<InputField>().text = title;
		}
		r = GameObject.Find("R");
		g = GameObject.Find("G");
		b = GameObject.Find("B");
		rB = GameObject.Find("Block R");
		gB = GameObject.Find("Block G");
		bB = GameObject.Find("Block B");
		rHandle = GameObject.Find("R Area");
		gHandle = GameObject.Find("G Area");
		bHandle = GameObject.Find("B Area");
		colorHolder = GameObject.Find("Color Holder");
		Vector3[] corners = new Vector3[4];
		rHandle.GetComponent<RectTransform>().GetWorldCorners(corners);
		gradientSize = (corners[2].y - corners[0].y) * 0.38f;
		r.GetComponent<Slider>().value = (Camera.main.backgroundColor.r * 255);
		g.GetComponent<Slider>().value = (Camera.main.backgroundColor.g * 255);
		b.GetComponent<Slider>().value = (Camera.main.backgroundColor.b * 255);
		rB.GetComponent<Slider>().value = sR;
		gB.GetComponent<Slider>().value = sG;
		bB.GetComponent<Slider>().value = sB;
		rGradient = gradientHandlePosition(
			rHandle.transform.position.x + sRInc * (gradientSize * 10), 
			rHandle.transform.position.y + sRInc2 * (gradientSize * 10), 
			rHandle
		);
		gGradient = gradientHandlePosition(
			gHandle.transform.position.x + sGInc * (gradientSize * 10), 
			gHandle.transform.position.y + sGInc2 * (gradientSize * 10), 
			gHandle
		);
		bGradient = gradientHandlePosition(
			bHandle.transform.position.x + sBInc * (gradientSize * 10), 
			bHandle.transform.position.y + sBInc2 * (gradientSize * 10), 
			bHandle
		);
		colorHolder.transform.localScale = Vector3.zero;
		r.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBackgroundColor(); });
		g.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBackgroundColor();	});
		b.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBackgroundColor(); });
		rB.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBackgroundColor(); });
		gB.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBackgroundColor(); });
		bB.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBackgroundColor(); });
		changeBackgroundColor();
		showMain();
	}

	void Update() {
		if (transition) { 
			transitionInterface();
		} else {
			if (Input.GetMouseButtonDown(0) && cubes.transform.position == Vector3.zero && transitionNum == 0) {
				GetComponent<LevelEditor>().mouseDown();
			}
			if (Input.GetMouseButtonUp(0)) {
				if (changingColor) {
					colorTimer = colorTimerLimit;
					changeBackgroundColor();
				}
				movingSlider = false;
				touchingRhandle = false;
				touchingGhandle = false;
				touchingBhandle = false;
				changingColor = false;
				colorTimer = colorTimerLimit;
				GetComponent<LevelEditor>().mouseUp();
			}
			if (GetComponent<LevelEditor>().getMouseDrag() && !movingSlider) {
				GetComponent<LevelEditor>().mouseDrag();
			}
			if (Input.GetMouseButton(0) && transitionNum == 2) {
				Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				if (rHandle.GetComponentsInChildren<Image>()[1].GetComponent<BoxCollider2D>().OverlapPoint(mousePos) && !touchingGhandle && !touchingBhandle) {
					touchingRhandle = true;
				} else if (gHandle.GetComponentsInChildren<Image>()[1].GetComponent<BoxCollider2D>().OverlapPoint(mousePos) && !touchingRhandle && !touchingBhandle) {
					touchingGhandle = true;
				} else if (bHandle.GetComponentsInChildren<Image>()[1].GetComponent<BoxCollider2D>().OverlapPoint(mousePos) && !touchingRhandle && !touchingGhandle) {
					touchingBhandle = true;
				}
				if (touchingRhandle) {
					rGradient = gradientHandlePosition(Input.mousePosition.x, Input.mousePosition.y, rHandle);
				} else if (touchingGhandle) {
					gGradient = gradientHandlePosition(Input.mousePosition.x, Input.mousePosition.y, gHandle);
				} else if (touchingBhandle) {
					bGradient = gradientHandlePosition(Input.mousePosition.x, Input.mousePosition.y, bHandle);
				}
				if (touchingBhandle || touchingRhandle || touchingGhandle) {
					changeBackgroundColor();
				}
			}
		}
	}

	void transitionInterface() {
		deltaTime += Time.deltaTime * timeSpeed;
		if (deltaTime > transitionLength) {
			GetComponent<LevelEditor>().mouseUp();
			transition = false;
			deltaTime = 0;
			if (transitionNum == 0) {
				Camera.main.orthographicSize = minOrtho;
			}
		}
		if (transitionNum == 0) {
			if (deltaTime > (transitionLength * 0.33f)) {
				GetComponent<BlurOptimized>().enabled = false;
			}
			menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.one, deltaTime);
			colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
			optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
			popUp.transform.localScale = Vector3.Slerp(popUp.transform.localScale, new Vector3(1,0,1), deltaTime);
			transform.position = Vector3.Lerp(transform.position, initialCamPos, deltaTime);
			GetComponent<BlurOptimized>().blurSize = (optionHolder.transform.localScale.y + popUp.transform.localScale.y) * maxBlurSize;
			Camera.main.orthographicSize -= deltaTime;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minOrtho, maxOrtho);
			uiHolder.GetComponent<Image>().color = Color.Lerp(uiHolder.GetComponent<Image>().color, new Color (1, 1, 1, 0.25f), deltaTime/2);
			if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 1) {
				menuHolder.transform.localScale = Vector3.one;
				colorHolder.transform.localScale = Vector3.zero;
				optionHolder.transform.localScale = Vector3.zero;
				popUp.transform.localScale = new Vector3(1,0,1);
				transform.position = initialCamPos;
				Camera.main.orthographicSize = minOrtho;
				transition = false;
				deltaTime = 0;
				GetComponent<LevelEditor>().mouseUp();
				uiHolder.GetComponent<Image>().color = new Color (1, 1, 1, 0.25f);
			}
		} else if (transitionNum == 1) {
			menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, new Vector3(1,0,1), deltaTime);
			colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
			optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.one, deltaTime);
			transform.position = Vector3.Lerp(transform.position, initialCamPos, deltaTime);
			GetComponent<BlurOptimized>().blurSize = optionHolder.transform.localScale.y * maxBlurSize;
			Camera.main.orthographicSize -= deltaTime;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minOrtho, maxOrtho);
			uiHolder.GetComponent<Image>().color = Color.Lerp(uiHolder.GetComponent<Image>().color, Color.clear, deltaTime/2);
			if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 1) {
				menuHolder.transform.localScale = new Vector3(1,0,1);
				colorHolder.transform.localScale = Vector3.zero;
				optionHolder.transform.localScale = Vector3.one;
				transform.position = initialCamPos;
				Camera.main.orthographicSize = minOrtho;
				transition = false;
				deltaTime = 0;
				GetComponent<LevelEditor>().mouseUp();
				uiHolder.GetComponent<Image>().color = Color.clear;
			}
		} else if (transitionNum == 2) {
			menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, new Vector3(1,0,1), deltaTime);
			colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.one, deltaTime);
			optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
			transform.position = Vector3.Lerp(transform.position, colorMenuCamPos, deltaTime);
			Camera.main.orthographicSize += deltaTime;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minOrtho, maxOrtho);
			uiHolder.GetComponent<Image>().color = Color.Lerp(uiHolder.GetComponent<Image>().color, Color.clear, deltaTime/2);
			if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 1) {
				menuHolder.transform.localScale = new Vector3(1,0,1);
				colorHolder.transform.localScale = Vector3.one;
				optionHolder.transform.localScale = Vector3.zero;
				transform.position = colorMenuCamPos;
				Camera.main.orthographicSize = maxOrtho;
				transition = false;
				deltaTime = 0;
				GetComponent<LevelEditor>().mouseUp();
				uiHolder.GetComponent<Image>().color = Color.clear;
			}
		} else if (transitionNum == 3) {
			menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, new Vector3(1,0,1), deltaTime);
			popUp.transform.localScale = Vector3.Slerp(popUp.transform.localScale, Vector3.one, deltaTime);
			uiHolder.GetComponent<Image>().color = Color.Lerp(uiHolder.GetComponent<Image>().color, Color.clear, deltaTime/2);
			GetComponent<BlurOptimized>().blurSize = popUp.transform.localScale.y * maxBlurSize;
			if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 1) {
				menuHolder.transform.localScale = new Vector3(1,0,1);
				popUp.transform.localScale = Vector3.one;
				transition = false;
				deltaTime = 0;
				GetComponent<LevelEditor>().mouseUp();
				uiHolder.GetComponent<Image>().color = Color.clear;
			}
		}
	}

	/* -------------------------------------------------- Main Screen ----------------------------------------------- */

	public void showMain() {
		optionHolder.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = "Save";
		menuOn = true;
		transition = true;
		transitionNum = 0;
		deltaTime = 0;
	}

	public void showColorMenu() {
		menuOn = false;
		transition = true;
		transitionNum = 2;
		deltaTime = 0;
	}

	public void showOptionMenu() {
		menuOn = false;
		transition = true;
		transitionNum = 1;
		deltaTime = 0;
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			GetComponent<BlurOptimized>().enabled = true;
		}
	}

	void turnOnButton (Button b, GameObject block) {
		b.onClick.AddListener(() => {
			GetComponent<LevelEditor>().changeBlock(block);
		});
		b.onClick.AddListener(() => {
			shiftHighlight(b.gameObject);
		});
		b.GetComponent<Image>().color = Color.white;
	}

	void turnOffButton (Button b, int world) {
		b.onClick.AddListener(() => {
			setWorldNumber(world);
		});
		b.onClick.AddListener(() => {
			showPopUp();
		});
		b.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
	}

	void turnOffRegularButton (Button b) {
		if (b.GetComponentInChildren<Text>() != null) {
			b.GetComponentInChildren<Text>().color = Color.clear;
		}
		b.GetComponent<Image>().color = Color.clear;
		b.interactable = false;
	}

	void turnOnRegularButton (Button b) {
		if (b.GetComponentInChildren<Text>() != null) {
			b.GetComponentInChildren<Text>().color = Color.white;
		}
		b.GetComponent<Image>().color = Color.white;
		b.interactable = true;
	}

	public void setWorldNumber (int n) {
		worldNumber = n;
	}

	public void showPopUp () {
		menuOn = false;
		transition = true;
		transitionNum = 3;
		deltaTime = 0;
		popUp.GetComponentInChildren<Text>().text = "Locked!\nMust beat all levels from the previous world or select option:";
		turnOnRegularButton(popUp.GetComponentsInChildren<Button>()[0]);
		turnOnRegularButton(popUp.GetComponentsInChildren<Button>()[1]);
		if (PlayerPrefs.GetInt(VariableManagement.savePower, 0) == 0) {
			GetComponent<BlurOptimized>().enabled = true;
		}
	}

	public void unlockWorld () {
		if (worldNumber == 0) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld2();
		} else if (worldNumber == 1) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld3();
		} else if (worldNumber == 2) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld4();
		} else if (worldNumber == 3) {
			GetComponent<InAppPurchases>().BuyNonConsumableWorld5();
		}
	}

	public void unlockAllWorlds () {
		GetComponent<InAppPurchases>().BuyNonConsumableAll();
	}

	public void unlockedAllWorlds () {
		for (int i = 0; i < 50; i++) {
			PlayerPrefs.SetInt("World" + i, 1);
		}
		PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
		popUp.GetComponentInChildren<Text>().text = "You unlocked all the worlds and block pieces!";
		showButtons();
		turnOffRegularButton(popUp.GetComponentsInChildren<Button>()[0]);
		turnOffRegularButton(popUp.GetComponentsInChildren<Button>()[1]);
		GetComponent<SoundsAndMusic>().playUnlockSound();
	}

	public void unlockedWorld () {
		PlayerPrefs.SetInt("World" + worldNumber, 1);
		PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
		popUp.GetComponentInChildren<Text>().text = "You unlocked a new world and block pieces!";
		showButtons();
		turnOffRegularButton(popUp.GetComponentsInChildren<Button>()[0]);
		turnOffRegularButton(popUp.GetComponentsInChildren<Button>()[1]);
		GetComponent<SoundsAndMusic>().playUnlockSound();
	}

	public bool isMenuOn() {
		return menuOn;
	}

	public void shiftHighlight(GameObject button) {
		highlight.transform.position = button.transform.position;
	}

	public void deauthorizedLevel () {
		uiHolder.GetComponentsInChildren<Image>()[1].color = Color.clear;
		deauthorize = true;
	}

	/* -------------------------------------------------- Options Menu ---------------------------------------------- */

	public void toMainMenu() {
		PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.userLevelMenu);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.toMainFromEditor);
	}

	public void nextScene(int n) {
		SceneManager.LoadScene(n);
	}

	public void testLevel() {
		PlayerPrefs.SetString(VariableManagement.lastMenu, VariableManagement.editorMenu);
		saveLevel();
		GetComponent<VariableManagement>().turnOffCameraShift();
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition(VariableManagement.toTestFromEditor);
	}

	public void saveLevel() {
		string levelData = "";
		checkText();
		optionHolder.GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>().text = "Saved";
		List<List<GameObject>> blocks = GetComponent<LevelEditor>().getBlocks();
		File.Delete(filePath);
		if (optionHolder.GetComponentInChildren<InputField>().text != "") {
			levelData += optionHolder.GetComponentInChildren<InputField>().text;
		} else {
			levelData += "Untitled";
		}
		levelData += "\n";
		if (PlayerPrefs.GetString(VariableManagement.userName) == "") {
			levelData += "Unknown";
		} else {
			levelData += PlayerPrefs.GetString(VariableManagement.userName);
		}
		levelData += "\n";
		levelData += r.GetComponent<Slider>().value + "," + g.GetComponent<Slider>().value + "," + b.GetComponent<Slider>().value + "\n";
		levelData += rB.GetComponent<Slider>().value + "," + gB.GetComponent<Slider>().value + "," + bB.GetComponent<Slider>().value + "\n";
		levelData += rGradient.x + "," + gGradient.x + "," + bGradient.x + "\n";
		levelData += rGradient.y + "," + gGradient.y + "," + bGradient.y + "\n";
		for (int i = 13; i >= 0; i--) {
			for (int j = 0; j < 8; j++) { 
				if (blocks[i][j] == null) {
					levelData += VariableManagement.noBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.standardBlock + VariableManagement.clone) {
					levelData += VariableManagement.standardBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.multistepBlock + VariableManagement.clone) {
					levelData += VariableManagement.multistepBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.switchBlock + VariableManagement.clone) {
					levelData += VariableManagement.switchBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.activeBlock + VariableManagement.clone) {
					levelData += VariableManagement.activeBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.inactiveBlock + VariableManagement.clone) {
					levelData += VariableManagement.inactiveBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.rotateRBlock + VariableManagement.clone) {
					levelData += VariableManagement.rotateRBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.rotateLBlock + VariableManagement.clone) {
					levelData += VariableManagement.rotateLBlockTile.ToString();
				} else if (blocks[i][j].name == VariableManagement.bombBlock + VariableManagement.clone) {
					levelData += VariableManagement.bombBlockTile.ToString();
				}
			}
			levelData += "\n";
		}
		levelData += VariableManagement.levelDelimiter.ToString();
		File.AppendAllText(filePath, levelData);
		PlayerPrefs.SetString("Data" + GetComponent<VariableManagement>().getUserLevel(), levelData);
		if (deauthorize) {
			GetComponent<OnlineServices>().createLevelUnlock();
			GetComponent<VariableManagement>().setLevelAuthorization(0);
			GetComponent<VariableManagement>().setLevelPostValue(0);
		}
	}

	/* -------------------------------------------------- Color Menu ------------------------------------------------ */

	Vector2 gradientHandlePosition (float x, float y, GameObject handle) {
		Vector3 newPos = new Vector3(
			Mathf.Clamp(x, handle.transform.position.x - gradientSize, handle.transform.position.x + gradientSize), 
			Mathf.Clamp(y, handle.transform.position.y - gradientSize, handle.transform.position.y + gradientSize), 
			0
		);
		handle.GetComponentsInChildren<Image>()[1].transform.position = newPos;
		Vector2 handlePos = new Vector2(
			(handle.GetComponentsInChildren<Image>()[1].transform.position.x - handle.transform.position.x) / (gradientSize * 10), 
			(handle.GetComponentsInChildren<Image>()[1].transform.position.y - handle.transform.position.y) / (gradientSize * 10)
		);
		return handlePos;
	}

	public void ifMovingSlider () {
		movingSlider = true;
	}

	public void changeBackgroundColor() {
		changingColor = true;
		colorTimer += Time.deltaTime;
		if (colorTimer > colorTimerLimit) {
			colorTimer = 0;
			byte backR = byte.Parse(r.GetComponent<Slider>().value.ToString());
			byte backG = byte.Parse(g.GetComponent<Slider>().value.ToString());
			byte backB = byte.Parse(b.GetComponent<Slider>().value.ToString());
			r.GetComponentsInChildren<Image>()[1].color = new Color32(backR, 0, 0, 255);
			g.GetComponentsInChildren<Image>()[1].color = new Color32(0, backG, 0, 255);
			b.GetComponentsInChildren<Image>()[1].color = new Color32(0, 0, backB, 255);
			Camera.main.backgroundColor = new Color32(backR, backG, backB, 255);
			changeBlockColors();
		}
	}

	public void changeBlockColors() {
		List<List<GameObject>> blocks = GetComponent<LevelEditor>().getBlocks();
		rB.GetComponentsInChildren<Image>()[1].color = new Color(rB.GetComponent<Slider>().value * 2, 0, 0);
		gB.GetComponentsInChildren<Image>()[1].color = new Color(0, gB.GetComponent<Slider>().value * 2, 0);
		bB.GetComponentsInChildren<Image>()[1].color = new Color(0, 0, bB.GetComponent<Slider>().value * 2);
		if (blocks != null) {
			for (int i = 0; i < 14; i++) {
				for (int j = 0; j < 8; j++) { 
					if (blocks[i][j] != null) {
						changeBlockColor(blocks[i][j]);
					}
				}
			}
		}
	}

	public void changeBlockColor(GameObject block) {
		float tempR, tempG, tempB;
		float blockX = block.transform.position.x - 3.5f;
		float blockZ = block.transform.position.z - 6.5f;
		tempR = rB.GetComponent<Slider>().value + ((rGradient.x * blockX) + (rGradient.y * blockZ));
		tempG = gB.GetComponent<Slider>().value + ((gGradient.x * blockX) + (gGradient.y * blockZ));
		tempB = bB.GetComponent<Slider>().value + ((bGradient.x * blockX) + (bGradient.y * blockZ));
		if (block.name == VariableManagement.multistepBlock + VariableManagement.clone) {
			tempR = ((tempR + Camera.main.backgroundColor.r) / 2);
			tempG = ((tempG + Camera.main.backgroundColor.g) / 2);
			tempB = ((tempB + Camera.main.backgroundColor.b) / 2);
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB, blockAlpha);
		} else {
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB);
		}
	}

	public void checkText () {
		string newInputFiltered = ""; 
		foreach (char c in optionHolder.GetComponentInChildren<InputField>().text) { 
			if (System.Convert.ToInt32(c) < 50000 && c != '\n') { 
				newInputFiltered += c;
			} 
		}
		optionHolder.GetComponentInChildren<InputField>().text = newInputFiltered;
	}

	public void randomColor() {
		rGradient = gradientHandlePosition(
			Random.Range(rHandle.transform.position.x - gradientSize, rHandle.transform.position.x + gradientSize), 
			Random.Range(rHandle.transform.position.y - gradientSize, rHandle.transform.position.y + gradientSize), 
			rHandle
		);
		gGradient = gradientHandlePosition(
			Random.Range(gHandle.transform.position.x - gradientSize, gHandle.transform.position.x + gradientSize), 
			Random.Range(gHandle.transform.position.y - gradientSize, gHandle.transform.position.y + gradientSize), 
			gHandle
		);
		bGradient = gradientHandlePosition(
			Random.Range(bHandle.transform.position.x - gradientSize, bHandle.transform.position.x + gradientSize), 
			Random.Range(bHandle.transform.position.y - gradientSize, bHandle.transform.position.y + gradientSize), 
			bHandle
		);
		r.GetComponent<Slider>().value = Random.Range(r.GetComponent<Slider>().minValue, r.GetComponent<Slider>().maxValue);
		g.GetComponent<Slider>().value = Random.Range(g.GetComponent<Slider>().minValue, g.GetComponent<Slider>().maxValue);
		b.GetComponent<Slider>().value = Random.Range(b.GetComponent<Slider>().minValue, b.GetComponent<Slider>().maxValue);
		rB.GetComponent<Slider>().value = Random.Range(rB.GetComponent<Slider>().minValue, rB.GetComponent<Slider>().maxValue);
		gB.GetComponent<Slider>().value = Random.Range(gB.GetComponent<Slider>().minValue, gB.GetComponent<Slider>().maxValue);
		bB.GetComponent<Slider>().value = Random.Range(bB.GetComponent<Slider>().minValue, bB.GetComponent<Slider>().maxValue);
	}
}
