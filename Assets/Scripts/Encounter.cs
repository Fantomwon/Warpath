using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour 
{
    public CommanderData enemyCommanderData;
    public List<string> units = new List<string>();

    public void AddUnits(string unit, int amount) {
        for (int i = 0; i < amount; i++) {
            units.Add(unit);
        }
    }

    public void SetUnits() {
        foreach (string unit in units) {
            GlobalObject.instance.player2DeckSelect.Add(unit);
        }
    }

}