using System;

public static class GameConstants
{
    public const string RESOURCE_PATH_PREFIX_COMMANDERS = "PrefabsCommanders/";
    //Scene Indices
    public const int SCENE_INDEX_COMMANDER_SELECT = 6;

    //In game factions. Certain cards are surfaced during selection process based on the selected Commander's faction
    [Serializable]
    public enum FactionType {Holy,Undead,Druid }


    //Tags
    public const string TAG_COMMANDER_SELECT_UI_MANAGER = "commanderSelectUIManager";

    //UI Offsets
    public const float SELECTED_COMMANDER_IMAGE_OFFSET_POS_X = 100;
    public const float SELECTED_COMMANDER_IMAGE_OFFSET_POS_Y = 100;
}
