using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Deck : MonoBehaviour {

	public List<GameObject> player1Deck;
	public List<GameObject> player2Deck;
	public List<GameObject> player1Discard;
	public List<GameObject> player2Discard;
	public GameObject card1;
	public GameObject card2;
	public Text cardsRemaining;

	private int maxHandSize = 5;


	// Use this for initialization
	void Start () {
		BuildDeck ();
		ShuffleDeck (player1Deck);
		ShuffleDeck (player2Deck);
	}

	void BuildDeck () {

		//TEMP: Just manually adding knight and cavalry to the deck for now. Eventually will need to hook up deck loadouts.
		for (int i=0; i < card1.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card1);
		}
		for (int i=0; i < card2.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card2);
		}

		//TEMP: Just manually adding knight and cavalry to the deck for now. Eventually will need to hook up deck loadouts.
		for (int i=0; i < card1.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card1);
		}
		for (int i=0; i < card2.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card2);
		}

//		if (FindObjectOfType<PlayField>().player1Turn) {
//			//TEMP: Just manually adding knight and cavalry to the deck for now. Eventually will need to hook up deck loadouts.
//			for (int i=0; i < card1.GetComponent<Card>().quantity; i++) {
//				player1Deck.Add (card1);
//			}
//			for (int i=0; i < card2.GetComponent<Card>().quantity; i++) {
//				player1Deck.Add (card2);
//			}
//		} else if (!FindObjectOfType<PlayField>().player1Turn) {
//			//TEMP: Just manually adding knight and cavalry to the deck for now. Eventually will need to hook up deck loadouts.
//			for (int i=0; i < card1.GetComponent<Card>().quantity; i++) {
//				player2Deck.Add (card1);
//			}
//			for (int i=0; i < card2.GetComponent<Card>().quantity; i++) {
//				player2Deck.Add (card2);
//			}
//		}

	}

	void ShuffleDeck (List<GameObject> myDeck) {
		for (int i=0; i < myDeck.Count; i++) {
			GameObject temp = myDeck[i];
			int randomIndex = Random.Range(i, myDeck.Count);
			myDeck[i] = myDeck[randomIndex];
			myDeck[randomIndex] = temp;
		}
	}

	public void Player1DealCards () {
		int currentCards = GameObject.Find("Player1 Hand").transform.childCount;
		if (currentCards < maxHandSize) {
			for (int i = 0; i < maxHandSize-currentCards; i++) {
				GameObject newCard = Instantiate (player1Deck [0]) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("Player1 Hand").transform, false);
				player1Deck.RemoveAt (0);
			}
		}
		cardsRemaining.text = player1Deck.Count.ToString ();
	}

	public void Player2DealCards () {
		int currentCards = GameObject.Find("Player2 Hand").transform.childCount;
		if (currentCards < maxHandSize) {
			for (int i = 0; i < maxHandSize-currentCards; i++) {
				GameObject newCard = Instantiate (player2Deck [0]) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("Player2 Hand").transform, false);
				player2Deck.RemoveAt (0);
			}
		}
		cardsRemaining.text = player2Deck.Count.ToString ();
	}
}
