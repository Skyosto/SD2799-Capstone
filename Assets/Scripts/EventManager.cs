using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviour {

	static EventManager instance;
	public int scriptLineNumber;

	#region Flags
	public static bool isWaitingForInput;
	public static bool isEndOfScript;
	#endregion
	#region Components
	public DialogPanel dialogPanel;
	public Text dialogText;
	public MusicManager musicManager;
	public ScriptContainer scriptContainer;
	#endregion	
	#region WAIT timers
	public static float waitTime;
	float timeWaited;
	#endregion

	#region Unity LifeCycle Events
	// Use this for initialization
	void Start () {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}


	void OnLevelWasLoaded(int level) {
		if(level != 2 && level != 0) {
			//dialogPanel = FindObjectOfType<DialogPanel>();
			//dialogText = dialogPanel.dialogText;
			scriptLineNumber = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		ReadEventLine ();
	}
	#endregion

	public void progressEvent() {
		print ("Trying to execute.");
			if (isEndOfScript) {
				//Move to next scene
				print ("End of script was true?");
			}
			else if (isWaitingForInput) {
			print ("I was waiting for input.");
				dialogText.text = "";
				dialogPanel.SetStateOfDialogue (dialogPanel.GetStateOfDialogue () + 1);;
				isWaitingForInput = false;
			} else {
			print ("Ending the line automatically.");
				dialogPanel.DisplayDialogLine (false);
			}
	}

	public void LoadLevel(string name) {
		Application.LoadLevel (name);
	}

	public void LoadNextLevel() {
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	void ReadEventLine() {
		string currentLine = scriptContainer.dialogLines [scriptLineNumber];
		if (currentLine != null && currentLine != "") {
			string key = scriptContainer.GetKeyInLine (scriptContainer.dialogLines [scriptLineNumber]);
			switch(key) {
			case "#WAIT#":
				currentLine = scriptContainer.FilterKeyInLine("#WAIT#",currentLine);
				while(timeWaited < waitTime) {
					timeWaited += Time.deltaTime;
				}
				waitTime = 0;
				timeWaited = 0;
					break;
			}
		} else {
			dialogPanel.CheckForSpeaker(currentLine);
		}
	}


}