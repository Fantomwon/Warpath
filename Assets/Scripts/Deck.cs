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
		//If my current hand size is under maxHandSize then deal me cards until I'm back up to max
		//As cards are dealt they are removed from the deck
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

	public void Player1AddCardToDiscard (GameObject card) {
		player1Discard.Add(card);
		Debug.LogError("CARD ADDED TO DISCARD PILE: " + card.name);
		Debug.LogError("NUMBER OF CARDS IN DISCARD PILE: " + player1Discard.Count);
	}

	public void Player2AddCardToDiscard (GameObject card) {
		player2Discard.Add(card);
	}
}
