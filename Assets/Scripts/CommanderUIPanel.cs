using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommanderUIPanel : MonoBehaviour
{
    protected GameObject commanderAbilityButtonUIEmphasis;
    protected GameObject commanderAbilityButton;
    protected Text currentAbilityResourceText;
    protected Text maxResourceText;
    protected int commanderTeamId = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int teamId) {
        Debug.Log("Commander Panel Initialize called!");
        //if commander ability button UI emphasis is null then find the gameobject in the scene and cache it as a reference
        if( this.commanderAbilityButtonUIEmphasis == null) {
            this.commanderAbilityButtonUIEmphasis = this.transform.Find("CommanderAbilityButtonEmphasis").gameObject;
            this.commanderAbilityButtonUIEmphasis.SetActive(false);
        }
        //if commander ability button is null then find the gameobject in the scene and cache it as a reference
        if( this.commanderAbilityButton == null) {
            this.commanderAbilityButton = this.transform.Find("CommanderAbilityButton").gameObject;
            //Set this UI element to not be interactable until enough resource has been accumulated
            this.commanderAbilityButton.GetComponent<Button>().interactable = false;
        }
        //if text gameobjects were unset then find the gameobjects in the scene and cache it as a reference
        if( this.currentAbilityResourceText == null) {
            this.currentAbilityResourceText = this.transform.Find("CommanderTextPanel/CurrentCommanderResourceTxt").GetComponent<Text>();
        }

        //if text gameobjects were unset then find the gameobjects in the scene and cache it as a reference
        if (this.maxResourceText == null) {
            this.maxResourceText = this.transform.Find("CommanderTextPanel/MaxCommanderResourceTxt").GetComponent<Text>();
        }

        //Cache team id. Only the player's team will be able
        this.commanderTeamId = teamId;
    }

    /// <summary>
    /// Set values of display text for commander's current and max resource. This override is used for initializing the display
    /// </summary>
    /// <param name="maxResource"></param>
    /// <param name="currentResource"></param>
    public void SetCommanderResourceText( int currentResource, int maxResource ) {
        this.currentAbilityResourceText.text = currentResource.ToString();
        this.maxResourceText.text = maxResource.ToString();
    }

    /// <summary>
    /// Set values of display text for commander's current resource
    /// </summary>
    /// <param name="currentResource"></param>
    public void SetCommanderResourceText( int currentResource) {
        this.currentAbilityResourceText.text = currentResource.ToString();
    }

    /// <summary>
    /// Function to notify this UI script as to whether it should set the commander ability active for use or not
    /// </summary>
    /// <param name="shouldSetActive"></param> pass true for value if should be active and false to deactivate.
    public void SetCommanderAbilityButtonActive(bool shouldSetActive) {
        //Disable UI emphasis element
        this.commanderAbilityButtonUIEmphasis.SetActive(shouldSetActive);
        this.commanderAbilityButton.GetComponent<Button>().interactable = shouldSetActive;
    }

    public void CommanderAbilityButtonClicked() {
        Debug.LogWarning("COMMANDER ABILITY BUTTON CLICKED (1)");
        //Only human player can activate their ability through a click and only on their turn
        if ( this.commanderTeamId == GameConstants.HUMAN_PLAYER_ID && PlayField.instance.player1Turn) {
            Debug.LogWarning("COMMANDER ABILITY BUTTON CLICKED (2)");
            //Set commander ability active on mouse
            PlayField.instance.commanderAbilityActiveOnMouse = true;
        }
    }
}
