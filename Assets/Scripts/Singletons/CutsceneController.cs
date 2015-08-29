using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour {

	static CutsceneController instance;

	public EventManager eventManager;

	public GameObject[] charactersArray;

	public static string orpForm = "";
	public Sprite[] orpFormsArray;
	public GameObject[] itemsArray;

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
	public void MoveCharacterToPosition(GameObject character, Vector3 position, int direction) {
	}

	public void ChangeOrpForm(string form) {
		GameObject orpObject = GameObject.Find ("Orp");
		SpriteRenderer orpRenderer = orpObject.GetComponent<SpriteRenderer>();
		BoxCollider2D orpCollider = orpObject.GetComponent<BoxCollider2D> ();
		switch(form) {
		case "shovel":
			orpRenderer.sprite = orpFormsArray[0];
			orpObject.transform.localScale = new Vector3 (5f,5f);
			orpCollider.size = new Vector3 (0.35f, 0.35f);
			break;
		}
	}

	public void SpawnObject(string name, Vector3 position) {
		if(name == "shed") {
			Object.Instantiate(itemsArray[0], new Vector3(position.x, position.y, 2f), Quaternion.identity);
		}
		if(name == "shovel") {
			Object.Instantiate(itemsArray[1], new Vector3(position.x, position.y, 1f), Quaternion.identity);
		}
		if (name == "turtleEggs") {
			Object.Instantiate(itemsArray[2], new Vector3(position.x, position.y, 1f), Quaternion.identity);
		}
	}
}
