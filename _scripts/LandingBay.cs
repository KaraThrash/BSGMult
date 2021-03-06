﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingBay : Photon.PunBehaviour
{
    public int myShipInList; //uses masterShipList 1:galactica 2:fleet 3:basestar 4:space
    public int shipGroup;

    public GameObject myShip;
    public GameObject spaceExit;
    public List<GameObject> dockedShips = new List<GameObject>();
    public List<GameObject> dockingSpaces = new List<GameObject>();

    public GameObject[] shipSpots;
    public bool jumping;
    public int hangarSpots = 8;
    public int shipsDocked;
    public GameObject nextAvailableSpot;
    public GameObject shipToDock;
    public GameObject forPeopleObject;
    
    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
    public void OnCollisionEnter(Collision col2)
    {
        



    }
    public void OnTriggerExit(Collider col)
    {
       

    }

    public void AddThisShip(GameObject shipToDock)
    {

            if (!dockedShips.Contains(shipToDock))
            {
                dockedShips.Add(shipToDock);
                shipsDocked++;
                
            }
        int openSpace = FindAvailableSpace();
        if (openSpace >= 0) {

            nextAvailableSpot = dockingSpaces[openSpace];
            shipToDock.GetComponent<Fighter>().inHangar = true;
            shipToDock.GetComponent<Fighter>().hangarSpace = nextAvailableSpot;

            shipToDock.transform.parent = nextAvailableSpot.transform;
            shipToDock.transform.position = nextAvailableSpot.transform.position;
            shipToDock.transform.rotation = nextAvailableSpot.transform.rotation;
            if (shipToDock.GetComponent<Fighter>().pilot != null)
            {
                shipToDock.GetComponent<Fighter>().pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer, myShipInList, shipToDock.GetComponent<Fighter>().cockpitEntrance.transform.position,shipGroup);
            }

            shipToDock.GetComponent<Fighter>().pilot = null;
        }
        if (openSpace - 1 <= 0) { GetComponent<Collider>().enabled = false; }

    }
    

   [PunRPC]
    public void Jumped()
    {
        int shipsCurrentlyInHangar = dockedShips.Count;
        
        for (int i = shipsCurrentlyInHangar - 1; i >= 0; --i)
        {
            if (dockedShips[i] != null)
            {
                dockedShips[i].GetComponent<Fighter>().currentCords = myShip.GetComponent<FTLDrive>().currentCords;
                dockedShips[i].GetComponent<Fighter>().jumpManager = myShip.GetComponent<FTLDrive>().jumpManager;
                dockedShips[i].GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient);


                shipsCurrentlyInHangar--;

            }
            else { dockedShips.Remove(dockedShips[i]); shipsDocked--; }
        }



    }

    public int FindAvailableSpace()
    {
        int openSpace = -1;
        for (int i = dockingSpaces.Count - 1; i >= 0; --i)
        {
            if (dockingSpaces[i].GetComponent<DockingSpace>().spaceOpen == true && openSpace == -1)
            {
                dockingSpaces[i].GetComponent<DockingSpace>().spaceOpen = false;
                openSpace  = i;

            }
        }
        return openSpace;

    }

    //for ships that exit from the interior hangar
    public void ShipLeavesHangar(GameObject leavingShip)
    {
        if (dockedShips.Contains(leavingShip) )
        {

           dockedShips.Remove(leavingShip);
            GetComponent<Collider>().enabled = true;
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
