using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoundManager : MonoBehaviour {
    public GameObject fleet;
    public GameObject localPlayer;

    public int currentRound;
    public GameObject objectiveTextObj;
    public Text playerObjective;
    public Text teamObjective;
    public Text cylonObjective;
    public Text humanObjective;
    public Text pointTotal;

    public int fuelAtRoundStart;
    public int foodAtRoundStart;
    public int cylonsKilledThisRound;
    public bool wasFrakked;

    public float currentPoints;
    public float pointsThisRound;
    public float totalPoints;

    public float fuelBonus;
    public float foodBonus;
    public float dontDieBonus;
    public float cylonKillBonus;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewRound()
    {
        CalculatePoints();
        pointsThisRound = 0;
        fuelAtRoundStart = fleet.GetComponent<Fleet>().fuel;
        foodAtRoundStart = fleet.GetComponent<Fleet>().food;
        wasFrakked = false;
        cylonsKilledThisRound = 0;
        objectiveTextObj.active = true;
        cylonKillBonus = 1.0f;
        fuelBonus = 1.0f;
        foodBonus = 1.0f;
        dontDieBonus = 1.0f;

        playerObjective.text = ObjectiveList();
        teamObjective.text = ObjectiveList();
        currentRound++;
    }
    public void CalculatePoints()
    {
        pointsThisRound += (fleet.GetComponent<Fleet>().fuel - fuelAtRoundStart) * fuelBonus;
        pointsThisRound += (fleet.GetComponent<Fleet>().food - foodAtRoundStart) * foodBonus;
        if (wasFrakked == false) { pointsThisRound += (5 * dontDieBonus); }
        pointsThisRound += (cylonsKilledThisRound * cylonKillBonus);
        totalPoints += pointsThisRound;
        pointTotal.text = totalPoints.ToString();
 
        localPlayer.GetComponent<PhotonView>().RPC("UpdateScore", PhotonTargets.AllViaServer, totalPoints);
    }

    public string ObjectiveList()
    {
        string newObj = "Don't Get Frakked";
        switch (Random.Range(0,4))
        {
            case 1:
                newObj = "Frak Cylons";
                cylonKillBonus = 1.2f;
                break;
            case 2:
                newObj = "Get Fuel";
                fuelBonus = 1.2f;
                break;
            case 3:
                newObj = "Get Food";
                foodBonus = 1.2f;
                break;
           
            default:
                newObj = "Don't Get Frakked";
                dontDieBonus = 1.2f;
                break;
        }
        return newObj;
    }
}
