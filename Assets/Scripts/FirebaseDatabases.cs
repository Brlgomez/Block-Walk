using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseDatabases : MonoBehaviour {

	List<List<String>> publicLevels;

	public void fireBaseMostRecent() {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

		FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Date").LimitToLast(25).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError(e2.DatabaseError.Message);
				return;
			}

			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
				publicLevels = new List<List<String>>();
				int i = 0;
				foreach (var childSnapshot in e2.Snapshot.Children) {
					publicLevels.Insert(i, new List<String>());
					publicLevels[i].Add(childSnapshot.Child("Name").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Username").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Date").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Downloads").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Data").Value.ToString());
					i++;
				}
				if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6) {
					GetComponent<MainMenuInterface>().toPublicLevels();
				}
			}
		};
	}

	public void fireBaseMostDownloaded () {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");

		FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Downloads").LimitToLast(25).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError(e2.DatabaseError.Message);
				return;
			}

			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
				publicLevels = new List<List<String>>();
				int i = 0;
				foreach (var childSnapshot in e2.Snapshot.Children) {
					publicLevels.Insert(i, new List<String>());
					publicLevels[i].Add(childSnapshot.Child("Name").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Username").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Date").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Downloads").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Data").Value.ToString());
					i++;
				}
				if (GetComponent<MainMenuInterface>().getInterfaceNumber() == 6) {
					GetComponent<MainMenuInterface>().toPublicLevels();
				}
			}
		};
	}

	public void postLevel (string data, string name) {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		string key = reference.Child("scores").Push().Key;
		reference.Child("Levels").Child(key).Child("Data").SetValueAsync(data);
		reference.Child("Levels").Child(key).Child("Date").SetValueAsync(ServerValue.Timestamp);
		reference.Child("Levels").Child(key).Child("Downloads").SetValueAsync(0);
		reference.Child("Levels").Child(key).Child("Name").SetValueAsync(name);
		reference.Child("Levels").Child(key).Child("User ID").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userId, "Unknown"));
		reference.Child("Levels").Child(key).Child("Username").SetValueAsync(PlayerPrefs.GetString(VariableManagement.userName, "Unknown"));
	}

	public List<List<String>> getLevelList () {
		return publicLevels;
	}
}
