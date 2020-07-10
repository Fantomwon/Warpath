using UnityEngine;
using System.Collections;
using System;

public class CardinalCommander : Commander {

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
        Debug.Log("$ Cardinal Event Start Turn $");
    }

}
