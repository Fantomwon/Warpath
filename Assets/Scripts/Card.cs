using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Card : MonoBehaviour {

	public static GameObject selectedHero;
	public GameObject heroPrefab;
	public static GameObject selectedCard;
	public static GameObject cardPrefab;
	//public GameObject selectedPhysicalCard;
	public Text text;
	public string cardName;
	public int quantity;

	// Use this for initialization
	void Start () {
		text.text = cardName.ToString();
	}

	void OnMouseDown () {
		selectedHero = heroPrefab;
		selectedCard = gameObject;
		//selectedPhysicalCard = this.gameObject;
		Debug.Log("Current Selected Game Object is " + selectedCard);
	}
}
