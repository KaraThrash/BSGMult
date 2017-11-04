using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCrate : Photon.PunBehaviour
{
    public bool food; //or fuel
    public bool fuel;
    public int quantity;
    public bool countedForFleetStats; //whether or not this quantity is counted in the fleet totals.
    public bool ammo;

    public int itemInList; //like the mastership list. This simplifies passing gameobjects around the server
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Interact(GameObject whoUsedMe)
    {
        
        if (whoUsedMe.GetComponent<PlayerCharacter>().carriedObject == -1)
        {
           // GetComponent<Rigidbody>().useGravity = true;
            
            whoUsedMe.GetComponent<PhotonView>().RPC("PickedUpObject", PhotonTargets.AllViaServer, itemInList);
            GetComponent<PhotonView>().RPC("PickedUp", PhotonTargets.AllViaServer);
            if (countedForFleetStats == false) {
                //todo change fleet totals
            }
           
        }
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Storage")
        {
            GetComponent<Rigidbody>().useGravity = false;
            //TODO: combine crates?
        }
    }


    [PunRPC]
    public void PickedUp()
    {
        Destroy(this.gameObject);
       // this.gameObject.active = false;
    }
 

}
