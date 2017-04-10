using UnityEngine;
using System.Collections;

public class MusicVolumeUp : MonoBehaviour {

	GameObject music;
	bool increase;

	void Start () {
		music = GameObject.Find("Music");
	}

	void Update () {
		if (increase) {
			music.GetComponent<MusicManager>().increaseVolume();
			if (music.GetComponent<MusicManager>().getVolume() >= 1 || PlayerPrefs.GetInt(VariableManagement.playMusic) == 1) {
				Destroy(GetComponent<MusicVolumeUp>());
			}
		} else {
			music.GetComponent<MusicManager>().decreaseVolume();
			if (music.GetComponent<MusicManager>().getVolume() <= 0 || PlayerPrefs.GetInt(VariableManagement.playMusic) == 1) {
				Destroy(GetComponent<MusicVolumeUp>());
			}
		}
	}

	public void direction (bool dir) {
		increase = dir;
	}
}
