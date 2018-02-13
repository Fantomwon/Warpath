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
	public GameObject archerCard, assassinCard, blacksmithCard, bloodMageCard, cavalryCard, championCard, chaosMageCard, divinerCard, druidCard, slingerCard, dwarfCard, footSoldierCard, ghostCard, knightCard, monkCard, paladinCard, rogueCard, sapperCard, sorcererCard, wolfCard,
					  armorCard, buffMightCard, buffShroudCard, debuffRootCard, fireballCard, healCard, rockThrowCard, windGustCard;
	public string player1Class, player2Class;

	public List<GameObject> boss01PlayerCards, boss02PlayerCards, boss03PlayerCards, boss04PlayerCards, boss05PlayerCards;

	void Awake () {
//		Debug.Log("RUNNING AWAKE FUNCTION OF GLOBALOBJECT.CS");
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}

		if (boss01PlayerCards.Count == 0) {
			AssignPlayerCards();
		}

		if (SceneManager.GetActiveScene().name == "CardSelectSinglePlayer") {
			Debug.LogError("FUCK YES I LOADED THE CardSelectSinglePlayer SCENE");
			InstantiatePlayerCards ();
		}
	}

	void Start () {
	}

	void AssignPlayerCards () {
		//boss01 player cards
		boss01PlayerCards.Add(archerCard);
		boss01PlayerCards.Add(footSoldierCard);
		boss01PlayerCards.Add(rogueCard);
		boss01PlayerCards.Add(knightCard);

		//boss02 player cards
		boss02PlayerCards.Add(archerCard);
		boss02PlayerCards.Add(footSoldierCard);
		boss02PlayerCards.Add(rogueCard);
		boss02PlayerCards.Add(knightCard);

		//boss03 player cards
		boss03PlayerCards.Add(archerCard);
		boss03PlayerCards.Add(footSoldierCard);
		boss03PlayerCards.Add(rogueCard);
		boss03PlayerCards.Add(knightCard);
		boss03PlayerCards.Add(bloodMageCard);

		//boss04 player cards
		boss04PlayerCards.Add(archerCard);
		boss04PlayerCards.Add(footSoldierCard);
		boss04PlayerCards.Add(rogueCard);
		boss04PlayerCards.Add(knightCard);
		boss04PlayerCards.Add(bloodMageCard);

		//boss05 player cards
		boss05PlayerCards.Add(archerCard);
		boss05PlayerCards.Add(footSoldierCard);
		boss05PlayerCards.Add(rogueCard);
		boss05PlayerCards.Add(knightCard);
		boss05PlayerCards.Add(bloodMageCard);
		boss05PlayerCards.Add(assassinCard);
	}

	void InstantiatePlayerCards ()
	{
		if (currentlyActiveStory == "boss01") {
			foreach (GameObject card in boss01PlayerCards) {
				GameObject newCard = Instantiate (card) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
			}
		} else if (currentlyActiveStory == "boss02") {
			foreach (GameObject card in boss02PlayerCards) {
				GameObject newCard = Instantiate (card) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
			}
		} else if (currentlyActiveStory == "boss03") {
			foreach (GameObject card in boss03PlayerCards) {
				GameObject newCard = Instantiate (card) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
			}
		} else if (currentlyActiveStory == "boss04") {
			foreach (GameObject card in boss04PlayerCards) {
				GameObject newCard = Instantiate (card) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
			}
		} else if (currentlyActiveStory == "boss05") {
			foreach (GameObject card in boss05PlayerCards) {
				GameObject newCard = Instantiate (card) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
			}
		}
	}
}
