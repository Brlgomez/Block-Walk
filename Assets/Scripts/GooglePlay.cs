using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GooglePlay : MonoBehaviour {

	void Start () {
		if (PlayerPrefs.GetInt (VariableManagement.isOnline, 0) == 0) {
			PlayGamesPlatform.DebugLogEnabled = false;
			PlayGamesPlatform.Activate ();
			logIn ();
		}
	}

	public void logIn() {
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					PlayerPrefs.SetInt (VariableManagement.isOnline, 0);
					PlayerPrefs.SetString(VariableManagement.userName, Social.localUser.userName);
					PlayerPrefs.SetString(VariableManagement.userId, Social.localUser.id);
					PlayerPrefs.Save ();
				} else {
					PlayerPrefs.SetInt (VariableManagement.isOnline, 1);
					PlayerPrefs.Save ();
				}
			});
		}
	}

	public void activateAchievements() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				PlayerPrefs.SetInt (VariableManagement.isOnline, 0);
				PlayerPrefs.Save ();
				Social.ShowAchievementsUI();
			} else {
				PlayerPrefs.SetInt (VariableManagement.isOnline, 1);
				PlayerPrefs.Save ();
			}
		});
	}
}
