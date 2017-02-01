using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class BackgroundColorTransition : MonoBehaviour {

	private float levelStartLength = 1.1f;
	private int transitionLength = 1;
	private float uiSpeed = 0.5f;
	private float cubesSpeed = 0.2f;
	private float colorSpeed = 0.5f;

	bool transitionToColor = false;
	bool levelStart = false;
	int colorIndex = 0;
	float timer = 0;
	string scene;
	Color32 newColor;
	Transform ui;
	Transform cubes;
	Vector3 positionOfCubes;

	void Awake () {
		ui = GameObject.Find ("Handle").transform;
		cubes = GameObject.Find ("Cubes").transform;
		positionOfCubes = cubes.position;
		/* Debug.Log (Mathf.Round(Camera.main.backgroundColor.r * 255) + ", " + 
			Mathf.Round(Camera.main.backgroundColor.g * 255) + ", " + 
			Mathf.Round(Camera.main.backgroundColor.b * 255)); */
	}

	void Update () {
		if (levelStart) {
			timer += Time.deltaTime;
			ui.localScale = Vector3.Lerp (ui.localScale, Vector3.one, timer * uiSpeed);
			cubes.position = Vector3.Lerp (cubes.position, positionOfCubes, timer * cubesSpeed);
			if (timer > levelStartLength) {
				cubes.position = positionOfCubes;
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
				if (scene == "Next Level From Game") {
					GetComponent<GameplayInterface> ().nextScene (49);
				} else if (scene == "Next Level From Main Menu") {
					GetComponent<MainMenuInterface> ().nextScene (49);
				} else if (scene == "Main Menu") {
					GetComponent<GameplayInterface> ().nextScene (0);
				} else if (scene == "Restart") {
					GetComponent<GameplayInterface> ().nextScene (49);
				}
				Destroy (GetComponent<BackgroundColorTransition> ());
			}
		}
	}

	public void transition (int n, string s) {
		colorIndex = n;
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
		TextAsset t = Resources.Load ("Levels") as TextAsset;
		string[] level = t.text.Split ("*"[0]);
		string[] lines = level [PlayerPrefs.GetInt("Level", 0)].Split("\n"[0]);
		string[] color = lines [0].Split (","[0]);
		Color32 c = new Color32 (byte.Parse (color [0]), byte.Parse (color [1]), byte.Parse (color [2]), 255);
		return c;
	}
}
