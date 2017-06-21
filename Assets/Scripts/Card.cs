using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour {

	public static GameObject selectedHero;
	public GameObject heroPrefab;
	public Text text;
	public string cardName;

	// Use this for initialization
	void Start () {
		text.text = cardName.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		Debug.Log(name + " selected");
		selectedHero = heroPrefab;
	}
}
