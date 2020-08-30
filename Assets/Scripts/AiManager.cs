using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AiManager : MonoBehaviour {

	public List<Vector2> validHeroSpawnCoords;
	public List<Vector2> validHeroSpawnCoordsCopy;
	public List<Vector2> openRowCoords;
	public List<GameObject> validCardsToPlay;
	public GameObject archer,assassin,bloodmage,cavalry,diviner,druid,footsoldier,knight,monk,rogue,tower,wall;
	public int aiSequenceTracker;
	public Transform player2Hand;

	private int aiTurnTracker;
	private PlayField playField;
	private GlobalObject globalObject;
	private Deck deck;
	//private Deck deck;
	private Card card;

	// Use this for initialization - Test for commit
	void Start () {
		aiSequenceTracker = 0;
		globalObject  = GameObject.FindObjectOfType<GlobalObject>();
		playField = GameObject.FindObjectOfType<PlayField>();
		deck = GameObject.FindObjectOfType<Deck>();
		card = GameObject.FindObjectOfType<Card>();
		AiStoryInitialBoardSetup ();
	}

	public void AiTakeTurn () {
        Debug.Log("AI TAKING ITS TURN!!!");
        //		AiSelectHeroCardToPlay();
        //		AiPlayHeroCard();

        //Evaluate if the NPC commander's resource has reached cost
        if(GlobalObject.instance.useCommanders) {
            this.AiTryUseCommanderAbility();
        }

		if (aiSequenceTracker >= 1) {
			AiSelectCardToPlay();
			AiPlayCard();
			//Debug.Log("aiSequenceTracker is at: " + aiSequenceTracker);
		} else if (aiSequenceTracker >= 20000) {
			//Debug.Log("RUNNING aiSequenceTracker - SEQUENCE # 2");
			AiForcePlaySpellCard("fireball");
			aiSequenceTracker = 0;
		}
	}

	public int AiAlterCardCost ( GameObject cardToCheck ) {
	//	Debug.Log("RUNNING AiAlterCardCost");
		if (GlobalObject.currentlyActiveStory == "boss01") {
			switch (cardToCheck.GetComponent<Card>().cardId) {
    //            case "archer":
    //                return 3;
    //            case "slinger":
				//	return 4;
				//case "assassin":
				//	return 5;
    //            case "sapper":
    //                return 2;
    //            case "rockthrow":
    //                return 1;
    //            case "fireball":
    //                return 3;
                default:
					return cardToCheck.GetComponent<Card>().manaCost;
			}
		} else if (GlobalObject.currentlyActiveStory == "boss02") {
			switch (cardToCheck.GetComponent<Card>().cardId) {
				case "archer":
					return 0;
				default:
					return cardToCheck.GetComponent<Card>().manaCost;
			}
		} else {
			return cardToCheck.GetComponent<Card>().manaCost;
		}
	}

	private void AiSelectCardToPlay () {
        //Clear the 'validCardsToPlay' list so we can build it so fresh and so clean clean
        validCardsToPlay.Clear();

		//Add all cards that the player has enough mana to play to the 'validCardsToPlay' list
		foreach (Transform currentCard in GameObject.Find("Player2 Hand").transform) {
			//Debug.LogWarning("FOUND a CaRd In Player2's HaND");
			if (currentCard.GetComponent<Card>().manaCost <= playField.player2Mana) {
				//If the card is a spell card and it doesn't pass its 'condition check', do not add it to the 'validCardsToPlay' list
				if (currentCard.GetComponent<Card>().type == "spell" && !currentCard.GetComponent<Card>().SpellAiConditionCheck()) {
					Debug.LogError("SPELL CARD CANNOT BE CAST, NOT ADDING TO LIST OF VALID CARDS TO PLAY");
				} else {
					//Add this card to the 'validCardsToPlay' list
					validCardsToPlay.Add(currentCard.gameObject);	
				}
			}
		}

		//Set selectedCard and selectedHero to a random card gameobject in the 'validCardsToPlay' list that was built above 
		if (validCardsToPlay.ToArray().Length > 0) {
//			Debug.Log("validCardsToPLay length is: " + validCardsToPlay.ToArray().Length);
			int validCardsListLength = validCardsToPlay.ToArray().Length;
//			Debug.Log("temp is: " + temp);
			//Choose a random index
			int randomIndex = Random.Range(0, validCardsListLength);
//			Debug.Log("randomIndex is: " + randomIndex);
			Card.selectedCard = validCardsToPlay[randomIndex];
            Debug.Log("Card.selectedCard is: " + Card.selectedCard.GetComponent<Card>().cardId);
            if (validCardsToPlay[randomIndex].GetComponent<Card>().type == "hero") {
				Card.selectedHero = validCardsToPlay[randomIndex].GetComponent<Card>().heroPrefab;
			}

		}
	}

	public void AiPlayCard () {
		if (Card.selectedHero && !CheckIfHomeRowIsFull() && Card.selectedCard && Card.selectedCard.GetComponent<Card>().type != "spell") {
			if (Card.selectedCard.GetComponent<Card>().cardId == "wolf") {
				playField.SpawnHeroForPlayer2(ReturnValidHeroSpawnCoords());
				StartCoroutine("AiTakeTurnAfterDelay");
			} else {
				playField.SpawnHeroForPlayer2(ReturnValidHeroSpawnCoords());
				StartCoroutine("AiTakeTurnAfterDelay");
			}
		} else if ( Card.selectedCard && Card.selectedCard.GetComponent<Card>().type == "spell" ) {
			Card.selectedCard.GetComponent<Card>().CastSpell();
			StartCoroutine("AiTakeTurnAfterDelay");
		} else {
			playField.EndTurn();
		}
	}

	private void AiForcePlaySpellCard (string spellCardId) {
		if (spellCardId == "fireball") {
			globalObject.SetTemplateSpellCardAttributes(spellCardId);
			GameObject spawnedSpellCard = Instantiate (globalObject.templateHeroCard, player2Hand.transform) as GameObject;
			spawnedSpellCard.GetComponent<Card>().manaCost = AiAlterCardCost(spawnedSpellCard);
			spawnedSpellCard.GetComponent<Card>().cardId = "SpellCardForced";
			Card.selectedCard = spawnedSpellCard.gameObject;
		}

		//If a spell card was selected, cast it
		if (Card.selectedCard) {
			Card.selectedCard.GetComponent<Card>().CastSpell();
			StartCoroutine("AiEndTurnAfterDelay");
		}
	}

    private bool AiTryUseCommanderAbility() {
        bool abilitySuccessfullyCast = false;
        //Cache enemy commander reference
        Commander enemyCommander = PlayField.instance.enemyCommander;
        //Early out as false if commander didn't have enough resource to cast
        if( enemyCommander.currentAbilityCharge < enemyCommander.abilityChargeCost) {
            return abilitySuccessfullyCast;
        }
        //Evaluate if the necessary condition is met to cast based on the ability type
        switch (enemyCommander.abilityTargetType) {
            case (GameConstants.CommanderAbilityTargetType.Enemy):
                //Verify existence of at least one human player soldier
                List<Transform> potentialEnemyTargets = new List<Transform>();
                potentialEnemyTargets = PlayField.instance.TargetSpellCheckEntireBoardOneRandomHero("enemy");
                if( potentialEnemyTargets.Count > 0) {
                    //Get a random target from the list
                    int randomIndex = Random.Range(0, potentialEnemyTargets.Count);
                    Transform target = potentialEnemyTargets[randomIndex];
                    //Attempt to activate the commander ability on the selected target
                    abilitySuccessfullyCast = enemyCommander.ActivateCommanderAbility(target.GetComponent<Hero>());
                }
                break;
            case (GameConstants.CommanderAbilityTargetType.Ally):
                List<Transform> potentialAllyTargets = new List<Transform>();
                potentialAllyTargets = PlayField.instance.TargetSpellCheckEntireBoardOneRandomHero("ally", "heal");
                if( potentialAllyTargets.Count > 0) {
                    //Get a random target from the list
                    int randomIndex = Random.Range(0, potentialAllyTargets.Count);
                    Transform target = potentialAllyTargets[randomIndex];
                    //Attempt to activate the commander ability on the selected target
                    abilitySuccessfullyCast = enemyCommander.ActivateCommanderAbility(target.GetComponent<Hero>());
                }
                break;
            default:
                Debug.LogWarning("AiManager attempt to cast commander ability did not find ability type!");
                break;
        }
        return abilitySuccessfullyCast;
    }

    IEnumerator AiTakeTurnAfterDelay () {
		deck.RemoveCardFromHandAndAddToDiscard();
		playField.ClearSelectedHeroAndSelectedCard ();
		yield return new WaitForSeconds(1f);
		AiTakeTurn ();
	}

	IEnumerator AiEndTurnAfterDelay () {
		yield return new WaitForSeconds(2f);
		playField.EndTurn();
	}

	private Vector2 ReturnValidHeroSpawnCoords () {
		//Clear the valid hero spawn coords list and add all of the possible valid spawn coords back to the list
		validHeroSpawnCoords.Clear();
		validHeroSpawnCoordsCopy.Clear();
		for (int y = 1; y < 6; y++) {
			validHeroSpawnCoords.Add(new Vector2(7,y));
			validHeroSpawnCoordsCopy.Add(new Vector2(7,y));
		}
		//Go through each coordinate in the player 2 home row and remove the coords that already have a hero there (invalid coords don't belong in this list yo!)
		foreach (Vector2 validCoord in validHeroSpawnCoordsCopy) {
			foreach (Transform hero in playField.player2.transform) {
				if (validCoord.x == hero.transform.position.x && validCoord.y == hero.transform.position.y) {
					validHeroSpawnCoords.Remove(validCoord);
				}
			}
		}

		//Check to see if there is an enemy hero that is unimpeded in a row
		List<Vector2> highPriorityCoords = new List<Vector2>();
		Vector2 highestPriorityCoord = new Vector2(0,0);

		foreach (Vector2 validCoord in validHeroSpawnCoords) {
			bool spawnMe = false;
			foreach (Transform hero in playField.player1.transform) {
				if (validCoord.y == hero.transform.position.y) {
					spawnMe = true;
				}
			}
			foreach (Transform hero in playField.player2.transform) {
				if (validCoord.y == hero.transform.position.y) {
					spawnMe = false;
				}
			}
			if (spawnMe) {
				highPriorityCoords.Add(validCoord);
				//return validCoord;
			}
		}

		//If there are any rows that have unimpeded heroes, search through those rows for the one that has the hero that is closest to scoring and spawn a hero there to block them
		if (highPriorityCoords.ToArray().Length > 0) {
			Vector2 closestEnemyCoord = new Vector2(0,0);
			foreach (Vector2 highPriorityCoord in highPriorityCoords) {
				foreach (Transform hero in playField.player1.transform) {
					if (hero.transform.position.y == highPriorityCoord.y && hero.transform.position.x > closestEnemyCoord.x) {
						closestEnemyCoord.x = hero.transform.position.x;
						closestEnemyCoord.y = hero.transform.position.y;
						highestPriorityCoord = highPriorityCoord;
					}
				}
			}
			return highestPriorityCoord;
		}

		//If hero is a melee hero try to spawn it in a row that doesn't have another melee hero already in it
		//List<Vector2> openRowCoords = new List<Vector2>();

		if (Card.selectedCard.GetComponent<Card>().range == 1) {
			openRowCoords.Clear();
			//Debug.Log("SPAWNCHECK: SPAWNING A HERO WITH RANGE == 1");
			foreach (Vector2 validCoord in validHeroSpawnCoords) {
				openRowCoords.Add(validCoord);
			}
			foreach (Vector2 validCoord in validHeroSpawnCoords) {
				foreach (Transform heroCoord in playField.player2.transform) {
					if (validCoord.y == heroCoord.transform.position.y) {
						//Debug.Log("SPAWNCHECK: FOUND A HERO IN A ROW, REMOVING validCoord: " + validCoord);
						openRowCoords.Remove(validCoord);
					}
				}
			}
			//Debug.Log("SPAWNCHECK: openRowCoords.length is: " + openRowCoords.ToArray().Length);
			if (openRowCoords.ToArray().Length > 0) {
				//Debug.Log("SPANWCHECK: ATTEMPTING TO RETURN OPENROWCOORDS");
				return openRowCoords[ReturnRandomValue(openRowCoords.ToArray().Length)];
			}
		}

		//If a hero hasn't already been spawned then choose a random valid coord to spawn them at
//		int validCoordsListLength = validHeroSpawnCoords.ToArray().Length;
		//Choose a random index
//		int randomIndex = Random.Range(0, validCoordsListLength);
		//Return the a random Vector2 that represents an open square in player 2's home row
		return validHeroSpawnCoords[ReturnRandomValue(validHeroSpawnCoords.ToArray().Length)];
	}

	private int ReturnRandomValue (int maxValue) {
		int randomValue = Random.Range(0, maxValue);
		return randomValue;
	}

	private bool CheckIfHomeRowIsFull () {
		int numberOfHeroesInHomerow = 0;
		foreach (Transform hero in playField.player2.transform) {
			if (hero.transform.position.x == playField.player2HomeColumn) {
				numberOfHeroesInHomerow++;
			}
		}
		if (numberOfHeroesInHomerow >= 5) {
			return true;
		} else {
			return false;
		}
	}

	public void AiStoryTakeTurn () {
		aiTurnTracker++;
		Debug.Log("CURRENTLY ACTIVE STORY: " + GlobalObject.currentlyActiveStory);
		if (GlobalObject.currentlyActiveStory == "story01") {
			if (aiTurnTracker == 1) {
				SpawnHero("knight",new Vector2(7,3));
			} else if (aiTurnTracker == 2) {
				SpawnHero("druid",new Vector2(7,3));
			} else if (aiTurnTracker == 3) {
				SpawnHero("archer",new Vector2(7,2));
			} else if (aiTurnTracker == 4) {
				SpawnHero("druid",new Vector2(7,4));
			} else if (aiTurnTracker == 5) {
				SpawnHero("archer",ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 6) {
				SpawnHero("archer",ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 7) {
				SpawnHero("archer",ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker >= 8) {
				aiTurnTracker = 0;
			}
            //Evaluate if the NPC commander's resource has reached cost
            if (GlobalObject.instance.useCommanders) {
                this.AiTryUseCommanderAbility();
            }
            //End turn call - NOTE, is this structure right? duplicate calls
            playField.EndTurn();
		} else if (GlobalObject.currentlyActiveStory == "story02") {
			if (aiTurnTracker == 1) {
				SpawnHero("archer",new Vector2(7,2));
				SpawnHero("archer",new Vector2(7,4));
			} else if (aiTurnTracker == 2) {
				SpawnHero("druid",new Vector2(7,3));
			} else if (aiTurnTracker == 3) {
				SpawnHero("archer",ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 4) {
				SpawnHero("archer",ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 5) {
				SpawnHero("druid",ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 6) {
				SpawnHero("knight",ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker >= 7) {
				aiTurnTracker = 0;
			}
            //Evaluate if the NPC commander's resource has reached cost
            if (GlobalObject.instance.useCommanders) {
                this.AiTryUseCommanderAbility();
            }
            //End turn call - NOTE, is this structure right? duplicate calls
            playField.EndTurn();
		}
	}

	private void AiStoryInitialBoardSetup () {
		Debug.LogWarning("currentlyActiveStory is:" + GlobalObject.currentlyActiveStory);
		if (GlobalObject.currentlyActiveStory == "boss01") {
            //SpawnHero("cultacolyte", new Vector2(7, 3));
        } else if (GlobalObject.currentlyActiveStory == "story02") {
			SpawnHero("cultacolyte", new Vector2(7, 3));
		}
	}

	private void SpawnHero(string hero, Vector2 coords) {
        //Set the data on the template hero card according to which hero I want to spawn
        globalObject.SetTemplateHeroCardAttributes(hero);
        //Set the 'selectedCard' static var equal to the newly defined template hero card
        Card.selectedCard = globalObject.templateHeroCard;
        //Check if the coords we are trying to spawn at are already occupied
		playField.BuildFullHeroList();
		Debug.Log("INITIAL COORDS: " + coords);
		foreach (Vector2 heroCoords in playField.fullHeroCoords) {
			Debug.Log("FOUND heroCoords and they are: " + heroCoords);
			if (heroCoords == coords) {
				coords = ReturnValidHeroSpawnCoords();
				Debug.Log("heroCoords == coords and NEW coords are: " + coords);
				break;
			}
		}
        //Spawn the hero
		playField.SpawnHeroForPlayer2(coords);
	}
}