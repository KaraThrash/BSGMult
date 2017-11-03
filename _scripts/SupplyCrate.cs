using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCrate : MonoBehaviour {
    public bool food; //or fuel
    public int quantity;
    public bool countedForFleetStats; //whether or not this quantity is counted in the fleet totals.
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Interact(GameObject whoUsedMe)
    {
        
        if (whoUsedMe.GetComponent<PlayerCharacter>().carriedObject == null)
        {
            GetComponent<Rigidbody>().useGravity = true;
            whoUsedMe.GetComponent<PlayerCharacter>().carriedObject = this.gameObject;
            whoUsedMe.GetComponent<PhotonView>().RPC("PickedUpObject", PhotonTargets.AllBufferedViaServer);
            if (countedForFleetStats == false) {
                //todo change fleet totals
            }
            this.gameObject.active = false;
            this.transform.parent = whoUsedMe.transform;
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
}
