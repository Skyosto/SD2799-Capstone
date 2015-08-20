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


	// Use this for initialization
	void Start () {
		characterSpeed = 2.5f;
		characterAnimator = GetComponent<Animator> ();
		characterRenderer = GetComponentInChildren<SpriteRenderer> ();
		mapMovementController = Camera.main.GetComponent<MapMovementController>();
		playerPosition = gameObject.transform;
		positionBufferValues = characterRenderer.bounds.extents;
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
				Vector3 newPosition = new Vector3 (
					currentPosition.x - (Time.deltaTime * characterSpeed), 
					currentPosition.y);
				if(IsPositionOnScreen(newPosition)) {
					gameObject.transform.position = newPosition;
				}
				else {
					gameObject.transform.position = currentPosition;
				}
			} else if (direction == "Right") {
				characterAnimator.SetFloat ("Facing Direction", 1f);
				characterAnimator.SetBool ("isWalking", true);
				Vector3 currentPosition = gameObject.transform.position;
				Vector3 newPosition = new Vector3 (
					currentPosition.x + (Time.deltaTime * characterSpeed), 
					currentPosition.y);
				if(IsPositionOnScreen(newPosition)) {
					gameObject.transform.position = newPosition;
				}
				else {
					gameObject.transform.position = currentPosition;
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

	void OnCollisionEnter2D (Collision2D collider) {
		Debug.Log ("I triggered the "+collider.gameObject.tag);
		if(collider.gameObject.tag == "MainCamera") {
			if (MapMovementController.isTransitioning == false) {
				Vector3 positionInPixels = Camera.main.WorldToScreenPoint (gameObject.transform.position);
				if(positionInPixels.x > (Screen.width / 2) && MapMovementController.availableAreasArray[2] == true) {
					StartCoroutine(mapMovementController.moveCamera(new Vector3(Camera.main.transform.position.x + 16, Camera.main.transform.position.y)));
					gameObject.transform.position = new Vector3(gameObject.transform.position.x + 3, gameObject.transform.position.y);
				}
				else if (positionInPixels.x < (Screen.width / 2) && MapMovementController.availableAreasArray[0] == true) {
					StartCoroutine(mapMovementController.moveCamera(new Vector3(Camera.main.transform.position.x - 16, Camera.main.transform.position.y)));
					gameObject.transform.position = new Vector3(gameObject.transform.position.x - 3, gameObject.transform.position.y);
				}
				else {
				}
			}
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
	}

	void PickUpItem(Collider2D collider) {
		Debug.Log ("I've picked up a "+collider.gameObject.name+".");
		Destroy (collider.gameObject);
	}
}
