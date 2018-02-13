using UnityEngine;
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
	public Text player1ManaText, player2ManaText;
	public Text turnsPlayedText;
	public int player1Health = 3, player2Health = 3;
	public List<Vector2> myHeroCoords;
	public List<Vector2> sortedHeroCoords;
	public List<Vector2> fullHeroCoords;
	public List<Transform> fullHeroTransformList;
	public List<Transform> tempTransformList;
	public Vector2 roundedPos;
	public GameObject player1, player2;
	public bool heroAttackedATarget = false;
	public int player1HomeColumn = 1;
	public int player2HomeColumn = 7;
	public ParticleSystem manaParticle;
	public BuffManager buffManager;
	public int player1Mana = 0, player2Mana = 0;

	private Card card;
	private int player1ManaMax = 10, player2ManaMax = 10;
	private int manaPerTurn = 4;
	private int turnsPlayed = 0;

	// Use this for initialization
	void Start () {
		player1 = new GameObject("player1");
		player2 = new GameObject("player2");
		Player1TurnStart();
		player1HealthText.text = player1Health.ToString();
		player2HealthText.text = player2Health.ToString();
		card = FindObjectOfType<Card>();
		player2ManaText.text = player2Mana.ToString();
		buffManager = FindObjectOfType<BuffManager>();
	}

	void OnMouseDown(){
		//print (Input.mousePosition);
		//print (SnapToGrid(CalculateWorldPointOfMouseClick()));

		if (!Card.selectedCard) {
			Debug.LogWarning("NO CARD SELECTED");
			return;
		}

		//Check that the player has enough mana to play the selected card. If they do not, return.
		if (player1Turn) {
			if (Card.selectedCard.GetComponent<Card>().manaCost > player1Mana) {
				Debug.LogWarning("NOT ENOUGH MANA TO PLAY THIS CARD");
				return;
			}
		} else if (!player1Turn) {
			if (Card.selectedCard.GetComponent<Card>().manaCost > player2Mana) {
				Debug.LogWarning("NOT ENOUGH MANA TO PLAY THIS CARD");
				return;
			}
		}
			
		//Use CalculateWorldPointOfMouseClick method to get the 'raw position' (position in pixels) of where the player clicked in the world
		Vector2 rawPos = CalculateWorldPointOfMouseClick();
		//Use SnapToGrid method to turn rawPos into rounded integer units in world space coordinates
		roundedPos = SnapToGrid(rawPos);

		//If the selected card is a 'Class Spell', cast it
		if (Card.selectedCard.GetComponent<Card>().type == "Spell" && Card.selectedCard.GetComponent<Card>().cardName != "Tower" && Card.selectedCard.GetComponent<Card>().cardName != "Wall") {
			Card.selectedCard.GetComponent<Card>().CastSpell();
			return;
		}

		//If the selected card is a 'Spell Card', cast it
		if (Card.selectedCard.GetComponent<Card>().type == "SpellCard") {
			Card.selectedCard.GetComponent<Card>().CastSpell();
			return;
		}

		//Check to make sure the player is placing their hero in an appropriate location (e.g. Towers/Walls cannot be placed in home rows, all other heroes MUST be placed in home rows, etc...)
		if ((Card.selectedCard.GetComponent<Card>().cardName == "Tower" || Card.selectedCard.GetComponent<Card>().cardName == "Wall") && (roundedPos.x == player1HomeColumn || roundedPos.x == player2HomeColumn)) {
			Debug.LogWarning("THIS HERO CANNOT BE PLACED IN EITHER PLAYER'S HOME ROW");
			return;
		} else if (player1Turn && roundedPos.x != player1HomeColumn && Card.selectedCard.GetComponent<Card>().cardName != "Tower" && Card.selectedCard.GetComponent<Card>().cardName != "Wall") {
			Debug.LogWarning("YOU MUST PLACE THIS HERO IN THE PLAYER 1 HOME ROW");
			return;
		} else if (!player1Turn && roundedPos.x != player2HomeColumn && Card.selectedCard.GetComponent<Card>().cardName != "Tower" && Card.selectedCard.GetComponent<Card>().cardName != "Wall") {
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
		if (Card.selectedCard.GetComponent<Card>().type != "SpellCard") {
				if (player1Turn) {
				SpawnHeroForPlayer1 (roundedPos);
			} else if (!player1Turn) {
				SpawnHeroForPlayer2 (roundedPos);
			}
		}

		//This is here to catch any special-cased 'Class Spell' cards that did not get cast above (e.g. the 'Wall' and 'Tower' cards)
		if (Card.selectedCard.GetComponent<Card>().type == "Spell") {
			Card.selectedCard.GetComponent<Card>().SetSpellCooldown();
		}

		//Put the card that was just played into the appropriate player's discard pile.
		card.RemoveCardFromHandAndAddToDiscard ();

		//Set the 'selected hero' and 'selected card' variables to default so the game doesn't think I still have anything selected
		ClearSelectedHeroAndSelectedCard ();
	}

	public void SpawnHeroForPlayer1 (Vector2 roundedPos) {
		GameObject x = Instantiate (Card.selectedHero, roundedPos, Quaternion.identity) as GameObject;
		//Child the newly spawned hero to the appropriate player
		x.transform.SetParent (player1.transform, false);
		x.gameObject.tag = "player1";
		Color xAlpha = x.transform.FindChild ("Player1Indicator").GetComponent<SpriteRenderer> ().color;
		xAlpha.a = 0.5f;
		x.transform.FindChild ("Player1Indicator").GetComponent<SpriteRenderer> ().color = xAlpha;
		SubtractMana ();
	}

	public void SpawnHeroForPlayer2 (Vector2 roundedPos) {
		GameObject x = Instantiate (Card.selectedHero, roundedPos, Quaternion.identity) as GameObject;
		//Flip the hero so it faces to the left
		Vector3 scale = x.transform.FindChild ("Image").GetComponent<SpriteRenderer> ().transform.localScale;
		scale.x = (scale.x *= -1);
		x.transform.FindChild ("Image").GetComponent<SpriteRenderer> ().transform.localScale = scale;
		//Move the Armor and Health text so that it sits on the left side of the hero
		foreach (Transform text in x.transform) {
			Vector3 newTextPosition = text.transform.localPosition;
			newTextPosition.x *= -1;
			text.transform.localPosition = newTextPosition;
		}
		//Child the newly spawned hero to the appropriate player
		x.transform.SetParent (player2.transform, false);
		x.gameObject.tag = "player2";
		Color xAlpha = x.transform.FindChild ("Player2Indicator").GetComponent<SpriteRenderer> ().color;
		xAlpha.a = 0.5f;
		x.transform.FindChild ("Player2Indicator").GetComponent<SpriteRenderer> ().color = xAlpha;
		if (!GlobalObject.storyEnabled) {
			SubtractMana ();
		}
	}
	
	public Vector2 SnapToGrid(Vector2 rawWorldPosition){
		float newX = Mathf.RoundToInt(rawWorldPosition.x);
		float newY = Mathf.RoundToInt(rawWorldPosition.y);
		return new Vector2(newX, newY);
	}
	
	public Vector2 CalculateWorldPointOfMouseClick(){
		
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

	IEnumerator SwitchPlayerTurns () {
		yield return new WaitForSeconds(0.5f);
		player1Turn = !player1Turn;
		if (player1Turn) {
			Player1TurnStart ();
		} else if (!player1Turn) {
			Player2TurnStart ();
		}
	}

	public void MoveHeroes ()
	{
		//Debug.LogWarning("sortedHeroCoords has " + sortedHeroCoords.ToArray().Length + " entries");

		// If there are no more heroes left to move then end my turn
		if (sortedHeroCoords.ToArray().Length <= 0) {
			Debug.Log("CHANGING PLAYER TURNS");
			EndOfTurnEffects ();
			StartCoroutine("SwitchPlayerTurns");
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
						//Debug.Log("sortedHeroCoords has this many heroes: " + sortedHeroCoords.ToArray().Length);
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
		//Debug.Log("RUNNING playField.ClearSelectedHeroAndSelectedCard()");
		//Reset the 'selectedHero' variable so players can't place another hero before selecting another card
		Card.selectedHero = default(GameObject);
		//Debug.Log("selectedHero is " + Card.selectedHero);
		//Reset the 'selectedCard' variable so players can't add multiple of them to their discard pile
		Card.selectedCard = default(GameObject);
		//Debug.Log("selectedCard is " + Card.selectedCard);
	}

	void Player1MoveCheck (Transform currentHero) {
		BuildFullHeroList ();
		float closestHero = 999f;
		foreach (Vector2 hero in fullHeroCoords) {
			//Check for the hero that is closest to me on the x-axis in the direction that I'll be heading
			if (hero.y == currentHero.transform.position.y && ((hero.x - currentHero.transform.position.x) < closestHero) && ((hero.x - currentHero.transform.position.x) > 0)) {
				closestHero = hero.x - currentHero.transform.position.x;
				Debug.Log("FOUND A HERO IN MY WAY. I AM: " + currentHero.GetComponent<Hero>().name + " and there is another hero at: " + hero.x + "," + hero.y);
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

		//Special case for the Cavalry unit, which will move all the way to the next enemy in his row. If there is no enemy then he will move by his full move speed.
		if (currentHero.GetComponent<Hero>().id == "cavalry") {
			if (closestHero < 999f) {
				currentHero.GetComponent<Hero>().MoveSingleHeroRightAndAttack(Mathf.RoundToInt(closestHero)-1);
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

		//Special case for the Cavalry unit, which will move all the way to the next enemy in his row. If there is no enemy then he will move by his full move speed.
		if (currentHero.GetComponent<Hero>().id == "cavalry") {
			if (closestHero < 999f) {
				currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(Mathf.RoundToInt(closestHero)-1);
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

	public float FindClosestHeroToTheRight (Transform currentHero) {
		BuildFullHeroList ();
		float closestHero = 999f;
		foreach (Vector2 hero in fullHeroCoords) {
			//Check for the hero that is closest to me on the x-axis in the direction that I'll be heading
			if (hero.y == currentHero.transform.position.y && ((hero.x - currentHero.transform.position.x) < closestHero) && ((hero.x - currentHero.transform.position.x) > 0)) {
				closestHero = hero.x - currentHero.transform.position.x;
			}
		}

		return closestHero;
	}

	public float FindClosestHeroToTheLeft (Transform currentHero) {
		BuildFullHeroList ();
		float closestHero = 999f;
		foreach (Vector2 hero in fullHeroCoords) {
			//Check for the hero that is closest to me on the x-axis in the direction that I'll be heading
			if (hero.y == currentHero.transform.position.y && ((currentHero.transform.position.x - hero.x) < closestHero) && ((currentHero.transform.position.x - hero.x) > 0)) {
				closestHero = currentHero.transform.position.x - hero.x;
			}
		}

		return closestHero;
	}

	public void BuildFullHeroList () {
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

	//Gets called from AnimationEvents.cs via an animation event on the hero's attack animation
	public void HeroTargetCheck (Transform currentHero) {
		if (currentHero.GetComponent<Hero>().id == "rogue") {
			AttackEnemiesInList(currentHero, TargetCheckCardinalDirections(currentHero, "enemy"));
		} else if (currentHero.GetComponent<Hero>().id == "tower") {
			AttackEnemiesInList(currentHero, TargetCheckAllDirections(currentHero, "enemy", null));
		} else if (currentHero.GetComponent<Hero>().id == "archer" || currentHero.GetComponent<Hero>().id == "slinger") {
			AttackEnemiesInList(currentHero, TargetCheckAllHeroesInRange(currentHero, "enemy"));
		} else if (currentHero.GetComponent<Hero>().id == "sapper") {
			AttackEnemiesInList(currentHero, TargetCheckAllDirections(currentHero, "enemy", null));
		} else if (currentHero.GetComponent<Hero>().id == "chaosmage") {
			AttackEnemiesInList(currentHero, tempTransformList);
		} else {
			AttackEnemiesInList(currentHero, TargetCheckClosestHeroInRange(currentHero, "enemy"));
		}
	}

	//Takes a 'currentHero' and a list of enemy heroes, then the currentHero attacks all of the enemy heroes in the list using the 'TakeDamage()' method
	public void AttackEnemiesInList (Transform currentHero, List<Transform> enemies) {
		foreach (Transform enemy in enemies) {

			//SPECIAL CASES for modifying damage
			//If my dodge chance succeeds, dodge the incoming attack
			if (enemy.GetComponent<Hero>().dodgeChance > Random.Range(0.0f, 0.99f)) {
				enemy.GetComponent<Hero>().DodgeDamage();
				Debug.Log("DODGED AN ATTACK B/C OF MY DODGE CHANCE");
				return;
			}

			//If the attacker is a Dwarf check if the enemy has any armor. If they do, destroy all of that armor and then do damage to the enemy. Otherwise, just do damage to the enemy.
			if (currentHero.GetComponent<Hero>().id == "dwarf") {
				if (enemy.GetComponent<Hero>().currentArmor > 0) {
					enemy.GetComponent<Hero>().currentArmor = 0;
				}

				enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().power);
				return;
			}

			//If the attacker is an assassin and the target is below a specific health threshold, kill the target
			if (currentHero.GetComponent<Hero>().id == "assassin") {
				if (enemy.GetComponent<Hero>().currentHealth < 5) {
					enemy.GetComponent<Hero>().TakeDamage(enemy.GetComponent<Hero>().currentHealth + enemy.GetComponent<Hero>().currentArmor);
					return;
				}
			}

			//If the defender is a monk and the attacker is further away than melee range, the monk will dodge the incoming attack
			if (enemy.GetComponent<Hero>().id == "monk") {
				if (Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) != 1 && 
				Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) != -1) {
					enemy.GetComponent<Hero>().DodgeDamage();
					Debug.Log("DODGED AN ATTACK B/C I'M A MONK");
					return;
				}
			}

			//If the attacker is a sorcerer add extra damage for each enemy in the sorcerer's row
			if (currentHero.GetComponent<Hero>().id == "sorcerer") {
				enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().power + (1 * CountEnemiesInMyRow(currentHero)));
				return;
			}

//			if (currentHero.GetComponent<Hero>().id == "sapper") {
//				enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().power);
////				currentHero.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().currentHealth + currentHero.GetComponent<Hero>().currentArmor);
////				MoveHeroes();
////				return;
//			}

			//Do your damage vs. the current target
			enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().power);
			//Do any special attack effects associated with the currentHero that is attacking
			currentHero.GetComponent<Hero>().HeroAttackEffects();
		}

		if (currentHero.GetComponent<Hero>().id == "sapper") {
			currentHero.GetComponent<Hero>().AfterAttackOperations();
			currentHero.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().currentHealth + currentHero.GetComponent<Hero>().currentArmor);
		}
	}

	//Takes a 'currentHero' and a list of other heroes, then the currentHero heals all of the heroes in the list using the 'HealPartial()' method
	private void HealHeroesInList (Transform currentHero, List<Transform> heroes) {
		foreach (Transform hero in heroes) {
			hero.GetComponent<Hero>().HealPartial(currentHero.GetComponent<Hero>().healValue);
		}
	}

	//Takes a 'currentHero' and a list of other heroes, then the currentHero adds armor all of the heroes in the list using the 'AddArmor()' method
	private void ArmorHeroesInList (Transform currentHero, List<Transform> heroes) {
		foreach (Transform hero in heroes) {
			hero.GetComponent<Hero>().AddArmor(1);
		}
	}


	//Takes a 'currentHero' and a 'herotype' to search for (valid types are "enemy" and "ally"). It then returns a list of the given herotypes that are currently located in any CARDINAL direction around the currenthero, NOT including diagonals
	public List<Transform> TargetCheckEntireBoardTwoRandomHeroes (Transform currentHero, string heroTypeToSearchFor) {
		List<Transform> validHeroes = new List<Transform>();
		BuildFullHeroTransformList();
		float currentHeroX = currentHero.transform.position.x;
		float currentHeroY = currentHero.transform.position.y;
		int currentHeroRange = currentHero.GetComponent<Hero>().range;

		//Check ALL of the heroes on the game board, then based on which type I'm checking for ("enemy" or "ally") add them to the appropriate list
		foreach (Transform otherHero in fullHeroTransformList) {
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

		int tempListCount = validHeroes.Count;
		for (int i = 0; i < tempListCount; i++) {
			if (validHeroes.Count > 2) {
				int temp = Random.Range(0, validHeroes.Count - 1);
				validHeroes.RemoveAt(temp);
			}
		}

		//We store the enemies that we'll be attacking in a tempTransformList so that we can spawn the red 'attack tell boxes' beneath them, which is called from hero.cs. We have to do this b/c we are picking heroes at random, so running 
		//the check again in a different part of the code would return possibly two different enemies than the ones that we are actually attacking.
		tempTransformList.Clear();
		tempTransformList = validHeroes;

		return validHeroes;
	}

	//Takes a 'currentHero' and a 'herotype' to search for (valid types are "enemy" and "ally"). It then returns a list of the given herotypes that are currently located in ANY direction around the currenthero, including diagonals
	//IMPORTANT: This target check currently only supports checking all directions at a range of 1. That is to say that a hero's range WILL NOT AFFECT HOW FAR THIS METHOD CHECKS TO RETURN TARGETS
	public List<Transform> TargetCheckAllDirections (Transform currentHero, string heroTypeToSearchFor, string heroIdToExclude) {
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

					if (heroIdToExclude != null) {
						if (otherHero.GetComponent<Hero>().id == heroIdToExclude) {
							break;	
						}
					}

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
	public List<Transform> TargetCheckCardinalDirections (Transform currentHero, string heroTypeToSearchFor) {
		List<Transform> validHeroes = new List<Transform>();
		BuildFullHeroTransformList();
		float currentHeroX = currentHero.transform.position.x;
		float currentHeroY = currentHero.transform.position.y;
		int currentHeroRange = currentHero.GetComponent<Hero>().range;

		foreach (Transform otherHero in fullHeroTransformList) {
			if ( //Check all of the squares around my hero (cardinal directions only) to see if "otherHero" is in range... then based on which type I'm checking for ("enemy" or "ally") add them to the appropriate list
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

	public List<Transform> TargetCheckAllHeroesInRange (Transform currentHero, string heroTypeToSearchFor) {
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

	public List<Transform> TargetCheckClosestHeroInRange (Transform currentHero, string heroTypeToSearchFor) {
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

	//Takes a 'currentHero' and a 'herotype' to search for (valid types are "enemy" and "ally"). It then returns a single hero that is located in ANY direction around the currenthero, including diagonals
	//IMPORTANT: This target check currently only supports checking all directions at a range of 1. That is to say that a hero's range WILL NOT AFFECT HOW FAR THIS METHOD CHECKS TO RETURN TARGETS
	public List<Transform> TargetCheckAllDirectionsOneRandomHero (Transform currentHero, string heroTypeToSearchFor, string heroIdToExclude) {
		Debug.Log("currentHero name is: " + currentHero.name);
		List<Transform> validHeroes = new List<Transform>();
		List<Transform> validHero = new List<Transform>();
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

					if (heroIdToExclude != null) {
						if (otherHero.GetComponent<Hero>().id == heroIdToExclude) {
							break;	
						}
					}

					if (heroTypeToSearchFor == "enemy") {
						if (currentHero.tag != otherHero.tag) {
							validHeroes.Add(otherHero);
							heroAttackedATarget = true;
						}
					} else if (heroTypeToSearchFor == "ally") {
						if (currentHero.tag == otherHero.tag) {
							if (currentHero.GetComponent<Hero>().id == "paladin") {
								Debug.Log("FOUND A PALADIN");
								if (otherHero.GetComponent<Hero>().currentHealth < otherHero.GetComponent<Hero>().maxHealth) {
									Debug.Log("ADDING A HERO TO VALIDHEROES LIST FOR PALADIN");
									validHeroes.Add(otherHero);
								}
							} else {
								validHeroes.Add(otherHero);
							}
						}
					}	
				}
			}

		if (validHeroes.Count > 0) {
			//Pick a random hero from the list of validHeroes and add that to the 'validHero' list
			validHero.Add(validHeroes[Random.Range(0,validHeroes.Count())]);
		}

		return validHero;
	}

	private int CountEnemiesInMyRow (Transform currentHero) {
		int enemyCount = 0;
		if (player1Turn) {
			foreach (Transform hero in player2.transform) {
				if (currentHero.transform.position.y == hero.transform.position.y) {
					enemyCount++;
				}
			}
		} else if (!player1Turn) {
			foreach (Transform hero in player1.transform) {
				if (currentHero.transform.position.y == hero.transform.position.y) {
					enemyCount++;
				}
			}
		}
		return enemyCount;
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
		IncrementTurnsPlayedTracker ();
		turnIndicator.text = "<color=blue>Player 1's Turn</color>";
		EnablePlayer1HandAndHidePlayer2Hand ();
		EnablePlayer1SpellsAndHidePlayer2Spells ();

		Player1AddMana (manaPerTurn);
		StartOfTurnEffects ();
		ReduceSpellCooldowns ();

		//Checks your current hand size and deals you back up to max
		FindObjectOfType<Deck>().Player1DealCards();
	}

	void Player2TurnStart () {
		IncrementTurnsPlayedTracker ();
		turnIndicator.text = "<color=red>Player 2's Turn</color>";
		EnablePlayer2HandAndHidePlayer1Hand ();
		EnablePlayer2SpellsAndHidePlayer1Spells ();

		Player2AddMana (manaPerTurn);
		StartOfTurnEffects ();
		ReduceSpellCooldowns ();

		//Checks your current hand size and deals you back up to max
		if (!GlobalObject.storyEnabled) {
			FindObjectOfType<Deck>().Player2DealCards();
		}

		if (GlobalObject.aiEnabled == true) {
			//Debug.Log("AI is enabled!!!");
			FindObjectOfType<AiManager>().GetComponent<AiManager>().AiTakeTurn();
			//Player2AddMana (1);
		} else if (GlobalObject.storyEnabled) {
			FindObjectOfType<AiManager>().GetComponent<AiManager>().AiStoryTakeTurn();
			Debug.Log("STORY MODE ENABLED");
		}
	}

	void EnablePlayer1HandAndHidePlayer2Hand () {
		//Enable collision on player 1 cards
		BoxCollider2D[] player1Cards = GameObject.Find ("Player1 Hand").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player1Cards) {
			col.enabled = true;
		}
		//Disable collision on player 2 cards
		BoxCollider2D[] player2Cards = GameObject.Find ("Player2 Hand").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player2Cards) {
			col.enabled = false;
		}
		//Visually show player 1 cards and hide player 2 cards
		GameObject.Find ("Player1 Hand").GetComponentInChildren<CanvasGroup> ().alpha = 1;
		GameObject.Find ("Player1 Hand").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = true;
		GameObject.Find ("Player2 Hand").GetComponentInChildren<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Player2 Hand").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = false;
	}

	void EnablePlayer2HandAndHidePlayer1Hand () {
		//Disable collision on player 1 cards
		BoxCollider2D[] player1Cards = GameObject.Find ("Player1 Hand").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player1Cards) {
			col.enabled = false;
		}
		//Enable collision on player 2 cards
		BoxCollider2D[] player2Cards = GameObject.Find ("Player2 Hand").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player2Cards) {
			col.enabled = true;
		}
		//Visually show player 2 cards and hide player 1 cards
		GameObject.Find ("Player1 Hand").GetComponentInChildren<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Player1 Hand").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = false;
		GameObject.Find ("Player2 Hand").GetComponentInChildren<CanvasGroup> ().alpha = 1;
		GameObject.Find ("Player2 Hand").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = true;
	}

	void EnablePlayer1SpellsAndHidePlayer2Spells () {
		//Enable collision on player 1 cards
		BoxCollider2D[] player1Spells = GameObject.Find ("Player1 Spells").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player1Spells) {
			col.enabled = true;
		}
		//Disable collision on player 2 cards
		BoxCollider2D[] player2Spells = GameObject.Find ("Player2 Spells").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player2Spells) {
			col.enabled = false;
		}
		//Visually show player 1 cards and hide player 2 cards
		GameObject.Find ("Player1 Spells").GetComponentInChildren<CanvasGroup> ().alpha = 1;
		GameObject.Find ("Player1 Spells").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = true;
		GameObject.Find ("Player2 Spells").GetComponentInChildren<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Player2 Spells").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = false;
	}

	void EnablePlayer2SpellsAndHidePlayer1Spells () {
		//Disable collision on player 1 cards
		BoxCollider2D[] player1Spells = GameObject.Find ("Player1 Spells").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player1Spells) {
			col.enabled = false;
		}
		//Enable collision on player 2 cards
		BoxCollider2D[] player2Spells = GameObject.Find ("Player2 Spells").GetComponentsInChildren<BoxCollider2D> ();
		foreach (BoxCollider2D col in player2Spells) {
			col.enabled = true;
		}
		//Visually show player 2 cards and hide player 1 cards
		GameObject.Find ("Player1 Spells").GetComponentInChildren<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Player1 Spells").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = false;
		GameObject.Find ("Player2 Spells").GetComponentInChildren<CanvasGroup> ().alpha = 1;
		GameObject.Find ("Player2 Spells").GetComponentInChildren<CanvasGroup> ().blocksRaycasts = true;
	}

	void Player1AddMana (int amt)
	{
		//Add one mana to this player's mana pool
		if (amt < player1ManaMax) {
			player1Mana += amt;
			if (player1Mana > player1ManaMax) {
				player1Mana = player1ManaMax;
			}
		}
		player1ManaText.text = player1Mana.ToString ();
	}

	void Player2AddMana (int amt)
	{
		//Add one mana to this player's mana pool
		if (player2Mana < player2ManaMax) {
			player2Mana += amt;
			if (player2Mana > player2ManaMax) {
				player2Mana = player2ManaMax;
			}
		}
		player2ManaText.text = player2Mana.ToString ();
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

	void StartOfTurnEffects () {
		//Add mana for each Diviner that the player has on the field
		BuildFullHeroTransformList ();
		List<Transform> divinerList = new List<Transform>();

		foreach (Transform hero in fullHeroTransformList) {
			if (hero.GetComponent<Hero>().id == "diviner") {
				divinerList.Add(hero);
			}
		}
		foreach (Transform diviner in divinerList) {
			if (player1Turn && diviner.tag == "player1") {
				if (player1Mana < player1ManaMax) {
					Instantiate (manaParticle, diviner.transform.position, Quaternion.identity);
				}

				Player1AddMana(1);
			} else if (!player1Turn && diviner.tag == "player2") {
				if (player2Mana < player2ManaMax) {
					Instantiate (manaParticle, diviner.transform.position, Quaternion.identity);
				}

				Player2AddMana(1);
			}
		}

		//Decrement the buffs on each hero that the player has on the field
		if (player1Turn) {
			foreach (Transform hero in player1.transform) {
				buffManager.DecrementBuffDurations(hero);
			}
		} else if (!player1Turn) {
			foreach (Transform hero in player2.transform) {
				buffManager.DecrementBuffDurations(hero);
			}
		}

	}

	void EndOfTurnEffects () {
		BuildFullHeroTransformList ();

		//We have to build a separate list of just the druids b/c otherwise if we were to iterate through the fullHeroTransformList and try to make
		//each druid in that full list run TargetCheckAllDirections() we would be modifying the fullHeroTransform list again before we were
		//done with it. This throws an error and will screw things up so don't do it yah knucklehead.
		List<Transform> druidList = new List<Transform>();
		List<Transform> blacksmithList = new List<Transform>();
		List<Transform> paladinList = new List<Transform>();

		foreach (Transform hero in fullHeroTransformList) {

			if (hero.GetComponent<Hero>().id == "druid") {
				druidList.Add(hero);
			} else if (hero.GetComponent<Hero>().id == "blacksmith") {
				blacksmithList.Add(hero);
			} else if (hero.GetComponent<Hero>().id == "paladin") {
				paladinList.Add(hero);
			} else if (hero.GetComponent<Hero>().id == "champion") {
				if (player1Turn && hero.tag == "player1") {
					hero.GetComponent<Hero>().AddArmor(1);
				} else if (!player1Turn && hero.tag == "player2") {
					hero.GetComponent<Hero>().AddArmor(1);
				}

			}
		}

		foreach (Transform druid in druidList) {
			if (player1Turn && druid.tag == "player1") {
				HealHeroesInList(druid, TargetCheckAllDirections(druid, "ally", "ghost"));
			} else if (!player1Turn && druid.tag == "player2") {
				HealHeroesInList(druid, TargetCheckAllDirections(druid, "ally", "ghost"));
			}
		}

		foreach (Transform blacksmith in blacksmithList) {
			if (player1Turn && blacksmith.tag == "player1") {
				ArmorHeroesInList(blacksmith, TargetCheckAllDirectionsOneRandomHero(blacksmith, "ally", "ghost"));
			} else if (!player1Turn && blacksmith.tag == "player2") {
				ArmorHeroesInList(blacksmith, TargetCheckAllDirectionsOneRandomHero(blacksmith, "ally", "ghost"));
			}
		}

		foreach (Transform paladin in paladinList) {
			if (player1Turn && paladin.tag == "player1") {
				HealHeroesInList(paladin, TargetCheckAllDirectionsOneRandomHero(paladin, "ally", "ghost"));
			} else if (!player1Turn && paladin.tag == "player2") {
				HealHeroesInList(paladin, TargetCheckAllDirectionsOneRandomHero(paladin, "ally", "ghost"));
			}
		}
	}

	public void SubtractMana () {
		if (player1Turn) {
			player1Mana = player1Mana - Card.selectedCard.GetComponent<Card>().manaCost;
			player1ManaText.text = player1Mana.ToString();
		} else if (!player1Turn) {
			player2Mana = player2Mana - Card.selectedCard.GetComponent<Card>().manaCost;
			player2ManaText.text = player2Mana.ToString();
		}
	}

	void ReduceSpellCooldowns () {
		if (player1Turn) {
			foreach (Transform spell in GameObject.Find("Player1 Spells").transform) {
				spell.GetComponent<Card>().ReduceSpellCooldown();
			}
		} else if (!player1Turn) {
			foreach (Transform spell in GameObject.Find("Player2 Spells").transform) {
				spell.GetComponent<Card>().ReduceSpellCooldown();
			}
		}
	}

	void IncrementTurnsPlayedTracker ()
	{
		turnsPlayed += 1;
		turnsPlayedText.text = turnsPlayed.ToString ();
	}

	public void TestTest () {
		Debug.LogWarning("TEST TEST");
	}
}