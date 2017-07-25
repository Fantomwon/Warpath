using UnityEngine;
using System.Collections;

public class HandHider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void ShowHand () {
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void HideHand () {
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
