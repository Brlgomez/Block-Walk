using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class OnlineServices : MonoBehaviour {

	void Start () {
		/*
		PlayerPrefs.SetInt (VariableManagement.isOnline, 0);
		PlayerPrefs.SetString(VariableManagement.userName, "Chellywashere");
		PlayerPrefs.SetString(VariableManagement.userId, "143");
		*/
		if (PlayerPrefs.GetInt (VariableManagement.isOnline, 0) == 0) {
			PlayGamesPlatform.DebugLogEnabled = false;
			PlayGamesPlatform.Activate ();
			logIn ();
		}
	}

	public void logIn() {
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

	public void createLevelUnlock () {
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			Social.ReportProgress(GPGSIds.achievement_the_creator, 100.0f, (bool success) => {});
		}
	}

	public void uploadLevelUnlock () {
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			Social.ReportProgress(GPGSIds.achievement_sending_precious_cargo, 100.0f, (bool success) => {});
		}
	}

	public void downloadLevelUnlock () {
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			Social.ReportProgress(GPGSIds.achievement_beyond_the_voyage, 100.0f, (bool success) => {});
		}
	}

    public void beatLevel (int world) {
        if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
            Social.ReportProgress(GPGSIds.achievement_welcome_to_the_voyage, 100.0f, (bool success) => {});
            bool beatWorld = true;
            for (int i = 0; i < 16; i++) {
                int levelNumber = (world * 16) + i;
                if (PlayerPrefs.GetInt(levelNumber.ToString(), 0) == 0) {
                    beatWorld = false;
                    break;
                }
            }
            if (beatWorld) {
                if (world == 0) {
                    Social.ReportProgress(GPGSIds.achievement_beat_world_1, 100.0f, (bool success) => {});
                } else if (world == 1) {
                    Social.ReportProgress(GPGSIds.achievement_beat_world_2, 100.0f, (bool success) => {});
                } else if (world == 2) {
                    Social.ReportProgress(GPGSIds.achievement_beat_world_3, 100.0f, (bool success) => {});
                } else if (world == 3) {
                    Social.ReportProgress(GPGSIds.achievement_beat_world_4, 100.0f, (bool success) => {});
                }
            }
        }
    }
}
