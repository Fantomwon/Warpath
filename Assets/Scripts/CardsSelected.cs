using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardsSelected : MonoBehaviour {

	public Text selectedSpellCards;
	public Text maxSpellCardsText;
	public Text selectedCharacterCards;
	public Text maxCharacterCardsText;

	public int selectedSpellCardsNumber;
	public int selectedCharacterCardsNumber;
	public int maxCharacterCards = 6;
	public int maxSpellCards;

	// Use this for initialization
	void Start () {
		selectedSpellCards.text = selectedSpellCardsNumber.ToString();
		maxSpellCardsText.text = maxSpellCards.ToString();
		selectedCharacterCards.text = selectedCharacterCardsNumber.ToString();
		maxCharacterCardsText.text = maxCharacterCards.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IncrementSelectedNumber (string cardType) {
		if (cardType == "hero") {
			selectedCharacterCardsNumber += 1;
			selectedCharacterCards.text = selectedCharacterCardsNumber.ToString();
		} else if (cardType == "spell") {
			selectedSpellCardsNumber += 1;
			selectedSpellCards.text = selectedSpellCardsNumber.ToString();
		}
	}

	public void DecrementSelectedNumber (string cardType) {
		if (cardType == "hero") {
			selectedCharacterCardsNumber -= 1;
			selectedCharacterCards.text = selectedCharacterCardsNumber.ToString();
		} else if (cardType == "spell") {
			selectedSpellCardsNumber -= 1;
			selectedSpellCards.text = selectedSpellCardsNumber.ToString();
		}
	}
}
