using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommanderSelectUIManager : MonoBehaviour
{
    public GameObject commanderSelectedStatsUI;
    public GameObject textName;
    public GameObject textHpAmount;
    public GameObject textHandSizeAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedCommanderText( Commander commanderScript) {
        this.textName.GetComponent<Text>().text = commanderScript.commanderName;
        this.textHpAmount.GetComponent<Text>().text = commanderScript.hp.ToString();
        this.textHandSizeAmount.GetComponent<Text>().text = commanderScript.handSize.ToString();
    }
}
