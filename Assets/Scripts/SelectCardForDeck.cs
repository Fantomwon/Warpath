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
		if (SceneManager.GetActiveScene().name == "CardSelectP1" || SceneManager.GetActiveScene().name == "CardSelectSinglePlayer") {
			if (associatedCard.name == "ArcherCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.archerCard);
			} else if (associatedCard.name == "AssassinCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.assassinCard);
			} else if (associatedCard.name == "BlacksmithCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.blacksmithCard);
			} else if (associatedCard.name == "BloodmageCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.bloodMageCard);
			} else if (associatedCard.name == "CavalryCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.cavalryCard);
			} else if (associatedCard.name == "ChampionCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.championCard);
			} else if (associatedCard.name == "ChaosMageCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.chaosMageCard);
			} else if (associatedCard.name == "DivinerCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.divinerCard);
			} else if (associatedCard.name == "DruidCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.druidCard);
			} else if (associatedCard.name == "SlingerCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.slingerCard);
			} else if (associatedCard.name == "DwarfCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.dwarfCard);
			} else if (associatedCard.name == "FootSoldierCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.footSoldierCard);
			} else if (associatedCard.name == "GhostCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.ghostCard);
			} else if (associatedCard.name == "KnightCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.knightCard);
			} else if (associatedCard.name == "MonkCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.monkCard);
			} else if (associatedCard.name == "PaladinCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.paladinCard);
			} else if (associatedCard.name == "RogueCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.rogueCard);	
			} else if (associatedCard.name == "SapperCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.sapperCard);
			} else if (associatedCard.name == "SorcererCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.sorcererCard);	
			} else if (associatedCard.name == "WolfCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.wolfCard);	
			} else if (associatedCard.name == "ArmorCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.armorCard);	
			} else if (associatedCard.name == "BuffMightCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.buffMightCard);	
			} else if (associatedCard.name == "BuffShroudCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.buffShroudCard);	
			} else if (associatedCard.name == "DebuffRootCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.debuffRootCard);	
			} else if (associatedCard.name == "FireballCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.fireballCard);	
			} else if (associatedCard.name == "HealCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.healCard);	
			} else if (associatedCard.name == "RockThrowCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.rockThrowCard);	
			} else if (associatedCard.name == "WindGustCard") {
				GlobalObject.instance.player1DeckSelect.Add(GlobalObject.instance.windGustCard);	
			}
		} else if (SceneManager.GetActiveScene().name == "CardSelectP2") {
			if (associatedCard.name == "ArcherCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.archerCard);
			} else if (associatedCard.name == "AssassinCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.assassinCard);
			} else if (associatedCard.name == "BlacksmithCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.blacksmithCard);
			} else if (associatedCard.name == "BloodmageCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.bloodMageCard);
			} else if (associatedCard.name == "CavalryCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.cavalryCard);
			} else if (associatedCard.name == "ChampionCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.championCard);
			} else if (associatedCard.name == "ChaosMageCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.chaosMageCard);
			} else if (associatedCard.name == "DivinerCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.divinerCard);
			} else if (associatedCard.name == "DruidCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
			} else if (associatedCard.name == "SlingerCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.slingerCard);
			} else if (associatedCard.name == "DwarfCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.dwarfCard);
			} else if (associatedCard.name == "FootSoldierCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.footSoldierCard);
			} else if (associatedCard.name == "GhostCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.ghostCard);
			}  else if (associatedCard.name == "KnightCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.knightCard);
			} else if (associatedCard.name == "MonkCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.monkCard);
			} else if (associatedCard.name == "PaladinCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
			} else if (associatedCard.name == "RogueCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);	
			} else if (associatedCard.name == "SapperCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.sapperCard);
			}  else if (associatedCard.name == "SorcererCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.sorcererCard);	
			} else if (associatedCard.name == "WolfCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.wolfCard);	
			} else if (associatedCard.name == "ArmorCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.armorCard);	
			} else if (associatedCard.name == "BuffMightCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.buffMightCard);	
			} else if (associatedCard.name == "BuffShroudCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.buffShroudCard);	
			} else if (associatedCard.name == "DebuffRootCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.debuffRootCard);	
			} else if (associatedCard.name == "FireballCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.fireballCard);	
			} else if (associatedCard.name == "HealCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.healCard);	
			} else if (associatedCard.name == "RockThrowCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rockThrowCard);	
			} else if (associatedCard.name == "WindGustCard") {
				GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.windGustCard);	
			}
		}
	}

	void RemoveCardFromDeckSelectList () {
		if (SceneManager.GetActiveScene().name == "CardSelectP1" || SceneManager.GetActiveScene().name == "CardSelectSinglePlayer") {
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