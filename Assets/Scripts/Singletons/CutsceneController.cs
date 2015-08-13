using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour {

	static CutsceneController instance;

	public EventManager eventManager;

	public GameObject[] charactersArray;

	void Awake () {
		//Start the singleton pattern
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}

		FindSingletons ();
	}

	// Use this for initialization
	void Start () {
		
	}

	void OnLevelWasLoaded(int level) {
		FindSingletons ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FindSingletons() {
		eventManager = FindObjectOfType<EventManager> ();
	}

	//For the position parameter, 16 is the farthest right and 9 is the farthest top. So:
	//	Vector3(16,9) is the top-right corner and Vector3(0,0) is the bottom-left corner
	//
	//For the direction paramater, 0 is North and goes clockwise. So:
	//	0 = North
	//	1 = East
	//	2 = South
	//	3 = West
	void MoveCharacterToPosition(GameObject character, Vector3 position, int direction) {
	}
}
