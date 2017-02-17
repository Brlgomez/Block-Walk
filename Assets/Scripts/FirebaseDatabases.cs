using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseDatabases : MonoBehaviour {

	List<List<String>> publicLevels;

	public void fireBaseMostRecent(Text text) {
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
			}
		};
	}

	public void fireBaseMostDownloaded (Text text) {
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
			}
		};
	}

	public void getYourLevels (Text text) {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

		FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("User ID").
		StartAt(PlayerPrefs.GetString(VariableManagement.userId, "Unknown")).EndAt(PlayerPrefs.GetString(VariableManagement.userId, "Unknown")).ValueChanged += 
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
	}

	public void searchUsername (Text text, string username) {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

		FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Username").StartAt(username).EndAt(username).ValueChanged += 
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
	}

	public void searchMapName (Text text, string mapName) {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

		FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Name").StartAt(mapName).EndAt(mapName).ValueChanged += 
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
	}

	public void deleteLevel (string idOfMap, Text text) {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		reference.Child("Levels").Child(idOfMap).RemoveValueAsync();
		getYourLevels(text);
	}

	public void postLevel (string data, string name, Text text) {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		string key = reference.Child("Levels").Push().Key;
		reference.Child("Levels").Child(key).Child("Data").SetValueAsync(data);
		reference.Child("Levels").Child(key).Child("Date").SetValueAsync(ServerValue.Timestamp);
		reference.Child("Levels").Child(key).Child("Downloads").SetValueAsync(0);
		reference.Child("Levels").Child(key).Child("Name").SetValueAsync(name);
		reference.Child("Levels").Child(key).Child("User ID").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userId, "Unknown"));
		reference.Child("Levels").Child(key).Child("Username").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userName, "Unknown"));

		text.text = "Posted";
		GetComponent<VariableManagement>().setLevelPostValue(1);
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
