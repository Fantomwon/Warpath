using UnityEngine;
using System.Collections;
using System;

public class CardinalCommander : Commander {

    public readonly int COMMANDER_ABILITY_HEALING = 2;

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
        //Debug.Log("$ Knight Commander Event Start Turn $ and pId: " + playerId);
        if (playerId == this.playerId) {
            //Debug.Log("Knight Commander! My player is taking turn!");
            //Try and increase this character's current charge and update UI accordingly
            if (this.currentAbilityCharge < this.abilityChargeCost) {
                this.IncreaseCommanderAbilityCharge();
            }
        }
    }

    public override bool ActivateCommanderAbility(Hero heroTarget) {
        Debug.LogWarning("CARDINAL COMMANDER USING HEAVENLY HEALIN'!!");
        //Apply damage to target per this ability
        heroTarget.HealPartial(this.COMMANDER_ABILITY_HEALING);
        //Drain resource
        this.currentAbilityCharge = 0;
        //Update resource UI
        this.commanderUIPanel.SetCommanderResourceText(this.currentAbilityCharge);
        //Reset UI elements
        this.commanderUIPanel.SetCommanderAbilityButtonActive(false);
        return true;
    }
}
