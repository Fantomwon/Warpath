using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private static string currentLevel;

    void Awake() {

    }

    public void LoadLevel(string name){
		//Debug.Log ("Level load requested for "+ name);
		SceneManager.LoadScene(name);
		SetCurrentLevel();
	}

    //TODO: This is currently where the game loads the CardSelectScene which triggers behavior for the card select manager
    public void LoadLevel(int levelIndex) {
        Debug.Log ("HELLO! Level load requested for "+ name);
        SceneManager.LoadScene(levelIndex);
        SetCurrentLevel();
    }

    public void QuitRequest(){
 		Debug.Log ("Quit Request sent");
 	}
 	
 	public void LoadNextLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//		SetCurrentLevel();
 	}

    //Load list of cards for upcoming selection
    //public void SetCardsListForSelectionScene() {
        /*load entire cards list from this point on*/
    //}
 	
 	public void SetCurrentLevel(){
		PlayerPrefs.SetString("Current Level", SceneManager.GetActiveScene().name);
		currentLevel = PlayerPrefs.GetString ("Current Level");
	}
 	
 	public void RestartLevel() {
		SceneManager.LoadScene (currentLevel);
 	}
} 