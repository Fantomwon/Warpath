using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class Deck : MonoBehaviour {

	public List<GameObject> player1Deck;
	public List<GameObject> player2Deck;
	public List<GameObject> player1Discard;
	public List<GameObject> player2Discard;
//	public GameObject player1Card1, player1Card2, player1Card3, player1Card4, player1Card5, player2Card1, player2Card2, player2Card3, player2Card4, player2Card5;
	public GameObject blacksmithSpell1, blacksmithSpell2, blacksmithSpell3, druidSpell1, druidSpell2, druidSpell3, mageSpell1, mageSpell2, mageSpell3, rogueSpell1, rogueSpell2, rogueSpell3;
	public Text cardsRemaining;

	private string player1Class, player2Class;
	private int maxHandSize = 5;
	private PlayField playField;

	// Use this for initialization
	void Start () {
//		Debug.Log("RUNNING START FUNCTION OF DECK.CS");
		BuildDeck ();
		ShuffleDeck (player1Deck);
		ShuffleDeck (player2Deck);
		playField = GameObject.FindObjectOfType<PlayField>();
		player1Class = GlobalObject.instance.player1Class;
		player2Class = GlobalObject.instance.player2Class;
		SetPlayerSpells();
	}

	void BuildDeck () {
//		Debug.Log("RUNNING BUILDDECK()");
		foreach (GameObject card in GlobalObject.instance.player1DeckSelect) {
			for (int i=0; i<card.GetComponent<Card>().quantity; i++) {
//				Debug.Log("FOUND A CARD IN PLAYER1DECKSELECTLIST");
				player1Deck.Add(card);
			}
		}

		foreach (GameObject card in GlobalObject.instance.player2DeckSelect) {
			for (int i=0; i<card.GetComponent<Card>().quantity; i++) {
				player2Deck.Add(card);
			}
		}

		//TEMP: Just manually adding cards to the deck for now. Eventually will need to hook up deck loadouts.
//		for (int i=0; i < player1Card1.GetComponent<Card>().quantity; i++) {
//			player1Deck.Add (player1Card1);
//		}
//		for (int i=0; i < player1Card2.GetComponent<Card>().quantity; i++) {
//			player1Deck.Add (player1Card2);
//		}
//		for (int i=0; i < player1Card3.GetComponent<Card>().quantity; i++) {
//			player1Deck.Add (player1Card3);
//		}
//		for (int i=0; i < player1Card4.GetComponent<Card>().quantity; i++) {
//			player1Deck.Add (player1Card4);
//		}
//		for (int i=0; i < player1Card5.GetComponent<Card>().quantity; i++) {
//			player1Deck.Add (player1Card5);
//		}

		//TEMP: Just manually adding cards to the deck for now. Eventually will need to hook up deck loadouts.
//		for (int i=0; i < player2Card1.GetComponent<Card>().quantity; i++) {
//			player2Deck.Add (player2Card1);
//		}
//		for (int i=0; i < player2Card2.GetComponent<Card>().quantity; i++) {
//			player2Deck.Add (player2Card2);
//		}
//		for (int i=0; i < player2Card3.GetComponent<Card>().quantity; i++) {
//			player2Deck.Add (player2Card3);
//		}
//		for (int i=0; i < player2Card4.GetComponent<Card>().quantity; i++) {
//			player2Deck.Add (player2Card4);
//		}
//		for (int i=0; i < player2Card5.GetComponent<Card>().quantity; i++) {
//			player2Deck.Add (player2Card5);
//		}
	}

	public void RedealHand () {
		if (playField.player1Turn) {
			//Cycle through each card in my hand and remove it and add it to my discard pile
			foreach (Transform currentCard in GameObject.Find("Player1 Hand").transform) {
				Card.selectedCard = currentCard.gameObject;
				Card.selectedCard.GetComponent<Card>().RemoveCardFromHandAndAddToDiscard();
			}
			//Here we manually remove children from the "Player1 Hand" game object b/c they are destroyed, and when objects are destroyed they are not removed from
			//their parent object until the end of the frame, which causes Player1DealCards to think that the player still has 5 cards in their hand even though they have zero
			GameObject.Find("Player1 Hand").transform.DetachChildren();

			Player1DealCards ();
		} else if (!playField.player1Turn) {
			//Cycle through each card in my hand and remove it and add it to my discard pile
			foreach (Transform currentCard in GameObject.Find("Player2 Hand").transform) {
				Card.selectedCard = currentCard.gameObject;
				Card.selectedCard.GetComponent<Card>().RemoveCardFromHandAndAddToDiscard();
			}
			//Here we manually remove children from the "Player1 Hand" game object b/c they are destroyed, and when objects are destroyed they are not removed from
			//their parent object until the end of the frame, which causes Player2DealCards to think that the player still has 5 cards in their hand even though they have zero
			GameObject.Find("Player2 Hand").transform.DetachChildren();

			Player2DealCards ();
		}
	}

	void ShuffleDeck (List<GameObject> myDeck) {
//		Debug.Log("RUNNING SHUFFLE DECK");
		for (int i=0; i < myDeck.Count; i++) {
			GameObject temp = myDeck[i];
			int randomIndex = Random.Range(i, myDeck.Count);
			myDeck[i] = myDeck[randomIndex];
			myDeck[randomIndex] = temp;
		}
	}

	public void Player1DealCards () {
//		Debug.Log("RUNNING PLAYER1DEALCARDS");
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
//		Debug.Log("FINISHED PLAYER1DEALCARDS");
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
//		for (int i=0; i<player1Discard.Count(); i++) {
//			Debug.Log(player1Discard[i].name);
//		}
	}

	public void Player2AddCardToDiscard (GameObject card) {
		player2Discard.Add(card);
//		for (int i=0; i<player2Discard.Count(); i++) {
//			Debug.Log(player2Discard[i].name);
//		}
	}


	void SetPlayerSpells () {
		if (player1Class == "Mage") {
			GameObject newCard1 = Instantiate (mageSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard2 = Instantiate (mageSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard3 = Instantiate (mageSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
		} else if (player1Class == "Rogue") {
			GameObject newCard1 = Instantiate (rogueSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard2 = Instantiate (rogueSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard3 = Instantiate (rogueSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
		} else if (player1Class == "Druid") {
			GameObject newCard1 = Instantiate (druidSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard2 = Instantiate (druidSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard3 = Instantiate (druidSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
		} else if (player1Class == "Blacksmith") {
			GameObject newCard1 = Instantiate (blacksmithSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard2 = Instantiate (blacksmithSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
			GameObject newCard3 = Instantiate (blacksmithSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player1 Spells").transform, false);
		}

		if (player2Class == "Mage") {
			GameObject newCard1 = Instantiate (mageSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard2 = Instantiate (mageSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard3 = Instantiate (mageSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
		} else if (player2Class == "Rogue") {
			GameObject newCard1 = Instantiate (rogueSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard2 = Instantiate (rogueSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard3 = Instantiate (rogueSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
		} else if (player2Class == "Druid") {
			GameObject newCard1 = Instantiate (druidSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard2 = Instantiate (druidSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard3 = Instantiate (druidSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
		} else if (player2Class == "Blacksmith") {
			GameObject newCard1 = Instantiate (blacksmithSpell1) as GameObject;
			newCard1.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard2 = Instantiate (blacksmithSpell2) as GameObject;
			newCard2.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
			GameObject newCard3 = Instantiate (blacksmithSpell3) as GameObject;
			newCard3.transform.SetParent (GameObject.Find ("Player2 Spells").transform, false);
		}
	}
}
