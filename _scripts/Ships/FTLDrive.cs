using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTLDrive : Photon.PunBehaviour
{
    public int myShipGroup; //1: galactica + active fleet 2: Cylon fleet 3: space/planet/leftbehind
    public GameObject myParent;
    public int currentCords;
    public int targetCords;
    public GameObject jumpEffect;
    public float timeToJump;
    public bool jumping;
    public bool prepareJump;
    public float ftlCost;
    public float ftlSpoolingUp;

    public GameObject jumpManager;
    public Transform peopleOnBoard;
    public GameObject myHangar;
    public string UpdateMyShipString;
    public GameObject baseStar;
    public bool updateJumpManager;
    public bool fleetShip;

    public bool isGalactica;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (prepareJump == true)
        {
            ftlSpoolingUp += Time.deltaTime;

            if (ftlSpoolingUp >= ftlCost)
            {
                
                prepareJump = false; timeToJump = 1; jumping = true;
                Instantiate(jumpEffect, transform.position, transform.rotation);


            }

        }
        if (jumping == true)
        {
            timeToJump -= Time.deltaTime;

            if (timeToJump <= 0)
            {
                JumpAway();
                //GetComponent<PhotonView>().RPC("JumpAway", PhotonTargets.AllBufferedViaServer);
            }

        }
    }
	[PunRPC]
    public void StartJump(int newCords) //small cooldown that plays the FTL animation when a jump is ordered
    {

        ForPassengersDuringJump(targetCords, true);
        ftlSpoolingUp = 0;
        targetCords = newCords;
        timeToJump = 1;
        prepareJump = true;
        
    }
    
    public void JumpAway()
    {
        currentCords = targetCords;
        ForPassengersDuringJump(currentCords, false);
        ftlSpoolingUp = 0;
        timeToJump = 1;
        prepareJump = false;
       
        jumping = false;
        if (myHangar != null) { myHangar.GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer); }
        

        if (updateJumpManager == true) { jumpManager.GetComponent<PhotonView>().RPC(UpdateMyShipString, PhotonTargets.AllBufferedViaServer, currentCords); }
       

        if (isGalactica == true) { jumpManager.GetComponent<JumpManager>().GalacticaJumped(currentCords); }
    }

    public void ForPassengersDuringJump(int newCords, bool startCamera)
    {
        if (peopleOnBoard != null)
        {
            foreach (Transform child in peopleOnBoard)
            {
                if (child.GetComponent<PlayerCharacter>().localPlayer != null && child.gameObject.active == true)
                {
                    //TODO: what about people sitting on the flight deck? >> handled on fighter script currently
                   // if (child.GetComponent<PlayerCharacter>().flying == false)
                   // {
                        if (startCamera == true) { child.GetComponent<PlayerCharacter>().JumpCameraEffects(); }
                        else
                        {
                            child.GetComponent<PlayerCharacter>().localPlayer.GetComponent<PlayerMain>().spaceCoordinates = newCords;
                            child.GetComponent<PlayerCharacter>().JumpEffects(newCords,myShipGroup);
                            //jumpManager.GetComponent<JumpManager>().ManageJump(newCords, 0, 0, newCords); //galactica cords, fleet cords, basestar cords, localPlayer cords
                        }
                   // }
                  //  else { jumpManager.GetComponent<JumpManager>().ManageJump(newCords, 0, 0, 0); }

                }



            }
        }
    }



}
