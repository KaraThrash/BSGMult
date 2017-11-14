using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterShipList : MonoBehaviour {
    public GameObject galactica;
    public GameObject baseStar;
    public GameObject fleet;
    public GameObject space;

    public GameObject peopleInGalactica;
    public GameObject peopleInBaseStar;
    public GameObject peopleInFleet;
    public GameObject peopleInSpace;

    public GameObject galacticaHangar;
    public GameObject baseStarHangar;
    public GameObject fleetHangar;
    public GameObject spaceHangar;

    public List<GameObject> hangars = new List<GameObject>();
    public List<GameObject> shipInteriors = new List<GameObject>();
    public List<GameObject> largeShips = new List<GameObject>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject ParentFighterToShip(int shipNUmberInList)
    {
        //GameObject tempfighterObject;
        //switch (shipNUmberInList)
        //{
        //    case 1:
        //        tempfighterObject = galacticaHangar;
        //        break;
        //    case 2:
        //        tempfighterObject = fleetHangar;
        //        break;
        //    case 3:
        //        tempfighterObject = baseStarHangar;
        //        break;
          
        //    default:
        //        tempfighterObject = space;
        //        break;
        //}
       // return tempfighterObject;
        if (hangars[shipNUmberInList] != null)
        { return hangars[shipNUmberInList]; }
        else { return hangars[0]; }

    }
    public GameObject ParentHumanToShip(int shipNUmberInList)
    {
        //GameObject tempHumanObject;
        //switch (shipNUmberInList)
        //{
        //    case 1:
        //        tempHumanObject = peopleInGalactica;
        //        break;
        //    case 2:
        //        tempHumanObject = peopleInFleet;
        //        break;
        //    case 3:
        //        tempHumanObject = peopleInBaseStar;
        //        break;

        //    default:
        //        tempHumanObject = peopleInSpace;
        //        break;
        //}
        //return tempHumanObject;
        if (shipInteriors[shipNUmberInList] != null)
        { return shipInteriors[shipNUmberInList]; }
        else { return shipInteriors[0]; }

    }
}
