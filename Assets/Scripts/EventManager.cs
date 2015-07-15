using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviour {

	public static bool isWaitingForInput;
	public static bool isEndOfScript;

	public DialogPanel dialogPanel;
	public Text dialogText;

	#region Unity LifeCycle Events
	// Use this for initialization
	void Start () {
		GameObject.DontDestroyOnLoad (gameObject);
	}

	void Awake() {
		//dialogPanel = GetComponent<DialogPanel> ();
	}

	void OnLevelWasLoaded(int level) {
		dialogPanel = FindObjectOfType<DialogPanel>();
		dialogText = dialogPanel.dialogText;
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
				dialogPanel.SetStateOfDialogue (dialogPanel.GetStateOfDialogue () + 1);
				dialogPanel.SetPositionInDialogLine (0);
				EventManager.isWaitingForInput = false;
			} else {
				print (dialogPanel.GetStateOfDialogue ());
				print ("Automatically finishing line");
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