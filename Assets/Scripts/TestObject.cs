using UnityEngine;
using System.Collections;

public class TestObject : MonoBehaviour {

	private PlayField playField;
	private Deck deck;
	private int quantityPerCard = 3;
	// Use this for initialization
	void Start () {
		playField = FindObjectOfType<PlayField>();
		deck = FindObjectOfType<Deck>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FindHeroesInOpponentsTransform () {
		if (playField.player1Turn) {
			foreach (Transform hero in playField.player2.transform) {
				Debug.Log("FLAME STRIKE FOUND AN ENEMY HERO " + hero);
			} 
		} else if (!playField.player1Turn) {
			foreach (Transform hero in playField.player1.transform) {
				Debug.Log("FLAME STRIKE FOUND AN ENEMY HERO " + hero);
			}
		}
	}

	public void ReturnRandomNumber () {
		//Choose a random index
		int randomIndex = Random.Range(0, 2);

		Debug.Log(Mathf.RoundToInt(randomIndex));
	}

	public void FindEachCardInPlayer2Hand () {
//		foreach (Transform card in GameObject.Find("Player2 Hand").transform) {
//				Debug.LogError("FOUND A CARD IN PLAYER 2's HAND");
//		}
	}

	public void TEST () {
		Debug.Log("Player1 Deck Count is :" + deck.player1Deck.Count);
	}
}

