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

public class FirebaseDatabases : MonoBehaviour {

	List<List<String>> publicLevels;

	public bool isConnected() {
		string resource = "http://google.com";
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		try {
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse()) {
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess) {
					return true;
				}
			}
		}
		catch {
			return false;
		}
		return false;
	}

	public void fireBaseMostRecent(Text text) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Date").LimitToLast(25).ValueChanged += 
			(object sender2, ValueChangedEventArgs e2) => {
				if (e2.DatabaseError != null) {
					text.text = "Error, please try again";
					Debug.LogError(e2.DatabaseError.Message);
					return;
				}

				if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
					List<List<String>> temp = new List<List<String>>();
					int i = 0;
					foreach (var childSnapshot in e2.Snapshot.Children) {
						temp.Insert(i, new List<String>());
						temp[i].Add(childSnapshot.Child("Name").Value.ToString());
						temp[i].Add(childSnapshot.Child("Username").Value.ToString());
						temp[i].Add(childSnapshot.Child("Date").Value.ToString());
						temp[i].Add(childSnapshot.Child("Downloads").Value.ToString());
						temp[i].Add(childSnapshot.Child("Data").Value.ToString());
						temp[i].Add(childSnapshot.Key);
						i++;
					}
					publicLevels = new List<List<String>>();
					for (int j = temp.Count - 1; j >= 0; j--) {
						publicLevels.Add(temp[j]);
					}

					if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6) {
						GetComponent<MainMenuInterface>().toPublicLevels();
					}
				} else {
					text.text = "No Posts";
					return;
				}
			};
		} else {
			text.text = "No Connection";
		}
	}

	public void fireBaseMostDownloaded (Text text) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Downloads").LimitToLast(25).ValueChanged += 
			(object sender2, ValueChangedEventArgs e2) => {
				if (e2.DatabaseError != null) {
					text.text = "Error, please try again";
					Debug.LogError(e2.DatabaseError.Message);
					return;
				}

				if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
					List<List<String>> temp = new List<List<String>>();
					int i = 0;
					foreach (var childSnapshot in e2.Snapshot.Children) {
						temp.Insert(i, new List<String>());
						temp[i].Add(childSnapshot.Child("Name").Value.ToString());
						temp[i].Add(childSnapshot.Child("Username").Value.ToString());
						temp[i].Add(childSnapshot.Child("Date").Value.ToString());
						temp[i].Add(childSnapshot.Child("Downloads").Value.ToString());
						temp[i].Add(childSnapshot.Child("Data").Value.ToString());
						temp[i].Add(childSnapshot.Key);
						i++;
					}

					publicLevels = new List<List<String>>();

					for (int j = temp.Count - 1; j >= 0; j--) {
						publicLevels.Add(temp[j]);
					}

					if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6) {
						GetComponent<MainMenuInterface>().toPublicLevels();
					}
				} else {
					text.text = "No Posts";
				}
			};
		} else {
			text.text = "No Connection";
		}
	}

	public void getYourLevels (Text text) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("User ID").
			StartAt(PlayerPrefs.GetString(VariableManagement.userId, "Unknown")).
			EndAt(PlayerPrefs.GetString(VariableManagement.userId, "Unknown")).ValueChanged += 
			(object sender2, ValueChangedEventArgs e2) => {
				if (e2.DatabaseError != null) {
					text.text = "Error, please try again";
					Debug.LogError(e2.DatabaseError.Message);
					return;
				}

				if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
					List<List<String>> temp = new List<List<String>>();
					int i = 0;
					foreach (var childSnapshot in e2.Snapshot.Children) {
						temp.Insert(i, new List<String>());
						temp[i].Add(childSnapshot.Child("Name").Value.ToString());
						temp[i].Add(childSnapshot.Child("Username").Value.ToString());
						temp[i].Add(childSnapshot.Child("Date").Value.ToString());
						temp[i].Add(childSnapshot.Child("Downloads").Value.ToString());
						temp[i].Add(childSnapshot.Child("Data").Value.ToString());
						temp[i].Add(childSnapshot.Key);
						i++;
					}

					publicLevels = new List<List<String>>();

					for (int j = temp.Count - 1; j >= 0; j--) {
						publicLevels.Add(temp[j]);
					}

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
			};
		} else {
			text.text = "No Connection";
		}
	}

	public void searchUsername (Text text, string username) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

			string usernameLower = username.ToLower();
			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Username Lower").
			StartAt(usernameLower).EndAt(usernameLower).ValueChanged += 
			(object sender2, ValueChangedEventArgs e2) => {
				if (e2.DatabaseError != null) {
					text.text = "Error, please try again";
					Debug.LogError(e2.DatabaseError.Message);
					return;
				}

				if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
					List<List<String>> temp = new List<List<String>>();
					int i = 0;
					foreach (var childSnapshot in e2.Snapshot.Children) {
						temp.Insert(i, new List<String>());
						temp[i].Add(childSnapshot.Child("Name").Value.ToString());
						temp[i].Add(childSnapshot.Child("Username").Value.ToString());
						temp[i].Add(childSnapshot.Child("Date").Value.ToString());
						temp[i].Add(childSnapshot.Child("Downloads").Value.ToString());
						temp[i].Add(childSnapshot.Child("Data").Value.ToString());
						temp[i].Add(childSnapshot.Key);
						i++;
					}

					publicLevels = new List<List<String>>();

					for (int j = temp.Count - 1; j >= 0; j--) {
						publicLevels.Add(temp[j]);
					}

					if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 9) {
						GetComponent<MainMenuInterface>().toPublicLevels();
					}
				} else {
					text.text = "No Posts";
				}
			};
		} else {
			text.text = "No Connection";
		}
	}

	public void searchMapName (Text text, string mapName) {
		if (isConnected()) {
			FirebaseApp app = FirebaseApp.DefaultInstance;

			string mapNameLower = mapName.ToLower();
			app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

			FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Name Lower").StartAt(mapNameLower).EndAt(mapNameLower).ValueChanged += 
			(object sender2, ValueChangedEventArgs e2) => {
				if (e2.DatabaseError != null) {
					text.text = "Error, please try again";
					Debug.LogError(e2.DatabaseError.Message);
					return;
				}

				if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
					List<List<String>> temp = new List<List<String>>();
					int i = 0;
					foreach (var childSnapshot in e2.Snapshot.Children) {
						temp.Insert(i, new List<String>());
						temp[i].Add(childSnapshot.Child("Name").Value.ToString());
						temp[i].Add(childSnapshot.Child("Username").Value.ToString());
						temp[i].Add(childSnapshot.Child("Date").Value.ToString());
						temp[i].Add(childSnapshot.Child("Downloads").Value.ToString());
						temp[i].Add(childSnapshot.Child("Data").Value.ToString());
						temp[i].Add(childSnapshot.Key);
						i++;
					}

					publicLevels = new List<List<String>>();

					for (int j = temp.Count - 1; j >= 0; j--) {
						publicLevels.Add(temp[j]);
					}

					if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 9) {
						GetComponent<MainMenuInterface>().toPublicLevels();
					}
				} else {
					text.text = "No Posts";
					return;
				}
			};
		} else {
			text.text = "No Connection";
		}
	}

	public void deleteLevel (string idOfMap, Text text) {
		if (isConnected()) {
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

			reference.Child("Levels").Child(idOfMap).RemoveValueAsync();
			getYourLevels(text);
		} else {
			text.text = "No Connection";
		}
	}

	public void postLevel (string data, string name, Button b) {
		if (isConnected()) {
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

			string key = reference.Child("Levels").Push().Key;
			reference.Child("Levels").Child(key).Child("Data").SetValueAsync(data);
			reference.Child("Levels").Child(key).Child("Date").SetValueAsync(ServerValue.Timestamp);
			reference.Child("Levels").Child(key).Child("Downloads").SetValueAsync(0);
			reference.Child("Levels").Child(key).Child("Name").SetValueAsync(name);
			reference.Child("Levels").Child(key).Child("User ID").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userId, "Unknown"));
			reference.Child("Levels").Child(key).Child("Username").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userName, "Unknown"));
			reference.Child("Levels").Child(key).Child("Name Lower").SetValueAsync(name.ToLower());
			reference.Child("Levels").Child(key).Child("Username Lower").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userName, "Unknown").ToLower());

			b.GetComponentInChildren<Text>().text = "Posted";
			b.GetComponentInChildren<Text>().color = Color.white;
			b.GetComponent<Image>().color = Color.clear;
			GetComponent<VariableManagement>().setLevelPostValue(1);
		} else {
			b.GetComponentInChildren<Text>().text = "No Connection";
		}
	}

	public void incrementDownloadCount (string id, int n) {
		if (GetComponent<VariableManagement>().shouldDownloadCountUp(id)) {
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
			reference.Child("Levels").Child(id).Child("Downloads").SetValueAsync(n + 1);
		}
	}

	public List<List<String>> getLevelList () {
		return publicLevels;
	}
}
