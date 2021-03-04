using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CardSelectManager : MonoBehaviour
{
    public List<string> cards;
    public static CardSelectManager instance;

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
            this.InstantiatePlayerCards();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
