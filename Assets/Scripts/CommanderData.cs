using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommanderData
{
	public CommanderData()
	{

	}

    public CommanderData( string characterName, GameConstants.FactionType faction, int commanderHP, int startingHandSize, string prefabResourcePath, GameConstants.CommanderAbilityChargeType abilityChargeType, int abilityChargeCost, GameConstants.CommanderAbilityTargetType abilityTargetType) {
        this._charName = characterName;
        this._faction = faction;
        this._hp = commanderHP;
        this._startingHandSize = startingHandSize;
        this._prefabPath = prefabResourcePath;
        //Initialize other values
        this._currentHandSize = this._startingHandSize;
        this._maxHp = this._hp;
        this._abilityChargeType = abilityChargeType;
        this._abilityChargeCost = abilityChargeCost;
        this._abilityTargetType = abilityTargetType;
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
    private GameConstants.FactionType _faction;

    /// <summary>
    /// Gets the commander's faction. Used for determining which cards to supply in certain contexts.
    /// </summary>
    public GameConstants.FactionType Faction {
        get {
            return this._faction;
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
