using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerMain : Photon.PunBehaviour
{
    public GameObject galactica;
    public GameObject roundManager;
    public int spaceCoordinates;
    public float score;
    //Parent script for each player on the server
    public GameObject myCamera;
    public GameObject currentController;
    public GameObject humanoidObject;
    public bool atBattleStation;
    public int currentCord;
    public GameObject jumpManager;
    public GameObject gameManager;
    public int shipGroup;  //0:space/planet //1: galactica + active fleet 2: Cylon fleet 3:  leftbehind
    public GameObject playerHud;
    public GameObject hpTextObj;
    public GameObject ammoTextObj;
    public Text scoreText;

    // Use this for initialization
    void Start () {
        // scoreText = GameObject.Find("Menu").GetComponent<ItemList>().supplyCrates[GetComponent<PhotonView>().ownerId].GetComponent<Text>();
        if (photonView.isMine == true)
        {
            gameManager = GameObject.Find("GameManager");
            //playerHud = GameObject.Find("PlayerHud");
            // hpTextObj = playerHud.transform.Find("HpText").gameObject;
            // ammoTextObj = playerHud.transform.Find("AmmoText").gameObject;
            myCamera.active = true;
            myCamera.transform.parent = null;

        }
        //else { scoreText = GameObject.Find("Menu").GetComponent<ItemList>().supplyCrates[GetComponent<PhotonView>().ownerId].GetComponent<Text>(); }


	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine == true)
        {



            if (SceneManager.GetActiveScene().name == "PlayerLobby") { PickCharacter(); }
        }
    }
    void Awake()
    {
        if (GameObject.Find("Menu") != null) { scoreText = GameObject.Find("Menu").GetComponent<ItemList>().supplyCrates[GetComponent<PhotonView>().ownerId].GetComponent<Text>(); }
        
        // SceneManager.sceneLoaded()
        Debug.Log("PlayerMain In New Scene");
        DontDestroyOnLoad(this.gameObject);
      //  if (galactica != null) { if (currentCord == galactica.GetComponent<Galactica>().currentCord) { galactica.active = true; } }
       
    }
    [PunRPC]
    public void SetShipGroup(int newGroup)
    {
        shipGroup = newGroup;

    }
    [PunRPC]
    public void SetHumanActive()
    {
        if (humanoidObject != null) {

            humanoidObject.active = true;
        }
       
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
    [PunRPC]
    public void UpdateScore(float newScore)
    {
        if (scoreText == null)
        {
            if (GameObject.Find("Menu") != null) { scoreText = GameObject.Find("Menu").GetComponent<ItemList>().supplyCrates[GetComponent<PhotonView>().ownerId].GetComponent<Text>(); }
        }
        score = newScore;
        scoreText.text = score.ToString();
    }
    public void CharacterDied()
    {
        roundManager.GetComponent<RoundManager>().wasFrakked = true;

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
             stream.SendNext(score);

        }
        else
        {
            score = (float)stream.ReceiveNext();

        }
    }
}
