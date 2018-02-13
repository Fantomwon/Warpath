using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectPlayerClass : MonoBehaviour {

	public Toggle mageButton, rogueButton, druidButton, blacksmithButton;
	public GameObject mageSpells, rogueSpells, druidSpells, blacksmithSpells;
	public Text currentlySelectedClass;

	void Start () {
		currentlySelectedClass.text = "no class selected";
	}

	public void UpdateButtonStates () {
		if (mageButton.isOn) {
			currentlySelectedClass.text = "Mage";
			mageButton.transform.Find("Text").GetComponent<Text>().enabled = false;
			mageButton.transform.Find("Image").GetComponent<Image>().enabled = true;
			rogueButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			rogueButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			druidButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			druidButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			blacksmithButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			blacksmithButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			mageSpells.GetComponent<CanvasGroup>().alpha = 1;
			rogueSpells.GetComponent<CanvasGroup>().alpha = 0;
			druidSpells.GetComponent<CanvasGroup>().alpha = 0;
			blacksmithSpells.GetComponent<CanvasGroup>().alpha = 0;
		} else if (rogueButton.isOn) {
			currentlySelectedClass.text = "Rogue";
			mageButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			mageButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			rogueButton.transform.Find("Text").GetComponent<Text>().enabled = false;
			rogueButton.transform.Find("Image").GetComponent<Image>().enabled = true;
			druidButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			druidButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			blacksmithButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			blacksmithButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			mageSpells.GetComponent<CanvasGroup>().alpha = 0;
			rogueSpells.GetComponent<CanvasGroup>().alpha = 1;
			druidSpells.GetComponent<CanvasGroup>().alpha = 0;
			blacksmithSpells.GetComponent<CanvasGroup>().alpha = 0;
		} else if (druidButton.isOn) {
			currentlySelectedClass.text = "Druid";
			mageButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			mageButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			rogueButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			rogueButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			druidButton.transform.Find("Text").GetComponent<Text>().enabled = false;
			druidButton.transform.Find("Image").GetComponent<Image>().enabled = true;
			blacksmithButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			blacksmithButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			mageSpells.GetComponent<CanvasGroup>().alpha = 0;
			rogueSpells.GetComponent<CanvasGroup>().alpha = 0;
			druidSpells.GetComponent<CanvasGroup>().alpha = 1;
			blacksmithSpells.GetComponent<CanvasGroup>().alpha = 0;
		} else if (blacksmithButton.isOn) {
			currentlySelectedClass.text = "Blacksmith";
			mageButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			mageButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			rogueButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			rogueButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			druidButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			druidButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			blacksmithButton.transform.Find("Text").GetComponent<Text>().enabled = false;
			blacksmithButton.transform.Find("Image").GetComponent<Image>().enabled = true;
			mageSpells.GetComponent<CanvasGroup>().alpha = 0;
			rogueSpells.GetComponent<CanvasGroup>().alpha = 0;
			druidSpells.GetComponent<CanvasGroup>().alpha = 0;
			blacksmithSpells.GetComponent<CanvasGroup>().alpha = 1;
		} else {
			currentlySelectedClass.text = "no class selected";
			mageButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			mageButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			rogueButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			rogueButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			druidButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			druidButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			blacksmithButton.transform.Find("Text").GetComponent<Text>().enabled = true;
			blacksmithButton.transform.Find("Image").GetComponent<Image>().enabled = false;
			mageSpells.GetComponent<CanvasGroup>().alpha = 0;
			rogueSpells.GetComponent<CanvasGroup>().alpha = 0;
			druidSpells.GetComponent<CanvasGroup>().alpha = 0;
			blacksmithSpells.GetComponent<CanvasGroup>().alpha = 0;
		}

		SetPlayerClass ();
	}

	private void SetPlayerClass () {
		if (SceneManager.GetActiveScene().name == "CardSelectP1" || SceneManager.GetActiveScene().name == "CardSelectSinglePlayer") {
			if (mageButton.isOn) {
				GlobalObject.instance.player1Class = "Mage";
			} else if (rogueButton.isOn) {
				GlobalObject.instance.player1Class = "Rogue";
			} else if (druidButton.isOn) {
				GlobalObject.instance.player1Class = "Druid";
			} else if (blacksmithButton.isOn) {
				GlobalObject.instance.player1Class = "Blacksmith";
			} else {
				GlobalObject.instance.player1Class = null;
			}
		} else if (SceneManager.GetActiveScene().name == "CardSelectP2") {
			if (mageButton.isOn) {
				GlobalObject.instance.player2Class = "Mage";
			} else if (rogueButton.isOn) {
				GlobalObject.instance.player2Class = "Rogue";
			} else if (druidButton.isOn) {
				GlobalObject.instance.player2Class = "Druid";
			} else if (blacksmithButton.isOn) {
				GlobalObject.instance.player2Class = "Blacksmith";
			} else {
				GlobalObject.instance.player2Class = null;
			}
		}
	}
}