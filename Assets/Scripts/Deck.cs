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
	public Text cardsRemaining;

	private GlobalObject globalObject;
	private AiManager aiManager;
	private string player1Class, player2Class;
	private int maxHandSize = 5;
	private PlayField playField;

    public Commander player1Commander;
    public Commander player2Commander;

    void Awake() {
        SceneManager.sceneLoaded += this.OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == GameConstants.SCENE_INDEX_GAME_COMMANDERS) {
            BuildDeck();
            ShuffleDeck(player1Deck);
            if (!GlobalObject.storyEnabled) {
                ShuffleDeck(player2Deck);
            }
            globalObject = GameObject.FindObjectOfType<GlobalObject>();
            playField = GameObject.FindObjectOfType<PlayField>();
            aiManager = GameObject.FindObjectOfType<AiManager>();
            if (GlobalObject.instance.useCommanders) {
                //TODO - may need to load specific assets here - DECK START BATTLE
            }
        }
    }

	void BuildDeck () {
        foreach (CardEntry cardEntry in GlobalObject.instance.humanPlayerCommanderData.Deck) {
            for( int i = 0; i <cardEntry.CardAmount; i++) {
                player1Deck.Add(cardEntry.Card.ToString());
            }
        }

        foreach (string cardId in GlobalObject.instance.player2DeckSelect) {
            player2Deck.Add(cardId.ToString());
        }
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
                    //Instantiate the new Spell Card or Hero Card in my hand
					if (!CheckIfCardIdIsASpellCard(player1Deck[0])) {
						globalObject.SetTemplateHeroCardAttributes(player1Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateHeroCard) as GameObject;
						newCard.transform.SetParent (GameObject.Find ("Player1 Hand").transform, false);
						globalObject.SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId.ToString(), newCard);
					} else if (CheckIfCardIdIsASpellCard(player1Deck[0])) {
						globalObject.SetTemplateSpellCardAttributes(player1Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateSpellCard) as GameObject;
						newCard.transform.SetParent (GameObject.Find ("Player1 Hand").transform, false);
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

                //Need to change this to just check for a "card type" of type "spell" or something rather than explicitly listing each spell here
				if (CheckIfAllCardsInHandAreSpellCards() && (player2Deck[0] == "armor" || player2Deck[0] == "might" || player2Deck[0] == "shroud" || player2Deck[0] == "root" || 
					player2Deck[0] == "fireball" || player2Deck[0] == "heal" || player2Deck[0] == "rockthrow" || player2Deck[0] == "windgust")) {
					Debug.Log("4 SPELL CARDS IN HAND, NOT LETTING THAT HAPPEN!");
					string nextCardInDeck = player2Deck[0];
					player2Deck.RemoveAt(0);
					player2Discard.Add(nextCardInDeck);
					Player2DealCards();
					//player2Deck.Insert(Random.Range(1, player2Deck.Count - 1), nextCardInDeck);
				} else {
                    Debug.Log("Player 2 deal cards! player2Deck count:" + player2Deck.Count.ToString());
					if (!CheckIfCardIdIsASpellCard(player2Deck[0])) {
						globalObject.SetTemplateHeroCardAttributes(player2Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateHeroCard) as GameObject;
						//Alter card cost if ai is enabled
						if ( GlobalObject.aiEnabled == true ) {
							//Debug.Log("SETTING NEW MANA COST FOR CARD");
							newCard.GetComponent<Card>().manaCost = aiManager.AiAlterCardCost(newCard);
						}
						newCard.transform.SetParent (GameObject.Find ("Player2 Hand").transform, false);
						globalObject.SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId.ToString(), newCard);
					} else if (CheckIfCardIdIsASpellCard(player2Deck[0])) {
						globalObject.SetTemplateSpellCardAttributes(player2Deck[0]);
						GameObject newCard = Instantiate (globalObject.templateSpellCard) as GameObject;
						//Alter card cost if ai is enabled
						if ( GlobalObject.aiEnabled == true ) {
							//Debug.Log("SETTING NEW MANA COST FOR CARD");
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
			 cardId == "rockthrow" || cardId == "windgust" || cardId == "drainlife" || cardId == "blessing" || cardId == "haste") {
			return true;
		} else {
			return false;
		}
	}

	public void RemoveCardFromHandAndAddToDiscard () {
		//if (Card.selectedCard) {
		//	Debug.LogWarning("SELECTED CARD EXISTS****************" + Card.selectedCard.GetComponent<Card>().cardId);
		//}
		//If the card is a Tower or Wall card DO NOT add it to my discard as the Tower and Wall cards are used more like spells
		if (Card.selectedCard.GetComponent<Card>().cardName != "Tower" && Card.selectedCard.GetComponent<Card> ().cardName != "Wall") {
			if (playField.player1Turn) {
				player1Discard.Add(Card.selectedCard.GetComponent<Card>().cardId.ToString());
			} else if (!playField.player1Turn) {
				player2Discard.Add(Card.selectedCard.GetComponent<Card>().cardId.ToString());
				//Debug.Log("Adding card to player2 discard pile: " + Card.selectedCard.GetComponent<Card>().cardName);
			}
			//Get rid of the card from my hand that I just used so I can't use it again
			DestroyImmediate (Card.selectedCard.gameObject);
		}
	}
}
