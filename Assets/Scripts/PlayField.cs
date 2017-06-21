using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayField : MonoBehaviour {

	public Camera myCamera;
	public bool placedHero = false;
	public bool player1Turn = true;
	public Text turnIndicator;
	public List<Vector2> heroCoords;
	public List<Vector2> sortedCoords;

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
			Vector3 newScale = x.transform.localScale;
			newScale.x *= -1;
			x.transform.localScale = newScale;

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
			MoveHeroes ();
			player1Turn = !player1Turn;
			if (player1Turn) {
				turnIndicator.text = "<color=blue>Player 1's Turn</color>";
			} else if (!player1Turn) {
				turnIndicator.text = "<color=red>Player 2's Turn</color>";
			}
		}

	void MoveHeroes () {

		//Empty the heroCoords list so we can build it again
		heroCoords.Clear();

		//Find each hero the player has placed and add the hero coordinates into a list
		if (player1Turn) {
				foreach (Transform child in player1.transform) {
				float x = child.transform.position.x;
				float y = child.transform.position.y;
				Vector2 coord = new Vector2(x, y);
				heroCoords.Add(coord);
			}
		} else if (!player1Turn) {
				foreach (Transform child in player2.transform) {
				float x = child.transform.position.x;
				float y = child.transform.position.y;
				Vector2 coord = new Vector2(x, y);
				heroCoords.Add(coord);
			}
		}

		//Sort all of the hero coordinates in the order we want them to be evaluated for movement/attacking
		if (player1Turn) {
			Debug.Log("Trying to sort player 1's heroes");
			sortedCoords = heroCoords.OrderByDescending(value => value.y).ThenByDescending(value => value.x).ToList();
		} else if (!player1Turn) {
			Debug.Log("Trying to sort player 2's heroes");
			sortedCoords = heroCoords.OrderByDescending(value => value.y).ThenBy(value => value.x).ToList();
		}

		//Search each hero to see if their coords match the first set of coords in the 'sortedCoords' list. If they do, move that hero, then do the same thing for the next set of coords until
		//we've gone through all of them
		if (player1Turn) {
			for (int i=0; i < sortedCoords.ToArray().Length; i++ ) {
				foreach (Transform child in player1.transform) {
//					foreach (Transform enemy in player2.transform) {
//						if (enemy.transform.position.x == child.transform.position.x+child.GetComponent<Hero>().range) {
//							//ATTACK ENEMY
//							Debug.Log(child.name + " TRYING TO ATTACK ENEMY " + enemy.name);
//						}
//					}
					if (child.transform.position.x == sortedCoords[i].x && child.transform.position.y == sortedCoords[i].y) {
						child.transform.Translate(Vector2.right * child.GetComponent<Hero>().speed);
						}
					}
			}
		} else if (!player1Turn) {
			for (int i=0; i < sortedCoords.ToArray().Length; i++ ) {
				foreach (Transform child in player2.transform) {
//					foreach (Transform enemy in player1.transform) {
//						if (enemy.transform.position.x == child.transform.position.x-child.GetComponent<Hero>().range) {
//							//ATTACK ENEMY
//							Debug.Log(child.name + " TRYING TO ATTACK ENEMY " + enemy.name);
//						}
//					}
					if (child.transform.position.x == sortedCoords[i].x && child.transform.position.y == sortedCoords[i].y) {
						child.transform.Translate(Vector2.right * child.GetComponent<Hero>().speed);
						}
					}
			}
		}
	}
}