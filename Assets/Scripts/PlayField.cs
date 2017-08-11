﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayField : MonoBehaviour {

	public Camera myCamera;
	//public bool placedHero = false;
	public bool player1Turn = true;
	public Text turnIndicator;
	public Text player1HealthText, player2HealthText;
	public int player1Health = 5, player2Health = 5;
	public List<Vector2> myHeroCoords;
	public List<Vector2> sortedHeroCoords;
	public List<Vector2> fullHeroCoords;
	public List<Transform> fullHeroTransformList;
	public Vector2 roundedPos;
	public GameObject player1, player2;
	public int cardPlayLimit = 2;
	public int cardsPlayed = 0;
	public bool heroAttackedATarget = false;
	public int player1HomeColumn = 1;
	public int player2HomeColumn = 7;

	private Card card;


	// Use this for initialization
	void Start () {
		player1 = new GameObject("player1");
		player2 = new GameObject("player2");
		Player1TurnStart();
		player1HealthText.text = player1Health.ToString();
		player2HealthText.text = player2Health.ToString();
		card = FindObjectOfType<Card>();
	}

	void OnMouseDown(){
		//print (Input.mousePosition);
		//print (SnapToGrid(CalculateWorldPointOfMouseClick()));
		if (cardsPlayed >= cardPlayLimit) {
			Debug.LogWarning("CANNOT PLAY ANY MORE CARDS THIS TURN");
			return;
		}
			
		//Use CalculateWorldPointOfMouseClick method to get the 'raw position' (position in pixels) of where the player clicked in the world
		Vector2 rawPos = CalculateWorldPointOfMouseClick();
		//Use SnapToGrid method to turn rawPos into rounded integer units in world space coordinates
		roundedPos = SnapToGrid(rawPos);

		//If the selected card is a spell card, cast it, ELSE treat the card as a hero card
		if (Card.selectedCard.GetComponent<Card>().type == "Spell") {
			Card.selectedCard.GetComponent<Card>().CastSpell();
			return;
		}

		//Check to make sure the player is placing the 
		if (Card.selectedCard.GetComponent<Card>().cardName == "Tower" && (roundedPos.x == player1HomeColumn || roundedPos.x == player2HomeColumn)) {
			Debug.LogWarning("THIS HERO CANNOT BE PLACED IN EITHER PLAYER'S HOME ROW");
			return;
		} else if (player1Turn && roundedPos.x != player1HomeColumn && Card.selectedCard.GetComponent<Card>().cardName != "Tower") {
			Debug.LogWarning("YOU MUST PLACE THIS HERO IN THE PLAYER 1 HOME ROW");
			return;
		} else if (!player1Turn && roundedPos.x != player2HomeColumn && Card.selectedCard.GetComponent<Card>().cardName != "Tower") {
			Debug.LogWarning("YOU MUST PLACE THIS HERO IN THE PLAYER 2 HOME ROW");
			return;
		}

		//Build full hero list then check to make sure that there aren't any other heroes already at that location on the board. If there are, return.
		BuildFullHeroList();
		foreach (Vector2 heroCoords in fullHeroCoords) {
			if (heroCoords == roundedPos) {
				Debug.Log("CAN'T PLACE HERO HERE, THERE'S ALREADY A HERO OCCUPYING THIS LOCATION");
				return;
			}
		}
			
		//Spawn the selectedHero at the appropriate location on the game grid
		GameObject x = Instantiate(Card.selectedHero, roundedPos, Quaternion.identity) as GameObject;
		if (player1Turn) {
			//Child the newly spawned hero to the appropriate player
			x.transform.SetParent(player1.transform, false);
			x.gameObject.tag = "player1";
		} else if (!player1Turn) {
			//Flip the hero so it faces to the left
			Vector3 scale = x.GetComponentInChildren<SpriteRenderer>().transform.localScale;
			scale.x = (scale.x *= -1);
			x.GetComponentInChildren<SpriteRenderer>().transform.localScale = scale;

			//Move the text so that it sits on the left side of the hero
			Vector3 newTextPosition = x.GetComponentInChildren<Text>().rectTransform.localPosition;
			newTextPosition.x *= -1;
			x.GetComponentInChildren<Text>().rectTransform.localPosition = newTextPosition;

			//Child the newly spawned hero to the appropriate player
			x.transform.SetParent(player2.transform, false);
			x.gameObject.tag = "player2";
		}

		//Put the card that was just played into the appropriate player's discard pile
		card.RemoveCardFromHandAndAddToDiscard();
	}
	
	Vector2 SnapToGrid(Vector2 rawWorldPosition){
		float newX = Mathf.RoundToInt(rawWorldPosition.x);
		float newY = Mathf.RoundToInt(rawWorldPosition.y);
		return new Vector2(newX, newY);
	}
	
	Vector2 CalculateWorldPointOfMouseClick(){
		
		//Get the pixel coordinate for mouse input x and y, and also set the distance of the game camera, which doesn't really matter for this game b/c we're...
		//... using an orthographic camera
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		float distanceFromCamera = 10f;
		
		//This is called 'weirdTriplet' b/c the distanceFromCamera 'z' coord is relative to the camera's rotation, or at least it would be in a game where...
		//... we are not using an orthographic camera.
		Vector3 weirdTriplet = new Vector3(mouseX,mouseY,distanceFromCamera);
		//Pass in the above Vector3 and return the x and y coords in world space units by using ScreenToWorldPoint. These values will update appropriately...
		//... if our distanceFromCamera value was being updated/rotated/manipulated in whatever way, however in this game it will not be changing.
		Vector2 worldPos = myCamera.ScreenToWorldPoint(weirdTriplet);
		
		return worldPos;
	} 

	public void EndTurn () {
		BuildSortedHeroList ();
		MoveHeroes ();
		ClearSelectedHeroAndSelectedCard ();
		FindObjectOfType<HandHider>().HideHand();
		cardsPlayed = 0;
	}

	void BuildSortedHeroList () {
		//Debug.LogWarning("BUILDING SORTED HERO LIST");

		//Empty the heroCoords list so we can build it again
		myHeroCoords.Clear();

		//Find each hero the player has placed and add the hero coordinates into a list
		if (player1Turn) {
				foreach (Transform child in player1.transform) {
					float x = child.transform.position.x;
					float y = child.transform.position.y;
					Vector2 coord = new Vector2(x, y);
					myHeroCoords.Add(coord);
			}
		} else if (!player1Turn) {
				foreach (Transform child in player2.transform) {
					float x = child.transform.position.x;
					float y = child.transform.position.y;
					Vector2 coord = new Vector2(x, y);
					myHeroCoords.Add(coord);
			}
		}

		//Sort all of the hero coordinates in the order we want them to be evaluated for movement/attacking
		if (player1Turn) {
			//Debug.Log("Trying to sort player 1's heroes");
			sortedHeroCoords = myHeroCoords.OrderByDescending(value => value.y).ThenByDescending(value => value.x).ToList();
		} else if (!player1Turn) {
			//Debug.Log("Trying to sort player 2's heroes");
			sortedHeroCoords = myHeroCoords.OrderByDescending(value => value.y).ThenBy(value => value.x).ToList();
		}

	}

	public void MoveHeroes ()
	{
//		Debug.LogWarning("sortedHeroCoords has " + sortedHeroCoords.ToArray().Length + " entries");

		// If there are no more heroes left to move then end my turn
		if (sortedHeroCoords.ToArray().Length <= 0) {
			Debug.Log("CHANGING PLAYER TURNS");
			EndOfTurnEffects ();
			player1Turn = !player1Turn;
			if (player1Turn) {
				Player1TurnStart ();
			} else if (!player1Turn) {
				Player2TurnStart ();
			}
			return;
		}
		//Debug.LogWarning("RUNNING MOVEHEROES");

		//Search each hero to see if their coords match the first set of coords in the 'sortedCoords' list. If they do, move that hero, then remove that hero's
		//coords from the sortedHeroCoords list and wait for this "MoveHeroes" method to be called again.
		if (player1Turn) {
			for (int i = 0; i < 1; i++) {
				foreach (Transform child in player1.transform) {
					if (child.transform.position.x == sortedHeroCoords [i].x && child.transform.position.y == sortedHeroCoords [i].y) {
						Player1MoveCheck (child);
						sortedHeroCoords.RemoveAt(i);
						break;
					}
				}
			}
		} else if (!player1Turn) {
			for (int i = 0; i < 1; i++) {
				foreach (Transform child in player2.transform) {
					if (child.transform.position.x == sortedHeroCoords [i].x && child.transform.position.y == sortedHeroCoords [i].y) {
						Player2MoveCheck (child);
						sortedHeroCoords.RemoveAt(i);
						break;
					}
				}
			}
		}
	}

	public void ClearSelectedHeroAndSelectedCard ()
	{
		//Reset the 'selectedHero' variable so players can't place another hero before selecting another card
		Card.selectedHero = default(GameObject);
		//Reset the 'selectedCard' variable so players can't add multiple of them to their discard pile
		Card.selectedCard = default(GameObject);
	}

	void Player1MoveCheck (Transform currentHero) {
		BuildFullHeroList ();
		float closestHero = 999f;
		foreach (Vector2 hero in fullHeroCoords) {
			//Check for the hero that is closest to me on the x-axis in the direction that I'll be heading
			if (hero.y == currentHero.transform.position.y && ((hero.x - currentHero.transform.position.x) < closestHero) && ((hero.x - currentHero.transform.position.x) > 0)) {
				closestHero = hero.x - currentHero.transform.position.x;
			}
		}

		//Check to see if I have any enemies in range. If I do, run MoveSingleHeroRightAndAttack but don't actually move the hero
		foreach (Transform enemy in player2.transform) {
			if ((Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) <= currentHero.GetComponent<Hero>().range)
				&& (Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) > 0)
				&& enemy.transform.position.y == currentHero.transform.position.y) {
					currentHero.GetComponent<Hero>().MoveSingleHeroRightAndAttack(0);
					return;
			}
		}

		//If there's nothing in my way move me forward by my full 'speed' stat, else move me forward as far as I am able to
		 if (currentHero.GetComponent<Hero>().speed < Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroRightAndAttack(currentHero.GetComponent<Hero>().speed);
		} else if (currentHero.GetComponent<Hero>().speed >= Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroRightAndAttack(Mathf.RoundToInt(closestHero)-1);

		}
	}

	void Player2MoveCheck (Transform currentHero) {
		BuildFullHeroList ();
		float closestHero = 999f;
		foreach (Vector2 hero in fullHeroCoords) {
			//Check for the hero that is closest to me on the x-axis in the direction that I'll be heading
			if (hero.y == currentHero.transform.position.y && ((currentHero.transform.position.x - hero.x) < closestHero) && ((currentHero.transform.position.x - hero.x) > 0)) {
				closestHero = currentHero.transform.position.x - hero.x;
			}
		}

		//Check to see if I have any enemies in range. If I do, run MoveSingleHeroRightAndAttack but don't actually move the hero
		foreach (Transform enemy in player1.transform) {
			if ((Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) <= currentHero.GetComponent<Hero>().range)
			&& (Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) > 0)
			&& enemy.transform.position.y == currentHero.transform.position.y) {
				currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(0);
				return;
			}
		}

		//If there's nothing in my way move me forward by my full 'speed' stat, else move me forward as far as I am able to
		if (currentHero.GetComponent<Hero>().speed < Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(currentHero.GetComponent<Hero>().speed);
		} else if (currentHero.GetComponent<Hero>().speed >= Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(Mathf.RoundToInt(closestHero)-1);
		}
	}

	void BuildFullHeroList () {
		fullHeroCoords.Clear();
		foreach (Transform hero in player1.transform) {
			float x = hero.transform.position.x;
			float y = hero.transform.position.y;
			Vector2 coord = new Vector2(x, y);
			fullHeroCoords.Add(coord);
		}
		foreach (Transform hero in player2.transform) {
			float x = hero.transform.position.x;
			float y = hero.transform.position.y;
			Vector2 coord = new Vector2(x, y);
			fullHeroCoords.Add(coord);
		}
	}

	void BuildFullHeroTransformList () {
		fullHeroTransformList.Clear();
		foreach (Transform hero in player1.transform) {
			fullHeroTransformList.Add(hero);
		}
		foreach (Transform hero in player2.transform) {
			fullHeroTransformList.Add(hero);
		}
	}

	//Gets called from hero.cs after the hero has finished moving
	public void HeroTargetCheck (Transform currentHero) {
		if (currentHero.GetComponent<Hero>().id == "rogue") {
			AttackEnemiesInList(currentHero, TargetCheckCardinalDirections(currentHero, "enemy"));
		} else if (currentHero.GetComponent<Hero>().id == "tower") {
			AttackEnemiesInList(currentHero, TargetCheckAllDirections(currentHero, "enemy"));
		} else if (currentHero.GetComponent<Hero>().id == "archer") {
			AttackEnemiesInList(currentHero, TargetCheckAllHeroesInRange(currentHero, "enemy"));
		} else {
			AttackEnemiesInList(currentHero, TargetCheckClosestHeroInRange(currentHero, "enemy"));
		}
	}

	//Takes a 'currentHero' and a list of enemy heroes, then the currentHero attacks all of the enemy heroes in the list using the 'TakeDamage()' method
	private void AttackEnemiesInList (Transform currentHero, List<Transform> enemies) {
		foreach (Transform enemy in enemies) {
			enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().power);
		}
	}

	//Takes a 'currentHero' and a list of other heroes, then the currentHero heals all of the heroes in the list using the 'HealPartial()' method
	private void HealHeroesInList (Transform currentHero, List<Transform> heroes) {
		foreach (Transform hero in heroes) {
			hero.GetComponent<Hero>().HealPartial(currentHero.GetComponent<Hero>().healValue);
		}
	}

	//Takes a 'currentHero' and a 'herotype' to search for (valid types are "enemy" and "ally"). It then returns a list of the given herotypes that are currently located in ANY direction around the currenthero, including diagonals
	//IMPORTANT: This target check currently only supports checking all directions at a range of 1. That is to say that a hero's range WILL NOT AFFECT HOW FAR THIS METHOD CHECKS TO RETURN TARGETS
	private List<Transform> TargetCheckAllDirections (Transform currentHero, string heroTypeToSearchFor) {
		List<Transform> validHeroes = new List<Transform>();
		BuildFullHeroTransformList();
		float currentHeroX = currentHero.transform.position.x;
		float currentHeroY = currentHero.transform.position.y;
		//IMPORTANT: This is where I manually set this method to ONLY CHECK ALL DIRECTIONS AT A RANGE OF 1. I.E. A HERO'S RANGE WILL NOT AFFECT HOW FAR THIS METHOD CHECKS TO RETURN TARGETS
		int currentHeroRange = 1;

		foreach (Transform otherHero in fullHeroTransformList) {
			if ( //Check all of the squares around "currentHero" (including diagonal squares) to see if "otherHero" is in range... then based on which type I'm checking for ("enemy" or "ally") add them to the list if appropriate
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) - currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) - currentHeroRange) || 
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) - currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY)) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) - currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) + currentHeroRange) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) - currentHeroRange) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) + currentHeroRange) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) + currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) - currentHeroRange) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) + currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY)) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) + currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) + currentHeroRange)
				) {
					if (heroTypeToSearchFor == "enemy") {
						if (currentHero.tag != otherHero.tag) {
							validHeroes.Add(otherHero);
							heroAttackedATarget = true;
						}
					} else if (heroTypeToSearchFor == "ally") {
						if (currentHero.tag == otherHero.tag) {
							validHeroes.Add(otherHero);
						}
					}	
			}
		}
		return validHeroes;
	}

	//Takes a 'currentHero' and a 'herotype' to search for (valid types are "enemy" and "ally"). It then returns a list of the given herotypes that are currently located in any CARDINAL direction around the currenthero, NOT including diagonals
	private List<Transform> TargetCheckCardinalDirections (Transform currentHero, string heroTypeToSearchFor) {
		List<Transform> validHeroes = new List<Transform>();
		BuildFullHeroTransformList();
		float currentHeroX = currentHero.transform.position.x;
		float currentHeroY = currentHero.transform.position.y;
		int currentHeroRange = currentHero.GetComponent<Hero>().range;

		foreach (Transform otherHero in fullHeroTransformList) {
			if ( //Check all of the squares around my hero (cardinal directions only) to see if "otherHero" is in range... then based on which type I'm checking for ("enemy" or "ally") add them to the list if appropriate
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) + currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY)) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) - currentHeroRange && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY)) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) + currentHeroRange) ||
				(Mathf.RoundToInt(otherHero.transform.position.x) == Mathf.RoundToInt(currentHeroX) && Mathf.RoundToInt(otherHero.transform.position.y) == Mathf.RoundToInt(currentHeroY) - currentHeroRange)
				) {
					if (heroTypeToSearchFor == "enemy") {
						if (currentHero.tag != otherHero.tag) {
							validHeroes.Add(otherHero);
							heroAttackedATarget = true;
						}
					} else if (heroTypeToSearchFor == "ally") {
						if (currentHero.tag == otherHero.tag) {
							validHeroes.Add(otherHero);
						}
					}
			}
		}
		return validHeroes;
	}

	private List<Transform> TargetCheckAllHeroesInRange (Transform currentHero, string heroTypeToSearchFor) {
		List<Transform> validHeroes = new List<Transform>();
		BuildFullHeroTransformList();
		float currentHeroX = currentHero.transform.position.x;
		float currentHeroY = currentHero.transform.position.y;
		int currentHeroRange = currentHero.GetComponent<Hero>().range;

		if (player1Turn) {
			foreach (Transform otherHero in fullHeroTransformList) {
				//Check if the otherHero is in range and NOT behind me
				if ((Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX) <= currentHeroRange)
					&& (Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX) > 0)
					&& otherHero.transform.position.y == currentHeroY) {
					//Of the otherHeroes that are in range and NOT behind me, add the appropriate ones to my target list based on which hero type I'm looking for ('enemy' or 'ally')
					if (heroTypeToSearchFor == "enemy") {
						if (currentHero.tag != otherHero.tag) {
							validHeroes.Add(otherHero);
							heroAttackedATarget = true;
						}
					} else if (heroTypeToSearchFor == "ally") {
						if (currentHero.tag == otherHero.tag) {
							validHeroes.Add(otherHero);
						}
					}
				}
			}
		} else if (!player1Turn) {
			foreach (Transform otherHero in player1.transform) {
				//Check if the otherHero is in range and NOT behind me
				if ((Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x) <= currentHeroRange)
					&& (Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x) > 0)
					&& otherHero.transform.position.y == currentHeroY) {
					//Of the otherHeroes that are in range and NOT behind me, add the appropriate ones to my target list based on which hero type I'm looking for ('enemy' or 'ally')
					if (heroTypeToSearchFor == "enemy") {
						if (currentHero.tag != otherHero.tag) {
							validHeroes.Add(otherHero);
							heroAttackedATarget = true;
						}
					} else if (heroTypeToSearchFor == "ally") {
						if (currentHero.tag == otherHero.tag) {
							validHeroes.Add(otherHero);
						}
					}
				}
			}
		}
		return validHeroes;
	}

	private List<Transform> TargetCheckClosestHeroInRange (Transform currentHero, string heroTypeToSearchFor) {
		List<Transform> validHeroes = new List<Transform>();
		float closestHeroX = 999f;
		BuildFullHeroTransformList();
		float currentHeroX = currentHero.transform.position.x;
		float currentHeroY = currentHero.transform.position.y;
		int currentHeroRange = currentHero.GetComponent<Hero>().range;

		if (player1Turn) {
			foreach (Transform otherHero in fullHeroTransformList) {
				//Check if the otherHero is in range and NOT behind me
				if ((Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX) <= currentHeroRange)
					&& (Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX) > 0)
					&& otherHero.transform.position.y == currentHeroY) {
					//Of the otherHeroes that are in range and NOT behind me, add the appropriate one to my target list based on which hero type I'm looking for ('enemy' or 'ally') AND if that hero is the closest hero to me
					if (heroTypeToSearchFor == "enemy") {
						if (currentHero.tag != otherHero.tag && Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX) < closestHeroX) {
							validHeroes.Clear();
							validHeroes.Add(otherHero);
							closestHeroX = Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX);
							heroAttackedATarget = true;
						}
					} else if (heroTypeToSearchFor == "ally") {
						if (currentHero.tag == otherHero.tag && Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX) < closestHeroX) {
							validHeroes.Clear();
							validHeroes.Add(otherHero);
							closestHeroX = Mathf.RoundToInt(otherHero.transform.position.x) - Mathf.RoundToInt(currentHeroX);
						}
					}
				}
			}
		} else if (!player1Turn) {
			foreach (Transform otherHero in player1.transform) {
				//Check if the otherHero is in range and NOT behind me
				if ((Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x) <= currentHeroRange)
					&& (Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x) > 0)
					&& otherHero.transform.position.y == currentHeroY) {
					//Of the otherHeroes that are in range and NOT behind me, add the appropriate one to my target list based on which hero type I'm looking for ('enemy' or 'ally') AND if that hero is the closest hero to me
					if (heroTypeToSearchFor == "enemy") {
						if (currentHero.tag != otherHero.tag && Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x) < closestHeroX) {
							validHeroes.Clear();
							validHeroes.Add(otherHero);
							closestHeroX = Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x);
							heroAttackedATarget = true;
						}
					} else if (heroTypeToSearchFor == "ally") {
						if (currentHero.tag == otherHero.tag && Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x) < closestHeroX) {
							validHeroes.Clear();
							validHeroes.Add(otherHero);
							closestHeroX = Mathf.RoundToInt(currentHeroX) - Mathf.RoundToInt(otherHero.transform.position.x);
						}
					}
				}
			}
		}
		return validHeroes;
	}

	public void Player1MoveHasteCheck (Transform currentHero) {
		BuildFullHeroList ();
		float closestHero = 999f;
		foreach (Vector2 hero in fullHeroCoords) {
			//Check for the hero that is closest to me on the x-axis in the direction that I'll be heading
			if (hero.y == currentHero.transform.position.y && ((hero.x - currentHero.transform.position.x) < closestHero) && ((hero.x - currentHero.transform.position.x) > 0)) {
				closestHero = hero.x - currentHero.transform.position.x;
			}
		}

		//Check to see if I have any enemies in range. If I do, run MoveSingleHeroRightAndAttack but don't actually move the hero
		foreach (Transform enemy in player2.transform) {
			if ((Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) <= currentHero.GetComponent<Hero>().range)
				&& (Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) > 0)
				&& enemy.transform.position.y == currentHero.transform.position.y) {
					currentHero.GetComponent<Hero>().MoveSingleHeroRightAndAttack(0);
					return;
			}
		}

		//If there's nothing in my way move me forward by my full 'speed' stat, else move me forward as far as I am able to
		 if (currentHero.GetComponent<Hero>().speed < Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroRightAndAttack(currentHero.GetComponent<Hero>().speed);
		} else if (currentHero.GetComponent<Hero>().speed >= Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroRightAndAttack(Mathf.RoundToInt(closestHero)-1);

		}
	}

	public void Player2MoveHasteCheck (Transform currentHero) {
		BuildFullHeroList ();
		float closestHero = 999f;
		foreach (Vector2 hero in fullHeroCoords) {
			//Check for the hero that is closest to me on the x-axis in the direction that I'll be heading
			if (hero.y == currentHero.transform.position.y && ((currentHero.transform.position.x - hero.x) < closestHero) && ((currentHero.transform.position.x - hero.x) > 0)) {
				closestHero = currentHero.transform.position.x - hero.x;
			}
		}

		foreach (Transform enemy in player1.transform) {
			if ((Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) <= currentHero.GetComponent<Hero>().range)
			&& (Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) > 0)
			&& enemy.transform.position.y == currentHero.transform.position.y) {
				currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(0);
				return;
			}
		}

		//If there's nothing in my way move me forward by my full 'speed' stat, else move me forward as far as I am able to
		if (currentHero.GetComponent<Hero>().speed < Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(currentHero.GetComponent<Hero>().speed);
		} else if (currentHero.GetComponent<Hero>().speed >= Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(Mathf.RoundToInt(closestHero)-1);
		}
	}

	void Player1TurnStart () {
		turnIndicator.text = "<color=blue>Player 1's Turn</color>";
		//Enable collision on player 1 cards
		BoxCollider2D[] player1Cards = GameObject.Find("Player1 Hand").GetComponentsInChildren<BoxCollider2D>();
		foreach (BoxCollider2D col in player1Cards) {
			col.enabled = true;
		}
		//Disable collision on player 2 cards
		BoxCollider2D[] player2Cards = GameObject.Find("Player2 Hand").GetComponentsInChildren<BoxCollider2D>();
		foreach (BoxCollider2D col in player2Cards) {
			col.enabled = false;
		}
		//Visually show player 1 cards and hide player 2 cards
		GameObject.Find("Player1 Hand").GetComponentInChildren<CanvasGroup>().alpha = 1;
		GameObject.Find("Player1 Hand").GetComponentInChildren<CanvasGroup>().blocksRaycasts = true;
		GameObject.Find("Player2 Hand").GetComponentInChildren<CanvasGroup>().alpha = 0;
		GameObject.Find("Player2 Hand").GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;

		//Checks your current hand size and deals you back up to max
		FindObjectOfType<Deck>().Player1DealCards();
	}

	void Player2TurnStart () {
		turnIndicator.text = "<color=red>Player 2's Turn</color>";
		//Disable collision on player 1 cards
		BoxCollider2D[] player1Cards = GameObject.Find("Player1 Hand").GetComponentsInChildren<BoxCollider2D>();
		foreach (BoxCollider2D col in player1Cards) {
			col.enabled = false;
		}
		//Enable collision on player 2 cards
		BoxCollider2D[] player2Cards = GameObject.Find("Player2 Hand").GetComponentsInChildren<BoxCollider2D>();
		foreach (BoxCollider2D col in player2Cards) {
			col.enabled = true;
		}
		//Visually show player 2 cards and hide player 1 cards
		GameObject.Find("Player1 Hand").GetComponentInChildren<CanvasGroup>().alpha = 0;
		GameObject.Find("Player1 Hand").GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;
		GameObject.Find("Player2 Hand").GetComponentInChildren<CanvasGroup>().alpha = 1;
		GameObject.Find("Player2 Hand").GetComponentInChildren<CanvasGroup>().blocksRaycasts = true;

		//Checks your current hand size and deals you back up to max
		FindObjectOfType<Deck>().Player2DealCards();
	}

	public void LosePlayerHealth (int dmg) {
		if (player1Turn) {
			player2Health -= dmg;
			player2HealthText.text = player2Health.ToString();
		} else if (!player1Turn) {
			player1Health -= dmg;
			player1HealthText.text = player1Health.ToString();
		}
	}

	void EndOfTurnEffects () {
		BuildFullHeroTransformList ();

		List<Transform> druidList = new List<Transform>();
		foreach (Transform hero in fullHeroTransformList) {
			if (hero.GetComponent<Hero>().id == "druid") {
				druidList.Add(hero);
			}
		}
		foreach (Transform druid in druidList) {
			if (player1Turn && druid.tag == "player1") {
				HealHeroesInList(druid, TargetCheckAllDirections(druid, "ally"));
			} else if (!player1Turn && druid.tag == "player2") {
				HealHeroesInList(druid, TargetCheckAllDirections(druid, "ally"));
			}
		}
	}

	public void IncrementCardsPlayedCounter () {
		cardsPlayed += 1;
	}

	public void TestTest () {
		Debug.LogWarning("TEST TEST");
	}
}