using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DestroyWhitePanel() {
		EventManager.animationIsPlaying = false;
	}

	public void DestroyMyself() {
		Destroy (this.gameObject);
	}
}
