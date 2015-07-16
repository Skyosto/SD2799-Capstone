using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviour {

	public static bool isWaitingForInput;
	public static bool isEndOfScript;

	public DialogPanel dialogPanel;
	public Text dialogText;

	MusicManager musicManager;
	static EventManager instance;

	#region Unity LifeCycle Events
	// Use this for initialization
	void Start () {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}

		musicManager = FindObjectOfType<MusicManager> ();
	}


	void OnLevelWasLoaded(int level) {
		if(level != 2 && level != 0) {
			dialogPanel = FindObjectOfType<DialogPanel>();
			dialogText = dialogPanel.dialogText;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	#endregion

	public void progressEvent() {
			if (EventManager.isEndOfScript) {
				//Move to next scene
			}
			else if (EventManager.isWaitingForInput) {
				dialogText.text = "";
				dialogPanel.SetStateOfDialogue (dialogPanel.GetStateOfDialogue () + 1);;
				EventManager.isWaitingForInput = false;
			} else {
				dialogPanel.DisplayDialogLine (false);
			}
	}

	public void LoadLevel(string name) {
		Application.LoadLevel (name);
	}

	public void LoadNextLevel() {
		Application.LoadLevel(Application.loadedLevel + 1);
	}

}