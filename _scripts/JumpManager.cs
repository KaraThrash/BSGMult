using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : Photon.PunBehaviour
{
    public List<GameObject> Humanoids = new List<GameObject>();
    public List<GameObject> CapitalShips = new List<GameObject>();
    public List<GameObject> jumpableGameObjects = new List<GameObject>();
    public GameObject galactica;
    public int galacticaCoordinates;
    public GameObject baseStar;
    public int baseStarCoordinates;
    public GameObject fleet;
    public int fleetCoordinates;
    public int localPlayerCords;
    public GameObject localPlayer;
    public Transform activePersistantFighters; //fighters in space when jumps are made
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Y)) { PhotonNetwork.InstantiateSceneObject("humanBullet", Vector3.zero, Quaternion.identity, 0, null); }

    }
    public void ManageJump(int galacticaCord,int fleetCord,int basestarCord,int localCord)
    {
        if (localPlayer != null)
        {
            // int localPlayerSpaceCoordinates = localPlayer.GetComponent<PlayerMain>().spaceCoordinates;
            if (localCord != 0) { localPlayerCords = localCord; } else
            { localPlayerCords = localPlayer.GetComponent<PlayerMain>().spaceCoordinates; }
         
            //for (int i = jumpableGameObjects.Count; i > 0; --i)
            //{
            //    jumpableGameObjects[i]
            //        if(jumpableGameObjects[i])

            //}
            if (galacticaCord != 0) { galacticaCoordinates = galacticaCord; }
            if (basestarCord != 0) { baseStarCoordinates = basestarCord; }
            if (fleetCord != 0) { fleetCoordinates = fleetCord; }

            if (baseStarCoordinates == localPlayerCords) { baseStar.active = true; } else { baseStar.active = false; }
            if (galacticaCoordinates == localPlayerCords) { galactica.active = true; } else { galactica.active = false; Debug.Log("Turn Galactica Off: " + galacticaCoordinates + localPlayerCords); }

            if (fleetCoordinates == localPlayerCords) { fleet.active = true; } else { fleet.active = false; }
            foreach (Transform fighter in activePersistantFighters) {
                if (fighter.GetComponent<Fighter>().currentCords == localPlayerCords) { fighter.gameObject.active = true; }
                else { fighter.gameObject.active = false; }
            }
        }

    }
    [PunRPC]
    public void UpdateLocationGalactica(int newGalacticaCordinates)
    {
        galacticaCoordinates = newGalacticaCordinates;

       // if (galacticaCoordinates == localPlayerCords) { galactica.active = true; } else { galactica.active = false; }

    }
    [PunRPC]
    public void UpdateLocationFleet(int newFleetCordinates)
    {
        fleetCoordinates = newFleetCordinates;

    }
    [PunRPC]
    public void UpdateLocationBaseStar(int newBaseStarCordinates)
    {
        baseStarCoordinates = newBaseStarCordinates;
        if (localPlayer != null) {
            //make sure to check for local player otherwise the server turns the basestar off
            if (baseStarCoordinates == localPlayerCords) { baseStar.active = true; }
            else { baseStar.active = false; }
        }
        
    }
    public void PlayerJoin (GameObject joiningPlayer)
    {
        localPlayer = joiningPlayer;
             //joiningPlayer.GetComponent<PlayerMain>().spaceCoordinates = galacticaCoordinates;

        joiningPlayer.GetComponent<PlayerMain>().Jumping(galacticaCoordinates);

            if (baseStarCoordinates == galacticaCoordinates) { baseStar.active = true; } else { baseStar.active = false; }
           

            if (fleetCoordinates == galacticaCoordinates) { fleet.active = true; } else { fleet.active = false; }

        galactica.active = true;
        //should also turn on the fighters currently docked
    }

     public bool CheckCoordinates(int fighterCords)
    {
        Debug.Log(fighterCords == localPlayerCords);
        return fighterCords == localPlayerCords;
    }
    public bool CheckCoordinatesBaseStarForFleetGalactica()
    {
        if (baseStarCoordinates == fleetCoordinates || baseStarCoordinates == galacticaCoordinates)
        { return true; }
        else { return false; }

    }



    void Awake()
    {

        //DontDestroyOnLoad(this.gameObject);
    }
}
