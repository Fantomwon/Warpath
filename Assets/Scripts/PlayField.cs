﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class PlayField : MonoBehaviour {

	public Camera myCamera;
	public bool placedHero = false;
	public bool player1Turn = true;
	public Text turnIndicator;
	public List<Vector2> myHeroCoords;
	public List<Vector2> sortedHeroCoords;
	public List<Vector2> fullHeroCoords;

	private GameObject player1, player2;


	// Use this for initialization
	void Start () {
		player1 = new GameObject("player1");
		player2 = new GameObject("player2");
		turnIndicator.text = "<color=blue>Player 1's Turn</color>";
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
		Vector2 roundedPos = SnapToGrid(rawPos);

		GameObject x = Instantiate(Card.selectedHero, roundedPos, Quaternion.identity) as GameObject;

		//Child the newly spawned hero to the appropriate player
		if (player1Turn) {
			x.transform.parent = player1.transform;
			x.gameObject.tag = "player1";
		} else if (!player1Turn) {
			//Flip the hero so it faces to the left
			x.GetComponentInChildren<SpriteRenderer>().transform.localScale *= -1;

			//Move the text so that it sits on the left side of the hero
			Vector3 newTextPosition = x.GetComponentInChildren<Text>().rectTransform.localPosition;
			newTextPosition.x *= -1;
			x.GetComponentInChildren<Text>().rectTransform.localPosition = newTextPosition;

			x.transform.parent = player2.transform;
			x.gameObject.tag = "player2";
		}
		placedHero = true;
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
		}

	void BuildSortedHeroList () {
		Debug.LogWarning("BUILDING SORTED HERO LIST");
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

		MoveHeroes ();
	}

	public void MoveHeroes ()
	{
		Debug.LogWarning("sortedHeroCoords has " + sortedHeroCoords.ToArray().Length + " entries");
		if (sortedHeroCoords.ToArray().Length <= 0) {
			player1Turn = !player1Turn;
			if (player1Turn) {
				turnIndicator.text = "<color=blue>Player 1's Turn</color>";
			} else if (!player1Turn) {
				turnIndicator.text = "<color=red>Player 2's Turn</color>";
			}
			return;
		}
		Debug.LogWarning("RUNNING MOVEHEROES");
		//Search each hero to see if their coords match the first set of coords in the 'sortedCoords' list. If they do, move that hero, then remove that hero's
		//coords from the sortedHeroCoords list and wait for this "MoveHeroes" method to be called again.
		if (player1Turn) {
			for (int i = 0; i < 1; i++) {
				print ("FOR LOOP IS AT: " + i);
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
				print ("FOR LOOP IS AT: " + i);
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
		//If there's nothing in my way move me forward by my full 'speed' stat
		if (currentHero.GetComponent<Hero>().speed < Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroRight(currentHero.GetComponent<Hero>().speed);
		} else if (currentHero.GetComponent<Hero>().speed >= Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroRight(Mathf.RoundToInt(closestHero)-1);

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
		//If there's nothing in my way move me forward by my full 'speed' stat
		if (currentHero.GetComponent<Hero>().speed < Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroLeft(currentHero.GetComponent<Hero>().speed);
		} else if (currentHero.GetComponent<Hero>().speed >= Mathf.RoundToInt(closestHero)) {
			currentHero.GetComponent<Hero>().MoveSingleHeroLeft(Mathf.RoundToInt(closestHero)-1);
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

	public void Player1EnemyCheck (Transform currentHero) {

		foreach (Transform enemy in player2.transform) {
			if ((Mathf.RoundToInt(enemy.transform.position.x) - Mathf.RoundToInt(currentHero.transform.position.x) <= currentHero.GetComponent<Hero>().range) && enemy.transform.position.y == currentHero.transform.position.y) {
				enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().damage);
				Debug.Log("ATTACKING FOR: " + currentHero.GetComponent<Hero>().damage);
				Debug.Log("ENEMY HP IS: " + enemy.GetComponent<Hero>().health);
			} else {
				Debug.Log("DIDN'T FIND ANYONE TO ATTACK");
			}
		}
	}

	public void Player2EnemyCheck (Transform currentHero) {
		foreach (Transform enemy in player1.transform) {
			if ((Mathf.RoundToInt(currentHero.transform.position.x) - Mathf.RoundToInt(enemy.transform.position.x) <= currentHero.GetComponent<Hero>().range) && enemy.transform.position.y == currentHero.transform.position.y) {
				enemy.GetComponent<Hero>().TakeDamage(currentHero.GetComponent<Hero>().damage);
				Debug.Log("ATTACKING FOR: " + currentHero.GetComponent<Hero>().damage);
				Debug.Log("ENEMY HP IS: " + enemy.GetComponent<Hero>().health);
			}
		}

	}

	public void TestTest () {
		Debug.LogWarning("TEST TEST");
	}
}