using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeCanvas : MonoBehaviour {

	//1 and 2 should be black faders and 3 and 4 should be white out faders
	public RectTransform[] fadingPanels = new RectTransform[4];

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void WhiteOut() { 
		FadeCanvas fadeCanvas = FindObjectOfType<FadeCanvas>();
		RectTransform whitePanel = null;

		foreach (RectTransform child in fadeCanvas.transform) {
			if (child.name == "White Panel") {
				whitePanel = child;
				break;
			}
		}

		if(whitePanel == null) {
			whitePanel = Object.Instantiate(fadingPanels[2]);
			whitePanel.SetParent(fadeCanvas.transform);
			whitePanel.transform.position = fadeCanvas.transform.position;
			whitePanel.sizeDelta = new Vector2(100, 100);
			fadeCanvas.StartCoroutine(WhiteOutAnimation(whitePanel));
		}
	}

	IEnumerator WhiteOutAnimation(RectTransform whiteOutPanel) {
		EventManager.animationIsPlaying = true;
		Image whiteOutPanelColor = whiteOutPanel.GetComponent<Image> ();
		Color currentColor = whiteOutPanelColor.color;
		currentColor.a = 0;
		float timeWaited = 0f;
		float whiteOutTime = 5.0f;


		while (whiteOutPanelColor.color.a < 255) {
			timeWaited += Time.deltaTime;
			currentColor.a += 1;
			whiteOutPanelColor.color = currentColor;
			yield return 0;
		}

		Destroy(whiteOutPanel.gameObject);
		EventManager.animationIsPlaying = false;
	}
}
