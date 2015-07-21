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
	public int stateOfDialogue;

	//Window to the script
	public ScriptContainer scriptContainer;
	static DialogPanel instance = null;

	//Children?
	public GameObject dialogArrow;


	#region Unity LifeCycle Events

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

		//Initialize the timer
		timeTillNextCharacter = textSpeedInSeconds;

		//Initialize typing "cursor" at the beginning
		positionInDialogLine = 0;
		stateOfDialogue = 0;

		scriptContainer = FindObjectOfType<ScriptContainer> ();
		dialogArrow = GameObject.Find ("Waiting Input Arrow");

		//Start typing script immediately?
		EventManager.isWaitingForInput = false;
	}

	//When a level is loaded
	void OnLevelWasLoaded(int level) {
		scriptContainer = FindObjectOfType<ScriptContainer> ();
		//Obtain text speed from the PlayerPrefsManager
		AdjustTextSpeed ();
	}
	
	// Update is called once per frame
	void Update () {


		//If we haven't reached the end of the script yet
		//Display the line
		/*if (stateOfDialogue < scriptContainer.dialogLines.Length && !EventManager.isWaitingForInput) {
			if(PlayerPrefsManager.GetTextSpeed() > 0f) {
				DisplayDialogLine (true);
			} else { 
				DisplayDialogLine (false);
			}
		} else if (stateOfDialogue >= scriptContainer.dialogLines.Length) {
			//We've reached the end of the script
			EventManager.isEndOfScript = true;
		} else {
		}*/

	}
	#endregion

	#region Getters and Setters
	public int GetStateOfDialogue() {return stateOfDialogue;}
	public void SetStateOfDialogue(int state) {stateOfDialogue = state;}

	public int GetPositionInDialogLine() {return positionInDialogLine;}
	public void SetPositionInDialogLine(int position) {positionInDialogLine = position;}

	public void UpdateSpeaker() {
		//Display the current charater talking
		speakerName.text = scriptContainer.currentSpeaker;
	}
	#endregion

	#region Script Filters

	//Checks to see if the line has a different speaker
	public string CheckForSpeaker(string line) {
		line = scriptContainer.FilterKeyInLine ("#SPKR#", line);
		return line;
	}

	#endregion

	public void DisplayDialogLine(bool typeWrite, string line) {
		int i = positionInDialogLine;

		if (typeWrite) {
			while(i < line.Length) {
				EventManager.isWaitingForInput = false;
				if (timeTillNextCharacter <= 0){
					dialogText.text += line[i];
					positionInDialogLine++;
					timeTillNextCharacter = textSpeedInSeconds;
				}
				else {
					timeTillNextCharacter -= Time.deltaTime;
				}
			}
			EventManager.isWaitingForInput = true;
		} else {
			dialogText.text = line;
			positionInDialogLine = 0;
			timeTillNextCharacter = textSpeedInSeconds;
			EventManager.isWaitingForInput = true;
		}
	}
	public void ToggleDialogBoxVisibility(bool visibility) {
		gameObject.SetActive (visibility);
	}


	/* 0 Being instant and 1 - 3 varying speeds, 3 being the fastest.
	 * 3 = about 60 characters a second;
	 * 2 = about 10 characters a second;
	 * 1 = about 2 characters a second;
	 */
	public void AdjustTextSpeed() { 
		int textSpeed = (int)Mathf.Round (PlayerPrefsManager.GetTextSpeed ());
		switch(textSpeed) {
		case 1:
			textSpeedInSeconds = 0.5f;
			break;
		case 2:
			textSpeedInSeconds = 0.1f;
			break;
		case 3:
			textSpeedInSeconds = 0.012f;
			break;
		default:
			Debug.LogError("Text speed found from PlayerPrefs is not valid.");
			break;
		}
	}

	public void PlayDialogArrowAnimation(bool play) {
		Animator dialogArrowAnimator = dialogArrow.GetComponent<Animator> ();
		if (play = true && dialogArrowAnimator.)
		dialogArrowAnimator.SetBool ("isWaitingForInput", play);
	}

}
