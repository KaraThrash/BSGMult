using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoundManager : MonoBehaviour {
    public int currentRound;
    public GameObject objectiveTextObj;
    public Text playerObjective;
    public Text teamObjective;
    public Text cylonObjective;
    public Text humanObjective;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewRound()
    {
        objectiveTextObj.active = true;
        playerObjective.text = ObjectiveList();
        teamObjective.text = ObjectiveList();
    }
    public string ObjectiveList()
    {
        string newObj = "Don't Get Frakked";
        switch (Random.Range(0,4))
        {
            case 1:
                newObj = "Visit the BaseStar";
                break;
            case 2:
                newObj = "Get Fuel";
                break;
            case 3:
                newObj = "Get Food";
                break;
           
            default:
                newObj = "Frak Everyone Else";
                break;
        }
        return newObj;
    }
}
