using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Card : MonoBehaviour {

	public static GameObject selectedHero;
	public GameObject heroPrefab;
	public GameObject cardReferenceObject;
	public static GameObject selectedCard;
	public Text NameText, TypeText, PowerText, HealthText, SpeedText, RangeText;
	public string cardName;
	public string type;
	public int quantity;
	public int spellDamage;

	private PlayField playField;
	private Deck deck;

	// Use this for initialization
	void Start () {
		NameText.text = cardName.ToString();
		TypeText.text = type.ToString();
		playField = FindObjectOfType<PlayField>();
		deck = FindObjectOfType<Deck>();
		if (type == "Hero") {
			PowerText.text = heroPrefab.GetComponent<Hero>().power.ToString();
			HealthText.text = heroPrefab.GetComponent<Hero>().maxHealth.ToString();
			SpeedText.text = heroPrefab.GetComponent<Hero>().speed.ToString();
			RangeText.text = heroPrefab.GetComponent<Hero>().range.ToString();
		}
	}

	void OnMouseDown () {
		selectedCard = gameObject;
		if (type == "Hero") {
			selectedHero = heroPrefab;
		}
	}

	public void CastSpell () {
		if (cardName == "Fireball") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y) {
						hero.GetComponent<Hero>().TakeDamage(spellDamage);
						RemoveCardFromHandAndAddToDiscard();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y) {
						hero.GetComponent<Hero>().TakeDamage(spellDamage);
						RemoveCardFromHandAndAddToDiscard();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardName == "Flame Strike") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x) {
						hero.GetComponent<Hero>().TakeDamage(spellDamage);
						RemoveCardFromHandAndAddToDiscard();
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x) {
						hero.GetComponent<Hero>().TakeDamage(spellDamage);
						RemoveCardFromHandAndAddToDiscard();
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardName == "Heal") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health then heal them to full
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth) {
						hero.GetComponent<Hero>().HealFull();
						RemoveCardFromHandAndAddToDiscard();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health then heal them to full
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth) {
						hero.GetComponent<Hero>().HealFull();
						RemoveCardFromHandAndAddToDiscard();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardName == "Haste") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on set 'movingRight' variable in Hero.cs to true
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y) {
						hero.GetComponent<Hero>().usingHaste = true;
						playField.Player1MoveHasteCheck(hero);
						RemoveCardFromHandAndAddToDiscard();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on set 'movingLeft' variable in Hero.cs to true
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y) {
						hero.GetComponent<Hero>().usingHaste = true;
						playField.Player2MoveHasteCheck(hero);
						RemoveCardFromHandAndAddToDiscard();
						return;
					} 
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		}
	}

	//Adds the card I just played to my discard pile. We can't add the ACTUAL card object we just played b/c it gets destroyed from your hand, and
	//when that happens it also destroys it from the discard list. SO WHAT WE DO INSTEAD is each spell card has a variable for a 'cardReference' gameobject, 
	//which is a really simple gameobject that just has a gameobject variable that points to the appropriate card gameobject (i.e. instead of referencing
	//the actual card gameobject that was just played, we point to the card gameobject in general)
	public void RemoveCardFromHandAndAddToDiscard () {
		if (playField.player1Turn) {
			deck.Player1AddCardToDiscard (Card.selectedCard.GetComponent<Card> ().cardReferenceObject.GetComponent<CardReference>().cardReference);
		}
		else if (!playField.player1Turn) {
			deck.Player2AddCardToDiscard (Card.selectedCard.GetComponent<Card> ().cardReferenceObject.GetComponent<CardReference>().cardReference);
		}
		//Get rid of the  card from my hand that I just used so I can't use it again
		Destroy (Card.selectedCard);
		//Set the 'selected hero' and 'selected card' variables to default so the game doesn't think I still have anything selected
		playField.ClearSelectedHeroAndSelectedCard();
		//Increment the integer varaible 'cardsPlayed' to keep track of how many cards the player has succesfully played this turn
		playField.IncrementCardsPlayedCounter();
	}
}
