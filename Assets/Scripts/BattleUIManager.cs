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


    public void SetSelectedCommanderBattleImages() {
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
        //Parent newly created prefab to UI element for positioning
        playerCommanderInstance.transform.SetParent(GameObject.Find("PanelCommanderBattleP1/Image").transform, false);

        /* Setting commander images for player 2 (Enemy) */
        GameObject enemyCommanderPrefab = Resources.Load<GameObject>(enemyCommanderData.PrefabPath);
        Debug.Log("BATTLE UI MANAGER: Prefab path is= " + enemyCommanderData.PrefabPath);
        //Instantiate UI template object for player 1's commander
        GameObject enemyCommanderInstance = Instantiate(enemyCommanderPrefab) as GameObject;
        //Parent newly created prefab to UI element for positioning
        enemyCommanderInstance.transform.SetParent(GameObject.Find("PanelCommanderBattleP2/Image").transform, false);
        //flip sprite
        Transform enemyCommanderSprite = enemyCommanderInstance.transform.Find("Hero");
        enemyCommanderSprite.localScale = new Vector3(enemyCommanderSprite.transform.localScale.x * -1, enemyCommanderSprite.transform.localScale.y, enemyCommanderSprite.transform.localScale.z);
    }
}
