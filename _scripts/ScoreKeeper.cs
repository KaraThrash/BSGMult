using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreKeeper : Photon.PunBehaviour
{
    public int playerScoreOne;
    public int playerScoreTwo;
    public int playerScoreThree;
    public int playerScoreFour;
    public Text score1;
    public Text score2;
    public Text score3;
    public Text score4;
    public GameObject scoreObj1;
    public GameObject scoreObj2;
    public GameObject scoreObj3;
    public GameObject scoreObj4;
    // Use this for initialization
    void Start () {
        //score1 = GameObject.Find("score1").GetComponent<Text>();
        //score2 = GameObject.Find("score2").GetComponent<Text>();
        //score3 = GameObject.Find("score3").GetComponent<Text>();
        //score4 = GameObject.Find("score4").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    [PunRPC]
    public void Score(int whoScored)
    {
        
            switch (whoScored)
            {
                case 1:
                    playerScoreOne++;
                scoreObj1.GetComponent<NetStatTracker>().stat++;
                break;
                case 2:
                    playerScoreTwo++;
                scoreObj2.GetComponent<NetStatTracker>().stat++;
                break;
                case 3:
                    playerScoreThree++;
                scoreObj3.GetComponent<NetStatTracker>().stat++;
                break;
                case 4:
                    playerScoreFour++;
                scoreObj4.GetComponent<NetStatTracker>().stat++;
                break;
                default:
                    break;
            }
        //playerScoreOne = playerScoreOne;
        //playerScoreFour = playerScoreFour;
        //playerScoreThree = playerScoreThree;
        //playerScoreTwo = playerScoreTwo;
        score4.text = scoreObj1.GetComponent<NetStatTracker>().stat.ToString();
        score3.text = scoreObj3.GetComponent<NetStatTracker>().stat.ToString();
        score2.text = scoreObj2.GetComponent<NetStatTracker>().stat.ToString();
        score1.text = scoreObj4.GetComponent<NetStatTracker>().stat.ToString();
        //GetComponent<PhotonView>().RPC("Score", PhotonTargets.AllViaServer, playerScoreOne, playerScoreTwo, playerScoreThree, playerScoreFour);

    }
    //[PunRPC]
    public void ScoreAHit(int bulletOwner)
    { //   GetComponent<PhotonView>().RPC("Score", PhotonTargets.AllBuffered, bulletOwner);
      //TODO: figure out why the score earned from logged out players doesnt show when a new player logs in
      //>>> maybe fixed it with streaming?

        //score4.text = playerScoreFour.ToString();
        //score3.text = playerScoreThree.ToString();
        //score2.text = playerScoreTwo.ToString();
        //score1.text = playerScoreOne.ToString();
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
           
        }
        else
        {

        }
    }

}
