using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;


public class KnightCommander : Commander {

    public readonly int COMMANDER_ABILITY_DAMAGE = 1;

    // Use this for initialization
    void Start() {
        
    }

    /// <summary>
    /// Actions this commander performs on start of battle
    /// </summary>
    public override void OnBattleStart() {
        //Register for turn started event
        BattleEventManager._instance.RegisterForEvent(BattleEventManager.EventType.StartTurn, this);
    }

    /// <summary>
    /// Actions this commander performs on start of any turn
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public override void EventStartTurn(object sender, EventArgs e, int playerId) {
        Debug.Log("$ Knight Commander Event Start Turn $ and pId: " + playerId);
        if( playerId == this.playerId) {
            Debug.Log("Knight Commander! My player is taking turn!");
            //Try and increase this character's current charge and update UI accordingly
            if( this.currentAbilityCharge < this.abilityChargeCost) {
                this.IncreaseCommanderAbilityCharge();
            }
        }
    }

    public void IncreaseCommanderAbilityCharge() {
        this.currentAbilityCharge++;
        this.commanderUIPanel.SetCommanderResourceText(this.currentAbilityCharge);
        this.CheckActivateCommanderButton();
    }

    public void CheckActivateCommanderButton() {
        if (this.currentAbilityCharge >= this.abilityChargeCost) {
            //Activate UI element - This probably needs to be a prefab or part of this prefab
            this.commanderUIPanel.SetCommanderAbilityButtonActive( true );
        }
    }

    public override bool ActivateCommanderAbility(Hero heroTarget) {
        Debug.LogWarning("KNIGHT COMMANDER USING HEAVENLY HAMMA!!");
        //Apply damage to target per this ability
        heroTarget.TakeDamage(this.COMMANDER_ABILITY_DAMAGE);
        //Drain resource
        this.currentAbilityCharge = 0;
        //Reset UI elements
        this.commanderUIPanel.SetCommanderAbilityButtonActive(false);
        return true;
    }
}
