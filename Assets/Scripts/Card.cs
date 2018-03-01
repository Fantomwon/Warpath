using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Card : MonoBehaviour {

	public static GameObject selectedHero;
	public static GameObject selectedCard;
	public GameObject heroPrefab;
	public GameObject cardReferenceObject;
	public Text ManaCost, NameText, PowerText, HealthText, SpeedText, RangeText;
	public string cardId;
	public string cardName;
	public string type;
	public int manaCost;
	public int quantity;
	public int spellDamage;
	public int spellCooldownStart, spellCooldownCurrent;
	public GameObject spellParticle;

	private PlayField playField;
	private Deck deck;
	private BuffManager buffManager;
	private GameObject player1,player2;

	void Awake () {
		Debug.Log("Running Awake() function for: " + cardName);
		playField = FindObjectOfType<PlayField>();
	}

	// Use this for initialization
	void Start () {
		deck = FindObjectOfType<Deck>();
		buffManager = FindObjectOfType<BuffManager>();
		player1 = GameObject.Find("player1");
		player2 = GameObject.Find("player2");
		ManaCost.text = manaCost.ToString();
		if (type != "Spell") {
			NameText.text = cardName.ToString();
		}
		if (type == "Hero" || cardName == "Tower" || cardName == "Wall") {
			PowerText.text = heroPrefab.GetComponent<Hero>().power.ToString();
			HealthText.text = heroPrefab.GetComponent<Hero>().maxHealth.ToString();
			SpeedText.text = heroPrefab.GetComponent<Hero>().speed.ToString();
			RangeText.text = heroPrefab.GetComponent<Hero>().range.ToString();
		}
	}

	void OnMouseDown () {
		if (spellCooldownCurrent <= 0) {
			selectedCard = gameObject;
			Debug.Log("SELECTED CARD IS " + selectedCard.name);
			if (type == "Hero" || cardName =="Tower" || cardName == "Wall") {
				selectedHero = heroPrefab;
			}
		}
	}

	public void DoSpellDamage (Transform hero,int spellDamage) {
		hero.GetComponent<Hero>().TakeDamage(spellDamage);
	}

	public void CastSpell () { 

		if (cardId == "rockthrow") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them (spell damage is applied through 'EndOfSpellEffects' method which is part of Spell.cs and is attached to
					//the 'Rock' object nested under the 'RockThrowParticle' object)
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						spellParticle.GetComponentInChildren<Spell>().hero = hero;
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, player1.transform);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them (spell damage is applied through 'EndOfSpellEffects' method which is part of Spell.cs and is attached to
					//the 'Rock' object nested under the 'RockThrowParticle' object)
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						spellParticle.GetComponentInChildren<Spell>().hero = hero;
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, player2.transform);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "fireball") {
			Debug.Log("FIREBALL MARKER # 1");
			if (playField.player1Turn) {
				Debug.Log("FIREBALL MARKER # 2");
				foreach (Transform hero in playField.player2.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them (spell damage is applied through 'EndOfSpellEffects' method which is part of Spell.cs and is attached to
					//the 'Fireball' object nested under the 'FireballParticle' object)
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						spellParticle.GetComponentInChildren<Spell>().hero = hero;
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, player1.transform);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				Debug.Log("FIREBALL MARKER # 3");
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them (spell damage is applied through 'EndOfSpellEffects' method which is part of Spell.cs and is attached to
					//the 'Fireball' object nested under the 'FireballParticle' object)
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						spellParticle.GetComponentInChildren<Spell>().hero = hero;
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, player2.transform);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "flamestrike") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If there are any enemies in the COLUMN that I clicked on then do spell damage to all of them (spell damage is applied through 'EndOfSpellEffects' method)
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x) {
						Debug.Log("FOUND A HERO");
						spellParticle.GetComponentInChildren<Spell>().hero = hero;
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, player1.transform);
					} 
				}
				SpellCleanup ();
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If there are any enemies in the COLUMN that I clicked on then do spell damage to all of them (spell damage is applied through 'EndOfSpellEffects' method)
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x) {
						Debug.Log("FOUND A HERO");
						spellParticle.GetComponentInChildren<Spell>().hero = hero;
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, player2.transform);

					}
				}
				SpellCleanup ();
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "heal") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health AND they are not a 'Ghost' then heal them for the proper amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
					    && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth
					    && hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity);
						hero.GetComponent<Hero>().HealPartial(3);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health AND they are not a 'Ghost' then heal them for the proper amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
					    && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth
						&& hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity);
						hero.GetComponent<Hero>().HealPartial(3);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "haste") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on set 'movingRight' variable in Hero.cs to true
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().usingHaste = true;
						playField.Player1MoveHasteCheck(hero);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on set 'movingLeft' variable in Hero.cs to true
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().usingHaste = true;
						playField.Player2MoveHasteCheck(hero);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "might") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on then give them the 'Might' buff for the appropriate duration
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("might", hero);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on then give them the 'Might' buff for the appropriate duration
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("might", hero);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "shroud") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on then give them the 'Shroud' buff for the appropriate duration
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("shroud", hero);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on then give them the 'Shroud' buff for the appropriate duration
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("shroud", hero);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "armor") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on AND they are not a 'Ghost' then add to their armor value by the appropriate amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
						&& hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().AddArmor(2);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on AND they are not a 'Ghost' then add to their armor value by the appropriate amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
						&& hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().AddArmor(2);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "blessing") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on AND they are not a 'Ghost' then add to their armor value by the appropriate amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
						&& hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().HealFull();
						hero.GetComponent<Hero>().AddArmor(3);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on AND they are not a 'Ghost' then add to their armor value by the appropriate amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
						&& hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().HealFull();
						hero.GetComponent<Hero>().AddArmor(3);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "root") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If there is an enemy in the square I clicked on then cast the 'Root' debuff on them
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("root", hero);
						//DoSpellDamage(hero,spellDamage);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then cast the 'Root' debuff on them
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("root", hero);
						//DoSpellDamage(hero,spellDamage);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId == "windgust") {
			float maxDistToMove = 3f;
			float currentDistToMove;
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If there is an enemy in the square I clicked on then do attempt to cast this spell on them
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						if (playField.FindClosestHeroToTheRight(hero) == 1 || hero.transform.position.x == playField.player2HomeColumn) {
							Debug.LogWarning("Cannot move hero b/c there is no space behind them");
							return;
						}

						//If there are NOT any heroes in the way of my maxDistToMove then move that max distance, else move as far as I can
						if (playField.FindClosestHeroToTheRight(hero) > maxDistToMove) {
							currentDistToMove = maxDistToMove + 1;
							//If the currentDistToMove would place me beyond the player2HomeColumn then adjust the value so it only takes me as far as the player2HomeColumn
							if (currentDistToMove + hero.transform.position.x > playField.player2HomeColumn) {
								currentDistToMove = playField.player2HomeColumn - hero.transform.position.x + 1;
							}
						} else {
							currentDistToMove = playField.FindClosestHeroToTheRight(hero);
						}

						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().MoveSingleHeroRight(currentDistToMove - 1);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then do attempt to cast this spell on them
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						if (playField.FindClosestHeroToTheLeft(hero) == 1 || hero.transform.position.x == playField.player1HomeColumn) {
							Debug.LogWarning("Cannot move hero b/c there is no space behind them");
							return;
						}

						//If there are NOT any heroes in the way of my maxDistToMove then move that max distance, else move as far as I can
						if (playField.FindClosestHeroToTheLeft(hero) > maxDistToMove) {
							currentDistToMove = maxDistToMove + 1;
							//If the currentDistToMove would place me beyond the player2HomeColumn then adjust the value so it only takes me as far as the player2HomeColumn
							if (hero.transform.position.x - currentDistToMove < playField.player1HomeColumn) {
								currentDistToMove = hero.transform.position.x - playField.player1HomeColumn + 1;
							}
						} else {
							currentDistToMove = playField.FindClosestHeroToTheLeft(hero);
						}

						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().MoveSingleHeroLeft(currentDistToMove - 1);
						SpellCleanup ();
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		}
	}

	void SpellCleanup ()
	{
		playField.SubtractMana ();

		if (Card.selectedCard.GetComponent<Card>().type == "Spell") {
			SetSpellCooldown ();
		}

		if (Card.selectedCard.GetComponent<Card>().type == "SpellCard") {
			RemoveCardFromHandAndAddToDiscard();
		}

		playField.ClearSelectedHeroAndSelectedCard ();
	}

	public void SetSpellCooldown () {
		spellCooldownCurrent = spellCooldownStart + 1;
		this.GetComponent<Button>().interactable = false;
		this.gameObject.transform.Find("CooldownText").GetComponent<CanvasGroup>().alpha = 1;
		this.gameObject.transform.Find("CooldownText").GetComponent<Text>().text = spellCooldownCurrent.ToString();
	}

	public void ReduceSpellCooldown () {
		if (spellCooldownCurrent > 0) {
			spellCooldownCurrent -= 1;
		}

		this.gameObject.transform.Find("CooldownText").GetComponent<Text>().text = spellCooldownCurrent.ToString();

		if (spellCooldownCurrent <= 0) {
			this.GetComponent<Button>().interactable = true;
			this.gameObject.transform.Find("CooldownText").GetComponent<CanvasGroup>().alpha = 0;
		}
	}

	//Adds the card I just played to my discard pile. We can't add the ACTUAL card object we just played b/c it gets destroyed from your hand, and
	//when that happens it also destroys it from the discard list. SO WHAT WE DO INSTEAD is each spell card has a variable for a 'cardReference' gameobject, 
	//which is a really simple gameobject that just has a gameobject variable that points to the appropriate card gameobject (i.e. instead of referencing
	//the actual card gameobject that was just played, we point to the card gameobject in general)
	public void RemoveCardFromHandAndAddToDiscard () {
		//If the card is a Tower or Wall card DO NOT remove it as the Tower and Wall cards are used more like spells
		if (Card.selectedCard.GetComponent<Card> ().cardName != "Tower" && Card.selectedCard.GetComponent<Card> ().cardName != "Wall") {
			if (playField.player1Turn) {
				deck.Player1AddCardToDiscard (Card.selectedCard.GetComponent<Card>().cardReferenceObject.GetComponent<CardReference>().cardReference);
			} else if (!playField.player1Turn) {
				deck.Player2AddCardToDiscard (Card.selectedCard.GetComponent<Card>().cardReferenceObject.GetComponent<CardReference>().cardReference);
				//Debug.Log("Adding card to player2 discard pile: " + Card.selectedCard.GetComponent<Card>().cardName);
			}
			//Get rid of the  card from my hand that I just used so I can't use it again
			DestroyImmediate (Card.selectedCard.gameObject);
		}
	}
}
