using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingBay : Photon.PunBehaviour
{
    public GameObject myShip;
    public List<GameObject> dockedShips = new List<GameObject>();
    public GameObject[] shipSpots;
    public bool jumping;
    public int hangarSpots = 8;
    public int shipsDocked;
    // Use this for initialization
    void Start () {
       // GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer);
    }
	
	// Update is called once per frame
	void Update () {
       // if (Input.GetKeyDown(KeyCode.P)) { GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer); }
	}
    public void OnCollisionEnter(Collision col2)
    {
       // if (col2.gameObject.GetComponent<ViperControls>()) { col2.transform.parent = myShip.transform; }

        if (col2.gameObject.GetComponent<ViperControls>() != null && !dockedShips.Contains(col2.gameObject))
        { 
                 dockedShips.Add(col2.gameObject);
             

        }


    }
    public void OnCollisionExit(Collision col)
    {
        // if (col.gameObject.GetComponent<ViperControls>()) { col.transform.parent = null; }
        if (dockedShips.Contains(col.gameObject) && jumping == false)
        {

            dockedShips.Remove(col.gameObject);
         
        }
         
    }
   [PunRPC]
    public void Jumped()
    {
        shipsDocked = 0;
        foreach (GameObject child in dockedShips)
        {
           
          
                if (shipsDocked < hangarSpots)
                {
                    //child.transform.GetChild(shipsDocked);
                    child.GetComponent<PhotonView>().ownerId = 0;
                    child.GetComponent<Rigidbody>().isKinematic = true;
                    child.transform.position = shipSpots[shipsDocked].transform.position;
                    child.transform.rotation = shipSpots[shipsDocked].transform.rotation;
                    child.GetComponent<PhotonView>().RPC("LandOnDockingBay", PhotonTargets.AllBufferedViaServer);
                    shipsDocked++;
                }
                else { break; }
          

            



        }
        
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
           // stream.SendNext(flying);
            //stream.SendNext(transform.position);
        }
        else
        {
           // flying = (bool)stream.ReceiveNext();
            // transform.position = (Vector3)stream.ReceiveNext();
        }
    }

}
