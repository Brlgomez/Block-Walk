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
	public AudioClip eraser;
	public AudioClip dropBlock;
	public AudioClip beatLevel;
	public AudioClip loseLevel;
	public AudioClip onSwitch;
	public AudioClip offSwitch;
	public AudioClip blockWalk;
	public AudioClip multistepBlock;

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
			source.pitch = 1;
			source.PlayOneShot (button);
		}
	}

	public void playNextItemSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (nextItem);
		}
	}

	public void playWarningSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (warning);
		}
	}

	public void playTrashSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (trash);
		}
	}

	public void playSuccessSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (success);
		}
	}

	public void playUnlockSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (unlock);
		}
	}

	public void playEraserSound() {
		if (playSoundEffects == 0) {
			source.Stop();
			source.pitch = Random.Range(0.75f, 1.25f);
			source.PlayOneShot (eraser);
		}
	}

	public void playDropBlockSound() {
		if (playSoundEffects == 0) {
			source.Stop();
			source.pitch = Random.Range(0.5f, 1.5f);
			source.PlayOneShot (dropBlock);
		}
	}

	public void playBeatLevelSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (beatLevel);
		}
	}

	public void playLoseLevelSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (loseLevel);
		}
	}

	public void playOnSwitchSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (onSwitch);
		}
	}

	public void playOffSwitchSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (offSwitch);
		}
	}

	public void playBlockWalkSound(float pitch) {
		if (playSoundEffects == 0) {
			source.Stop();
			source.pitch = pitch + Random.Range(-0.01f, 0.01f);
			source.PlayOneShot (blockWalk);
		}
	}

	public void playMultiStepSound(float pitch) {
		if (playSoundEffects == 0) {
			source.Stop();
			source.pitch = pitch + Random.Range(-0.01f, 0.01f);
			source.PlayOneShot (multistepBlock);
		}
	}
}
