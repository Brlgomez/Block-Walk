using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class EditorInterface : MonoBehaviour {

	private int blurDownsample = 2;

	GameObject menuHolder;
	GameObject colorHolder;
	GameObject optionHolder;

	GameObject r, g, b;
	GameObject rB, gB, bB;
	GameObject rInc, gInc, bInc;
	GameObject rInc2, gInc2, bInc2;

	GameObject highlight;

	bool menuOn = true;

	private string filePath;

	bool transition;
	int transitionNum = 0;
	float deltaTime;

	void Start () {
		filePath = Application.persistentDataPath + "/"+ (PlayerPrefs.GetInt ("User Level", 0)) + ".txt";
		menuHolder = GameObject.Find("Menu Holder");
		colorHolder = GameObject.Find("Color Holder");
		optionHolder = GameObject.Find("Option Holder");
		highlight = GameObject.Find("Block Highlight");
		GetComponent<BlurOptimized> ().downsample = blurDownsample;
		GetComponent<BackgroundColorTransition> ().levelStarting ();
		showMain();
	}

	void Update () {
		if (transition) { 
			deltaTime += Time.deltaTime * 1.5f;
			if (deltaTime > 1) {
				transition = false;
				deltaTime = 0;
				if (transitionNum == 0) {
					Camera.main.orthographicSize = 8;
				}
			}
			if (transitionNum == 0 && deltaTime > 0.25f) {
				GetComponent<BlurOptimized>().enabled = false;
			}
			if (transitionNum == 0) {
				menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.one, deltaTime);
				colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
				optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
				GetComponent<BlurOptimized>().blurSize = colorHolder.transform.localScale.x * 10;
				Camera.main.orthographicSize -= deltaTime;
				Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 8, 100);
			} else if (transitionNum == 1) {
				menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.zero, deltaTime);
				colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
				optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.one, deltaTime);
				GetComponent<BlurOptimized>().blurSize = optionHolder.transform.localScale.x * 10;
				Camera.main.orthographicSize -= deltaTime;
				Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 8, 100);
			} else if (transitionNum == 2) {
				menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.zero, deltaTime);
				colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.one, deltaTime);
				optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
				Camera.main.orthographicSize += deltaTime;
				Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 8, 100);
			}
		}
	}

	public void setVariables (float sliderR, float sliderG, float sliderB, 
		float sliderRInc, float sliderGInc, float sliderBInc, float sliderRInc2, float sliderGInc2, float sliderBInc2) {
		r = GameObject.Find("R");
		g = GameObject.Find("G");
		b = GameObject.Find("B");
		rB = GameObject.Find("Block R");
		gB = GameObject.Find("Block G");
		bB = GameObject.Find("Block B");
		rInc = GameObject.Find("R Inc");
		gInc = GameObject.Find("G Inc");
		bInc = GameObject.Find("B Inc");
		rInc2 = GameObject.Find("R Inc 2");
		gInc2 = GameObject.Find("G Inc 2");
		bInc2 = GameObject.Find("B Inc 2");

		r.GetComponent<Slider>().value = (Camera.main.backgroundColor.r * 255);
		g.GetComponent<Slider>().value = (Camera.main.backgroundColor.g * 255);
		b.GetComponent<Slider>().value = (Camera.main.backgroundColor.b * 255);
		rB.GetComponent<Slider>().value = sliderR;
		gB.GetComponent<Slider>().value = sliderG;
		bB.GetComponent<Slider>().value = sliderB;
		rInc.GetComponent<Slider>().value = sliderRInc;
		gInc.GetComponent<Slider>().value = sliderGInc;
		bInc.GetComponent<Slider>().value = sliderBInc;
		rInc2.GetComponent<Slider>().value = sliderRInc2;
		gInc2.GetComponent<Slider>().value = sliderGInc2;
		bInc2.GetComponent<Slider>().value = sliderBInc2;

		r.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		g.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		b.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		rB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		rInc.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gInc.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bInc.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		rInc2.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gInc2.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bInc2.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});

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
		tempR = rB.GetComponent<Slider>().value + ((rInc.GetComponent<Slider>().value * blockX) + (rInc2.GetComponent<Slider>().value * blockZ));
		tempG = gB.GetComponent<Slider>().value + ((gInc.GetComponent<Slider>().value * blockX) + (gInc2.GetComponent<Slider>().value * blockZ));
		tempB = bB.GetComponent<Slider>().value + ((bInc.GetComponent<Slider>().value * blockX) + (bInc2.GetComponent<Slider>().value * blockZ));
		if (block.name == "Multistep Block(Clone)") {
			block.GetComponent<Renderer>().material.color = new Color((tempR + Camera.main.backgroundColor.r)/2, (tempG + Camera.main.backgroundColor.g)/2, (tempB + Camera.main.backgroundColor.b)/2);
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
		File.AppendAllText(filePath, rInc.GetComponent<Slider>().value + "," + gInc.GetComponent<Slider>().value + "," + bInc.GetComponent<Slider>().value);
		File.AppendAllText(filePath, "\n");
		File.AppendAllText(filePath, rInc2.GetComponent<Slider>().value + "," + gInc2.GetComponent<Slider>().value + "," + bInc2.GetComponent<Slider>().value);		File.AppendAllText(filePath, "\n");
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
				}
			}
			File.AppendAllText(filePath, "\n");
		}
		File.AppendAllText(filePath, "*");
	}

	public void randomColor () {
		r.GetComponent<Slider>().value = Random.Range (0, 255);
		g.GetComponent<Slider>().value = Random.Range (0, 255);
		b.GetComponent<Slider>().value = Random.Range (0, 255);
		rB.GetComponent<Slider>().value = Random.Range (rB.GetComponent<Slider>().minValue, rB.GetComponent<Slider>().maxValue);
		gB.GetComponent<Slider>().value = Random.Range (gB.GetComponent<Slider>().minValue, gB.GetComponent<Slider>().maxValue);
		bB.GetComponent<Slider>().value = Random.Range (bB.GetComponent<Slider>().minValue, bB.GetComponent<Slider>().maxValue);
		rInc.GetComponent<Slider>().value = Random.Range (rInc.GetComponent<Slider>().minValue, rInc.GetComponent<Slider>().maxValue);
		gInc.GetComponent<Slider>().value = Random.Range (gInc.GetComponent<Slider>().minValue, gInc.GetComponent<Slider>().maxValue);
		bInc.GetComponent<Slider>().value = Random.Range (bInc.GetComponent<Slider>().minValue, bInc.GetComponent<Slider>().maxValue);
		rInc2.GetComponent<Slider>().value = Random.Range (rInc2.GetComponent<Slider>().minValue, rInc2.GetComponent<Slider>().maxValue);
		gInc2.GetComponent<Slider>().value = Random.Range (gInc2.GetComponent<Slider>().minValue, gInc2.GetComponent<Slider>().maxValue);
		bInc2.GetComponent<Slider>().value = Random.Range (bInc2.GetComponent<Slider>().minValue, bInc2.GetComponent<Slider>().maxValue);
	}
}
