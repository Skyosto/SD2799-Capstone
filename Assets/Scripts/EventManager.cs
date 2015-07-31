using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviour {

	static EventManager instance;
	public static int scriptLineNumber = 0;
	string currentLine;

	#region Flags
	public static bool isInMainMenus;
	public static bool isWaitingForInput;
	public static bool isEndOfScript;
	public static bool isWaitingForTimer;
	public static bool isScriptPaused;
	public static bool isGamePaused;
	public static bool playerHasControl;
	public static bool isDialogTyping;
	public static bool animationIsPlaying;
	#endregion
	#region Components & GameObjects
	public DialogPanel dialogPanel;
	public Text dialogText;
	public MusicManager musicManager;
	public static ScriptContainer scriptContainer;
	public GameObject fadePanel;
	public CutsceneController cutsceneController;
	#endregion	
	#region WAIT timers
	public static float waitTime;
	float timeWaited;
	#endregion

	#region Unity LifeCycle Events
	// Use this for initialization
	void Awake () {
		//Start the singleton pattern
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}

		InitializeFlagDefaults ();
		FindSingletons ();
	}


	void OnLevelWasLoaded(int level) {

		InitializeFlagDefaults ();
		FindSingletons ();

		//Find the dialog panel in this level. Then hide it from sight.
		if(dialogPanel != null) {
			dialogPanel.gameObject.SetActive (false);
		}
	}

	void InitializeFlagDefaults() {
		//Check to see if the scene we are in is a "Main Menu"
		CheckIfInMainMenus ();

		scriptLineNumber = 0;

		//Set all the other flags to not active (these are flipped when certain thigns happen...
		isScriptPaused = false;
		playerHasControl = false;
		isWaitingForTimer = false;
		isWaitingForInput = false;
		isDialogTyping = false;
		isEndOfScript = false;
		animationIsPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("The current line number is: "+ scriptLineNumber);
		CheckForScriptEnd ();
		
		//Are we in a main menu?
		if (!isInMainMenus && scriptContainer.isScriptAvailable ()) {
			//Is the scrip active in executing?
			if (!isScriptPaused) {
				//Does the player have control over their character?
				if(!playerHasControl) {
					//Are we waiting for any wait timers?
					if (!isWaitingForTimer) {
						//Are we waiting for the player to tap the dialog box?
						if (!isWaitingForInput) {
							//Is an animation currently playing?
							if(!animationIsPlaying){
								if(dialogPanel != null){
									dialogPanel.PlayDialogArrowAnimation(false);
								}
								//Is the dialog panel currently typing dialog?
								if(isEndOfScript) {
									Debug.Log ("We have reached the end of the script. Load next level.");
									return;
								}
								if (!isDialogTyping) {
									ReadEventLine ();
									if(!isDialogTyping && !isEndOfScript) {
										scriptLineNumber++;
									}
									//
								} else {
									Debug.Log("Dialog is being typed. Stopping this line of execution.");
									dialogPanel.DisplayDialogLine (currentLine);
								}
							} else {
								Debug.Log ("Cutscene or another animation is currently playing. Stopping this " +
									"line of execution.");
							}
						} else {
							Debug.Log ("Waiting for user to tap dialog box.");
							if(dialogPanel != null) {
								dialogPanel.PlayDialogArrowAnimation(true);
							}
						}
					} else {
						Debug.Log ("Waiting for wait timer to expire..");
					}
				} else {
					Debug.Log("Player currently has control over character. Stopping this line of execution.");
				}
			} else {
				Debug.Log ("Script is paused...");
			}
		} else {
			Debug.Log ("We are in a main menu, or the script is unavailable.");
		}
	}
	#endregion

	public void progressEvent() {
		DialogPanel dialogBox = FindObjectOfType<DialogPanel>();
		if (isWaitingForInput) {
			dialogBox.ClearDialogBox ();
			scriptLineNumber++;
			isWaitingForInput = false;
		} else {
			if (!isWaitingForTimer && !animationIsPlaying) {
				print ("Ending dialog line automatically.");
				currentLine = scriptContainer.dialogLines [scriptLineNumber];
				dialogBox.DisplayDialogLine (false, currentLine);
			}
		}
	}

	void CheckIfInMainMenus() {
		int[] MainMenus = {
			0,
			2
		};
		for (int i = 0; i < MainMenus.Length; i++) {
			if(MainMenus[i] == Application.loadedLevel) {
				isInMainMenus = true;
				break;
			}
			else if (i >= MainMenus.Length - 1) {
				isInMainMenus = false;
			}
		}
		Debug.Log ("I'm in a main menu: "+isInMainMenus);

	}

	void CheckForScriptEnd() {
		if (scriptLineNumber >= scriptContainer.dialogLines.Length) {
			isEndOfScript = true;
		}
	}

	void FindSingletons () {
		musicManager = FindObjectOfType<MusicManager> ();
		dialogPanel = FindObjectOfType<DialogPanel> ();
		scriptContainer = FindObjectOfType<ScriptContainer> ();
		fadePanel = GameObject.Find ("Fade Panel");
		cutsceneController = FindObjectOfType<CutsceneController> ();
	}

	public void LoadLevel(string name) {
		Application.LoadLevel (name);
	}

	public void LoadNextLevel() {
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	void ReadEventLine() {
		currentLine = scriptContainer.dialogLines [scriptLineNumber];
		CheckForScriptEnd();

		Debug.Log ("Line before key checking:\n"+currentLine);
		if (scriptContainer.DoesLineContainKey (currentLine)) {
			string key = scriptContainer.GetKeyInLine (scriptContainer.dialogLines [scriptLineNumber]);
			ReactToKey (key);
			Debug.Log ("Line after key checking:\n" + currentLine);
		}
		else {
			dialogPanel.DisplayDialogLine (currentLine);
		}
	}

	void ReactToKey(string key) {
		switch (key) {
		case "#FADE_IN#":
			Debug.Log ("Fading into scene.");
			break;
		case "#WAIT#":
			isWaitingForTimer = true;
			currentLine = scriptContainer.FilterKeyInLine (key, currentLine);
			StartCoroutine (WaitTimer ());
			break;
		case "#SPKR#":
			Debug.Log ("Replacing speaker...");
			currentLine = scriptContainer.FilterKeyInLine (key, currentLine);
			dialogPanel.UpdateSpeaker ();
			break;
		case "#STRT_Dialog#":
			Debug.Log ("Bringing up the DialogPanel.");
			currentLine = scriptContainer.FilterKeyInLine (key, currentLine);
			dialogPanel.gameObject.SetActive (true);
			break;
		case "#END_Dialog#":
			Debug.Log ("Closing the DialogPanel.");
			currentLine = scriptContainer.FilterKeyInLine (key, currentLine);
			dialogPanel.gameObject.SetActive (false);
			break;
		case "#PAUSE#":
			Debug.Log ("Pausing script 'Execution'.");
			currentLine = scriptContainer.FilterKeyInLine (key, currentLine);
			isScriptPaused = true;
			break;
		}
	}

	IEnumerator WaitTimer() {
		while (timeWaited < waitTime) {
			timeWaited += Time.deltaTime;
			yield return 0;
		}
		isWaitingForTimer = false;
		waitTime = 0;
		timeWaited = 0;
	}
}