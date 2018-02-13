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
			if (associatedCard.GetComponent<Card>().type == "Hero" && cardsSelected.selectedCharacterCardsNumber >= cardsSelected.maxCharacterCards) {
				Debug.Log("CANNOT ADD ANYMORE CHARACTER CARDS TO DECK, CURRENTLY AT MAX CHARACTER CARDS ALLOWED");
				return;
			} else if (associatedCard.GetComponent<Card>().type == "SpellCard" && cardsSelected.selectedSpellCardsNumber >= cardsSelected.maxSpellCards)	 {
				Debug.Log("CANNOT ADD ANYMORE SPELL CARDS TO DECK, CURRENTLY AT MAX SPELL CARDS ALLOWED");
				return;
			} else {
				AddCardToDeckSelectList ();
				selected = true;
				text.GetComponent<Text>().enabled = false;
				checkmark.GetComponent<Image>().enabled = true;

				if (associatedCard.GetComponent<Card>().type == "Hero") {
					cardsSelected.IncrementSelectedNumber("Hero");
				} else if (associatedCard.GetComponent<Card>().type == "SpellCard") {
					cardsSelected.IncrementSelectedNumber("SpellCard");
				}
			}
		} else if (selected == true) {
			selected = false;
			text.GetComponent<Text>().enabled = true;
			checkmark.GetComponent<Image>().enabled = false;
			RemoveCardFromDeckSelectList ();

			if (associatedCard.GetComponent<Card>().type == "Hero") {
				cardsSelected.DecrementSelectedNumber("Hero");
			} else if (associatedCard.GetComponent<Card>().type == "SpellCard") {
				cardsSelected.DecrementSelectedNumber("SpellCard");
			}
		}
	}

	void AddCardToDeckSelectList () {
		//Debug.Log("RUNNING AddCardToDeckSelectList");
		if (SceneManager.GetActiveScene().name == "CardSelectP1" || SceneManager.GetActiveScene().name == "CardSelectSinglePlayer") {
			if (associatedCard.name.Contains("ArcherCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.archerCard);
			} else if (associatedCard.name.Contains("AssassinCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.assassinCard);
			} else if (associatedCard.name.Contains("BlacksmithCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.blacksmithCard);
			} else if (associatedCard.name.Contains("BloodmageCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.bloodMageCard);
			} else if (associatedCard.name.Contains("CavalryCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.cavalryCard);
			} else if (associatedCard.name.Contains("ChampionCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.championCard);
			} else if (associatedCard.name.Contains("ChaosMageCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.chaosMageCard);
			} else if (associatedCard.name.Contains("DivinerCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.divinerCard);
			} else if (associatedCard.name.Contains("DruidCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.druidCard);
			} else if (associatedCard.name.Contains("SlingerCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.slingerCard);
			} else if (associatedCard.name.Contains("DwarfCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.dwarfCard);
			} else if (associatedCard.name.Contains("FootSoldierCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.footSoldierCard);
			} else if (associatedCard.name.Contains("GhostCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.ghostCard);
			} else if (associatedCard.name.Contains("KnightCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.knightCard);
			} else if (associatedCard.name.Contains("MonkCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.monkCard);
			} else if (associatedCard.name.Contains("PaladinCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.paladinCard);
			} else if (associatedCard.name.Contains("RogueCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.rogueCard);	
			} else if (associatedCard.name.Contains("SapperCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.sapperCard);
			} else if (associatedCard.name.Contains("SorcererCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.sorcererCard);	
			} else if (associatedCard.name.Contains("WolfCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.wolfCard);	
			} else if (associatedCard.name.Contains("ArmorCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.armorCard);	
			} else if (associatedCard.name.Contains("BuffMightCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.buffMightCard);	
			} else if (associatedCard.name.Contains("BuffShroudCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.buffShroudCard);	
			} else if (associatedCard.name.Contains("DebuffRootCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.debuffRootCard);	
			} else if (associatedCard.name.Contains("FireballCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.fireballCard);	
			} else if (associatedCard.name.Contains("HealCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.healCard);	
			} else if (associatedCard.name.Contains("RockThrowCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.rockThrowCard);	
			} else if (associatedCard.name.Contains("WindGustCard")) {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.windGustCard);	
			}
		} else if (SceneManager.GetActiveScene().name == "CardSelectP2") {
			if (associatedCard.name.Contains("ArcherCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.archerCard);
			} else if (associatedCard.name.Contains("AssassinCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.assassinCard);
			} else if (associatedCard.name.Contains("BlacksmithCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.blacksmithCard);
			} else if (associatedCard.name.Contains("BloodmageCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.bloodMageCard);
			} else if (associatedCard.name.Contains("CavalryCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.cavalryCard);
			} else if (associatedCard.name.Contains("ChampionCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.championCard);
			} else if (associatedCard.name.Contains("ChaosMageCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.chaosMageCard);
			} else if (associatedCard.name.Contains("DivinerCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.divinerCard);
			} else if (associatedCard.name.Contains("DruidCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
			} else if (associatedCard.name.Contains("SlingerCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.slingerCard);
			} else if (associatedCard.name.Contains("DwarfCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.dwarfCard);
			} else if (associatedCard.name.Contains("FootSoldierCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.footSoldierCard);
			} else if (associatedCard.name.Contains("GhostCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.ghostCard);
			} else if (associatedCard.name.Contains("KnightCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.knightCard);
			} else if (associatedCard.name.Contains("MonkCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.monkCard);
			} else if (associatedCard.name.Contains("PaladinCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
			} else if (associatedCard.name.Contains("RogueCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);	
			} else if (associatedCard.name.Contains("SapperCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.sapperCard);
			} else if (associatedCard.name.Contains("SorcererCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.sorcererCard);	
			} else if (associatedCard.name.Contains("WolfCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.wolfCard);	
			} else if (associatedCard.name.Contains("ArmorCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.armorCard);	
			} else if (associatedCard.name.Contains("BuffMightCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.buffMightCard);	
			} else if (associatedCard.name.Contains("BuffShroudCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.buffShroudCard);	
			} else if (associatedCard.name.Contains("DebuffRootCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.debuffRootCard);	
			} else if (associatedCard.name.Contains("FireballCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.fireballCard);	
			} else if (associatedCard.name.Contains("HealCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.healCard);	
			} else if (associatedCard.name.Contains("RockThrowCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rockThrowCard);	
			} else if (associatedCard.name.Contains("WindGustCard")) {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.windGustCard);	
			}
		}
	}

	void RemoveCardFromDeckSelectList () {
		if (SceneManager.GetActiveScene().name == "CardSelectP1" || SceneManager.GetActiveScene().name == "CardSelectSinglePlayer") {
			foreach (GameObject card in GlobalObject.instance.player1DeckSelect) {
				if (associatedCard.name.Contains(card.name)) {
					GlobalObject.instance.player1DeckSelect.Remove(card);
					return;
				}
			}
		} else if (SceneManager.GetActiveScene().name == "CardSelectP2") {
			foreach (GameObject card in GlobalObject.instance.player2DeckSelect) {
				if (associatedCard.name.Contains(card.name)) {
					GlobalObject.instance.player2DeckSelect.Remove(card);
					return;
				}
			}
		}
	}
}