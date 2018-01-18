using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStar : Photon.PunBehaviour
{
    public GameObject gameManager;
    public GameObject myShip;
    public int myShipGroup; //1: galactica + active fleet 2: Cylon fleet 3: space/planet/leftbehind
    public GameObject myHangar;
    public GameObject launchBay;
    public GameObject raider;
    public GameObject galactica;
    public string objectToSpawn = "Raider";
    public float spawnClock;
    public int ftlCoords;
    public float ftlClock;
    public int numberOfRaiderWings;
    public GameObject raiderParentObjectPrefab;
    public GameObject raiderParentObject;
    public GameObject attackpatrolPoints;
    public GameObject jumpManager;
    public GameObject masterShipList;
    public bool hasTarget; //galactica or fleet in same space
    public bool jumping;
    public Transform peopleOnBoard;
    public GameObject dradisModel;
    // Use this for initialization
    void Start () {
        if (raiderParentObject == null)
        {
            GameObject parentclone = Instantiate(raiderParentObjectPrefab, transform.position, transform.rotation) as GameObject;
            raiderParentObject = parentclone;
        }
        // galactica = GameObject.Find("Galactica(Clone)");
        if (Vector3.Distance(galactica.transform.position, transform.position) < 13000)
        { attackpatrolPoints.transform.position = galactica.transform.position; }
    }
	
	// Update is called once per frame
	void Update () {

        if (jumping == true)
        {
            ftlClock -= Time.deltaTime;
            if (ftlClock < 0)
            { jumping = false; ftlClock = 3; Jump(); }

        }
        if (photonView.isMine == true)
        {
            if (spawnClock <= 0 && jumping == false )
            {
                if (numberOfRaiderWings >= 5)
                {
                    //GetComponent<PhotonView>().RPC("LaunchRaiders", PhotonTargets.AllViaServer);
                   LaunchRaiders();
                    
                    spawnClock = 4;
                }
            }
            else { spawnClock -= Time.deltaTime; }

        }
    }
    void Awake()
    {
        if (Vector3.Distance(galactica.transform.position, transform.position) < 3000)
        { attackpatrolPoints.transform.position = galactica.transform.position; }
        Debug.Log("basestar In New Scene");
       // DontDestroyOnLoad(this.gameObject);
    }
    //[PunRPC]
    void LaunchRaiders()
    {
        if (raiderParentObject == null)
        {
            GameObject parentclone = Instantiate(raiderParentObjectPrefab, transform.position, transform.rotation) as GameObject;
            raiderParentObject = parentclone;
        }
         GameObject clone = PhotonNetwork.InstantiateSceneObject(objectToSpawn, launchBay.transform.position, launchBay.transform.rotation, 0, null);
        clone.transform.parent = raiderParentObject.transform;
        clone.GetComponent<FighterWing>().roundManager = gameManager.GetComponent<GameManager>().roundManager;
        numberOfRaiderWings -= 5;

        
        
      //  clone.transform.parent = raiderParentObject.transform;
    
        
        

    }
    [PunRPC]
    void StartFTL()
    {
        ftlClock = 3;
        jumping = true;
      //  ftlCoords = coords;
       
    }

    public void Jump()
    {
        //TODO: modify this so that it is recalling it's fighters and not justs deleteing them
        hasTarget = false;
        jumping = false;
        ftlClock = 3;
        //Using the parent object was only deleteing on the server
        if (raiderParentObject != null && photonView.isMine == true)
        {
            foreach (Transform child in raiderParentObject.transform)
            {
                if (child.GetComponent<FighterWing>() != null)
                {
                   // if (child.childCount > 0) { numberOfRaiderWings += child.childCount; }
                    PhotonNetwork.Destroy(child.gameObject);
                    // Destroy(child.gameObject);
                }
            }


            // Destroy(raiderParentObject);
        }
        numberOfRaiderWings += 12;
        GetComponent<FTLDrive>().currentCords = ftlCoords;
        // Application.LoadLevel(ftlCoords.ToString()); //this only works if the server isnt also a player
        //jumpManager.GetComponent<PhotonView>().RPC("UpdateLocationBaseStar", PhotonTargets.AllBufferedViaServer, ftlCoords);

        jumpManager.GetComponent<JumpManager>().BaseStarJump();

        if (Vector3.Distance(galactica.transform.position, transform.position) < 3000)
        { attackpatrolPoints.transform.position = galactica.transform.position; }
        //jumpManager.GetComponent<JumpManager>().ManageJump(0, 0, 0, 0);
        //hasTarget = jumpManager.GetComponent<JumpManager>().CheckCoordinatesBaseStarForFleetGalactica();


        ForPassengersDuringJump(ftlCoords);
        myHangar.GetComponent<LandingBay>().jumping = true;
    }


    
public void ForPassengersDuringJump(int newCords)
{
    foreach (Transform child in peopleOnBoard)
    {
        if (child.GetComponent<PlayerCharacter>().localPlayer != null)
        {
            //TODO: what about people sitting on the flight deck? >> handled on fighter script currently
            if (child.GetComponent<PlayerCharacter>().flying == false)
            {

                child.GetComponent<PlayerCharacter>().localPlayer.GetComponent<PlayerMain>().spaceCoordinates = newCords;
                child.GetComponent<PlayerCharacter>().JumpEffects(newCords, myShipGroup);
                    jumpManager.GetComponent<JumpManager>().ManageJump(0, 0, newCords, newCords); //galactica cords, fleet cords, basestar cords, localPlayer cords

            }
            else { jumpManager.GetComponent<JumpManager>().ManageJump(0, 0, newCords, 0); }

        }



    }

}






void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        }
        else
        {

        }
    }
}
