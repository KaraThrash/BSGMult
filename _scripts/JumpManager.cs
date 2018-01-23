using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : Photon.PunBehaviour
{
    public GameObject roundManager;
    public GameObject gameManager;
    public List<GameObject> Humanoids = new List<GameObject>();
    public List<GameObject> CapitalShips = new List<GameObject>();
    public List<GameObject> jumpableGameObjects = new List<GameObject>();
    public GameObject galactica;
    public int galacticaCoordinates;
    public GameObject baseStar;
    public GameObject baseStarShip;
    public int baseStarCoordinates;
    public GameObject fleet;
    public int fleetCoordinates;
    public int localPlayerCords;
    public GameObject localPlayer;
    public Transform activePersistantFighters; //fighters in space when jumps are made
    public GameObject activeHumanFleet;
    public GameObject fleetLeftBehind;
    public GameObject cylonFleetLeftBehind;
    public GameObject activeCylonFleet; //currently in Galactica space space
    public GameObject otherCylonFleet;// not currently in Galactica space space
                                      //ShipGroup; 0:space/planet //1: galactica + active fleet 2: Cylon fleet 3:  leftbehind  ??4: scouting??

    public float timeSinceLastJump;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (baseStar.active == false)
        { timeSinceLastJump += Time.deltaTime; }
        
        if (timeSinceLastJump > 20.0f) { timeSinceLastJump = 0; BaseStarJump(); }
        if (localPlayer == null) { localPlayer = gameManager.GetComponent<GameManager>().localPlayer; }
       // if (Input.GetKeyUp(KeyCode.Y)) { PhotonNetwork.InstantiateSceneObject("humanBullet", Vector3.zero, Quaternion.identity, 0, null); }

    }

    public void ManageJump(int galacticaCord,int fleetCord,int basestarCord,int localCord)
    {
        //if (localPlayer != null)
        //{
        //    if (localCord != 0) { localPlayerCords = localCord; } else
        //    { localPlayerCords = localPlayer.GetComponent<PlayerMain>().spaceCoordinates; }

        //    if (galacticaCord != 0) { galacticaCoordinates = galacticaCord; }
        //    if (basestarCord != 0) { baseStarCoordinates = basestarCord; }
        //    if (fleetCord != 0) { fleetCoordinates = fleetCord; }

        //    if (baseStarCoordinates == localPlayerCords) { baseStarShip.active = true; } else { baseStarShip.active = false; }
        //    if (galacticaCoordinates == localPlayerCords) { galactica.active = true; } else { galactica.active = false; Debug.Log("Turn Galactica Off: " + galacticaCoordinates + localPlayerCords); }
        //        //fleet.GetComponent<Fleet>().CheckAllShipsLocation(localPlayerCords);

        //    foreach (Transform fighter in activePersistantFighters) {
        //        if (fighter.GetComponent<Fighter>().currentCords == localPlayerCords) { fighter.gameObject.active = true; }
        //        else { fighter.gameObject.active = false; }
        //    }
        //}

    }

    public void GalacticaJumped(int newScene) //new round
    {
        timeSinceLastJump = 0;
        galacticaCoordinates = newScene;
        baseStar.GetComponent<PhotonView>().RPC("StartFTL", PhotonTargets.AllViaServer);
        //TODO: all the cylons go away, and the basestar does the clean up when it jumps so there is no reason to pick and choose the cylon ship parents
        //foreach (Transform child in activeCylongFleet.transform)
        //{ child.parent = cylonFleetLeftBehind.transform; }



        //note: anything 2 jumps behind is destroyed
        fleet.GetComponent<Fleet>().CheckAllShipsLocation();
        activeHumanFleet.active = true;
        fleetLeftBehind.active = true;
        if (localPlayer != null)
        {
            if (localPlayer.GetComponent<PlayerMain>().shipGroup != 1 && localPlayer.GetComponent<PlayerMain>().humanoidObject.GetComponent<PlayerCharacter>().flying == false)
            {
                localPlayer.GetComponent<PlayerMain>().humanoidObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 99, 0);
            }

        }
        foreach (Transform child in activePersistantFighters.transform)
        {
          
            child.GetComponent<Fighter>().Die();

        }

       

        

            gameManager.GetComponent<GameManager>().NewRound();
        //roundManager.GetComponent<RoundManager>().NewRound();
        if (localPlayer != null)
        {
                galactica.active = true;
                cylonFleetLeftBehind.active = false;
                baseStarShip.active = false;

                activeCylonFleet.active = false;
                fleetLeftBehind.active = false;
             
            
        }
    }

    public void BaseStarJump() //new round
    {



        baseStarShip.active = true;
        activeCylonFleet.active = true;
        if (localPlayer != null)
        {
            

            if (localPlayer.GetComponent<PlayerMain>().shipGroup == 2)
            {
                galactica.active = true;
                activeHumanFleet.active = true;
                fleetLeftBehind.active = false;
                cylonFleetLeftBehind.active = false;
                baseStarShip.active = true;

                //Application.LoadLevel(galacticaCoordinates.ToString());

            }
            if (localPlayer.GetComponent<PlayerMain>().shipGroup == 3)
            {
                
                activeCylonFleet.active = false;
                baseStarShip.active = false;
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
           // if (baseStarCoordinates == localPlayerCords) { baseStarShip.active = true; }
           // else { baseStarShip.active = false; }
        }
        
    }
    public void PlayerJoin (GameObject joiningPlayer)
    {
        localPlayer = joiningPlayer;
             //joiningPlayer.GetComponent<PlayerMain>().spaceCoordinates = galacticaCoordinates;

        joiningPlayer.GetComponent<PlayerMain>().Jumping(galacticaCoordinates,1);

            //if (baseStarCoordinates == galacticaCoordinates) { baseStarShip.active = true; } else { baseStarShip.active = false; }


        // if (fleetCoordinates == galacticaCoordinates) { fleet.active = true; } else { fleet.active = false; }
        fleetLeftBehind.active = false;
        Application.LoadLevel(galacticaCoordinates.ToString());
        galactica.active = true;
        //should also turn on the fighters currently docked
    }

     public bool CheckCoordinates(int fighterCords)
    {
        Debug.Log(fighterCords == localPlayerCords);
       // return fighterCords == localPlayerCords;

        //If
        return galactica.active;
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
