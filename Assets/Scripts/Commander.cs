using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    public string commanderName;
    public GameObject commanderPrefab;
    public string selectedCommanderPrefabPath;
    public Sprite commanderAbilitySprite;
    public int hp;
    public int handSize;
    public GameConstants.FactionType faction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ActivateCommanderAbility() {
        return true;
    }

    public void SetCommanderAttributes( string name, GameObject prefab, string selectedCommanderPrefabPath, int startingHp, int startingHandSize ) {
        //Data members
        this.commanderName = name;
        this.commanderPrefab = prefab;
        this.selectedCommanderPrefabPath = selectedCommanderPrefabPath;
        this.hp = startingHp;
        this.handSize = startingHandSize;
    }

    public void InitializeSelectUI() {

    }
}
