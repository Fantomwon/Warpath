using UnityEngine;
using System.Collections;

public class TestObject : MonoBehaviour {

	private PlayField playField;

	// Use this for initialization
	void Start () {
		playField = FindObjectOfType<PlayField>();
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

	public void BuildDeck () {
		Debug.Log("RUNNING BUILDDECK()");
		foreach (GameObject card in GlobalObject.instance.player1DeckSelect) {
			for (int i=0; i<card.GetComponent<Card>().quantity; i++) {
				Debug.Log("FOUND A CARD IN PLAYER1DECKSELECTLIST");
				FindObjectOfType<Deck>().player1Deck.Add(card);
			}
		}
	}
}
