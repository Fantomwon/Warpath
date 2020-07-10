using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour, IEventListener {
    public string commanderName;
    public GameObject commanderPrefab;
    public string selectedCommanderPrefabPath;
    public Sprite commanderAbilitySprite;
    public int hp;
    public int handSize;
    public CommanderData commanderData;
    public GameConstants.FactionType faction;
    public GameConstants.CommanderAbilityChargeType abilityChargeType;
    public int playerId;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    

    public bool ActivateCommanderAbility() {
        return true;
    }

    public void SetCommanderAttributes( string name, GameObject prefab, string selectedCommanderPrefabPath, int startingHp, int startingHandSize, CommanderData cData, int playerId) {
        //Data members
        this.commanderName = name;
        this.commanderPrefab = prefab;
        this.selectedCommanderPrefabPath = selectedCommanderPrefabPath;
        this.hp = startingHp;
        this.handSize = startingHandSize;
        this.playerId = playerId;
        this.commanderData = cData;
    }

    public void SetCommanderAttributes( CommanderData data) {
        //Data members
        this.commanderName = data.CharName;
        this.selectedCommanderPrefabPath = data.PrefabPath;
        this.hp = data.HP;
        this.handSize = data.CurrentHandSize;
        this.commanderData = data;

        Debug.Log("(2) SetCommanderAttributes called by " + data.CharName);
    }

    public void InitializeSelectUI() {

    }

    /// <summary>
    /// Commander overrides this with method calls, such as registering for events
    /// </summary>
    public virtual void OnBattleStart() {

    }

    /// <summary>
    /// Override this event in commander or unit when listening to turn star event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public virtual void EventStartTurn(object sender, EventArgs e, int playerId) {

    }


}
