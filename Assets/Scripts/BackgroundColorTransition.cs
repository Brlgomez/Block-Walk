using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundColorTransition : MonoBehaviour {

	Color32[] colorOfLevels = new Color32[49];
	bool transitionColor = false;
	bool levelStart = true;
	int colorIndex = 0;
	float timer = 0;
	string scene;
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
		// main
		colorOfLevels [0] = new Color32 (60, 78, 87, 255);
		// world 1
		colorOfLevels [1] = new Color32 (255, 128, 128, 255);
		colorOfLevels [2] = new Color32 (89, 203, 209, 255);
		colorOfLevels [3] = new Color32 (255, 255, 128, 255);
		colorOfLevels [4] = new Color32 (192, 229, 203, 255);

		colorOfLevels [5] = new Color32 (255, 160, 127, 255);
		colorOfLevels [6] = new Color32 (127, 192, 255, 255);
		colorOfLevels [7] = new Color32 (172, 220, 201, 255);
		colorOfLevels [8] = new Color32 (255, 160, 160, 255);

		colorOfLevels [9] = new Color32 (172, 122, 155, 255);
		colorOfLevels [10] = new Color32 (16, 0, 48, 255);
		colorOfLevels [11] = new Color32 (161, 255, 222, 255);
		colorOfLevels [12] = new Color32 (255, 192, 208, 255);

		colorOfLevels [13] = new Color32 (64, 32, 48, 255);
		colorOfLevels [14] = new Color32 (255, 255, 160, 255);
		colorOfLevels [15] = new Color32 (155, 229, 219, 255);
		colorOfLevels [16] = new Color32 (234, 222, 212, 255);
		// world 2
		colorOfLevels [17] = new Color32 (177, 207, 189, 255);
		colorOfLevels [18] = new Color32 (215, 131, 118, 255);
		colorOfLevels [19] = new Color32 (255, 217, 214, 255);
		colorOfLevels [20] = new Color32 (195, 188, 228, 255);

		colorOfLevels [21] = new Color32 (56, 76, 87, 255);
		colorOfLevels [22] = new Color32 (60, 0, 52, 255);
		colorOfLevels [23] = new Color32 (41, 49, 53, 255);
		colorOfLevels [24] = new Color32 (77, 82, 120, 255);

		colorOfLevels [25] = new Color32 (165, 250, 228, 255);
		colorOfLevels [26] = new Color32 (255, 228, 193, 255);
		colorOfLevels [27] = new Color32 (185, 255, 236, 255);
		colorOfLevels [28] = new Color32 (211, 255, 222, 255);

		colorOfLevels [29] = new Color32 (255, 139, 29, 255);
		colorOfLevels [30] = new Color32 (1, 31, 48, 255);
		colorOfLevels [31] = new Color32 (39, 40, 65, 255);
		colorOfLevels [32] = new Color32 (74, 0, 88, 255);
		// world 3
		colorOfLevels [33] = new Color32 (255, 171, 195, 255);
		colorOfLevels [34] = new Color32 (255, 204, 176, 255);
		colorOfLevels [35] = new Color32 (159, 143, 122, 255);
		colorOfLevels [36] = new Color32 (166, 118, 108, 255);

		colorOfLevels [37] = new Color32 (121, 153, 187, 255);
		colorOfLevels [38] = new Color32 (29, 38, 53, 255);
		colorOfLevels [39] = new Color32 (153, 164, 196, 255);
		colorOfLevels [40] = new Color32 (76, 52, 52, 255);

		colorOfLevels [41] = new Color32 (241, 166, 165, 255);
		colorOfLevels [42] = new Color32 (118, 121, 179, 255);
		colorOfLevels [43] = new Color32 (63, 234, 183, 255);
		colorOfLevels [44] = new Color32 (176, 225, 180, 255);

		colorOfLevels [45] = new Color32 (166, 231, 205, 255);
		colorOfLevels [46] = new Color32 (12, 10, 48, 255);
		colorOfLevels [47] = new Color32 (138, 80, 80, 255);
		colorOfLevels [48] = new Color32 (227, 126, 167, 255);
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
			if (colorOfLevels.Length > colorIndex && timer < 0.95f) {
				Camera.main.backgroundColor = Color32.Lerp (Camera.main.backgroundColor, colorOfLevels [colorIndex], timer / 2);
				ui.transform.localScale = Vector3.Lerp (ui.transform.localScale, Vector3.zero, timer / 2);
				cubes.transform.position = Vector3.Lerp (cubes.transform.position, Vector3.right * 25, timer / 4);
			}
			else if (timer > 0.95f && timer < 1) {
				Camera.main.backgroundColor = colorOfLevels [colorIndex];
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
	}

	public void levelStarting () {
		cubes.transform.position = Vector3.left * 25;
		ui.transform.localScale = Vector3.zero;
	}
}
