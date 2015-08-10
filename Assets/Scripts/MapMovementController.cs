using UnityEngine;
using System.Collections;

public class MapMovementController : MonoBehaviour {
	Transform playerCharacter;
	Camera camera;
	float cameraMoveSpeed;

	public static bool isTransitioning;
	// Use this for initialization
	void Start () {
		playerCharacter = GameObject.Find ("Rizma").transform;
		camera = GetComponent<Camera> ();
		cameraMoveSpeed = 5f;

		isTransitioning = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isTransitioning) {
			Vector3 characterPosInPixels = camera.WorldToScreenPoint(playerCharacter.position);
			if (characterPosInPixels.x >= Screen.width) {
				Debug.Log ("Moving screen right.");
				StartCoroutine(moveCamera(new Vector3(Camera.main.transform.position.x + 16, Camera.main.transform.position.y)));
				playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 3, playerCharacter.transform.position.y);
			} else if (characterPosInPixels.x <= 0) {
				Debug.Log("Moving screen left.");
				StartCoroutine(moveCamera(new Vector3(Camera.main.transform.position.x - 16, Camera.main.transform.position.y)));
				playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 3, playerCharacter.transform.position.y);
			}
		}
	}

	IEnumerator moveCamera(Vector3 target) {
		isTransitioning = true;
		EventManager.playerHasControl = false;

		while (Camera.main.transform.position.x != target.x) {
			if(target.x > Camera.main.transform.position.x) {
				Camera.main.transform.position += new Vector3(
					Mathf.Clamp(
						Time.deltaTime * cameraMoveSpeed,
				        0,
						(target.x - Camera.main.transform.position.x)
					),
					0
				);
			}
			else if (target.x < Camera.main.transform.position.x){
				Camera.main.transform.position += new Vector3(
					Mathf.Clamp(
						-(Time.deltaTime * cameraMoveSpeed),
						(target.x - Camera.main.transform.position.x),
						0
					),
					0
					);
			}
			yield return 0;

		}
		isTransitioning = false;
		EventManager.playerHasControl = true;

	}
}
