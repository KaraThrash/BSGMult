﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.PunBehaviour
{
    //Has Access to all other objects to make it easier to manage

    public List<GameObject> objectsToEnable = new List<GameObject>();
    public GameObject startGameButton;

    public GameObject roundManager;
    public GameObject crisisManager;
    public GameObject spaceManager;
    public GameObject resourceManager; //fleet
    public GameObject shipList;
    
    public GameObject itemList;
    public GameObject characterList;
    public GameObject cylonManager;
    public bool firstScene;

    public int currentScene; //space coordinates
    public int humanLead;
    public GameObject localPlayer;
    public GameObject everything;
    public bool gameOn;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Space) && firstScene == true) { ButtonStartGame(); }

	}
    [PunRPC]
    public void StartGame()
    {
        gameOn = true;
        startGameButton.active = false;
        for (int i = objectsToEnable.Count - 1; i >= 0; --i)
        {
            objectsToEnable[i].active = true;
        }
        spaceManager.GetComponent<SpaceManager>().SpawnSpace(0);
    }
    [PunRPC]
    public void MasterClientStart()
    {
       // PhotonNetwork.Instantiate("Everything", Vector3.zero, new Quaternion(0, 0, 0, 0), 0, null);
        GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.AllBufferedViaServer);
    }

    public void NewRound()
    {
        roundManager.GetComponent<RoundManager>().NewRound();
        if (roundManager.GetComponent<RoundManager>().currentRound % 2 == 0)
        { crisisManager.GetComponent<CrisisManager>().AttackingBaseStar(); }
        else { crisisManager.GetComponent<CrisisManager>().HiddenBomb(); }
        spaceManager.GetComponent<SpaceManager>().SpawnSpace(roundManager.GetComponent<RoundManager>().currentRound);
    }

    public void ButtonStartGame()
    {

        GetComponent<PhotonView>().RPC("MasterClientStart", PhotonTargets.MasterClient);
        
    }
    [PunRPC]
    public void PlayerDie()
    {
        resourceManager.GetComponent<Fleet>().UpdateResources(0,0,0,-1,0);

    }
    [PunRPC]
    public void RestartGame()
    {
        everything.GetComponent<LevelManager>().temp = true;
        Application.LoadLevel("PlayerLobby");
        
    }
    public void ButtonRestartGame()
    {
      //  everything.GetComponent<LevelManager>().temp = true;
        GetComponent<PhotonView>().RPC("RestartGame", PhotonTargets.AllViaServer);
        // Application.LoadLevel("PlayerLobby");

        // GetComponent<PhotonView>().RequestOwnership();
        // PhotonNetwork.Instantiate("Everything", Vector3.zero, new Quaternion(0, 0, 0, 0), 0, null);

        //GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("MasterClientStart", PhotonTargets.MasterClient);

        // GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.AllBufferedViaServer);
    }

}
