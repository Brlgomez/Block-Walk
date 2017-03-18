﻿using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	AudioSource musicSource;

	static bool AudioBegin = false; 

	void Awake() {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource = GetComponent<AudioSource>();
			if (!AudioBegin) {
				musicSource.volume = 0;
				musicSource.Play();
				DontDestroyOnLoad(gameObject);
				AudioBegin = true;
			} 
		}
	}

	public void destroy () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource.Stop();
			AudioBegin = false;
			Destroy(gameObject);
		}
	}

	public void increaseVolume () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource.volume += 0.02f;
		}
	}

	public void setMaxVolume () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource.volume = 1;
		}
	}

	public void decreaseVolume () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource.volume -= 0.02f;
		}
	}

	public void setMinVolume () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource.volume = 0;
		}
	}
}

