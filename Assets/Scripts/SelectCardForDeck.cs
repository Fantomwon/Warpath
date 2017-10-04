using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCardForDeck : MonoBehaviour {

	public GameObject associatedCard;
	public Text text;
	public Image checkmark;

	private bool selected = false;
	private CardsSelected cardsSelected;

	void Start () {
		cardsSelected = FindObjectOfType<CardsSelected>();
		if (SceneManager.GetActiveScene().name == "Game") {
			Destroy (gameObject);
		}
	}

	void OnMouseDown () {
		if (selected == false) {
			if (cardsSelected.selectedNumber >= cardsSelected.maxNumber) {
				Debug.Log("CANNOT ADD ANYMORE CARDS TO DECK, CURRENTLY AT MAX CARDS ALLOWED");
				return;
			} else {
				AddCardToDeckSelectList ();
				selected = true;
				text.GetComponent<Text>().enabled = false;
				checkmark.GetComponent<Image>().enabled = true;
				cardsSelected.IncrementSelectedNumer();
			}
		} else if (selected == true) {
			selected = false;
			text.GetComponent<Text>().enabled = true;
			checkmark.GetComponent<Image>().enabled = false;
			RemoveCardFromDeckSelectList ();
			cardsSelected.DecrementSelectedNumer();
		}
	}

	void AddCardToDeckSelectList () {
		if (SceneManager.GetActiveScene().name == "CardSelectP1") {
			if (associatedCard.name == "ArcherCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.archerCard);
			} else if (associatedCard.name == "AssassinCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.assassinCard);
			} else if (associatedCard.name == "BloodmageCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.bloodMageCard);
			} else if (associatedCard.name == "CavalryCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.cavalryCard);
			} else if (associatedCard.name == "DivinerCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.divinerCard);
			} else if (associatedCard.name == "DruidCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.druidCard);
			} else if (associatedCard.name == "FootSoldierCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.footSoldierCard);
			} else if (associatedCard.name == "KnightCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.knightCard);
			} else if (associatedCard.name == "MonkCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.monkCard);
			} else if (associatedCard.name == "RogueCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.rogueCard);	
			}
		} else if (SceneManager.GetActiveScene().name == "CardSelectP2") {
			if (associatedCard.name == "ArcherCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.archerCard);
			} else if (associatedCard.name == "AssassinCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.assassinCard);
			} else if (associatedCard.name == "BloodmageCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.bloodMageCard);
			} else if (associatedCard.name == "CavalryCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.cavalryCard);
			} else if (associatedCard.name == "DivinerCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.divinerCard);
			} else if (associatedCard.name == "DruidCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
			} else if (associatedCard.name == "FootSoldierCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.footSoldierCard);
			} else if (associatedCard.name == "KnightCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.knightCard);
			} else if (associatedCard.name == "MonkCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.monkCard);
			} else if (associatedCard.name == "RogueCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);	
			}
		}
	}

	void RemoveCardFromDeckSelectList () {
		if (SceneManager.GetActiveScene().name == "CardSelectP1") {
			foreach (GameObject card in GlobalObject.instance.player1DeckSelect) {
				if (card.name == associatedCard.name) {
					GlobalObject.instance.player1DeckSelect.Remove(card);
					return;
				}
			}
		} else if (SceneManager.GetActiveScene().name == "CardSelectP2") {
			foreach (GameObject card in GlobalObject.instance.player2DeckSelect) {
				if (card.name == associatedCard.name) {
					GlobalObject.instance.player2DeckSelect.Remove(card);
					return;
				}
			}
		}
	}
}