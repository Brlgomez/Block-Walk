﻿using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
//using GooglePlayGames;

public class OnlineServices: MonoBehaviour {

	void Start () {
		if (PlayerPrefs.GetInt (VariableManagement.isOnline, 0) == 0) {
			if (Application.platform == RuntimePlatform.Android) {
				//PlayGamesPlatform.DebugLogEnabled = false;
				//PlayGamesPlatform.Activate ();
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			}
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
				PlayerPrefs.SetString(VariableManagement.userName, Social.localUser.userName);
				PlayerPrefs.SetString(VariableManagement.userId, Social.localUser.id);
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
			if (Application.platform == RuntimePlatform.Android) {
				Social.ReportProgress(GPGSIds.achievement_the_creator, 100.0f, (bool success) => {});
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				Social.ReportProgress(GCIds.achievement_the_creator, 100.0f, (bool success) => {});
			}
		}
	}

	public void uploadLevelUnlock () {
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			if (Application.platform == RuntimePlatform.Android) {
				Social.ReportProgress(GPGSIds.achievement_sending_precious_cargo, 100.0f, (bool success) => {});
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				Social.ReportProgress(GCIds.achievement_sending_precious_cargo, 100.0f, (bool success) => {});
			}
		}
	}

	public void downloadLevelUnlock () {
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			if (Application.platform == RuntimePlatform.Android) {
				Social.ReportProgress(GPGSIds.achievement_beyond_the_voyage, 100.0f, (bool success) => {});
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				Social.ReportProgress(GCIds.achievement_beyond_the_voyage, 100.0f, (bool success) => {});
			}
		}
	}

	public void beatLevel (int world) {
		if (PlayerPrefs.GetInt(VariableManagement.isOnline, 0) == 0) {
			if (Application.platform == RuntimePlatform.Android) {
				Social.ReportProgress(GPGSIds.achievement_welcome_to_the_voyage, 100.0f, (bool success) => {});
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				Social.ReportProgress(GCIds.achievement_welcome_to_the_voyage, 100.0f, (bool success) => {});
			}
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
					if (Application.platform == RuntimePlatform.Android) {
						Social.ReportProgress(GPGSIds.achievement_block_walk, 100.0f, (bool success) => {});
					} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                        Social.ReportProgress(GCIds.achievement_block_walk, 100.0f, (bool success) => {});
					}
				} else if (world == 1) {
					if (Application.platform == RuntimePlatform.Android) {
						Social.ReportProgress(GPGSIds.achievement_extra_steps_involved, 100.0f, (bool success) => {});
					} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                        Social.ReportProgress(GCIds.achievement_extra_steps_involved, 100.0f, (bool success) => {});
					}
				} else if (world == 2) {
					if (Application.platform == RuntimePlatform.Android) {
						Social.ReportProgress(GPGSIds.achievement_switches_and_platforms, 100.0f, (bool success) => {});
					} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                        Social.ReportProgress(GCIds.achievement_switches_and_platforms, 100.0f, (bool success) => {});
					}
				} else if (world == 3) {
					if (Application.platform == RuntimePlatform.Android) {
						Social.ReportProgress(GPGSIds.achievement_mind_twisting, 100.0f, (bool success) => {});
					} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                        Social.ReportProgress(GCIds.achievement_mind_twisting, 100.0f, (bool success) => {});
					}
				} else if (world == 4) {
					if (Application.platform == RuntimePlatform.Android) {
						Social.ReportProgress(GPGSIds.achievement_ignited, 100.0f, (bool success) => {});
					} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                        Social.ReportProgress(GCIds.achievement_ignited, 100.0f, (bool success) => {});
					}
				}
			}
		}
	}
}
