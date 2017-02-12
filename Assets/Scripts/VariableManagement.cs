using UnityEngine;
using System.Collections;

public class VariableManagement : MonoBehaviour {

	// scene transitions
	public static string levelFromMain = "Level From Main Menu";
	public static string toMainFromLevel = "To Main Menu From Level";
	public static string restartOrNextLevel = "Restart Level Or Next Level";
	public static string toEditorFromMain = "To Editor From Main Menu";
	public static string toEditorFromTest = "To Editor From Test";
	public static string toMainFromEditor = "To Main Menu From Editor";
	public static string toTestFromEditor = "To Level From Editor";

	// tags
	public static string player = "Player";
	public static string block = "Block";
	public static string active = "RedBlock";
	public static string inactive = "BlueBlock";
	public static string switchTag = "Switch";
	public static string rotateR = "RotatorR";
	public static string rotateL = "RotatorL";

	// block names
	public static string clone = "(Clone)";
	public static string standardBlock = "Standard Block";
	public static string multistepBlock = "Multistep Block";
	public static string activeBlock = "Red Block";
	public static string inactiveBlock = "Blue Block";
	public static string switchBlock = "Switch Block";
	public static string rotateRBlock = "Rotate Block R";
	public static string rotateLBlock = "Rotate Block L";

	// block tiles from text files
	public static char levelDelimiter = '*';
	public static char noBlockTile = '-';
	public static char standardBlockTile = 'C';
	public static char multistepBlockTile = 'M';
	public static char switchBlockTile = 'S';
	public static char activeBlockTile = 'R';
	public static char inactiveBlockTile = 'B';
	public static char rotateRBlockTile = 'E';
	public static char rotateLBlockTile = 'W';

	// text files
	public static string world1 = "World1";
	public static string world2 = "World2";
	public static string world3 = "World3";

	// scene number
	public static int mainMenu = 0;
	public static int level = 1;
	public static int editor = 2;

	// player prefs
	public static string worldLevel = "Level";
	public static string userLevel = "User Level";
	public static string lastMenu = "Last Menu";
	public static string shiftCamera = "Shift Camera";
	public static string newWorldUnlocked = "New World";

	// last menus
	public static string userLevelMenu = "User";
	public static string worldMenu = "Campaign";
	public static string editorMenu = "Editor";

	// current world level
	public int getWorldLevel () {
		return PlayerPrefs.GetInt(VariableManagement.worldLevel, 0);
	}

	public int getUserLevel () {
		return PlayerPrefs.GetInt(VariableManagement.userLevel, 0);
	}

	public string getLastMenu () {
		return PlayerPrefs.GetString(VariableManagement.lastMenu);
	}

	public int getCameraShift () {
		return PlayerPrefs.GetInt(VariableManagement.shiftCamera, 0);
	}

	public void turnOnCameraShift () {
		PlayerPrefs.SetInt(VariableManagement.shiftCamera, 1);
	}

	public void turnOffCameraShift () {
		PlayerPrefs.SetInt(VariableManagement.shiftCamera, 0);
	}
}
