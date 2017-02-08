using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class BackgroundColorTransition : MonoBehaviour {

	private float levelStartLength = 1.1f;
	private int transitionLength = 1;
	private int uiSpeed = 1;
	private float cubesSpeed = 0.2f;
	private float colorSpeed = 0.5f;

	bool transitionToColor = false;
	bool levelStart = false;
	float timer = 0;
	string scene;
	Color32 newColor;
	Transform ui;
	Transform cubes;
	Vector3 positionOfCubes;

	void Awake () {
		ui = GameObject.Find ("Floor").transform;
		cubes = GameObject.Find ("Cubes").transform;
		positionOfCubes = cubes.position;
	}

	void Update () {
		if (levelStart) {
			timer += Time.deltaTime;
			ui.localScale = Vector3.Lerp (ui.localScale, Vector3.one, timer * uiSpeed);
			cubes.position = Vector3.Lerp (cubes.position, positionOfCubes, timer * cubesSpeed);
			if (timer > levelStartLength) {
				cubes.position = positionOfCubes;
				ui.localScale = Vector3.one;
				Destroy (GetComponent<BackgroundColorTransition> ());
			}
		}
		if (transitionToColor) {
			timer += Time.deltaTime;
			if (timer < transitionLength - 0.1f) {
				Camera.main.backgroundColor = Color32.Lerp (Camera.main.backgroundColor, newColor, timer * colorSpeed);
				ui.localScale = Vector3.Lerp (ui.localScale, Vector3.zero, timer * uiSpeed);
				cubes.position = Vector3.Lerp (cubes.position, Vector3.right * 25, timer * cubesSpeed);
			} else if (timer > transitionLength - 0.1f && timer < transitionLength) {
				Camera.main.backgroundColor = newColor;
			} else if (timer > transitionLength) {
				if (scene == "Level From Main Menu") {
					GetComponent<MainMenuInterface> ().nextScene (1);
				} else if (scene == "To Main Menu") {
					GetComponent<GameplayInterface> ().nextScene (0);
				} else if (scene == "Restart") {
					GetComponent<GameplayInterface> ().nextScene (1);
				} else if (scene == "Editor From Main Menu") {
					GetComponent<MainMenuInterface> ().nextScene (2);
				} else if (scene == "To Editor From Test") {
					GetComponent<GameplayInterface> ().nextScene (2);
				} else if (scene == "To Main Menu From Editor") {
					GetComponent<EditorInterface> ().nextScene (0);
				} else if (scene == "To Level From Editor") {
					GetComponent<EditorInterface> ().nextScene (1);
				} 
				Destroy (GetComponent<BackgroundColorTransition> ());
			}
		}
	}

	public void transition (string s) {
		scene = s;
		transitionToColor = true;
		newColor = getColorFromFile();
	}

	public void levelStarting () {
		ui.localScale = Vector3.zero;
		cubes.position = Vector3.left * 25;
		levelStart = true;
	}

	Color32 getColorFromFile () {
		TextAsset t = new TextAsset ();
		string[] level;
		string[] lines;
		if (scene == "To Main Menu" || scene == "To Main Menu From Editor") {
			return new Color32(60, 78, 87, 255);
		}
		else if (PlayerPrefs.GetString("Last Menu") == "Campaign") {
			if (PlayerPrefs.GetInt("Level", 0) >= 1 && PlayerPrefs.GetInt("Level", 0) <= 16) {
				t = Resources.Load("World1") as TextAsset;
			} else if (PlayerPrefs.GetInt("Level", 0) >= 17 && PlayerPrefs.GetInt("Level", 0) <= 32) {
				t = Resources.Load("World2") as TextAsset;
			} else if (PlayerPrefs.GetInt("Level", 0) >= 33 && PlayerPrefs.GetInt("Level", 0) <= 48) {
				t = Resources.Load("World3") as TextAsset;
			} 
			level = t.text.Split("*"[0]);
			lines = level[(PlayerPrefs.GetInt("Level", 0) - 1) % 16].Split("\n"[0]);
		} else {
			string filePath = Application.persistentDataPath + "/" + (PlayerPrefs.GetInt("User Level", 0)) + ".txt";
			StreamReader r;
			if (File.Exists(filePath)) {
				r = File.OpenText(filePath);
				level = r.ReadToEnd().Split("*"[0]);
				lines = level[0].Split("\n"[0]);
			} else {
				return new Color32(160, 192, 224, 255);
			}
		} 
		string[] color = lines [0].Split (","[0]);
		Color32 c = new Color32 (byte.Parse (color [0]), byte.Parse (color [1]), byte.Parse (color [2]), 255);
		return c;
	}
}
