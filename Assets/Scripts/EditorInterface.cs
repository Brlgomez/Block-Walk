using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class EditorInterface : MonoBehaviour {

	private int maxBlurSize = 10;
	private int blurDownsample = 2;

	GameObject cubes;
	GameObject menuHolder;
	GameObject colorHolder;
	GameObject optionHolder;
	GameObject r, g, b;
	GameObject rB, gB, bB;
	GameObject rIncX, gIncX, bIncX;
	GameObject rIncZ, gIncZ, bIncZ;
	GameObject highlight;

	bool menuOn = true;
	private string filePath;
	bool transition;
	int transitionNum = 0;
	float deltaTime;
	Vector3 initialCamPos;
	Vector3 colorMenuCamPos;

	void Start () {
		filePath = Application.persistentDataPath + "/"+ (PlayerPrefs.GetInt ("User Level", 0)) + ".txt";
		cubes = GameObject.Find ("Cubes");
		menuHolder = GameObject.Find("Menu Holder");
		colorHolder = GameObject.Find("Color Holder");
		optionHolder = GameObject.Find("Option Holder");
		highlight = GameObject.Find("Block Highlight");
		GetComponent<BlurOptimized> ().downsample = blurDownsample;
		GetComponent<BackgroundColorTransition> ().levelStarting ();
		initialCamPos = transform.position;
		colorMenuCamPos = new Vector3(transform.position.x, transform.position.y, 4.75f);
		showMain();
	}

	void Update () {
		if (transition) { 
			transitionInterface();
		} else {
			if (Input.GetMouseButtonDown (0) && cubes.transform.position == Vector3.zero && transitionNum == 0) {
				GetComponent<LevelEditor>().mouseDown();
			}
			if (Input.GetMouseButtonUp (0)) {
				GetComponent<LevelEditor>().mouseUp();
			}
			if (GetComponent<LevelEditor>().getMouseDrag()) {
				GetComponent<LevelEditor>().mouseDrag();
			}
		}
	}

	void transitionInterface () {
		deltaTime += Time.deltaTime * 1.5f;
		if (deltaTime > 1) {
			GetComponent<LevelEditor>().mouseUp();
			transition = false;
			deltaTime = 0;
			if (transitionNum == 0) {
				Camera.main.orthographicSize = 8;
			}
		}
		if (transitionNum == 0) {
			if (deltaTime > 0.25f) {
				GetComponent<BlurOptimized>().enabled = false;
			}
			menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.one, deltaTime);
			colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
			optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
			transform.position = Vector3.Lerp(transform.position, initialCamPos, deltaTime);
			GetComponent<BlurOptimized>().blurSize = colorHolder.transform.localScale.x * maxBlurSize;
			Camera.main.orthographicSize -= deltaTime;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 8, 17);
		} else if (transitionNum == 1) {
			menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.zero, deltaTime);
			colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
			optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.one, deltaTime);
			transform.position = Vector3.Lerp(transform.position, initialCamPos, deltaTime);
			GetComponent<BlurOptimized>().blurSize = optionHolder.transform.localScale.x * maxBlurSize;
			Camera.main.orthographicSize -= deltaTime;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 8, 17);
		} else if (transitionNum == 2) {
			menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.zero, deltaTime);
			colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.one, deltaTime);
			optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
			transform.position = Vector3.Lerp(transform.position, colorMenuCamPos, deltaTime);
			Camera.main.orthographicSize += deltaTime;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 8, 17);
		}
	}

	public void setVariables (float sR, float sG, float sB, float sRInc, float sGInc, float sBInc, float sRInc2, float sGInc2, float sBInc2) {
		r = GameObject.Find("R");
		g = GameObject.Find("G");
		b = GameObject.Find("B");
		rB = GameObject.Find("Block R");
		gB = GameObject.Find("Block G");
		bB = GameObject.Find("Block B");
		rIncX = GameObject.Find("R Inc");
		gIncX = GameObject.Find("G Inc");
		bIncX = GameObject.Find("B Inc");
		rIncZ = GameObject.Find("R Inc 2");
		gIncZ = GameObject.Find("G Inc 2");
		bIncZ = GameObject.Find("B Inc 2");

		r.GetComponent<Slider>().value = (Camera.main.backgroundColor.r * 255);
		g.GetComponent<Slider>().value = (Camera.main.backgroundColor.g * 255);
		b.GetComponent<Slider>().value = (Camera.main.backgroundColor.b * 255);
		rB.GetComponent<Slider>().value = sR;
		gB.GetComponent<Slider>().value = sG;
		bB.GetComponent<Slider>().value = sB;
		rIncX.GetComponent<Slider>().value = sRInc;
		gIncX.GetComponent<Slider>().value = sGInc;
		bIncX.GetComponent<Slider>().value = sBInc;
		rIncZ.GetComponent<Slider>().value = sRInc2;
		gIncZ.GetComponent<Slider>().value = sGInc2;
		bIncZ.GetComponent<Slider>().value = sBInc2;

		r.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		g.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		b.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		rB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		rIncX.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gIncX.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bIncX.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		rIncZ.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gIncZ.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bIncZ.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});

		changeBackgroundColor();
	}

	public void showMain () {
		menuOn = true;
		transition = true;
		transitionNum = 0;
		deltaTime = 0;
	}

	public void showColorMenu () {
		menuOn = false;
		transition = true;
		transitionNum = 2;
		deltaTime = 0;
	}

	public void showOptionMenu () {
		menuOn = false;
		transition = true;
		transitionNum = 1;
		deltaTime = 0;
		GetComponent<BlurOptimized>().enabled = true;
	}

	public void toMainMenu () {
		PlayerPrefs.SetString("Last Menu", "User");
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition("To Main Menu From Editor");
	}

	public void nextScene (int n) {
		SceneManager.LoadScene (n);
	}

	public void testLevel () {
		PlayerPrefs.SetString("Last Menu", "Editor");
		saveLevel();
		PlayerPrefs.SetInt ("Shift Camera", 0);
		gameObject.AddComponent<BackgroundColorTransition>();
		GetComponent<BackgroundColorTransition>().transition("To Level From Editor");
	}
		
	public bool isMenuOn () {
		return menuOn;
	}

	public void shiftHighlight (GameObject button) {
		highlight.transform.position = button.transform.position;
	}

	public void changeBackgroundColor () {
		byte backR = byte.Parse(r.GetComponent<Slider>().value.ToString());
		byte backG = byte.Parse(g.GetComponent<Slider>().value.ToString());
		byte backB = byte.Parse(b.GetComponent<Slider>().value.ToString());
		r.GetComponentsInChildren<Image>()[2].color = new Color32 (backR, 0, 0, 255);
		g.GetComponentsInChildren<Image>()[2].color = new Color32 (0, backG, 0, 255);
		b.GetComponentsInChildren<Image>()[2].color = new Color32 (0, 0, backB, 255);
		Camera.main.backgroundColor = new Color32(backR, backG, backB, 255);
		changeBlockColors();
	}

	public void changeBlockColors () {
		List<List<GameObject>> blocks = GetComponent<LevelEditor>().getBlocks();
		rB.GetComponentsInChildren<Image>()[2].color = new Color (rB.GetComponent<Slider>().value * 2, 0, 0);
		gB.GetComponentsInChildren<Image>()[2].color = new Color (0, gB.GetComponent<Slider>().value * 2, 0);
		bB.GetComponentsInChildren<Image>()[2].color = new Color (0, 0, bB.GetComponent<Slider>().value * 2);
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

	public void changeBlockColor (GameObject block) {
		float tempR, tempG, tempB;
		float blockX = block.transform.position.x - 3.5f;
		float blockZ = block.transform.position.z - 6.5f;
		tempR = rB.GetComponent<Slider>().value + ((rIncX.GetComponent<Slider>().value * blockX) + (rIncZ.GetComponent<Slider>().value * blockZ));
		tempG = gB.GetComponent<Slider>().value + ((gIncX.GetComponent<Slider>().value * blockX) + (gIncZ.GetComponent<Slider>().value * blockZ));
		tempB = bB.GetComponent<Slider>().value + ((bIncX.GetComponent<Slider>().value * blockX) + (bIncZ.GetComponent<Slider>().value * blockZ));
		if (block.name == "Multistep Block(Clone)") {
			tempR = ((tempR + Camera.main.backgroundColor.r) / 2);
			tempG = ((tempG + Camera.main.backgroundColor.g) / 2);
			tempB = ((tempB + Camera.main.backgroundColor.b) / 2);
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB, 0.75f);
		} else {
			block.GetComponent<Renderer>().material.color = new Color(tempR, tempG, tempB);
		}
	}

	public void saveLevel () {
		List<List<GameObject>> blocks = GetComponent<LevelEditor>().getBlocks();
		File.Delete(filePath);
		File.AppendAllText(filePath, r.GetComponent<Slider>().value + "," + g.GetComponent<Slider>().value + "," + b.GetComponent<Slider>().value);
		File.AppendAllText(filePath, "\n");
		File.AppendAllText(filePath, rB.GetComponent<Slider>().value + "," + gB.GetComponent<Slider>().value + "," + bB.GetComponent<Slider>().value);
		File.AppendAllText(filePath, "\n");
		File.AppendAllText(filePath, rIncX.GetComponent<Slider>().value + "," + gIncX.GetComponent<Slider>().value + "," + bIncX.GetComponent<Slider>().value);
		File.AppendAllText(filePath, "\n");
		File.AppendAllText(filePath, rIncZ.GetComponent<Slider>().value + "," + gIncZ.GetComponent<Slider>().value + "," + bIncZ.GetComponent<Slider>().value);		
		File.AppendAllText(filePath, "\n");
		for (int i = 13; i >= 0; i--) {
			for (int j = 0; j < 8; j++) { 
				if (blocks[i][j] == null) {
					File.AppendAllText(filePath, "-");
				} else if (blocks[i][j].name == "Standard Block(Clone)") {
					File.AppendAllText(filePath, "C");
				} else if (blocks[i][j].name == "Multistep Block(Clone)") {
					File.AppendAllText(filePath, "M");
				} else if (blocks[i][j].name == "Switch Block(Clone)") {
					File.AppendAllText(filePath, "S");
				} else if (blocks[i][j].name == "Red Block(Clone)") {
					File.AppendAllText(filePath, "R");
				} else if (blocks[i][j].name == "Blue Block(Clone)") {
					File.AppendAllText(filePath, "B");
				} else if (blocks[i][j].name == "Rotate Block R(Clone)") {
					File.AppendAllText(filePath, "E");
				} else if (blocks[i][j].name == "Rotate Block L(Clone)") {
					File.AppendAllText(filePath, "W");
				}
			}
			File.AppendAllText(filePath, "\n");
		}
		File.AppendAllText(filePath, "*");
	}

	public void randomColor () {
		r.GetComponent<Slider>().value = Random.Range (r.GetComponent<Slider>().minValue, r.GetComponent<Slider>().maxValue);
		g.GetComponent<Slider>().value = Random.Range (g.GetComponent<Slider>().minValue, g.GetComponent<Slider>().maxValue);
		b.GetComponent<Slider>().value = Random.Range (b.GetComponent<Slider>().minValue, b.GetComponent<Slider>().maxValue);
		rB.GetComponent<Slider>().value = Random.Range (rB.GetComponent<Slider>().minValue, rB.GetComponent<Slider>().maxValue);
		gB.GetComponent<Slider>().value = Random.Range (gB.GetComponent<Slider>().minValue, gB.GetComponent<Slider>().maxValue);
		bB.GetComponent<Slider>().value = Random.Range (bB.GetComponent<Slider>().minValue, bB.GetComponent<Slider>().maxValue);
		rIncX.GetComponent<Slider>().value = Random.Range (rIncX.GetComponent<Slider>().minValue, rIncX.GetComponent<Slider>().maxValue);
		gIncX.GetComponent<Slider>().value = Random.Range (gIncX.GetComponent<Slider>().minValue, gIncX.GetComponent<Slider>().maxValue);
		bIncX.GetComponent<Slider>().value = Random.Range (bIncX.GetComponent<Slider>().minValue, bIncX.GetComponent<Slider>().maxValue);
		rIncZ.GetComponent<Slider>().value = Random.Range (rIncZ.GetComponent<Slider>().minValue, rIncZ.GetComponent<Slider>().maxValue);
		gIncZ.GetComponent<Slider>().value = Random.Range (gIncZ.GetComponent<Slider>().minValue, gIncZ.GetComponent<Slider>().maxValue);
		bIncZ.GetComponent<Slider>().value = Random.Range (bIncZ.GetComponent<Slider>().minValue, bIncZ.GetComponent<Slider>().maxValue);
	}
}
