using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float characterSpeed;
	Animator characterAnimator;

	// Use this for initialization
	void Start () {
		characterSpeed = 2.5f;
		characterAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (MapMovementController.isTransitioning) {
			characterAnimator.SetBool("isWalking", false);
		}
		if(EventManager.playerHasControl) {
			if (Input.GetKey(KeyCode.LeftArrow)){
				MoveCharacter("Left");
			}
			if (Input.GetKey(KeyCode.RightArrow)){
				MoveCharacter("Right");
			}
			if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) {
				characterAnimator.SetBool("isWalking", false);
			}
		}
	}

	void MoveCharacter(string direction) {

		if (direction == "Left") {
			Debug.Log ("I'm moving left.");
			characterAnimator.SetFloat("Facing Direction", -1f);
			characterAnimator.SetBool("isWalking", true);
			Vector3 currentPosition = gameObject.transform.position;
			Vector3 newPosition = new Vector3 (currentPosition.x - (Time.deltaTime * characterSpeed), currentPosition.y);
			gameObject.transform.position = newPosition;
		}
		else if (direction == "Right") {
			Debug.Log ("I'm moving right.");
			characterAnimator.SetFloat("Facing Direction", 1f);
			characterAnimator.SetBool("isWalking", true);
			Vector3 currentPosition = gameObject.transform.position;
			Vector3 newPosition = new Vector3 (currentPosition.x + (Time.deltaTime * characterSpeed), currentPosition.y);
			gameObject.transform.position = newPosition;
		}
	}
}
