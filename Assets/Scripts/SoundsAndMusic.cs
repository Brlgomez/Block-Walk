using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundsAndMusic : MonoBehaviour {

	public AudioClip button;
	public AudioClip nextItem;
	public AudioClip warning;
	public AudioClip trash;
	public AudioClip success;
	public AudioClip unlock;

	private AudioSource source;

	int playSoundEffects;

	void Awake () {
		source = GetComponent<AudioSource>();
		source.pitch = 1;
		playSoundEffects = PlayerPrefs.GetInt(VariableManagement.playSounds, 0);
	}

	public void updatePlaySounds () {
		playSoundEffects = PlayerPrefs.GetInt(VariableManagement.playSounds, 0);
	}

	public void playButtonSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (button);
		}
	}

	public void playNextItemSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (nextItem);
		}
	}

	public void playWarningSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (warning);
		}
	}

	public void playTrashSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (trash);
		}
	}

	public void playSuccessSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (success);
		}
	}

	public void playUnlockSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (unlock);
		}
	}
}
