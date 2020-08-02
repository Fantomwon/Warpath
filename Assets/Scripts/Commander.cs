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
    public int abilityChargeCost;
    public int currentAbilityCharge = 0;
    public CommanderData commanderData;
    public GameConstants.FactionType faction;
    public GameConstants.CommanderAbilityChargeType abilityChargeType;
    public GameConstants.CommanderAbilityTargetType abilityTargetType;
    public int playerId;
    public CommanderUIPanel commanderUIPanel;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    

    public virtual bool ActivateCommanderAbility( Hero heroTarget ) {
        return true;
    }

    public void SetCommanderAttributes( string name, GameObject prefab, string selectedCommanderPrefabPath, int startingHp, int startingHandSize, CommanderData cData, int playerId, int abilityChargeCost, GameConstants.CommanderAbilityTargetType abilityTargetType) {
        //Data members
        this.commanderName = name;
        this.commanderPrefab = prefab;
        this.selectedCommanderPrefabPath = selectedCommanderPrefabPath;
        this.hp = startingHp;
        this.handSize = startingHandSize;
        this.playerId = playerId;
        this.commanderData = cData;
        this.abilityChargeCost = abilityChargeCost;
        this.abilityTargetType = abilityTargetType;
    }

    public void SetCommanderAttributes( CommanderData data) {
        //Data members
        this.commanderName = data.CharName;
        this.selectedCommanderPrefabPath = data.PrefabPath;
        this.hp = data.HP;
        this.handSize = data.CurrentHandSize;
        this.abilityChargeCost = data.AbilityChargeCost;
        this.abilityChargeType = data.AbilityChargeType;
        this.commanderData = data;
        this.abilityTargetType = data.AbilityTargetType;

        Debug.Log("(2) SetCommanderAttributes called by " + data.CharName);
    }

    public void InitializeSelectUI() {

    }

    /// <summary>
    /// Commander overrides this with method calls, such as registering for events
    /// </summary>
    public virtual void OnBattleStart() {
        //Get generic references to UI that all commanders will need
    }

    /// <summary>
    /// Override this event in commander or unit when listening to turn star event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public virtual void EventStartTurn(object sender, EventArgs e, int playerId) {

    }


}
