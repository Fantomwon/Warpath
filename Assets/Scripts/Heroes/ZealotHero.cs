using UnityEngine;
using System.Collections;
using System;

public class ZealotHero : Hero {

    public bool loadFromPrefab = true;
    //public string prefabPath = "PrefabsHeroes/ZealotHero";

    public override void OnSpawnEffects() {
        //Register for unit summoned event
        BattleEventManager._instance.RegisterForEvent(BattleEventManager.EventType.UnitSummoned, this);
    }

    public override void EventUnitSummoned(object sender, EventArgs e, int playerId, Hero summonedHero) {
        Debug.Log("~~~~ZEALOT HEARD A UNIT SUMMONED!");
        //if(summonedHero typ)
    }
}
