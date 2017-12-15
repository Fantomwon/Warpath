using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GlobalObject : MonoBehaviour {

	public static GlobalObject instance;
	public static bool aiEnabled = false;
	public static bool storyEnabled = false;
	public static string currentlyActiveStory;
	public List<GameObject> player1DeckSelect;
	public List<GameObject> player2DeckSelect;
	public GameObject archerCard, assassinCard, blacksmithCard, bloodMageCard, cavalryCard, championCard, chaosMageCard, divinerCard, druidCard, dwarfCard, footSoldierCard, ghostCard, knightCard, monkCard, paladinCard, rogueCard, sapperCard, sorcererCard, wolfCard;
	public string player1Class, player2Class;

	void Awake () {
//		Debug.Log("RUNNING AWAKE FUNCTION OF GLOBALOBJECT.CS");
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
//		if (SceneManager.GetActiveScene().name == "BossSelect") {
//			aiEnabled = true;
//		}
	}
}
