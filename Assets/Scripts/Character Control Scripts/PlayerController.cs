using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	public float characterSpeed;
	public Transform playerPosition;
	public Vector3 playerPositionInPixels;
	public Vector3 positionBufferValues;
	public MapMovementController mapMovementController;
	Animator characterAnimator;
	SpriteRenderer characterRenderer;

	Inventroy inventory;


	// Use this for initialization
	void Start () {
		characterSpeed = 2.5f;
		characterAnimator = GetComponent<Animator> ();
		characterRenderer = GetComponentInChildren<SpriteRenderer> ();
		mapMovementController = Camera.main.GetComponent<MapMovementController>();
		playerPosition = gameObject.transform;
		positionBufferValues = characterRenderer.bounds.extents;
		EventManager.playerHasControl = false;

		inventory = GetComponent<Inventroy> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (MapMovementController.isTransitioning) {
			characterAnimator.SetBool("isWalking", false);
		}

		float horizontalAxis = CrossPlatformInputManager.GetAxis ("Horizontal");

		if(EventManager.playerHasControl) {
			if (Input.GetKey(KeyCode.LeftArrow) || horizontalAxis < 0){
				MoveCharacter("Left");
			}
			if (Input.GetKey(KeyCode.RightArrow) || horizontalAxis > 0){
				MoveCharacter("Right");
			}
			if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && horizontalAxis == 0) {
				characterAnimator.SetBool("isWalking", false);
			}
		}
	}

	void MoveCharacter(string direction) {	
			if (direction == "Left") {
				characterAnimator.SetFloat ("Facing Direction", -1f);
				characterAnimator.SetBool ("isWalking", true);
				Vector3 currentPosition = gameObject.transform.position;
				
				if(MapMovementController.availableAreasArray[0] == true) {
					Vector3 newPosition = new Vector3 (
						currentPosition.x - (Time.deltaTime * characterSpeed), 
						currentPosition.y);
					gameObject.transform.position = newPosition;
				}
				else {
					Vector3 edgeOfCamera = new Vector3(0, 0);
					Debug.Log (Camera.main.ScreenToWorldPoint(edgeOfCamera).x);
					Vector3 newPosition = new Vector3 (
					Mathf.Clamp (currentPosition.x - (Time.deltaTime * characterSpeed),Camera.main.ScreenToWorldPoint(edgeOfCamera).x + characterRenderer.bounds.extents.x,9999), 
						currentPosition.y);
					gameObject.transform.position = newPosition;
				}
			} else if (direction == "Right") {
				//Start the animation
				characterAnimator.SetFloat ("Facing Direction", 1f);
				characterAnimator.SetBool ("isWalking", true);

				//Calculate the next positon
				Vector3 currentPosition = gameObject.transform.position;
				if(MapMovementController.availableAreasArray[2] == true) {
					Vector3 newPosition = new Vector3 (
						currentPosition.x + (Time.deltaTime * characterSpeed), 
						currentPosition.y);
					gameObject.transform.position = newPosition;
				}
				else {
				Vector3 edgeOfCamera = new Vector3(Screen.width, 0);
				Debug.Log (Camera.main.ScreenToWorldPoint(edgeOfCamera).x);
					Vector3 newPosition = new Vector3 (
						Mathf.Clamp (currentPosition.x + (Time.deltaTime * characterSpeed),0,Camera.main.ScreenToWorldPoint(edgeOfCamera).x - characterRenderer.bounds.extents.x), 
						currentPosition.y);
					gameObject.transform.position = newPosition;
				}
			}
	}

	bool IsPositionOnScreen(Vector3 position) {
		Vector3 positionInPixels = Camera.main.WorldToScreenPoint (position);
		Collider2D playerCollider = gameObject.GetComponent<BoxCollider2D>();
		if (positionInPixels.x < 0 + playerCollider.bounds.extents.x || positionInPixels.x > Screen.width - playerCollider.bounds.extents.x) 
		{
			Debug.Log ("The next step is out of bounds.");
			return false;
		} else {
			return true;
		}

	}

	void OnTriggerStay2D (Collider2D collider) {
		Debug.Log ("I'm in a collider.");
		if (collider.gameObject.tag == "Item") {
			Debug.Log ("I'm in an item's collider.");
			if(EventManager.playerHasControl) {
				if (Input.GetKey(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump")){
					PickUpItem(collider);
				}
			}
		}
		if (collider.gameObject.tag == "NPC") {
			Debug.Log ("I'm in an NPC's collider.");
			if(EventManager.playerHasControl) {
				if (Input.GetKey(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump")){
					TalkToNPC(collider);
				}
			}
		}
	}

	void PickUpItem(Collider2D collider) {
		Debug.Log ("I've picked up a "+collider.gameObject.name+".");
		inventory.AddItemToInventory (collider.gameObject);
		collider.gameObject.transform.position = new Vector3 (-1000,-1000);
		Debug.Log ("I need a: "+EventManager.itemNeeded);
		EventManager.hasItemNeeded = true;
		//collider.gameObject.SetActive (false);
		//Destroy (collider.gameObject);
	}

	void TalkToNPC(Collider2D collider) {
		Debug.Log ("Do I have what I need? "+EventManager.hasItemNeeded);
		if(EventManager.hasItemNeeded == true) {
			Debug.Log ("I'm talking to a "+collider.gameObject.name+".");
			EventManager.playerHasControl = false;
			EventManager.isScriptPaused = false;
		}
	}

}
