using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	public float characterSpeed;
	public Transform playerPosition;
	public Vector3 playerPositionInPixels;
	public Vector3 positionBufferValues;
	Animator characterAnimator;
	SpriteRenderer characterRenderer;


	// Use this for initialization
	void Start () {
		characterSpeed = 2.5f;
		characterAnimator = GetComponent<Animator> ();
		characterRenderer = GetComponentInChildren<SpriteRenderer> ();
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
			characterAnimator.SetFloat("Facing Direction", -1f);
			characterAnimator.SetBool("isWalking", true);
			Vector3 currentPosition = gameObject.transform.position;
			Vector3 newPosition = new Vector3 (
				currentPosition.x - (Time.deltaTime * characterSpeed), 
				currentPosition.y);
			if(!IsPositionOffScreen(newPosition) || MapMovementController.availableAreasArray[0] == true) {
				gameObject.transform.position = newPosition;
			}
		}
		else if (direction == "Right") {
			characterAnimator.SetFloat("Facing Direction", 1f);
			characterAnimator.SetBool("isWalking", true);
			Vector3 currentPosition = gameObject.transform.position;
			Vector3 newPosition = new Vector3 (
				currentPosition.x + (Time.deltaTime * characterSpeed), 
				currentPosition.y);
			if(!IsPositionOffScreen(newPosition) || MapMovementController.availableAreasArray[2] == true) {
				gameObject.transform.position = newPosition;
			}
		}
	}

	bool IsPositionOffScreen(Vector3 position) {
		Vector3 positionInPixels = Camera.main.WorldToScreenPoint (position);
		Vector3 currentPositionInPixels = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		float bufferValueXInPixels = Camera.main.WorldToScreenPoint (playerPosition.position + positionBufferValues).x - Camera.main.WorldToScreenPoint (playerPosition.position).x;
		if (
			positionInPixels.x < 0 + bufferValueXInPixels ||
		    positionInPixels.x > Screen.width - bufferValueXInPixels
		) {
			return true;
		} else {
			return false;
		}
	}
}
