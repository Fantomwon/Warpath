using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonScript : MonoBehaviour
{
    public Button uiButtonScript;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize reference to button script
        //if( this.uiButtonScript == null) {
        //    this.uiButtonScript = this.GetComponent<Button>();
        //    //Ensure level load fuction is set
        //    if( this.uiButtonScript.onClick == null) {
        //        Debug.LogWarning("NEXT BUTTON FUCTION MISSING!");
        //    }
        //    this.uiButtonScript.onClick.AddListener( delegate { LevelManager.instance.LoadLevel(GameConstants.SCENE_INDEX_CARD_SELECT_SINGLE_PLAYER); } );
        //}    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
