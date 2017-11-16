using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.PunBehaviour
{
    public List<GameObject> onjectsToEnable = new List<GameObject>();
    public GameObject startGameButton;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
    public void ButtonStartGame()
    {
        GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.AllBufferedViaServer);
    }
}
