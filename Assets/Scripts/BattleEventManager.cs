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
            OnTurnStart(null, null,playerId);
        }
    }

    public void RegisterForEvent( BattleEventManager.EventType eventType, IEventListener listener) {
        Debug.Log("BATTLE EVENT MANAGER!! - Register For Event called!");
        switch( eventType) {
            case BattleEventManager.EventType.StartTurn:
                BattleEventManager._instance.OnTurnStart += listener.EventStartTurn;
                break;
            default:
                break;
        }
        
    }

    public delegate void OnTurnStartHandler(object sender, EventArgs e, int playerId);
    public event OnTurnStartHandler OnTurnStart;

    //Possible Event Types
    public enum EventType { StartTurn, UnitDeath, UnitReceiveDamage };
}