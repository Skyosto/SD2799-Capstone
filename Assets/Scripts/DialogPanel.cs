using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogPanel : MonoBehaviour {
	//"Views" for the dialogue panel
	//TODO  Obtain the image and animate it whenever waiting for input
	public Text dialogText;
	public Text speakerName;
	public ScriptContainer scriptContainer;
	public GameObject dialogArrow;

	//Properties for typing
	private float textSpeedInSeconds;
	private float timeTillNextCharacter;
	private int positionInDialogLine;

	//Window to the script
	static DialogPanel instance = null;

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
		FindSingletons ();
		//Obtain text speed from the PlayerPrefsManager
		AdjustTextSpeed ();
	}

	//When a level is loaded
	void OnLevelWasLoaded(int level) {
		//Obtain text speed from the PlayerPrefsManager
		AdjustTextSpeed ();
		FindSingletons ();
	}
	
	// Update is called once per frame
	void Update () {

	}
	#endregion

	#region Getters and Setters
	public int GetPositionInDialogLine() {return positionInDialogLine;}
	public void SetPositionInDialogLine(int position) {positionInDialogLine = position;}
	public Text GetDialogText() {
		return dialogText;
	}

	public void UpdateSpeaker() {
		//Display the current charater talking
		speakerName.text = scriptContainer.currentSpeaker;
	}
	public float GetTextSpeed() {
		return textSpeedInSeconds;
	}

	void FindSingletons () {
		scriptContainer = FindObjectOfType<ScriptContainer> ();
		dialogText = GameObject.Find ("Dialog Text").GetComponent<Text> ();
		dialogArrow = GameObject.Find ("Waiting Input Arrow");
		GameObject speakerNameObj = GameObject.Find ("Speaker Name");
		speakerName = speakerNameObj.GetComponent<Text> ();
	}
	#endregion

	public void DisplayDialogLine(bool typeWrite, string line) {
		EventManager.isDialogTyping = true;
		if (typeWrite) {
			DisplayDialogLine(line);
		} else {
			dialogText.text = line;
			positionInDialogLine = 0;
			timeTillNextCharacter = textSpeedInSeconds;
			EventManager.isWaitingForInput = true;
			EventManager.isDialogTyping = false;
		}
	}

	public void DisplayDialogLine(string line) {
		if (positionInDialogLine < line.Length){
			EventManager.isDialogTyping = true;
			if (timeTillNextCharacter <= 0) {
				dialogText.text += line [positionInDialogLine];
				positionInDialogLine++;
				timeTillNextCharacter = textSpeedInSeconds;
			} else {
				timeTillNextCharacter -= Time.deltaTime;
			}
		}
		if (positionInDialogLine >= line.Length) {
			positionInDialogLine = 0;
			EventManager.isWaitingForInput = true;
			EventManager.isDialogTyping = false;
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
		case 0:
			textSpeedInSeconds = 0f;
			break;
		case 1:
			textSpeedInSeconds = 0.25f;
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
		if (dialogArrow != null) {
			Animator dialogArrowAnimator = dialogArrow.GetComponent<Animator> ();
			dialogArrowAnimator.SetBool ("isWaitingForInput", play);
		}
	}

	public void ClearDialogBox() {
		dialogText.text = "";
	}
}
