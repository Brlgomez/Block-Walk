using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	AudioSource musicSource;

	static bool AudioBegin = false; 

	void Awake() {
		setMusic();
	}

	void setMusic () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource = GetComponent<AudioSource>();
			if (!AudioBegin) {
				musicSource.volume = 0;
				musicSource.time = Random.Range(0, musicSource.clip.length);
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

	public void turnOffOnMusic () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			if (AudioBegin) {
				musicSource.Stop();
			}
			PlayerPrefs.SetInt(VariableManagement.playMusic, 1);
		} else {
			PlayerPrefs.SetInt(VariableManagement.playMusic, 0);
			if (AudioBegin) {
				musicSource.Play();
			} else {
				setMusic();
			}
			musicSource.volume = 1;
		}
	}

	public void increaseVolume () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource.volume += 0.02f;
		}
	}

	public void decreaseVolume () {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource.volume -= 0.02f;
		}
	}
}

