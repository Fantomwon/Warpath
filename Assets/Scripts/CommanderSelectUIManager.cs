using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommanderSelectUIManager : MonoBehaviour
{
    public GameObject commanderSelectedStatsUI;
    public GameObject textName;
    public GameObject textHpAmount;
    public GameObject textHandSizeAmount;
    public GameObject templateCommanderSelected;
    public GameObject currentlySelectedCommander;
    public GameObject currentlySelectedCommanderImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedCommander( Commander commanderScript ) {
        this.SetSelectedCommanderText(commanderScript);
        this.SetSelectedCommanderImages(commanderScript);
        GlobalObject.instance.SetSelectedCommander(commanderScript.commanderData);
    }

    public void SetSelectedCommanderText( Commander commanderScript) {
        this.textName.GetComponent<Text>().text = commanderScript.commanderName;
        this.textHpAmount.GetComponent<Text>().text = commanderScript.hp.ToString();
        this.textHandSizeAmount.GetComponent<Text>().text = commanderScript.handSize.ToString();
    }

    public void SetSelectedCommanderImages( Commander commanderScript ) {
        //If commander hasn't been selected yet, instantiate gameobject used for visual representation
        if( this.currentlySelectedCommander == null) {
            GameObject potentialSelectedCommander = GameObject.Instantiate(this.templateCommanderSelected) as GameObject;
            if( potentialSelectedCommander != null) {

                this.currentlySelectedCommander = potentialSelectedCommander;
                Commander selectedCommanderScript = this.currentlySelectedCommander.GetComponent<Commander>();
                //Load prefab
                GameObject selectedCommanderPrefab = Resources.Load<GameObject>(commanderScript.selectedCommanderPrefabPath);
                selectedCommanderScript.SetCommanderAttributes(commanderScript.commanderName, selectedCommanderPrefab, commanderScript.selectedCommanderPrefabPath, selectedCommanderScript.hp, selectedCommanderScript.handSize, commanderScript.commanderData, commanderScript.playerId, commanderScript.abilityChargeCost, commanderScript.abilityTargetType );
                //Parent newly created prefab to UI element for positioning
                this.currentlySelectedCommander.transform.SetParent( GameObject.Find("CommanderSelectUIManager/PanelSelectedCommander").transform, false );
                //Create image to use on selected UI element
                this.currentlySelectedCommanderImage = GameObject.Instantiate(selectedCommanderPrefab) as GameObject;
                this.currentlySelectedCommanderImage.transform.SetParent(this.currentlySelectedCommander.transform.Find("Image").transform, false);
                //Set commander UI image offset from origin within its panel
                RectTransform commanderRTrans = this.currentlySelectedCommander.GetComponent<RectTransform>();
                commanderRTrans.anchoredPosition.Set(GameConstants.SELECTED_COMMANDER_IMAGE_OFFSET_POS_X, GameConstants.SELECTED_COMMANDER_IMAGE_OFFSET_POS_Y);
            }
        } else {
            GameObject selectedCommanderPrefab = Resources.Load<GameObject>(commanderScript.selectedCommanderPrefabPath);
            GameObject previousCommanderImage = this.currentlySelectedCommanderImage;
            this.currentlySelectedCommanderImage = null;
            GameObject.Destroy(previousCommanderImage);
            this.currentlySelectedCommanderImage = GameObject.Instantiate(selectedCommanderPrefab) as GameObject;
            Commander selectedCommanderScript = this.currentlySelectedCommander.GetComponent<Commander>();
            selectedCommanderScript.SetCommanderAttributes(commanderScript.commanderName, selectedCommanderPrefab, commanderScript.selectedCommanderPrefabPath, selectedCommanderScript.hp, selectedCommanderScript.handSize, commanderScript.commanderData, commanderScript.playerId, commanderScript.abilityChargeCost, commanderScript.abilityTargetType);
            this.currentlySelectedCommanderImage.transform.SetParent(this.currentlySelectedCommander.transform.Find("Image").transform, false);
        }

    }

    public void ConfirmCommanderSelection() {
        GlobalObject.instance.LoadLevel(GameConstants.SCENE_INDEX_BOSS_SELECT);
    }
}
