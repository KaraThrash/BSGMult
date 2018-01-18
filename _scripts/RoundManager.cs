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
    public int cylonsKilledThisRoundByLocalPlayer;
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
        cylonsKilledThisRoundByLocalPlayer = 0;
        objectiveTextObj.active = true;
        cylonKillBonus = 1.0f;
        fuelBonus = 1.0f;
        foodBonus = 1.0f;
        dontDieBonus = 1.0f;
        currentRound++;
        playerObjective.text = ObjectiveList(Random.Range(0, 4));
        teamObjective.text = ObjectiveList(4 % currentRound);
        
        //transform.parent.gameObject.GetComponent<GameManager>().crisisManager.GetComponent<PhotonView>().RPC("AttackingBaseStar", PhotonTargets.AllViaServer);

    }
    public void CalculatePoints()
    {
        if (fleet.GetComponent<Fleet>().fuel - fuelAtRoundStart > 0)
        { pointsThisRound += (fleet.GetComponent<Fleet>().fuel - fuelAtRoundStart) * fuelBonus; }
        if (fleet.GetComponent<Fleet>().food - foodAtRoundStart > 0)
        { pointsThisRound += (fleet.GetComponent<Fleet>().food - foodAtRoundStart) * foodBonus; }
        
        if (wasFrakked == false) { pointsThisRound += (5 * dontDieBonus); }
        pointsThisRound += (cylonsKilledThisRound * cylonKillBonus);
        totalPoints = localPlayer.GetComponent<PlayerMain>().score + pointsThisRound;
        //totalPoints += pointsThisRound;
        pointTotal.text = totalPoints.ToString();
 
        localPlayer.GetComponent<PhotonView>().RPC("UpdateScore", PhotonTargets.AllViaServer, totalPoints);
    }

    public string ObjectiveList(int newRoundVariable)
    {
        string newObj = "Don't Get Frakked";
        switch (newRoundVariable)
        {
            case 1:
                newObj = "Frak Cylon Raiders";
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
    public void CylonKilled(int type,int byWho)
    {
        cylonsKilledThisRound++;
        if (byWho == localPlayer.GetComponent<PhotonView>().ownerId)
        { cylonsKilledThisRoundByLocalPlayer++; }
    }

}
