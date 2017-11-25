using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.PunBehaviour
{
    //Has Access to all other objects to make it easier to manage

    public List<GameObject> onjectsToEnable = new List<GameObject>();
    public GameObject startGameButton;

    public GameObject roundManager;
    public GameObject crisisManager;
    public GameObject resourceManager; //fleet
    public GameObject shipList;
    
    public GameObject itemList;
    public GameObject characterList;
    public GameObject cylonManager;
    public bool firstScene;
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
        startGameButton.active = false;
        for (int i = onjectsToEnable.Count - 1; i >= 0; --i)
        {
            onjectsToEnable[i].active = true;
        }
    }
    [PunRPC]
    public void MasterClientStart()
    {
        PhotonNetwork.Instantiate("Everything", Vector3.zero, new Quaternion(0, 0, 0, 0), 0, null);
        GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.AllBufferedViaServer);
    }

    public void NewRound()
    {
        roundManager.GetComponent<RoundManager>().NewRound();

        crisisManager.GetComponent<CrisisManager>().AttackingBaseStar();
    }

    public void ButtonStartGame()
    {

        GetComponent<PhotonView>().RPC("MasterClientStart", PhotonTargets.MasterClient);
        // Application.LoadLevel("PlayerLobby");

        // GetComponent<PhotonView>().RequestOwnership();
        // PhotonNetwork.Instantiate("Everything", Vector3.zero, new Quaternion(0, 0, 0, 0), 0, null);

        //GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("MasterClientStart", PhotonTargets.MasterClient);
       
       // GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.AllBufferedViaServer);
    }

  

}
