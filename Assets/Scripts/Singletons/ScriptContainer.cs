using UnityEngine;
using System.Collections;
using System;

public class ScriptContainer : MonoBehaviour {

	static ScriptContainer instance = null;
	private TextAsset currentScript;

	private string pathToDialogScripts = "Event Scripts/";
	private string[] dialogKeys = {
		"~playerName~",
		"#SPKR#",
		"#STRT_Dialog#",
		"#END_Dialog#",
		"#WAIT#",
		"#PAUSE#",
		"#CHAR-A#",
		"#CHAR-B#",
		"#WHITE_OUT#",
		"#FADE_OUT#"
	};

	public string[] dialogLines;
	public string currentSpeaker = "";

	public string GetCurrentSpeaker() {
		return currentSpeaker;
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	}

	void OnLevelWasLoaded(int level) {
		if (level != 2 && level != 0 && level != 1) {
			LoadScript (level);
			ParseScriptIntoLines ();
			FilterScriptForKeys (dialogLines);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LoadScript(int level) {
		switch (level) {
		case 3:
			currentScript = Resources.Load(pathToDialogScripts+"Introduction") as TextAsset;
			break;
		}
	}

	public bool isScriptAvailable() {
		//Was a script loaded OR finished loading?
		if (currentScript != null) {
			return true;
		}
		//Else
		return false;
	}

	private void ParseScriptIntoLines() {
		dialogLines = currentScript.text.Split ('\n');
	}

	//TODO Create more filters in order to correct:
	//	Replaceing keys with player created name
	private string[] FilterScriptForKeys (string[] scriptLines) {
		//For each dialog line
		for(int i = 0; i < scriptLines.Length; i++) {
			//Scan for each key
			foreach (string key in dialogKeys) {
				//If it conatins a key check which key it is and filter via
				//Replace or Remove and do something.

				/*~playerName~
				 * 		Key is replaced with entered protagonist's name
				 * */
				if(scriptLines[i].Contains(key)){
					if (key == dialogKeys[0]) {
						//scriptLines[i] = scriptLines[i].Replace(key, charName);
					}
				}
			}
		}
		return scriptLines;
	}

	public bool DoesLineContainKey(string line) {
		//Check through our whole list if it contains any keys
		foreach (string key in dialogKeys) {
			bool lineContainsKey = line.Contains(key);
			if (lineContainsKey) {
				return true;
			}
		}
		//If the line didn't contain a key
		return false;
	}

	public string GetKeyInLine(string line) {
		foreach (string key in dialogKeys) {
			if(line.Contains(key)) {
				return key;
			}
		}
		Debug.LogWarning ("No key found using GetKeyInLine(). Returning null.");
		return null;
	}

	public string FilterKeyInLine(string key, string line) {

		if (key == dialogKeys [1]) {//#SPKR 
			line = line.Replace (dialogKeys [1], "");
			currentSpeaker = line.Substring (0, line.IndexOf (':'));
			line = line.Replace (currentSpeaker + ":", "");

		} 
		else if (key == dialogKeys [2]) { //#STRT_Dialog#
			line = line.Replace(dialogKeys[2], "");
		}
		else if (key == dialogKeys [3]) { //#END_Dialog#
			line = line.Replace(dialogKeys[3], "");
		}
		else if (key == dialogKeys[4]) { //#WAIT#
			float time;
			//Remove key
			line = line.Replace(dialogKeys[4], "");
			string timeString = line.Substring(0,line.IndexOf('$'));
			time = Convert.ToSingle(timeString);
			line = line.Replace(timeString+"$","");
			EventManager.waitTime = time;
		}
		else if (key == dialogKeys [5]) { //#PAUSE#
			line = line.Replace(dialogKeys[5], "");
		}
		else if (key == dialogKeys [8]) { //#WHITE_OUT#
			line = line.Replace(dialogKeys[8], "");
		}
		else if (key == dialogKeys [9]) { //#PAUSE#
			line = line.Replace(dialogKeys[9], "");
		}
		else {
			Debug.LogError(key+" is not valid a valid dialog key.");
		}
		return line;
	}
}
