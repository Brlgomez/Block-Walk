using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class BackgroundColorTransition : MonoBehaviour {

	bool transitionColor = false;
	bool levelStart = true;
	int colorIndex = 0;
	float timer = 0;
	string scene;
	Color32 transitionalColor;
	GameObject ui;
	GameObject cubes;
	GameObject menuBackground;
	Vector3 positionOfCubes;

	void Start () {
		ui = GameObject.Find ("UI Holder");
		cubes = GameObject.Find ("Cubes");
		positionOfCubes = cubes.transform.position;
		levelStarting ();
		//Debug.Log (Mathf.Round(Camera.main.backgroundColor.r * 255) + ", " + 
		//	Mathf.Round(Camera.main.backgroundColor.g * 255) + ", " + 
		//	Mathf.Round(Camera.main.backgroundColor.b * 255));
	}

	void Update () {
		if (levelStart) {
			timer += Time.deltaTime;
			ui.transform.localScale = Vector3.Lerp (ui.transform.localScale, Vector3.one, timer / 2);
			cubes.transform.position = Vector3.Lerp (cubes.transform.position, positionOfCubes, timer / 4);
			if (timer > 1.1f) {
				cubes.transform.position = positionOfCubes;
				levelStart = false;
				timer = 0;
			}
		}
		if (transitionColor) {
			timer += Time.deltaTime;
			if (timer < 0.95f) {
				Camera.main.backgroundColor = Color32.Lerp (Camera.main.backgroundColor, transitionalColor, timer / 2);
				ui.transform.localScale = Vector3.Lerp (ui.transform.localScale, Vector3.zero, timer / 2);
				cubes.transform.position = Vector3.Lerp (cubes.transform.position, Vector3.right * 25, timer / 4);
			}
			else if (timer > 0.95f && timer < 1) {
				Camera.main.backgroundColor = transitionalColor;
			}
			else if (timer > 1) {
				if (scene == "Next Level From Game") {
					GetComponent<GameplayInterface> ().nextScene (colorIndex);
				} else if (scene == "Next Level From Main Menu") {
					GetComponent<MainMenuInterface> ().nextScene (colorIndex + 1);
				} else if (scene == "Main Menu") {
					GetComponent<GameplayInterface> ().nextScene (0);
				} else if (scene == "Restart") {
					GetComponent<GameplayInterface> ().nextScene (colorIndex);
				}
				transitionColor = false;
			}
		}
	}

	public void transition (int n, string s) {
		colorIndex = n;
		scene = s;
		transitionColor = true;
		transitionalColor = getColorFromFile (colorIndex);
	}

	public void levelStarting () {
		cubes.transform.position = Vector3.left * 25;
		ui.transform.localScale = Vector3.zero;
	}

	Color32 getColorFromFile (int line) {
		TextAsset colors = Resources.Load("Colors") as TextAsset;
		string[] lines = colors.text.Split("\n"[0]);
		string[] nums = lines [line].Split (","[0]);
		Color32 c = new Color32 (byte.Parse(nums[0]), byte.Parse(nums[1]), byte.Parse(nums[2]), byte.Parse(nums[3]));
		return c;
	}
}
