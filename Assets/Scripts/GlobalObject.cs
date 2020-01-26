﻿using UnityEngine;
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
	public GameObject templateHeroCard;
	public GameObject templateSpellCard;
	public string player1Class, player2Class;

	//Idk know why these variables need to be public... but every time I set them to 'private' ZERO cards will show up in the various 'deckSelect' screens. Maybe one day I'll figure this out, or perhaps I never will...
	public List<string> boss01PlayerHeroCards, boss02PlayerHeroCards, boss03PlayerHeroCards, boss04PlayerHeroCards, boss05PlayerHeroCards;
	public List<string> boss01PlayerSpellCards, boss02PlayerSpellCards, boss03PlayerSpellCards, boss04PlayerSpellCards, boss05PlayerSpellCards;
	public List<string> fullPlayerHeroCardList;
	public List<string> fullPlayerSpellCardList;

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

    public GameObject SetTemplateHeroCardAttributes (string id) {
        if (id == "wall") {
            return SetTemplateHeroCardAttributesConstructor("WallCard", "wall", "heroStationary", 2, "Test: Wall", "PrefabsHeroes/Wall");
        } else if (id == "archer") {
            return SetTemplateHeroCardAttributesConstructor("ArcherCard", "archer", "hero", 3, "Test: Archer", "PrefabsHeroes/Archer");
        } else if (id == "footsoldier") {
            return SetTemplateHeroCardAttributesConstructor("FootSoldierCard", "footsoldier", "hero", 3, "Test:Foot Soldier", "PrefabsHeroes/FootSoldier");
        } else if (id == "dwarf") {
            return SetTemplateHeroCardAttributesConstructor("DwarfCard", "dwarf", "hero", 3, "Test:Dwarf", "PrefabsHeroes/Dwarf");
        } else if (id == "ghost") {
            return SetTemplateHeroCardAttributesConstructor("GhostCard", "ghost", "hero", 3, "Test:Ghost", "PrefabsHeroes/Ghost");
        } else if (id == "sapper") {
            return SetTemplateHeroCardAttributesConstructor("SapperCard", "sapper", "hero", 3, "Test:Sapper", "PrefabsHeroes/Sapper");
        } else if (id == "rogue") {
            return SetTemplateHeroCardAttributesConstructor("RogueCard", "rogue", "hero", 4, "Test:Rogue", "PrefabsHeroes/Rogue");
        } else if (id == "druid") {
            return SetTemplateHeroCardAttributesConstructor("DruidCard", "druid", "hero", 4, "Test:Druid", "PrefabsHeroes/Druid");
        } else if (id == "monk") {
            return SetTemplateHeroCardAttributesConstructor("MonkCard", "monk", "hero", 4, "Test:monk", "PrefabsHeroes/Monk");
        } else if (id == "slinger") {
            return SetTemplateHeroCardAttributesConstructor("SlingerCard", "slinger", "hero", 4, "Test:slinger", "PrefabsHeroes/Slinger");
        } else if (id == "bloodmage") {
            return SetTemplateHeroCardAttributesConstructor("BloodmageCard", "bloodmage", "hero", 4, "Test:bloodmage", "PrefabsHeroes/Bloodmage");
        } else if (id == "blacksmith") {
            return SetTemplateHeroCardAttributesConstructor("BlacksmithCard", "blacksmith", "hero", 4, "Test:blacksmith", "PrefabsHeroes/Blacksmith");
        } else if (id == "wolf") {
            return SetTemplateHeroCardAttributesConstructor("WolfCard", "wolf", "hero", 4, "Test:wolf", "PrefabsHeroes/Wolf");
        } else if (id == "chaosmage") {
            return SetTemplateHeroCardAttributesConstructor("ChaosMageCard", "chaosmage", "hero", 4, "Test:chaosmage", "PrefabsHeroes/ChaosMage");
        } else if (id == "sorcerer") {
            return SetTemplateHeroCardAttributesConstructor("SorcererCard", "sorcerer", "hero", 4, "Test:sorcerer", "PrefabsHeroes/Sorcerer");
        } else if (id == "assassin") {
            return SetTemplateHeroCardAttributesConstructor("AssassinCard", "assassin", "hero", 4, "Test:assassin", "PrefabsHeroes/Assassin");
        } else if (id == "knight") {
            return SetTemplateHeroCardAttributesConstructor("KnightCard", "knight", "hero", 5, "Test:knight", "PrefabsHeroes/Knight");
        } else if (id == "paladin") {
            return SetTemplateHeroCardAttributesConstructor("PaladinCard", "paladin", "hero", 5, "Test:paladin", "PrefabsHeroes/Paladin");
        } else if (id == "tower") {
            return SetTemplateHeroCardAttributesConstructor("TowerCard", "tower", "heroStationary", 5, "Test:tower", "PrefabsHeroes/Tower");
        } else if (id == "champion") {
            return SetTemplateHeroCardAttributesConstructor("ChampionCard", "champion", "hero", 5, "Test:champion", "PrefabsHeroes/Champion");
        } else if (id == "cavalry") {
            return SetTemplateHeroCardAttributesConstructor("CavalryCard", "cavalry", "hero", 5, "Test:cavalry", "PrefabsHeroes/Cavalry");
        } else if (id == "diviner") {
            return SetTemplateHeroCardAttributesConstructor("DivinerCard", "diviner", "hero", 5, "Test:diviner", "PrefabsHeroes/Diviner");
        } else if (id == "crossbowman") {
            return SetTemplateHeroCardAttributesConstructor("CrossbowmanCard", "crossbowman", "hero", 3, "Test:crossbowman", "PrefabsHeroes/Crossbowman");
        }else if (id == "bloodknight") {
            return SetTemplateHeroCardAttributesConstructor("BloodknightCard", "bloodknight", "hero", 5, "Test:bloodknight", "PrefabsHeroes/Bloodknight");
        } else {
			return null;
		}
	}

	public GameObject SetTemplateSpellCardAttributes (string id) {
		if (id == "armor") {
			return SetTemplateSpellCardAttributesConstructor("ArmorCard", "armor", "spell", 2, "Test:armor", "Particles/DodgeParticle");
		} else if (id == "rockthrow") {
			return SetTemplateSpellCardAttributesConstructor("RockthrowCard", "rockthrow", "spell", 2, "Test:rock throw", "Particles/RockThrowParticle");
		} else if (id == "root") {
			return SetTemplateSpellCardAttributesConstructor("RootCard", "root", "spell", 3, "Test:root", "Particles/DodgeParticle");
		} else if (id == "windgust") {
			return SetTemplateSpellCardAttributesConstructor("WindGustCard", "windgust", "spell", 3, "Test:wind gust", "Particles/WindGustParticle");
		} else if (id == "heal") {
			return SetTemplateSpellCardAttributesConstructor("HealCard", "heal", "spell", 3, "Test:heal", "Particles/HealLightParticle");
		}  else if (id == "shroud") {
			return SetTemplateSpellCardAttributesConstructor("ShroudCard", "shroud", "spell", 3, "Test:shroud", "Particles/DodgeParticle");
		}  else if (id == "might") {
			return SetTemplateSpellCardAttributesConstructor("MightCard", "might", "spell", 3, "Test:might", "Particles/DodgeParticle");
		}  else if (id == "fireball") {
			return SetTemplateSpellCardAttributesConstructor("FireballCard", "fireball", "spell", 4, "Test:fireball", "Particles/FireballParticle");
		} else {
			return null;
		}
	}

	private GameObject SetTemplateHeroCardAttributesConstructor (string name, string cardId, string type, int manaCost, string cardName, string heroPrefab) {
		templateHeroCard.GetComponent<Card>().name = name;
		templateHeroCard.GetComponent<Card>().cardId = cardId;
		templateHeroCard.GetComponent<Card>().type = type;
		templateHeroCard.GetComponent<Card>().manaCost = manaCost;
		templateHeroCard.GetComponent<Card>().cardName = cardName;
		templateHeroCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>(heroPrefab);
		return templateHeroCard;
	}

	private GameObject SetTemplateSpellCardAttributesConstructor (string name, string cardId, string type, int manaCost, string cardName, string spellParticle) {
		templateSpellCard.GetComponent<Card>().name = name;
		templateSpellCard.GetComponent<Card>().cardId = cardId;
		templateSpellCard.GetComponent<Card>().type = type;
		templateSpellCard.GetComponent<Card>().manaCost = manaCost;
		templateSpellCard.GetComponent<Card>().cardName = cardName;
		templateSpellCard.GetComponent<Card>().spellParticle = Resources.Load<GameObject>(spellParticle);
		return templateSpellCard;
	}

	public void SetTemplateHeroCardImage (string id, GameObject card) {
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
		//Full list of all player hero cards for pass and play
		fullPlayerHeroCardList.Add("wall");
		fullPlayerHeroCardList.Add("archer");
		fullPlayerHeroCardList.Add("footsoldier");
		fullPlayerHeroCardList.Add("dwarf");
		fullPlayerHeroCardList.Add("ghost");
		fullPlayerHeroCardList.Add("sapper");
		fullPlayerHeroCardList.Add("rogue");
		fullPlayerHeroCardList.Add("druid");
		fullPlayerHeroCardList.Add("monk");
		fullPlayerHeroCardList.Add("slinger");
		fullPlayerHeroCardList.Add("bloodmage");
		fullPlayerHeroCardList.Add("blacksmith");
		fullPlayerHeroCardList.Add("wolf");
		fullPlayerHeroCardList.Add("chaosmage");
		fullPlayerHeroCardList.Add("sorcerer");
		fullPlayerHeroCardList.Add("assassin");
		fullPlayerHeroCardList.Add("paladin");
		fullPlayerHeroCardList.Add("tower");
		fullPlayerHeroCardList.Add("knight");
		fullPlayerHeroCardList.Add("champion");
		fullPlayerHeroCardList.Add("cavalry");
		fullPlayerHeroCardList.Add("diviner");
        fullPlayerHeroCardList.Add("crossbowman");
        fullPlayerHeroCardList.Add("bloodknight");

        //Full list of all player spell cards for pass and play
        fullPlayerSpellCardList.Add("armor");
		fullPlayerSpellCardList.Add("rockthrow");
		fullPlayerSpellCardList.Add("root");
		fullPlayerSpellCardList.Add("windgust");
		fullPlayerSpellCardList.Add("heal");
		fullPlayerSpellCardList.Add("shroud");
		fullPlayerSpellCardList.Add("might");
		fullPlayerSpellCardList.Add("fireball");

		//boss01 player cards
		boss01PlayerHeroCards.Add("archer");
		boss01PlayerHeroCards.Add("footsoldier");
		boss01PlayerHeroCards.Add("rogue");
		boss01PlayerHeroCards.Add("druid");
		boss01PlayerHeroCards.Add("sapper");
		boss01PlayerSpellCards.Add("heal");
		boss01PlayerSpellCards.Add("windgust");
		boss01PlayerSpellCards.Add("fireball");

		//boss02 player cards
		boss02PlayerHeroCards.Add("archer");
		boss02PlayerHeroCards.Add("footsoldier");
		boss02PlayerHeroCards.Add("footsoldier");
		boss02PlayerHeroCards.Add("footsoldier");

		//boss03 player cards
		boss03PlayerHeroCards.Add("archer");
		boss03PlayerHeroCards.Add("footsoldier");

		//boss04 player cards
		boss04PlayerHeroCards.Add("archer");
		boss04PlayerHeroCards.Add("footsoldier");

		//boss05 player cards
		boss05PlayerHeroCards.Add("archer");
		boss05PlayerHeroCards.Add("footsoldier");
	}

	void InstantiatePlayerCards () {
		if (currentlyActiveStory == "boss01") {
			foreach (string cardId in boss01PlayerHeroCards) {
				SetTemplateHeroCardAttributes(cardId);
				GameObject newCard = Instantiate (templateHeroCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
			foreach (string cardId in boss01PlayerSpellCards) {
				SetTemplateSpellCardAttributes(cardId);
				GameObject newCard = Instantiate (templateSpellCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("SpellSelectionScrollList/Viewport/Content/Card Container").transform, false);
				//SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss02") {
			foreach (string cardId in boss02PlayerHeroCards) {
				SetTemplateHeroCardAttributes(cardId);
				GameObject newCard = Instantiate (templateHeroCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
			foreach (string cardId in boss02PlayerSpellCards) {
				SetTemplateSpellCardAttributes(cardId);
				GameObject newCard = Instantiate (templateSpellCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("SpellSelectionScrollList/Viewport/Content/Card Container").transform, false);
				//SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss03") {
			foreach (string cardId in boss03PlayerHeroCards) {
				SetTemplateHeroCardAttributes(cardId);
				GameObject newCard = Instantiate (templateHeroCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
			foreach (string cardId in boss03PlayerSpellCards) {
				SetTemplateSpellCardAttributes(cardId);
				GameObject newCard = Instantiate (templateSpellCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("SpellSelectionScrollList/Viewport/Content/Card Container").transform, false);
				//SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss04") {
			foreach (string cardId in boss04PlayerHeroCards) {
				SetTemplateHeroCardAttributes(cardId);
				GameObject newCard = Instantiate (templateHeroCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
			foreach (string cardId in boss04PlayerSpellCards) {
				SetTemplateSpellCardAttributes(cardId);
				GameObject newCard = Instantiate (templateSpellCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("SpellSelectionScrollList/Viewport/Content/Card Container").transform, false);
				//SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else if (currentlyActiveStory == "boss05") {
			foreach (string cardId in boss05PlayerHeroCards) {
				SetTemplateHeroCardAttributes(cardId);
				GameObject newCard = Instantiate (templateHeroCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
			foreach (string cardId in boss05PlayerSpellCards) {
				SetTemplateSpellCardAttributes(cardId);
				GameObject newCard = Instantiate (templateSpellCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("SpellSelectionScrollList/Viewport/Content/Card Container").transform, false);
				//SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		} else {
			foreach (string cardId in fullPlayerHeroCardList) {
				SetTemplateHeroCardAttributes(cardId);
				GameObject newCard = Instantiate (templateHeroCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
				SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
			foreach (string cardId in fullPlayerSpellCardList) {
				SetTemplateSpellCardAttributes(cardId);
				GameObject newCard = Instantiate (templateSpellCard) as GameObject;
				newCard.transform.SetParent (GameObject.Find ("SpellSelectionScrollList/Viewport/Content/Card Container").transform, false);
				//SetTemplateCardImage(newCard.GetComponent<Card>().cardId, newCard);
			}
		}
	}
}
//https://www.youtube.com/watch?v=D9_Z4wb7940&ab_channel=MichelKlaasen
