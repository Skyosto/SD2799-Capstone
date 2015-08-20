using UnityEngine;
using System.Collections;

public class MapMovementController : MonoBehaviour {
	Transform playerCharacter;
	Vector3 playerPositionInPixels;
	Camera camera;
	float cameraMoveSpeed;

	PlayerController playerController;


	public static bool isTransitioning;

	//In this array position 0 will represent "Left" or "West" and goes clockwise ending with
	// 3 which is "Bottom" or "South"
	public static bool[] availableAreasArray = new bool[4];

	// Use this for initialization
	void Start () {
		playerCharacter = GameObject.Find ("Rizma").transform;
		cameraMoveSpeed = 7.5f;
		playerController = playerCharacter.gameObject.GetComponentInChildren<PlayerController>();

		isTransitioning = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator moveCamera(Vector3 target) {
		isTransitioning = true;
		EventManager.playerHasControl = false;

		while (Camera.main.transform.position.x != target.x) {
			if(target.x > Camera.main.transform.position.x) {
				Camera.main.transform.position += new Vector3(
					Mathf.Clamp(Time.deltaTime * cameraMoveSpeed, 0, (target.x - Camera.main.transform.position.x)),
					0
				);
			}
			else if (target.x < Camera.main.transform.position.x){
				Camera.main.transform.position += new Vector3(
					Mathf.Clamp(-(Time.deltaTime * cameraMoveSpeed), (target.x - Camera.main.transform.position.x), 0),
					0
				);
			}
			yield return 0;

		}
		isTransitioning = false;
		EventManager.playerHasControl = true;

	}
}
