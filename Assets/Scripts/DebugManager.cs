using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

    public static DebugManager _instance;

    private readonly int startMenuSceneIndex = 0;
    private readonly bool printScene = false;
    private readonly string debugMenuName = "DebugMenu";

    private Canvas canvas = null;
    private GameObject debugMenu = null;

	// Use this for initialization
	void Start () {
        //Singleton
        if( DebugManager._instance != null && DebugManager._instance != this) {
            Destroy(this.gameObject);
        } else {
            DebugManager._instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Cache reference to graphic raycaster script
            if( DebugManager._instance.canvas == null) {
                DebugManager._instance.canvas = this.GetComponent<Canvas>();
                if(DebugManager._instance.canvas.worldCamera == null) {
                    DebugManager._instance.canvas.worldCamera = Camera.main;
                }

                if(DebugManager._instance.debugMenu == null) {
                    DebugManager._instance.debugMenu = DebugManager._instance.transform.Find(this.debugMenuName).gameObject;
                }
            }
            //Set inactive to start
            DebugManager._instance.ToggleDebugMenu();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyUp( KeyCode.BackQuote ) ) {
            DebugManager._instance.ToggleDebugMenu();
        }else if( Input.GetKeyUp( KeyCode.Alpha1 )) {//Test loading scene of post battle with card select
            Debug.LogWarning("DEBUG KEY CODE ALPHA 1 PRESSED!");
            //Set necessary data for the Card Select Manager
            //List<string> testSelectCardsList = new List<string>() {"cultadept", "cultinitiate", "cultsentinel", "cultacolyte", "cultfanatic", "rockthrow", "fireball" };
            List<string> testSelectCardsList = new List<string>() { "cultadept","cultsentinel", "cultacolyte" };
            CardSelectManager.instance.SetCardsForSelect( testSelectCardsList );
            SceneManager.LoadScene(GameConstants.SCENE_INDEX_POST_BATTLE_CARD_SELECT);
        }

	}

    public void ToggleDebugMenu() {
        var activeStatus = this.debugMenu.activeInHierarchy;
        this.debugMenu.SetActive(!activeStatus);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(this.startMenuSceneIndex);
    }

    void OnEnable() {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if(DebugManager._instance != null && DebugManager._instance.printScene ) {
            Debug.Log("Level Loaded");
            Debug.Log(scene.name);
            Debug.Log(mode);
        }


        //Ensure camera is populated
        if (DebugManager._instance != null && DebugManager._instance.canvas.worldCamera == null) {
            DebugManager._instance.canvas.worldCamera = Camera.main;
        }
    }
}
