using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour {
    public bool temp;
    public bool turnOffForTesting;
	// Use this for initialization
	void Start () {
       //if (turnOffForTesting == true) { this.gameObject.active = false; }
	}
	
	// Update is called once per frame
	void Update () {
        // Application.LoadLevel("Game");
        // if (Input.GetKeyDown(KeyCode.E)) { Application.LoadLevel("Game"); }


        if (Input.GetKeyDown(KeyCode.J))
        {
           // SceneManager.LoadScene("Game 3");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
           // SceneManager.LoadScene("Game 4");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
          //  SceneManager.LoadScene("Game 1");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
           // SceneManager.LoadScene("ServerStart");
        }
    }
    void Awake()
    {
        // SceneManager.sceneLoaded()
       
        if (temp == false)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void JoinGame() { Application.LoadLevel("PlayerLobby"); }
    public void ReJoinLobby() { Application.LoadLevel("Lobby"); }
}
