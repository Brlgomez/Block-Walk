﻿using UnityEngine;
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

	// last menus
	public static string userLevelMenu = "User";
	public static string worldMenu = "Campaign";

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
}