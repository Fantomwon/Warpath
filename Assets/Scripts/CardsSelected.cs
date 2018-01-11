using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardsSelected : MonoBehaviour {

	public Text selectedNumberText;
	public Text maxNumberText;

	public int selectedNumber;
	public int maxNumber = 6;

	// Use this for initialization
	void Start () {
		selectedNumberText.text = selectedNumber.ToString();
		maxNumberText.text = maxNumber.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IncrementSelectedNumer () {
		selectedNumber += 1;
		selectedNumberText.text = selectedNumber.ToString();
	}

	public void DecrementSelectedNumer () {
		selectedNumber -= 1;
		selectedNumberText.text = selectedNumber.ToString();
	}
}
