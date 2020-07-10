using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;


public class KnightCommander : Commander {

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
        }
    }

}
