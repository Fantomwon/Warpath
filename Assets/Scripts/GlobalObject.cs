using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GlobalObject : MonoBehaviour {

	public static GlobalObject instance;
	public static bool aiEnabled = false;
	public static bool storyEnabled = false;
	public static string currentlyActiveStory;
	public List<string> player1DeckSelect;
	public List<string> player2DeckSelect;
	public GameObject templateCard;
	public string player1Class, player2Class;

	public List<string> boss01PlayerCards, boss02PlayerCards, boss03PlayerCards, boss04PlayerCards, boss05PlayerCards;

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
			//Debug.LogError("YES I LOADED THE CardSelectSinglePlayer SCENE");
			InstantiatePlayerCards ();
		}
	}

	void Start () {
	}

	public GameObject SetTemplateCardAttributes (string id) {
		if (id == "archer") {
			Debug.Log("SETTING CARD ATTRIBUTES TO ARCHER");		
			templateCard.GetComponent<Card>().name = "ArcherCard";
			templateCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>("PrefabsHeroes/Archer");
			templateCard.GetComponent<Card>().cardId = "archer";
			templateCard.GetComponent<Card>().cardName = "Test:Archer";
			templateCard.GetComponent<Card>().type = "hero";
			templateCard.GetComponent<Card>().manaCost = 3;
			return templateCard;
		} else if (id == "footsoldier") {
			Debug.Log("SETTING CARD ATTRIBUTES TO FOOTSOLDIER");		
			templateCard.GetComponent<Card>().name = "FootSoldierCard";
			templateCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>("PrefabsHeroes/FootSoldier");
			templateCard.GetComponent<Card>().cardId = "footsoldier";
			templateCard.GetComponent<Card>().cardName = "Test:Foot Soldier";
			templateCard.GetComponent<Card>().type = "hero";
			templateCard.GetComponent<Card>().manaCost = 3;
			return templateCard;
		} else if (id == "rogue") {
			Debug.Log("SETTING CARD ATTRIBUTES TO ROGUE");	
			templateCard.GetComponent<Card>().name = "RogueCard";
			templateCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>("PrefabsHeroes/Rogue");
			templateCard.GetComponent<Card>().cardId = "rogue";
			templateCard.GetComponent<Card>().cardName = "Test:Rogue";
			templateCard.GetComponent<Card>().type = "hero";
			templateCard.GetComponent<Card>().manaCost = 4;
			return templateCard;
		} else if (id == "druid") {
			Debug.Log("SETTING CARD ATTRIBUTES TO DRUID");	
			templateCard.GetComponent<Card>().name = "DruidCard";
			templateCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>("PrefabsHeroes/Druid");
			templateCard.GetComponent<Card>().cardId = "druid";
			templateCard.GetComponent<Card>().cardName = "Test:Druid";
			templateCard.GetComponent<Card>().type = "hero";
			templateCard.GetComponent<Card>().manaCost = 4;
			return templateCard;
		}  
		else {
			return null;
		}
	}

	public void SetTemplateCardImage (string id, GameObject card) {
		if (id == "archer") {
			GameObject newHero = Instantiate (Resources.Load<GameObject>("PrefabsHeroes/Archer")) as GameObject;  
			newHero.transform.SetParent (card.transform.Find("Image").transform, false);   
		} else if (id == "footsoldier") {
			GameObject newHero = Instantiate (Resources.Load<GameObject>("PrefabsHeroes/FootSoldier")) as GameObject;  
			newHero.transform.SetParent (card.transform.Find("Image").transform, false);
		} else if (id == "rogue") {
			GameObject newHero = Instantiate (Resources.Load<GameObject>("PrefabsHeroes/Rogue")) as GameObject;  
			newHero.transform.SetParent (card.transform.Find("Image").transform, false);
		} else if (id == "druid") {
			GameObject newHero = Instantiate (Resources.Load<GameObject>("PrefabsHeroes/Druid")) as GameObject;  
			newHero.transform.SetParent (card.transform.Find("Image").transform, false);
		}
	}

	void AssignPlayerCards () {
		//boss01 player cards
		boss01PlayerCards.Add("archer");
		boss01PlayerCards.Add("footsoldier");
		boss01PlayerCards.Add("rogue");
		boss01PlayerCards.Add("druid");
		//boss01PlayerCards.Add("knight");

		//boss02 player cards
		boss02PlayerCards.Add("archer");
		boss02PlayerCards.Add("footsoldier");
		//boss01PlayerCards.Add("rogue");
		//boss01PlayerCards.Add("knight");

		//boss03 player cards
		boss03PlayerCards.Add("archer");
		boss03PlayerCards.Add("footsoldier");
		//boss01PlayerCards.Add("rogue");
		//boss01PlayerCards.Add("knight");

		//boss04 player cards
		boss04PlayerCards.Add("archer");
		boss04PlayerCards.Add("footsoldier");
		//boss01PlayerCards.Add("rogue");
		//boss01PlayerCards.Add("knight");

		//boss05 player cards
		boss05PlayerCards.Add("archer");
		boss05PlayerCards.Add("footsoldier");
		//boss01PlayerCards.Add("rogue");
		//boss01PlayerCards.Add("knight");
	}

	void InstantiatePlayerCards ()
	{
		if (currentlyActiveStory == "boss01") {
			foreach (string cardId in boss01PlayerCards) {
				Debug.Log("In$$$stantiate player card: " + cardId);
				SetTemplateCardAttributes(cardId);
				GameObject newCard = Instantiate (templateCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss02") {
			foreach (string cardId in boss02PlayerCards) {
				SetTemplateCardAttributes(cardId);
				GameObject newCard = Instantiate (templateCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss03") {
			foreach (string cardId in boss03PlayerCards) {
				SetTemplateCardAttributes(cardId);
				GameObject newCard = Instantiate (templateCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss04") {
			foreach (string cardId in boss04PlayerCards) {
				SetTemplateCardAttributes(cardId);
				GameObject newCard = Instantiate (templateCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss05") {
			foreach (string cardId in boss05PlayerCards) {
				SetTemplateCardAttributes(cardId);
				GameObject newCard = Instantiate (templateCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		}
	}
}
