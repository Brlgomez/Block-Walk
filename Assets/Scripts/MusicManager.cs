using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	AudioSource musicSource;

	static bool AudioBegin = false; 

	void Awake() {
		if (PlayerPrefs.GetInt(VariableManagement.playMusic) == 0) {
			musicSource = GetComponent<AudioSource>();
			if (!AudioBegin) {
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
}

