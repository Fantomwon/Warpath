using UnityEngine;
using System.Collections;
using System;

public class BattleEventManager : MonoBehaviour {

    public static BattleEventManager _instance;
    
    void Awake() {
        //Enforce singleton pattern and only allow one battleEventManager to live at a time
        if (BattleEventManager._instance == null) {
            BattleEventManager._instance = this;
        } else if (BattleEventManager._instance != this) {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void NotifyOnTurnStart(int playerId) {
        //Notify all listeners of the on turn start event
        if (OnTurnStart != null) {
            OnTurnStart(null, null, playerId);
        }
    }

    public void NotifyUnitReceiveDamage( int playerId, Hero damageReceiver) {
        //Notify all listeners of a unit receiving damage event
        if( OnUnitReceiveDamage != null) {
            OnUnitReceiveDamage(null, null, playerId, damageReceiver);
        }
    }

    /// <summary>
    /// Call this method to register for an event
    /// </summary>
    /// <param name="eventType">Id for the event type you want</param>
    /// <param name="listener">The commander or unit that will be notified</param>
    public void RegisterForEvent( BattleEventManager.EventType eventType, IEventListener listener) {
        Debug.Log("BATTLE EVENT MANAGER!! - Register For Event called!");
        switch( eventType) {
            case BattleEventManager.EventType.StartTurn:
                BattleEventManager._instance.OnTurnStart += listener.EventStartTurn;
                break;
            case BattleEventManager.EventType.UnitReceiveDamage:
                BattleEventManager._instance.OnUnitReceiveDamage += listener.EventUnitReceiveDamage;
                break;
            default:
                break;
        }
        
    }

    public delegate void OnTurnStartHandler(object sender, EventArgs e, int playerId);
    public delegate void OnUnitReceiveDamageHandler(object sender, EventArgs e, int playerId, Hero damageReceiver);
    public event OnTurnStartHandler OnTurnStart;
    public event OnUnitReceiveDamageHandler OnUnitReceiveDamage;

    //Possible Event Types
    public enum EventType { StartTurn, UnitDeath, UnitReceiveDamage };
}