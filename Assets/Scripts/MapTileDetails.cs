using UnityEngine;
using System.Collections;

public class MapTileDetails : MonoBehaviour {

	public SpriteRenderer mapTileRenderer;

	public bool LeftAreaIsAvailable;
	public bool TopAreaIsAvailable;
	public bool RightAreaIsAvailable;
	public bool BottomAreaIsAvailable;

	// Use this for initialization
	void Start () {
		mapTileRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!MapMovementController.isTransitioning) {
			if(mapTileRenderer.isVisible) {
				SetAvailableAreas();
			}
		}
	}

	void SetAvailableAreas() {
		for (int i = 0; i < MapMovementController.availableAreasArray.Length; i++) {
			switch(i) {
			case 0: //Left
				MapMovementController.availableAreasArray[0] = LeftAreaIsAvailable;
				break;
			case 1: //Top
				MapMovementController.availableAreasArray[0] = TopAreaIsAvailable;
				break;
			case 2: //Right
				MapMovementController.availableAreasArray[0] = RightAreaIsAvailable;
				break;
			case 3: //Bottom
				MapMovementController.availableAreasArray[0] = BottomAreaIsAvailable;
				break;
			}
		}
	}
}
