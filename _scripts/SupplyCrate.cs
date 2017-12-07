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
    public bool container; //can have greater quantity than 1
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
            if (countedForFleetStats == false)
            {
                //todo change fleet totals
            }
           // whoUsedMe.GetComponent<PhotonView>().RPC("PickedUpObject", PhotonTargets.AllViaServer, itemInList);
           // GetComponent<PhotonView>().RPC("PickedUp", PhotonTargets.AllViaServer);
            
           
        }
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SupplyCrate>() != null && container == true)
        {
            if (collision.gameObject.GetComponent<SupplyCrate>().food == food || collision.gameObject.GetComponent<SupplyCrate>().fuel == fuel)
                //collision.gameObject.GetComponent<PhotonView>().RPC("PickedUp", PhotonTargets.AllViaServer);
                Destroy(collision.gameObject);
            quantity++;
        }
        

            
        
    }
    public void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Storage")
        {
            GetComponent<Rigidbody>().useGravity = false;
            Destroy(this.gameObject);
            //TODO: combine crates?
        }

        if (col.gameObject.GetComponent<PlayerCharacter>() != null)
        {
            if (col.gameObject.GetComponent<PlayerCharacter>().carriedObject == -1)
            {
                col.gameObject.GetComponent<PlayerCharacter>().backpack.active = true;
                col.gameObject.GetComponent<PlayerCharacter>().carriedObject = itemInList;
                Destroy(this.gameObject);

            }
        }



    }

    [PunRPC]
    public void PickedUp()
    {
        //Destroy(this.gameObject);
        //if (photonView.isMine == true)
        //{  }
        //else { }

        this.gameObject.active = false;
    }
 

}
