using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
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

    //List of all card entries which is utilized for card selection scenes by LevelManager => CardSelectManager
    public List<CardEntry> allCardEntries = new List<CardEntry>();

    //Dictionary for strings
    IDictionary<string, string> stringDictionary = new Dictionary<string, string>();

    void Awake() {
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
        foreach (GameObject commanderPrefab in this.commanderList) {
            Commander commander = commanderPrefab.GetComponent<Commander>();
            this.loadedCommanders.Add(commander);
        }

        //Build list of all cards for use with card selection manager or wherever else it is needed
        this.CreateCardEntryList();

        //Do something based on loaded scene
        if (SceneManager.GetActiveScene().name != "BossSelect"
            && SceneManager.GetActiveScene().name != "Game"
            && SceneManager.GetActiveScene().name != "GameCommanders"
            && SceneManager.GetActiveScene().name != "CommanderSelect"
            && SceneManager.GetActiveScene().name != "Map"
            && SceneManager.GetActiveScene().buildIndex != GameConstants.SCENE_INDEX_POST_BATTLE_CARD_SELECT) {
            Debug.LogWarning("WARNING! Scene Manger is Loading a non specified scene which it assumes is the card select!!!");
        }

        //Build the strings dictionary
        ReadWriteCSV.CsvFileReader fileReader = new ReadWriteCSV.CsvFileReader("Assets/Resources/Localization/localization-en.csv");
        List<string> stringList = fileReader.BuildStringsList();
        for (int i=0; i<stringList.Count; i++) {
            if (i % 2 == 0) {
                stringDictionary.Add(stringList[i], stringList[i + 1]);
            }
        }
    }

    void Start() {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("Global object: Scene loaded! => name:" + scene.name + " index: " + scene.buildIndex.ToString());
        Debug.Log(mode.ToString());
        if (scene.buildIndex == GameConstants.SCENE_INDEX_COMMANDER_SELECT) {
            GlobalObject.instance.LoadCommanderSelectUI();
        } else if (SceneManager.GetActiveScene().buildIndex == GameConstants.SCENE_INDEX_GAME_COMMANDERS) {

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
            Debug.LogWarning("GlobalObject: setting mana per turn: " + PlayField.instance.manaPerTurn.ToString() + " and mana per turn AI is: " + PlayField.instance.manaPerTurnAi.ToString());
            //Iterate through list of commanders thrown back by the UI manager that set images
            for (int i = 0; i < battleCommanders.Count; i++) {
                Commander currCommander = battleCommanders[i];
                Debug.LogWarning("Global object! commanders index: " + i.ToString() + " and player id is:" + currCommander.playerId.ToString());
                //TODO: need unique conten ids or something better to match on here
                if (currCommander == null) {
                    Debug.LogWarning("CURR COMMANDER IS NULL!!!");
                }
                //Set player ids for the enemy commmander. We know it's not the player's if the player id is UNSET
                if (currCommander.playerId == GameConstants.UNSET_PLAYER_ID) {
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
                Debug.Log("SUHHHHHHHH************* " + currCommander.commanderName);
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
            if (nameTransform && nameTransform != null) {
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

    public GameObject SetTemplateHeroCardAttributes(string id) {
        if (id == "wall") {
            return SetTemplateHeroCardAttributesConstructor("WallCard", GameConstants.Card.wall, "heroStationary", 2, "Test: Wall", "cardDesc", "PrefabsHeroes/Wall", 0, 4, 0, 0);
        } else if (id == "archer") {
            return SetTemplateHeroCardAttributesConstructor("ArcherCard", GameConstants.Card.archer, "hero", 2, "SOLDIER_ARCHER_NAME", "SOLDIER_ARCHER_DESC", "PrefabsHeroes/Archer", 2, 2, 2, 3);
        } else if (id == "footsoldier") {
            return SetTemplateHeroCardAttributesConstructor("FootSoldierCard", GameConstants.Card.footsoldier, "hero", 2, "SOLDIER_FOOTSOLDIER_NAME", "SOLDIER_FOOTSOLDIER_DESC", "PrefabsHeroes/FootSoldier", 2, 3, 3, 1);
        } else if (id == "dwarf") {
            return SetTemplateHeroCardAttributesConstructor("DwarfCard", GameConstants.Card.dwarf, "hero", 3, "Test:Dwarf", "cardDesc", "PrefabsHeroes/Dwarf", 3, 1, 5, 2);
        } else if (id == "ghost") {
            return SetTemplateHeroCardAttributesConstructor("GhostCard", GameConstants.Card.ghost, "hero", 3, "Test:Ghost", "cardDesc", "PrefabsHeroes/Ghost", 1, 3, 2, 1);
        } else if (id == "sapper") {
            return SetTemplateHeroCardAttributesConstructor("SapperCard", GameConstants.Card.sapper, "hero", 2, "Test:Sapper", "cardDesc", "PrefabsHeroes/Sapper", 2, 2, 2, 1);
        } else if (id == "rogue") {
            return SetTemplateHeroCardAttributesConstructor("RogueCard", GameConstants.Card.rogue, "hero", 3, "SOLDIER_ROGUE_NAME", "SOLDIER_ROGUE_DESC", "PrefabsHeroes/Rogue", 2, 4, 2, 1);
        } else if (id == "druid") {
            return SetTemplateHeroCardAttributesConstructor("DruidCard", GameConstants.Card.druid, "hero", 3, "Test:Druid", "cardDesc", "PrefabsHeroes/Druid", 1, 4, 2, 2);
        } else if (id == "monk") {
            return SetTemplateHeroCardAttributesConstructor("MonkCard", GameConstants.Card.monk, "hero", 4, "Test:monk", "cardDesc", "PrefabsHeroes/Monk", 2, 5, 2, 1);
        } else if (id == "slinger") {
            return SetTemplateHeroCardAttributesConstructor("SlingerCard", GameConstants.Card.slinger, "hero", 4, "Test:slinger", "cardDesc", "PrefabsHeroes/Slinger", 2, 4, 2, 2);
        } else if (id == "bloodmage") {
            return SetTemplateHeroCardAttributesConstructor("BloodmageCard", GameConstants.Card.bloodmage, "hero", 4, "Test:bloodmage", "cardDesc", "PrefabsHeroes/Bloodmage", 2, 4, 2, 2);
        } else if (id == "blacksmith") {
            return SetTemplateHeroCardAttributesConstructor("BlacksmithCard", GameConstants.Card.blacksmith, "hero", 4, "Test:blacksmith", "cardDesc", "PrefabsHeroes/Blacksmith", 1, 5, 2, 1);
        } else if (id == "wolf") {
            return SetTemplateHeroCardAttributesConstructor("WolfCard", GameConstants.Card.wolf, "hero", 4, "Test:wolf", "cardDesc", "PrefabsHeroes/Wolf", 2, 3, 2, 1);
        } else if (id == "chaosmage") {
            return SetTemplateHeroCardAttributesConstructor("ChaosMageCard", GameConstants.Card.chaosmage, "hero", 4, "Test:chaosmage", "cardDesc", "PrefabsHeroes/ChaosMage", 1, 4, 1, 10);
        } else if (id == "sorcerer") {
            return SetTemplateHeroCardAttributesConstructor("SorcererCard", GameConstants.Card.sorcerer, "hero", 4, "Test:sorcerer", "cardDesc", "PrefabsHeroes/Sorcerer", 1, 5, 1, 3);
        } else if (id == "assassin") {
            return SetTemplateHeroCardAttributesConstructor("AssassinCard", GameConstants.Card.assassin, "hero", 4, "Test:assassin", "cardDesc", "PrefabsHeroes/Assassin", 2, 5, 2, 1);
        } else if (id == "knight") {
            return SetTemplateHeroCardAttributesConstructor("KnightCard", GameConstants.Card.knight, "hero", 4, "SOLDIER_KNIGHT_NAME", "SOLDIER_KNIGHT_DESC", "PrefabsHeroes/Knight", 3, 7, 1, 1);
        } else if (id == "paladin") {
            return SetTemplateHeroCardAttributesConstructor("PaladinCard", GameConstants.Card.paladin, "hero", 5, "Test:paladin", "cardDesc", "PrefabsHeroes/Paladin", 2, 5, 1, 1);
        } else if (id == "tower") {
            return SetTemplateHeroCardAttributesConstructor("TowerCard", GameConstants.Card.tower, "heroStationary", 5, "Test:tower", "cardDesc", "PrefabsHeroes/Tower", 1, 8, 0, 1);
        } else if (id == "champion") {
            return SetTemplateHeroCardAttributesConstructor("ChampionCard", GameConstants.Card.champion, "hero", 4, "Test:champion", "cardDesc", "PrefabsHeroes/Champion", 2, 5, 1, 1);
        } else if (id == "cavalry") {
            return SetTemplateHeroCardAttributesConstructor("CavalryCard", GameConstants.Card.cavalry, "hero", 4, "SOLDIER_CAVALRY_NAME", "SOLDIER_CAVALRY_DESC", "PrefabsHeroes/Cavalry", 2, 6, 3, 1);
        } else if (id == "diviner") {
            return SetTemplateHeroCardAttributesConstructor("DivinerCard", GameConstants.Card.diviner, "hero", 5, "Test:diviner", "cardDesc", "PrefabsHeroes/Diviner", 1, 5, 1, 3);
        } else if (id == "crossbowman") {
            return SetTemplateHeroCardAttributesConstructor("CrossbowmanCard", GameConstants.Card.crossbowman, "hero", 3, "Test:crossbowman", "cardDesc", "PrefabsHeroes/Crossbowman", 3, 3, 2, 3);
        } else if (id == "bloodknight") {
            return SetTemplateHeroCardAttributesConstructor("BloodknightCard", GameConstants.Card.bloodknight, "hero", 5, "Test:bloodknight", "cardDesc", "PrefabsHeroes/Bloodknight", 2, 6, 1, 1);
        } else if (id == "cultinitiate") {
            return SetTemplateHeroCardAttributesConstructor("CultInitiateCard", GameConstants.Card.cultinitiate, "hero", 2, "Test:cultinitiate", "cardDesc", "PrefabsHeroes/CultInitiate", 2, 4, 3, 1);
        } else if (id == "cultadept") {
            return SetTemplateHeroCardAttributesConstructor("CultAdeptCard", GameConstants.Card.cultadept, "hero", 1, "Test:cultadept", "cardDesc", "PrefabsHeroes/CultAdept", 2, 2, 2, 3);
        } else if (id == "cultacolyte") {
            return SetTemplateHeroCardAttributesConstructor("CultAcolyteCard", GameConstants.Card.cultacolyte, "hero", 3, "Test:cultacolyte", "cardDesc", "PrefabsHeroes/CultAcolyte", 2, 3, 2, 3);
        } else if (id == "cultsentinel") {
            return SetTemplateHeroCardAttributesConstructor("CultSentinelCard", GameConstants.Card.cultsentinel, "hero", 3, "Test:cultsentinel", "cardDesc", "PrefabsHeroes/CultSentinel", 4, 10, 1, 1);
        } else if (id == "cultfanatic") {
            return SetTemplateHeroCardAttributesConstructor("CultFanaticCard", GameConstants.Card.cultfanatic, "hero", 1, "Test:cultfanatic", "cardDesc", "PrefabsHeroes/CultFanatic", 2, 2, 2, 1);
        } else if (id == "zealot") {
            return SetTemplateHeroCardAttributesConstructor("ZealotCard", GameConstants.Card.zealot, "hero", 2, "SOLDIER_ZEALOT_NAME", "SOLDIER_ZEALOT_DESC", "PrefabsHeroes/Zealot", 1, 3, 1, 1, true);
        }else if(id == "vesselOfLight") {
            return SetTemplateHeroCardAttributesConstructor("VesselOfLightCard", GameConstants.Card.vesselOfLight, "hero", 2, "SOLDIER_VESSELOFLIGHT_NAME", "SOLDIER_VESSELOFLIGHT_DESC", "PrefabsHeroes/VesselOfLight", 1, 3, 1, 2, true);
        } else {
            return null;
        }
    }

    public GameObject SetTemplateSpellCardAttributes(string id) {
        if (id == "armor") {
            return SetTemplateSpellCardAttributesConstructor("ArmorCard", GameConstants.Card.armor, "spell", 1, "Test:armor", "cardDesc", "Particles/DodgeParticle", "Images/Icons/SpellIcons/ArmorIcon");
        } else if (id == "rockthrow") {
            return SetTemplateSpellCardAttributesConstructor("RockthrowCard", GameConstants.Card.rockthrow, "spell", 1, "SPELL_ROCKTHROW_NAME", "SPELL_ROCKTHROW_DESC", "Particles/RockThrowParticle", "Images/Icons/SpellIcons/RockIcon");
        } else if (id == "root") {
            return SetTemplateSpellCardAttributesConstructor("RootCard", GameConstants.Card.root, "spell", 3, "Test:root", "cardDesc", "Particles/DodgeParticle", "Images/Icons/SpellIcons/RootIcon");
        } else if (id == "windgust") {
            return SetTemplateSpellCardAttributesConstructor("WindGustCard", GameConstants.Card.windgust, "spell", 3, "Test:wind gust", "cardDesc", "Particles/WindGustParticle", "Images/Icons/SpellIcons/WindgustIcon");
        } else if (id == "heal") {
            return SetTemplateSpellCardAttributesConstructor("HealCard", GameConstants.Card.heal, "spell", 2, "Test:heal", "cardDesc", "Particles/HealLightParticle", "Images/Icons/SpellIcons/HealIcon");
        } else if (id == "shroud") {
            return SetTemplateSpellCardAttributesConstructor("ShroudCard", GameConstants.Card.shroud, "spell", 2, "Test:shroud", "cardDesc", "Particles/DodgeParticle", "Images/Icons/SpellIcons/ShroudIcon");
        } else if (id == "might") {
            return SetTemplateSpellCardAttributesConstructor("MightCard", GameConstants.Card.might, "spell", 1, "SPELL_MIGHT_NAME", "SPELL_MIGHT_DESC", "Particles/DodgeParticle", "Images/Icons/SpellIcons/MightIcon");
        } else if (id == "fireball") {
            return SetTemplateSpellCardAttributesConstructor("FireballCard", GameConstants.Card.fireball, "spell", 2, "SPELL_FIREBALL_NAME", "SPELL_FIREBALL_DESC", "Particles/FireballParticle", "Images/Icons/SpellIcons/FireballIcon");
        } else if (id == "drainlife") {
            return SetTemplateSpellCardAttributesConstructor("DrainLifeCard", GameConstants.Card.drainlife, "spell", 1, "Test:drainlife", "cardDesc", "Particles/DrainLifeParticle", "Images/Icons/SpellIcons/DrainLifeIcon");
        } else if (id == "haste") {
            return SetTemplateSpellCardAttributesConstructor("HasteCard", GameConstants.Card.haste, "spell", 2, "Test:haste", "cardDesc", "Particles/HasteParticle", "Images/Icons/SpellIcons/HasteIcon");
        } else if (id == "blessing") {
            return SetTemplateSpellCardAttributesConstructor("BlessingCard", GameConstants.Card.blessing, "spell", 2, "Test:blessing", "cardDesc", "Particles/HealHeavyParticleGameObject", "Images/Icons/SpellIcons/BlessingIcon");
        } else {
            return null;
        }
    }

    private GameObject SetTemplateHeroCardAttributesConstructor(string name, GameConstants.Card cardId, string type, int manaCost, string cardName, string cardDesc, string heroPrefab, int power, int maxHealth, int speed, int range, bool loadFromPrefab = false) {
        templateHeroCard.GetComponent<Card>().name = name;
        templateHeroCard.GetComponent<Card>().cardId = cardId;
        templateHeroCard.GetComponent<Card>().type = type;
        templateHeroCard.GetComponent<Card>().manaCost = manaCost;
        templateHeroCard.GetComponent<Card>().cardName = AddStringFromDictionary(cardName);
        templateHeroCard.GetComponent<Card>().cardDescription = AddStringFromDictionary(cardDesc);
        templateHeroCard.GetComponent<Card>().heroPrefab = Resources.Load<GameObject>(heroPrefab);
        templateHeroCard.GetComponent<Card>().power = power;
        templateHeroCard.GetComponent<Card>().maxHealth = maxHealth;
        templateHeroCard.GetComponent<Card>().speed = speed;
        templateHeroCard.GetComponent<Card>().range = range;
        templateHeroCard.GetComponent<Card>().loadFromPrefab = loadFromPrefab;
        return templateHeroCard;
    }

    public GameObject SetTemplateHeroUnitAttributesConstructor() {
        templateHeroUnit.GetComponent<Hero>().id = Card.selectedCard.GetComponent<Card>().cardId.ToString();
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

    public GameObject SetSpawnedHeroUnitAttributesConstructor( int playerId, GameObject spawnedHero) {
        spawnedHero.GetComponent<Hero>().playerId = playerId;
        spawnedHero.GetComponent<Hero>().id = Card.selectedCard.GetComponent<Card>().cardId.ToString();
        spawnedHero.GetComponent<Hero>().maxHealth = Card.selectedCard.GetComponent<Card>().maxHealth;
        spawnedHero.GetComponent<Hero>().currentHealth = Card.selectedCard.GetComponent<Card>().maxHealth;
        spawnedHero.GetComponent<Hero>().power = Card.selectedCard.GetComponent<Card>().power;
        spawnedHero.GetComponent<Hero>().speed = Card.selectedCard.GetComponent<Card>().speed;
        spawnedHero.GetComponent<Hero>().range = Card.selectedCard.GetComponent<Card>().range;
        spawnedHero.GetComponent<Hero>().power = Card.selectedCard.GetComponent<Card>().power;
        spawnedHero.GetComponent<Hero>().heroPrefab = Card.selectedCard.GetComponent<Card>().heroPrefab;
        spawnedHero.GetComponent<Hero>().currentArmor = 0;
        return spawnedHero;
    }

    private GameObject SetTemplateSpellCardAttributesConstructor(string name, GameConstants.Card cardId, string type, int manaCost, string cardName, string cardDesc, string spellParticle, string spellIcon) {
        templateSpellCard.GetComponent<Card>().name = name;
        templateSpellCard.GetComponent<Card>().cardName = AddStringFromDictionary(cardName);
        templateSpellCard.GetComponent<Card>().cardDescription = AddStringFromDictionary(cardDesc);
        templateSpellCard.GetComponent<Card>().cardId = cardId;
        templateSpellCard.GetComponent<Card>().type = type;
        templateSpellCard.GetComponent<Card>().manaCost = manaCost;
        templateSpellCard.GetComponent<Card>().spellParticle = Resources.Load<GameObject>(spellParticle);
        templateSpellCard.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(spellIcon);
        return templateSpellCard;
    }

    public void SetHeroUnitPrefab(GameObject spawnedUnit, GameObject heroPrefab) {
        GameObject newHeroPrefab = Instantiate(heroPrefab) as GameObject;
        newHeroPrefab.transform.SetParent(spawnedUnit.transform.Find("HeroPrefab").transform, false);
    }

    public void SetTemplateHeroCardImage(string id, GameObject card) {
        //Not sure what the point of this method is currently. It seems the intent is to set the image for a newly spawned hero, but we are actually spawning a whole hero prefab instead of just setting an image.
        if (id == "archer") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/Archer")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        } else if (id == "footsoldier") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/FootSoldier")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        } else if (id == "rogue") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/Rogue")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
        } else if (id == "druid") {
            GameObject newHero = Instantiate(Resources.Load<GameObject>("PrefabsHeroes/Druid")) as GameObject;
            newHero.transform.SetParent(card.transform.Find("Image").transform, false);
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

    /// <summary>
    /// Creates list of all card entries to hold in data for use for card selection, etc.
    /// </summary>
    public void CreateCardEntryList(){
        //TODO: Should these entries be made into prefabs and loaded in, instead of a hard coded list? how can we make this more flexible?

        //Cards shared for all
        CardEntry footsoldier = new CardEntry(GameConstants.Card.footsoldier, 1, GameConstants.CardCommanderType.All);
        allCardEntries.Add(footsoldier);
        CardEntry archer = new CardEntry(GameConstants.Card.archer, 1, GameConstants.CardCommanderType.All);
        allCardEntries.Add(archer);
        CardEntry rogue = new CardEntry(GameConstants.Card.rogue, 1, GameConstants.CardCommanderType.All);
        allCardEntries.Add(rogue);
        CardEntry cavalry = new CardEntry(GameConstants.Card.cavalry, 1, GameConstants.CardCommanderType.All);
        allCardEntries.Add(cavalry);

        /*Cardinal exclusive cards*/
        CardEntry druid = new CardEntry(GameConstants.Card.druid, 1, GameConstants.CardCommanderType.Cardinal);
        allCardEntries.Add(druid);
        CardEntry monk = new CardEntry(GameConstants.Card.monk, 1, GameConstants.CardCommanderType.Cardinal);
        allCardEntries.Add(monk);
        CardEntry slinger = new CardEntry(GameConstants.Card.slinger, 1, GameConstants.CardCommanderType.Cardinal);
        allCardEntries.Add(slinger);
        //Cardinal spells
        CardEntry heal = new CardEntry(GameConstants.Card.heal, 1, GameConstants.CardCommanderType.Cardinal);
        allCardEntries.Add(heal);
        CardEntry might = new CardEntry(GameConstants.Card.might, 1, GameConstants.CardCommanderType.Cardinal);
        allCardEntries.Add(might);

        /*Crusader exclusive cards*/
        CardEntry crossbowman = new CardEntry(GameConstants.Card.crossbowman, 1, GameConstants.CardCommanderType.Crusader);
        allCardEntries.Add(crossbowman);
        CardEntry knight = new CardEntry(GameConstants.Card.knight, 1, GameConstants.CardCommanderType.Crusader);
        allCardEntries.Add(knight);
        CardEntry blacksmith = new CardEntry(GameConstants.Card.blacksmith, 1, GameConstants.CardCommanderType.Crusader);
        allCardEntries.Add(blacksmith);
        CardEntry champion = new CardEntry(GameConstants.Card.champion, 1, GameConstants.CardCommanderType.Crusader);
        allCardEntries.Add(champion);
        //Crusader spells
        CardEntry blessing = new CardEntry(GameConstants.Card.blessing, 1, GameConstants.CardCommanderType.Crusader);
        allCardEntries.Add(blessing);

        /*Templar exclusive cards*/
        CardEntry zealot = new CardEntry(GameConstants.Card.zealot, 1, GameConstants.CardCommanderType.Templar);
        allCardEntries.Add(zealot);
        CardEntry vesselOfLight = new CardEntry(GameConstants.Card.vesselOfLight, 1, GameConstants.CardCommanderType.Templar);
        allCardEntries.Add(vesselOfLight);
        CardEntry paladin = new CardEntry(GameConstants.Card.paladin, 1, GameConstants.CardCommanderType.Templar);
        allCardEntries.Add(paladin);

        //Templar spells
        CardEntry armor = new CardEntry(GameConstants.Card.armor, 1, GameConstants.CardCommanderType.Templar);
        allCardEntries.Add(armor);
        CardEntry rockthrow = new CardEntry(GameConstants.Card.rockthrow, 1, GameConstants.CardCommanderType.Templar);
        allCardEntries.Add(rockthrow);
        CardEntry root = new CardEntry(GameConstants.Card.root, 1, GameConstants.CardCommanderType.Templar);
        allCardEntries.Add(root);
        CardEntry windgust = new CardEntry(GameConstants.Card.windgust, 1, GameConstants.CardCommanderType.Templar);
        allCardEntries.Add(windgust);

        /*Warlock exclusive cards*/
        CardEntry diviner = new CardEntry(GameConstants.Card.diviner, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(diviner);
        CardEntry dwarf = new CardEntry(GameConstants.Card.dwarf, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(dwarf);
        CardEntry wolf = new CardEntry(GameConstants.Card.wolf, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(wolf);
        CardEntry ghost = new CardEntry(GameConstants.Card.ghost, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(ghost);
        CardEntry sapper = new CardEntry(GameConstants.Card.sapper, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(sapper);
        CardEntry chaosmage = new CardEntry(GameConstants.Card.chaosmage, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(chaosmage);
        CardEntry sorcerer = new CardEntry(GameConstants.Card.sorcerer, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(sorcerer);
        CardEntry assassin = new CardEntry(GameConstants.Card.assassin, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(assassin);
        CardEntry wallEntry = new CardEntry(GameConstants.Card.wall, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(wallEntry);
        CardEntry tower = new CardEntry(GameConstants.Card.tower, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(tower);
        CardEntry bloodknight = new CardEntry(GameConstants.Card.bloodknight, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(bloodknight);
        CardEntry cultinitiate = new CardEntry(GameConstants.Card.cultinitiate, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(cultinitiate);
        CardEntry cultadept = new CardEntry(GameConstants.Card.cultadept, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(cultadept);
        CardEntry cultacolyte = new CardEntry(GameConstants.Card.cultacolyte, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(cultacolyte);
        CardEntry cultsentinel = new CardEntry(GameConstants.Card.cultsentinel, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(cultsentinel);
        CardEntry cultfanatic = new CardEntry(GameConstants.Card.cultfanatic, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(cultfanatic);
        CardEntry bloodmage = new CardEntry(GameConstants.Card.bloodmage, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(bloodmage);
        //Warlock spells
        CardEntry drainlife = new CardEntry(GameConstants.Card.drainlife, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(drainlife);
        CardEntry shroud = new CardEntry(GameConstants.Card.shroud, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(shroud);
        CardEntry fireball = new CardEntry(GameConstants.Card.fireball, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(fireball);
        CardEntry spellcardforced = new CardEntry(GameConstants.Card.spellcardforced, 1, GameConstants.CardCommanderType.Warlock);
        allCardEntries.Add(spellcardforced);
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

    private string AddStringFromDictionary(string key) {
        if (stringDictionary.ContainsKey(key)) {
            return stringDictionary[key];
        } else {
            return key;
        }
    }
}
//https://www.youtube.com/watch?v=D9_Z4wb7940&ab_channel=MichelKlaasen
