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
	public GameObject card1, card2, card3, card4, card5, card6, card7, card8, card9;
	public Text cardsRemaining;

	private int maxHandSize = 5;

	// Use this for initialization
	void Start () {
		BuildDeck ();
		ShuffleDeck (player1Deck);
		ShuffleDeck (player2Deck);
	}

	void BuildDeck () {

		//TEMP: Just manually adding cards to the deck for now. Eventually will need to hook up deck loadouts.
		for (int i=0; i < card1.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card1);
		}
		for (int i=0; i < card2.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card2);
		}
		for (int i=0; i < card3.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card3);
		}
		for (int i=0; i < card4.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card4);
		}
		for (int i=0; i < card5.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card5);
		}
		for (int i=0; i < card6.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card6);
		}
		for (int i=0; i < card7.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card7);
		}
		for (int i=0; i < card8.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card8);
		}
		for (int i=0; i < card9.GetComponent<Card>().quantity; i++) {
			player1Deck.Add (card9);
		}

		//TEMP: Just manually adding cards to the deck for now. Eventually will need to hook up deck loadouts.
		for (int i=0; i < card1.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card1);
		}
		for (int i=0; i < card2.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card2);
		}
		for (int i=0; i < card3.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card3);
		}
		for (int i=0; i < card4.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card4);
		}
		for (int i=0; i < card5.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card5);
		}
		for (int i=0; i < card6.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card6);
		}
		for (int i=0; i < card7.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card7);
		}
		for (int i=0; i < card8.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card8);
		}
		for (int i=0; i < card9.GetComponent<Card>().quantity; i++) {
			player2Deck.Add (card9);
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
				//If I don't have any cards in my Deck, then shuffle my discard pile and make it my new deck
				if (player1Deck.Count == 0) {
					//Shuffle the discard pile
					ShuffleDeck(player1Discard);

					//Copy the contents of the discard pile over to the deck pile
					GameObject[] deckArray = new GameObject[player1Discard.Count];
					player1Discard.CopyTo(deckArray);
					player1Deck = deckArray.ToList();

					//Clear out the discard pile list (this WILL NOT effect the deck pile b/c we copied the list over here, we didn't just link the two lists together by setting them equal to each other.
					player1Discard.Clear();
				}
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
				//If I don't have any cards in my Deck, then shuffle my discard pile and make it my new deck
				if (player2Deck.Count == 0) {
					//Shuffle the discard pile
					ShuffleDeck(player2Discard);

					//Copy the contents of the discard pile over to the deck pile
					GameObject[] deckArray = new GameObject[player2Discard.Count];
					player2Discard.CopyTo(deckArray);
					player2Deck = deckArray.ToList();

					//Clear out the discard pile list (this WILL NOT effect the deck pile b/c we copied the list over here, we didn't just link the two lists together by setting them equal to each other.
					player2Discard.Clear();
				}
				GameObject newCard = Instantiate (player2Deck [0]) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("Player2 Hand").transform, false);
				player2Deck.RemoveAt (0);
			}
		}

		cardsRemaining.text = player2Deck.Count.ToString ();
	}

	public void Player1AddCardToDiscard (GameObject card) {
		player1Discard.Add(card);
	}

	public void Player2AddCardToDiscard (GameObject card) {
		player2Discard.Add(card);
	}
}
