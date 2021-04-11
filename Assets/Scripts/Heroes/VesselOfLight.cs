using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VesselOfLight : Hero {

    public bool loadFromPrefab = true;

    //When summoned, this soldier will grant the player 1 Magic per adjacent unit
    public override void OnSpawnEffects() {
        //Check to see how many allies are adjacent to the grid square that this soldier was summoned
        PlayField playField = FindObjectOfType<PlayField>();
        List<Transform> adjacentAllies = playField.AdjacencyCheckCardinalDirections(this.GetComponent<Transform>(), "ally");
        int magicIncrease = 0;
        foreach (Transform t in adjacentAllies) {
            magicIncrease++;    
        }

        if(magicIncrease > 0) {
            //Adjacent allies were present so call method on playField to update the summoner's magic and UI
            playField.ModifyMana(magicIncrease, this.playerId);
            //TODO: Add specific particle here. Should also do some kind of visual flourish on magic number increasing
            GameObject spawnedParticle = Instantiate(dodgeParticleGameObject, this.GetComponent<Transform>().position, Quaternion.identity);
        }
    }

    public override void CheckForValidTargetsToAttack(Transform currentHero) {
        //base.CheckForValidTargetsToAttack(currentHero);
        if( playField.TargetCheckAllHeroesInRange(currentHero, "enemy").Count > 0 ){
            StartCoroutine("PlayAttackAnimation");
        }
    }
}
