using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class Deck : MonoBehaviour {

	public List<string> player1Deck;
	public List<string> player2Deck;
	public List<string> player1Discard;
	public List<string> player2Discard;
//	public GameObject player1Card1, player1Card2, player1Card3, player1Card4, player1Card5, player2Card1, player2Card2, player2Card3, player2Card4, player2Card5;
	public GameObject blacksmithSpell1, blacksmithSpell2, blacksmithSpell3, druidSpell1, druidSpell2, druidSpell3, mageSpell1, mageSpell2, mageSpell3, rogueSpell1, rogueSpell2, rogueSpell3;
	public Text cardsRemaining;

	private GlobalObject globalObject;
	private AiManager aiManager;
	private string player1Class, player2Class;
	private int maxHandSize = 5;
	private int quantityPerCard = 5;
	private PlayField playField;

	// Use this for initialization
	void Start () {
//		Debug.Log("RUNNING START FUNCTION OF DECK.CS");
		BuildDeck ();
		ShuffleDeck (player1Deck);
		if (!GlobalObject.storyEnabled) {
			ShuffleDeck (player2Deck);
		}
		globalObject = GameObject.FindObjectOfType<GlobalObject>();
		playField = GameObject.FindObjectOfType<PlayField>();
		aiManager = GameObject.FindObjectOfType<AiManager>();
		player1Class = GlobalObject.instance.player1Class;
		player2Class = GlobalObject.instance.player2Class;
		SetPlayerSpells();
	}

	void BuildDeck () {
//		Debug.Log("RUNNING BUILDDECK()");
		foreach (string cardId in GlobalObject.instance.player1DeckSelect) {
			for (int i=0; i<quantityPerCard; i++) {
//				Debug.Log("FOUND A CARD IN PLAYER1DECKSELECTLIST");
				player1Deck.Add(cardId);
			}
		}

		foreach (string cardId in GlobalObject.instance.player2DeckSelect) {
			for (int i=0; i<quantityPerCard; i++) {
				player2Deck.Add(cardId);
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
				RemoveCardFromHandAndAddToDiscard();
				//Card.selectedCard.GetComponent<Card>().RemoveCardFromHandAndAddToDiscard();
			}
			//Here we manually remove children from the "Player1 Hand" game object b/c they are destroyed, and when objects are destroyed they are not removed from
			//their parent object until the end of the frame, which causes Player1DealCards to think that the player still has 5 cards in their hand even though they have zero
			GameObject.Find("Player1 Hand").transform.DetachChildren();

			Player1DealCards ();
		} else if (!playField.player1Turn) {
			//Cycle through each card in my hand and remove it and add it to my discard pile
			foreach (Transform currentCard in GameObject.Find("Player2 Hand").transform) {
				Card.selectedCard = currentCard.gameObject;
				RemoveCardFromHandAndAddToDiscard();
				//Card.selectedCard.GetComponent<Card>().RemoveCardFromHandAndAddToDiscard();
			}
			//Here we manually remove children from the "Player1 Hand" game object b/c they are destroyed, and when objects are destroyed they are not removed from
			//their parent object until the end of the frame, which causes Player2DealCards to think that the player still has 5 cards in their hand even though they have zero
			GameObject.Find("Player2 Hand").transform.DetachChildren();

			Player2DealCards ();
		}
	}

	void ShuffleDeck (List<string> myDeck) {
//		Debug.Log("RUNNING SHUFFLE DECK");
		for (int i=0; i < myDeck.Count; i++) {
			string temp = myDeck[i];
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
					Debug.Log("PLAYER DECK EMPTY");
					//Shuffle the discard pile
					ShuffleDeck(player1Discard);

					//Copy the contents of the discard pile over to the deck pile
					string[] deckArray = new string[player1Discard.Count];
					player1Discard.CopyTo(deckArray);
					player1Deck = deckArray.ToList();

					//Clear out the discard pile list (this WILL NOT effect the deck pile b/c we copied the list over here, we didn't just link the two lists together by setting them equal to each other.
					player1Discard.Clear();
				}
				//TODO Come up with a better way to check if the cardId in the List<string> is a spell card or not
				if (CheckIfAllCardsInHandAreSpellCards() && CheckIfCardIdIsASpellCard(player1Deck[0])) {
					Debug.Log("4 SPELL CARDS IN HAND, NOT LETTING THAT HAPPEN!");
					string nextCardInDeck = player1Deck[0];
					player1Deck.RemoveAt(0);
					player1Discard.Add(nextCardInDeck);
					Player1DealCards();
					//player1Deck.Insert(Random.Range(1, player1Deck.Count - 1), nextCardInDeck);
				} else {
					if (!CheckIfCardIdIsASpellCard(player1Deck[0])) {
						globalObject.SetTemplateHeroCardAttributes(player1Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateHeroCard) as GameObject;
						newCard.transform.SetParent (GameObject.Find ("Player1 Hand").transform, false);
						globalObject.SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
					} else if (CheckIfCardIdIsASpellCard(player1Deck[0])) {
						globalObject.SetTemplateSpellCardAttributes(player1Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateSpellCard) as GameObject;
						newCard.transform.SetParent (GameObject.Find ("Player1 Hand").transform, false);
						//globalObject.SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
					}
					player1Deck.RemoveAt(0);

				}
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
					Debug.Log("PLAYER DECK EMPTY");
					//Shuffle the discard pile
					ShuffleDeck(player2Discard);

					//Copy the contents of the discard pile over to the deck pile
					string[] deckArray = new string[player2Discard.Count];
					player2Discard.CopyTo(deckArray);
					player2Deck = deckArray.ToList();

					//Clear out the discard pile list (this WILL NOT effect the deck pile b/c we copied the list over here, we didn't just link the two lists together by setting them equal to each other.
					player2Discard.Clear();
				}

				if (CheckIfAllCardsInHandAreSpellCards() && (player2Deck[0] == "armor" || player2Deck[0] == "might" || player2Deck[0] == "shroud" || player2Deck[0] == "root" || 
					player2Deck[0] == "fireball" || player2Deck[0] == "heal" || player2Deck[0] == "rockthrow" || player2Deck[0] == "windgust")) {
					Debug.Log("4 SPELL CARDS IN HAND, NOT LETTING THAT HAPPEN!");
					string nextCardInDeck = player2Deck[0];
					player2Deck.RemoveAt(0);
					player2Discard.Add(nextCardInDeck);
					Player2DealCards();
					//player2Deck.Insert(Random.Range(1, player2Deck.Count - 1), nextCardInDeck);
				} else {
					if (!CheckIfCardIdIsASpellCard(player2Deck[0])) {
						globalObject.SetTemplateHeroCardAttributes(player2Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateHeroCard) as GameObject;
						//Alter card cost if ai is enabled
						if ( GlobalObject.aiEnabled == true ) {
							Debug.Log("SETTING NEW MANA COST FOR CARD");
							newCard.GetComponent<Card>().manaCost = aiManager.AiAlterCardCost(newCard);
						}
						newCard.transform.SetParent (GameObject.Find ("Player2 Hand").transform, false);
						globalObject.SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
					} else if (CheckIfCardIdIsASpellCard(player2Deck[0])) {
						globalObject.SetTemplateSpellCardAttributes(player2Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateSpellCard) as GameObject;
						//Alter card cost if ai is enabled
						if ( GlobalObject.aiEnabled == true ) {
							Debug.Log("SETTING NEW MANA COST FOR CARD");
							newCard.GetComponent<Card>().manaCost = aiManager.AiAlterCardCost(newCard);
						}
						newCard.transform.SetParent (GameObject.Find ("Player2 Hand").transform, false);
						//globalObject.SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
					}
					player2Deck.RemoveAt(0); 
				}
			}
		}
		cardsRemaining.text = player2Deck.Count.ToString ();
	}

	private bool CheckIfAllCardsInHandAreSpellCards () {
		int spellCardCount = 0;

		if (playField.player1Turn) {
			foreach (Transform card in GameObject.Find("Player1 Hand").transform) {
				if (card.GetComponent<Card>().type == "spell") {
					spellCardCount++;
					//Debug.Log("Found a spellcard in hand, spellCardCount is: " + spellCardCount);
				}
			}
		} else if (!playField.player1Turn) {
			foreach (Transform card in GameObject.Find("Player2 Hand").transform) {
				if (card.GetComponent<Card>().type == "spell") {
					spellCardCount++;
					//Debug.Log("Found a spellcard in hand, spellCardCount is: " + spellCardCount);
				}
			}
		}

		if (spellCardCount == 4) {
			return true;
		} else {
			return false;
		}
	}

	private bool CheckIfCardIdIsASpellCard (string cardId) {
		//MAKE SURE YOU UPDATE THIS LIST WHENEVER YOU ADD A NEW SPELL CARD TO ACCOUNT FOR THAT NEW CARD'S ID
		if ( cardId == "armor" || cardId == "might" || cardId == "shroud" || cardId == "root" || cardId == "fireball" || cardId == "heal" ||
			 cardId == "rockthrow" || cardId == "windgust") {
			return true;
		} else {
			return false;
		}
	}

	//Adds the card I just played to my discard pile. We can't add the ACTUAL card object we just played b/c it gets destroyed from your hand, and
	//when that happens it also destroys it from the discard list. SO WHAT WE DO INSTEAD is each spell card has a variable for a 'cardReference' gameobject, 
	//which is a really simple gameobject that just has a gameobject variable that points to the appropriate card gameobject (i.e. instead of referencing
	//the actual card gameobject that was just played, we point to the card gameobject in general)
	public void RemoveCardFromHandAndAddToDiscard () {
		if (Card.selectedCard) {
			Debug.LogWarning("SELECTED CARD EXISTS****************" + Card.selectedCard.GetComponent<Card>().cardId);
		}
		//If the card is a Tower or Wall card DO NOT add it to my discard as the Tower and Wall cards are used more like spells
		if (Card.selectedCard.GetComponent<Card>().cardName != "Tower" && Card.selectedCard.GetComponent<Card> ().cardName != "Wall") {
			if (playField.player1Turn) {
				player1Discard.Add(Card.selectedCard.GetComponent<Card>().cardId);
			} else if (!playField.player1Turn) {
				player2Discard.Add(Card.selectedCard.GetComponent<Card>().cardId);
				//Debug.Log("Adding card to player2 discard pile: " + Card.selectedCard.GetComponent<Card>().cardName);
			}
			//Get rid of the card from my hand that I just used so I can't use it again
			DestroyImmediate (Card.selectedCard.gameObject);
		}
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
