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
        if(commanderScript == null) {
            Debug.LogWarning("Uh oh! commanderscript parameter is null!");
        }
        //If commander hasn't been selected yet, instantiate gameobject used for visual representation
        if( this.currentlySelectedCommander == null) {

            GameObject potentialSelectedCommanderPrefab = Resources.Load<GameObject>(commanderScript.selectedCommanderPrefabPath);
            GameObject potentialSelectedCommander = GameObject.Instantiate(potentialSelectedCommanderPrefab) as GameObject;
            
            if ( potentialSelectedCommander != null) {
                this.currentlySelectedCommander = potentialSelectedCommander;
                //Parent newly created prefab to UI element for positioning and will also dictate the component find approach
                this.currentlySelectedCommander.transform.SetParent(GameObject.Find("CommanderSelectUIManager/PanelSelectedCommander/Image").transform, false);
                //Get commander component from within children objects
                Commander selectedCommanderScript = this.currentlySelectedCommander.GetComponent<Commander>();
                //Load prefab
                GameObject selectedCommanderPrefab = Resources.Load<GameObject>(commanderScript.selectedCommanderPrefabPath);
                if(selectedCommanderScript == null) {
                    Debug.LogWarning("Ruh ROhh!! cselected commander scirpt easty null");
                }
                //Populate data 
                selectedCommanderScript.SetCommanderAttributes(commanderScript.commanderName, selectedCommanderPrefab, commanderScript.selectedCommanderPrefabPath, selectedCommanderScript.hp, selectedCommanderScript.handSize, commanderScript.commanderData, commanderScript.playerId, commanderScript.abilityChargeCost, commanderScript.abilityTargetType );
                //Scale up so image is visible
                this.currentlySelectedCommander.transform.localScale = new Vector3(100, 100, 1);
            }
        } else {
            GameObject selectedCommanderPrefab = Resources.Load<GameObject>(commanderScript.selectedCommanderPrefabPath);
            GameObject previousCommander = this.currentlySelectedCommander;
            GameObject.Destroy(previousCommander);

            this.currentlySelectedCommander = GameObject.Instantiate(selectedCommanderPrefab) as GameObject;
            this.currentlySelectedCommander.transform.SetParent(GameObject.Find("CommanderSelectUIManager/PanelSelectedCommander/Image").transform, false);
            this.currentlySelectedCommander.transform.localScale = new Vector3(100, 100, 1);
            Commander selectedCommanderScript = this.currentlySelectedCommander.GetComponent<Commander>();
            
            selectedCommanderScript.SetCommanderAttributes(commanderScript.commanderName, selectedCommanderPrefab, commanderScript.selectedCommanderPrefabPath, selectedCommanderScript.hp, selectedCommanderScript.handSize, commanderScript.commanderData, commanderScript.playerId, commanderScript.abilityChargeCost, commanderScript.abilityTargetType);
        }

    }

    public void ConfirmCommanderSelection() {
        GlobalObject.instance.LoadLevel(GameConstants.SCENE_INDEX_MAP);
    }
}