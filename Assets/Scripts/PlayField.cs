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
	public int player1Health = 5, player2Health = 5;
	public List<Vector2> myHeroCoords;
	public List<Vector2> sortedHeroCoords;
	public List<Vector2> fullHeroCoords;
	public Vector2 roundedPos;
	public GameObject player1, player2;

	private Hero hero;


	// Use this for initialization
	void Start () {
		player1 = new GameObject("player1");
		player2 = new GameObject("player2");
		hero = FindObjectOfType<Hero>();
		Player1TurnStart();
		player1HealthText.text = player1Health.ToString();
		player2HealthText.text = player2Health.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		//print (Input.mousePosition);
		//print (SnapToGrid(CalculateWorldPointOfMouseClick()));
			
		//Use CalculateWorldPointOfMouseClick method to get the 'raw position' (position in pixels) of where the player clicked in the world
		Vector2 rawPos = CalculateWorldPointOfMouseClick();
		//Use SnapToGrid method to turn rawPos into rounded integer units in world space coordinates
		roundedPos = SnapToGrid(rawPos);

		//If the selected card is a spell card, cast it, ELSE treat the card as a hero card
		if (Card.selectedCard.GetComponent<Card>().type == "Spell") {
			Card.selectedCard.GetComponent<Card>().CastSpell();
			//Remove the card that I just played from my hand
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

			//Put the card that was just played into the appropriate player's discard pile
			FindObjectOfType<Deck>().Player1AddCardToDiscard(Card.selectedHero.GetComponent<Hero>().cardPrefab);

		} else if (!player1Turn) {
			//Flip the hero so it faces to the left
			Vector3 scale = x.GetComponentInChildren<SpriteRenderer>().transform.localScale;
			scale.x = scale.x *= -1;
			x.GetComponentInChildren<SpriteRenderer>().transform.localScale = scale;

			//Move the text so that it sits on the left side of the hero
			Vector3 newTextPosition = x.GetComponentInChildren<Text>().rectTransform.localPosition;
			newTextPosition.x *= -1;
			x.GetComponentInChildren<Text>().rectTransform.localPosition = newTextPosition;

			//Child the newly spawned hero to the appropriate player
			x.transform.SetParent(player2.transform, false);
			x.gameObject.tag = "player2";

			//Put the card that was just played into the appropriate player's discard pile
			FindObjectOfType<Deck>().Player2AddCardToDiscard(Card.selectedHero.GetComponent<Hero>().cardPrefab);
		}
	
		//Remove the card that I just played from my hand
		Destroy(Card.selectedCard);
		//Reset the 'selectedHero' variable so players can't place another hero before selecting another card
		Card.selectedHero = default(GameObject);
		//Reset the 'selectedCard' variable so players can't add multiple of them to their discard pile
		Card.selectedCard = default(GameObject);
		//placedHero = true;
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
		//Debug.LogWarning("sortedHeroCoords has " + sortedHeroCoords.ToArray().Length + " entries");

		// If there are no more heroes left to move then end my turn
		if (sortedHeroCoords.ToArray().Length <= 0) {
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

		 //If there's nothing in my way move me forward by my full 'speed' stat
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

		foreach (Transform enemy in player1.transform) {
			if ((Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) <= currentHero.GetComponent<Hero>().range)
			&& (Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) > 0)
			&& enemy.transform.position.y == currentHero.transform.position.y) {
				currentHero.GetComponent<Hero>().MoveSingleHeroLeftAndAttack(0);
				return;
			}
		}

		//If there's nothing in my way move me forward by my full 'speed' stat
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
			Debug.Log("ADDING FULL HERO COORDS: " + coord);
		}
		foreach (Transform hero in player2.transform) {
			float x = hero.transform.position.x;
			float y = hero.transform.position.y;
			Vector2 coord = new Vector2(x, y);
			fullHeroCoords.Add(coord);
		}
	}

	public void Player1EnemyCheck (Transform currentHero) {
		foreach (Transform enemy in player2.transform) {
			if ((Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) <= currentHero.GetComponent<Hero>().range)
				&& (Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) > 0)
				&& enemy.transform.position.y == currentHero.transform.position.y) {
					enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().power);
					//Debug.Log("ATTACKING FOR: " + currentHero.GetComponent<Hero>().damage);
					//Debug.Log("ENEMY HP IS: " + enemy.GetComponent<Hero>().health);
			} else {
				//Debug.Log("DIDN'T FIND ANYONE TO ATTACK");
			}
		}
	}

	public void Player2EnemyCheck (Transform currentHero) {
		foreach (Transform enemy in player1.transform) {
			if ((Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) <= currentHero.GetComponent<Hero>().range)
			&& (Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) > 0)
			&& enemy.transform.position.y == currentHero.transform.position.y) {
				enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().power);
				//Debug.Log("ATTACKING FOR: " + currentHero.GetComponent<Hero>().damage);
				//Debug.Log("ENEMY HP IS: " + enemy.GetComponent<Hero>().health);
			}
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
			Debug.Log("RUNNING LosePlayerHealth");
			player2Health -= dmg;
			player2HealthText.text = player2Health.ToString();
		} else if (!player1Turn) {
			player1Health -= dmg;
			player1HealthText.text = player1Health.ToString();
		}
	}

	public void TestTest () {
		Debug.LogWarning("TEST TEST");
	}
}