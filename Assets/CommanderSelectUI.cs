using UnityEngine;

public class CommanderSelectUI : MonoBehaviour
{
    public CommanderSelectUIManager commanderUiManager;
    public Commander commanderScript;

    //Called on object creation
    private void Awake() {
        GameObject possibleUIManager = GameObject.FindGameObjectWithTag(GameConstants.TAG_COMMANDER_SELECT_UI_MANAGER);
        if (possibleUIManager != null) {
            this.commanderUiManager = possibleUIManager.GetComponent<CommanderSelectUIManager>();
        } else {
            Debug.LogWarning("Commander Select Ui manager not found!");
        }
        Commander possibleCommander = this.GetComponent<Commander>();
        if( possibleCommander != null) {
            this.commanderScript = possibleCommander;
        } else {
            Debug.LogWarning("Commander Select Ui: Commander not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectClicked() {
        Debug.Log("Commander Select Clicked!");
        if( this.commanderUiManager != null) {
            this.commanderUiManager.SetSelectedCommanderText(commanderScript);
        }
    }
}
