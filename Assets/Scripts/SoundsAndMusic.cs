﻿using UnityEngine;
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
	public AudioClip destroyed;
	public AudioClip normalWeight;
	public AudioClip lightWeight;
	public AudioClip heavyWeight;

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
			source.volume = 1;
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

	public void playDestroyedSound() {
		if (playSoundEffects == 0) {
			source.volume = 1;
			source.pitch = 1;
			source.PlayOneShot (destroyed);
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
			} else if (name == VariableManagement.resizeBigBlock + VariableManagement.clone) {
				sound = playClipAt(heavyWeight, blockPos);
			} else if (name == VariableManagement.resizeNormalBlock + VariableManagement.clone) {
				sound = playClipAt(normalWeight, blockPos);
			} else if (name == VariableManagement.resizeSmallBlock + VariableManagement.clone) {
				sound = playClipAt(lightWeight, blockPos);
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
			sound.volume = setVolume();
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playOffSwitchSound(float pitch, Vector3 pos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(offSwitch, pos);
			sound.volume = setVolume();
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playBlockWalkSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(blockWalk, blockPos);
			sound.volume = setVolume();
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playMultiStepSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(multistepBlock, blockPos);
			sound.volume = setVolume();
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playRotateRightSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(rotateRight, blockPos);
			sound.volume = setVolume();
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playRotateLeftSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(rotateLeft, blockPos);
			sound.volume = setVolume();
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playExplosionSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(explosion, blockPos);
			sound.volume = setVolume();
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playFuseSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(fuse, blockPos);
			sound.volume = setVolume();
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

	public void playNormalWeightSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(normalWeight, blockPos);
			sound.volume = 1;
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playLightWeightSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(lightWeight, blockPos);
			sound.volume = 1;
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	public void playHeavyWeightSound(float pitch, Vector3 blockPos) {
		if (playSoundEffects == 0) {
			AudioSource sound = playClipAt(heavyWeight, blockPos);
			sound.volume = 1;
			sound.pitch = pitch + Random.Range(-0.025f, 0.025f);
		}
	}

	float setVolume () {
		if (GetComponent<CharacterMovement>() != null) {
			if (GetComponent<CharacterMovement>().getPlayerSize() > 0) {
				return 1.5f;
			} else if (GetComponent<CharacterMovement>().getPlayerSize() < 0) {
				return 0.1f;
			}
			return 1;
		} else {
			return 1;
		}
	}
}
