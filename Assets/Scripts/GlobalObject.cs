using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GlobalObject : MonoBehaviour {

	public static GlobalObject instance;
	public static bool aiEnabled = false;
	public static bool storyEnabled = false;
	public static string currentlyActiveStory;
	public List<string> player2DeckSelect;
	public GameObject templateHeroCard;
	public GameObject templateSpellCard;
    public GameObject templateCommander;
    public GameObject templateHeroUnit;
    //UI objects
    public BattleUIManager battleUIManagerScript;

	public string player1Class, player2Class;
    //Flag for whether battles should try to use commanders to make testing easier
    public bool useCommanders = true;
    //Using hard coded value for now to quickly populate commanders for each encounter. Ideally we will be able to later read these in from a file
    public int numEncounters = 8;

    //List for commanders used for Selection UI purposes or any time all commanders are needed to be loaded
    public List<GameObject> commanderList;
    public List<Commander> loadedCommanders;
    public List<Commander> battleCommanders;

    //Player and npc selected commander
    public CommanderData humanPlayerCommanderData;
    public CommanderData enemyCommanderData;

    void Awake () {
		Debug.Log("RUNNING AWAKE FUNCTION OF GLOBALOBJECT.CS");
		if (GlobalObject.instance == null) {
			GlobalObject.instance = this;
			DontDestroyOnLoad(this.gameObject);
            //Register for OnSceneLoaded event
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        } else if (instance != this) {
            Debug.LogError("DUPLICATE GLOBAL OBJECT FOUND! DELETING THIS ONE");
			Destroy(this.gameObject);
		}

        //Build list of commander scripts from prefabs
        foreach( GameObject commanderPrefab in this.commanderList) {
            Commander commander = commanderPrefab.GetComponent<Commander>();
            this.loadedCommanders.Add(commander);
        }

        if (SceneManager.GetActiveScene().name != "BossSelect"
            && SceneManager.GetActiveScene().name != "Game"
            && SceneManager.GetActiveScene().name != "GameCommanders"
            && SceneManager.GetActiveScene().name != "CommanderSelect"
            && SceneManager.GetActiveScene().name != "Map"
            && SceneManager.GetActiveScene().buildIndex != GameConstants.SCENE_INDEX_POST_BATTLE_CARD_SELECT) {
            Debug.LogWarning("WARNING! Scene Manger is Loading a non specified scene which it assumes is the card select!!!");
        }
    }

	void Start () {
		
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("Global object: Scene loaded! => name:" + scene.name + " index: " + scene.buildIndex.ToString() );
        Debug.Log(mode.ToString());
        if( scene.buildIndex == GameConstants.SCENE_INDEX_COMMANDER_SELECT) {
            GlobalObject.instance.LoadCommanderSelectUI();
        }else if (SceneManager.GetActiveScene().buildIndex == GameConstants.SCENE_INDEX_GAME_COMMANDERS) {

            Debug.Log("GLOBAL OBJECT TRYING TO LOAD COMMANDER GAME SCENE!");
            //Try to find UI manager object for loading commander images
            GameObject battleUIManagerObj = GameObject.Find("LevelCanvas");
            GlobalObject.instance.battleUIManagerScript = battleUIManagerObj.GetComponent<BattleUIManager>();

            this.battleCommanders = new List<Commander>();

            //Trigger UI object to load commander images
            this.battleCommanders = GlobalObject.instance.battleUIManagerScript.SetSelectedCommanderBattleImages();
            
            //Set data attributes of the commanders
            CommanderData playerCommanderData = GlobalObject.instance.humanPlayerCommanderData;
            CommanderData enemyCommanderData = GlobalObject.instance.enemyCommanderData;
            //Set mana per turn in the playfield from the commanders
            PlayField.instance.manaPerTurn = playerCommanderData.ManaPerTurn;
            PlayField.instance.manaPerTurnAi = enemyCommanderData.ManaPerTurn;
            Debug.LogWarning("GlobalObject: setting mana per turn: " + PlayField.instance.manaPerTurn.ToString() + " and mana per turn AI is: " + PlayField.instance.manaPerTurnAi.ToString() );
            //Iterate through list of commanders thrown back by the UI manager that set images
            for (int i = 0; i < battleCommanders.Count; i++) {
                Commander currCommander = battleCommanders[i];
                Debug.LogWarning("Global object! commanders index: " + i.ToString() + " and player id is:" + currCommander.playerId.ToString());
                //TODO: need unique conten ids or something better to match on here
                if (currCommander == null) {
                    Debug.LogWarning("CURR COMMANDER IS NULL!!!");
                }
                //Set player ids for the enemy commmander. We know it's not the player's if the player id is UNSET
                if(currCommander.playerId == GameConstants.UNSET_PLAYER_ID) {
                    currCommander.playerId = GameConstants.ENEMY_PLAYER_ID;
                    Debug.LogWarning("CURR COMMANDER HAS UNSET ID! SETTING TO ENEMY ID!");
                }
                //TODO: Fix bug here - matching off of commander name 
                //if (currCommander.commanderData.CharName == playerCommanderData.CharName) { 
                //    currCommander.playerId = GameConstants.HUMAN_PLAYER_ID;
                //} else if (currCommander.commanderData.CharName == enemyCommanderData.CharName) {
                //    currCommander.playerId = GameConstants.ENEMY_PLAYER_ID;
                //}
                //Set the rest of attributes
                currCommander.SetCommanderAttributes(currCommander.commanderData);
                //Set necessary UI references - not 100% sure why this didn't work when attempting to do so from within the BattleUIManager's SetSelectedCommanderBattleImages but for some reason results in a null ref if not set here
                GlobalObject.instance.battleUIManagerScript.SetCommanderUIPanel(ref currCommander);
                //Initialize some UI data to store variables and then set the display text for current charge and total cost
                currCommander.commanderUIPanel.Initialize(currCommander.playerId);
                currCommander.commanderUIPanel.SetCommanderResourceText(0, currCommander.abilityChargeCost);
                //Fire off notification for Commanders that the battle has started
                currCommander.OnBattleStart();
            }

            //TODO: Need to change the logic for this at some point
        }
    }

    void LoadCommanderSelectUI() {
        //Load commanders
        foreach (Commander c in GlobalObject.instance.loadedCommanders) {
            CommanderData cData = c.commanderData;
            /*Commented this out as deprecated*/
            //Rename object so instance name matches current commander
            //this.templateCommander.GetComponent<Commander>().name = cData.CharName+"Commander";


            GameObject commanderPrefab = Resources.Load<GameObject>(c.selectedCommanderPrefabPath);
            Debug.Log("GLOBAL OBJECT cData.PrefabPath is: " + c.selectedCommanderPrefabPath);
            //Instantiate empty template commander UI element
            GameObject listItem = GameObject.Instantiate(this.templateCommander) as GameObject;
            
            //commanderChildListItem.SetCommanderAttributes(cData.CharName, commanderPrefab, cData.PrefabPath, cData.MaxHP, cData.StartingHandSize, cData, GameConstants.HUMAN_PLAYER_ID, cData.AbilityChargeCost, cData.AbilityTargetType);
            //Add list item to scrollview UI
            listItem.transform.SetParent(GameObject.Find("CommanderSelectUIManager/CommanderSelectionScrollList/Viewport/Content/CommanderContainer").transform, false);
            //Create prefab to use as image on list item
            GameObject commanderGameObject = GameObject.Instantiate(commanderPrefab) as GameObject;
            commanderGameObject.transform.SetParent(listItem.transform.Find("Image").transform, false);

            //populate data for commander
            Commander commanderListItem = listItem.GetComponentInChildren<Commander>();//listItem.GetComponent<Commander>();

            //Set the value for all of these UI commanders player ID to be human player because any selected commander is used by the player
            commanderListItem.SetCommanderAttributes(
                cData.CharName, 
                commanderPrefab, 
                c.selectedCommanderPrefabPath,
                cData.MaxHP,
                cData.StartingHandSize,
                cData,
                GameConstants.HUMAN_PLAYER_ID,
                cData.AbilityChargeCost,
                cData.AbilityTargetType,
                cData.ManaPerTurn
                );
            //Set display name
            var nameTransform = listItem.transform.Find("header/Name");
            if( nameTransform && nameTransform != null) {
                nameTransform.GetComponent<Text>().text = cData.CharName;
            }
        }
    }

    //TODO: Delete this safely!
    /*
    public void AssignEnemyCommander() {
        switch (currentlyActiveStory) {
            case ("boss01"):
                this.enemyCommanderData = this.enemyEncounterCommanders[0];
                break;
            case ("boss02"):
                this.enemyCommanderData = this.enemyEncounterCommanders[1];
                break;
            case ("boss03"):
                this.enemyCommanderData = this.enemyEncounterCommanders[2];
                break;
            case ("boss04"):
                this.enemyCommanderData = this.enemyEncounterCommanders[3];
                break;
            case ("boss05"):
                this.enemyCommanderData = this.enemyEncounterCommanders[4];
                break;
            case ("story01"):
                this.enemyCommanderData = this.enemyEncounterCommanders[5];
                break;
            case ("story02"):
                this.enemyCommanderData = this.enemyEncounterCommanders[6];
                break;
            case ("story03"):
                this.enemyCommanderData = this.enemyEncounterCommanders[7];
                break;
            case ("bossA1"):
                this.enemyCommanderData = this.enemyEncounterCommanders[7];
                break;
            default:
                this.enemyCommanderData = this.enemyEncounterCommanders[0];
                break;
        }
    }
    */

    public GameObject SetTemplateHeroCardAttributes (string id) {
        if (id == "wall") {
            return SetTemplateHeroCardAttributesConstructor("WallCard", GameConstants.Card.wall, "heroStationary", 2, "Test: Wall", "PrefabsHeroes/Wall", 0, 4, 0, 0);
        } else if (id == "archer") {
            return SetTemplateHeroCardAttributesConstructor("ArcherCard", GameConstants.Card.archer, "hero", 2, "Test: Archer", "PrefabsHeroes/Archer", 2, 2, 2, 3);
        } else if (id == "footsoldier") {
            return SetTemplateHeroCardAttributesConstructor("FootSoldierCard", GameConstants.Card.footsoldier, "hero", 2, "Test:Foot Soldier", "PrefabsHeroes/FootSoldier", 2, 3, 3, 1);
        } else if (id == "dwarf") {
            return SetTemplateHeroCardAttributesConstructor("DwarfCard", GameConstants.Card.dwarf, "hero", 3, "Test:Dwarf", "PrefabsHeroes/Dwarf", 3, 1, 5, 2);
        } else if (id == "ghost") {
            return SetTemplateHeroCardAttributesConstructor("GhostCard", GameConstants.Card.ghost, "hero", 3, "Test:Ghost", "PrefabsHeroes/Ghost", 1, 3, 2, 1);
        } else if (id == "sapper") {
            return SetTemplateHeroCardAttributesConstructor("SapperCard", GameConstants.Card.sapper, "hero", 2, "Test:Sapper", "PrefabsHeroes/Sapper", 2, 2, 2, 1);
        } else if (id == "rogue") {
            return SetTemplateHeroCardAttributesConstructor("RogueCard", GameConstants.Card.rogue, "hero", 3, "Test:Rogue", "PrefabsHeroes/Rogue", 2, 4, 2, 1);
        } else if (id == "druid") {
            return SetTemplateHeroCardAttributesConstructor("DruidCard", GameConstants.Card.druid, "hero", 3, "Test:Druid", "PrefabsHeroes/Druid", 1, 4, 2, 2);
        } else if (id == "monk") {
            return SetTemplateHeroCardAttributesConstructor("MonkCard", GameConstants.Card.monk, "hero", 4, "Test:monk", "PrefabsHeroes/Monk", 2, 5, 2, 1);
        } else if (id == "slinger") {
            return SetTemplateHeroCardAttributesConstructor("SlingerCard", GameConstants.Card.slinger, "hero", 4, "Test:slinger", "PrefabsHeroes/Slinger", 2, 4, 2, 2);
        } else if (id == "bloodmage") {
            return SetTemplateHeroCardAttributesConstructor("BloodmageCard", GameConstants.Card.bloodmage, "hero", 4, "Test:bloodmage", "PrefabsHeroes/Bloodmage", 2, 4, 2, 2);
        } else if (id == "blacksmith") {
            return SetTemplateHeroCardAttributesConstructor("BlacksmithCard", GameConstants.Card.blacksmith, "hero", 4, "Test:blacksmith", "PrefabsHeroes/Blacksmith", 1, 5, 2, 1);
        } else if (id == "wolf") {
            return SetTemplateHeroCardAttributesConstructor("WolfCard", GameConstants.Card.wolf, "hero", 4, "Test:wolf", "PrefabsHeroes/Wolf", 2, 3, 2, 1);
        } else if (id == "chaosmage") {
            return SetTemplateHeroCardAttributesConstructor("ChaosMageCard", GameConstants.Card.chaosmage, "hero", 4, "Test:chaosmage", "PrefabsHeroes/ChaosMage", 1, 4, 1, 10);
        } else if (id == "sorcerer") {
            return SetTemplateHeroCardAttributesConstructor("SorcererCard", GameConstants.Card.sorcerer, "hero", 4, "Test:sorcerer", "PrefabsHeroes/Sorcerer", 1, 5, 1, 3);
        } else if (id == "assassin") {
            return SetTemplateHeroCardAttributesConstructor("AssassinCard", GameConstants.Card.assassin, "hero", 4, "Test:assassin", "PrefabsHeroes/Assassin", 2, 5, 2, 1);
        } else if (id == "knight") {
            return SetTemplateHeroCardAttributesConstructor("KnightCard", GameConstants.Card.knight, "hero", 4, "Test:knight", "PrefabsHeroes/Knight", 3, 7, 1, 1);
        } else if (id == "paladin") {
            return SetTemplateHeroCardAttributesConstructor("PaladinCard", GameConstants.Card.paladin, "hero", 5, "Test:paladin", "PrefabsHeroes/Paladin", 2, 5, 1, 1);
        } else if (id == "tower") {
            return SetTemplateHeroCardAttributesConstructor("TowerCard", GameConstants.Card.tower, "heroStationary", 5, "Test:tower", "PrefabsHeroes/Tower", 1, 8, 0, 1);
        } else if (id == "champion") {
            return SetTemplateHeroCardAttributesConstructor("ChampionCard", GameConstants.Card.champion, "hero", 5, "Test:champion", "PrefabsHeroes/Champion", 2, 5, 1, 1);
        } else if (id == "cavalry") {
            return SetTemplateHeroCardAttributesConstructor("CavalryCard", GameConstants.Card.cavalry, "hero", 5, "Test:cavalry", "PrefabsHeroes/Cavalry", 2, 7, 3, 1);
        } else if (id == "diviner") {
            return SetTemplateHeroCardAttributesConstructor("DivinerCard", GameConstants.Card.diviner, "hero", 5, "Test:diviner", "PrefabsHeroes/Diviner", 1, 5, 1, 3);
        } else if (id == "crossbowman") {
            return SetTemplateHeroCardAttributesConstructor("CrossbowmanCard", GameConstants.Card.crossbowman, "hero", 3, "Test:crossbowman", "PrefabsHeroes/Crossbowman", 2, 3, 2, 3);
        } else if (id == "bloodknight") {
            return SetTemplateHeroCardAttributesConstructor("BloodknightCard", GameConstants.Card.bloodknight, "hero", 5, "Test:bloodknight", "PrefabsHeroes/Bloodknight", 2, 6, 1, 1);
        } else if (id == "cultinitiate") {
            return SetTemplateHeroCardAttributesConstructor("CultInitiateCard", GameConstants.Card.cultinitiate, "hero", 2, "Test:cultinitiate", "PrefabsHeroes/CultInitiate", 2, 4, 3, 1);
        } else if (id == "cultadept") {
            return SetTemplateHeroCardAttributesConstructor("CultAdeptCard", GameConstants.Card.cultadept, "hero", 2, "Test:cultadept", "PrefabsHeroes/CultAdept", 2, 3, 2, 3);
        } else if (id == "cultacolyte") {
            return SetTemplateHeroCardAttributesConstructor("CultAcolyteCard", GameConstants.Card.cultacolyte, "hero", 3, "Test:cultacolyte", "PrefabsHeroes/CultAcolyte", 2, 3, 2, 3);
        } else if (id == "cultsentinel") {
            return SetTemplateHeroCardAttributesConstructor("CultSentinelCard", GameConstants.Card.cultsentinel, "hero", 3, "Test:cultsentinel", "PrefabsHeroes/CultSentinel", 4, 10, 1, 1);
        } else if (id == "cultfanatic") {
            return SetTemplateHeroCardAttributesConstructor("CultFanaticCard", GameConstants.Card.cultfanatic, "hero", 2, "Test:cultfanatic", "PrefabsHeroes/CultFanatic", 2, 2, 2, 1);
        } else {
			return null;
		}
	}

    public GameObject SetTemplateHeroUnitAttributes(string id) {
         if (id == "archer") {
            return SetTemplateHeroCardAttributesConstructor("ArcherCard", GameConstants.Card.archer, "hero", 2, "Test: Archer", "PrefabsHeroes/Archer", 2, 2, 2, 3);
        }
        return null;
    }

    public GameObject SetTemplateSpellCardAttributes (string id) {
		if (id == "armor") {
			return SetTemplateSpellCardAttributesConstructor("ArmorCard", GameConstants.Card.armor, "spell", 1, "Test:armor", "Particles/DodgeParticle", "Images/Icons/SpellIcons/ArmorIcon");
		} else if (id == "rockthrow") {
			return SetTemplateSpellCardAttributesConstructor("RockthrowCard", GameConstants.Card.rockthrow, "spell", 1, "Test:rock throw", "Particles/RockThrowParticle", "Images/Icons/SpellIcons/RockIcon");
		} else if (id == "root") {
			return SetTemplateSpellCardAttributesConstructor("RootCard", GameConstants.Card.root, "spell", 3, "Test:root", "Particles/DodgeParticle", "Images/Icons/SpellIcons/RootIcon");
		} else if (id == "windgust") {
			return SetTemplateSpellCardAttributesConstructor("WindGustCard", GameConstants.Card.windgust, "spell", 3, "Test:wind gust", "Particles/WindGustParticle", "Images/Icons/SpellIcons/WindgustIcon");
		} else if (id == "heal") {
			return SetTemplateSpellCardAttributesConstructor("HealCard", GameConstants.Card.heal, "spell", 2, "Test:heal", "Particles/HealLightParticle", "Images/Icons/SpellIcons/HealIcon");
		} else if (id == "shroud") {
			return SetTemplateSpellCardAttributesConstructor("ShroudCard", GameConstants.Card.shroud, "spell", 2, "Test:shroud", "Particles/DodgeParticle", "Images/Icons/SpellIcons/ShroudIcon");
		} else if (id == "might") {
			return SetTemplateSpellCardAttributesConstructor("MightCard", GameConstants.Card.might, "spell", 2, "Test:might", "Particles/DodgeParticle", "Images/Icons/SpellIcons/MightIcon");
		} else if (id == "fireball") {
			return SetTemplateSpellCardAttributesConstructor("FireballCard", GameConstants.Card.fireball, "spell", 2, "Test:fireball", "Particles/FireballParticle", "Images/Icons/SpellIcons/FireballIcon");
        } else if (id == "drainlife") {
            return SetTemplateSpellCardAttributesConstructor("DrainLifeCard", GameConstants.Card.drainlife, "spell", 1, "Test:drainlife", "Particles/DrainLifeParticle", "Images/Icons/SpellIcons/DrainLifeIcon");
        } else {
			return null;
		}
	}

	private GameObject SetTemplateHeroCardAttributesConstructor (string name, GameConstants.Card cardId, string type, int manaCost, string cardName, string heroPrefab, int power, int maxHealth, int speed, int range) {
		templateHeroCard.GetComponent<Card>().name = name;
		templateHeroCard.GetComponent<Card>().cardId = cardId;
		templateHeroCard.GetComponent<Card>().type = type;
		templateHeroCard.GetComponent<Card>().manaCost = manaCost;
		templateHeroCard.GetComponent<Card>().cardName = cardName;
		templateHeroCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>(heroPrefab);
        templateHeroCard.GetComponent<Card>().power = power;
        templateHeroCard.GetComponent<Card>().maxHealth = maxHealth;
        templateHeroCard.GetComponent<Card>().speed = speed;
        templateHeroCard.GetComponent<Card>().range = range;
        return templateHeroCard;
	}

    public GameObject SetTemplateHeroUnitAttributesConstructor() {
        templateHeroUnit.GetComponent<Hero>().id = Card.selectedCard.GetComponent<Card>().name;
        templateHeroUnit.GetComponent<Hero>().maxHealth = Card.selectedCard.GetComponent<Card>().maxHealth;
        templateHeroUnit.GetComponent<Hero>().currentHealth = Card.selectedCard.GetComponent<Card>().maxHealth;
        templateHeroUnit.GetComponent<Hero>().power = Card.selectedCard.GetComponent<Card>().power;
        templateHeroUnit.GetComponent<Hero>().speed = Card.selectedCard.GetComponent<Card>().speed;
        templateHeroUnit.GetComponent<Hero>().range = Card.selectedCard.GetComponent<Card>().range;
        templateHeroUnit.GetComponent<Hero>().power = Card.selectedCard.GetComponent<Card>().power;
        templateHeroUnit.GetComponent<Hero>().heroPrefab = Card.selectedCard.GetComponent<Card>().heroPrefab;
        templateHeroUnit.GetComponent<Hero>().currentArmor = 0;
        return templateHeroUnit;
    }

    private GameObject SetTemplateSpellCardAttributesConstructor (string name, GameConstants.Card cardId, string type, int manaCost, string cardName, string spellParticle, string spellIcon) {
		templateSpellCard.GetComponent<Card>().name = name;
		templateSpellCard.GetComponent<Card>().cardId = cardId;
		templateSpellCard.GetComponent<Card>().type = type;
		templateSpellCard.GetComponent<Card>().manaCost = manaCost;
		templateSpellCard.GetComponent<Card>().cardName = cardName;
		templateSpellCard.GetComponent<Card>().spellParticle = Resources.Load<GameObject>(spellParticle);
        templateSpellCard.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(spellIcon);
        return templateSpellCard;
	}

    public void SetHeroUnitPrefab(GameObject spawnedUnit, GameObject heroPrefab) {
        GameObject newHeroPrefab = Instantiate(heroPrefab) as GameObject;
        newHeroPrefab.transform.SetParent(spawnedUnit.transform.Find("HeroPrefab").transform, false);
    }

    public void SetTemplateHeroCardImage (string id, GameObject card) {
        //Not sure what the point of this method is currently. It seems the intent is to set the image for a newly spawned hero, but we are actually spawning a whole hero prefab instead of just setting an image.
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
        } else if (id == "knight") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/Knight")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        } else if (id == "cultinitiate") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/CultInitiate")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        } else if (id == "cultadept") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/CultAdept")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        } else if (id == "cultacolyte") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/CultAcolyte")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        } else if (id == "cultsentinel") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/CultSentinel")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        }
    }

    /* Commander data code*/
    public void SetSelectedCommander( CommanderData cData) {
        this.humanPlayerCommanderData = cData;
    }

    public void LoadLevel( int levelIndex) {
        GameObject levelManagerObject = GameObject.Find("LevelManager");
        levelManagerObject.GetComponent<LevelManager>().LoadLevel(levelIndex);
        //TODO - Figure out when we can remove the commented code below that was forcing the boss select scene to lead
        //levelManagerObject.GetComponent<LevelManager>().LoadLevel(GameConstants.SCENE_INDEX_BOSS_SELECT);
    }
}
//https://www.youtube.com/watch?v=D9_Z4wb7940&ab_channel=MichelKlaasen
