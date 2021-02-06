using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    public GameObject buttonPrefab;
    public RectTransform ParentPanel;
    private List<Encounter> encounters = new List<Encounter>();

    void Awake() {
        //Build the full list of encounters
        BuildEncounters();
    }

    void Start() {
        for (int i = 0; i < encounters.Count; i++) {
            buttonPrefab = Instantiate(buttonPrefab);
            buttonPrefab.transform.SetParent(ParentPanel);
            buttonPrefab.transform.localScale = new Vector3(1, 1, 1);

            Button tempButton = buttonPrefab.GetComponent<Button>();

            tempButton.onClick.AddListener(() => ButtonClicked(tempButton));
            tempButton.transform.Find("Text").GetComponent<Text>().text = "Level " + (i + 1);
            //Assign the proper encounter to the button
            tempButton.GetComponent<LevelToggleScript>().encounter = encounters[i];
        }
    }

    void ButtonClicked(Button buttonRef) {
        //Turn the AI on (there may be a better place to do this than here)
        GlobalObject.aiEnabled = true;
        //TODO - Need to associate a given Commander with a given encounter
        //GlobalObject.instance.enemyCommanderData = GlobalObject.instance.commandersData.Find(c => c.CharName == "The Cardinal");

        //Human player's cards come from their selected commander
        CommanderData playerCommanderData = GlobalObject.instance.humanPlayerCommanderData;
        foreach( GameConstants.Card card in playerCommanderData.Deck) {
            GlobalObject.instance.player1DeckSelect.Add( card.ToString() );
        }

        //Assign the enemy AI their cards
        buttonRef.GetComponent<LevelToggleScript>().encounter.SetUnits();
        //Load into combat
        GlobalObject.instance.LoadLevel(GameConstants.SCENE_INDEX_GAME_COMMANDERS);
    }

    void BuildEncounters() {
        //encounter01
        Encounter encounter01 = new Encounter();
        encounter01.AddUnits("archer", 5);
        encounter01.AddUnits("footsoldier", 5);
        encounter01.AddUnits("cultinitiate", 3);
        encounter01.AddUnits("cultadept", 3);
        encounter01.AddUnits("cultfanatic", 3);
        encounter01.AddUnits("cultsentinel", 1);
        encounter01.AddUnits("drainlife", 4);
        encounter01.AddUnits("fireball", 1);
        //encounter02
        Encounter encounter02 = new Encounter();
        encounter02.AddUnits("archer", 5);
        encounter02.AddUnits("footsoldier", 5);
        //encounter03
        Encounter encounter03 = new Encounter();
        encounter03.AddUnits("cultadept", 5);
        encounter03.AddUnits("cultfanatic", 5);
        //encounter04
        Encounter encounter04 = new Encounter();
        encounter04.AddUnits("archer", 5);
        encounter04.AddUnits("cultinitiate", 10);

        //Add all encounters to the list of encounters
        encounters.Add(encounter01);
        encounters.Add(encounter02);
        encounters.Add(encounter03);
        encounters.Add(encounter04);
    }
}