using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectBoss : MonoBehaviour {

	public Toggle boss01Button, boss02Button, boss03Button, boss04Button, boss05Button;
	public Toggle story01Button,story02Button,story03Button;

	public void UpdateButtonStates () {
		if (boss01Button.isOn) {
			GlobalObject.aiEnabled = true;
			GlobalObject.storyEnabled = false;
			TurnOffAllStoryButtons();
			boss01Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			boss01Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
			boss02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss04Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss04Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss05Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss05Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		} else if (boss02Button.isOn) {
			GlobalObject.aiEnabled = true;
			GlobalObject.storyEnabled = false;
			TurnOffAllStoryButtons();
			boss01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss02Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			boss02Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
			boss03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss04Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss04Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss05Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss05Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		} else if (boss03Button.isOn) {
			GlobalObject.aiEnabled = true;
			GlobalObject.storyEnabled = false;
			TurnOffAllStoryButtons();
			boss01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss03Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			boss03Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
			boss04Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss04Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss05Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss05Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		} else if (boss04Button.isOn) {
			GlobalObject.aiEnabled = true;
			GlobalObject.storyEnabled = false;
			TurnOffAllStoryButtons();
			boss01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss04Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			boss04Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
			boss05Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss05Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		} else if (boss05Button.isOn) {
			GlobalObject.aiEnabled = true;
			GlobalObject.storyEnabled = false;
			TurnOffAllStoryButtons();
			boss01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss04Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			boss04Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			boss05Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			boss05Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
		} else if (story01Button.isOn) {
			GlobalObject.aiEnabled = false;
			GlobalObject.storyEnabled = true;
			GlobalObject.currentlyActiveStory = "story01";
			TurnOffAllBossButtons();
			story01Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			story01Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
			story02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			story02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			story03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			story03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		} else if (story02Button.isOn) {
			GlobalObject.aiEnabled = false;
			GlobalObject.storyEnabled = true;
			GlobalObject.currentlyActiveStory = "story02";
			TurnOffAllBossButtons();
			story01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			story01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			story02Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			story02Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
			story03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			story03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		} else if (story03Button.isOn) {
			GlobalObject.aiEnabled = false;
			GlobalObject.storyEnabled = true;
			GlobalObject.currentlyActiveStory = "story03";
			TurnOffAllBossButtons();
			story01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			story01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			story02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
			story02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
			story03Button.transform.FindChild("Text").GetComponent<Text>().enabled = false;
			story03Button.transform.FindChild("Image").GetComponent<Image>().enabled = true;
		} else {
			TurnOffAllBossButtons();
			TurnOffAllStoryButtons();
		}

		BuildBossDeck();
	}

	void TurnOffAllBossButtons () {
		boss01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		boss01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		boss02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		boss02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		boss03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		boss03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		boss04Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		boss04Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		boss05Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		boss05Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
	}

	void TurnOffAllStoryButtons () {
		story01Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		story01Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		story02Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		story02Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
		story03Button.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		story03Button.transform.FindChild("Image").GetComponent<Image>().enabled = false;
	}

	void BuildBossDeck () {
		GlobalObject.instance.player2DeckSelect.Clear();

		if (boss01Button.isOn) {
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.archerCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.knightCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);
		} else if (boss02Button.isOn) {
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.archerCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.cavalryCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
		} else if (boss03Button.isOn) {
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.footSoldierCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.wolfCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.monkCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.rogueCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
		} else if (boss04Button.isOn) {
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.paladinCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.blacksmithCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.knightCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.blacksmithCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
		} else if (boss05Button.isOn) {
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.sorcererCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.bloodMageCard);
			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.bloodMageCard);
//			GlobalObject.instance.player2DeckSelect.Add(GlobalObject.instance.druidCard);
		}
	}
}