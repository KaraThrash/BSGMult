﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMain : Photon.PunBehaviour
{
    public GameObject galactica;
    public int spaceCoordinates;
    public int score;
    //Parent script for each player on the server
    public GameObject myCamera;
    public GameObject currentController;
    public GameObject humanoidObject;
    public bool atBattleStation;
    public int currentCord;
    public GameObject jumpManager;
    public int shipGroup; //1: galactica + active fleet 2: Cylon fleet 3: space/planet/leftbehind
    // Use this for initialization
    void Start () {
        if (photonView.isMine == true)
        {
            myCamera.active = true;
            myCamera.transform.parent = null;
            
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine == true)
        {
           
        }

        if (SceneManager.GetActiveScene().name == "PlayerLobby") { PickCharacter(); }
    }
    void Awake()
    {
        // SceneManager.sceneLoaded()
        Debug.Log("PlayerMain In New Scene");
        DontDestroyOnLoad(this.gameObject);
      //  if (galactica != null) { if (currentCord == galactica.GetComponent<Galactica>().currentCord) { galactica.active = true; } }
       
    }


    public void PickCharacter() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = myCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    hit.transform.gameObject.GetComponent<SelectCharacter>().IChooseYou(this.gameObject, this.GetComponent<PhotonView>().ownerId);
                    
                }


            }
        }
    }
    public void Jumping(int newCords, int newShipGroup)
    {
        Debug.Log("Joined game and changing to new scene: " + newCords);
        // SceneManager.LoadScene(newCords.ToString());
        spaceCoordinates = newCords;

        //Application.LoadLevel(newCords.ToString());
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // stream.SendNext(transform.rotation);

        }
        else
        {
            // transform.rotation = (Quaternion)stream.ReceiveNext();

        }
    }
}
