﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStation : Photon.PunBehaviour
{
    PhotonView m_PhotonView;
    public GameObject onOffObject;
    public bool on;
    public GameObject myCam;
    public GameObject myStation;
    public GameObject cameraLocation;
    public GameObject user;
    //TODO: types of computers
    //>> spawns, builds, actively use >> guns,dradis,communications
    // Use this for initialization
    void Start()
    {
        //myCam = GameObject.Find("GalacticaCamera");
        m_PhotonView = GetComponent<PhotonView>();
        

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) ) {  }
        
        if (on == true) { onOffObject.active = true; } else { onOffObject.active = false; }
    }
    [PunRPC]
    public void ToggleOnOff()
    {
        //TODO: in use or not in use
        if (on == false)
        {
            on = true; onOffObject.active = true;


        }
        else { on = false; onOffObject.active = false; }

    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject == user) {
            on = false;
            onOffObject.active = false;

        }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(food);
            stream.SendNext(on);
        }
        else
        {
            // food = (int)stream.ReceiveNext();
            on = (bool)stream.ReceiveNext();
        }
    }
    public void Interact(GameObject whoUsedMe)
    {
        if (on == false) {
            GetComponent<PhotonView>().RPC("ToggleOnOff", PhotonTargets.AllViaServer);
            user = whoUsedMe;
            whoUsedMe.GetComponent<PlayerCharacter>().myCamera.active = false;
            myStation.SendMessage("Manned");
            whoUsedMe.GetComponent<RPGMovement>().canMove = false;
            whoUsedMe.GetComponent<PlayerCharacter>().station = this.gameObject;
        }
        

    }
    [PunRPC]
    public void MakeAvailable() {
        on = false;
        onOffObject.active = false;
        user = null;
        myStation.SendMessage("NotManned");
    }
}