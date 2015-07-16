using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	private const string MASTER_VOLUME_KEY = "master_volume";
	private const string TEXT_SPEED_KEY = "text_speed";
	private const string MUSIC_VOLUME_KEY = "music_volume";
	private const string SOUND_FX_VOLUME_KEY = "sound_fx_volume";

	#region Volumes
	public static float GetMasterVolume() {
		return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
	}
	public static void SetMasterVolume(float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Volume out of range");
		}
	}
	public static float GetMusicVolume() {
		return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
	}
	
	public static void SetMusicVolume(float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MUSIC_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Volume out of range");
		}
	}

	public static float GetSoundFXVolume() {
		return PlayerPrefs.GetFloat(SOUND_FX_VOLUME_KEY);
	}
	public static void SetSoundFXVolume(float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (SOUND_FX_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Volume out of range");
		}
	}

	#endregion

	public static float GetTextSpeed() {
		return PlayerPrefs.GetFloat(TEXT_SPEED_KEY);
	}
	public static void SetTextSpeed(float speed) {
		if (speed >= 0f && speed <= 3f) {
			speed = Mathf.Round(speed);
			PlayerPrefs.SetFloat (TEXT_SPEED_KEY, speed);
		}
	}
}
