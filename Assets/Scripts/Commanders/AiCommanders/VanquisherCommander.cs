using UnityEngine;
using System.Collections;
using System;


public class VanquisherCommander : Commander {
    private AiManager aiManager;
    /// <summary>
    /// Actions this commander performs on start of battle
    /// </summary>
    public override void OnBattleStart() {
        Debug.Log("VANQUISHER $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ " + this.playerId);
        //Register for turn started event
        BattleEventManager._instance.RegisterForEvent(BattleEventManager.EventType.StartTurn, this);
        aiManager = FindObjectOfType<AiManager>();
    }

    /// <summary>
    /// Actions this commander performs on start of any turn
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="playerId"></param>
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

    public override bool ActivateCommanderAbilityOnSpawnPoint(Vector2 vec2) {
        //Spawn the soldier
        aiManager.SpawnHero("cultsentinel", vec2, false);
        //Drain resource
        this.currentAbilityCharge = 0;
        //Update resource UI
        this.commanderUIPanel.SetCommanderResourceText(this.currentAbilityCharge);
        //Reset UI elements
        this.commanderUIPanel.SetCommanderAbilityButtonActive(false);
        return true;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}