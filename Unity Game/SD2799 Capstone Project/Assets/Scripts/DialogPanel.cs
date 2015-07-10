using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogPanel : MonoBehaviour {
	//Assets in the background
	private TextAsset dialogScript;
	private string pathToDialogScripts = "Event Scripts/";
	private string[] dialogLines;

	//"Views" for the dialogue panel
	public Text dialogText;
	public Text speakerName;

	//Properties for typing
	private float textSpeedInSeconds = 0.012f;
	private float timeTillNextCharacter;
	private int positionInDialogLine;
	private int stateOfDialogue;
	public bool isWaitingForInput;
	public bool isEndOfScript;

	//Testing stuff
	public string charName = "Skyosto";
	public string characterTalking;

	#region Unity LifeCycle Events
	// Use this for initialization
	void Start () {
		//The dialog panel persists
		//TODO Use Persistent Singleton pattern to prevent unnecissisary copies
		GameObject.DontDestroyOnLoad (gameObject);

		//TODO Fix so the script shows who is talking instead of default me.
		characterTalking = charName;

		//Initialize the timer
		timeTillNextCharacter = textSpeedInSeconds;

		//Initialize typing "cursor" at the beginning
		positionInDialogLine = 0;
		stateOfDialogue = 0;

		//Start typing script immediately?
		isWaitingForInput = false;
	}

	//When a level is loaded
	void OnLevelWasLoaded(int level) {
		//Find the script and parse it into lines to be displayed
		getScript (Application.loadedLevel);
		dialogLines = ParseScript (dialogScript);
	}
	
	// Update is called once per frame
	void Update () {
		//Display the current charater talking
		//TODO Create method to correct find current character talking
		speakerName.text = characterTalking;

		//If we haven't reached the end of the script yet
		//Display the line
		//TODO Add flags and methods to check if an animation is playin
		if (stateOfDialogue < dialogLines.Length && !isWaitingForInput) {
			DisplayDialogLine ();
		} else if (stateOfDialogue >= dialogLines.Length) {
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

	private void getScript(int levelIndex) {
		switch (levelIndex) {
		case 1:
			dialogScript = Resources.Load(pathToDialogScripts+"Introduction") as TextAsset;
			break;
		}
	}
	#endregion

	#region Script Filters
	//TODO Create more filters in order to correct:
	//	Show people talking
	//	Replaceing keys with player created name
	string[] ParseScript(TextAsset script) {
		string[] parsedScript;
		parsedScript = script.text.Split ('\n'); 
		return parsedScript;
	}
	#endregion

	public void DisplayDialogLine() {
		int i = positionInDialogLine;
		if (timeTillNextCharacter <= 0) {
			if(i < dialogLines [stateOfDialogue].Length) {
				isWaitingForInput = false;
				dialogText.text += dialogLines [stateOfDialogue] [i];
				positionInDialogLine++;
				timeTillNextCharacter = textSpeedInSeconds;
			}
			else {
				isWaitingForInput = true;
			}
		} else {
			timeTillNextCharacter -= Time.deltaTime;
		}
	}

	public void DisplayDialogLine(bool typeWrite) {
		if (typeWrite == false) {
			dialogText.text = dialogLines [stateOfDialogue];
			positionInDialogLine = 0;
			timeTillNextCharacter = textSpeedInSeconds;
			isWaitingForInput = true;
		} else {
			DisplayDialogLine();
		}
	}
}
