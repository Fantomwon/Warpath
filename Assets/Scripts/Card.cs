using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public static GameObject selectedHero;
	public static GameObject selectedCard;
	public GameObject heroPrefab;
	public Text ManaCost, NameText, PowerText, HealthText, SpeedText, RangeText;
	public GameConstants.Card cardId;
	public string cardName;
	public string type;
	public GameObject image;
    public int power;
    public int maxHealth;
    public int speed;
    public int range;
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
		//Debug.Log("Running Awake() function for: " + cardName);
		playField = FindObjectOfType<PlayField>();

        //Need to set all of this in Awake() and NOT Start() or else on the NPC's first turn the stats don't get set in time before the NPC spawns their first hero for some reason
        ManaCost.text = manaCost.ToString();
        if (type != "Spell") {
            NameText.text = cardName.ToString();
        }
        if (type == "hero" || type == "heroStationary") {
            //Set text values on the card
            PowerText.text = power.ToString();
            HealthText.text = maxHealth.ToString();
            SpeedText.text = speed.ToString();
            RangeText.text = range.ToString();
            //Set the hero stats on the hero prefab associated with this card
            //heroPrefab.GetComponent<Hero>().power = power;
            //heroPrefab.GetComponent<Hero>().maxHealth = maxHealth;
            //heroPrefab.GetComponent<Hero>().currentHealth = maxHealth;
            //heroPrefab.GetComponent<Hero>().range = range;
            //heroPrefab.GetComponent<Hero>().speed = speed;
            //heroPrefab.GetComponent<Hero>().id = cardId;
        }
    }

	// Use this for initialization
	void Start () {
		deck = FindObjectOfType<Deck>();
		buffManager = FindObjectOfType<BuffManager>();
		player1 = GameObject.Find("player1");
		player2 = GameObject.Find("player2");
	}

	void OnMouseDown () {
		if (spellCooldownCurrent <= 0) {
			selectedCard = gameObject;
			Debug.Log("SELECTED CARD IS " + selectedCard.name);
			if (type == "hero" || type == "heroStationary") {
				selectedHero = heroPrefab;
			}
		}
	}

	public void DoSpellDamage (Transform hero,int spellDamage) {
        PlayField.instance.DamageHero(hero.GetComponent<Hero>(), spellDamage);
	}

	public void CastSpell () { 
		if (cardId.ToString() == "rockthrow") {
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
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
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
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
				Transform hero;
				hero = playField.TargetSpellCheckEntireBoardOneRandomHero("enemy")[0];
				spellParticle.GetComponentInChildren<Spell>().hero = hero;
				Instantiate(spellParticle, hero.transform.localPosition, Quaternion.identity);
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "fireball") {
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
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
				Debug.Log("FIREBALL MARKER # 3");
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them (spell damage is applied through 'EndOfSpellEffects' method which is part of Spell.cs and is attached to
					//the 'Fireball' object nested under the 'FireballParticle' object)
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						spellParticle.GetComponentInChildren<Spell>().hero = hero;
						Instantiate(spellParticle, hero.transform.localPosition, Quaternion.identity, player2.transform);
						SpellCleanup ();
						return;
					}
				}
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
				Debug.Log("FIREBALL MARKER # 4");
				Transform hero;
				hero = playField.TargetSpellCheckEntireBoardOneRandomHero("enemy")[0];
				//Debug.Log("The hero I've found is: " + hero.GetComponent<Hero>().name);
				spellParticle.GetComponentInChildren<Spell>().hero = hero;
				//Debug.Log("spellParticle is: " + spellParticle.name);
				Instantiate(spellParticle, hero.transform.localPosition, Quaternion.identity);
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "flamestrike") {
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
		} else if (cardId.ToString() == "heal") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health AND they are not a 'Ghost' then heal them for the proper amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
					    && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth
					    && hero.GetComponent<Hero>().id != "ghost") {
						//Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity);
						hero.GetComponent<Hero>().HealPartial(2);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health AND they are not a 'Ghost' then heal them for the proper amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
					    && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth
						&& hero.GetComponent<Hero>().id != "ghost") {
						//Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity);
						hero.GetComponent<Hero>().HealPartial(2);
						SpellCleanup ();
						return;
					}
				}
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
                //Heal a random ally
                List<Transform> allies = playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "heal");
                if (allies.Count() > 0) {
                    Transform allyToHeal = allies[0];
                    allyToHeal.GetComponent<Hero>().HealPartial(2);
                }
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "haste") {
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
		} else if (cardId.ToString() == "might") {
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
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled ) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on then give them the 'Might' buff for the appropriate duration
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("might", hero);
						SpellCleanup ();
						return;
					}
				}
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
				Transform hero;
				hero = playField.TargetSpellCheckEntireBoardOneRandomHero("ally","might")[0];
				Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
				buffManager.ApplyBuff("might", hero);
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "shroud") {
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
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on then give them the 'Shroud' buff for the appropriate duration
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						buffManager.ApplyBuff("shroud", hero);
						SpellCleanup ();
						return;
					}
				}
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
				Transform hero;
				hero = playField.TargetSpellCheckEntireBoardOneRandomHero("ally","shroud")[0];
				Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
				buffManager.ApplyBuff("shroud", hero);
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "armor") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on AND they are not a 'Ghost' then add to their armor value by the appropriate amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
						&& hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().AddArmor(1);
						SpellCleanup ();
						return;
					} 
				}
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on AND they are not a 'Ghost' then add to their armor value by the appropriate amount
					if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y
						&& hero.GetComponent<Hero>().id != "ghost") {
						Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
						hero.GetComponent<Hero>().AddArmor(1);
						SpellCleanup ();
						return;
					}
				}
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
				Transform hero;
				hero = playField.TargetSpellCheckEntireBoardOneRandomHero("ally","armor")[0];
				Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
				hero.GetComponent<Hero>().AddArmor(1);
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "blessing") {
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
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
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
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
				Transform hero;
				hero = playField.TargetSpellCheckEntireBoardOneRandomHero("ally","blessing")[0];
				Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
				hero.GetComponent<Hero>().HealFull();
				hero.GetComponent<Hero>().AddArmor(3);
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "root") {
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
			} else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
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
			} else if (!playField.player1Turn && GlobalObject.aiEnabled) {
				Transform hero;
				hero = playField.TargetSpellCheckEntireBoardOneRandomHero("enemy","root")[0];
				Instantiate(spellParticle,hero.transform.localPosition, Quaternion.identity, hero.transform);
				buffManager.ApplyBuff("root", hero);
				//DoSpellDamage(hero,spellDamage);
				SpellCleanup ();
				return;
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardId.ToString() == "windgust") {
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
		} else if (cardId.ToString() == "drainlife") {
            if (playField.player1Turn) {
                foreach (Transform hero in playField.player2.transform) {
                    //Logic for player 1 selecting an enemy hero
                    if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
                        //TODO Replace with a new unique particle when we have one for this Spell
                        ParticleSystem hitParticle = hero.GetComponent<Hero>().hitParticle;
                        Instantiate(hitParticle, hero.transform.position, Quaternion.identity);
                        //Do the damage to the enemy
                        PlayField.instance.DamageHero(hero.GetComponent<Hero>(), 1);
                        //Heal a random ally
                        List<Transform> allies = playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "heal");
                        if (allies.Count() > 0) {
                            Transform allyToHeal = allies[0];
                            allyToHeal.GetComponent<Hero>().HealPartial(1);
                        }
                        SpellCleanup();
                        return;
                    }
                }
            } else if (!playField.player1Turn && !GlobalObject.aiEnabled) {
                foreach (Transform hero in playField.player1.transform) {
                    //Logic for player 2 selecting an enemy hero
                    if (Mathf.RoundToInt(hero.transform.position.x) == playField.roundedPos.x && Mathf.RoundToInt(hero.transform.position.y) == playField.roundedPos.y) {
                        //TODO Replace with a new unique particle when we have one for this Spell
                        ParticleSystem hitParticle = hero.GetComponent<Hero>().hitParticle;
                        Instantiate(hitParticle, hero.transform.position, Quaternion.identity);
                        //Do the damage to the enemy
                        PlayField.instance.DamageHero(hero.GetComponent<Hero>(), 1);
                        //Heal a random ally
                        List<Transform> allies = playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "heal");
                        if (allies.Count() > 0) {
                            Transform allyToHeal = allies[0];
                            allyToHeal.GetComponent<Hero>().HealPartial(1);
                        }
                        SpellCleanup();
                        return;
                    }
                }
            } else if (!playField.player1Turn && GlobalObject.aiEnabled) {
                //Logic for the AI selecting an enemy hero
                //Find a player soldier to attack
                Transform heroToAttack;
                heroToAttack = playField.TargetSpellCheckEntireBoardOneRandomHero("enemy")[0];
                //TODO Replace with a new unique particle when we have one for this Spell
                ParticleSystem hitParticle = heroToAttack.GetComponent<Hero>().hitParticle;
                Instantiate(hitParticle, heroToAttack.transform.position, Quaternion.identity);
                //Do the damage to the enemy
                PlayField.instance.DamageHero( heroToAttack.GetComponent<Hero>(), 1 );
                //Heal a random ally
                List<Transform> allies = playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "heal");
                if (allies.Count() > 0) {
                    Transform allyToHeal = allies[0];
                    allyToHeal.GetComponent<Hero>().HealPartial(1);
                }
                SpellCleanup();
                return;
            }
            Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
        }
    }

	public bool SpellAiConditionCheck () {
		if (cardId.ToString() == "fireball" && playField.TargetSpellCheckEntireBoardOneRandomHero("enemy").Count > 0) {
			return true;
		} else if (cardId.ToString() == "rockthrow" && playField.TargetSpellCheckEntireBoardOneRandomHero("enemy").Count > 0) {
			return true;
		} else if (cardId.ToString() == "blessing" && playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "blessing").Count > 0) {
			return true;
		} else if (cardId.ToString() == "heal" && playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "heal").Count > 0) {
			return true;
		} else if (cardId.ToString() == "armor" && playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "armor").Count > 0) {
			return true; 
		} else if (cardId.ToString() == "might" && playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "might").Count > 0) {
			return true; 
		} else if (cardId.ToString() == "shroud" && playField.TargetSpellCheckEntireBoardOneRandomHero("ally", "shroud").Count > 0) {
			return true; 
		} else if (cardId.ToString() == "root" && playField.TargetSpellCheckEntireBoardOneRandomHero("enemy", "root").Count > 0) {
			return true;
        } else if (cardId.ToString() == "drainlife" && playField.TargetSpellCheckEntireBoardOneRandomHero("enemy").Count > 0) {
            return true;
        } else if (cardId.ToString() == "haste" && playField.TargetSpellCheckEntireBoardOneRandomHero("ally").Count > 0) {
            return true;
        } else {
			return false;
		}
	}

	void SpellCleanup () {
		playField.SubtractMana ();

		if (Card.selectedCard.GetComponent<Card>().type == "Spell") {
			Debug.LogError("Card.selectedCard is a SPELL ALERT ALERT ALERT");
			SetSpellCooldown ();
			playField.ClearSelectedHeroAndSelectedCard ();
		}

		if (Card.selectedCard && Card.selectedCard.GetComponent<Card>().type == "spell" && ((!GlobalObject.aiEnabled) || (GlobalObject.aiEnabled && playField.player1Turn)) ) {
			deck.RemoveCardFromHandAndAddToDiscard();
		}

		//If Ai is enabled and the card is a spell card then destroy that card then RETURN and do not add that card to my discard pile like we would normally do
		if (Card.selectedCard && Card.selectedCard.GetComponent<Card>().type == "SpellCardForced") {
			Debug.Log("NOT ADDING THIS CARD TO DISCARD B/C AI IS ENABLED AND I SHOULDN'T DO THAT");
			DestroyImmediate (Card.selectedCard.gameObject);
			playField.ClearSelectedHeroAndSelectedCard ();
		}
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
}
