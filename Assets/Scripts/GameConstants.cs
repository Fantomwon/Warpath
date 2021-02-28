using System;

public static class GameConstants
{
    public const string RESOURCE_PATH_PREFIX_COMMANDERS = "PrefabsCommanders/";

    //Scene Indices
    public const int SCENE_INDEX_BOSS_SELECT = 3;
    public const int SCENE_INDEX_CARD_SELECT_SINGLE_PLAYER = 4;
    public const int SCENE_INDEX_COMMANDER_SELECT = 6;
    public const int SCENE_INDEX_GAME_COMMANDERS = 7;
    public const int SCENE_INDEX_MAP = 8;
    public const int SCENE_INDEX_POST_BATTLE_CARD_SELECT = 9;

    //In game factions. Certain cards are surfaced during selection process based on the selected Commander's faction
    [Serializable]
    public enum FactionType {Holy, Undead, Druid};

    //Commander ability charge type. Will determine which combat events charge the commander's ability
    [Serializable]
    public enum CommanderAbilityChargeType {StartTurn, UnitDeath, UnitReceiveDamage};

    //Commander ability target type. Will determine which units are affected by a commander's ability on activation
    [Serializable]
    public enum CommanderAbilityTargetType { Enemy, Ally };

    //List of cards for use with creating enemy encounters and nodes, etc.
    [Serializable]
    public enum Card {
    wall,
    archer,
    footsoldier,
    dwarf,
    ghost,
    sapper,
    rogue,
    druid,
    monk,
    slinger,
    bloodmage,
    blacksmith,
    wolf,
    chaosmage,
    sorcerer,
    assassin,
    knight,
    paladin,
    tower,
    champion,
    cavalry,
    diviner,
    crossbowman,
    bloodknight,
    cultinitiate,
    cultadept,
    cultacolyte,
    cultsentinel,
    cultfanatic,
    armor, //Start spells
	rockthrow,
	root,
	windgust,
	heal,
	shroud,
	might,
	fireball,
    spellcardforced,
    drainlife
    }

    //Tags
    public const string TAG_COMMANDER_SELECT_UI_MANAGER = "commanderSelectUIManager";

    //UI Offsets
    public const float SELECTED_COMMANDER_IMAGE_OFFSET_POS_X = 100;
    public const float SELECTED_COMMANDER_IMAGE_OFFSET_POS_Y = 100;

    //Player Ids
    public const int UNSET_PLAYER_ID = -1;
    public const int HUMAN_PLAYER_ID = 0;
    public const int ENEMY_PLAYER_ID = 1;
}
