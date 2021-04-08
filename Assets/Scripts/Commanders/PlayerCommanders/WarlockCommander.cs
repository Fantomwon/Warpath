using UnityEngine;
using System.Collections;
using System;


public class WarlockCommander : Commander {

    protected readonly int COMMANDER_ABILITY_ARMOR = 2;

    // Use this for initialization
    void Start() {

    }

    /// <summary>
    /// Actions this commander performs on start of battle
    /// </summary>
    public override void OnBattleStart() {
        //Register for turn started event
        BattleEventManager._instance.RegisterForEvent(BattleEventManager.EventType.UnitReceiveDamage, this);
    }

    /// <summary>
    /// Event fired on any unit receiving damage. Crusader commander gains ability charge on an allied soldier taking damage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="playerId"></param>
    public override void EventUnitReceiveDamage(object sender, EventArgs e, int playerId, Hero damageReceiver) {
        //Crusader only gains charge if allied soldier is damaged
        if (playerId == this.playerId && this.currentAbilityCharge < this.abilityChargeCost) {
            //Increase charge
            this.IncreaseCommanderAbilityCharge();
        }

    }

    public override bool ActivateCommanderAbilityOnHero(Hero heroTarget) {
        Debug.LogWarning("WARLOCK COMMANDER USING SHIELD!!");
        //Grant ability target shield
        heroTarget.AddArmor(this.COMMANDER_ABILITY_ARMOR);
        //Drain resource
        this.currentAbilityCharge = 0;
        //Update UI 
        this.commanderUIPanel.SetCommanderResourceText(this.currentAbilityCharge);
        //Reset UI elements
        this.commanderUIPanel.SetCommanderAbilityButtonActive(false);
        return true;
    }

    // Update is called once per frame
    void Update() {

    }
}
