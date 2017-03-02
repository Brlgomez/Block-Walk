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
	public static string world0 = "World0";
	public static string world1 = "World1";
	public static string world2 = "World2";
	public static string world3 = "World3";
	public static string world4 = "World4";

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
	public static string isOnline = "Online";
	public static string userName = "User Name";
	public static string userId = "User ID";
	public static string userMapName = "User Map Name";
	public static string savePower = "Save Power";
	public static string playSounds = "Play Sounds";

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

	public bool isLevelAuthorized () {
		return (PlayerPrefs.GetInt("User" + GetComponent<VariableManagement>().getUserLevel()) == 1);
	}

	public void setLevelAuthorization (int n) {
		PlayerPrefs.SetInt("User" + GetComponent<VariableManagement>().getUserLevel(), n);
	}

	public bool isLevelPosted () {
		return (PlayerPrefs.GetInt("Posted" + GetComponent<VariableManagement>().getUserLevel()) == 1);
	}

	public bool isOnlineCheck () {
		return (PlayerPrefs.GetInt(isOnline, 0) == 0 && Social.localUser.id != "1000" && Social.localUser.id != "0" && 
			PlayerPrefs.GetString(VariableManagement.userName) != "" &&
			PlayerPrefs.GetString(VariableManagement.userId) != "");
		//return true;
	}

	public void setLevelPostValue (int n) {
		PlayerPrefs.SetInt("Posted" + GetComponent<VariableManagement>().getUserLevel(), n);
	}

	public bool shouldDownloadCountUp (string name) {
		bool shouldCountUp = true;
		PlayerPrefs.SetInt("DownloadIndex", PlayerPrefs.GetInt("DownloadIndex") + 1);
		if (PlayerPrefs.GetInt("DownloadIndex") > 100) {
			PlayerPrefs.SetInt("DownloadIndex", 0);
		}
		for (int i = 0; i < 100; i++) {
			if (PlayerPrefs.GetString(("Download" + i)) == name) {
				shouldCountUp = false;
				break;
			}
		}
		PlayerPrefs.SetString(("Download" + PlayerPrefs.GetInt("DownloadIndex")), name);
		return shouldCountUp;
	}

	public void unlockWorld2 () {
		PlayerPrefs.SetInt(VariableManagement.world0, 1);
		PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 1);
	}

	public void unlockWorld3 () {
		PlayerPrefs.SetInt(VariableManagement.world1, 1);
		PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 2);
	}

	public void unlockWorld4 () {
		PlayerPrefs.SetInt(VariableManagement.world2, 1);
		PlayerPrefs.SetInt(VariableManagement.newWorldUnlocked, 3);
	}
}
