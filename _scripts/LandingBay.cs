using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingBay : Photon.PunBehaviour
{
    public int myShipInList; //uses masterShipList 1:galactica 2:fleet 3:basestar 4:space
    public GameObject myShip;
    public List<GameObject> dockedShips = new List<GameObject>();
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

            }

    }
    

   [PunRPC]
    public void Jumped()
    {
        shipsDocked = dockedShips.Count;
        for (int i = dockedShips.Count - 1; i >= 0; --i)
        {
            if (dockedShips[i] != null) { 
            //if (shipsDocked > hangarSpots)
           // {
                if (dockedShips[i].GetComponent<Fighter>().currentHangar == this.gameObject && dockedShips[i].GetComponent<Fighter>().flying == false)
                {
                        dockedShips[i].GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient);
                    dockedShips[i].GetComponent<Rigidbody>().isKinematic = true;
                   // dockedShips[i].GetComponent<Fighter>().currentHangar = this.gameObject;
                        dockedShips[i].GetComponent<Fighter>().currentCords = myShip.GetComponent<Galactica>().currentCord;
                        dockedShips[i].GetComponent<Fighter>().jumpManager = myShip.GetComponent<Galactica>().jumpManager;
                        //dockedShips[i].transform.parent = this.transform;
                    dockedShips[i].transform.position = shipSpots[shipsDocked].transform.position; 
                    dockedShips[i].transform.rotation = shipSpots[shipsDocked].transform.rotation;
                    dockedShips[i].GetComponent<PhotonView>().RPC("LandOnDockingBay", PhotonTargets.AllViaServer, myShip.GetComponent<FTLDrive>().currentCords, shipsDocked, myShipInList);
                    
                }
                    shipsDocked--;
               // }
           // else { break; }
            }
            else { dockedShips.Remove(dockedShips[i]); }
        }
        
    }


    //for ships that exit from the interior hangar
    public void ShipLeavesHangar(GameObject leavingShip)
    {
        if (dockedShips.Contains(leavingShip) )
        {

           // dockedShips.Remove(leavingShip);

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
