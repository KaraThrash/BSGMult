using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCrate : MonoBehaviour {
    public bool food; //or fuel
    public int quantity;
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
            whoUsedMe.GetComponent<PlayerCharacter>().carriedObject = this.gameObject;
            whoUsedMe.GetComponent<PhotonView>().RPC("PickedUpObject", PhotonTargets.AllBufferedViaServer);
            this.gameObject.active = false;
            this.transform.parent = whoUsedMe.transform;
        }
        
    }
}
