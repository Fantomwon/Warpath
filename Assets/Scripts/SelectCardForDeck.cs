using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SelectCardForDeck : MonoBehaviour {

	void Start () {
		if (SceneManager.GetActiveScene().name == "Game") {
			Destroy (gameObject);
		}
	}

	void OnMouseDown () {
        Debug.Log("SelectCardOnMouseDown 1");
        //Add the cardId to the appropriate player's deckSelect list
        if (SceneManager.GetActiveScene().buildIndex == GameConstants.SCENE_INDEX_POST_BATTLE_CARD_SELECT) {
            Debug.Log("SelectCardOnMouseDown 2");
            /*Add clicked card to the human player's deck*/
            //If card was already in the deck then increase entry Amt by 1
            CardEntry addedCard = new CardEntry( GetComponentInParent<Card>().cardId, 1 );
            GlobalObject.instance.humanPlayerCommanderData.Deck.Add( addedCard );
            //Normal flow would be to load back to the world map
            GlobalObject.instance.LoadLevel(GameConstants.SCENE_INDEX_MAP);
        }  
    }
}