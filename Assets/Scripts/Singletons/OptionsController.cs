using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsController : MonoBehaviour {

	MusicManager musicManager;
	AudioSource musicManagerAudioSource;

	public Slider MasterVolumeSlider;
	public Slider MusicVolumeSlider;
	public Slider SoundFXVolumeSlider;
	public Slider TextSpeedSlider;

	public EventManager eventManager;
	static OptionsController instance = null;

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
		musicManager = FindObjectOfType<MusicManager> ();
		musicManagerAudioSource = musicManager.GetComponent<AudioSource> ();

		eventManager = FindObjectOfType<EventManager> ();
	}

	void OnLevelWasLoaded(int level) {
		if (level == 2) {
			MasterVolumeSlider = GameObject.Find ("Canvas/Master Volume Slider").GetComponent<Slider>();
			MusicVolumeSlider = GameObject.Find ("Canvas/Music Slider").GetComponent<Slider>();
			SoundFXVolumeSlider = GameObject.Find ("Canvas/Sound Effects Slider").GetComponent<Slider>();
			TextSpeedSlider = GameObject.Find ("Canvas/Text Speed Slider").GetComponent<Slider>();

			MasterVolumeSlider.value = PlayerPrefsManager.GetMasterVolume();
			MusicVolumeSlider.value = PlayerPrefsManager.GetMusicVolume();
			SoundFXVolumeSlider.value = PlayerPrefsManager.GetSoundFXVolume();
			TextSpeedSlider.value = PlayerPrefsManager.GetTextSpeed();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//TODO Change this value to the correct one for the Options Menu
		if(Application.loadedLevel == 2){
			musicManager.AdjustMusicVolume ((MusicVolumeSlider.value) * (MasterVolumeSlider.value));
		}
	}

	public void SaveAndExit () {
		MasterVolumeSlider = GameObject.Find ("Canvas/Master Volume Slider").GetComponent<Slider>();
		MusicVolumeSlider = GameObject.Find ("Canvas/Music Slider").GetComponent<Slider>();
		SoundFXVolumeSlider = GameObject.Find ("Canvas/Sound Effects Slider").GetComponent<Slider>();
		TextSpeedSlider = GameObject.Find ("Canvas/Text Speed Slider").GetComponent<Slider>();
		
		PlayerPrefsManager.SetMasterVolume(MasterVolumeSlider.value);
		PlayerPrefsManager.SetMusicVolume(MusicVolumeSlider.value);
		PlayerPrefsManager.SetSoundFXVolume(SoundFXVolumeSlider.value);
		PlayerPrefsManager.SetTextSpeed(TextSpeedSlider.value);

		eventManager.LoadLevel ("01a Start Menu");
	}

	public void ResetToDefaults () {
		MasterVolumeSlider = GameObject.Find ("Canvas/Master Volume Slider").GetComponent<Slider>();
		MusicVolumeSlider = GameObject.Find ("Canvas/Music Slider").GetComponent<Slider>();
		SoundFXVolumeSlider = GameObject.Find ("Canvas/Sound Effects Slider").GetComponent<Slider>();
		TextSpeedSlider = GameObject.Find ("Canvas/Text Speed Slider").GetComponent<Slider>();

		MasterVolumeSlider.value = 1f;
		MusicVolumeSlider.value = 1f;
		SoundFXVolumeSlider.value = 1f;
		TextSpeedSlider.value = 3f;
	}
}
