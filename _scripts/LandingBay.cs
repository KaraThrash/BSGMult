using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingBay : Photon.PunBehaviour
{
    public int myShipInList; //uses masterShipList 1:galactica 2:fleet 3:basestar 4:space
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
        // if (Input.GetKeyDown(KeyCode.P)) { GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer); }
    }
    public void OnCollisionEnter(Collision col2)
    {
        

        if (col2.gameObject.GetComponent<Fighter>() != null && !dockedShips.Contains(col2.gameObject))
        {
            dockedShips.Add(col2.gameObject);


        }


    }
    public void OnTriggerExit(Collider col)
    {
        // if (col.gameObject.GetComponent<ViperControls>()) { col.transform.parent = null; }
        if (dockedShips.Contains(col.gameObject))
        {
            //TODO: fixed the collision for this because right now it's causing problems over the network with residual forces causing the ship to drift slightly away.
           // dockedShips.Remove(col.gameObject);

        }

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
            if (shipToDock.GetComponent<Fighter>().pilot != null)
            { shipToDock.GetComponent<Fighter>().pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer, myShipInList); }

            shipToDock.transform.position = nextAvailableSpot.transform.position;
            shipToDock.transform.rotation = nextAvailableSpot.transform.rotation;

            
            shipToDock.GetComponent<Fighter>().pilot = null;
        }

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


                //if (shipsDocked > hangarSpots)
                // {
                //if (dockedShips[i].GetComponent<Fighter>().currentHangar == this.gameObject && dockedShips[i].GetComponent<Fighter>().flying == false)
                //{
                        
                //    dockedShips[i].GetComponent<Rigidbody>().isKinematic = true;
                //   // dockedShips[i].GetComponent<Fighter>().currentHangar = this.gameObject;
                        
                //        //dockedShips[i].transform.parent = this.transform;
                //    dockedShips[i].transform.position = shipSpots[shipsCurrentlyInHangar].transform.position; 
                //    dockedShips[i].transform.rotation = shipSpots[shipsCurrentlyInHangar].transform.rotation;
                //    dockedShips[i].GetComponent<PhotonView>().RPC("LandOnDockingBay", PhotonTargets.AllViaServer, myShip.GetComponent<FTLDrive>().currentCords, shipsCurrentlyInHangar, myShipInList);
                    
                //}
                shipsCurrentlyInHangar--;
               // }
           // else { break; }
            }
            else { dockedShips.Remove(dockedShips[i]); shipsDocked--; }
        }
        //if (dockingSpaces[dockedShips.Count] != null) { nextAvailableSpot = dockedShips[dockedShips.Count]; }


    }

    public int FindAvailableSpace()
    {
        int openSpace = -1;
        for (int i = dockingSpaces.Count - 1; i >= 0; --i)
        {
            Debug.Log("find space at: " + i.ToString() + dockingSpaces[i].GetComponent<DockingSpace>().spaceOpen);
            if (dockingSpaces[i].GetComponent<DockingSpace>().spaceOpen == true && openSpace == -1)
            {
                dockingSpaces[i].GetComponent<DockingSpace>().spaceOpen = false;
                Debug.Log("openspace found");
                openSpace  = i;

            }
        }
        Debug.Log("no openspace found");
        return openSpace;

    }

    //for ships that exit from the interior hangar
    public void ShipLeavesHangar(GameObject leavingShip)
    {
        if (dockedShips.Contains(leavingShip) )
        {

           dockedShips.Remove(leavingShip);

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
