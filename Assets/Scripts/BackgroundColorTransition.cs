using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class BackgroundColorTransition : MonoBehaviour {

	private int transitionLength = 1;
	private int uiSpeed = 1;
	private float cubesSpeed = 0.2f;
	private float colorSpeed = 0.5f;
	private int cubeDistance = 25;
	private float minCubeDistance = 0.025f;

	bool transitionToColor = false;
	bool levelStart = false;
	float timer = 0;
	string nextScene;
	Color32 newColor;
	Transform ui;
	Transform cubes;
	Vector3 positionOfCubes;
	Vector3 endPosOfUi;
	Vector3 uiPosition;

	void Awake() {
		ui = GameObject.Find("Floor").transform;
		cubes = GameObject.Find("Cubes").transform;
		positionOfCubes = cubes.position;
		uiPosition = ui.transform.position;
		endPosOfUi = new Vector3(-Screen.width / 2, ui.position.y, ui.position.z);
	}

	void Update() {
		if (levelStart) {
			timer += Time.deltaTime;
			ui.position = Vector3.Lerp(ui.position, uiPosition, timer * uiSpeed);
			cubes.position = Vector3.Lerp(cubes.position, positionOfCubes, timer * cubesSpeed);
			if (Vector3.Distance(cubes.position, positionOfCubes) < minCubeDistance || PlayerPrefs.GetInt(VariableManagement.savePower) == 1) {
				cubes.position = positionOfCubes;
				ui.position = uiPosition;
				if (GetComponent<CharacterMovement>() != null) {
					GetComponent<CharacterMovement>().setIfPlayerCanMove(true);
				}
				Destroy(GetComponent<BackgroundColorTransition>());
			}
		}
		if (transitionToColor) {
			timer += Time.deltaTime;
			if (timer < transitionLength - 0.1f && PlayerPrefs.GetInt(VariableManagement.savePower) == 0) {
				Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, newColor, timer * colorSpeed);
				ui.position = Vector3.Lerp(ui.position, endPosOfUi, timer * uiSpeed);
				cubes.position = Vector3.Lerp(cubes.position, Vector3.right * cubeDistance, timer * cubesSpeed);
			} else if (timer > transitionLength - 0.1f && timer < transitionLength && PlayerPrefs.GetInt(VariableManagement.savePower) == 0) {
				Camera.main.backgroundColor = newColor;
			} else if (timer > transitionLength || PlayerPrefs.GetInt(VariableManagement.savePower) == 1) {
				goingToNextScene();
				Destroy(GetComponent<BackgroundColorTransition>());
			}
		}
	}

	void goingToNextScene() {
		if (nextScene == VariableManagement.levelFromMain) {
			GetComponent<MainMenuInterface>().nextScene(VariableManagement.level);
		} else if (nextScene == VariableManagement.toMainFromLevel) {
			GetComponent<GameplayInterface>().nextScene(VariableManagement.mainMenu);
		} else if (nextScene == VariableManagement.restartOrNextLevel) {
			GetComponent<GameplayInterface>().nextScene(VariableManagement.level);
		} else if (nextScene == VariableManagement.toEditorFromMain) {
			GetComponent<MainMenuInterface>().nextScene(VariableManagement.editor);
		} else if (nextScene == VariableManagement.toEditorFromTest) {
			GetComponent<GameplayInterface>().nextScene(VariableManagement.editor);
		} else if (nextScene == VariableManagement.toMainFromEditor) {
			GetComponent<EditorInterface>().nextScene(VariableManagement.mainMenu);
		} else if (nextScene == VariableManagement.toTestFromEditor) {
			GetComponent<EditorInterface>().nextScene(VariableManagement.level);
		}
	}

	public void transition(string s) {
		nextScene = s;
		transitionToColor = true;
		newColor = getColorFromFile();
	}

	public void levelStarting() {
		ui.transform.position = new Vector3(-Screen.width / 2, ui.position.y, 0);
		cubes.position = Vector3.left * cubeDistance;
		levelStart = true;
	}

	Color32 getColorFromFile() {
		TextAsset t = new TextAsset();
		string[] level;
		string[] lines;
		if (nextScene == VariableManagement.toMainFromLevel || nextScene == VariableManagement.toMainFromEditor) {
			if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.userLevelMenu) {
				return MenuColors.editorColor;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 0) {
				return MenuColors.world1Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 1) {
				return MenuColors.world2Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 2) {
				return MenuColors.world3Color;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 3) {
				return MenuColors.world4Color;
			}
			return MenuColors.menuColor;
		} else if (GetComponent<VariableManagement>().getLastMenu() == VariableManagement.worldMenu) {
			if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 0) {
				t = Resources.Load(VariableManagement.world1) as TextAsset;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 1) {
				t = Resources.Load(VariableManagement.world2) as TextAsset;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 2) {
				t = Resources.Load(VariableManagement.world3) as TextAsset;
			} else if (((GetComponent<VariableManagement>().getWorldLevel() - 1) / 16) == 3) {
				t = Resources.Load(VariableManagement.world4) as TextAsset;
			} 
			level = t.text.Split(VariableManagement.levelDelimiter.ToString()[0]);
			lines = level[(GetComponent<VariableManagement>().getWorldLevel() - 1) % 16].Split("\n"[0]);
		} else {
			string filePath = Application.persistentDataPath + "/" + GetComponent<VariableManagement>().getUserLevel() + ".txt";
			StreamReader r;
			if (File.Exists(filePath)) {
				r = File.OpenText(filePath);
				level = r.ReadToEnd().Split(VariableManagement.levelDelimiter.ToString()[0]);
				lines = level[0].Split("\n"[0]);
			} else {
				return MenuColors.editorInterface;
			}
		} 
		string[] color = lines[2].Split(","[0]);
		Color32 c = new Color32(byte.Parse(color[0]), byte.Parse(color[1]), byte.Parse(color[2]), 255);
		return c;
	}
}
