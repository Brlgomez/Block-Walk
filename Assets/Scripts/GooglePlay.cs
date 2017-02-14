using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GooglePlay : MonoBehaviour {

	void Start () {
		//if (PlayerPrefs.GetInt ("Online", 0) == 0) {
			PlayGamesPlatform.DebugLogEnabled = false;
			PlayGamesPlatform.Activate ();
			logIn ();
		//}
	}

	public void logIn() {
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					PlayerPrefs.SetInt ("Online", 0);
					PlayerPrefs.Save ();
				} else {
					PlayerPrefs.SetInt ("Online", 1);
					PlayerPrefs.Save ();
				}
			});
		}
	}

	public void activateAchievements() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				PlayerPrefs.SetInt ("Online", 0);
				PlayerPrefs.Save ();
				Social.ShowAchievementsUI();
			} else {
				PlayerPrefs.SetInt ("Online", 1);
				PlayerPrefs.Save ();
			}
		});
	}
}
