using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectBoss : MonoBehaviour {

    public Toggle[] toggles;
	public Toggle boss01Button, boss02Button, boss03Button, boss04Button, boss05Button, bossA1Button;
	public Toggle story01Button,story02Button,story03Button;

    public void UpdateButtonStates() {
        bool onToggleFound = false;
        foreach ( Toggle t in toggles) {
            if( t.isOn) {
                LevelToggleScript levelToggleScript = t.GetComponent<LevelToggleScript>();
                GlobalObject.currentlyActiveStory = levelToggleScript.levelIdString;
                //set flag based on level settings for ai
                GlobalObject.aiEnabled = levelToggleScript.isAiEnabled;
                t.transform.Find("Text").GetComponent<Text>().enabled = true;
                t.transform.Find("Image").GetComponent<Image>().enabled = true;
                onToggleFound = true;
            } else {
                t.transform.Find("Text").GetComponent<Text>().enabled = false;
                t.transform.Find("Image").GetComponent<Image>().enabled = false;
            }
    
            if (!onToggleFound) {
                this.TurnOffAllToggles();
            }
        }
		BuildBossDeck();
	}

    void TurnOffAllToggles() {

    }

	void TurnOffAllBossButtons () {
		boss01Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		boss01Button.transform.Find("Image").GetComponent<Image>().enabled = false;
		boss02Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		boss02Button.transform.Find("Image").GetComponent<Image>().enabled = false;
		boss03Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		boss03Button.transform.Find("Image").GetComponent<Image>().enabled = false;
		boss04Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		boss04Button.transform.Find("Image").GetComponent<Image>().enabled = false;
		boss05Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		boss05Button.transform.Find("Image").GetComponent<Image>().enabled = false;
	}

	void TurnOffAllStoryButtons () {
		story01Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		story01Button.transform.Find("Image").GetComponent<Image>().enabled = false;
		story02Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		story02Button.transform.Find("Image").GetComponent<Image>().enabled = false;
		story03Button.transform.Find("Text").GetComponent<Text>().enabled = true;
		story03Button.transform.Find("Image").GetComponent<Image>().enabled = false;
	}

    void BuildBossDeck() {
        GlobalObject.instance.player2DeckSelect.Clear();

        if (boss01Button.isOn) {
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultfanatic");
            GlobalObject.instance.player2DeckSelect.Add("cultfanatic");
            GlobalObject.instance.player2DeckSelect.Add("cultfanatic");
            GlobalObject.instance.player2DeckSelect.Add("cultsentinel");
            GlobalObject.instance.player2DeckSelect.Add("drainlife");
            GlobalObject.instance.player2DeckSelect.Add("drainlife");
            GlobalObject.instance.player2DeckSelect.Add("drainlife");
            GlobalObject.instance.player2DeckSelect.Add("drainlife");
            GlobalObject.instance.player2DeckSelect.Add("fireball");
        } else if (boss02Button.isOn) {
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultinitiate");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
        } else if (boss03Button.isOn) {
            GlobalObject.instance.player2DeckSelect.Add("archer");
            GlobalObject.instance.player2DeckSelect.Add("rogue");
            GlobalObject.instance.player2DeckSelect.Add("knight");
            GlobalObject.instance.player2DeckSelect.Add("heal");
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
        } else if (boss04Button.isOn) {
            GlobalObject.instance.player2DeckSelect.Add("archer");
            GlobalObject.instance.player2DeckSelect.Add("rogue");
            GlobalObject.instance.player2DeckSelect.Add("knight");
            GlobalObject.instance.player2DeckSelect.Add("heal");
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.blacksmithCard);
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
        } else if (boss05Button.isOn) {
            GlobalObject.instance.player2DeckSelect.Add("archer");
            GlobalObject.instance.player2DeckSelect.Add("rogue");
            GlobalObject.instance.player2DeckSelect.Add("knight");
            GlobalObject.instance.player2DeckSelect.Add("heal");
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.bloodMageCard);
            //			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
        } else if (boss05Button.isOn) {
            GlobalObject.instance.player2DeckSelect.Add("archer");
            GlobalObject.instance.player2DeckSelect.Add("rogue");
            GlobalObject.instance.player2DeckSelect.Add("knight");
            GlobalObject.instance.player2DeckSelect.Add("heal");
        } else if (bossA1Button.isOn) {
            GlobalObject.instance.player2DeckSelect.Add("cultsentinel");
            GlobalObject.instance.player2DeckSelect.Add("cultsentinel");
            GlobalObject.instance.player2DeckSelect.Add("cultsentinel");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("cultadept");
            GlobalObject.instance.player2DeckSelect.Add("heal");
            GlobalObject.instance.player2DeckSelect.Add("heal");
            GlobalObject.instance.player2DeckSelect.Add("heal");
        } else {
            Debug.LogWarning("WARNING! ALERT! SelectBoss script found no specific deck configuration and is falling back to default deck.");
            //Catch all for non boss decks
            GlobalObject.instance.player2DeckSelect.Add("archer");
            GlobalObject.instance.player2DeckSelect.Add("rogue");
            GlobalObject.instance.player2DeckSelect.Add("knight");
            GlobalObject.instance.player2DeckSelect.Add("archer");
            GlobalObject.instance.player2DeckSelect.Add("rogue");
            GlobalObject.instance.player2DeckSelect.Add("knight");
            GlobalObject.instance.player2DeckSelect.Add("heal");
            GlobalObject.instance.player2DeckSelect.Add("fireball");
        }
    }
}