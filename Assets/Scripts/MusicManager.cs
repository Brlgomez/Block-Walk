using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	AudioSource musicSource;

	static bool AudioBegin = false; 

	void Awake() {
		musicSource = GetComponent<AudioSource>();
		if (!AudioBegin) {
			musicSource.Play ();
			DontDestroyOnLoad (gameObject);
			AudioBegin = true;
		} 
	}

	public void destroy () {
		musicSource.Stop();
		AudioBegin = false;
		Destroy(gameObject);
	}
}

