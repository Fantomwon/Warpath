using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CardSelectManager : MonoBehaviour
{
    public List<string> cards;
    public static CardSelectManager instance;
    private int _numCardsForSelection = 3;

    public int NumCardsForSelection {
        get {
            return this._numCardsForSelection;
        }
        set {
            this._numCardsForSelection = value;
        }
    }

    private void Awake() {
        if (CardSelectManager.instance == null) {
            CardSelectManager.instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Register for OnSceneLoaded event
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        } else if (instance != this) {
            Debug.LogError("DUPLICATE GLOBAL OBJECT FOUND! DELETING THIS ONE");
            Destroy(this.gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if ( scene.buildIndex == GameConstants.SCENE_INDEX_POST_BATTLE_CARD_SELECT) {
            //Set cards for select
            this.UpdateSelectionCards();
            //Create actual game objects from list
            this.InstantiatePlayerCards();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSelectionCards() {
        //Start with creating list to hold all card entries
        GameConstants.CardCommanderType type = GlobalObject.instance.humanPlayerCommanderData.CardCommanderType;
        //filter to only have cards matching commander type
        List<CardEntry> filteredCardEntries = GlobalObject.instance.allCardEntries.FindAll(c => c.CommanderType == type || c.CommanderType == GameConstants.CardCommanderType.All);
        List<string> randomSelection = new List<string>();
        //Grab random cards from list
        for(int i = 0; i < this.NumCardsForSelection; i++) {
            int randomIndex = Random.Range(0, filteredCardEntries.Count);
            randomSelection.Add(filteredCardEntries[randomIndex].Card.ToString());
            //Splice out the entry from original list to avoid repeats
            filteredCardEntries.RemoveAt(randomIndex);
            //Break loop if we have exhausted all careds
            if( filteredCardEntries.Count == 0) {
                break;
            }
        }
        //Set the cards list from our random selection
        this.SetCardsForSelect(randomSelection);
    }

    public void SetCardsForSelect( List<string> selectionCards) {
        this.cards = selectionCards;
    }

    void InstantiatePlayerCards() {
        Debug.Log("GLOBAL OBJECT INSTANTIATE PLAYER CARDS CALLED");
        
        foreach (string cardId in cards) {
            GlobalObject.instance.SetTemplateHeroCardAttributes(cardId);
            GameObject newCard = Instantiate(GlobalObject.instance.templateHeroCard) as GameObject;
            newCard.transform.SetParent(GameObject.Find("CardSelectionScrollList/Viewport/Content/Card Container").transform, false);
            GlobalObject.instance.SetTemplateHeroCardImage(newCard.GetComponent<Card>().cardId.ToString(), newCard);
        }

    }
}
