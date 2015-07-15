using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	private string MASTER_VOLUME_KEY = "master_volume";
	private string TEXT_SPEED_KEY = "text_speed";
	private string MUSIC_VOLUME_KEY = "music_volume";
	private string SOUND_FX_VOLUME_KEY = "sound_fx_volume";

	#region Volumes
	public float GetMasterVolume() {
		return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
	}
	public void SetMasterVolume(float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Volume out of range");
		}
	}
	public float GetMusicVolume() {
		return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
	}
	
	public void SetMusicVolume(float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MUSIC_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Volume out of range");
		}
	}

	public float GetSoundFXVolume() {
		return PlayerPrefs.GetFloat(SOUND_FX_VOLUME_KEY);
	}
	public void SetSoundFXVolume(float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (SOUND_FX_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Volume out of range");
		}
	}

	#endregion

	public float GetTextSpeed() {
		return PlayerPrefs.GetFloat(TEXT_SPEED_KEY);
	}
	public void SetTextSpeed(float speed) {
		if (speed >= 0f && speed <= 3f) {
			speed = Mathf.Round(speed);
			PlayerPrefs.SetFloat (TEXT_SPEED_KEY, speed);
		}
	}
}
