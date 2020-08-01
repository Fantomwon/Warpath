using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour {

    public GameObject templateCommanderSelected;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public List<Commander> SetSelectedCommanderBattleImages() {
        List<Commander> commanders = new List<Commander>();

        Debug.Log("BATTLE UI MANAGER! ATTEMPTING TO SET COMMANDER IMAGES!!");
        /* Setting commander images for player 1 */

        //Get reference to player's commander data
        CommanderData playerCommanderData = GlobalObject.instance.selectedCommanderData;
        CommanderData enemyCommanderData = GlobalObject.instance.enemyCommanderData;

        //Load corresponding prefab based on path in the player's commander data
        GameObject playerCommanderPrefab = Resources.Load<GameObject>(playerCommanderData.PrefabPath);
        Debug.Log("BATTLE UI MANAGER: Prefab path is= " + playerCommanderData.PrefabPath);
        //Instantiate UI template object for player 1's commander
        GameObject playerCommanderInstance = Instantiate(playerCommanderPrefab) as GameObject;
        Commander playerCommanderScript = playerCommanderInstance.GetComponent<Commander>();
        playerCommanderScript.SetCommanderAttributes(playerCommanderData);
        commanders.Add(playerCommanderScript);
        //Parent newly created prefab to UI element for positioning
        playerCommanderInstance.transform.SetParent(GameObject.Find("PanelCommanderBattleP1/Image").transform, false);
        //NOTE! Had previously attempted to set the Commander UI panel variable reference in here for each commander but for some reason it ends up being null after Global Object gets the list back

        /*Setting commander images for player 2 (Enemy)*/
        GameObject enemyCommanderPrefab = Resources.Load<GameObject>(enemyCommanderData.PrefabPath);
        Debug.Log("BATTLE UI MANAGER: Prefab path is= " + enemyCommanderData.PrefabPath);
        //Instantiate UI template object for player 1's commander
        GameObject enemyCommanderInstance = Instantiate(enemyCommanderPrefab) as GameObject;
        Commander enemyCommanderScript = enemyCommanderInstance.GetComponent<Commander>();
        //Set the necessary data
        enemyCommanderScript.SetCommanderAttributes(enemyCommanderData);
        commanders.Add(enemyCommanderScript);
        //Parent newly created prefab to UI element for positioning
        enemyCommanderInstance.transform.SetParent(GameObject.Find("PanelCommanderBattleP2/Image").transform, false);
        //flip sprite
        Transform enemyCommanderSprite = enemyCommanderInstance.transform.Find("Hero");
        enemyCommanderSprite.localScale = new Vector3(enemyCommanderSprite.transform.localScale.x * -1, enemyCommanderSprite.transform.localScale.y, enemyCommanderSprite.transform.localScale.z);
        Debug.LogWarning("BATTLE UI MANAGER! SetSelectedCommanderBattleImages called! passing back list of commanders with length: " + commanders.Count);
        return commanders;
    }

    public void SetCommanderUIPanel( ref Commander commander) {
        if( commander.playerId == GameConstants.HUMAN_PLAYER_ID) {
            //Get component for matching UI script and cache it with the PlayerCommanderInstance for Player 1
            CommanderUIPanel commanderUIPanelScript = GameObject.Find("PanelCommanderBattleP1").GetComponent<CommanderUIPanel>();
            commander.commanderUIPanel = commanderUIPanelScript;
        } else if( commander.playerId == GameConstants.ENEMY_PLAYER_ID) {
            //Get component for matching UI script and cache it with the PlayerCommanderInstance for Player 1
            CommanderUIPanel commanderUIPanelScript = GameObject.Find("PanelCommanderBattleP2").GetComponent<CommanderUIPanel>();
            commander.commanderUIPanel = commanderUIPanelScript;
        }
    }
}
