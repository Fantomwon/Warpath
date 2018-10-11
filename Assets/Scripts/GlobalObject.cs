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

	//Idk know why these variables need to be public... but every time I set them to 'private' ZERO cards will show up in the various 'deckSelect' screens. Maybe one day I'll figure this out, or perhaps I never will...
	public List<string> boss01PlayerCards, boss02PlayerCards, boss03PlayerCards, boss04PlayerCards, boss05PlayerCards;
	public List<string> fullPlayerCardList;

	void Awake () {
//		Debug.Log("RUNNING AWAKE FUNCTION OF GLOBALOBJECT.CS");
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}

		if (SceneManager.GetActiveScene().name != "BossSelect" && SceneManager.GetActiveScene().name != "Game") {
			AssignPlayerCards();
			InstantiatePlayerCards ();
		}
	}

	void Start () {
		
	}

	public GameObject SetTemplateCardAttributes (string id) {
		if (id == "wall") {
			return SetTemplateCardAttributesConstructor("WallCard", "wall", "heroStationary", 2, "Test: Wall", "PrefabsHeroes/Wall");
		} else if (id == "archer") {
			return SetTemplateCardAttributesConstructor("ArcherCard", "archer", "hero", 3, "Test: Archer", "PrefabsHeroes/Archer");
		} else if (id == "footsoldier") {
			return SetTemplateCardAttributesConstructor("FootSoldierCard", "footsoldier", "hero", 3, "Test:Foot Soldier", "PrefabsHeroes/FootSoldier");
		} else if (id == "dwarf") {
			return SetTemplateCardAttributesConstructor("DwarfCard", "dwarf", "hero", 3, "Test:Dwarf", "PrefabsHeroes/Dwarf");
		} else if (id == "ghost") {
			return SetTemplateCardAttributesConstructor("GhostCard", "ghost", "hero", 3, "Test:Ghost", "PrefabsHeroes/Ghost");
		} else if (id == "sapper") {
			return SetTemplateCardAttributesConstructor("SapperCard", "sapper", "hero", 3, "Test:Sapper", "PrefabsHeroes/Sapper");
		} else if (id == "rogue") {
			return SetTemplateCardAttributesConstructor("RogueCard", "rogue", "hero", 4, "Test:Rogue", "PrefabsHeroes/Rogue");
		} else if (id == "druid") {
			return SetTemplateCardAttributesConstructor("DruidCard", "druid", "hero", 4, "Test:Druid", "PrefabsHeroes/Druid");
		} else if (id == "monk") {
			return SetTemplateCardAttributesConstructor("MonkCard", "monk", "hero", 4, "Test:monk", "PrefabsHeroes/Monk");
		} else if (id == "slinger") {
			return SetTemplateCardAttributesConstructor("SlingerCard", "slinger", "hero", 4, "Test:slinger", "PrefabsHeroes/Slinger");
		} else if (id == "bloodmage") {
			return SetTemplateCardAttributesConstructor("BloodmageCard", "bloodmage", "hero", 4, "Test:bloodmage", "PrefabsHeroes/Bloodmage");
		} else if (id == "blacksmith") {
			return SetTemplateCardAttributesConstructor("BlacksmithCard", "blacksmith", "hero", 4, "Test:blacksmith", "PrefabsHeroes/Blacksmith");
		} else if (id == "wolf") {
			return SetTemplateCardAttributesConstructor("WolfCard", "wolf", "hero", 4, "Test:wolf", "PrefabsHeroes/Wolf");
		} else if (id == "chaosmage") {
			return SetTemplateCardAttributesConstructor("ChaosMageCard", "chaosmage", "hero", 4, "Test:chaosmage", "PrefabsHeroes/ChaosMage");
		} else if (id == "sorcerer") {
			return SetTemplateCardAttributesConstructor("SorcererCard", "sorcerer", "hero", 4, "Test:sorcerer", "PrefabsHeroes/Sorcerer");
		} else if (id == "assassin") {
			return SetTemplateCardAttributesConstructor("AssassinCard", "assassin", "hero", 4, "Test:assassin", "PrefabsHeroes/Assassin");
		} else if (id == "knight") {
			return SetTemplateCardAttributesConstructor("KnightCard", "knight", "hero", 5, "Test:knight", "PrefabsHeroes/Knight");
		} else if (id == "paladin") {
			return SetTemplateCardAttributesConstructor("PaladinCard", "paladin", "hero", 5, "Test:paladin", "PrefabsHeroes/Paladin");
		} else if (id == "tower") {
			return SetTemplateCardAttributesConstructor("TowerCard", "tower", "heroStationary", 5, "Test:tower", "PrefabsHeroes/Tower");
		} else if (id == "champion") {
			return SetTemplateCardAttributesConstructor("ChampionCard", "champion", "hero", 5, "Test:champion", "PrefabsHeroes/Champion");
		} else if (id == "cavalry") {
			return SetTemplateCardAttributesConstructor("CavalryCard", "cavalry", "hero", 5, "Test:cavalry", "PrefabsHeroes/Cavalry");
		} else if (id == "diviner") {
			return SetTemplateCardAttributesConstructor("DivinerCard", "diviner", "hero", 5, "Test:diviner", "PrefabsHeroes/Diviner");
		} else {
			return null;
		}
	}

	private GameObject SetTemplateCardAttributesConstructor (string name, string cardId, string type, int manaCost, string cardName, string heroPrefab) {
		templateCard.GetComponent<Card>().name = name;
		templateCard.GetComponent<Card>().cardId = cardId;
		templateCard.GetComponent<Card>().type = type;
		templateCard.GetComponent<Card>().manaCost = manaCost;
		templateCard.GetComponent<Card>().cardName = cardName;
		templateCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>(heroPrefab);
		return templateCard;
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

	private void AssignPlayerCards () {
		//Full list of all players cards for pass and play
		fullPlayerCardList.Add("wall");
		//fullPlayerCardList.Add("archer");
		//fullPlayerCardList.Add("footsoldier");
		fullPlayerCardList.Add("dwarf");
		fullPlayerCardList.Add("ghost");
		fullPlayerCardList.Add("sapper");
		fullPlayerCardList.Add("rogue");
		fullPlayerCardList.Add("druid");
		fullPlayerCardList.Add("monk");
		fullPlayerCardList.Add("slinger");
		fullPlayerCardList.Add("bloodmage");
		fullPlayerCardList.Add("blacksmith");
		fullPlayerCardList.Add("wolf");
		fullPlayerCardList.Add("chaosmage");
		fullPlayerCardList.Add("sorcerer");
		fullPlayerCardList.Add("assassin");
		fullPlayerCardList.Add("paladin");
		fullPlayerCardList.Add("tower");
		fullPlayerCardList.Add("knight");
		fullPlayerCardList.Add("champion");
		fullPlayerCardList.Add("cavalry");
		fullPlayerCardList.Add("diviner");

		//boss01 player cards
		boss01PlayerCards.Add("archer");
		boss01PlayerCards.Add("footsoldier");
		boss01PlayerCards.Add("rogue");
		boss01PlayerCards.Add("druid");
		boss01PlayerCards.Add("sapper");
		//boss01PlayerCards.Add("knight");

		//boss02 player cards
		boss02PlayerCards.Add("archer");
		boss02PlayerCards.Add("footsoldier");
		boss02PlayerCards.Add("footsoldier");
		boss02PlayerCards.Add("footsoldier");

		//boss03 player cards
		boss03PlayerCards.Add("archer");
		boss03PlayerCards.Add("footsoldier");

		//boss04 player cards
		boss04PlayerCards.Add("archer");
		boss04PlayerCards.Add("footsoldier");

		//boss05 player cards
		boss05PlayerCards.Add("archer");
		boss05PlayerCards.Add("footsoldier");
	}

	void InstantiatePlayerCards () {
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
		} else {
			foreach (string cardId in fullPlayerCardList) {
				SetTemplateCardAttributes(cardId);
				GameObject newCard = Instantiate (templateCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		}
	}
}
