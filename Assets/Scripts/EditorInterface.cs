using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class EditorInterface : MonoBehaviour {

	GameObject menuHolder;
	GameObject colorHolder;
	GameObject optionHolder;

	GameObject r, g, b;
	GameObject rB, gB, bB;
	GameObject rInc, gInc, bInc;
	GameObject rXorZ, gXorZ, bXorZ;

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
		GetComponent<BackgroundColorTransition> ().levelStarting ();
		showMain();
	}

	void Update () {
		if (transition) { 
			deltaTime += Time.deltaTime * 1.5f;
			if (deltaTime > 1) {
				transition = false;
				deltaTime = 0;
			}
			if (transitionNum == 0) {
				menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.one, deltaTime);
				colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
				optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
			} else if (transitionNum == 1) {
				menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.zero, deltaTime);
				colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.zero, deltaTime);
				optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.one, deltaTime);
			} else if (transitionNum == 2) {
				menuHolder.transform.localScale = Vector3.Slerp(menuHolder.transform.localScale, Vector3.zero, deltaTime);
				colorHolder.transform.localScale = Vector3.Slerp(colorHolder.transform.localScale, Vector3.one, deltaTime);
				optionHolder.transform.localScale = Vector3.Slerp(optionHolder.transform.localScale, Vector3.zero, deltaTime);
			}
		}
	}

	public void setVariables (float sliderR, float sliderG, float sliderB, 
		float sliderRInc, float sliderGInc, float sliderBInc, bool toggleR, bool toggleG, bool toggleB) {
		r = GameObject.Find("R");
		g = GameObject.Find("G");
		b = GameObject.Find("B");
		rB = GameObject.Find("Block R");
		gB = GameObject.Find("Block G");
		bB = GameObject.Find("Block B");
		rInc = GameObject.Find("R Inc");
		gInc = GameObject.Find("G Inc");
		bInc = GameObject.Find("B Inc");
		rXorZ = GameObject.Find("R Toggle");
		gXorZ = GameObject.Find("G Toggle");
		bXorZ = GameObject.Find("B Toggle");

		r.GetComponent<Slider>().value = (Camera.main.backgroundColor.r * 255);
		g.GetComponent<Slider>().value = (Camera.main.backgroundColor.g * 255);
		b.GetComponent<Slider>().value = (Camera.main.backgroundColor.b * 255);
		rB.GetComponent<Slider>().value = sliderR;
		gB.GetComponent<Slider>().value = sliderG;
		bB.GetComponent<Slider>().value = sliderB;
		rInc.GetComponent<Slider>().value = sliderRInc;
		gInc.GetComponent<Slider>().value = sliderGInc;
		bInc.GetComponent<Slider>().value = sliderBInc;
		rXorZ.GetComponent<Toggle>().isOn = toggleR;
		gXorZ.GetComponent<Toggle>().isOn = toggleG;
		bXorZ.GetComponent<Toggle>().isOn = toggleB;

		r.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		g.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		b.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBackgroundColor ();});
		rB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bB.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		rInc.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gInc.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bInc.GetComponent<Slider>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		rXorZ.GetComponent<Toggle>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		gXorZ.GetComponent<Toggle>().onValueChanged.AddListener (delegate { changeBlockColors ();});
		bXorZ.GetComponent<Toggle>().onValueChanged.AddListener (delegate { changeBlockColors ();});
	}

	public void showMain () {
		menuOn = true;
		colorMenu(Color.clear, false);
		optionMenu(Color.clear, false);
		mainMenu(Color.white, true);
	}

	public void showBlockMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		colorMenu(Color.clear, false);
		optionMenu(Color.clear, false);
	}

	public void showColorMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		optionMenu(Color.clear, false);
		colorMenu(Color.white, true);
	}

	public void showOptionMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		colorMenu(Color.clear, false);
		optionMenu(Color.white, true);
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

	void colorMenu (Color c, bool b) {
		transition = true;
		transitionNum = 2;
		deltaTime = 0;
	}

	void mainMenu (Color c, bool b) {
		transition = true;
		transitionNum = 0;
		deltaTime = 0;
	}

	void optionMenu (Color c, bool b) {
		transition = true;
		transitionNum = 1;
		deltaTime = 0;
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
		if (rXorZ.GetComponent<Toggle>().isOn) {
			tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (blockX));
		} else {
			tempR = rB.GetComponent<Slider>().value + (rInc.GetComponent<Slider>().value * (blockZ));
		}
		if (gXorZ.GetComponent<Toggle>().isOn) {
			tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (blockX));
		} else {
			tempG = gB.GetComponent<Slider>().value + (gInc.GetComponent<Slider>().value * (blockZ));
		}
		if (bXorZ.GetComponent<Toggle>().isOn) {
			tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (blockX));
		} else {
			tempB = bB.GetComponent<Slider>().value + (bInc.GetComponent<Slider>().value * (blockZ));
		}
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
		File.AppendAllText(filePath, rXorZ.GetComponent<Toggle>().isOn + "," + gXorZ.GetComponent<Toggle>().isOn + "," + bXorZ.GetComponent<Toggle>().isOn);
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
		if (Random.value > 0.5f) {
			rXorZ.GetComponent<Toggle>().isOn = !rXorZ.GetComponent<Toggle>().isOn;
		}
		if (Random.value > 0.5f) {
			gXorZ.GetComponent<Toggle>().isOn = !gXorZ.GetComponent<Toggle>().isOn;
		}
		if (Random.value > 0.5f) {
			bXorZ.GetComponent<Toggle>().isOn = !bXorZ.GetComponent<Toggle>().isOn;
		}
	}
}
