using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectCharacter : Photon.PunBehaviour
{
    public bool selected;
    public GameObject myPlayer;
    public GameObject myCharacter;
    public GameObject galactica;
    public GameObject jumpManager;
    public GameObject scoreTextObj;
    public Text nameText;
    public Text scoreText;
    public string name;
    public GameObject scoreBoardItemList;
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
            
            myPlayer = pickedBy;
            myPlayer.GetComponent<PlayerMain>().humanoidObject = myCharacter;
            myPlayer.GetComponent<PlayerMain>().galactica = galactica;
            myPlayer.GetComponent<PlayerMain>().jumpManager = jumpManager;
            myPlayer.GetComponent<PlayerMain>().scoreText = scoreText;
            myPlayer.GetComponent<PlayerMain>().scoreText = scoreText;

            jumpManager.GetComponent<JumpManager>().PlayerJoin(pickedBy) ;
            myCharacter.GetComponent<HumanControls>().SetAsMyPlayer();
            myCharacter.GetComponent<PlayerCharacter>().SetAsMyPlayer(pickedBy);

            myCharacter.GetComponent<PhotonView>().ownerId = newView;
            myCharacter.GetComponent<HumanControls>().cam.GetComponent<PhotonView>().ownerId = newView;
            GetComponent<PhotonView>().RPC("Selected", PhotonTargets.AllBufferedViaServer, name, newView);
        }

    }
    [PunRPC]
    public void Selected(string characterName, int scoreBoard)
    {

        //scoreText = scoreBoardItemList.GetComponent<ItemList>().supplyCrates[scoreBoard].GetComponent<Text>();
        scoreTextObj.active = true;
        nameText.text = name;
        selected = true;
        this.gameObject.active = false;
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.isWriting)
        //{
        //     stream.SendNext(selected);
           
        //}
        //else
        //{
        //    selected = (bool)stream.ReceiveNext();
        
        //}
    }
}
