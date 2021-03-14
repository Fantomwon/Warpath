using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    public GameObject buttonPrefab;
    public RectTransform ParentPanel;
    private List<Encounter> encounters = new List<Encounter>();
    public List<GameObject> commanderList;
    //public List<Commander> loadedCommanders;
    public Dictionary<int, CommanderData> enemyEncounterCommanders;

    public static MapManager instance;

    bool isInitialLoad = true;

    void Awake() {
        if (MapManager.instance == null) {
            MapManager.instance = this;
            DontDestroyOnLoad(this.gameObject);
            //register for scene loaded event
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        } else if (instance != this) {
            Destroy(this.gameObject);
        }
        //Instantiate dictionary for enemy commanders and populate it with commanders corresponding to the encounter IDs
        this.enemyEncounterCommanders = new Dictionary<int, CommanderData>();
        //Build list of commander scripts from prefabs
        int i = 0;
        foreach (GameObject commanderPrefab in this.commanderList) {
            Commander commander = commanderPrefab.GetComponent<Commander>();
            this.enemyEncounterCommanders.Add(i, commander.commanderData);
            i++;
        }
        //Build the full list of encounters
        BuildEncounters();
    }

    void Start() {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if( scene.buildIndex == GameConstants.SCENE_INDEX_MAP) {
            //Get reference to rect transform in scene for UI stuff
            this.ParentPanel = GameObject.Find("ToggleGroup").GetComponent<RectTransform>();
            //Create buttons
            for (int i = 0; i < encounters.Count; i++) {
                GameObject btnFromPrefab = Instantiate(buttonPrefab);
                btnFromPrefab.transform.SetParent(ParentPanel);
                btnFromPrefab.transform.localScale = new Vector3(1, 1, 1);

                Button tempButton = btnFromPrefab.GetComponent<Button>();

                tempButton.onClick.AddListener(() => ButtonClicked(tempButton));
                tempButton.transform.Find("Text").GetComponent<Text>().text = "Level " + (i + 1);
                //Assign the proper encounter to the button
                tempButton.GetComponent<LevelToggleScript>().encounter = encounters[i];
            }
        }
    }

    void ButtonClicked(Button buttonRef) {
        //Turn the AI on (there may be a better place to do this than here)
        GlobalObject.aiEnabled = true;

        //Assign the enemy AI their cards
        buttonRef.GetComponent<LevelToggleScript>().encounter.SetUnits();

        //Get enemy commander for the selected encounter and cache with global object for level load
        GlobalObject.instance.enemyCommanderData = buttonRef.GetComponent<LevelToggleScript>().encounter.enemyCommanderData;

        //Load into combat
        GlobalObject.instance.LoadLevel(GameConstants.SCENE_INDEX_GAME_COMMANDERS);
    }

    void BuildEncounters() {
        /*encounter01*/
        Encounter encounter01 = new Encounter();
        //Set commander
        encounter01.enemyCommanderData = this.enemyEncounterCommanders[0];
        //Cards
        foreach( CardEntry cardEntry in encounter01.enemyCommanderData.Deck) {
            encounter01.AddUnits(cardEntry.Card.ToString(), cardEntry.CardAmount); //TODO => should deck's somehow be structs of cards + count for ease of editing/ sanity?
        }

        /*encounter02*/
        Encounter encounter02 = new Encounter();
        //Set commander
        encounter02.enemyCommanderData = this.enemyEncounterCommanders[1];
        //Cards
        foreach (CardEntry cardEntry in encounter02.enemyCommanderData.Deck) {
            encounter02.AddUnits(cardEntry.Card.ToString(), cardEntry.CardAmount);
        }

        //encounter03
        Encounter encounter03 = new Encounter();
        encounter03.enemyCommanderData = this.enemyEncounterCommanders[2];
        //Cards
        foreach (CardEntry cardEntry in encounter03.enemyCommanderData.Deck) {
            encounter03.AddUnits(cardEntry.Card.ToString(), cardEntry.CardAmount);
        }

        //encounter04
        Encounter encounter04 = new Encounter();
        encounter04.enemyCommanderData = this.enemyEncounterCommanders[0];
        //Cards
        foreach (CardEntry cardEntry in encounter04.enemyCommanderData.Deck) {
            encounter04.AddUnits(cardEntry.ToString(), cardEntry.CardAmount);
        }

        //Add all encounters to the list of encounters
        encounters.Add(encounter01);
        encounters.Add(encounter02);
        encounters.Add(encounter03);
        encounters.Add(encounter04);
    }
}