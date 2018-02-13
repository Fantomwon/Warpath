using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private static string currentLevel;

	public void LoadLevel(string name){
		//Debug.Log ("Level load requested for "+ name);
		SceneManager.LoadScene(name);
		SetCurrentLevel();
	}
 	
 	public void QuitRequest(){
 		Debug.Log ("Quit Request sent");
 	}
 	
 	public void LoadNextLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//		SetCurrentLevel();
 	}
 	
 	public void SetCurrentLevel(){
		PlayerPrefs.SetString("Current Level", SceneManager.GetActiveScene().name);
		currentLevel = PlayerPrefs.GetString ("Current Level");
	}
 	
 	public void RestartLevel() {
		SceneManager.LoadScene (currentLevel);
 	}
} 