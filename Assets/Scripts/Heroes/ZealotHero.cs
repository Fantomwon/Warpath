using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        //if summoned unit has the same owning player as this soldier
        if(this.playerId == playerId) {
            Debug.Log("ZEALOT: Hero had the same owning player~~~");
            //Then check if it was summoned in a cardinal direction to this hero (adjacent)
            PlayField playField = FindObjectOfType<PlayField>();
            List<Transform> adjacentAllies = playField.AdjacencyCheckCardinalDirections(this.GetComponent<Transform>(), "ally");
            foreach (Transform t in adjacentAllies) {
                Debug.Log("ZEALOT:checkin transform loop");
                if (t == summonedHero.GetComponent<Transform>()) {
                    Debug.Log("ZEALOT: Hero was adjacent!!!!");
                    this.power += 1;
                    //TODO: Replace this with other particle
                    GameObject spawnedParticle = Instantiate(dodgeParticleGameObject, this.GetComponent<Transform>().position, Quaternion.identity);
                    break;
                }
            }
        }
    }

    public override void OnDeathEffects() {
        //Unsubscribe from events to allow the object to be garbage collected and avoid null references
        BattleEventManager._instance.UnregisterForEvent(BattleEventManager.EventType.UnitSummoned, this);
    }
}
