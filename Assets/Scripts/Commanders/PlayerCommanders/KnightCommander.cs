using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;


public class KnightCommander : Commander {

    public readonly int COMMANDER_ABILITY_DAMAGE = 2;

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
        if( playerId == this.playerId) {
            //Try and increase this character's current charge and update UI accordingly
            if( this.currentAbilityCharge < this.abilityChargeCost) {
                this.IncreaseCommanderAbilityCharge();
            }
        }
    }

    public override bool ActivateCommanderAbility(Hero heroTarget) {
        //Apply damage to target per this ability
        PlayField.instance.DamageHero(heroTarget, this.COMMANDER_ABILITY_DAMAGE);
        //Drain resource
        this.currentAbilityCharge = 0;
        //Update UI 
        this.commanderUIPanel.SetCommanderResourceText(this.currentAbilityCharge);
        //Reset UI elements
        this.commanderUIPanel.SetCommanderAbilityButtonActive(false);
        return true;
    }
}
