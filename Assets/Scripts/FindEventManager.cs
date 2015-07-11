using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FindEventManager : MonoBehaviour {
	Button button;
	EventManager eventManager;

	// Use this for initialization
	void Start () {
	}

	void OnLevelWasLoaded() {
		button = GetComponent<Button>();
		eventManager = FindObjectOfType<EventManager>();
		button.onClick.AddListener (() => {eventManager.progressEvent();});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
