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
			if (child.tag == "White Panel") {
				whitePanel = child;
				break;
			}
		}

		Debug.Log ("Does a white panel exist?: "+whitePanel == null);

		if(whitePanel == null) {
			whitePanel = Object.Instantiate(fadingPanels[2]);
			whitePanel.SetParent(fadeCanvas.transform);
			whitePanel.transform.position = fadeCanvas.transform.position;
			whitePanel.sizeDelta = new Vector2(100, 100);
		}

		//fadeCanvas.StartCoroutine(WhiteOutAnimation(whitePanel));
		EventManager.animationIsPlaying = true;
		Animator whitePanelAnimator = whitePanel.GetComponent<Animator> ();
		whitePanelAnimator.SetFloat ("Alpha", 0f);
		whitePanelAnimator.SetTrigger ("Fade Out");
		whitePanelAnimator.SetFloat ("Alpha", 1f);

	}

	public void WhiteIn() { 
		FadeCanvas fadeCanvas = FindObjectOfType<FadeCanvas>();
		RectTransform whitePanel = null;
		
		foreach (RectTransform child in fadeCanvas.transform) {
			if (child.tag == "White Panel") {
				whitePanel = child;
				break;
			}
		}
		
		if(whitePanel == null) {
			whitePanel = Object.Instantiate(fadingPanels[2]);
			whitePanel.SetParent(fadeCanvas.transform);
			whitePanel.transform.position = fadeCanvas.transform.position;
			whitePanel.sizeDelta = new Vector2(100, 100);
		}
		
		//fadeCanvas.StartCoroutine(WhiteOutAnimation(whitePanel));
		Animator whitePanelAnimator = whitePanel.GetComponent<Animator> ();
		EventManager.animationIsPlaying = true;
		whitePanelAnimator.SetFloat ("Alpha", 1f);
		whitePanelAnimator.SetTrigger ("Fade In");
		whitePanelAnimator.SetFloat ("Alpha", 0f);

	}

	public void BlackOut() { 
		FadeCanvas fadeCanvas = FindObjectOfType<FadeCanvas>();
		RectTransform blackPanel = null;
		
		foreach (RectTransform child in fadeCanvas.transform) {
			if (child.tag == "Black Panel") {
				blackPanel = child;
				break;
			}
		}
		
		Debug.Log ("Does a black panel exist?: "+blackPanel == null);
		
		if(blackPanel == null) {
			blackPanel = Object.Instantiate(fadingPanels[0]);
			blackPanel.SetParent(fadeCanvas.transform);
			blackPanel.transform.position = fadeCanvas.transform.position;
			blackPanel.sizeDelta = new Vector2(100, 100);
		}
		
		//fadeCanvas.StartCoroutine(WhiteOutAnimation(whitePanel));
		EventManager.animationIsPlaying = true;
		Animator whitePanelAnimator = blackPanel.GetComponent<Animator> ();
		whitePanelAnimator.SetFloat ("Alpha", 0f);
		whitePanelAnimator.SetTrigger ("Black Out");
		whitePanelAnimator.SetFloat ("Alpha", 1f);
		
	}
	
	public void BlackIn() { 
		FadeCanvas fadeCanvas = FindObjectOfType<FadeCanvas>();
		RectTransform blackPanel = null;
		
		foreach (RectTransform child in fadeCanvas.transform) {
			if (child.tag == "Black Panel") {
				blackPanel = child;
				break;
			}
		}
		
		if(blackPanel == null) {
			blackPanel = Object.Instantiate(fadingPanels[0]);
			blackPanel.SetParent(fadeCanvas.transform);
			blackPanel.transform.position = fadeCanvas.transform.position;
			blackPanel.sizeDelta = new Vector2(100, 100);
		}
		
		//fadeCanvas.StartCoroutine(WhiteOutAnimation(whitePanel));
		Animator whitePanelAnimator = blackPanel.GetComponent<Animator> ();
		EventManager.animationIsPlaying = true;
		whitePanelAnimator.SetFloat ("Alpha", 1f);
		whitePanelAnimator.SetTrigger ("Black In");
		whitePanelAnimator.SetFloat ("Alpha", 0f);
		
	}
}
