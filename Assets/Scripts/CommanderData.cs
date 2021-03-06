﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommanderData {
    public CommanderData() {

    }

    public CommanderData(string characterName, GameConstants.CardCommanderType cardCommanderType, int commanderHP, int startingHandSize, string prefabResourcePath, GameConstants.CommanderAbilityChargeType abilityChargeType, int abilityChargeCost, GameConstants.CommanderAbilityTargetType abilityTargetType, List<CardEntry> deck) {
        this._charName = characterName;
        this._cardCommanderType = cardCommanderType;
        this._hp = commanderHP;
        this._startingHandSize = startingHandSize;
        this._prefabPath = prefabResourcePath;
        //Initialize other values
        this._currentHandSize = this._startingHandSize;
        this._maxHp = this._hp;
        this._abilityChargeType = abilityChargeType;
        this._abilityChargeCost = abilityChargeCost;
        this._abilityTargetType = abilityTargetType;
        this._deck = deck;
    }

    [SerializeField]
    public List<CardEntry> _deck;


    public List<CardEntry> Deck {
        get{
            return this._deck;
        }
        set {
            this._deck = value;
        }
    }


    [SerializeField]
    private string _charName;

    public string CharName {
        get {
            return this._charName;
        }
        set {
            this._charName = value;
        }
    }

    /// <summary>
    /// The commander's faction. Determines what faction cards are supplied
    /// </summary>
    [SerializeField]
    private GameConstants.CardCommanderType _cardCommanderType;

    /// <summary>
    /// Gets the commander's faction. Used for determining which cards to supply in certain contexts.
    /// </summary>
    public GameConstants.CardCommanderType CardCommanderType {
        get {
            return this._cardCommanderType;
        }
    }

    /// <summary>
    /// The commander's ability charge type. Determines what events charge the commander's ability. 
    /// </summary>
    [SerializeField]
    private GameConstants.CommanderAbilityChargeType _abilityChargeType;

    /// <summary>
    /// Gets the commander's charge type. Used for determining which events charge the commander's ability.
    /// </summary>
    public GameConstants.CommanderAbilityChargeType AbilityChargeType {
        get {
            return this._abilityChargeType;
        }
    }

    /// <summary>
    /// The commander's ability charge type. Determines what events charge the commander's ability. 
    /// </summary>
    [SerializeField]
    private GameConstants.CommanderAbilityTargetType _abilityTargetType;

    /// <summary>
    /// Gets the commander's charge type. Used for determining which events charge the commander's ability.
    /// </summary>
    public GameConstants.CommanderAbilityTargetType AbilityTargetType {
        get {
            return this._abilityTargetType;
        }
    }

    [SerializeField]
    private int _abilityChargeCost;

    public int AbilityChargeCost {
        get {
            return this._abilityChargeCost;
        }
    }

    /// <summary>
    /// The commander's current health. If reduced to 0, the character dies.
    /// </summary>
    [SerializeField]
    private int _hp;

    /// <summary>
    /// Gets or sets the character's health.
    /// </summary>
    /// <value>The Health.</value>
    public int HP {
        get {
            return this._hp;
        }
        set {
            this._hp = value;
        }
    }

    /// <summary>
    /// The character's current maximum health. Current health value can't be increased beyond this.
    /// </summary>
    [SerializeField]
    private int _maxHp;

    /// <summary>
    /// Gets or sets the commander's maximum health. Current health value can't be increased beyond this.
    /// </summary>
    /// <value>The Health.</value>
    public int MaxHP {
        get {
            return this._hp;
        }
        set {
            this._hp = value;
        }
    }

    /// <summary>
    /// The commander's health. If reduced to 0, the character dies.
    /// </summary>
    [SerializeField]
    private int _startingHandSize;

    /// <summary>
    /// Gets the commander's starting hand size
    /// </summary>
    /// <value>The Health.</value>
    public int StartingHandSize {
        get {
            return this._startingHandSize;
        }
    }

    /// <summary>
    /// The commander's health. If reduced to 0, the character dies.
    /// </summary>
    [SerializeField]
    private int _currentHandSize;

    /// <summary>
    /// Gets the commander's starting hand size
    /// </summary>
    /// <value>The Health.</value>
    public int CurrentHandSize {
        get {
            return this._currentHandSize;
        }
        set {
            this._currentHandSize = value;
        }
    }

    /// <summary>
    /// The commander's mana gained per turn.
    /// </summary>
    [SerializeField]
    private int _manaPerTurn;

    /// <summary>
    /// Property for amount of mana this commander generates each turn.
    /// </summary>
    public int ManaPerTurn {
        get {
            return this._manaPerTurn;
        }
        set {
            this._manaPerTurn = value;
        }
    }

    /// <summary>
    /// String to this commander's prefab asset. Used for loading resources at runtime
    /// </summary>
    [SerializeField]
    private string _prefabPath;

    /// <summary>
    /// Gets or sets the string path to this hero's prefab asset
    /// </summary>
    /// <value> The string value for the path to the resource </value>
    public string PrefabPath {
        get {
            return this._prefabPath;
        }
        set {
            this._prefabPath = value;
        }
    }
}

[System.Serializable]
public class CardEntry {
    public CardEntry( GameConstants.Card card, int cardAmount) {
        Card = card;
        CardAmount = cardAmount;
    }

    public CardEntry(GameConstants.Card card, int cardAmount, GameConstants.CardCommanderType commanderType) {
        Card = card;
        CardAmount = cardAmount;
        CommanderType = commanderType;
    }

    [SerializeField]
    public GameConstants.CardCommanderType CommanderType;

    [SerializeField]
    public GameConstants.Card Card;

    [SerializeField]
    public int CardAmount;
}