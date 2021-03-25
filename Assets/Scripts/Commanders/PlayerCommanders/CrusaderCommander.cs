using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class CrusaderCommander : Commander{

    private PlayField playField;
    
    // Use this for initialization
    void Start() {

    }

    /// <summary>
    /// Actions this commander performs on start of battle
    /// </summary>
    public override void OnBattleStart() {
        //Register for turn started event
        BattleEventManager._instance.RegisterForEvent(BattleEventManager.EventType.UnitReceiveDamage, this);
        //Find the PlayField and set the playField variable
        playField = FindObjectOfType<PlayField>();
    }

    /// <summary>
    /// Event fired on any unit receiving damage. Crusader commander gains ability charge on an allied soldier taking damage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="playerId"></param>
    public override void EventUnitReceiveDamage(object sender, EventArgs e, int playerId, Hero damageReceiver) { 
        //Crusader only gains charge if allied soldier is damaged
        if( playerId == this.playerId && this.currentAbilityCharge < this.abilityChargeCost ) {
            //Increase charge
            this.IncreaseCommanderAbilityCharge();
        }

    }

    public override bool ActivateCommanderAbility(Hero heroTarget) {
        Debug.LogWarning("CRUSADER COMMANDER USING CHARGE!!");
        //Grant ability target haste
        heroTarget.transform.GetComponent<Hero>().usingHaste = true;
        if (playField.player1Turn) {
            playField.Player1MoveHasteCheck(heroTarget.transform);
        } else {
            playField.Player2MoveHasteCheck(heroTarget.transform);
        }
        //Drain resource
        this.currentAbilityCharge = 0;
        //Update UI 
        this.commanderUIPanel.SetCommanderResourceText(this.currentAbilityCharge);
        //Reset UI elements
        this.commanderUIPanel.SetCommanderAbilityButtonActive(false);
        return true;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
