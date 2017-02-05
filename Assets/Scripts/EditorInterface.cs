using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class EditorInterface : MonoBehaviour {

	GameObject menuHolder;
	GameObject colorHolder;
	GameObject blockHolder;
	GameObject optionHolder;
	GameObject blockButton;

	GameObject r, g, b;
	GameObject rB, gB, bB;
	GameObject rInc, gInc, bInc;
	GameObject rXorZ, gXorZ, bXorZ;

	bool menuOn = true;

	private string filePath;

	void Start () {
		filePath = Application.persistentDataPath + "/"+ (PlayerPrefs.GetInt ("User Level", 0)) + ".txt";
		menuHolder = GameObject.Find("Menu Holder");
		colorHolder = GameObject.Find("Color Holder");
		blockHolder = GameObject.Find("Block Holder");
		optionHolder = GameObject.Find("Option Holder");
		blockButton = GameObject.Find("Change Block");
		GetComponent<BackgroundColorTransition> ().levelStarting ();
		showMain();
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
		blockMenu(Color.clear, false);
		colorMenu(Color.clear, false);
		optionMenu(Color.clear, false);
		mainMenu(Color.white, true);
	}

	public void showBlockMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		colorMenu(Color.clear, false);
		optionMenu(Color.clear, false);
		blockMenu(Color.white, true);
	}

	public void showColorMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		blockMenu(Color.clear, false);
		optionMenu(Color.clear, false);
		colorMenu(Color.white, true);
	}

	public void showOptionMenu () {
		menuOn = false;
		mainMenu(Color.clear, false);
		blockMenu(Color.clear, false);
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

	void blockMenu (Color c, bool b) {
		menuHolder.transform.position = Vector3.one * 10000;
		blockHolder.transform.position = Vector3.zero;
		colorHolder.transform.position = Vector3.one * 10000;
		optionHolder.transform.position = Vector3.one * 10000;
	}

	void colorMenu (Color c, bool b) {
		menuHolder.transform.position = Vector3.one * 10000;
		blockHolder.transform.position = Vector3.one * 10000;
		colorHolder.transform.position = Vector3.zero;
		optionHolder.transform.position = Vector3.one * 10000;
	}

	void mainMenu (Color c, bool b) {
		menuHolder.transform.position = Vector3.zero;
		blockHolder.transform.position = Vector3.one * 10000;
		colorHolder.transform.position = Vector3.one * 10000;
		optionHolder.transform.position = Vector3.one * 10000;
	}

	void optionMenu (Color c, bool b) {
		menuHolder.transform.position = Vector3.one * 10000;
		blockHolder.transform.position = Vector3.one * 10000;
		colorHolder.transform.position = Vector3.one * 10000;
		optionHolder.transform.position = Vector3.zero;
	}

	public bool isMenuOn () {
		return menuOn;
	}

	public void changeBlockIcon (Sprite s) {
		blockButton.GetComponent<Image>().sprite = s;
	}

	public void changeBackgroundColor () {
		byte backR = byte.Parse(r.GetComponent<Slider>().value.ToString());
		byte backG = byte.Parse(g.GetComponent<Slider>().value.ToString());
		byte backB = byte.Parse(b.GetComponent<Slider>().value.ToString());
		Camera.main.backgroundColor = new Color32(backR, backG, backB, 255);
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
			block.GetComponent<Renderer>().material.color = new Color((tempR + Camera.main.backgroundColor.r)/2, (tempG + Camera.main.backgroundColor.r)/2, (tempB + Camera.main.backgroundColor.r)/2);
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
}
