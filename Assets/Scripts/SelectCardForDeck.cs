using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCardForDeck : MonoBehaviour {

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
			if (GetComponentInParent<Card>().type == "hero" && cardsSelected.selectedCharacterCardsNumber >= cardsSelected.maxCharacterCards) {
				Debug.Log("CANNOT ADD ANYMORE CHARACTER CARDS TO DECK, CURRENTLY AT MAX CHARACTER CARDS ALLOWED");
				return;
			} else if (GetComponentInParent<Card>().type == "spell" && cardsSelected.selectedSpellCardsNumber >= cardsSelected.maxSpellCards)	 {
				Debug.Log("CANNOT ADD ANYMORE SPELL CARDS TO DECK, CURRENTLY AT MAX SPELL CARDS ALLOWED");
				return;
			} else {
				GlobalObject.instance.player1DeckSelect.Add(GetComponentInParent<Card>().cardId);
				selected = true;
				text.GetComponent<Text>().enabled = false;
				checkmark.GetComponent<Image>().enabled = true;

				if (GetComponentInParent<Card>().type == "hero") {
					cardsSelected.IncrementSelectedNumber("hero");
				} else if (GetComponentInParent<Card>().type == "spell") {
					cardsSelected.IncrementSelectedNumber("spell");
				}
			}
		} else if (selected == true) {
			selected = false;
			text.GetComponent<Text>().enabled = true;
			checkmark.GetComponent<Image>().enabled = false;
			RemoveCardFromDeckSelectList ();

			if (GetComponentInParent<Card>().type == "hero") {
				cardsSelected.DecrementSelectedNumber("hero");
			} else if (GetComponentInParent<Card>().type == "spell") {
				cardsSelected.DecrementSelectedNumber("spell");
			}
		}
	}

	void RemoveCardFromDeckSelectList () {
		if (SceneManager.GetActiveScene().name == "CardSelectP1" || SceneManager.GetActiveScene().name == "CardSelectSinglePlayer") {
			foreach (string cardId in GlobalObject.instance.player1DeckSelect) {
				if (GetComponentInParent<Card>().cardId == cardId) {
					GlobalObject.instance.player1DeckSelect.Remove(cardId);
					return;
				}
			}
		} else if (SceneManager.GetActiveScene().name == "CardSelectP2") {
			foreach (string cardId in GlobalObject.instance.player2DeckSelect) {
				if (GetComponentInParent<Card>().cardId == cardId) {
					GlobalObject.instance.player2DeckSelect.Remove(cardId);
					return;
				}
			}
		}
	}
}