using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class FirebaseDatabases : MonoBehaviour {

	static string url = "https://vox-voyager-87607159.firebaseio.com/";

	List<List<String>> publicLevels;
	WebClient client;
	Stream stream;

	bool isConnected () {
		try {
			client = new System.Net.WebClient();
			stream = client.OpenRead("http://www.google.com");
			return true;
		}
		catch  {
			return false;
		}
		finally {
			client.Dispose(); 
			stream.Dispose();
		}
	}

	public void fireBaseMostRecent(Text text) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl(url);

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Date").LimitToLast(50).
			GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					text.text = "Error, please try again";
					return;
				}
				else if (task.IsCompleted) {
					if (task.Result.ChildrenCount > 0) {
						setList(task.Result.Children);
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6 ||
							GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toPublicLevels();
						}
					} else {
						text.text = "No Posts";
					}
				}
			});
			app.Dispose();
		} else {
			text.text = "No Connection";
		}
	}

	public void fireBaseMostDownloaded (Text text) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl(url);

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Downloads").LimitToLast(50).
			GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					text.text = "Error, please try again";
					return;
				}
				else if (task.IsCompleted) {
					if (task.Result.ChildrenCount > 0) {
						setList(task.Result.Children);
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6 ||
							GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toPublicLevels();
						}
					} else {
						text.text = "No Posts";
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toDatabase();
						}
					}
				}
			});
			app.Dispose();
		} else {
			text.text = "No Connection";
		}
	}

	public void fireBaseMostDownloadedRecent (Text text) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl(url);

			DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			long curtime = (long)(System.DateTime.UtcNow - epochStart).TotalSeconds;
			curtime -= 604800;
			curtime *= 1000;

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Date").StartAt(curtime).LimitToLast(50).
			GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					text.text = "Error, please try again";
					return;
				}
				else if (task.IsCompleted) {
					if (task.Result.ChildrenCount > 0) {
						setList(task.Result.Children);
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6 ||
							GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toPublicLevels();
						}
					} else {
						text.text = "No Posts";
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toDatabase();
						}
					}
				}
			});
			app.Dispose();
		} else {
			text.text = "No Connection";
		}
	}

	public void getYourLevels (Text text) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl(url);

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("User ID").
			EqualTo(PlayerPrefs.GetString(VariableManagement.userId)).GetValueAsync().
			ContinueWith(task => {
				if (task.IsFaulted) {
					text.text = "Error, please try again";
					return;
				} else if (task.IsCompleted) {
					if (task.Result.ChildrenCount > 0) {
						setList(task.Result.Children);
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6 ||
							GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toPublicLevels();
						}
					} else {
						text.text = "No Posts";
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toDatabase();
						}
					}
				}
			});
			app.Dispose();
		} else {
			text.text = "No Connection";
		}
	}

	public void searchUsername (Text text, string username) {
		if (isConnected()) {
			string usernameLower = username.ToLower();
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl(url);

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Username Lower").
			EqualTo(usernameLower).LimitToLast(50).GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					text.text = "Error, please try again";
					return;
				}
				else if (task.IsCompleted) {
					if (task.Result.ChildrenCount > 0) {
						setList(task.Result.Children);
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 9 ||
							GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toPublicLevels();
						}
					} else {
						text.text = "No Posts";
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toDatabase();
						}
					}
				}
			});
			app.Dispose();
		} else {
			text.text = "No Connection";
		}
	}

	public void searchMapName (Text text, string mapName) {
		if (isConnected()) {
			string mapNameLower = mapName.ToLower();
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl(url);

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Name Lower").
			EqualTo(mapNameLower).LimitToLast(50).GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					text.text = "Error, please try again";
					return;
				}
				else if (task.IsCompleted) {
					if (task.Result.ChildrenCount > 0) {
						setList(task.Result.Children);
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 9 ||
							GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toPublicLevels();
						}
					} else {
						text.text = "No Posts";
						if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 8) {
							GetComponent<MainMenuInterface>().toDatabase();
						}
					}
				}
			});
			app.Dispose();
		} else {
			text.text = "No Connection";
		}
	}

	public void deleteLevel (string idOfMap, Text text, int lastScreen, string search) {
		if (isConnected()) {
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(url);
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
			reference.Child("Levels").Child(idOfMap).RemoveValueAsync();
			if (lastScreen == 0) {
				fireBaseMostDownloaded(text);
			} else if (lastScreen == 1) {
				fireBaseMostRecent(text);
			} else if (lastScreen == 2) {
				getYourLevels(text);
			} else if (lastScreen == 3) {
				searchUsername(text, search);
			} else if (lastScreen == 4) {
				searchMapName(text, search);
			}
		} else {
			text.text = "No Connection";
		}
	}

	public void postLevel (string data, string name, Button b) {
		if (isConnected()) {
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(url);
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

			string key = reference.Child("Levels").Push().Key;
			reference.Child("Levels").Child(key).Child("Data").SetValueAsync(data);
			reference.Child("Levels").Child(key).Child("Date").SetValueAsync(ServerValue.Timestamp);
			reference.Child("Levels").Child(key).Child("Downloads").SetValueAsync(0);
			reference.Child("Levels").Child(key).Child("Name").SetValueAsync(name);
			reference.Child("Levels").Child(key).Child("User ID").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userId));
			reference.Child("Levels").Child(key).Child("Username").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userName));
			reference.Child("Levels").Child(key).Child("Name Lower").SetValueAsync(name.ToLower());
			reference.Child("Levels").Child(key).Child("Username Lower").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userName).ToLower());

			b.GetComponentInChildren<Text>().text = "Posted";
			b.GetComponentInChildren<Text>().color = Color.white;
			b.GetComponent<Image>().color = Color.clear;
			GetComponent<VariableManagement>().setLevelPostValue(1);
			GetComponent<OnlineServices>().uploadLevelUnlock();
			GetComponent<SoundsAndMusic>().playSuccessSound();
		} else {
			b.GetComponentInChildren<Text>().text = "No Connection";
		}
	}

	public void incrementDownloadCount (string id, int n) {
		if (GetComponent<VariableManagement>().shouldDownloadCountUp(id)) {
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(url);
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
			reference.Child("Levels").Child(id).Child("Downloads").SetValueAsync(n + 1);
		}
	}

	public List<List<String>> getLevelList () {
		return publicLevels;
	}

	void setList (IEnumerable<DataSnapshot> children) {
		List<List<String>> temp = new List<List<String>>();
		int i = 0;
		foreach (var childSnapshot in children) {
			temp.Insert(i, new List<String>());
			temp[i].Add(childSnapshot.Child("Name").Value.ToString());
			temp[i].Add(childSnapshot.Child("Username").Value.ToString());
			temp[i].Add(childSnapshot.Child("Date").Value.ToString());
			temp[i].Add(childSnapshot.Child("Downloads").Value.ToString());
			temp[i].Add(childSnapshot.Child("Data").Value.ToString());
			temp[i].Add(childSnapshot.Key);
			temp[i].Add(childSnapshot.Child("User ID").Value.ToString());
			i++;
		}

		publicLevels = new List<List<String>>();

		for (int j = temp.Count - 1; j >= 0; j--) {
			publicLevels.Add(temp[j]);
		}
	}
}
