using UnityEngine;
using System.Collections;
using System;

public interface IEventListener  {

    /// <summary>
    /// Override this event in commander or unit when listening to turn star event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void EventStartTurn(object sender, EventArgs e, int playerId);

    /// <summary>
    /// Override this event in commander or unit when listening to event for units receiving damage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="playerId"></param>
    void EventUnitReceiveDamage(object sender, EventArgs e, int playerId, Hero damageReceiver);
}
