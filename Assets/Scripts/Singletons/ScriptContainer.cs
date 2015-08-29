using UnityEngine;
using System.Collections;
using System;

public class ScriptContainer : MonoBehaviour {

	static ScriptContainer instance = null;
	private TextAsset currentScript;
	CutsceneController cutsceneController;
	public GameObject musicManager;

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
		"#WHITE_IN#",
		"#BLACK_IN#",
		"#BLACK_OUT#",
		"#ORP#",
		"#SPAWN#",
		"#REQUIRES#",
		"#PLAY_MUSIC#"
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
		FindSingletons ();
	}

	void FindSingletons() {
		cutsceneController = FindObjectOfType<CutsceneController> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LoadScript(int level) {
		switch (level) {
		case 3:
			currentScript = Resources.Load(pathToDialogScripts+"Introduction") as TextAsset;
			break;
		case 4:
			currentScript = Resources.Load(pathToDialogScripts+"Rizmas_Bedroom") as TextAsset;
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

		} else if (key == dialogKeys [2]) { //#STRT_Dialog#
			line = line.Replace (dialogKeys [2], "");
		} else if (key == dialogKeys [3]) { //#END_Dialog#
			line = line.Replace (dialogKeys [3], "");
		} else if (key == dialogKeys [4]) { //#WAIT#
			float time;
			//Remove key
			line = line.Replace (dialogKeys [4], "");
			string timeString = line.Substring (0, line.IndexOf ('$'));
			time = Convert.ToSingle (timeString);
			line = line.Replace (timeString + "$", "");
			EventManager.waitTime = time;
		} else if (key == dialogKeys [5]) { //#PAUSE#
			line = line.Replace (dialogKeys [5], "");
		} else if (key == dialogKeys [8]) { //#WHITE_OUT#
			line = line.Replace (dialogKeys [8], "");
		} else if (key == dialogKeys [9]) { //#WHITE_IN#
			line = line.Replace (dialogKeys [9], "");
		} else if (key == dialogKeys [10]) { //#BLACK_IN#
			line = line.Replace (dialogKeys [10], "");
		} else if (key == dialogKeys [11]) { //#BLACK_OUT#
			line = line.Replace (dialogKeys [11], "");
		} else if (key == dialogKeys [12]) { //#ORP#
			line = line.Replace (dialogKeys [12], "");
			Debug.Log ("Found Orp key.");
			if(line.Contains("&CHANGE&")) {
				Debug.Log ("Found change for orp key.");
				line = line.Replace ("&CHANGE&", "");
				if(line.Contains("%shovel%")){
					Debug.Log ("Tell to change to shovel.");
					line = line.Replace ("%shovel%", "");
					CutsceneController.orpForm = "shovel";
				}
			}
		} else if (key == dialogKeys [13]) { //#SPAWN# 
			line = line.Replace (dialogKeys [13], "");
			String whatToSpawn = "";
			Vector3 position;
			Debug.Log ("Found Spawn key.");
			if(line.Contains("&OBJECT&")) {
				Debug.Log ("Spawning object.");
				line = line.Replace ("&OBJECT&", "");
				if(line.Contains("%shed%")){
					Debug.Log ("Spawning shed.");
					whatToSpawn = "shed";
					line = line.Replace ("%shed%", "");
				}
				if(line.Contains("%turtleEggs%")){
					Debug.Log ("Spawning turtle eggs.");
					whatToSpawn = "turtleEggs";
					line = line.Replace ("%turtleEggs%", "");
				}
			}
			if(line.Contains("&ITEM&")) {
				Debug.Log ("Spawning item.");
				line = line.Replace ("&ITEM&", "");
				if(line.Contains("%shovel%")){
					Debug.Log ("Spawning shovel.");
					whatToSpawn = "shovel";
					line = line.Replace ("%shovel%", "");
				}
			}
			String[] coordinates = line.Split(',');
			float coordinateX = Convert.ToSingle(coordinates[0]);
			float coordinateY = Convert.ToSingle(coordinates[1]);
			position = new Vector3(coordinateX, coordinateY);
			if(whatToSpawn != "" && position != null) {
				cutsceneController.SpawnObject(whatToSpawn, position);
			}
			else {
				Debug.LogError("Could not spawn an object, please check your perameters.");
			}
		} else if (key == dialogKeys [14]) { //#REQUIRES#
			line = line.Replace (dialogKeys [14], "");
		} else if (key == dialogKeys [15]) { //#PLAY_MUSIC#
			line = line.Replace (dialogKeys [15], "");
			musicManager = GameObject.Find ("Music Manager");
			MusicManager musicManagerScript = musicManager.GetComponent<MusicManager>();
			if(line == "Meeting Orp") {
				musicManagerScript.PlayMusic(3);
			}
		}
		else {
			Debug.LogError(key+" is not valid a valid dialog key.");
		}
		return line;
	}
}
