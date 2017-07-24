using UnityEngine;
using System.Collections;

public class HandHider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
	}

	public void ShowHand () {
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
	}
}
