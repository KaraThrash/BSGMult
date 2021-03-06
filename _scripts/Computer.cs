﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer : Photon.PunBehaviour
{
    PhotonView m_PhotonView;
    public GameObject onOffObject;
    public bool on;
    public GameObject statTracker;
    public int myStat;
    public Text foodtext;
    public float interactTimer;
    public float timeCost; //How long it needs to be interacted with
    public bool spawnObject;
    public string objectToSpawn;
    public GameObject objectToSpawnPrefab;
    public GameObject whereToSpawn;
    public GameObject loadingObject;
    //TODO: types of computers
    //>> spawns, builds, actively use >> guns,dradis,communications
    // Use this for initialization
    void Start()
    {
        m_PhotonView = GetComponent<PhotonView>();
 

    }
        // Update is called once per frame
        void Update () {
       
        if (interactTimer >= 0) { interactTimer -= (Time.deltaTime * 0.5f);   }
        if (on == true) { onOffObject.active = true; } else { onOffObject.active = false; }
	}
    [PunRPC]
    public void ToggleOnOff() {
      
        //TODO: in use or not in use
        
            if (on == true)
            {
            on = false; onOffObject.active = false;
            } else
            { 
            //statTracker.GetComponent<NetStatTracker>().stat++;
           
                on = true; onOffObject.active = true;
                //foodtext.text = food.ToString();
            }
            
        

    }

    public void OnTriggerStay(Collider col)
    {
        //if (Input.GetKeyDown(KeyCode.F) && col.gameObject.tag == "Player") {
           
        //}
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
          
            stream.SendNext(on);
        }
        else
        {
          
            on = (bool)stream.ReceiveNext();
          
        }
    }
    public void Interact(GameObject whoUsedMe)
    {
        loadingObject.active = true;
        interactTimer += Time.deltaTime;
        if (interactTimer >= timeCost) { interactTimer = 0; PhotonNetwork.Instantiate(objectToSpawn, whereToSpawn.transform.position, whereToSpawn.transform.rotation, 0, null); GetComponent<PhotonView>().RPC("ToggleOnOff", PhotonTargets.AllBufferedViaServer); }
        
    }

    public void Repair(GameObject whoUsedMe)
    {

    }

    public void Sabotage(GameObject whoUsedMe)
    {
        interactTimer--;
    }

}
