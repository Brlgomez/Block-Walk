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
	public AudioClip rotateRight;
	public AudioClip rotateLeft;
	public AudioClip explosion;
	public AudioClip fuse;

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

	public void playEraserSound(Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(eraser, blockPos);
			sound.pitch = Random.Range(0.75f, 1.25f);
		}
	}

	public void playDropBlockSound(Vector3 blockPos, float pitch, string name) {
		if (playSoundEffects == 0) {
			AudioSource sound;
			if (name == VariableManagement.multistepBlock + VariableManagement.clone) {
				sound = playClipAt(multistepBlock, blockPos);
			} else if (name == VariableManagement.switchBlock + VariableManagement.clone) {
				sound = playClipAt(offSwitch, blockPos);
			} else if (name == VariableManagement.rotateLBlock + VariableManagement.clone) {
				sound = playClipAt(rotateLeft, blockPos);
			} else if (name == VariableManagement.rotateRBlock + VariableManagement.clone) {
				sound = playClipAt(rotateRight, blockPos);
			} else if (name == VariableManagement.bombBlock + VariableManagement.clone) {
				sound = playClipAt(fuse, blockPos);
			} else {
				sound = playClipAt(blockWalk, blockPos);
			}
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playBeatLevelSound() {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(beatLevel, transform.position);
			sound.pitch = Random.Range(0.8f, 1.2f);
		}
	}

	public void playLoseLevelSound() {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(loseLevel, transform.position);
			sound.pitch = Random.Range(0.8f, 1.2f);
		}
	}

	public void playOnSwitchSound(float pitch, Vector3 pos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(onSwitch, pos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playOffSwitchSound(float pitch, Vector3 pos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(offSwitch, pos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playBlockWalkSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(blockWalk, blockPos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playMultiStepSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(multistepBlock, blockPos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playRotateRightSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(rotateRight, blockPos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playRotateLeftSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(rotateLeft, blockPos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playExplosionSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(explosion, blockPos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playFuseSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(fuse, blockPos);
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	AudioSource playClipAt(AudioClip clip, Vector3 pos) {
		GameObject tempGO = new GameObject("TempAudio");
		tempGO.transform.position = pos;
		tempGO.AddComponent<AudioSource>();
		AudioSource aSource = tempGO.GetComponent<AudioSource>();
		aSource.clip = clip;
		aSource.Play();
		Destroy(tempGO, clip.length); 
		return aSource; 
	}

	public float getPitchOfBlock (GameObject block) {
		float pitch = (
			block.GetComponent<Renderer>().material.color.r +
			block.GetComponent<Renderer>().material.color.g +
			block.GetComponent<Renderer>().material.color.b
		);
		pitch = Mathf.Clamp(pitch, 0.25f, 3);
		return pitch;
	}
}
