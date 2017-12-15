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

	private int aiTurnTracker;
	private PlayField playField;
	private Deck deck;
	private Card card;

	// Use this for initialization
	void Start () {
		playField = GameObject.FindObjectOfType<PlayField>();
		deck = GameObject.FindObjectOfType<Deck>();
		card = GameObject.FindObjectOfType<Card>();
		AiStoryInitialBoardSetup ();
	}

	public void AiTakeTurn () {
		AiSelectHeroCardToPlay();
		if (Card.selectedHero && !CheckIfHomeRowIsFull()) {
			if (Card.selectedHero.GetComponent<Hero>().id == "wolf") {
				AiPlayHeroCard ();
				StartCoroutine("AiRemoveHeroCardAfterDelay");
			} else {
				AiPlayHeroCard ();
				AiRemoveHeroCard ();
			}

		} else {
			playField.EndTurn();
		}
	}

	IEnumerator AiRemoveHeroCardAfterDelay () {
		yield return new WaitForSeconds(2f);
		AiRemoveHeroCard ();
	}

	private void AiSelectHeroCardToPlay () {
		//Clear the 'validCardsToPlay' list so we can build it so fresh and so clean clean
		validCardsToPlay.Clear();

		//Add all cards that the player has enough mana to play to the 'validCardsToPlay' list
		foreach (Transform card in GameObject.Find("Player2 Hand").transform) {
			if (card.GetComponent<Card>().manaCost <= playField.player2Mana) {
				validCardsToPlay.Add(card.gameObject);
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
			Card.selectedHero = validCardsToPlay[randomIndex].GetComponent<Card>().heroPrefab;
		}
	}

	public void AiPlayHeroCard () {
		playField.SpawnHeroForPlayer2(ReturnValidHeroSpawnCoords());
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

		if (Card.selectedHero.GetComponent<Hero>().range == 1) {
			openRowCoords.Clear();
			Debug.Log("SPAWNCHECK: SPAWNING A HERO WITH RANGE == 1");
			foreach (Vector2 validCoord in validHeroSpawnCoords) {
				openRowCoords.Add(validCoord);
			}
			foreach (Vector2 validCoord in validHeroSpawnCoords) {
				foreach (Transform heroCoord in playField.player2.transform) {
					if (validCoord.y == heroCoord.transform.position.y) {
						Debug.Log("SPAWNCHECK: FOUND A HERO IN A ROW, REMOVING validCoord: " + validCoord);
						openRowCoords.Remove(validCoord);
					}
				}
			}
			Debug.Log("SPAWNCHECK: openRowCoords.length is: " + openRowCoords.ToArray().Length);
			if (openRowCoords.ToArray().Length > 0) {
				Debug.Log("SPANWCHECK: ATTEMPTING TO RETURN OPENROWCOORDS");
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

	private void AiRemoveHeroCard () {
		card.RemoveCardFromHandAndAddToDiscard();
		playField.ClearSelectedHeroAndSelectedCard ();
		AiTakeTurn();
	}

	public void AiStoryTakeTurn () {
		aiTurnTracker++;
		Debug.Log("CURRENTLY ACTIVE STORY: " + GlobalObject.currentlyActiveStory);
		if (GlobalObject.currentlyActiveStory == "story01") {
			if (aiTurnTracker == 1) {
				SpawnHero(knight,new Vector2(7,3));
			} else if (aiTurnTracker == 2) {
				SpawnHero(druid,new Vector2(7,3));
			} else if (aiTurnTracker == 3) {
				SpawnHero(archer,new Vector2(7,2));
			} else if (aiTurnTracker == 4) {
				SpawnHero(druid,new Vector2(7,4));
			} else if (aiTurnTracker == 5) {
				SpawnHero(archer,ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 6) {
				SpawnHero(archer,ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 7) {
				SpawnHero(archer,ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker >= 8) {
				aiTurnTracker = 0;
			}
			playField.EndTurn();
		} else if (GlobalObject.currentlyActiveStory == "story02") {
			if (aiTurnTracker == 1) {
				SpawnHero(archer,new Vector2(7,2));
				SpawnHero(archer,new Vector2(7,4));
			} else if (aiTurnTracker == 2) {
				SpawnHero(druid,new Vector2(7,3));
			} else if (aiTurnTracker == 3) {
				SpawnHero(archer,ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 4) {
				SpawnHero(archer,ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 5) {
				SpawnHero(druid,ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker == 6) {
				SpawnHero(knight,ReturnValidHeroSpawnCoords());
			} else if (aiTurnTracker >= 7) {
				aiTurnTracker = 0;
			}
			playField.EndTurn();
		}
	}

	private void AiStoryInitialBoardSetup () {
		Debug.Log("currentlyActiveStory is:" + GlobalObject.currentlyActiveStory);
		if (GlobalObject.currentlyActiveStory == "story01") {
			//DO NOTHING
		} else if (GlobalObject.currentlyActiveStory == "story02") {
			SpawnHero(knight, new Vector2(7,3));
		}
	}

	private void SpawnHero(GameObject hero, Vector2 coords) {
		Card.selectedHero = hero;
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
		playField.SpawnHeroForPlayer2(coords);
	}
}