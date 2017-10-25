using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : Photon.PunBehaviour
{
    public bool selected;
    public GameObject myPlayer;
    public GameObject myCharacter;
    public GameObject galactica;
    public GameObject jumpManager;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void IChooseYou(GameObject pickedBy, int newView)
    {
        if (selected == false)
        { selected = true;
            GetComponent<PhotonView>().RPC("Selected", PhotonTargets.AllBufferedViaServer);
            myPlayer = pickedBy;
            myPlayer.GetComponent<PlayerMain>().humanoidObject = myCharacter;
            myPlayer.GetComponent<PlayerMain>().galactica = galactica;
            myPlayer.GetComponent<PlayerMain>().jumpManager = jumpManager;
            jumpManager.GetComponent<JumpManager>().PlayerJoin(pickedBy) ;
            myCharacter.GetComponent<HumanControls>().SetAsMyPlayer();
            myCharacter.GetComponent<PlayerCharacter>().localPlayer = pickedBy;
            myCharacter.GetComponent<PhotonView>().ownerId = newView;
            myCharacter.GetComponent<HumanControls>().cam.GetComponent<PhotonView>().ownerId = newView;
        }

    }
    [PunRPC]
    public void Selected() { selected = true; this.gameObject.active = false; }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
             stream.SendNext(selected);
           
        }
        else
        {
            selected = (bool)stream.ReceiveNext();
        
        }
    }
}
