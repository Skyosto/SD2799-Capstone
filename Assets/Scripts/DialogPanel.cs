using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogPanel : MonoBehaviour {
	//Assets in the background
	private TextAsset dialogScript;
	private string pathToDialogScripts = "Event Scripts/";
	private string[] dialogLines;
	private string[] dialogKeys = {
		"~playerName~",
		"#SPKR"
	};

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

	//Testing stuff
	public string charName = "Skyosto";


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
		dialogLines = FilterScript (dialogLines);
	}
	
	// Update is called once per frame
	void Update () {
		//Display the current charater talking
		speakerName.text = characterTalking;

		//If we haven't reached the end of the script yet
		//Display the line
		//TODO Add flags and methods to check if an animation is playing
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

	string[] ParseScript(TextAsset script) {
		string[] parsedScript;
		parsedScript = script.text.Split ('\n');
		parsedScript = FilterScript (parsedScript);
		return parsedScript;
	}

	//TODO Create more filters in order to correct:
	//	Replaceing keys with player created name
	string[] FilterScript (string[] scriptLines) {
		//For each dialog line
		for(int i = 0; i < scriptLines.Length; i++) {
			//Scan for each key
			foreach (string key in dialogKeys) {
				//If it conatins a key check which key it is and filter via
				//Replace or Remove and do something.
				if(scriptLines[i].Contains(key)){
					if (key == dialogKeys[0]) {
						scriptLines[i] = scriptLines[i].Replace(key, charName);
					}
				}
			}
		}
		return scriptLines;
	}

	//Checks to see if the line has a different speaker
	string CheckForSpeaker(string line) {
		//If line contains the #SPKR key
		if (line.Contains(dialogKeys[1])) {//#SPKR 
			//Remove the key
			line = line.Replace(dialogKeys[1], "");
			//Change the talker to the found speaker
			print ("I'm attempting to read the talking character..");
			characterTalking = line.Substring(0,line.IndexOf(':'));
			print (characterTalking);
			//Remove the speaker
			line = line.Replace(characterTalking+":","");
		}
		return line;
	}

	#endregion

	public void DisplayDialogLine() {
		int i = positionInDialogLine;
		//Check if the line contains a different speaker
		dialogLines[stateOfDialogue] = CheckForSpeaker (dialogLines[stateOfDialogue]);

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
