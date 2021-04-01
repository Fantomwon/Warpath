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
    public int manaPerTurn;
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

    public virtual void TakeDamage(int damageAmount) {
        this.hp = Math.Max(0, this.hp - damageAmount);
    }

    public virtual bool ActivateCommanderAbility(Hero heroTarget) {
        return true;
    }

    /// <summary>
    /// Override this method for custom behavior that charges the commander's ability
    /// </summary>
    public virtual void IncreaseCommanderAbilityCharge() {
        this.currentAbilityCharge++;
        this.commanderUIPanel.SetCommanderResourceText(this.currentAbilityCharge);
        this.CheckActivateCommanderButton();
    }

    /// <summary>
    /// Method that checks if charge threshold has been hit for commander ability and then sets ui element active.
    /// Override for custom behavior
    /// </summary>
    public virtual void CheckActivateCommanderButton() {
        if (this.currentAbilityCharge >= this.abilityChargeCost) {
            //Activate UI element - This probably needs to be a prefab or part of this prefab
            this.commanderUIPanel.SetCommanderAbilityButtonActive(true);
        }
    }

    public void SetCommanderAttributes(string name, GameObject prefab, string selectedCommanderPrefabPath, int startingHp, int startingHandSize, CommanderData cData, int playerId, int abilityChargeCost, GameConstants.CommanderAbilityTargetType abilityTargetType, int manaPerTurn) {
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
        this.manaPerTurn = manaPerTurn;
    }

    public void SetCommanderAttributes(CommanderData data) {
        //Data members
        this.commanderName = data.CharName;
        this.selectedCommanderPrefabPath = data.PrefabPath;
        this.hp = data.HP;
        this.handSize = data.CurrentHandSize;
        this.abilityChargeCost = data.AbilityChargeCost;
        this.abilityChargeType = data.AbilityChargeType;
        this.commanderData = data;
        this.abilityTargetType = data.AbilityTargetType;
        this.manaPerTurn = data.ManaPerTurn;
        Debug.Log("(2) SetCommanderAttributes called by " + data.CharName);
    }

    public void InitializeSelectUI() {

    }



    /********** Virtual methods for override **********/

    /// <summary>
    /// Commander overrides this with method calls, such as registering for events
    /// </summary>
    public virtual void OnBattleStart() {
        //Get generic references to UI that all commanders will need
    }

    /********** Event listening methods for override **********/

    /// <summary>
    /// Override this event in commander or unit when listening to turn start event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public virtual void EventStartTurn(object sender, EventArgs e, int playerId) {

    }

    /// <summary>
    /// Override this event in commander or unit when listening to event fired when a unit receives damage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="playerId"></param>
    public virtual void EventUnitReceiveDamage(object sender, EventArgs e, int playerId, Hero damageReceiver) {

    }

    /// <summary>
    /// Override this event in commander or unit when listening to event fired when a unit is summoned
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="playerId"></param>
    /// <param name="summonedUnitCard"></param>
    public virtual void EventUnitSummoned(object sender, EventArgs e, int playerId, Hero summonedHero) {

    }

}
