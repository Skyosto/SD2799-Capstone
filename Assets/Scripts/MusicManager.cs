using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] musicTrackArray;
	public AudioClip[] SoundFXArray;
	static MusicManager instance = null;
	private AudioSource audioSource;

	public const string MASTER_SOUND = "master_sound";
	public const string MUSIC_SOUND = "music_sound";
	public const string SOUND_FX = "sound_fx";
	public const string AMBIENCE = "ambience_sound";
	
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		PlayMusic (0);
	}

	void OnLevelWasLoaded(int level) {
		switch (level) {
		case 0:
			PlayMusic(0);
			break;
		case 2:
			PlayMusic(1);
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PlayMusic(int soundIndex) {
		audioSource.clip = musicTrackArray [soundIndex];
		audioSource.volume = CalculateVolume (MUSIC_SOUND); 
		audioSource.loop = true;
		audioSource.Play ();
	}

	public void PlaySoundFX(int soundIndex) {
		if (SoundFXArray [soundIndex]) {
			GameObject soundFXObject = new GameObject ("tempSoundFX");
			AudioSource objectAudioSource = soundFXObject.AddComponent<AudioSource> ();
			objectAudioSource.clip = SoundFXArray [soundIndex];
			objectAudioSource.volume = CalculateVolume(SOUND_FX);
			objectAudioSource.Play ();
			Destroy (soundFXObject, objectAudioSource.clip.length);
		} else {
			Debug.LogError("Audio clip not found.");
		}
	}

	public void AdjustMusicVolume(float volume) {
		audioSource.volume = volume;
	}

	float CalculateVolume (string volumeType) {
		float volume = 1;
		if (volumeType == MUSIC_SOUND) {
			volume = (PlayerPrefsManager.GetMusicVolume()) * (PlayerPrefsManager.GetMasterVolume());
			return volume;
		} else if (volumeType == SOUND_FX) {
			volume = (PlayerPrefsManager.GetSoundFXVolume()) * (PlayerPrefsManager.GetMasterVolume());
			return volume;
		} else if (volumeType == AMBIENCE) {
			// There is not setting for ambience.... yet.
		} else {
			Debug.LogError("Volume type not found. Reseting volume to max.");
		}
		return volume;

	}
}
