using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogPanel : MonoBehaviour {
	//"Views" for the dialogue panel
	//TODO  Obtain the image and animate it whenever waiting for input
	public Text dialogText;
	public Text speakerName;

	//Properties for typing
	private float textSpeedInSeconds = 0.012f;
	private float timeTillNextCharacter;
	private int positionInDialogLine;
	private int stateOfDialogue;
	public bool isWaitingForInput;
	public bool isEndOfScript;
	public string characterTalking;

	private ScriptContainer scriptContainer;


	#region Unity LifeCycle Events
	// Use this for initialization
	void Start () {
		//The dialog panel persists
		//TODO Use Persistent Singleton pattern to prevent unnecissisary copies
		GameObject.DontDestroyOnLoad (gameObject);

		//TODO Fix so the script shows who is talking instead of default me.
		characterTalking = scriptContainer.currentSpeaker;

		//Initialize the timer
		timeTillNextCharacter = textSpeedInSeconds;

		//Initialize typing "cursor" at the beginning
		positionInDialogLine = 0;
		stateOfDialogue = 0;

		//Find the script and parse it into lines to be displayed
		//getScript (Application.loadedLevel);

		//Find the script parser
		scriptContainer = FindObjectOfType<ScriptContainer> ();

		//Start typing script immediately?
		isWaitingForInput = false;
	}

	//When a level is loaded
	void OnLevelWasLoaded(int level) {
		//Find the script and parse it into lines to be displayed
		if (scriptContainer == null) {
			scriptContainer = FindObjectOfType<ScriptContainer> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Display the current charater talking
		speakerName.text = scriptContainer.currentSpeaker;

		//If we haven't reached the end of the script yet
		//Display the line
		//TODO Add flags and methods to check if an animation is playing
		if (stateOfDialogue < scriptContainer.dialogLines.Length && !isWaitingForInput) {
			DisplayDialogLine (true);
		} else if (stateOfDialogue >= scriptContainer.dialogLines.Length) {
			//We've reached the end of the script
			isEndOfScript = true;
		} else {
		}

	}
	#endregion

	#region Getters and Setters
	public int GetStateOfDialogue() {return stateOfDialogue;}
	public void SetStateOfDialogue(int state) {stateOfDialogue = state;}

	public int GetPositionInDialogLine() {return positionInDialogLine;}
	public void SetPositionInDialogLine(int position) {positionInDialogLine = position;}
	#endregion

	#region Script Filters

	//Checks to see if the line has a different speaker
	string CheckForSpeaker(string line) {
		line = scriptContainer.FilterKeyInLine ("#SPKR#", line);
		return line;
	}

	#endregion

	public void DisplayDialogLine(bool typeWrite) {
		int i = positionInDialogLine;
		string[] dialogLines = scriptContainer.dialogLines;
		//Check if the line contains a different speaker
		dialogLines[stateOfDialogue] = CheckForSpeaker (dialogLines[stateOfDialogue]);

		if (typeWrite) {
			if (timeTillNextCharacter <= 0) {
				if (i < dialogLines [stateOfDialogue].Length) {
					isWaitingForInput = false;
					dialogText.text += dialogLines [stateOfDialogue] [i];
					positionInDialogLine++;
					timeTillNextCharacter = textSpeedInSeconds;
				} else {
					isWaitingForInput = true;
				}
			} else {
				timeTillNextCharacter -= Time.deltaTime;
			}
		} else {
			dialogText.text = dialogLines [stateOfDialogue];
			positionInDialogLine = 0;
			timeTillNextCharacter = textSpeedInSeconds;
			isWaitingForInput = true;
		}
	}
}
