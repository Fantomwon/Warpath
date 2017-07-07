using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Card : MonoBehaviour {

	public static GameObject selectedHero;
	public GameObject heroPrefab;
	public static GameObject selectedCard;
	public Text text;
	public string cardName;
	public int quantity;

	// Use this for initialization
	void Start () {
		text.text = cardName.ToString();
	}

	void OnMouseDown () {
		Debug.Log(name + " selected");
		selectedHero = heroPrefab;
		selectedCard = this.gameObject;
		Debug.Log("Current Selected Game Object is " + selectedCard);
	}

}
