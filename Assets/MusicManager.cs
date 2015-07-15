using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] musicTrackArray;
	public AudioClip[] SoundFXArray;
	static MusicManager instance = null;
	private AudioSource audioSource;
	
	void Awake() {
		Debug.Log ("Music player Awake "+GetInstanceID());
		if (instance != null) {
			Destroy (gameObject);
			print ("Duplicate music player self-destructing");
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PlayMusic(int soundIndex) {
		audioSource.clip = musicTrackArray [soundIndex];
		audioSource.loop = true;
		audioSource.Play ();
	}

	public void PlaySoundFX(int soundIndex, Vector3 position) {
		GameObject soundFXObject = new GameObject ("tempSoundFX");
		AudioSource objectAudioSource = soundFXObject.AddComponent<AudioSource> ();
		soundFXObject.transform.position = position;
		objectAudioSource.clip = SoundFXArray [soundIndex];
		/*
		 * Add volume effects here:
		 * objectAudioSource.volume = (Volume algorithm here);
		 * */
		objectAudioSource.Play ();
		Destroy (soundFXObject, objectAudioSource.clip.length);
	}
}
