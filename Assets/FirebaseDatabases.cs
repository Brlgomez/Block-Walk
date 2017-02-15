﻿using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseDatabases : MonoBehaviour {

	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	List<List<String>> publicLevels;

	void Start() {

		dependencyStatus = FirebaseApp.CheckDependencies();
		if (dependencyStatus != DependencyStatus.Available) {
			FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
				dependencyStatus = FirebaseApp.CheckDependencies();
				if (dependencyStatus == DependencyStatus.Available) {
					InitializeFirebase();
				} else {
					Debug.LogError(
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase();
		}
	}

	void InitializeFirebase() {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://vox-voyager-87607159.firebaseio.com/");


		FirebaseDatabase.DefaultInstance.GetReference("Levels").OrderByChild("Date").LimitToLast(10).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError(e2.DatabaseError.Message);
				return;
			}
				
			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
				publicLevels = new List<List<String>>();
				int i = 0;
				foreach (var childSnapshot in e2.Snapshot.Children) {
					publicLevels.Add(new List<String>());
					publicLevels[i].Add(childSnapshot.Child("Name").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Username").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Date").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Downloads").Value.ToString());
					publicLevels[i].Add(childSnapshot.Child("Data").Value.ToString());
					i++;
				}
			}
		};
	}

	public List<List<String>> getLevelList () {
		return publicLevels;
	}
}
