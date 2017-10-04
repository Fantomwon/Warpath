using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalObject : MonoBehaviour {

	public static GlobalObject instance;
	public List<GameObject> player1DeckSelect;
	public List<GameObject> player2DeckSelect;
	public GameObject archerCard, assassinCard, bloodMageCard, cavalryCard, divinerCard, druidCard, footSoldierCard, knightCard, monkCard, rogueCard;
	public string player1Class, player2Class;

	void Awake () {
		Debug.Log("RUNNING AWAKE FUNCTION OF GLOBALOBJECT.CS");
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
